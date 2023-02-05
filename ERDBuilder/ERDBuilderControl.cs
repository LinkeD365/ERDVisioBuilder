using LinkeD365.ERDBuilder.Forms;
using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Organization;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using VisioAutomation.VDX.Elements;
using VisioAutomation.VDX.Enums;
using VisioAutomation.VDX.Sections;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using VDX = VisioAutomation.VDX;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl : PluginControlBase, IGitHubPlugin, IPayPalPlugin
    {
        #region XrmToolBox Config
        public string DonationDescription => "ERD to Visio Fans";

        public string EmailAccount => "carl.cookson@gmail.com";
        public string RepositoryName => "ERDVisioBuilder";
        private AppInsights ai;
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";

        private const string aiKey = "cc383234-dfdb-429a-a970-d17847361df3";
        public string UserName => "LinkeD365";
        #endregion

        #region properties
        private static double pageWidth = 11;
        private static double pageHeight = 8;
        private Page page;
        private static VDX.Template template = new VDX.Template();
        private Drawing doc;
        private Face face;
        private double xMultiplier = 1.4;
        private double yMultiplier = 1;

        //private List<ListViewItem> allEntities = new List<ListViewItem>();
        private AllSettings mySettings;
        private bool overrideSave = false;
        private ColumnHeader sortingColumn = null;
        #endregion properties

        #region XrmToolBox Control Events
        public ERDBuilderControl()
        {
            InitializeComponent();
            ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly());
            ai.WriteEvent("Control Loaded");
        }

        /// <summary>
        /// Load settings if available
        /// #7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ERDBuilderControl_Load(object sender, EventArgs e)
        {
            ExecuteMethod(Helper.CreateConn, Service);

            try
            {
                if (SettingsManager.Instance.TryLoad(GetType(), out Settings oldSetting))
                {
                    LogWarning("Old Settings found, converting");
                    oldSetting.Name = "My First Setting";
                    mySettings = new AllSettings();
                    mySettings.Settings.Add(oldSetting);

                    SettingsManager.Instance.Save(typeof(AllSettings), mySettings);
                    AddSavedConfigs();
                }
                else if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
                {
                    mySettings = new AllSettings();

                    LogWarning("Settings not found => a new settings file has been created!");
                }
                else
                {
                    LogInfo("Settings found and loaded");

                    AddSavedConfigs();
                    // numLevel.Value = mySettings.Level;
                    ExecuteMethod(AddEntities, new Settings());// AddEntities(true);
                                                               //foreach (var relationship in mySettings.RelationshipMaps)
                                                               //{
                                                               //    checkRelationships.SetItemChecked(relationship, true);
                                                               //}

                }
            }
            catch (Exception)
            {
                if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
                {
                    mySettings = new AllSettings();

                    LogWarning("Settings not found => a new settings file has been created!");
                }
                else
                {
                    LogInfo("Settings found and loaded");

                    AddSavedConfigs();

                    // numLevel.Value = mySettings.Level;
                    ExecuteMethod(AddEntities, new Settings());// AddEntities(true);
                    //foreach (var relationship in mySettings.RelationshipMaps)
                    //{
                    //    checkRelationships.SetItemChecked(relationship, true);
                    //}
                }
            }
            InitSelectedGrid(new SBList<Table>());
            InitContainerGrid(Helper.Containers);
            SetDefaultOptions();
        }

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            ExecuteMethod(Helper.CreateConn, newService);
            ExecuteMethod(AddEntities, new Settings());
        }
        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        #endregion

        #region Buttons Search

        private void btnFromSolution_Click(object sender, EventArgs args)
        {
            SolutionPicker solPicker = new SolutionPicker(Service);
            if (solPicker.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Entities",
                Work = (wrk, e) =>
                {
                    var entities = Helper.GetSolutionEntities(solPicker.SelectedSolutions.Select(sol => sol.Id).ToArray());
                    e.Result = BuildEntityItems(entities);
                },
                PostWorkCallBack = e =>
                {


                    Helper.AllTables = (SBList<Table>)e.Result;
                    InitTableGrid(Helper.AllTables);
                    ExecuteMethod(GetAttributes, (Table)gvTables.SelectedRows[0].DataBoundItem);

                    //listEntities.Items.Clear();
                    //listEntities.Items.AddRange((ListViewItem[])e.Result);
                    //listEntities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            });
        }

        private void btnAllEntities_Click(object sender, EventArgs args)
        {

            ExecuteMethod(AddEntities, new Settings());// AddEntities(false);
        }
        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            gvTables.DataSource = null;
            if (string.IsNullOrEmpty(textSearch.Text))
            {
                gvTables.DataSource = Helper.AllTables;
            }
            else
            {

                gvTables.DataSource = new SBList<Table>(Helper.AllTables.Where(table =>
                                       table.DisplayName.ToLower().Contains(textSearch.Text.ToLower())
                                       || table.Logical.ToLower().Contains(textSearch.Text.ToLower())));
            }
            InitTableGrid();

        }

        private void textSearchCol_TextChanged(object sender, EventArgs e)
        {
            gvAttributes.DataSource = null;
            var selectedColumns = ((Table)gvTables.SelectedRows[0].DataBoundItem).Columns;
            if (string.IsNullOrEmpty(textSearchCol.Text))
            {
                gvAttributes.DataSource = selectedColumns;
            }
            else
            {

                gvAttributes.DataSource = new SBList<Column>(selectedColumns.Where(col =>
                                       col.DisplayName.ToLower().Contains(textSearchCol.Text.ToLower())
                                       || col.LogicalName.ToLower().Contains(textSearchCol.Text.ToLower())));
            }
            InitColGrid();

        }



        

        /// <summary>
        /// Generate Visio
        /// 7-7-20 Added primary key
        /// 12-7-20 Added Many to Many
        /// 20-8-20 Added check to see if file exists #8
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateVisio_Click(object sender, EventArgs e)
        {
            GenerateVisio(false);
            var selectedTables = (SBList<Table>)gvSelected.DataSource;
            //if (!selectedTables.Any())
            //{
            //    MessageBox.Show("Select one or more Tables before creating a Visio", "Select a Table", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    return;
            //}
            //string fileName = saveVisio(cboSelectSaved.SelectedItem?.ToString() ?? string.Empty);

            //if (fileName == null) return;

            //overrideSave = false;
            //try
            //{
            //    Cursor.Current = Cursors.WaitCursor;

            //    doc = new Drawing(template);
            //    page = new Page(pageWidth, pageHeight);
            //    face = doc.AddFace("Segoe UI");
            //    doc.Pages.Add(page);
            //    addedEntities.Clear();
            //    addedMtoM.Clear();

               
            //    AddFullEntity(selectedTables);

            //    foreach (var table in selectedTables)
            //    {
            //        if (addedEntities.Count == 0)
            //        {
            //            AddEntity(table, false, pageWidth / 2, pageHeight / 2);
            //        }
            //        else
            //        {
            //            AddEntity(table, false, nextX, nextY);
            //        }
            //    }

            //    tspProgress.Visible = true;
            //    tspProgress.Maximum = 100;
            //    tspProgress.Step = 1;
            //    tspProgress.Value = 25;
            //    if (checkRelationships.CheckedItems.Contains("Only Between Selected Tables"))
            //    {
            //        AddOnlySelectedRelationships(selectedTables);
            //        tspProgress.Value = 75;
            //    }
            //    else
            //    {
            //        foreach (var table in selectedTables)
            //        {
            //            var entityMeta = table.Entity;
            //            var primeEntity = addedEntities.First(pe => pe.LogicalName == entityMeta.LogicalName);

            //            if (checkRelationships.CheckedItems.Contains("One-To-Many"))
            //            {
            //                AddAllOneToMany(primeEntity, entityMeta, numLevel.Value);
            //            }

            //            tspProgress.Value = 25;
            //            if (checkRelationships.CheckedItems.Contains("Many-To-One"))
            //            {
            //                AddAllManyToOne(primeEntity, entityMeta, numLevel.Value);
            //            }

            //            tspProgress.Value = 50;
            //            if (checkRelationships.CheckedItems.Contains("Many-To-Many"))
            //            {
            //                AddAllManyToMany(primeEntity, entityMeta, numLevel.Value);
            //            }

            //            tspProgress.Value = 75;
            //        }
            //    }

            //    doc.Save(fileName);
            //    tspProgress.Visible = false;
            //    ai.WriteEvent("Visio Entities Count", addedEntities.Count);
            //    if (MessageBox.Show("Visio File created successfully. Do you want to open the Visio Diagram?", "Success!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        Process.Start(fileName);
            //    }
            //}
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //}
        }




        #endregion



        private void checkRelationships_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 3 && e.NewValue == CheckState.Checked)
            {
                checkRelationships.SetItemChecked(0, false);
                checkRelationships.SetItemChecked(1, false);
                checkRelationships.SetItemChecked(2, false);
            }
            else if (e.Index != 3 && e.NewValue == CheckState.Checked)
            {
                checkRelationships.SetItemChecked(3, false);
            }
        }




        /// <summary>
        /// Checks all the items in the Entity list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            ((SBList<Table>)gvTables.DataSource).ToList().ForEach(table => table.Selected = checkAll.Checked);

        }


        #region Table Grid View events
        private void gvTables_SelectionChanged(object sender, EventArgs e)
        {
            textSearchCol.Text = string.Empty;
            if (gvTables.SelectedRows.Count != 1)
            {
                gvAttributes.DataSource = null;
                return;
            }

            ExecuteMethod(GetAttributes, (Table)gvTables.SelectedRows[0].DataBoundItem);
            //Table selected = gvTables.SelectedRows[0].DataBoundItem as Table;
        }

        private void gvTables_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0) return;

            var table = (Table)gvTables.Rows[e.RowIndex].DataBoundItem;
            SBList<Table> selectedTables = (SBList<Table>)gvSelected.DataSource;
            var selectedTable = selectedTables.FirstOrDefault(tab => tab.Logical == table.Logical);
            if (table.Selected)
            {
                if (selectedTable == null) selectedTables.Add(selectedTable = new Table(table));
                selectedTable.Selected = true;

            }
            else
            {
                if (selectedTable != null) selectedTables.Remove(selectedTable);
            }
            InitSelectedGrid(selectedTables);
            //gvSelected.DataSource = null;
            //gvSelected.DataSource = new SortableBindingList<Table>(selectedTables);
        }

        private void gvTables_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gvTables.IsCurrentCellDirty) gvTables.CommitEdit(DataGridViewDataErrorContexts.Commit);

        }


        private void gvAttributes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gvAttributes.IsCurrentCellDirty) gvAttributes.CommitEdit(DataGridViewDataErrorContexts.Commit);

        }

        private void gvAttributes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0) return;

            var column = (Column)gvAttributes.Rows[e.RowIndex].DataBoundItem;
            var table = ((SBList<Table>)gvTables.DataSource).First(tab => tab.Logical == column.Attribute.EntityLogicalName);
            if (column.Selected)
            {
                if (!table.Selected) table.Selected = true;
            }
        }

        #endregion

        private void chkAllColumns_CheckedChanged(object sender, EventArgs e)
        {
            ((SBList<Column>)gvAttributes.DataSource).ToList().ForEach(column => column.Selected = chkAllColumns.Checked);
        }

        private void btnNewVisio_Click(object sender, EventArgs e)
        {
            GenerateVisio(true);
        }

        public void ShowSettings()
        {
            SettingsDialog  settingsDialog = new SettingsDialog();
            settingsDialog.propGrid.SelectedObject = this.mySettings.VisioDisplayConfig;
            settingsDialog.ShowDialog();

            if (settingsDialog.DialogResult == DialogResult.OK)
            {
                mySettings.VisioDisplayConfig = (VisioDisplayConfig) settingsDialog.propGrid.SelectedObject;
                
            }
            
        }

        private void gvSelected_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0) return;
            var selCol = (Table)gvSelected.Rows[e.RowIndex].DataBoundItem;
            var table = ((SBList<Table>)gvTables.DataSource).First(tab => tab.Logical == selCol.Logical);
            table.Selected = selCol.Selected;
        }

        private void gvSelected_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gvSelected.IsCurrentCellDirty) gvSelected.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void gvContainers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            AddContainerList((DataGridViewComboBoxColumn) gvSelected.Columns["containerCol"]);
        }
    }
}
