using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace LinkeD365.ERDBuilder
{
    public partial class SolutionPicker : Form
    {
        private readonly IOrganizationService service;
        public List<Microsoft.Xrm.Sdk.Entity> SelectedSolutions { get; } = new List<Microsoft.Xrm.Sdk.Entity>();

        public SolutionPicker(IOrganizationService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void SolutionPicker_Load(object sender, EventArgs e)
        {
            listSolutions.Items.Clear();

            BackgroundWorker wrkr = new BackgroundWorker();
            wrkr.DoWork += Wrkr_DoWork;
            wrkr.RunWorkerCompleted += PopulateList;
            wrkr.RunWorkerAsync();
        }

        private void Wrkr_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = RetrieveSolutions();
        }

        private void PopulateList(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Microsoft.Xrm.Sdk.Entity solution in ((EntityCollection)e.Result).Entities)
            {
                ListViewItem item = new ListViewItem(solution["friendlyname"].ToString());
                item.SubItems.Add(solution["version"].ToString());
                item.SubItems.Add(((EntityReference)solution["publisherid"]).Name);
                item.Tag = solution;

                listSolutions.Items.Add(item);
            }
            listSolutions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listSolutions.Enabled = true;
            btnOk.Enabled = true;
        }

        /// <summary>
        /// Get all the solutions
        /// Thanks to code in MsCrmTools.MetadataBroswer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private EntityCollection RetrieveSolutions()
        {
            try
            {
                QueryExpression solQry = new QueryExpression("solution");
                solQry.Distinct = true;
                solQry.ColumnSet = new ColumnSet("friendlyname", "version", "publisherid", "solutionid");
                solQry.Criteria = new FilterExpression();
                solQry.Criteria.AddCondition(new ConditionExpression("isvisible", ConditionOperator.Equal, true));
                solQry.Criteria.AddCondition(new ConditionExpression("uniquename", ConditionOperator.NotEqual, "Default"));

                return service.RetrieveMultiple(solQry);
            }
            catch (Exception err)
            {
                if (err.InnerException is FaultException) throw new Exception("Error while retrieving solutions: " + err.InnerException.Message);

                throw new Exception("Error while retrieving solutions: " + err.Message);
            }
        }

        private void listSolutions_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var list = (ListView)sender;
            list.Sorting = list.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            list.ListViewItemSorter = new ListViewComparer(e.Column, list.Sorting);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (listSolutions.SelectedItems.Count > 0)
            {
                SelectedSolutions.AddRange(listSolutions.SelectedItems.Cast<ListViewItem>().Select(item => (Microsoft.Xrm.Sdk.Entity)item.Tag));
                DialogResult = DialogResult.OK;
                Close();
            }
            else MessageBox.Show("Please select at least one solution", "Select a Solution", MessageBoxButtons.OK);
        }
    }
}