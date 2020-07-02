namespace LinkeD365.ERDBuilder
{
    partial class SolutionPicker
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
            this.listSolutions = new System.Windows.Forms.ListView();
            this.colFriendlyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPublisher = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitSolutions = new System.Windows.Forms.SplitContainer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitSolutions)).BeginInit();
            this.splitSolutions.Panel1.SuspendLayout();
            this.splitSolutions.Panel2.SuspendLayout();
            this.splitSolutions.SuspendLayout();
            this.SuspendLayout();
            // 
            // listSolutions
            // 
            this.listSolutions.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listSolutions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFriendlyName,
            this.colVersion,
            this.colPublisher});
            this.listSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSolutions.FullRowSelect = true;
            this.listSolutions.GridLines = true;
            this.listSolutions.HideSelection = false;
            this.listSolutions.Location = new System.Drawing.Point(0, 0);
            this.listSolutions.Name = "listSolutions";
            this.listSolutions.Size = new System.Drawing.Size(527, 250);
            this.listSolutions.TabIndex = 0;
            this.listSolutions.UseCompatibleStateImageBehavior = false;
            this.listSolutions.View = System.Windows.Forms.View.Details;
            this.listSolutions.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listSolutions_ColumnClick);
            // 
            // colFriendlyName
            // 
            this.colFriendlyName.Text = "Friendly Name";
            // 
            // colVersion
            // 
            this.colVersion.Text = "Version";
            // 
            // colPublisher
            // 
            this.colPublisher.Text = "Publisher";
            // 
            // splitSolutions
            // 
            this.splitSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSolutions.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitSolutions.Location = new System.Drawing.Point(0, 0);
            this.splitSolutions.Name = "splitSolutions";
            this.splitSolutions.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitSolutions.Panel1
            // 
            this.splitSolutions.Panel1.Controls.Add(this.listSolutions);
            // 
            // splitSolutions.Panel2
            // 
            this.splitSolutions.Panel2.Controls.Add(this.btnCancel);
            this.splitSolutions.Panel2.Controls.Add(this.btnOk);
            this.splitSolutions.Size = new System.Drawing.Size(527, 300);
            this.splitSolutions.SplitterDistance = 250;
            this.splitSolutions.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(279, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(153, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // SolutionPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(527, 300);
            this.Controls.Add(this.splitSolutions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SolutionPicker";
            this.Text = "SolutionPicker";
            this.Load += new System.EventHandler(this.SolutionPicker_Load);
            this.splitSolutions.Panel1.ResumeLayout(false);
            this.splitSolutions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSolutions)).EndInit();
            this.splitSolutions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listSolutions;
        private System.Windows.Forms.SplitContainer splitSolutions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ColumnHeader colFriendlyName;
        private System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ColumnHeader colPublisher;
    }
}