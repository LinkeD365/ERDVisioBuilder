using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LinkeD365.ERDBuilder
{
    public partial class Export : Form
    {
        public AllSettings Settings;
        public string SettingName;
        public Export(AllSettings allSettings)
        {
            InitializeComponent();
            Settings = allSettings;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cboConfig.SelectedItem == null)
            {
                DialogResult = DialogResult.None;
            }
            else SettingName = cboConfig.Text;
        }

        private void Export_Load(object sender, EventArgs e) => cboConfig.Items.AddRange(Settings.Settings.Select(con => con.Name).ToArray());
    }
}
