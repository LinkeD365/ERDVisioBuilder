using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl
    {
        public void InitContainerGrid(SBList<Container> containers)
        {
            gvContainers.DataSource = null;
            gvContainers.DataSource = containers;


        }
        private void gvContainers_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            gvSelected.Columns["containerCol"].Visible = Helper.Containers.Any();
        }

        private void AddContainerColumn()
        {
            DataGridViewComboBoxColumn containerCol = new DataGridViewComboBoxColumn();
            containerCol.HeaderText = "Container";
            containerCol.Name = "containerCol";
            AddContainerList(containerCol);
           
            //containerCol.Items.Insert(0, "Select");
            containerCol.DataPropertyName = "ContainerName";
            containerCol.ValueMember = "Title";


            gvSelected.Columns.Insert(gvSelected.Columns.Count, containerCol);
            containerCol.Visible = Helper.Containers.Any();

        }

        private void AddContainerList(DataGridViewComboBoxColumn containerCol)
        {
            var contList = new SBList<Container>(Helper.Containers);
            contList.Insert(0, new LinkeD365.ERDBuilder.Container() { Title = "None" });
            containerCol.DataSource = contList;
        }
    }
}
