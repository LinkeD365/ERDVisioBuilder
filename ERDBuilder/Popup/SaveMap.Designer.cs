namespace LinkeD365.ERDBuilder
{
    partial class SaveMap
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtSaveName = new System.Windows.Forms.TextBox();
            this.lblUpdateExisting = new System.Windows.Forms.Label();
            this.cboExisting = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblOr = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(87, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "New Map Name:";
            // 
            // txtSaveName
            // 
            this.txtSaveName.Location = new System.Drawing.Point(105, 6);
            this.txtSaveName.Name = "txtSaveName";
            this.txtSaveName.Size = new System.Drawing.Size(198, 20);
            this.txtSaveName.TabIndex = 1;
            // 
            // lblUpdateExisting
            // 
            this.lblUpdateExisting.AutoSize = true;
            this.lblUpdateExisting.Location = new System.Drawing.Point(12, 50);
            this.lblUpdateExisting.Name = "lblUpdateExisting";
            this.lblUpdateExisting.Size = new System.Drawing.Size(84, 13);
            this.lblUpdateExisting.TabIndex = 2;
            this.lblUpdateExisting.Text = "Update Existing:";
            // 
            // cboExisting
            // 
            this.cboExisting.FormattingEnabled = true;
            this.cboExisting.Location = new System.Drawing.Point(105, 47);
            this.cboExisting.Name = "cboExisting";
            this.cboExisting.Size = new System.Drawing.Size(198, 21);
            this.cboExisting.TabIndex = 3;
            this.cboExisting.SelectedValueChanged += new System.EventHandler(this.cboExisting_SelectedValueChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(115, 74);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblOr
            // 
            this.lblOr.AutoSize = true;
            this.lblOr.Location = new System.Drawing.Point(95, 31);
            this.lblOr.Name = "lblOr";
            this.lblOr.Size = new System.Drawing.Size(74, 13);
            this.lblOr.TabIndex = 5;
            this.lblOr.Text = "------- OR --------";
            // 
            // SaveMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 107);
            this.Controls.Add(this.lblOr);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboExisting);
            this.Controls.Add(this.lblUpdateExisting);
            this.Controls.Add(this.txtSaveName);
            this.Controls.Add(this.lblName);
            this.Name = "SaveMap";
            this.Text = "SaveMap";
            this.Load += new System.EventHandler(this.SaveMap_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        public System.Windows.Forms.TextBox txtSaveName;
        private System.Windows.Forms.Label lblUpdateExisting;
        private System.Windows.Forms.ComboBox cboExisting;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblOr;
    }
}