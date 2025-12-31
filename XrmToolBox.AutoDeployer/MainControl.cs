namespace XrmToolBox.AutoDeployer
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using XrmToolBox.Extensibility;
    using XrmToolBox.Extensibility.Interfaces;
    using System.Collections.Generic;
    using McTools.Xrm.Connection;

    public partial class MainControl : PluginControlBase, IGitHubPlugin, IWorkerHost, IAboutPlugin
    {
        #region Public Constructors

        public MainControl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        string IGitHubPlugin.RepositoryName =>
            "XrmToolBox.AutoDeployer";

        string IGitHubPlugin.UserName =>
            "Biznamics";

        #endregion Public Properties

        #region Public Methods

        public void ShowAboutDialog()
        {
            try
            {
                var about = new About();
                //StartPosition = FormStartPosition.CenterParent
                about.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion Public Methods

        #region Private/Internal Properties

      
        private readonly List<WatchWebResourceFile> _webResourceWatchers = new List<WatchWebResourceFile>();
        #endregion Private/Internal Properties

        #region Private Methods

        private void bAddPluginMenuItem_Click(object sender, EventArgs e)
        {
            AddPluginAssembly();
        }

        private void bAddWebResourceMenuItem_Click(object sender, EventArgs e)
        {
            if (ConnectionDetail == null)
            {
                MessageBox.Show("Connect to an environment first.", "AutoDeployer",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var cfg = LoadWebResourceConfig();

            using (var dialog = new WebResourcesManagerDialog(cfg))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SaveWebResourceConfig(dialog.Config);
                    RefreshWebResourceWatchers();
                }
            }
        }



        private void AddPluginAssembly()
        {
            if (ofdPlugin.ShowDialog() == DialogResult.OK)
            {
                var plugin = new WatchPluginFile(ofdPlugin.FileName, Service, this);
                plugin.Changed += Plugin_Changed;
                listWatching.Items.Add(plugin.ListItem);
                if (listWatching.SelectedItems.Count == 0)
                {
                    listWatching.Items[0].Selected = true;
                }
                bDelSelected.Enabled = true;
            }
        }

        private void Plugin_Changed(object sender, EventArgs e)
        {
            if (listWatching.SelectedItems.Count == 1)
            {
                var tag = listWatching.SelectedItems[0].Tag;
                switch (tag)
                {
                    case WatchPluginFile plugin:
                        txtLog.Text = plugin.Log;
                        return;
                    case WatchWebResourceFile wr:
                        txtLog.Text = wr.Log;
                        return;
                }
            }

            txtLog.Text = string.Empty;
        }


        #endregion Private Methods

        private void bDelSelected_Click(object sender, EventArgs e)
        {
            // Remove from highest index to lowest so indices don't shift
            for (int i = listWatching.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int idx = listWatching.SelectedIndices[i];
                var item = listWatching.Items[idx];

                if (item.Tag is WatchWebResourceFile wr)
                    _webResourceWatchers.Remove(wr);

                if (item.Tag is IDisposable disposable)
                    disposable.Dispose();

                listWatching.Items.RemoveAt(idx);
            }

            bDelSelected.Enabled = listWatching.SelectedItems.Count > 0;
            txtLog.Text = string.Empty;
        }




        private void listWatching_SelectedIndexChanged(object sender, EventArgs e)
        {
            Plugin_Changed(sender, e);
            bDelSelected.Enabled = listWatching.SelectedItems.Count > 0;
        }

        private string GetWebResourceSettingsKey()
        {
            var id = ConnectionDetail?.ConnectionId ?? Guid.Empty;
            return $"WebResourceWatchConfig.{id:D}";
        }

        private void RefreshWebResourceWatchers()
        {
            // Remove existing WR watchers from list + dispose them
            for (int i = listWatching.Items.Count - 1; i >= 0; i--)
            {
                var item = listWatching.Items[i];
                if (item.Tag is WatchWebResourceFile wr)
                {
                    wr.Dispose();
                    listWatching.Items.RemoveAt(i);
                }
            }
            _webResourceWatchers.Clear();

            var cfg = LoadWebResourceConfig();
            if (cfg == null || string.IsNullOrWhiteSpace(cfg.RootPath))
                return;

            if (Service == null)
            {
                MessageBox.Show("Not connected to an environment.", "AutoDeployer",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cfg.Mappings == null || cfg.Mappings.Count == 0)
                return;

            foreach (var mapping in cfg.Mappings)
            {
                if (!mapping.IsActive) continue;

                var watcher = new WatchWebResourceFile(cfg, mapping, Service, this);
                watcher.Changed += Plugin_Changed; // reuse existing log display
                _webResourceWatchers.Add(watcher);
                listWatching.Items.Add(watcher.ListItem);
            }

            bDelSelected.Enabled = listWatching.Items.Count > 0;
        }
        private void ClearWebResourceWatchers()
        {
            for (int i = listWatching.Items.Count - 1; i >= 0; i--)
            {
                var item = listWatching.Items[i];
                if (item.Tag is WatchWebResourceFile wr)
                {
                    wr.Dispose();
                    listWatching.Items.RemoveAt(i);
                }
            }
            _webResourceWatchers.Clear();
        }

        public override void UpdateConnection(Microsoft.Xrm.Sdk.IOrganizationService newService, McTools.Xrm.Connection.ConnectionDetail detail, string actionName,
            object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            // If not connected, clear WR watchers
            if (detail == null || newService == null)
            {
                ClearWebResourceWatchers();
                return;
            }

            // Start watchers for this connection (uses ConnectionDetail key + Service)
            RefreshWebResourceWatchers();
        }
        private string GetWebResourceSettingsName()
        {
            // “name” suffix => per connection file
            return (ConnectionDetail?.ConnectionId ?? Guid.Empty).ToString("D");
        }

        private WebResourceWatchConfig LoadWebResourceConfig()
        {
            var name = GetWebResourceSettingsName();

            if (SettingsManager.Instance.TryLoad(GetType(), out WebResourceWatchConfig cfg, name))
                return cfg;

            return new WebResourceWatchConfig { PublishEnabled = true, DebounceMs = 1500 };
        }
        private void SaveWebResourceConfig(WebResourceWatchConfig cfg)
        {
            var name = GetWebResourceSettingsName();
            SettingsManager.Instance.Save(GetType(), cfg, name);
        }

     
    }
}