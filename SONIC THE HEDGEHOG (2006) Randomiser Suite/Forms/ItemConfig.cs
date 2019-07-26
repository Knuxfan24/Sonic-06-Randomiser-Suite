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
    public partial class ItemConfig : Form
    {
        List<int> checkedItems;
        public ItemConfig(List<int> validItems)
        {
            InitializeComponent();
            checkedItems = validItems;

            for (int i = 0; i < validItems.Count; i++)
            {
                switch (validItems[i])
                {
                    case 1: itemConfigList.SetItemChecked(0, true); break;
                    case 2: itemConfigList.SetItemChecked(1, true); break;
                    case 3: itemConfigList.SetItemChecked(2, true); break;
                    case 4: itemConfigList.SetItemChecked(3, true); break;
                    case 5: itemConfigList.SetItemChecked(4, true); break;
                    case 6: itemConfigList.SetItemChecked(5, true); break;
                    case 7: itemConfigList.SetItemChecked(6, true); break;
                    case 8: itemConfigList.SetItemChecked(7, true); break;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Discard changes made to Item Capsule Configuration?", "SET Randomiser Item Configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (itemConfigList.CheckedIndices.Count == 0)
            {
                MessageBox.Show("At least one selection must be checked", "SET Randomiser Item Configuration");
                return;
            }
            SetRandomisationForm.validCharacters.Clear();
            foreach (int item in itemConfigList.CheckedIndices)
            {
                switch (item)
                {
                    case 0: SetRandomisationForm.validItems.Add(1); break;
                    case 1: SetRandomisationForm.validItems.Add(2); break;
                    case 2: SetRandomisationForm.validItems.Add(3); break;
                    case 3: SetRandomisationForm.validItems.Add(4); break;
                    case 4: SetRandomisationForm.validItems.Add(5); break;
                    case 5: SetRandomisationForm.validItems.Add(6); break;
                    case 6: SetRandomisationForm.validItems.Add(7); break;
                    case 7: SetRandomisationForm.validItems.Add(8); break;
                }
            }
            Close();
        }

        private void UncheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < itemConfigList.Items.Count; i++) itemConfigList.SetItemChecked(i, false);
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < itemConfigList.Items.Count; i++) itemConfigList.SetItemChecked(i, true);
        }
    }
}
