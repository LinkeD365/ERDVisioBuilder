using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinkeD365.ERDBuilder
{
    public partial class SaveMap : Form
    {
        public AllSettings settings;
        public string settingName;

        public SaveMap(AllSettings allSettings)
        {
            InitializeComponent();
            settings = allSettings;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSaveName.Text))
            {
                settingName = cboExisting.Text;
            }
            else
            {
                if (settings.Settings.Any(rw => rw.Name.ToLower() == txtSaveName.Text.ToLower()))
                {
                    MessageBox.Show("Please use a unique name for your configuration");
                    DialogResult = DialogResult.None;
                    return;
                }
                settingName = txtSaveName.Text;
            }
        }

        private void SaveMap_Load(object sender, EventArgs e)
        {
            cboExisting.Items.AddRange(settings.Settings.Select(rw => rw.Name).ToArray());
        }

        private void cboExisting_SelectedValueChanged(object sender, EventArgs e)
        {
            txtSaveName.Text = string.Empty;
        }
    }
}