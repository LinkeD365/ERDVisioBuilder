using McTools.Xrm.Connection;
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
using System.Linq;
using System.Windows.Forms;
using VisioAutomation.VDX.Elements;
using VisioAutomation.VDX.Enums;
using VisioAutomation.VDX.Sections;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using VDX = VisioAutomation.VDX;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl : PluginControlBase, IGitHubPlugin
    {
        public string RepositoryName => "ERD Visio Builder";

        public string UserName => "LinkeD365";

        private static double pageWidth = 11;
        private static double pageHeight = 8;
        private Page page;
        private static VDX.Template template = new VDX.Template();
        private Drawing doc;
        private Face face;
        private double xMultiplier = 1.4;
        private double yMultiplier = 1;

        public ERDBuilderControl()
        {
            InitializeComponent();
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
        }

        private void btnGenerateVisio_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFileName.Text))
            {
                MessageBox.Show("Please select a file name prior to generating a Visio", "Select File", MessageBoxButtons.OK);
                return;
            }

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
                {
                    if (addedEntities.Count == 0) AddEntity(entityMeta.LogicalName, pageWidth / 2, pageHeight / 2);
                    else AddEntity(entityMeta.LogicalName, nextX, nextY);
                }
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
                        var primeEntity = addedEntities.Where(pe => pe.Name == entityMeta.LogicalName).First();

                        if (checkRelationships.CheckedItems.Contains("One-To-Many")) AddAllOneToMany(primeEntity, entityMeta, numericUpDown1.Value);
                        tspProgress.Value = 50;
                        if (checkRelationships.CheckedItems.Contains("Many-To-One")) AddAllManyToOne(primeEntity, entityMeta, numericUpDown1.Value);
                        tspProgress.Value = 75;
                    }
                doc.Save(txtFileName.Text);
                tspProgress.Visible = false;
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
            System.Windows.Forms.SortOrder sort_order;
            if (sortingColumn == null)
            {
                // New column. Sort ascending.
                sort_order = SortOrder.Ascending;
            }
            else
            {
                // See if this is the same column.
                if (new_sorting_column == sortingColumn)
                {
                    // Same column. Switch the sort order.
                    if (sortingColumn.Text.StartsWith("> "))
                    {
                        sort_order = SortOrder.Descending;
                    }
                    else
                    {
                        sort_order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending.
                    sort_order = SortOrder.Ascending;
                }

                // Remove the old sort indicator.
                sortingColumn.Text = sortingColumn.Text.Substring(2);
            }

            // Display the new sort order.
            sortingColumn = new_sorting_column;
            if (sort_order == SortOrder.Ascending)
            {
                sortingColumn.Text = "> " + sortingColumn.Text;
            }
            else
            {
                sortingColumn.Text = "< " + sortingColumn.Text;
            }

            // Create a comparer.
            listEntities.ListViewItemSorter =
                new ListViewComparer(e.Column, sort_order);

            // Sort.
            listEntities.Sort();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (saveDialog.ShowDialog() == DialogResult.OK) txtFileName.Text = saveDialog.FileName;
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
        }

        private void checkRelationships_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 2 && e.NewValue == CheckState.Checked)
            {
                checkRelationships.SetItemChecked(0, false);
                checkRelationships.SetItemChecked(1, false);
            }
            else if (e.Index != 3 && e.NewValue == CheckState.Checked) checkRelationships.SetItemChecked(2, false);
        }

        private void fromSolutionToolStripMenuItem_Click(object sender, EventArgs args)
        {
            SolutionPicker solPicker = new SolutionPicker(Service);
            if (solPicker.ShowDialog() != DialogResult.OK) return;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Entities",
                Work = (wrk, e) =>
                {
                    var entities = GetSolutionEntities(solPicker.SelectedSolutions.Select(sol => sol.Id).ToArray());
                    e.Result = BuildEntityItems(entities);
                },
                PostWorkCallBack = e =>
                {
                    listEntities.Items.Clear();
                    listEntities.Items.AddRange((ListViewItem[])e.Result);
                    listEntities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            });
        }

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

                entList.ForEach(id =>
                {
                    eq.Criteria.Conditions.Add(
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.Equals, id));
                });
                var allEntQry = new RetrieveMetadataChangesRequest
                {
                    Query = eq,
                    ClientVersionStamp = null
                };

                return ((RetrieveMetadataChangesResponse)Service.Execute(allEntQry)).EntityMetadata.ToList();
            }
            return new List<EntityMetadata>();
        }

        private void allEntitiesToolStripMenuItem_Click(object sender, EventArgs args)
        {
            listEntities.Items.Clear();
            listSelected.Items.Clear();
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Entities",
                Work = (wrk, e) =>
                {
                    var query = new RetrieveAllEntitiesRequest();
                    //{
                    //    Query = new EntityQueryExpression { Criteria = new MetadataFilterExpression(LogicalOperator.Or),
                    //    Properties = new MetadataPropertiesExpression { }
                    //    }
                    //}
                    // query.ClientVersionStamp = null;
                    var entities = ((RetrieveAllEntitiesResponse)Service.Execute(query)).EntityMetadata.ToList();
                    e.Result = BuildEntityItems(entities);

                    //wrk.ReportProgress(50, "Populating List");
                },
                PostWorkCallBack = e =>
                {
                    listEntities.Items.AddRange((ListViewItem[])e.Result);
                    //List<EntityMetadata> entities = ((EntityMetadata[])e.Result).ToList();
                    //foreach (EntityMetadata entity in entities)
                    //{
                    //    //var newItem = new ListViewItem();
                    //    //newItem.SubItems.Add(new TreeListViewSubItem(0) {  = "1-->M" });
                    //    var lvItem = new ListViewItem(
                    //        new string[] {
                    //            entity.DisplayName.UserLocalizedLabel == null ? entity.EntitySetName : entity.DisplayName.UserLocalizedLabel.Label.ToString(),
                    //            entity.LogicalName,
                    //            entity.IsCustomEntity.ToString()
                    //        }
                    //    );
                    //    lvItem.Name = entity.EntitySetName;
                    //    //lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = "1->M" });
                    //    //lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = "M->1" });
                    //    //lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = "M->M" });
                    //    listEntities.Items.Add(lvItem);
                    //}
                    listEntities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                },
                ProgressChanged = e => { }
            });
        }
    }

    public static class Helpers
    {
        public static VDX.Elements.Face GetFontSafe(VDX.FaceList faces, string name)
        {
            if (faces.ContainsName(name))
            {
                return faces[name];
            }

            int max_id = faces.Items.Select(f => f.ID).Max();
            int new_id = max_id + 1;

            var face = new VDX.Elements.Face(new_id, name);

            faces.Add(face);

            return face;
        }
    }
}