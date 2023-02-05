using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using XrmToolBox.Extensibility;

namespace LinkeD365.ERDBuilder
{
    public partial class ERDBuilderControl
    {
        private void AddSavedConfigs()
        {
            cboSelectSaved.Items.Clear();
            cboSelectSaved.Items.AddRange(mySettings.Settings.Select(set => set.Name).ToArray());
        }

        private void cboSelectSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectSaved.SelectedItem == null)
            {
                return;
            }

            var selectedConfig = mySettings.Settings.First(stng => stng.Name == cboSelectSaved.SelectedItem.ToString());

            if (selectedConfig is null) return;
            Helper.Containers = new SBList<Container>(selectedConfig.Containers);
            InitContainerGrid(Helper.Containers);
            InitSelectedGrid(new SBList<Table>());
            

            SetOptions(selectedConfig);


            ExecuteMethod(AddEntities, selectedConfig);
        }

        private void SetDefaultOptions()
        {
            checkRelationships.SetItemChecked(3, true);
            chkListDisplay.SetItemChecked(0, true);
            chkListDisplay.SetItemChecked(1, true);
            chkListHide.SetItemChecked(0, true);
            chkListHide.SetItemChecked(1, true);
            chkListHide.SetItemChecked(2, true);
        }
        /// <summary>
        /// #32 Added to resolve
        /// </summary>
        /// <param name="selectedConfig"></param>
        private void SetOptions(Settings selectedConfig)
        {
            for (int i=0; i<=checkRelationships.Items.Count -1;i++)
            {
                checkRelationships.SetItemChecked(i, selectedConfig.RelationshipMaps.Contains(i));
            }

            for (int i = 0; i <= chkListHide.Items.Count - 1; i++)
            {
                chkListHide.SetItemChecked(i, selectedConfig.Hide.Contains(i));
            }

            for (int i = 0; i <= chkListDisplay.Items.Count - 1; i++)
            {
                chkListDisplay.SetItemChecked(i, selectedConfig.Display.Contains(i));
            }
           
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var exportConfig = new Export(mySettings);
            if (exportConfig.ShowDialog() != DialogResult.OK) return;
            exportFile.FileName = exportConfig.SettingName + ".xml";
            exportFile.OverwritePrompt = true;

            if (exportFile.ShowDialog() != DialogResult.OK) return;

            var setting = mySettings.Settings.First(con => con.Name == exportConfig.SettingName);

            XmlSerializer writer =
                new XmlSerializer(typeof(Settings));

            FileStream file = File.Create(exportFile.FileName);

            writer.Serialize(file, setting);
            file.Close();

            MessageBox.Show("Setting Exported", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //   overrideSave = true;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            exportFile.OverwritePrompt = false;
            if (exportFile.ShowDialog() != DialogResult.OK) return;

            XmlSerializer reader =
                new XmlSerializer(typeof(Settings));
            StreamReader file = new StreamReader(exportFile.FileName);

            var setting = (Settings)reader.Deserialize(file);
            var saveConfig = new SaveMap(mySettings);
            saveConfig.txtSaveName.Text = setting.Name;
            if (saveConfig.ShowDialog() != DialogResult.OK) return;

            setting.Name = saveConfig.settingName;
            SaveConfig(setting);
            ExecuteMethod(AddEntities, setting);
            MessageBox.Show("Setting Imported", "Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AddSavedConfigs();
        }

        private void AddConfig(Settings selectedConfig)
        {
            foreach (Table selectedTable in selectedConfig.Tables)
            {
                var table = Helper.AllTables.FirstOrDefault(tbl => tbl.Logical == selectedTable.Logical);
                if (table == null) continue;
                table.ContainerName = selectedTable.ContainerName;
                table.Selected = true;
                
                if (!selectedTable.Columns.Any()) continue;
                if (!table.Columns.Any()) table.Columns = BuildAttributeItems(table.Logical);
                table.Columns.Intersect(selectedTable.Columns, new ColumnComparer()).ToList().ForEach(col => col.Selected = true);

            }
        }

        /// <summary>
        /// Save current config
        /// #7
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            ShowSaveConfig();
            AddSavedConfigs();

        }

        private void ShowSaveConfig()
        {
            var saveConfig = new SaveMap(mySettings);
            if (saveConfig.ShowDialog() != DialogResult.OK) return;

            var setting = new Settings();
            setting.Name = saveConfig.settingName;
            setting.Level = numLevel.Value;
            foreach (var chkBox in checkRelationships.CheckedIndices)
            {
                setting.RelationshipMaps.Add((int)chkBox);
            }
            //foreach (ListViewItem entity in listSelected.Items)
            //{
            //    setting.Tables.Add(new Table ( entity.Text ));
            //}
            foreach (var chkBox in chkListHide.CheckedIndices)
            {
                setting.Hide.Add((int)chkBox);
            }

            foreach (var chkBox in chkListDisplay.CheckedIndices)
            {
                setting.Display.Add((int)chkBox);
            }
            setting.Tables = ((SBList<Table>)gvSelected.DataSource).ToList();
            setting.Containers = ((SBList<Container>)gvContainers.DataSource).ToList();
            SaveConfig(setting);
        }

        private void SaveConfig(Settings setting)
        {
            if (mySettings.Settings.Any(mr => mr.Name == setting.Name))
            {
                mySettings.Settings[mySettings.Settings.IndexOf(setting)] = setting;
            }
            else
            {
                mySettings.Settings.Add(setting);
            }
            SettingsManager.Instance.Save(typeof(AllSettings), mySettings);
        }
    }
}
