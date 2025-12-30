using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using McTools.Xrm.Connection;
using McTools.Xrm.Connection.WinForms.AppCode;
using XrmToolBox.Extensibility;

namespace XrmToolBox.AutoDeployer
{
    public partial class WebResourcesManagerDialog : Form
    {
        private WebResourceWatchConfig config;
        

        public WebResourcesManagerDialog(WebResourceWatchConfig initial)
        {
            InitializeComponent();
            config = initial ?? new WebResourceWatchConfig { PublishEnabled = true, DebounceMs = 1500 };
            PopulateFieldsFromConfig();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFieldsToConfig();
            DialogResult = DialogResult.OK;
            Close();
        }



        private void PopulateFieldsFromConfig()
        {
            if (config == null) config = new WebResourceWatchConfig();
            txtRootFolder.Text = config.RootPath ?? string.Empty;
            txtPrefix.Text = config.Prefix ?? string.Empty;
            txtPatterns.Text = config.Patterns ?? string.Empty;
            chkPublishAfterUpdate.Checked = config.PublishEnabled;
            numDebounce.Value = config.DebounceMs > 0 ? config.DebounceMs : 1500;
            dgvResources.Rows.Clear();
            if (config.Mappings != null)
            {
                foreach (var m in config.Mappings)
                {
                    dgvResources.Rows.Add(m.IsActive, m.RelativePath, m.CrmName, string.Empty, string.Empty);
                }
            }
        }

        private void SaveFieldsToConfig()
        {
            if (config == null) config = new WebResourceWatchConfig();
            config.RootPath = (txtRootFolder.Text ?? "").Trim();
            config.Prefix = (txtPrefix.Text ?? "").Trim();
            config.Patterns = txtPatterns.Text;
            config.PublishEnabled = chkPublishAfterUpdate.Checked;
            config.DebounceMs = (int)numDebounce.Value;
            config.Mappings = new List<WebResourceMapping>();
            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;
                config.Mappings.Add(new WebResourceMapping
                {
                    IsActive = Convert.ToBoolean(row.Cells["Watch"].Value ?? false),
                    RelativePath = row.Cells["RelativePath"].Value?.ToString() ?? "",
                    CrmName = row.Cells["CrmName"].Value?.ToString() ?? ""

                });
            }
        }

     

        private void btnBrowseRoot_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the root folder that contains the built webresource files";
                if (Directory.Exists(txtRootFolder.Text))
                    dlg.SelectedPath = txtRootFolder.Text;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                    txtRootFolder.Text = dlg.SelectedPath;
            }
        }
        private bool ValidateInputs(out string rootPath, out string prefix, out string[] patterns)
        {
            rootPath = (txtRootFolder.Text ?? string.Empty).Trim();
            prefix = (txtPrefix.Text ?? string.Empty).Trim();

            patterns = (txtPatterns.Text ?? string.Empty)
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray();

            if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath))
            {
                MessageBox.Show(this, "Please select a valid Root folder.", "AutoDeployer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                MessageBox.Show(this, "Please enter a Prefix (e.g. cint_mua).", "AutoDeployer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (patterns.Length == 0)
            {
                MessageBox.Show(this, "Please add at least one pattern (e.g. scripts\\*.js).", "AutoDeployer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private static bool WildcardMatch(string input, string pattern)
        {
            // Very small glob: * and ? only, case-insensitive
            // Normalize slashes for matching
            input = input.Replace('/', '\\');
            pattern = pattern.Replace('/', '\\');

            int i = 0, p = 0, star = -1, mark = -1;

            while (i < input.Length)
            {
                if (p < pattern.Length && (pattern[p] == '?' || char.ToLowerInvariant(pattern[p]) == char.ToLowerInvariant(input[i])))
                {
                    i++; p++;
                }
                else if (p < pattern.Length && pattern[p] == '*')
                {
                    star = p++;
                    mark = i;
                }
                else if (star != -1)
                {
                    p = star + 1;
                    i = ++mark;
                }
                else
                {
                    return false;
                }
            }

            while (p < pattern.Length && pattern[p] == '*') p++;
            return p == pattern.Length;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var rootPath, out var prefix, out var patterns))
                return;

            var allFiles = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

            int added = 0;
            int matched = 0;

            foreach (var file in allFiles)
            {
                var rel = GetRelativePath(rootPath, file); // scripts\foo.js
                if (!patterns.Any(pat => WildcardMatch(rel, pat)))
                    continue;

                matched++;

                var existingRow = FindRowByRelativePath(rel);
                if (existingRow != null)
                    continue;

                var crmName = BuildCrmName(prefix, rel);
                dgvResources.Rows.Add(false, rel, crmName, "", "");
                added++;
            }

            MessageBox.Show(this, $"Matched: {matched}\r\nAdded new: {added}", "AutoDeployer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private DataGridViewRow FindRowByRelativePath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return null;

            var key = relativePath.Trim();

            if (!dgvResources.Columns.Contains("RelativePath"))
                return null; 

            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;

                var val = row.Cells["RelativePath"].Value?.ToString();
                if (val != null && string.Equals(val.Trim(), key, StringComparison.OrdinalIgnoreCase))
                    return row;
            }

            return null;
        }


        private static string GetRelativePath(string root, string fullPath)
        {
            var rootNorm = Path.GetFullPath(root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar);
            var fullNorm = Path.GetFullPath(fullPath);

            if (!fullNorm.StartsWith(rootNorm, StringComparison.OrdinalIgnoreCase))
                return Path.GetFileName(fullNorm);

            return fullNorm.Substring(rootNorm.Length);
        }

        private static string BuildCrmName(string prefix, string relativePath)
        {
            prefix = prefix.Trim().TrimEnd('/', '\\');
            var rel = relativePath.Replace('\\', '/').TrimStart('/');
            return $"{prefix}/{rel}";
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvResources.SelectedRows)
            {
                if (!row.IsNewRow)
                    dgvResources.Rows.Remove(row);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public WebResourceWatchConfig Config => config;

    }
}
