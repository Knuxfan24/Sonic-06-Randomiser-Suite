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
    public partial class SurfaceConfig : Form
    {
        public SurfaceConfig()
        {
            InitializeComponent();

            for (int i = 0; i < CollisionPropertiesForm.validSurfaces.Count; i++)
            {
                switch (CollisionPropertiesForm.validSurfaces[i])
                {
                    case "0": surfaceConfigList.SetItemChecked(0, true); break;
                    case "1": surfaceConfigList.SetItemChecked(1, true); break;
                    case "2": surfaceConfigList.SetItemChecked(2, true); break;
                    case "3": surfaceConfigList.SetItemChecked(3, true); break;
                    case "5": surfaceConfigList.SetItemChecked(4, true); break;
                    case "6": surfaceConfigList.SetItemChecked(5, true); break;
                    case "8": surfaceConfigList.SetItemChecked(6, true); break;
                    case "9": surfaceConfigList.SetItemChecked(7, true); break;
                    case "A": surfaceConfigList.SetItemChecked(8, true); break;
                    case "E": surfaceConfigList.SetItemChecked(9, true); break;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Discard changes made to Surface Type Configuration?", "Collision Properties Randomiser Surface Type Configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (surfaceConfigList.CheckedIndices.Count == 0)
            {
                MessageBox.Show("At least one selection must be checked", "Collision Properties Randomiser Surface Type Configuration");
                return;
            }
            CollisionPropertiesForm.validSurfaces.Clear();
            foreach (int item in surfaceConfigList.CheckedIndices)
            {
                switch (item)
                {
                    case 0: CollisionPropertiesForm.validSurfaces.Add("0"); break;
                    case 1: CollisionPropertiesForm.validSurfaces.Add("1"); break;
                    case 2: CollisionPropertiesForm.validSurfaces.Add("2"); break;
                    case 3: CollisionPropertiesForm.validSurfaces.Add("3"); break;
                    case 4: CollisionPropertiesForm.validSurfaces.Add("5"); break;
                    case 5: CollisionPropertiesForm.validSurfaces.Add("6"); break;
                    case 6: CollisionPropertiesForm.validSurfaces.Add("8"); break;
                    case 7: CollisionPropertiesForm.validSurfaces.Add("9"); break;
                    case 8: CollisionPropertiesForm.validSurfaces.Add("A"); break;
                    case 9: CollisionPropertiesForm.validSurfaces.Add("E"); break;
                }
            }
            Close();
        }

        private void UncheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < surfaceConfigList.Items.Count; i++) surfaceConfigList.SetItemChecked(i, false);
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < surfaceConfigList.Items.Count; i++) surfaceConfigList.SetItemChecked(i, true);
        }
    }
}
