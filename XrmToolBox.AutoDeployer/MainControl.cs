namespace XrmToolBox.AutoDeployer
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using XrmToolBox.Extensibility;
    using XrmToolBox.Extensibility.Interfaces;

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
            "Innofactor";

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

        internal FileSystemWatcher Watcher
        {
            get;
            private set;
        }

        #endregion Private/Internal Properties

        #region Private Methods

        private void bAdd_ButtonClick(object sender, EventArgs e)
        {
            AddPluginAssembly();
        }

        private void bAddPluginMenuItem_Click(object sender, EventArgs e)
        {
            AddPluginAssembly();
        }

        private void bAddWebResourceMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new WebResourcesManagerDialog())
            {
                dialog.ShowDialog(this);
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
                bDelPlugin.Enabled = true;
            }
        }

        private void Plugin_Changed(object sender, EventArgs e)
        {
            if (listWatching.SelectedItems.Count == 1 && listWatching.SelectedItems[0].Tag is WatchPluginFile plugin)
            {
                txtLog.Text = plugin.Log;
            }
            else
            {
                txtLog.Text = string.Empty;
            }
        }

        #endregion Private Methods

        private void bDelPlugin_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listWatching.SelectedItems)
            {
                if (item.Tag is WatchPluginFile watch)
                {
                    watch.Dispose();
                }
                listWatching.Items.Remove(item);
            }
        }

        private void listWatching_SelectedIndexChanged(object sender, EventArgs e)
        {
            Plugin_Changed(sender, e);
            bDelPlugin.Enabled = listWatching.SelectedItems.Count > 0;
        }
    }
}