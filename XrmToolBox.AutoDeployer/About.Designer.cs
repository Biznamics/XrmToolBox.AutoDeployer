
namespace XrmToolBox.AutoDeployer
{
    partial class About
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabelImran = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabelAlexey = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblHeading = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listAssemblies = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.linkRappen = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 323);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Imran Akram";
            // 
            // linkLabelImran
            // 
            this.linkLabelImran.AutoSize = true;
            this.linkLabelImran.Location = new System.Drawing.Point(101, 323);
            this.linkLabelImran.Name = "linkLabelImran";
            this.linkLabelImran.Size = new System.Drawing.Size(90, 13);
            this.linkLabelImran.TabIndex = 54;
            this.linkLabelImran.TabStop = true;
            this.linkLabelImran.Text = "@imranakram365";
            this.linkLabelImran.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelImran_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 370);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 53;
            this.label3.Text = "Alexey Shytikov";
            // 
            // linkLabelAlexey
            // 
            this.linkLabelAlexey.AutoSize = true;
            this.linkLabelAlexey.Location = new System.Drawing.Point(101, 370);
            this.linkLabelAlexey.Name = "linkLabelAlexey";
            this.linkLabelAlexey.Size = new System.Drawing.Size(57, 13);
            this.linkLabelAlexey.TabIndex = 52;
            this.linkLabelAlexey.TabStop = true;
            this.linkLabelAlexey.Text = "@shytikov";
            this.linkLabelAlexey.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelAlexey_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 298);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 16);
            this.label1.TabIndex = 51;
            this.label1.Text = "Brought to you by:";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(343, 131);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(22, 13);
            this.lblVersion.TabIndex = 50;
            this.lblVersion.Text = "1.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(292, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 49;
            this.label5.Text = "Version:";
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(292, 84);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(276, 37);
            this.lblDescription.TabIndex = 48;
            this.lblDescription.Text = "Tool to automatically upload plugin assemblies and web resources as they are chan" +
    "ged during build process";
            // 
            // lblHeading
            // 
            this.lblHeading.AutoSize = true;
            this.lblHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(289, 40);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(199, 31);
            this.lblHeading.TabIndex = 47;
            this.lblHeading.Text = "Auto Deployer";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listAssemblies);
            this.groupBox1.Location = new System.Drawing.Point(295, 171);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(291, 222);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            // 
            // listAssemblies
            // 
            this.listAssemblies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listAssemblies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listAssemblies.FullRowSelect = true;
            this.listAssemblies.GridLines = true;
            this.listAssemblies.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listAssemblies.HideSelection = false;
            this.listAssemblies.Location = new System.Drawing.Point(2, 15);
            this.listAssemblies.Margin = new System.Windows.Forms.Padding(2);
            this.listAssemblies.Name = "listAssemblies";
            this.listAssemblies.Scrollable = false;
            this.listAssemblies.ShowGroups = false;
            this.listAssemblies.Size = new System.Drawing.Size(287, 205);
            this.listAssemblies.TabIndex = 0;
            this.listAssemblies.UseCompatibleStateImageBehavior = false;
            this.listAssemblies.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Assembly";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 100;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::XrmToolBox.AutoDeployer.Properties.Resources.ADT_withText_256;
            this.pictureBox1.Location = new System.Drawing.Point(18, 40);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(256, 256);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 46;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "Jonas Rapp";
            // 
            // linkRappen
            // 
            this.linkRappen.AutoSize = true;
            this.linkRappen.Location = new System.Drawing.Point(101, 346);
            this.linkRappen.Name = "linkRappen";
            this.linkRappen.Size = new System.Drawing.Size(51, 13);
            this.linkRappen.TabIndex = 57;
            this.linkRappen.TabStop = true;
            this.linkRappen.Text = "@rappen";
            this.linkRappen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRappen_LinkClicked);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 442);
            this.Controls.Add(this.linkRappen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLabelImran);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.linkLabelAlexey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblHeading);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            this.Text = "About";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabelImran;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabelAlexey;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listAssemblies;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkRappen;
    }
}