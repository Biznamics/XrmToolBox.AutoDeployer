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


        private readonly Microsoft.Xrm.Sdk.IOrganizationService _service;

        public WebResourcesManagerDialog(WebResourceWatchConfig initial, Microsoft.Xrm.Sdk.IOrganizationService service)
        {
            InitializeComponent();
            dgvResources.Columns["RelativePath"].ReadOnly = true;
            dgvResources.Columns["CrmName"].ReadOnly = true;
            dgvResources.Columns["ExistsInCrm"].ReadOnly = true;
            dgvResources.Columns["Status"].ReadOnly = true;

            // allow only the checkbox
            dgvResources.Columns["Watch"].ReadOnly = false;

            _service = service; // can be null
            config = initial ?? new WebResourceWatchConfig { PublishEnabled = true, DebounceMs = 1500 };
            PopulateFieldsFromConfig();
        }

        private void RebuildCrmNamesFromPrefix(string prefix)
        {
            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;

                var rel = (row.Cells["RelativePath"].Value?.ToString() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(rel)) continue;

                row.Cells["CrmName"].Value = BuildCrmName(prefix, rel);
            }
        }
        private HashSet<string> GetExistingWebResourceNames(IEnumerable<string> names)
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = names.Where(n => !string.IsNullOrWhiteSpace(n)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (list.Count == 0) return set;

            const int chunkSize = 200; // safe
            for (int i = 0; i < list.Count; i += chunkSize)
            {
                var chunk = list.Skip(i).Take(chunkSize).ToArray();

                var qe = new Microsoft.Xrm.Sdk.Query.QueryExpression("webresource")
                {
                    ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("name")
                };
                qe.Criteria.AddCondition("name", Microsoft.Xrm.Sdk.Query.ConditionOperator.In, chunk.Cast<object>().ToArray());

                var res = _service.RetrieveMultiple(qe);
                foreach (var e in res.Entities)
                {
                    var name = e.GetAttributeValue<string>("name");
                    if (!string.IsNullOrWhiteSpace(name))
                        set.Add(name);
                }
            }

            return set;
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
                var rel = (row.Cells["RelativePath"].Value?.ToString() ?? "").Trim();
                config.Mappings.Add(new WebResourceMapping
                {
                    IsActive = Convert.ToBoolean(row.Cells["Watch"].Value ?? false),
                    RelativePath = rel,
                    CrmName = BuildCrmName(config.Prefix, rel) // recompute, don’t trust grid
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
            RebuildCrmNamesFromPrefix(prefix);
            var allFiles = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

            int added = 0;
            int matched = 0;

            foreach (var file in allFiles)
            {
                var rel = GetRelativePath(rootPath, file); // scripts\foo.js
                if (!patterns.Any(pat => WildcardMatch(rel, pat)))
                    continue;

                matched++;

                var crmName = BuildCrmName(prefix, rel);

                var existingRow = FindRowByRelativePath(rel);
                if (existingRow != null)
                {
                    // Prefix may have changed since row was added => rewrite CrmName
                    existingRow.Cells["CrmName"].Value = crmName;
                    continue;
                }

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

        private async void btnValidate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out var rootPath, out var prefix, out var patterns))
                return;

            // Always rebuild names (prevents "stuck with mistake")
            RebuildCrmNamesFromPrefix(prefix);

            // Clear status columns
            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;
                row.Cells["ExistsInCrm"].Value = "";
                row.Cells["Status"].Value = "";
                
                ResetCellStyle(row.Cells["ExistsInCrm"]);
                ResetCellStyle(row.Cells["Status"]);
            }

            var errors = new List<string>();
            var relSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var crmSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // local validation first + collect crm names
            var crmNames = new List<string>();

            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;

                var rel = (row.Cells["RelativePath"].Value?.ToString() ?? "").Trim();
                var crm = (row.Cells["CrmName"].Value?.ToString() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(crm))
                {
                    row.Cells["Status"].Value = "Missing CrmName";
                    errors.Add($"Missing CrmName for: {rel}");
                    ApplyValidationCellStyles(row);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(rel))
                {
                    row.Cells["Status"].Value = "Missing RelativePath";
                    errors.Add("One or more rows are missing RelativePath.");
                    ApplyValidationCellStyles(row);
                    continue;
                }

                if (!relSet.Add(rel))
                {
                    row.Cells["Status"].Value = "Duplicate RelativePath";
                    errors.Add($"Duplicate RelativePath: {rel}");
                    ApplyValidationCellStyles(row);
                }

                if (!crmSet.Add(crm))
                {
                    row.Cells["Status"].Value = "Duplicate CrmName";
                    errors.Add($"Duplicate CrmName: {crm}");
                    ApplyValidationCellStyles(row);
                }

                var fullPath = Path.Combine(rootPath, rel);
                if (!File.Exists(fullPath))
                {
                    row.Cells["Status"].Value = "File not found under Root folder";
                    errors.Add($"File missing: {fullPath}");
                }
                ApplyValidationCellStyles(row);

                crmNames.Add(crm);
            }

            // Dataverse validation (only if we have a service)
            HashSet<string> existing = null;

            if (_service != null)
            {
                btnValidate.Enabled = false;
                Cursor = Cursors.WaitCursor;

                try
                {
                    existing = await Task.Run(() => GetExistingWebResourceNames(crmNames));
                }
                finally
                {
                    Cursor = Cursors.Default;
                    btnValidate.Enabled = true;
                }
            }

            // apply CRM results + final status
            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;

                var crm = (row.Cells["CrmName"].Value?.ToString() ?? "").Trim();
                var currentStatus = row.Cells["Status"].Value?.ToString();

                if (_service == null)
                {
                    // local-only fallback if no connection
                    if (string.IsNullOrWhiteSpace(currentStatus))
                        row.Cells["Status"].Value = "OK (local only)";
                    ApplyValidationCellStyles(row);
                    continue;
                }

                var exists = existing != null && existing.Contains(crm);
                row.Cells["ExistsInCrm"].Value = exists ? "Yes" : "No";

                if (!exists && string.IsNullOrWhiteSpace(currentStatus))
                    row.Cells["Status"].Value = "Not found in Dataverse";
                else if (exists && string.IsNullOrWhiteSpace(currentStatus))
                    row.Cells["Status"].Value = "OK";
                ApplyValidationCellStyles(row);
            }

            if (errors.Count > 0)
            {
                MessageBox.Show(this,
                    $"Validation completed with {errors.Count} local issue(s).\r\n\r\n" +
                    string.Join("\r\n", errors.Take(10)) + (errors.Count > 10 ? "\r\n..." : ""),
                    "AutoDeployer - Validate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show(this, "Validation completed.", "AutoDeployer - Validate",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void RecalculateCrmNames(string prefix, bool updateStatus = false)
        {
            prefix = (prefix ?? string.Empty).Trim();

            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                if (row.IsNewRow) continue;

                var rel = (row.Cells["RelativePath"].Value?.ToString() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(rel)) continue;

                var expected = BuildCrmName(prefix, rel);
                var current = (row.Cells["CrmName"].Value?.ToString() ?? "").Trim();

                if (!string.Equals(current, expected, StringComparison.OrdinalIgnoreCase))
                {
                    row.Cells["CrmName"].Value = expected;

                    if (updateStatus)
                    {
                        // only set Status if it isn't already an error you want to keep
                        var status = (row.Cells["Status"].Value?.ToString() ?? "").Trim();
                        if (string.IsNullOrWhiteSpace(status) || status.StartsWith("CrmName", StringComparison.OrdinalIgnoreCase) || status == "OK")
                            row.Cells["Status"].Value = "CrmName updated from Prefix";
                    }
                }
            }
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            var prefix = (txtPrefix.Text ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
                RecalculateCrmNames(prefix, updateStatus: false);
        }
        private void ApplyValidationCellStyles(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow) return;

            var existsCell = row.Cells["ExistsInCrm"];
            var statusCell = row.Cells["Status"];

            // Reset first (so old red/yellow goes away when things become OK)
            ResetCellStyle(existsCell);
            ResetCellStyle(statusCell);

            var exists = (existsCell.Value?.ToString() ?? "").Trim();   // "Yes"/"No"/""
            var status = (statusCell.Value?.ToString() ?? "").Trim();

            bool localError =
                status.StartsWith("Missing", StringComparison.OrdinalIgnoreCase) ||
                status.StartsWith("Duplicate", StringComparison.OrdinalIgnoreCase) ||
                status.IndexOf("File not found", StringComparison.OrdinalIgnoreCase) >= 0;

            bool notInDataverse =
                exists.Equals("No", StringComparison.OrdinalIgnoreCase) ||
                status.IndexOf("Not found in Dataverse", StringComparison.OrdinalIgnoreCase) >= 0;

            if (localError)
            {
                // Strong red for local errors
                MarkCellError(statusCell);
                return;
            }

            if (notInDataverse)
            {
                // Softer warning for Dataverse missing
                MarkCellWarning(statusCell);
                MarkCellError(existsCell);
                return;
            }

            // OK => green on BOTH Exists + Status
            bool ok =
                status.Equals("OK", StringComparison.OrdinalIgnoreCase) ||
                status.Equals("OK (local only)", StringComparison.OrdinalIgnoreCase);

            if (ok)
            {
                MarkCellOk(existsCell);
                MarkCellOk(statusCell);
                return;
            }
        }

        private void ResetCellStyle(DataGridViewCell cell)
        {
            if (cell == null) return;
            cell.Style.BackColor = dgvResources.DefaultCellStyle.BackColor;
            cell.Style.ForeColor = dgvResources.DefaultCellStyle.ForeColor;
            cell.Style.Font = dgvResources.DefaultCellStyle.Font;
        }

        private void MarkCellError(DataGridViewCell cell)
        {
            if (cell == null) return;
            cell.Style.BackColor = System.Drawing.Color.MistyRose;
            cell.Style.ForeColor = System.Drawing.Color.DarkRed;
            cell.Style.Font = new System.Drawing.Font(dgvResources.Font, System.Drawing.FontStyle.Bold);
        }

        private void MarkCellWarning(DataGridViewCell cell)
        {
            if (cell == null) return;
            cell.Style.BackColor = System.Drawing.Color.LemonChiffon;
            cell.Style.ForeColor = System.Drawing.Color.SaddleBrown;
        }
        private void MarkCellOk(DataGridViewCell cell)
        {
            if (cell == null) return;
            cell.Style.BackColor = System.Drawing.Color.Honeydew;
            cell.Style.ForeColor = System.Drawing.Color.DarkGreen;
            cell.Style.Font = new System.Drawing.Font(dgvResources.Font, System.Drawing.FontStyle.Bold);
        }


        private void dgvResources_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ApplyValidationCellStyles(dgvResources.Rows[e.RowIndex]);
        }
    }
}
