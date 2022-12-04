using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl
    {
        private SBList<Table> BuildEntityItems(List<EntityMetadata> entities)
        {


            if (entities.Count == 0) return new SBList<Table>();

            var tableList = new SBList<Table>();
            foreach (var entity in entities)
            {
                var table = new Table(entity);

                table.PropertyChanged += Table_PropertyChanged;
                tableList.Add(table);
            }
            return tableList;
            //return new SortableBindingList<Table>(entities.Select(ent => new Table
            //{
            //    DisplayName = ent.DisplayName?.UserLocalizedLabel?.Label?.ToString() ?? ent.LogicalName,
            //    Logical = ent.LogicalName,
            //    Custom = ent.IsCustomEntity ?? false,


            //}));
        }
        private SBList<Column> BuildAttributeItems(string tableName)
        {
            var request = new RetrieveEntityRequest();
            request.EntityFilters = EntityFilters.Attributes;
            request.LogicalName = tableName;
            //query.EntityFilters = EntityFilters.Entity;
            var attributes = ((RetrieveEntityResponse)Service.Execute(request)).EntityMetadata.Attributes.ToList();


            if (attributes.Count == 0) return new SBList<Column>();

            var colList = new SBList<Column>();
            foreach (var att in attributes)
            {
                var column = new Column(att);
                column.PropertyChanged += Column_PropertyChanged;
                colList.Add(column);
            }
            return colList;
        }

        private void Column_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var column = (Column)sender;
            var table = ((SBList<Table>)gvTables.DataSource).First(tab => tab.Logical == column.Attribute.EntityLogicalName);
            if (column.Selected)
            {
                if (!table.Selected) table.Selected = true;
                table = ((SBList<Table>)gvSelected.DataSource).First(tab => tab.Logical == column.Attribute.EntityLogicalName);
                table.Columns.Add(column);
            }
            else
            {
                table = ((SBList<Table>)gvSelected.DataSource).FirstOrDefault(tab => tab.Logical == column.Attribute.EntityLogicalName);
                if (table == null) return;
                var selectCol = table.Columns.FirstOrDefault(col => col.LogicalName == column.LogicalName);
                if (selectCol != null) table.Columns.Remove(selectCol);
            }
            gvSelected.Refresh();
        }

        private void Table_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var table = (Table)sender;
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
            //gvSelected.DataSource = null;
            //gvSelected.DataSource = new SortableBindingList<Table>(selectedTables);
        }



        private void AddEntities(Settings setting)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Tables",
                Work = (wrk, e) =>
                {
                    var query = new RetrieveAllEntitiesRequest();
                    var entities = ((RetrieveAllEntitiesResponse)Service.Execute(query)).EntityMetadata.Where(tbl => tbl.IsIntersect != true).ToList();
                    e.Result = BuildEntityItems(entities);

                    //wrk.ReportProgress(50, "Populating List");
                },
                PostWorkCallBack = e =>
                {
                    Helper.AllTables = (SBList<Table>)e.Result;
                    Helper.AllTables.Sort("DisplayName", ListSortDirection.Ascending);
                    //gvTables.DataSource = Helper.AllTables;
                    InitTableGrid(Helper.AllTables);
                    // SortTables(1);
                    if (setting.Name != null)
                    {
                        AddConfig(setting);
                    }
                    ExecuteMethod(GetAttributes, (Table)gvTables.SelectedRows[0].DataBoundItem);
                },
                ProgressChanged = e => { }
            });
            //listEntities.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        //private void GetAttributes(Table table)
        //{
        //    GetAttributes(table, null, true);
        //}
        private void GetAttributes(Table table)
        {
            if (table.Columns.Any())
            {
                gvAttributes.DataSource = null;
                gvAttributes.DataSource = table.Columns;
                InitColGrid();
                return;
            }
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Attributes",
                AsyncArgument = false,
                Work = (wrk, e) =>
                {

                    e.Result = BuildAttributeItems(table.Logical);

                    //wrk.ReportProgress(50, "Populating List");
                },
                PostWorkCallBack = e =>
                {
                   
                    table.Columns = ((SBList<Column>)e.Result);
                    table.Columns.Sort("DisplayName", ListSortDirection.Ascending);
                    gvAttributes.DataSource = null;
                    gvAttributes.DataSource = table.Columns;
                    InitColGrid();


                },
                ProgressChanged = e => { }
            });


        }

        private void InitColGrid()
        {
            gvAttributes.Columns["DisplayName"].MinimumWidth = 150;
            gvAttributes.Columns["DisplayName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gvAttributes.Columns["LogicalName"].MinimumWidth = 100;
            gvAttributes.Columns["LogicalName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            gvAttributes.Columns["Selected"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            gvAttributes.Columns["Custom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

        }

        private void InitTableGrid(List<Table> tables)
        {
            InitTableGrid(new SBList<Table>(tables));
        }
        private void InitTableGrid()
        {
            gvTables.Columns["DisplayName"].MinimumWidth = 150;
            gvTables.Columns["DisplayName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gvTables.Columns["Logical"].MinimumWidth = 100;
            gvTables.Columns["Logical"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            gvTables.Columns["Selected"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            gvTables.Columns["Custom"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            gvTables.Columns["ColumnList"].Visible = false;
        }
        private void InitTableGrid(SBList<Table> tableList)
        {
            gvTables.SelectionChanged -= gvTables_SelectionChanged;
            gvTables.DataSource = null;
            gvTables.DataSource = tableList;
            InitTableGrid();
            gvTables.SelectionChanged += gvTables_SelectionChanged;


        }

        private void InitSelectedGrid(SBList<Table> selectedList)
        {
            gvSelected.DataSource = null;
            gvSelected.DataSource = selectedList;
            gvSelected.Columns["DisplayName"].MinimumWidth = 80;
            gvSelected.Columns["DisplayName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gvSelected.Columns["Logical"].Visible = false;


            gvSelected.Columns["Selected"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            gvSelected.Columns["Custom"].Visible = false;

            gvSelected.Columns["ColumnList"].MinimumWidth = 100;

            gvSelected.Columns["ColumnList"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // gvSelected.Columns["ColumnList"]

            gvSelected.Columns["ColumnList"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void AddFullEntity(SBList<Table> tableSelected)
        {
            foreach (var table in tableSelected.Where(table => table.Entity.Attributes == null || table.Entity.Attributes.Count() == 0))
            {
                AddFullEntity(table);
            }
        }

        private void AddFullEntity(Table table)
        {

            var query = new RetrieveEntityRequest();
            query.LogicalName = table.Logical;
            query.EntityFilters = EntityFilters.All;
            table.Entity = ((RetrieveEntityResponse)Service.Execute(query)).EntityMetadata;
        }

        internal string saveVisio(string visioName, bool newVisio)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = newVisio ? "VSDX files|*.vsdx" : "VDX files|*.vdx";// filters for text files only
            sfd.DefaultExt = newVisio ? "vsdx" : "vdx";
            sfd.AddExtension = true;
            sfd.FileName = visioName + (newVisio ? ".vsdx" : ".vdx");
            sfd.Title = "Save Visio File";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }

            return null;
        }
    }

}
