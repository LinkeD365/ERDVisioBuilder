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
            this.btnAllEntities = new System.Windows.Forms.ToolStripButton();
            this.btnFromSolution = new System.Windows.Forms.ToolStripButton();
            this.cboSelectSaved = new System.Windows.Forms.ToolStripComboBox();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateVisio = new System.Windows.Forms.ToolStripButton();
            this.tspProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitEntitiesList = new System.Windows.Forms.SplitContainer();
            this.splitSearch = new System.Windows.Forms.SplitContainer();
            this.lblSearch = new System.Windows.Forms.Label();
            this.textSearch = new System.Windows.Forms.TextBox();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.listEntities = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEntity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCustom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitRight = new System.Windows.Forms.SplitContainer();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.chkListHide = new System.Windows.Forms.CheckedListBox();
            this.chkListDisplay = new System.Windows.Forms.CheckedListBox();
            this.checkRelationships = new System.Windows.Forms.CheckedListBox();
            this.lblLevels = new System.Windows.Forms.Label();
            this.numLevel = new System.Windows.Forms.NumericUpDown();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.grpSelected = new System.Windows.Forms.GroupBox();
            this.listSelected = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportFile = new System.Windows.Forms.SaveFileDialog();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitEntitiesList)).BeginInit();
            this.splitEntitiesList.Panel1.SuspendLayout();
            this.splitEntitiesList.Panel2.SuspendLayout();
            this.splitEntitiesList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSearch)).BeginInit();
            this.splitSearch.Panel1.SuspendLayout();
            this.splitSearch.Panel2.SuspendLayout();
            this.splitSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).BeginInit();
            this.splitRight.Panel1.SuspendLayout();
            this.splitRight.Panel2.SuspendLayout();
            this.splitRight.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLevel)).BeginInit();
            this.grpSelected.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.btnAllEntities,
            this.btnFromSolution,
            this.cboSelectSaved,
            this.btnSave,
            this.btnExport,
            this.btnImport,
            this.toolStripSeparator2,
            this.btnCreateVisio,
            this.tspProgress});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(854, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(64, 28);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // btnAllEntities
            // 
            this.btnAllEntities.Image = ((System.Drawing.Image)(resources.GetObject("btnAllEntities.Image")));
            this.btnAllEntities.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAllEntities.Name = "btnAllEntities";
            this.btnAllEntities.Size = new System.Drawing.Size(90, 28);
            this.btnAllEntities.Text = "All Entities";
            this.btnAllEntities.Click += new System.EventHandler(this.btnAllEntities_Click);
            // 
            // btnFromSolution
            // 
            this.btnFromSolution.Image = ((System.Drawing.Image)(resources.GetObject("btnFromSolution.Image")));
            this.btnFromSolution.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFromSolution.Name = "btnFromSolution";
            this.btnFromSolution.Size = new System.Drawing.Size(115, 28);
            this.btnFromSolution.Text = "From Solutions";
            this.btnFromSolution.Click += new System.EventHandler(this.btnFromSolution_Click);
            // 
            // cboSelectSaved
            // 
            this.cboSelectSaved.Name = "cboSelectSaved";
            this.cboSelectSaved.Size = new System.Drawing.Size(121, 31);
            this.cboSelectSaved.Text = "Select Saved Config";
            this.cboSelectSaved.SelectedIndexChanged += new System.EventHandler(this.cboSelectSaved_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 28);
            this.btnSave.Text = "Save Config";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(69, 28);
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(71, 28);
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // btnCreateVisio
            // 
            this.btnCreateVisio.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateVisio.Image")));
            this.btnCreateVisio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateVisio.Name = "btnCreateVisio";
            this.btnCreateVisio.Size = new System.Drawing.Size(97, 28);
            this.btnCreateVisio.Text = "Create Visio";
            this.btnCreateVisio.Click += new System.EventHandler(this.btnGenerateVisio_Click);
            // 
            // tspProgress
            // 
            this.tspProgress.Name = "tspProgress";
            this.tspProgress.Size = new System.Drawing.Size(100, 28);
            this.tspProgress.Visible = false;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 31);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.splitEntitiesList);
            this.splitMain.Panel1MinSize = 350;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitRight);
            this.splitMain.Size = new System.Drawing.Size(854, 605);
            this.splitMain.SplitterDistance = 548;
            this.splitMain.TabIndex = 5;
            // 
            // splitEntitiesList
            // 
            this.splitEntitiesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitEntitiesList.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitEntitiesList.IsSplitterFixed = true;
            this.splitEntitiesList.Location = new System.Drawing.Point(0, 0);
            this.splitEntitiesList.Name = "splitEntitiesList";
            this.splitEntitiesList.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitEntitiesList.Panel1
            // 
            this.splitEntitiesList.Panel1.Controls.Add(this.splitSearch);
            this.splitEntitiesList.Panel1MinSize = 20;
            // 
            // splitEntitiesList.Panel2
            // 
            this.splitEntitiesList.Panel2.Controls.Add(this.checkAll);
            this.splitEntitiesList.Panel2.Controls.Add(this.listEntities);
            this.splitEntitiesList.Size = new System.Drawing.Size(548, 605);
            this.splitEntitiesList.SplitterDistance = 25;
            this.splitEntitiesList.TabIndex = 2;
            // 
            // splitSearch
            // 
            this.splitSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSearch.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSearch.IsSplitterFixed = true;
            this.splitSearch.Location = new System.Drawing.Point(0, 0);
            this.splitSearch.Name = "splitSearch";
            // 
            // splitSearch.Panel1
            // 
            this.splitSearch.Panel1.Controls.Add(this.lblSearch);
            // 
            // splitSearch.Panel2
            // 
            this.splitSearch.Panel2.Controls.Add(this.textSearch);
            this.splitSearch.Size = new System.Drawing.Size(548, 25);
            this.splitSearch.SplitterDistance = 60;
            this.splitSearch.TabIndex = 1;
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(9, 5);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(41, 13);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Search";
            // 
            // textSearch
            // 
            this.textSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textSearch.Location = new System.Drawing.Point(0, 0);
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(484, 20);
            this.textSearch.TabIndex = 0;
            this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Location = new System.Drawing.Point(6, 4);
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(15, 14);
            this.checkAll.TabIndex = 1;
            this.checkAll.UseVisualStyleBackColor = true;
            this.checkAll.CheckedChanged += new System.EventHandler(this.checkAll_CheckedChanged);
            // 
            // listEntities
            // 
            this.listEntities.CheckBoxes = true;
            this.listEntities.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colEntity,
            this.colCustom});
            this.listEntities.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEntities.HideSelection = false;
            this.listEntities.Location = new System.Drawing.Point(0, 0);
            this.listEntities.Name = "listEntities";
            this.listEntities.Size = new System.Drawing.Size(548, 576);
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
            this.colName.Text = "        Name";
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
            this.splitRight.Size = new System.Drawing.Size(302, 605);
            this.splitRight.SplitterDistance = 221;
            this.splitRight.TabIndex = 4;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.chkListHide);
            this.grpSettings.Controls.Add(this.chkListDisplay);
            this.grpSettings.Controls.Add(this.checkRelationships);
            this.grpSettings.Controls.Add(this.lblLevels);
            this.grpSettings.Controls.Add(this.numLevel);
            this.grpSettings.Controls.Add(this.btnFile);
            this.grpSettings.Controls.Add(this.txtFileName);
            this.grpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSettings.Location = new System.Drawing.Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(302, 221);
            this.grpSettings.TabIndex = 0;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // chkListHide
            // 
            this.chkListHide.CheckOnClick = true;
            this.chkListHide.FormattingEnabled = true;
            this.chkListHide.Items.AddRange(new object[] {
            "Hide System",
            "Hide Activity Entities"});
            this.chkListHide.Location = new System.Drawing.Point(7, 181);
            this.chkListHide.Name = "chkListHide";
            this.chkListHide.Size = new System.Drawing.Size(185, 34);
            this.chkListHide.TabIndex = 5;
            // 
            // chkListDisplay
            // 
            this.chkListDisplay.CheckOnClick = true;
            this.chkListDisplay.FormattingEnabled = true;
            this.chkListDisplay.Items.AddRange(new object[] {
            "Entity Display Names",
            "Attribute Display Names"});
            this.chkListDisplay.Location = new System.Drawing.Point(7, 141);
            this.chkListDisplay.Name = "chkListDisplay";
            this.chkListDisplay.Size = new System.Drawing.Size(185, 34);
            this.chkListDisplay.TabIndex = 4;
            // 
            // checkRelationships
            // 
            this.checkRelationships.CheckOnClick = true;
            this.checkRelationships.FormattingEnabled = true;
            this.checkRelationships.Items.AddRange(new object[] {
            "One-To-Many",
            "Many-To-One",
            "Many-To-Many",
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
            // numLevel
            // 
            this.numLevel.Location = new System.Drawing.Point(66, 45);
            this.numLevel.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLevel.Name = "numLevel";
            this.numLevel.Size = new System.Drawing.Size(120, 20);
            this.numLevel.TabIndex = 2;
            this.numLevel.Value = new decimal(new int[] {
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
            this.grpSelected.Size = new System.Drawing.Size(302, 380);
            this.grpSelected.TabIndex = 3;
            this.grpSelected.TabStop = false;
            this.grpSelected.Text = "Selected Entities";
            // 
            // listSelected
            // 
            this.listSelected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSelected.HideSelection = false;
            this.listSelected.Location = new System.Drawing.Point(3, 16);
            this.listSelected.Name = "listSelected";
            this.listSelected.Size = new System.Drawing.Size(296, 361);
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
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "vdx";
            this.saveDialog.Filter = "VDX files|*.vdx";
            // 
            // exportFile
            // 
            this.exportFile.DefaultExt = "vdx";
            this.exportFile.Filter = "XML files|*.xml";
            // 
            // ERDBuilderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "ERDBuilderControl";
            this.PluginIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PluginIcon")));
            this.Size = new System.Drawing.Size(854, 636);
            this.TabIcon = global::ERDBuilder.Properties.Resources.smallIcon_32;
            this.Load += new System.EventHandler(this.ERDBuilderControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.splitEntitiesList.Panel1.ResumeLayout(false);
            this.splitEntitiesList.Panel2.ResumeLayout(false);
            this.splitEntitiesList.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitEntitiesList)).EndInit();
            this.splitEntitiesList.ResumeLayout(false);
            this.splitSearch.Panel1.ResumeLayout(false);
            this.splitSearch.Panel1.PerformLayout();
            this.splitSearch.Panel2.ResumeLayout(false);
            this.splitSearch.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSearch)).EndInit();
            this.splitSearch.ResumeLayout(false);
            this.splitRight.Panel1.ResumeLayout(false);
            this.splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitRight)).EndInit();
            this.splitRight.ResumeLayout(false);
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLevel)).EndInit();
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
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.SplitContainer splitRight;
        private System.Windows.Forms.GroupBox grpSelected;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Label lblLevels;
        private System.Windows.Forms.NumericUpDown numLevel;
        private System.Windows.Forms.ListView listSelected;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.CheckedListBox checkRelationships;
        private System.Windows.Forms.ToolStripProgressBar tspProgress;
        private System.Windows.Forms.CheckBox checkAll;
        private System.Windows.Forms.SplitContainer splitEntitiesList;
        private System.Windows.Forms.SplitContainer splitSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox textSearch;
        private System.Windows.Forms.ToolStripButton btnAllEntities;
        private System.Windows.Forms.ToolStripButton btnFromSolution;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.CheckedListBox chkListHide;
        private System.Windows.Forms.CheckedListBox chkListDisplay;
        private System.Windows.Forms.ToolStripComboBox cboSelectSaved;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.SaveFileDialog exportFile;
    }
}
