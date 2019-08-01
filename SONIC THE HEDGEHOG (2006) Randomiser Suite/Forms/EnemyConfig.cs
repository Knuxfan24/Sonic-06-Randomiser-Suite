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
    public partial class EnemyConfig : Form
    {
        public EnemyConfig()
        {
            InitializeComponent();
            for (int i = 0; i < SetRandomisationForm.validEnemies.Count; i++)
            {
                switch (SetRandomisationForm.validEnemies[i])
                {
                    case "cBiter": enemyConfigList.SetItemChecked(0, true); break;
                    case "cGolem": enemyConfigList.SetItemChecked(1, true); break;
                    case "cTaker": enemyConfigList.SetItemChecked(2, true); break;
                    case "cCrawler": enemyConfigList.SetItemChecked(3, true); break;

                    case "cGazer": enemyConfigList.SetItemChecked(4, true); break;
                    case "cStalker": enemyConfigList.SetItemChecked(5, true); break;
                    case "cTitan": enemyConfigList.SetItemChecked(6, true); break;
                    case "cTricker": enemyConfigList.SetItemChecked(7, true); break;

                    case "eArmor": enemyConfigList.SetItemChecked(8, true); break;
                    case "eBluster": enemyConfigList.SetItemChecked(9, true); break;
                    case "eBuster": enemyConfigList.SetItemChecked(10, true); break;
                    case "eBuster(Fly)": enemyConfigList.SetItemChecked(11, true); break;
                    case "eBomber": enemyConfigList.SetItemChecked(12, true); break;
                    case "eCannon": enemyConfigList.SetItemChecked(13, true); break;
                    case "eCannon(Fly)": enemyConfigList.SetItemChecked(14, true); break;
                    case "eChaser": enemyConfigList.SetItemChecked(15, true); break;
                    case "eCommander": enemyConfigList.SetItemChecked(16, true); break;
                    case "eFlyer": enemyConfigList.SetItemChecked(17, true); break;
                    case "eGuardian": enemyConfigList.SetItemChecked(18, true); break;
                    case "eGunner": enemyConfigList.SetItemChecked(19, true); break;
                    case "eGunner(Fly)": enemyConfigList.SetItemChecked(20, true); break;
                    case "eHunter": enemyConfigList.SetItemChecked(21, true); break;
                    case "eKeeper": enemyConfigList.SetItemChecked(22, true); break;
                    case "eLancer": enemyConfigList.SetItemChecked(23, true); break;
                    case "eLancer(Fly)": enemyConfigList.SetItemChecked(24, true); break;
                    case "eLiner": enemyConfigList.SetItemChecked(25, true); break;
                    case "eRounder": enemyConfigList.SetItemChecked(26, true); break;
                    case "eSearcher": enemyConfigList.SetItemChecked(27, true); break;
                    case "eStinger": enemyConfigList.SetItemChecked(28, true); break;
                    case "eStinger(Fly)": enemyConfigList.SetItemChecked(29, true); break;
                    case "eSweeper": enemyConfigList.SetItemChecked(30, true); break;
                    case "eWalker": enemyConfigList.SetItemChecked(31, true); break;

                    case "eCerberus": enemyConfigList.SetItemChecked(32, true); break;
                    case "eGenesis": enemyConfigList.SetItemChecked(33, true); break;
                    case "eWyvern": enemyConfigList.SetItemChecked(34, true); break;

                    case "firstIblis": enemyConfigList.SetItemChecked(35, true); break;
                    case "secondIblis": enemyConfigList.SetItemChecked(36, true); break;
                    case "thirdIblis": enemyConfigList.SetItemChecked(37, true); break;

                    case "firstmefiress": enemyConfigList.SetItemChecked(38, true); break;

                    case "solaris01": enemyConfigList.SetItemChecked(39, true); break;
                    case "solaris02": enemyConfigList.SetItemChecked(40, true); break;
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Discard changes made to Enemy Configuration?", "SET Randomiser Enemy Configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Close();
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (enemyConfigList.CheckedIndices.Count == 0)
            {
                MessageBox.Show("At least one selection must be checked", "SET Randomiser Enemy Configuration");
                return;
            }
            SetRandomisationForm.validEnemies.Clear();
            foreach (int item in enemyConfigList.CheckedIndices)
            {
                switch (item)
                {
                    case 0: SetRandomisationForm.validEnemies.Add("cBiter"); break;
                    case 1: SetRandomisationForm.validEnemies.Add("cGolem"); break;
                    case 2: SetRandomisationForm.validEnemies.Add("cTaker"); break;
                    case 3: SetRandomisationForm.validEnemies.Add("cCrawler"); break;

                    case 4: SetRandomisationForm.validEnemies.Add("cGazer"); break;
                    case 5: SetRandomisationForm.validEnemies.Add("cStalker"); break;
                    case 6: SetRandomisationForm.validEnemies.Add("cTitan"); break;
                    case 7: SetRandomisationForm.validEnemies.Add("cTricker"); break;

                    case 8: SetRandomisationForm.validEnemies.Add("eArmor"); break;
                    case 9: SetRandomisationForm.validEnemies.Add("eBluster"); break;
                    case 10: SetRandomisationForm.validEnemies.Add("eBuster"); break;
                    case 11: SetRandomisationForm.validEnemies.Add("eBuster(Fly)"); break;
                    case 12: SetRandomisationForm.validEnemies.Add("eBomber"); break;
                    case 13: SetRandomisationForm.validEnemies.Add("eCannon"); break;
                    case 14: SetRandomisationForm.validEnemies.Add("eCannon(Fly)"); break;
                    case 15: SetRandomisationForm.validEnemies.Add("eChaser"); break;
                    case 16: SetRandomisationForm.validEnemies.Add("eCommander"); break;
                    case 17: SetRandomisationForm.validEnemies.Add("eFlyer"); break;
                    case 18: SetRandomisationForm.validEnemies.Add("eGuardian"); break;
                    case 19: SetRandomisationForm.validEnemies.Add("eGunner"); break;
                    case 20: SetRandomisationForm.validEnemies.Add("eGunner(Fly)"); break;
                    case 21: SetRandomisationForm.validEnemies.Add("eHunter"); break;
                    case 22: SetRandomisationForm.validEnemies.Add("eKeeper"); break;
                    case 23: SetRandomisationForm.validEnemies.Add("eLancer"); break;
                    case 24: SetRandomisationForm.validEnemies.Add("eLancer(Fly)"); break;
                    case 25: SetRandomisationForm.validEnemies.Add("eLiner"); break;
                    case 26: SetRandomisationForm.validEnemies.Add("eRounder"); break;
                    case 27: SetRandomisationForm.validEnemies.Add("eSearcher"); break;
                    case 28: SetRandomisationForm.validEnemies.Add("eStinger"); break;
                    case 29: SetRandomisationForm.validEnemies.Add("eStinger(Fly)"); break;
                    case 30: SetRandomisationForm.validEnemies.Add("eSweeper"); break;
                    case 31: SetRandomisationForm.validEnemies.Add("eWalker"); break;

                    case 32: SetRandomisationForm.validEnemies.Add("eCerberus"); break;
                    case 33: SetRandomisationForm.validEnemies.Add("eGenesis"); break;
                    case 34: SetRandomisationForm.validEnemies.Add("eWyvern"); break;

                    case 35: SetRandomisationForm.validEnemies.Add("firstIblis"); break;
                    case 36: SetRandomisationForm.validEnemies.Add("secondIblis"); break;
                    case 37: SetRandomisationForm.validEnemies.Add("thirdIblis"); break;

                    case 38: SetRandomisationForm.validEnemies.Add("firstmefiress"); break;

                    case 39: SetRandomisationForm.validEnemies.Add("solaris01"); break;
                    case 40: SetRandomisationForm.validEnemies.Add("solaris02"); break;
                }
            }
            Close();
        }

        private void UncheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < enemyConfigList.Items.Count; i++) enemyConfigList.SetItemChecked(i, false);
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < enemyConfigList.Items.Count; i++) enemyConfigList.SetItemChecked(i, true);
            DialogResult dialogResult = MessageBox.Show("Include Bosses?", "SET Randomiser Enemy Configuration", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                enemyConfigList.SetItemChecked(30, false);
                enemyConfigList.SetItemChecked(31, false);
                enemyConfigList.SetItemChecked(32, false);
                enemyConfigList.SetItemChecked(33, false);
                enemyConfigList.SetItemChecked(34, false);
                enemyConfigList.SetItemChecked(35, false);
                enemyConfigList.SetItemChecked(36, false);
                enemyConfigList.SetItemChecked(37, false);
                enemyConfigList.SetItemChecked(38, false);
                enemyConfigList.SetItemChecked(39, false);
                enemyConfigList.SetItemChecked(40, false);
            }
        }
    }
}
