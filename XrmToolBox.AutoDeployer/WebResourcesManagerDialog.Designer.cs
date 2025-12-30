namespace XrmToolBox.AutoDeployer
{
    partial class WebResourcesManagerDialog
    {
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Button btnBrowseRoot;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.TextBox txtPatterns;
        private System.Windows.Forms.CheckBox chkPublishAfterUpdate;
        private System.Windows.Forms.NumericUpDown numDebounce;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Label lblPatterns;
        private System.Windows.Forms.Label lblPublishAfterUpdate;
        private System.Windows.Forms.Label lblDebounce;
        private System.Windows.Forms.DataGridView dgvResources;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colWatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelativePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCrmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExistsInCrm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseRoot = new System.Windows.Forms.Button();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.txtPatterns = new System.Windows.Forms.TextBox();
            this.chkPublishAfterUpdate = new System.Windows.Forms.CheckBox();
            this.numDebounce = new System.Windows.Forms.NumericUpDown();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.lblPatterns = new System.Windows.Forms.Label();
            this.lblPublishAfterUpdate = new System.Windows.Forms.Label();
            this.lblDebounce = new System.Windows.Forms.Label();
            this.dgvResources = new System.Windows.Forms.DataGridView();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numDebounce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 120;
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.Text = "Root folder:";
            this.lblRootFolder.Location = new System.Drawing.Point(10, 10);
            this.lblRootFolder.AutoSize = true;
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Location = new System.Drawing.Point(100, 7);
            this.txtRootFolder.Width = 500;
            // 
            // btnBrowseRoot
            // 
            this.btnBrowseRoot.Text = "Browse...";
            this.btnBrowseRoot.Location = new System.Drawing.Point(610, 5);
            this.btnBrowseRoot.Width = 80;
            // 
            // lblPrefix
            // 
            this.lblPrefix.Text = "Prefix:";
            this.lblPrefix.Location = new System.Drawing.Point(10, 40);
            this.lblPrefix.AutoSize = true;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(100, 37);
            this.txtPrefix.Width = 200;
            // 
            // lblPatterns
            // 
            this.lblPatterns.Text = "Patterns:";
            this.lblPatterns.Location = new System.Drawing.Point(320, 40);
            this.lblPatterns.AutoSize = true;
            // 
            // txtPatterns
            // 
            this.txtPatterns.Location = new System.Drawing.Point(390, 37);
            this.txtPatterns.Width = 300;
            this.txtPatterns.Height = 40;
            this.txtPatterns.Multiline = true;
            this.txtPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            // 
            // lblPublishAfterUpdate
            // 
            this.lblPublishAfterUpdate.Text = "Publish after update:";
            this.lblPublishAfterUpdate.Location = new System.Drawing.Point(10, 80);
            this.lblPublishAfterUpdate.AutoSize = true;
            // 
            // chkPublishAfterUpdate
            // 
            this.chkPublishAfterUpdate.Location = new System.Drawing.Point(140, 78);
            this.chkPublishAfterUpdate.Checked = true;
            // 
            // lblDebounce
            // 
            this.lblDebounce.Text = "Debounce ms:";
            this.lblDebounce.Location = new System.Drawing.Point(200, 80);
            this.lblDebounce.AutoSize = true;
            // 
            // numDebounce
            // 
            this.numDebounce.Location = new System.Drawing.Point(290, 77);
            this.numDebounce.Width = 80;
            this.numDebounce.Minimum = 100;
            this.numDebounce.Maximum = 10000;
            this.numDebounce.Value = 1500;
            // 
            // dgvResources
            // 
            this.dgvResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResources.AllowUserToAddRows = false;
            this.dgvResources.AllowUserToDeleteRows = false;
            this.dgvResources.ReadOnly = false;
            this.dgvResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResources.MultiSelect = true;
            this.dgvResources.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.colWatch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colRelativePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCrmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExistsInCrm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.colWatch.Name = "Watch";
            this.colWatch.HeaderText = "Watch";
            this.colWatch.Width = 60;

            this.colRelativePath.Name = "RelativePath";
            this.colRelativePath.HeaderText = "RelativePath";
            this.colRelativePath.ReadOnly = true;

            this.colCrmName.Name = "CrmName";
            this.colCrmName.HeaderText = "CrmName";

            this.colExistsInCrm.Name = "ExistsInCrm";
            this.colExistsInCrm.HeaderText = "ExistsInCrm";
            this.colExistsInCrm.ReadOnly = true;

            this.colStatus.Name = "Status";
            this.colStatus.HeaderText = "Status";
            this.colStatus.ReadOnly = true;

            this.dgvResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                                                this.colWatch,
                                                this.colRelativePath,
                                                this.colCrmName,
                                                this.colExistsInCrm,
                                                this.colStatus
                                            });

            // 
            // bottomPanel
            // 
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Height = 50;
            // 
            // btnScan
            // 
            this.btnScan.Text = "Scan";
            this.btnScan.Width = 90;
            this.btnScan.Height = 30;
            this.btnScan.Location = new System.Drawing.Point(10, 10);
            // 
            // btnValidate
            // 
            this.btnValidate.Text = "Validate";
            this.btnValidate.Width = 90;
            this.btnValidate.Height = 30;
            this.btnValidate.Location = new System.Drawing.Point(110, 10);
            // 
            // btnSave
            // 
            this.btnSave.Text = "Save";
            this.btnSave.Width = 90;
            this.btnSave.Height = 30;
            this.btnSave.Location = new System.Drawing.Point(210, 10);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.Width = 120;
            this.btnRemoveSelected.Height = 30;
            this.btnRemoveSelected.Location = new System.Drawing.Point(310, 10);
            // 
            // btnClose
            // 
            this.btnClose.Text = "Close";
            this.btnClose.Width = 90;
            this.btnClose.Height = 30;
            this.btnClose.Location = new System.Drawing.Point(760, 10);
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            
            // 
            // Add controls to bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.btnScan);
            this.bottomPanel.Controls.Add(this.btnValidate);
            this.bottomPanel.Controls.Add(this.btnSave);
            this.bottomPanel.Controls.Add(this.btnRemoveSelected);
            this.bottomPanel.Controls.Add(this.btnClose);
            // 
            // WebResourcesManagerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.dgvResources);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "WebResourcesManagerDialog";
            this.Text = "Web Resources Manager";
            ((System.ComponentModel.ISupportInitialize)(this.numDebounce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}