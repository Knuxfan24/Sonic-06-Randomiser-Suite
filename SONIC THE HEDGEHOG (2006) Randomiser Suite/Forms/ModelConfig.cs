using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    public partial class ModelConfig : Form
    {
        public ModelConfig()
        {
            InitializeComponent();

            for (int i = 0; i < CharacterRandomisationForm.validModels.Count; i++)
            {
                switch (CharacterRandomisationForm.validModels[i])
                {
                    case "\"player/sonic_new\"": characterConfigList.SetItemChecked(0, true); break;
                    case "\"player/snow_board\"": characterConfigList.SetItemChecked(1, true); break;
                    case "\"player/tails\"": characterConfigList.SetItemChecked(2, true); break;
                    case "\"player/knuckles\"": characterConfigList.SetItemChecked(3, true); break;
                    case "\"player/shadow\"": characterConfigList.SetItemChecked(4, true); break;
                    case "\"player/rouge\"": characterConfigList.SetItemChecked(5, true); break;
                    case "\"player/omega\"": characterConfigList.SetItemChecked(6, true); break;
                    case "\"player/silver\"": characterConfigList.SetItemChecked(7, true); break;
                    case "\"player/blaze\"": characterConfigList.SetItemChecked(8, true); break;
                    case "\"player/amy\"": characterConfigList.SetItemChecked(9, true); break;
                    case "\"player/princess_princess\"": characterConfigList.SetItemChecked(10, true); break;
                    case "\"player/supersonic\"": characterConfigList.SetItemChecked(11, true); break;
                    case "\"player/supershadow\"": characterConfigList.SetItemChecked(12, true); break;
                    case "\"player/supersilver\"": characterConfigList.SetItemChecked(13, true); break;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Discard changes made to Model Configuration?", "Character Attributes Randomiser Model Configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (characterConfigList.CheckedIndices.Count == 0)
            {
                MessageBox.Show("At least one selection must be checked", "Character Attributes Randomiser Model Configuration");
                return;
            }
            CharacterRandomisationForm.validModels.Clear();
            foreach (int item in characterConfigList.CheckedIndices)
            {
                switch (item)
                {
                    case 0: CharacterRandomisationForm.validModels.Add("\"player/sonic_new\""); break;
                    case 1: CharacterRandomisationForm.validModels.Add("\"player/snow_board\""); break;
                    case 2: CharacterRandomisationForm.validModels.Add("\"player/tails\""); break;
                    case 3: CharacterRandomisationForm.validModels.Add("\"player/knuckles\""); break;
                    case 4: CharacterRandomisationForm.validModels.Add("\"player/shadow\""); break;
                    case 5: CharacterRandomisationForm.validModels.Add("\"player/rouge\""); break;
                    case 6: CharacterRandomisationForm.validModels.Add("\"player/omega\""); break;
                    case 7: CharacterRandomisationForm.validModels.Add("\"player/silver\""); break;
                    case 8: CharacterRandomisationForm.validModels.Add("\"player/blaze\""); break;
                    case 9: CharacterRandomisationForm.validModels.Add("\"player/amy\""); break;
                    case 10: CharacterRandomisationForm.validModels.Add("\"player/princess_princess\""); break;
                    case 11: CharacterRandomisationForm.validModels.Add("\"player/supersonic\""); break;
                    case 12: CharacterRandomisationForm.validModels.Add("\"player/supershadow\""); break;
                    case 13: CharacterRandomisationForm.validModels.Add("\"player/supersilver\""); break;
                }
            }
            Close();
        }

        private void UncheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < characterConfigList.Items.Count; i++) characterConfigList.SetItemChecked(i, false);
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < characterConfigList.Items.Count; i++) characterConfigList.SetItemChecked(i, true);
        }
    }
}
