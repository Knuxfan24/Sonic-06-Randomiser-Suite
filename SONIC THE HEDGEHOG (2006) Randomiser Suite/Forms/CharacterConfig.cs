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
    public partial class CharacterConfig : Form
    {
        public CharacterConfig()
        {
            InitializeComponent();

            for (int i = 0; i < SetRandomisationForm.validCharacters.Count; i++)
            {
                switch (SetRandomisationForm.validCharacters[i])
                {
                    case "sonic_new": characterConfigList.SetItemChecked(0, true); break;
                    case "sonic_fast": characterConfigList.SetItemChecked(1, true); break;
                    case "princess": characterConfigList.SetItemChecked(2, true); break;
                    case "snow_board_wap": characterConfigList.SetItemChecked(3, true); break;
                    case "snow_board": characterConfigList.SetItemChecked(4, true); break;
                    case "tails": characterConfigList.SetItemChecked(5, true); break;
                    case "knuckles": characterConfigList.SetItemChecked(6, true); break;
                    case "shadow": characterConfigList.SetItemChecked(7, true); break;
                    case "rouge": characterConfigList.SetItemChecked(8, true); break;
                    case "omega": characterConfigList.SetItemChecked(9, true); break;
                    case "silver": characterConfigList.SetItemChecked(10, true); break;
                    case "blaze": characterConfigList.SetItemChecked(11, true); break;
                    case "amy": characterConfigList.SetItemChecked(12, true); break;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Discard changes made to Character Configuration?", "SET Randomiser Character Configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (characterConfigList.CheckedIndices.Count == 0)
            {
                MessageBox.Show("At least one selection must be checked", "SET Randomiser Character Configuration");
                return;
            }
            SetRandomisationForm.validCharacters.Clear();
            foreach (int item in characterConfigList.CheckedIndices)
            {
                switch (item)
                {
                    case 0: SetRandomisationForm.validCharacters.Add("sonic_new"); break;
                    case 1: SetRandomisationForm.validCharacters.Add("sonic_fast"); break;
                    case 2: SetRandomisationForm.validCharacters.Add("princess"); break;
                    case 3: SetRandomisationForm.validCharacters.Add("snow_board_wap"); break;
                    case 4: SetRandomisationForm.validCharacters.Add("snow_board"); break;
                    case 5: SetRandomisationForm.validCharacters.Add("tails"); break;
                    case 6: SetRandomisationForm.validCharacters.Add("knuckles"); break;
                    case 7: SetRandomisationForm.validCharacters.Add("shadow"); break;
                    case 8: SetRandomisationForm.validCharacters.Add("rouge"); break;
                    case 9: SetRandomisationForm.validCharacters.Add("omega"); break;
                    case 10: SetRandomisationForm.validCharacters.Add("silver"); break;
                    case 11: SetRandomisationForm.validCharacters.Add("blaze"); break;
                    case 12: SetRandomisationForm.validCharacters.Add("amy"); break;
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
