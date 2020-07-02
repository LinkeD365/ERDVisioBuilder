namespace LinkeD365.ERDBuilder
{
    partial class ERDBuilderControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ERDBuilderControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateVisio = new System.Windows.Forms.ToolStripButton();
            this.btnHideSystem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGrpEntities = new System.Windows.Forms.ToolStripDropDownButton();
            this.fromSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allEntitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.listEntities = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEntity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCustom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSelected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitRight = new System.Windows.Forms.SplitContainer();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.checkRelationships = new System.Windows.Forms.CheckedListBox();
            this.lblLevels = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.grpSelected = new System.Windows.Forms.GroupBox();
            this.listSelected = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.tspProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.grpSelected.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.btnGrpEntities,
            this.btnCreateVisio,
            this.btnHideSystem,
            this.toolStripSeparator1,
            this.tspProgress});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(854, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCreateVisio
            // 
            this.btnCreateVisio.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCreateVisio.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateVisio.Image")));
            this.btnCreateVisio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateVisio.Name = "btnCreateVisio";
            this.btnCreateVisio.Size = new System.Drawing.Size(73, 22);
            this.btnCreateVisio.Text = "Create Visio";
            this.btnCreateVisio.Click += new System.EventHandler(this.btnGenerateVisio_Click);
            // 
            // btnHideSystem
            // 
            this.btnHideSystem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnHideSystem.Checked = true;
            this.btnHideSystem.CheckOnClick = true;
            this.btnHideSystem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnHideSystem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHideSystem.Image = ((System.Drawing.Image)(resources.GetObject("btnHideSystem.Image")));
            this.btnHideSystem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHideSystem.Name = "btnHideSystem";
            this.btnHideSystem.Size = new System.Drawing.Size(77, 22);
            this.btnHideSystem.Text = "Hide System";
            this.btnHideSystem.ToolTipText = "Hide the standard relationships such as Created By, Modified By";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGrpEntities
            // 
            this.btnGrpEntities.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGrpEntities.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromSolutionToolStripMenuItem,
            this.allEntitiesToolStripMenuItem});
            this.btnGrpEntities.Image = ((System.Drawing.Image)(resources.GetObject("btnGrpEntities.Image")));
            this.btnGrpEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGrpEntities.Name = "btnGrpEntities";
            this.btnGrpEntities.Size = new System.Drawing.Size(79, 22);
            this.btnGrpEntities.Text = "List Entities";
            // 
            // fromSolutionToolStripMenuItem
            // 
            this.fromSolutionToolStripMenuItem.Name = "fromSolutionToolStripMenuItem";
            this.fromSolutionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fromSolutionToolStripMenuItem.Text = "From Solution";
            this.fromSolutionToolStripMenuItem.Click += new System.EventHandler(this.fromSolutionToolStripMenuItem_Click);
            // 
            // allEntitiesToolStripMenuItem
            // 
            this.allEntitiesToolStripMenuItem.Name = "allEntitiesToolStripMenuItem";
            this.allEntitiesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allEntitiesToolStripMenuItem.Text = "All Entities";
            this.allEntitiesToolStripMenuItem.Click += new System.EventHandler(this.allEntitiesToolStripMenuItem_Click);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 25);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.listEntities);
            this.splitMain.Panel1MinSize = 350;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitRight);
            this.splitMain.Size = new System.Drawing.Size(854, 611);
            this.splitMain.SplitterDistance = 544;
            this.splitMain.TabIndex = 5;
            // 
            // listEntities
            // 
            this.listEntities.CheckBoxes = true;
            this.listEntities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colEntity,
            this.colCustom,
            this.colSelected});
            this.listEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEntities.HideSelection = false;
            this.listEntities.Location = new System.Drawing.Point(0, 0);
            this.listEntities.Name = "listEntities";
            this.listEntities.Size = new System.Drawing.Size(544, 611);
            this.listEntities.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listEntities.TabIndex = 0;
            this.listEntities.UseCompatibleStateImageBehavior = false;
            this.listEntities.View = System.Windows.Forms.View.Details;
            this.listEntities.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listEntities_ColumnClick);
            this.listEntities.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listEntities_ItemChecked);
            this.listEntities.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listEntities_ItemSelectionChanged);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 25;
            // 
            // colEntity
            // 
            this.colEntity.Text = "Physical";
            // 
            // colCustom
            // 
            this.colCustom.Text = "Custom?";
            // 
            // colSelected
            // 
            this.colSelected.Text = "Relationships";
            // 
            // splitRight
            // 
            this.splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRight.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitRight.Location = new System.Drawing.Point(0, 0);
            this.splitRight.Name = "splitRight";
            this.splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitRight.Panel1
            // 
            this.splitRight.Panel1.Controls.Add(this.grpSettings);
            // 
            // splitRight.Panel2
            // 
            this.splitRight.Panel2.Controls.Add(this.grpSelected);
            this.splitRight.Size = new System.Drawing.Size(306, 611);
            this.splitRight.SplitterDistance = 193;
            this.splitRight.TabIndex = 4;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.checkRelationships);
            this.grpSettings.Controls.Add(this.lblLevels);
            this.grpSettings.Controls.Add(this.numericUpDown1);
            this.grpSettings.Controls.Add(this.btnFile);
            this.grpSettings.Controls.Add(this.txtFileName);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSettings.Location = new System.Drawing.Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(306, 193);
            this.grpSettings.TabIndex = 0;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // checkRelationships
            // 
            this.checkRelationships.CheckOnClick = true;
            this.checkRelationships.FormattingEnabled = true;
            this.checkRelationships.Items.AddRange(new object[] {
            "One-To-Many",
            "Many-To-One",
            "Only Between Selected Entities"});
            this.checkRelationships.Location = new System.Drawing.Point(7, 71);
            this.checkRelationships.Name = "checkRelationships";
            this.checkRelationships.Size = new System.Drawing.Size(185, 64);
            this.checkRelationships.TabIndex = 1;
            this.checkRelationships.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkRelationships_ItemCheck);
            // 
            // lblLevels
            // 
            this.lblLevels.AutoSize = true;
            this.lblLevels.Location = new System.Drawing.Point(17, 51);
            this.lblLevels.Name = "lblLevels";
            this.lblLevels.Size = new System.Drawing.Size(41, 13);
            this.lblLevels.TabIndex = 3;
            this.lblLevels.Text = "Levels:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(66, 45);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(7, 16);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(49, 23);
            this.btnFile.TabIndex = 1;
            this.btnFile.Text = "File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(66, 18);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(234, 20);
            this.txtFileName.TabIndex = 0;
            // 
            // grpSelected
            // 
            this.grpSelected.Controls.Add(this.listSelected);
            this.grpSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSelected.Location = new System.Drawing.Point(0, 0);
            this.grpSelected.Name = "grpSelected";
            this.grpSelected.Size = new System.Drawing.Size(306, 414);
            this.grpSelected.TabIndex = 3;
            this.grpSelected.TabStop = false;
            this.grpSelected.Text = "Selected Entities";
            // 
            // listSelected
            // 
            this.listSelected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSelected.HideSelection = false;
            this.listSelected.Location = new System.Drawing.Point(3, 16);
            this.listSelected.Name = "listSelected";
            this.listSelected.Size = new System.Drawing.Size(300, 395);
            this.listSelected.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listSelected.TabIndex = 3;
            this.listSelected.UseCompatibleStateImageBehavior = false;
            this.listSelected.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Physical";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Custom?";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Relationships";
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "vdx";
            this.saveDialog.Filter = "VDX files|*.vdx";
            // 
            // tspProgress
            // 
            this.tspProgress.Name = "tspProgress";
            this.tspProgress.Size = new System.Drawing.Size(100, 22);
            this.tspProgress.Visible = false;
            // 
            // ERDBuilderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "ERDBuilderControl";
            this.Size = new System.Drawing.Size(854, 636);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.grpSelected.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ToolStripButton btnCreateVisio;
        private System.Windows.Forms.ListView listEntities;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colEntity;
        private System.Windows.Forms.ColumnHeader colCustom;
        private System.Windows.Forms.ColumnHeader colSelected;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.SplitContainer splitRight;
        private System.Windows.Forms.GroupBox grpSelected;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Label lblLevels;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ListView listSelected;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.CheckedListBox checkRelationships;
        private System.Windows.Forms.ToolStripButton btnHideSystem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton btnGrpEntities;
        private System.Windows.Forms.ToolStripMenuItem fromSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allEntitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar tspProgress;
    }
}
