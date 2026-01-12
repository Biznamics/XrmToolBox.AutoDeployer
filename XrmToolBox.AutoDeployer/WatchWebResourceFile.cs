using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using XrmToolBox.Extensibility;

namespace XrmToolBox.AutoDeployer
{
    internal sealed class WatchWebResourceFile : IDisposable
    {
        #region Private Fields

        private readonly WebResourceWatchConfig config;
        private readonly PluginControlBase host;
        private readonly WebResourceMapping mapping;
        private readonly Control owner;
        private readonly object publishLock = new object();
        private readonly IOrganizationService service;
        private int isUpdating; // 0 = no, 1 = yes

        private System.Threading.Timer publishTimer;

        #endregion Private Fields

        #region Public Constructors

        public WatchWebResourceFile(WebResourceWatchConfig cfg, WebResourceMapping map, IOrganizationService svc, Control uiOwner)
        {
            config = cfg ?? throw new ArgumentNullException(nameof(cfg));
            mapping = map ?? throw new ArgumentNullException(nameof(map));
            service = svc ?? throw new ArgumentNullException(nameof(svc));
            owner = uiOwner ?? throw new ArgumentNullException(nameof(uiOwner));

            host = owner as PluginControlBase;

            FullPath = Path.GetFullPath(Path.Combine(config.RootPath ?? "", mapping.RelativePath ?? ""));
            FileName = Path.GetFileName(FullPath);
            FolderPath = Path.GetDirectoryName(FullPath);

            Log = $"Started at {DateTime.Now}\r\n";
            Log += $"CRM: {mapping.CrmName}\r\n";
            Log += $"File: {FullPath}\r\n";

            ListItem = new ListViewItem { Tag = this };

            WebResourceId = GetWebResourceIdByName(mapping.CrmName);

            if (WebResourceId == Guid.Empty)
            {
                Status = "Not found in Dataverse";
                Log += $"{DateTime.Now:HH:mm:ss.fff} ERROR: WebResource not found by name '{mapping.CrmName}'\r\n";
                UpdateList();
                return;
            }

            if (!File.Exists(FullPath))
            {
                Status = "File not found";
                Log += $"{DateTime.Now:HH:mm:ss.fff} ERROR: File not found '{FullPath}'\r\n";
                UpdateList();
                return;
            }

            Watcher = new FileSystemWatcher
            {
                Path = FolderPath,
                Filter = FileName,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName,
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            Watcher.Changed += OnFileChanged;
            Watcher.Renamed += OnFileRenamed;

            UpdateList();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler Changed;

        #endregion Public Events

        #region Public Properties

        public string CrmName => mapping?.CrmName;
        public string FileName { get; }
        public DateTime FileUpdated { get; private set; }
        public string FolderPath { get; }
        public string FullPath { get; }
        public ListViewItem ListItem { get; }
        public string Log { get; private set; } = "";
        public DateTime PublishedUpdated { get; private set; }
        public string RelativePath => mapping?.RelativePath;
        public string Status { get; private set; } = "Watching";
        public FileSystemWatcher Watcher { get; private set; }
        public Guid WebResourceId { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            try
            {
                if (Watcher != null)
                {
                    Watcher.EnableRaisingEvents = false;
                    Watcher.Changed -= OnFileChanged;
                    Watcher.Renamed -= OnFileRenamed;
                    Watcher.Dispose();
                    Watcher = null;
                }
            }
            catch { }

            lock (publishLock)
            {
                publishTimer?.Dispose();
                publishTimer = null;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static byte[] ReadFile(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        private static void WaitForFileReady(string fullPath)
        {
            // called from WorkAsync background thread
            const int maxAttempts = 40; // ~10s
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    using (File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        return;
                }
                catch (FileNotFoundException) { }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }

                Thread.Sleep(250);
            }

            // final attempt to throw meaningful exception
            using (File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) { }
        }

        private Guid GetWebResourceIdByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Guid.Empty;

            var query = new QueryExpression("webresource")
            {
                ColumnSet = new ColumnSet(false)
            };
            query.Criteria.AddCondition("name", ConditionOperator.Equal, name);

            return service.RetrieveMultiple(query).Entities.FirstOrDefault()?.Id ?? Guid.Empty;
        }

        private void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            // prevent overlapping uploads
            if (Interlocked.Exchange(ref isUpdating, 1) == 1)
                return;

            // Always hop to UI thread before calling WorkAsync
            owner.BeginInvoke((Action)(() =>
            {
                if (host == null)
                {
                    Interlocked.Exchange(ref isUpdating, 0);
                    return;
                }

                host.WorkAsync(new WorkAsyncInfo
                {
                    Message = $"Auto Deployer: Updating web resource '{mapping.CrmName}'...",
                    Work = (w, we) =>
                    {
                        // background thread
                        WaitForFileReady(FullPath);

                        var content = Convert.ToBase64String(ReadFile(FullPath));
                        var wr = new Entity("webresource", WebResourceId);
                        wr["content"] = content;
                        service.Update(wr);
                    },
                    PostWorkCallBack = (we) =>
                    {
                        try
                        {
                            if (we.Error != null)
                            {
                                Status = $"Update error: {we.Error.Message}";
                                Log += $"{DateTime.Now:HH:mm:ss.fff} ERROR: {we.Error}\r\n";
                                host.SetWorkingMessage(Status);
                            }
                            else
                            {
                                FileUpdated = File.GetLastWriteTime(FullPath);
                                Status = "Updated";
                                Log += $"{DateTime.Now:HH:mm:ss.fff} Updated in Dataverse\r\n";
                                host.SetWorkingMessage($"Web resource updated: {mapping.CrmName}");

                                if (config.PublishEnabled)
                                    QueuePublish();
                            }

                            UpdateList();
                        }
                        finally
                        {
                            Interlocked.Exchange(ref isUpdating, 0);
                        }
                    }
                });
            }));
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            Status = "Renamed (not watching)";
            Log += $"{DateTime.Now:HH:mm:ss.fff} File renamed: {e.OldName} -> {e.Name}\r\n";
            UpdateList();
        }

        private void PublishWebResource(Guid webResourceId)
        {
            var id = webResourceId.ToString("D").ToUpperInvariant();

            var request = new OrganizationRequest("PublishXml");
            request["ParameterXml"] =
                $"<importexportxml><webresources><webresource>{id}</webresource></webresources></importexportxml>";

            service.Execute(request);
        }

        private void QueuePublish()
        {
            lock (publishLock)
            {
                publishTimer?.Dispose();
                publishTimer = new System.Threading.Timer(_ =>
                {
                    try
                    {
                        if (owner.IsDisposed || !owner.IsHandleCreated)
                            return;

                        owner.BeginInvoke((Action)(() =>
                        {
                            if (host == null)
                                return;

                            host.WorkAsync(new WorkAsyncInfo
                            {
                                Message = $"Auto Deployer: Publishing web resource '{mapping.CrmName}'...",
                                Work = (w, we) =>
                                {
                                    PublishWebResource(WebResourceId);
                                },
                                PostWorkCallBack = (we) =>
                                {
                                    if (we.Error != null)
                                    {
                                        Status = $"Publish error: {we.Error.Message}";
                                        Log += $"{DateTime.Now:HH:mm:ss.fff} PUBLISH ERROR: {we.Error}\r\n";
                                        host.SetWorkingMessage(Status);
                                    }
                                    else
                                    {
                                        PublishedUpdated = DateTime.Now;
                                        Status = "Published";
                                        Log += $"{DateTime.Now:HH:mm:ss.fff} Published\r\n";
                                        host.SetWorkingMessage($"Web resource published: {mapping.CrmName}");
                                    }

                                    UpdateList();
                                }
                            });
                        }));
                    }
                    catch (Exception ex)
                    {
                        Status = $"Publish error: {ex.Message}";
                        Log += $"{DateTime.Now:HH:mm:ss.fff} PUBLISH ERROR: {ex}\r\n";
                        UpdateList();
                    }
                }, null, config.DebounceMs > 0 ? config.DebounceMs : 1500, Timeout.Infinite);
            }
        }

        private void UpdateList()
        {
            MethodInvoker mi = delegate
            {
                while (ListItem.SubItems.Count < 5)
                    ListItem.SubItems.Add(string.Empty);

                // Column 0: show CRM name (more useful than file name)
                ListItem.Text = mapping?.CrmName ?? FileName;
                ListItem.SubItems[1].Text = FolderPath;
                ListItem.SubItems[2].Text = FileUpdated.Ticks != 0 ? FileUpdated.ToString("HH:mm:ss.fff") : "";
                ListItem.SubItems[3].Text = PublishedUpdated.Ticks != 0 ? PublishedUpdated.ToString("HH:mm:ss.fff") : "";
                ListItem.SubItems[4].Text = $"WR: {Status}";

                OnChanged();
            };

            if (owner.InvokeRequired) owner.Invoke(mi);
            else mi();
        }

        #endregion Private Methods
    }
}