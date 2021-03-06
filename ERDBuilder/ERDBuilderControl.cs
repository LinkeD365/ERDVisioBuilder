﻿using McTools.Xrm.Connection;
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
using System.Data;
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

        public string DonationDescription => "ERD to Visio Fans";

        public string EmailAccount => "carl.cookson@gmail.com";
        public string RepositoryName => "ERDVisioBuilder";
        private AppInsights ai;
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";

        private const string aiKey = "cc383234-dfdb-429a-a970-d17847361df3";
        public string UserName => "LinkeD365";

        private static double pageWidth = 11;
        private static double pageHeight = 8;
        private Page page;
        private static VDX.Template template = new VDX.Template();
        private Drawing doc;
        private Face face;
        private double xMultiplier = 1.4;
        private double yMultiplier = 1;

        private List<ListViewItem> allEntities = new List<ListViewItem>();
        private AllSettings mySettings;
        private bool overrideSave = false;

        public ERDBuilderControl()
        {
            InitializeComponent();
            ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly());
            ai.WriteEvent("Control Loaded");
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            // ExecuteMethod(AddEntities,false);
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
            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                MessageBox.Show("Please select a file name prior to generating a Visio", "Select File", MessageBoxButtons.OK);
                return;
            }

            if (File.Exists(txtFileName.Text) && !overrideSave)
                if (MessageBox.Show("Do you want to override the file?", "File already exists", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            overrideSave = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                doc = new Drawing(template);
                page = new Page(pageWidth, pageHeight);
                face = doc.AddFace("Segoe UI");
                doc.Pages.Add(page);
                addedEntities.Clear();

                foreach (var entityMeta in from ListViewItem coreEntity in listSelected.Items
                                           let entityMeta = Service.GetEntityMetadata(coreEntity.SubItems[1].Text)
                                           select entityMeta)
                    if (addedEntities.Count == 0) AddEntity(entityMeta, false, pageWidth / 2, pageHeight / 2);
                    else AddEntity(entityMeta, false, nextX, nextY);
                tspProgress.Visible = true;
                tspProgress.Maximum = 100;
                tspProgress.Step = 1;
                tspProgress.Value = 25;
                if (checkRelationships.CheckedItems.Contains("Only Between Selected Entities"))
                {
                    AddOnlySelectedRelationships();
                    tspProgress.Value = 75;
                }
                else foreach (ListViewItem selectedEntity in listSelected.Items)
                    {
                        var entityMeta = Service.GetEntityMetadata(selectedEntity.SubItems[1].Text);
                        var primeEntity = addedEntities.First(pe => pe.LogicalName == entityMeta.LogicalName);

                        if (checkRelationships.CheckedItems.Contains("One-To-Many")) AddAllOneToMany(primeEntity, entityMeta, numLevel.Value);
                        tspProgress.Value = 25;
                        if (checkRelationships.CheckedItems.Contains("Many-To-One")) AddAllManyToOne(primeEntity, entityMeta, numLevel.Value);
                        tspProgress.Value = 50;
                        if (checkRelationships.CheckedItems.Contains("Many-To-Many")) AddAllManyToMany(primeEntity, entityMeta, numLevel.Value);
                        tspProgress.Value = 75;
                    }

                doc.Save(txtFileName.Text);
                tspProgress.Visible = false;
                ai.WriteEvent("Visio Entities Count", addedEntities.Count);
                MessageBox.Show("Visio File created successfully", "Success!", MessageBoxButtons.OK);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private ColumnHeader sortingColumn = null;

        private void listEntities_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Get the new sorting column.
            ColumnHeader new_sorting_column = listEntities.Columns[e.Column];

            // Figure out the new sorting order.
            SortOrder sort_order;
            if (sortingColumn == null)
                // New column. Sort ascending.
                sort_order = SortOrder.Ascending;
            else
            {
                // See if this is the same column.
                if (new_sorting_column == sortingColumn)
                    // Same column. Switch the sort order.
                    if (sortingColumn.Text.StartsWith("> "))
                        sort_order = SortOrder.Descending;
                    else
                        sort_order = SortOrder.Ascending;
                else
                    // New column. Sort ascending.
                    sort_order = SortOrder.Ascending;

                // Remove the old sort indicator.
                sortingColumn.Text = sortingColumn.Text.Substring(2);
            }

            // Display the new sort order.
            sortingColumn = new_sorting_column;
            if (sort_order == SortOrder.Ascending)
                sortingColumn.Text = "> " + sortingColumn.Text;
            else
                sortingColumn.Text = "< " + sortingColumn.Text;

            // Create a comparer.
            listEntities.ListViewItemSorter =
                new ListViewComparer(e.Column, sort_order);

            // Sort.
            listEntities.Sort();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK) txtFileName.Text = saveDialog.FileName;
            overrideSave = true;
        }

        private void listEntities_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
        }

        private void listEntities_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListViewItem lvSelectedItem = (ListViewItem)e.Item.Clone();
            lvSelectedItem.Name = "sel_" + e.Item.Name;
            if (e.Item.Checked) listSelected.Items.Add(lvSelectedItem);
            else listSelected.Items.RemoveByKey("sel_" + e.Item.Name);
            listSelected.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void checkRelationships_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 3 && e.NewValue == CheckState.Checked)
            {
                checkRelationships.SetItemChecked(0, false);
                checkRelationships.SetItemChecked(1, false);
                checkRelationships.SetItemChecked(2, false);
            }
            else if (e.Index != 3 && e.NewValue == CheckState.Checked) checkRelationships.SetItemChecked(3, false);
        }

        /// <summary>
        /// Create a list of all the entities
        /// 7-7-20 - Modified to return list to allow search
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private ListViewItem[] BuildEntityItems(List<EntityMetadata> entities)
        {
            if (entities.Count == 0) return new ListViewItem[] { };

            var entList = new List<ListViewItem>();

            foreach (var entity in entities)
            {
                var lvItem = new ListViewItem(
                        new string[] {
                                entity.DisplayName.UserLocalizedLabel == null ? entity.EntitySetName : entity.DisplayName.UserLocalizedLabel.Label.ToString(),
                                entity.LogicalName,
                                entity.IsCustomEntity.ToString()
                        }
                    );
                lvItem.Name = entity.EntitySetName;

                entList.Add(lvItem);
            }
            return entList.ToArray();
        }

        private List<EntityMetadata> GetSolutionEntities(Guid[] guids)
        {
            var qry = new QueryExpression("solutioncomponent")
            {
                ColumnSet = new ColumnSet("objectid"),
                NoLock = true,
                Criteria = new FilterExpression
                {
                    Conditions =
                            {
                                new ConditionExpression("solutionid",ConditionOperator.In,
                                                    guids),
                                new ConditionExpression("componenttype", ConditionOperator.Equal, 1)
                            }
                }
            };

            var results = Service.RetrieveMultiple(qry).Entities;
            var entList = results.Select(r => r.GetAttributeValue<Guid>("objectid")).ToList();

            if (entList.Count > 0)
            {
                var eq = new EntityQueryExpression
                {
                    Criteria = new MetadataFilterExpression(LogicalOperator.Or),
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = true
                    },
                    AttributeQuery = new AttributeQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                                {
                                    new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                                }
                        }
                    },
                    KeyQuery = new EntityKeyQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                                {
                                    new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, "filterout"),
                                }
                        }
                    },
                    RelationshipQuery = new RelationshipQueryExpression
                    {
                        Criteria = new MetadataFilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                                {
                                    new MetadataConditionExpression("SchemaName", MetadataConditionOperator.Equals, "filterout"),
                                }
                        }
                    }
                };

                entList.ForEach(id => eq.Criteria.Conditions.Add(
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.Equals, id)));
                var allEntQry = new RetrieveMetadataChangesRequest
                {
                    Query = eq,
                    ClientVersionStamp = null
                };

                return ((RetrieveMetadataChangesResponse)Service.Execute(allEntQry)).EntityMetadata.ToList();
            }
            return new List<EntityMetadata>();
        }

        /// <summary>
        /// Checks all the items in the Entity list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem entity in listEntities.Items)
                entity.Checked = checkAll.Checked;
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            listEntities.Items.Clear();
            if (string.IsNullOrEmpty(textSearch.Text))
                listEntities.Items.AddRange(allEntities.ToArray());
            else
                listEntities.Items.AddRange(allEntities.Where(lvi => lvi.SubItems[0].Text.ToLower().Contains(textSearch.Text.ToLower())).ToArray());
        }

        private void btnFromSolution_Click(object sender, EventArgs args)
        {
            SolutionPicker solPicker = new SolutionPicker(Service);
            if (solPicker.ShowDialog() != DialogResult.OK) return;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Entities",
                Work = (wrk, e) =>
                {
                    var entities = GetSolutionEntities(solPicker.SelectedSolutions.Select(sol => sol.Id).ToArray());
                    var lvi = BuildEntityItems(entities);
                    allEntities = lvi.ToList();
                    e.Result = lvi;
                },
                PostWorkCallBack = e =>
                {
                    listEntities.Items.Clear();
                    listEntities.Items.AddRange((ListViewItem[])e.Result);
                    listEntities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            });
        }

        private void btnAllEntities_Click(object sender, EventArgs args)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            ExecuteMethod(AddEntities, new Settings());// AddEntities(false);
        }

        /// <summary>
        /// Populate all entities
        /// 20-08-20 #7 Added Saved Entities population
        /// </summary>
        /// <param name="populateSaved"></param>
        /// <param name="setting"></param>
        private void AddEntities(Settings setting)
        {
            listEntities.BeginUpdate();
            listEntities.Items.Clear();
            listSelected.Items.Clear();
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Entities",
                Work = (wrk, e) =>
                {
                    var query = new RetrieveAllEntitiesRequest();
                    var entities = ((RetrieveAllEntitiesResponse)Service.Execute(query)).EntityMetadata.ToList();
                    var lvi = BuildEntityItems(entities);
                    allEntities = lvi.ToList();
                    e.Result = lvi;

                    //wrk.ReportProgress(50, "Populating List");
                },
                PostWorkCallBack = e =>
                {
                    listEntities.Items.AddRange((ListViewItem[])e.Result);

                    listEntities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    if (setting.Name != null)
                        AddConfig(setting);

                },
                ProgressChanged = e => { }
            });
            listEntities.EndUpdate();
            Cursor.Current = Cursors.Default;
        }



        /// <summary>
        /// Load settings if available
        /// #7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ERDBuilderControl_Load(object sender, EventArgs e)
        {
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

        }

    }

    public static class Helpers
    {
        public static Face GetFontSafe(VDX.FaceList faces, string name)
        {
            if (faces.ContainsName(name))
                return faces[name];

            int max_id = faces.Items.Select(f => f.ID).Max();
            int new_id = max_id + 1;

            var face = new Face(new_id, name);

            faces.Add(face);

            return face;
        }
    }
}
