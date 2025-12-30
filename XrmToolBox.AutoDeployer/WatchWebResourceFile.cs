using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace XrmToolBox.AutoDeployer
{
    internal class WatchWebResourceFile : IDisposable
    {
        private readonly Control owner;
        private readonly IOrganizationService service;
        private readonly WebResourceWatchConfig config;
        private readonly WebResourceMapping mapping;

        private System.Threading.Timer publishTimer;
        private readonly object publishLock = new object();

        public ListViewItem ListItem { get; }
        public string Log { get; private set; } = "";
        public string Status { get; private set; } = "Watching";

        public DateTime FileUpdated { get; private set; }
        public DateTime PublishedUpdated { get; private set; }

        public string FullPath { get; }
        public string FileName { get; }
        public string FolderPath { get; }

        public Guid WebResourceId { get; private set; }

        public FileSystemWatcher Watcher { get; }

        public event EventHandler Changed;
        protected virtual void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public WatchWebResourceFile(WebResourceWatchConfig cfg, WebResourceMapping map, IOrganizationService svc, Control uiOwner)
        {
            config = cfg ?? throw new ArgumentNullException(nameof(cfg));
            mapping = map ?? throw new ArgumentNullException(nameof(map));
            service = svc ?? throw new ArgumentNullException(nameof(svc));
            owner = uiOwner ?? throw new ArgumentNullException(nameof(uiOwner));

            // Build absolute file path
            FullPath = Path.Combine(config.RootPath ?? "", mapping.RelativePath ?? "");
            FullPath = Path.GetFullPath(FullPath);

            FileName = Path.GetFileName(FullPath);
            FolderPath = Path.GetDirectoryName(FullPath);

            Log = $"Started at {DateTime.Now}\r\n";
            Log += $"CRM: {mapping.CrmName}\r\n";
            Log += $"File: {FullPath}\r\n";

            WebResourceId = GetWebResourceIdByName(mapping.CrmName);

            ListItem = new ListViewItem();
            ListItem.Tag = this;

            if (WebResourceId == Guid.Empty)
            {
                Status = "Not found in CRM";
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
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            Watcher.Changed += OnFileChanged;
            Watcher.Renamed += OnFileRenamed;

            UpdateList();
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            // If renamed away, we can mark inactive
            Status = "Renamed (not watching)";
            Log += $"{DateTime.Now:HH:mm:ss.fff} File renamed: {e.OldName} -> {e.Name}\r\n";
            UpdateList();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!string.Equals(e.FullPath, FullPath, StringComparison.OrdinalIgnoreCase))
                return;

            // Wait until file is readable (same as your plugin watcher pattern)
            WaitUntilReadable(e.FullPath);

            try
            {
                var lastWriteTime = File.GetLastWriteTime(e.FullPath);
                if (lastWriteTime == FileUpdated)
                    return;

                FileUpdated = lastWriteTime;

                Status = "Updating...";
                Log += $"{DateTime.Now:HH:mm:ss.fff} File updated\r\n";
                UpdateList();

                // Update content
                var webResource = new Entity("webresource", WebResourceId);
                webResource["content"] = Convert.ToBase64String(ReadFile(e.FullPath));
                service.Update(webResource);

                Log += $"{DateTime.Now:HH:mm:ss.fff} Dataverse webresource updated\r\n";
                Status = config.PublishEnabled ? "Queued publish..." : "Update ok";
                UpdateList();

                // Publish with debounce
                if (config.PublishEnabled)
                    QueuePublish();
            }
            catch (Exception ex)
            {
                Status = $"Error: {ex.Message}";
                Log += $"{DateTime.Now:HH:mm:ss.fff} ERROR: {ex}\r\n";
                UpdateList();
            }
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
                        Status = "Publishing...";
                        UpdateList();

                        PublishWebResource(WebResourceId);

                        PublishedUpdated = DateTime.Now;
                        Status = "Published";
                        Log += $"{DateTime.Now:HH:mm:ss.fff} Published\r\n";
                        UpdateList();
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

        private void PublishWebResource(Guid webResourceId)
        {
            // Use GUID without braces
            var id = webResourceId.ToString("D").ToUpperInvariant();

            var request = new OrganizationRequest("PublishXml");
            request["ParameterXml"] =
                $"<importexportxml><webresources><webresource>{id}</webresource></webresources></importexportxml>";

            service.Execute(request);
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

        private static void WaitUntilReadable(string filePath)
        {
            while (true)
            {
                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (stream.Length >= 0)
                            break;
                    }
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
                Thread.Sleep(250);
            }
        }

        private static byte[] ReadFile(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        private void UpdateList()
        {
            MethodInvoker mi = delegate
            {
                while (ListItem.SubItems.Count < 5)
                    ListItem.SubItems.Add(string.Empty);

                // Reuse same columns as WatchPluginFile: File, Path, FileUpdated, PluginUpdated, Status
                ListItem.Text = FileName;
                ListItem.SubItems[1].Text = FolderPath;
                ListItem.SubItems[2].Text = FileUpdated.Ticks != 0 ? FileUpdated.ToString("HH:mm:ss.fff") : "";
                ListItem.SubItems[3].Text = PublishedUpdated.Ticks != 0 ? PublishedUpdated.ToString("HH:mm:ss.fff") : "";
                ListItem.SubItems[4].Text = $"WR: {Status}";

                OnChanged();
            };

            if (owner.InvokeRequired) owner.Invoke(mi);
            else mi();
        }

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
                }
            }
            catch { /* ignore */ }

            lock (publishLock)
            {
                publishTimer?.Dispose();
                publishTimer = null;
            }
        }
    }
}
