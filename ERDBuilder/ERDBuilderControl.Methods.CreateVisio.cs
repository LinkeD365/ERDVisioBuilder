using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioAutomation.VDX.Elements;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl
    {
        protected void GenerateVisio(bool newVisio = false)
        {
            var selectedTables = (SBList<Table>)gvSelected.DataSource;
            if (!selectedTables.Any())
            {
                MessageBox.Show("Select one or more Tables before creating a Visio", "Select a Table", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
#if DEBUG 
            string fileName = @"C:\Users\carl.cookson\OneDrive\Code\XrmToolBox\erd.vsdx";
#else
            string fileName = saveVisio(cboSelectSaved.SelectedItem?.ToString() ?? string.Empty, newVisio);
#endif
            if (fileName == null) return;
            AddFullEntity(selectedTables);
            overrideSave = false;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!newVisio) GenerateVDX(selectedTables, fileName);
                else GenerateVSDX(selectedTables, fileName);

                if (MessageBox.Show("Visio File created successfully. Do you want to open the Visio Diagram?", "Success!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start(fileName);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void GenerateVSDX(SBList<Table> selectedTables, string fileName)
        {
            Utils.TableDisplayNames = chkListDisplay.CheckedItems.Contains("Table Display Names");
            Utils.ColumnsDisplayName = chkListDisplay.CheckedItems.Contains("Column Display Names");
            Utils.OnlyBetweenTable = checkRelationships.CheckedItems.Contains("Only Between Selected Tables");
            Utils.OneToMany = checkRelationships.CheckedItems.Contains("One-To-Many");
            Utils.ManyToOne = checkRelationships.CheckedItems.Contains("Many-To-One");
            Utils.ManyToMany = checkRelationships.CheckedItems.Contains("Many-To-Many");
            Utils.HiddenList = HiddenSystemList;
            Utils.NoLevels = numLevel.Value;
            Utils.HideActivity = HideActivity;
            Utils.HideParent = chkListHide.CheckedItems.Contains("Hide Parent Tables");
            Utils.Service = Service;
            Utils.CreateVisio(selectedTables,fileName);
            Utils.CompleteVisio(fileName);
        }

        private void GenerateVDX(SBList<Table> selectedTables, string fileName)
        {
            doc = new Drawing(template);
            page = new Page(pageWidth, pageHeight);
            face = doc.AddFace("Segoe UI");
            doc.Pages.Add(page);
            addedEntities.Clear();
            addedMtoM.Clear();


            

            foreach (var table in selectedTables)
            {
                if (addedEntities.Count == 0)
                {
                    AddEntity(table, false, pageWidth / 2, pageHeight / 2);
                }
                else
                {
                    AddEntity(table, false, nextX, nextY);
                }
            }

            tspProgress.Visible = true;
            tspProgress.Maximum = 100;
            tspProgress.Step = 1;
            tspProgress.Value = 25;
            if (checkRelationships.CheckedItems.Contains("Only Between Selected Tables"))
            {
                AddOnlySelectedRelationships(selectedTables);
                tspProgress.Value = 75;
            }
            else
            {
                foreach (var table in selectedTables)
                {
                    var entityMeta = table.Entity;
                    var primeEntity = addedEntities.First(pe => pe.LogicalName == entityMeta.LogicalName);

                    if (checkRelationships.CheckedItems.Contains("One-To-Many"))
                    {
                        AddAllOneToMany(primeEntity, entityMeta, numLevel.Value);
                    }

                    tspProgress.Value = 25;
                    if (checkRelationships.CheckedItems.Contains("Many-To-One"))
                    {
                        AddAllManyToOne(primeEntity, entityMeta, numLevel.Value);
                    }

                    tspProgress.Value = 50;
                    if (checkRelationships.CheckedItems.Contains("Many-To-Many"))
                    {
                        AddAllManyToMany(primeEntity, entityMeta, numLevel.Value);
                    }

                    tspProgress.Value = 75;
                }
            }

            doc.Save(fileName);
            tspProgress.Visible = false;
            ai.WriteEvent("Visio Entities Count", addedEntities.Count);
        }
    }
}
