using System.Windows.Forms;

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
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseRoot = new System.Windows.Forms.Button();
            this.lblPrefix = new System.Windows.Forms.Label();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.lblPatterns = new System.Windows.Forms.Label();
            this.txtPatterns = new System.Windows.Forms.TextBox();
            this.lblPublishAfterUpdate = new System.Windows.Forms.Label();
            this.chkPublishAfterUpdate = new System.Windows.Forms.CheckBox();
            this.lblDebounce = new System.Windows.Forms.Label();
            this.numDebounce = new System.Windows.Forms.NumericUpDown();
            this.dgvResources = new System.Windows.Forms.DataGridView();
            this.Watch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RelativePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CrmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExistsInCrm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDebounce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.lblRootFolder);
            this.topPanel.Controls.Add(this.txtRootFolder);
            this.topPanel.Controls.Add(this.btnBrowseRoot);
            this.topPanel.Controls.Add(this.lblPrefix);
            this.topPanel.Controls.Add(this.txtPrefix);
            this.topPanel.Controls.Add(this.lblPatterns);
            this.topPanel.Controls.Add(this.txtPatterns);
            this.topPanel.Controls.Add(this.lblPublishAfterUpdate);
            this.topPanel.Controls.Add(this.chkPublishAfterUpdate);
            this.topPanel.Controls.Add(this.lblDebounce);
            this.topPanel.Controls.Add(this.numDebounce);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(900, 120);
            this.topPanel.TabIndex = 1;
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(10, 10);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(62, 13);
            this.lblRootFolder.TabIndex = 0;
            this.lblRootFolder.Text = "Root folder:";
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootFolder.Location = new System.Drawing.Point(100, 7);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.Size = new System.Drawing.Size(500, 20);
            this.txtRootFolder.TabIndex = 0;
            // 
            // btnBrowseRoot
            // 
            this.btnBrowseRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRoot.Location = new System.Drawing.Point(610, 5);
            this.btnBrowseRoot.Name = "btnBrowseRoot";
            this.btnBrowseRoot.Size = new System.Drawing.Size(80, 23);
            this.btnBrowseRoot.TabIndex = 0;
            this.btnBrowseRoot.Text = "Browse...";
            this.btnBrowseRoot.Click += new System.EventHandler(this.btnBrowseRoot_Click);
            // 
            // lblPrefix
            // 
            this.lblPrefix.AutoSize = true;
            this.lblPrefix.Location = new System.Drawing.Point(10, 40);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(36, 13);
            this.lblPrefix.TabIndex = 0;
            this.lblPrefix.Text = "Prefix:";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(100, 37);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(200, 20);
            this.txtPrefix.TabIndex = 0;
            // 
            // lblPatterns
            // 
            this.lblPatterns.AutoSize = true;
            this.lblPatterns.Location = new System.Drawing.Point(320, 40);
            this.lblPatterns.Name = "lblPatterns";
            this.lblPatterns.Size = new System.Drawing.Size(49, 13);
            this.lblPatterns.TabIndex = 0;
            this.lblPatterns.Text = "Patterns:";
            // 
            // txtPatterns
            // 
            this.txtPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPatterns.Location = new System.Drawing.Point(390, 37);
            this.txtPatterns.Multiline = true;
            this.txtPatterns.Name = "txtPatterns";
            this.txtPatterns.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPatterns.Size = new System.Drawing.Size(300, 40);
            this.txtPatterns.TabIndex = 0;
            // 
            // lblPublishAfterUpdate
            // 
            this.lblPublishAfterUpdate.AutoSize = true;
            this.lblPublishAfterUpdate.Location = new System.Drawing.Point(10, 80);
            this.lblPublishAfterUpdate.Name = "lblPublishAfterUpdate";
            this.lblPublishAfterUpdate.Size = new System.Drawing.Size(104, 13);
            this.lblPublishAfterUpdate.TabIndex = 0;
            this.lblPublishAfterUpdate.Text = "Publish after update:";
            // 
            // chkPublishAfterUpdate
            // 
            this.chkPublishAfterUpdate.Checked = true;
            this.chkPublishAfterUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPublishAfterUpdate.Location = new System.Drawing.Point(140, 78);
            this.chkPublishAfterUpdate.Name = "chkPublishAfterUpdate";
            this.chkPublishAfterUpdate.Size = new System.Drawing.Size(32, 24);
            this.chkPublishAfterUpdate.TabIndex = 0;
            // 
            // lblDebounce
            // 
            this.lblDebounce.AutoSize = true;
            this.lblDebounce.Location = new System.Drawing.Point(178, 80);
            this.lblDebounce.Name = "lblDebounce";
            this.lblDebounce.Size = new System.Drawing.Size(76, 13);
            this.lblDebounce.TabIndex = 0;
            this.lblDebounce.Text = "Debounce ms:";
            // 
            // numDebounce
            // 
            this.numDebounce.Location = new System.Drawing.Point(260, 78);
            this.numDebounce.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numDebounce.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numDebounce.Name = "numDebounce";
            this.numDebounce.Size = new System.Drawing.Size(80, 20);
            this.numDebounce.TabIndex = 0;
            this.numDebounce.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            // 
            // dgvResources
            // 
            this.dgvResources.AllowUserToAddRows = false;
            this.dgvResources.AllowUserToDeleteRows = false;
            this.dgvResources.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Watch,
            this.RelativePath,
            this.CrmName,
            this.ExistsInCrm,
            this.Status});
            this.dgvResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResources.Location = new System.Drawing.Point(0, 120);
            this.dgvResources.Name = "dgvResources";
            this.dgvResources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResources.Size = new System.Drawing.Size(900, 430);
            this.dgvResources.TabIndex = 0;
            // 
            // Watch
            // 
            this.Watch.HeaderText = "Watch";
            this.Watch.Name = "Watch";
            // 
            // RelativePath
            // 
            this.RelativePath.HeaderText = "RelativePath";
            this.RelativePath.Name = "RelativePath";
            this.RelativePath.ReadOnly = true;
            // 
            // CrmName
            // 
            this.CrmName.HeaderText = "CrmName";
            this.CrmName.Name = "CrmName";
            // 
            // ExistsInCrm
            // 
            this.ExistsInCrm.HeaderText = "ExistsInCrm";
            this.ExistsInCrm.Name = "ExistsInCrm";
            this.ExistsInCrm.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.btnScan);
            this.bottomPanel.Controls.Add(this.btnValidate);
            this.bottomPanel.Controls.Add(this.btnSave);
            this.bottomPanel.Controls.Add(this.btnRemoveSelected);
            this.bottomPanel.Controls.Add(this.btnClose);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 550);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(900, 50);
            this.bottomPanel.TabIndex = 2;
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(10, 10);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(90, 30);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(110, 10);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(90, 30);
            this.btnValidate.TabIndex = 1;
            this.btnValidate.Text = "Validate";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(210, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Location = new System.Drawing.Point(310, 10);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(120, 30);
            this.btnRemoveSelected.TabIndex = 3;
            this.btnRemoveSelected.Text = "Remove Selected";
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(800, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDebounce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridViewCheckBoxColumn Watch;
        private System.Windows.Forms.DataGridViewTextBoxColumn RelativePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn CrmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExistsInCrm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}