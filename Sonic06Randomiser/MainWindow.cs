using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sonic06Randomiser
{
    public partial class MainWindow : Form
    {
        //Setup Variables

        #region Enemy Variables
        bool cBiter = true;
        bool cCrawler = true;
        bool cGazer = true;
        bool cGolem = true;
        bool cStalker = true;
        bool cTaker = true;
        bool cTitan = true;
        bool cTricker = true;
        bool eArmor = true;
        bool eBomber = true;
        bool eBluster = true;
        bool eBuster = true;
        bool eCannon = true;
        bool eCommander = true;
        bool eFlyer = true;
        bool eGuardian = true;
        bool eGunner = true;
        bool eKeeper = true;
        bool eLancer = true;
        bool eLiner = true;
        bool eRounder = true;
        bool eSearcher = true;
        bool eStinger = true;
        bool eSweeper = true;
        bool eHunter = true;
        bool eWalker = true;
        bool eChaser = true;

        bool eCerberus = false;
        bool eGenesis = false;
        bool eWyvern = false;
        bool iblisOne = false;
        bool iblisTwo = false;
        bool iblisThree = false;
        bool mephiles = false;
        bool solaris = false;
        #endregion

        #region Character Variables
        bool sonic = true;
        bool tails = true;
        bool knuckles = true;
        bool sonicMachSpeed = false;
        bool sonicElise = false;
        bool sonicSnowboardWAP = false;
        bool sonicSnowboardCSC = false;
        bool shadow = true;
        bool rouge = true;
        bool omega = true;
        bool silver = true;
        bool blaze = true;
        bool amy = true;
        #endregion

        #region Item Capsule Variables
        bool emptyCapsule = false;
        bool fiveRingCapsule = true;
        bool tenRingCapsule = true;
        bool twentyRingCapsule = true;
        bool extraLifeCapsule = true;
        bool powerSneakersCapsule = true;
        bool powerGaugeRefillCapsule = true;
        bool invincibilityCapsule = true;
        bool shieldCapsule = true;
        #endregion

        #region Main Variables
        bool randomEnemies = false;
        bool randomCharacters = false;
        bool randomItems = false;
        bool randomVoices = false;
        bool spoilerLog = false;
        bool keepXML = false;
        bool randomiseFolder = false;
        int outputFolderType = 0; //0 = Custom, 1 = Source, 2 = Program
        string filepath = "";
        string output = "";
        Random rnd = new Random();
        string rndSeed = "";
        bool messageBox = true;
        bool bossWarningShown = false;
        #endregion

        public MainWindow()
        {
            //Basic Windows Forms setup as well as generating the first RND Seed.
            InitializeComponent();
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }

        #region Enemy Checkboxes
        #region Iblis Monsters
        private void cBiterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cBiterToolStripMenuItem.Checked)
            {
                cBiter = false;
                cBiterToolStripMenuItem.Checked = false;
            }
            else
            {
                cBiter = true;
                cBiterToolStripMenuItem.Checked = true;
            }
        }

        private void cCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cCrawlerToolStripMenuItem.Checked)
            {
                cCrawler = false;
                cCrawlerToolStripMenuItem.Checked = false;
            }
            else
            {
                cCrawler = true;
                cCrawlerToolStripMenuItem.Checked = true;
            }
        }

        private void cGolemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cGolemToolStripMenuItem.Checked)
            {
                cGolem = false;
                cGolemToolStripMenuItem.Checked = false;
            }
            else
            {
                cGolem = true;
                cGolemToolStripMenuItem.Checked = true;
            }
        }

        private void cTakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cTakerToolStripMenuItem.Checked)
            {
                cTaker = false;
                cTakerToolStripMenuItem.Checked = false;
            }
            else
            {
                cTaker = true;
                cTakerToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #region Mephiles Monsters
        private void cGazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cGazerToolStripMenuItem.Checked)
            {
                cGazer = false;
                cGazerToolStripMenuItem.Checked = false;
            }
            else
            {
                cGazer = true;
                cGazerToolStripMenuItem.Checked = true;
            }
        }

        private void cStalkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cStalkerToolStripMenuItem.Checked)
            {
                cStalker = false;
                cStalkerToolStripMenuItem.Checked = false;
            }
            else
            {
                cStalker = true;
                cStalkerToolStripMenuItem.Checked = true;
            }
        }

        private void cTitanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cTitanToolStripMenuItem.Checked)
            {
                cTitan = false;
                cTitanToolStripMenuItem.Checked = false;
            }
            else
            {
                cTitan = true;
                cTitanToolStripMenuItem.Checked = true;
            }
        }

        private void cTrickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cTrickerToolStripMenuItem.Checked)
            {
                cTricker = false;
                cTrickerToolStripMenuItem.Checked = false;
            }
            else
            {
                cTricker = true;
                cTrickerToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #region Eggman Robots
        private void eArmorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eArmorToolStripMenuItem.Checked)
            {
                eArmor = false;
                eArmorToolStripMenuItem.Checked = false;
            }
            else
            {
                eArmor = true;
                eArmorToolStripMenuItem.Checked = true;
            }
        }

        private void eBomberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eBomberToolStripMenuItem.Checked)
            {
                eBomber = false;
                eBomberToolStripMenuItem.Checked = false;
            }
            else
            {
                eBomber = true;
                eBomberToolStripMenuItem.Checked = true;
            }
        }

        private void eBlusterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eBlusterToolStripMenuItem.Checked)
            {
                eBluster = false;
                eBlusterToolStripMenuItem.Checked = false;
            }
            else
            {
                eBluster = true;
                eBlusterToolStripMenuItem.Checked = true;
            }
        }

        private void eBusterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eBusterToolStripMenuItem.Checked)
            {
                eBuster = false;
                eBusterToolStripMenuItem.Checked = false;
            }
            else
            {
                eBuster = true;
                eBusterToolStripMenuItem.Checked = true;
            }
        }

        private void eCannonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eCannonToolStripMenuItem.Checked)
            {
                eCannon = false;
                eCannonToolStripMenuItem.Checked = false;
            }
            else
            {
                eCannon = true;
                eCannonToolStripMenuItem.Checked = true;
            }
        }

        private void eCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eCommanderToolStripMenuItem.Checked)
            {
                eCommander = false;
                eCommanderToolStripMenuItem.Checked = false;
            }
            else
            {
                eCommander = true;
                eCommanderToolStripMenuItem.Checked = true;
            }
        }

        private void eFlyerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eFlyerToolStripMenuItem.Checked)
            {
                eFlyer = false;
                eFlyerToolStripMenuItem.Checked = false;
            }
            else
            {
                eFlyer = true;
                eFlyerToolStripMenuItem.Checked = true;
            }
        }

        private void eGuardianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eGuardianToolStripMenuItem.Checked)
            {
                eGuardian = false;
                eGuardianToolStripMenuItem.Checked = false;
            }
            else
            {
                eGuardian = true;
                eGuardianToolStripMenuItem.Checked = true;
            }
        }

        private void eGunnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eGunnerToolStripMenuItem.Checked)
            {
                eGunner = false;
                eGunnerToolStripMenuItem.Checked = false;
            }
            else
            {
                eGunner = true;
                eGunnerToolStripMenuItem.Checked = true;
            }
        }

        private void eKeeperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eKeeperToolStripMenuItem.Checked)
            {
                eKeeper = false;
                eKeeperToolStripMenuItem.Checked = false;
            }
            else
            {
                eKeeper = true;
                eKeeperToolStripMenuItem.Checked = true;
            }
        }

        private void eLancerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eLancerToolStripMenuItem.Checked)
            {
                eLancer = false;
                eLancerToolStripMenuItem.Checked = false;
            }
            else
            {
                eLancer = true;
                eLancerToolStripMenuItem.Checked = true;
            }
        }

        private void eLinerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eLinerToolStripMenuItem.Checked)
            {
                eLiner = false;
                eLinerToolStripMenuItem.Checked = false;
            }
            else
            {
                eLiner = true;
                eLinerToolStripMenuItem.Checked = true;
            }
        }

        private void eRounderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eRounderToolStripMenuItem.Checked)
            {
                eRounder = false;
                eRounderToolStripMenuItem.Checked = false;
            }
            else
            {
                eRounder = true;
                eRounderToolStripMenuItem.Checked = true;
            }
        }

        private void eSearcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eSearcherToolStripMenuItem.Checked)
            {
                eSearcher = false;
                eSearcherToolStripMenuItem.Checked = false;
            }
            else
            {
                eSearcher = true;
                eSearcherToolStripMenuItem.Checked = true;
            }
        }

        private void eStingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eStingerToolStripMenuItem.Checked)
            {
                eStinger = false;
                eStingerToolStripMenuItem.Checked = false;
            }
            else
            {
                eStinger = true;
                eStingerToolStripMenuItem.Checked = true;
            }
        }

        private void eSweeperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eSweeperToolStripMenuItem.Checked)
            {
                eSweeper = false;
                eSweeperToolStripMenuItem.Checked = false;
            }
            else
            {
                eSweeper = true;
                eSweeperToolStripMenuItem.Checked = true;
            }
        }

        private void eggWalkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eggWalkerToolStripMenuItem.Checked)
            {
                eWalker = false;
                eggWalkerToolStripMenuItem.Checked = false;
            }
            else
            {
                eWalker = true;
                eggWalkerToolStripMenuItem.Checked = true;
            }
        }

        private void eggHunterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eggHunterToolStripMenuItem.Checked)
            {
                eHunter = false;
                eggHunterToolStripMenuItem.Checked = false;
            }
            else
            {
                eHunter = true;
                eggHunterToolStripMenuItem.Checked = true;
            }
        }

        private void eggChaserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eggChaserToolStripMenuItem.Checked)
            {
                eChaser = false;
                eggChaserToolStripMenuItem.Checked = false;
            }
            else
            {
                eChaser = true;
                eggChaserToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #region Bosses
        private void eggCerberusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (eggCerberusToolStripMenuItem.Checked)
            {
                eCerberus = false;
                eggCerberusToolStripMenuItem.Checked = false;
            }
            else
            {
                eCerberus = true;
                eggCerberusToolStripMenuItem.Checked = true;
            }
        }

        private void eggGenesisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (eggGenesisToolStripMenuItem.Checked)
            {
                eGenesis = false;
                eggGenesisToolStripMenuItem.Checked = false;
            }
            else
            {
                eGenesis = true;
                eggGenesisToolStripMenuItem.Checked = true;
            }
        }

        private void eggWyvernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (eggWyvernToolStripMenuItem.Checked)
            {
                eWyvern = false;
                eggWyvernToolStripMenuItem.Checked = false;
            }
            else
            {
                eWyvern = true;
                eggWyvernToolStripMenuItem.Checked = true;
            }
        }

        private void iblisPhase1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (iblisPhase1ToolStripMenuItem.Checked)
            {
                iblisOne = false;
                iblisPhase1ToolStripMenuItem.Checked = false;
            }
            else
            {
                iblisOne = true;
                iblisPhase1ToolStripMenuItem.Checked = true;
            }
        }

        private void iblisPhase2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (iblisPhase2ToolStripMenuItem.Checked)
            {
                iblisTwo = false;
                iblisPhase2ToolStripMenuItem.Checked = false;
            }
            else
            {
                iblisTwo = true;
                iblisPhase2ToolStripMenuItem.Checked = true;
            }
        }

        private void iblisPhase3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (iblisPhase3ToolStripMenuItem.Checked)
            {
                iblisThree = false;
                iblisPhase3ToolStripMenuItem.Checked = false;
            }
            else
            {
                iblisThree = true;
                iblisPhase3ToolStripMenuItem.Checked = true;
            }
        }

        private void mephilesPhase1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (mephilesPhase1ToolStripMenuItem.Checked)
            {
                mephiles = false;
                mephilesPhase1ToolStripMenuItem.Checked = false;
            }
            else
            {
                mephiles = true;
                mephilesPhase1ToolStripMenuItem.Checked = true;
            }
        }

        private void solarisPhase1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
                return;
            }
            if (solarisPhase1ToolStripMenuItem.Checked)
            {
                solaris = false;
                solarisPhase1ToolStripMenuItem.Checked = false;
            }
            else
            {
                solaris = true;
                solarisPhase1ToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #endregion

        #region Character Checkboxes
        private void sonicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sonicToolStripMenuItem.Checked)
            {
                sonic = false;
                sonicToolStripMenuItem.Checked = false;
            }
            else
            {
                sonic = true;
                sonicToolStripMenuItem.Checked = true;
            }
        }

        private void tailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tailsToolStripMenuItem.Checked)
            {
                tails = false;
                tailsToolStripMenuItem.Checked = false;
            }
            else
            {
                tails = true;
                tailsToolStripMenuItem.Checked = true;
            }
        }

        private void knucklesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (knucklesToolStripMenuItem.Checked)
            {
                knuckles = false;
                knucklesToolStripMenuItem.Checked = false;
            }
            else
            {
                knuckles = true;
                knucklesToolStripMenuItem.Checked = true;
            }
        }

        private void sonicMachSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sonicMachSpeedToolStripMenuItem.Checked)
            {
                sonicMachSpeed = false;
                sonicMachSpeedToolStripMenuItem.Checked = false;
            }
            else
            {
                sonicMachSpeed = true;
                sonicMachSpeedToolStripMenuItem.Checked = true;
            }
        }

        private void sonicAndEliseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sonicAndEliseToolStripMenuItem.Checked)
            {
                sonicElise = false;
                sonicAndEliseToolStripMenuItem.Checked = false;
            }
            else
            {
                sonicElise = true;
                sonicAndEliseToolStripMenuItem.Checked = true;
            }
        }

        private void sonicSnowboardWAPMenuItem_Click(object sender, EventArgs e)
        {
            if (sonicSnowboardWAPMenuItem.Checked)
            {
                sonicSnowboardWAP = false;
                sonicSnowboardWAPMenuItem.Checked = false;
            }
            else
            {
                sonicSnowboardWAP = true;
                sonicSnowboardWAPMenuItem.Checked = true;
            }
        }

        private void sonicSnowboardCSCMenuItem_Click(object sender, EventArgs e)
        {
            if (sonicSnowboardCSCMenuItem.Checked)
            {
                sonicSnowboardCSC = false;
                sonicSnowboardCSCMenuItem.Checked = false;
            }
            else
            {
                sonicSnowboardCSC = true;
                sonicSnowboardCSCMenuItem.Checked = true;
            }
        }

        private void shadowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shadowToolStripMenuItem.Checked)
            {
                shadow = false;
                shadowToolStripMenuItem.Checked = false;
            }
            else
            {
                shadow = true;
                shadowToolStripMenuItem.Checked = true;
            }
        }

        private void rougeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rougeToolStripMenuItem.Checked)
            {
                rouge = false;
                rougeToolStripMenuItem.Checked = false;
            }
            else
            {
                rouge = true;
                rougeToolStripMenuItem.Checked = true;
            }
        }

        private void omegaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (omegaToolStripMenuItem.Checked)
            {
                omega = false;
                omegaToolStripMenuItem.Checked = false;
            }
            else
            {
                omega = true;
                omegaToolStripMenuItem.Checked = true;
            }
        }

        private void silverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (silverToolStripMenuItem.Checked)
            {
                silver = false;
                silverToolStripMenuItem.Checked = false;
            }
            else
            {
                silver = true;
                silverToolStripMenuItem.Checked = true;
            }
        }

        private void blazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (blazeToolStripMenuItem.Checked)
            {
                blaze = false;
                blazeToolStripMenuItem.Checked = false;
            }
            else
            {
                blaze = true;
                blazeToolStripMenuItem.Checked = true;
            }
        }

        private void amyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (amyToolStripMenuItem.Checked)
            {
                amy = false;
                amyToolStripMenuItem.Checked = false;
            }
            else
            {
                amy = true;
                amyToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #region Item Capsule Checkboxes
        private void emptyCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (emptyCapsuleToolStripMenuItem.Checked)
            {
                emptyCapsule = false;
                emptyCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                emptyCapsule = true;
                emptyCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void fiveRingCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fiveRingCapsuleToolStripMenuItem.Checked)
            {
                fiveRingCapsule = false;
                fiveRingCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                fiveRingCapsule = true;
                fiveRingCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void tenRingCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tenRingCapsuleToolStripMenuItem.Checked)
            {
                tenRingCapsule = false;
                tenRingCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                tenRingCapsule = true;
                tenRingCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void twentyRingCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (twentyRingCapsuleToolStripMenuItem.Checked)
            {
                twentyRingCapsule = false;
                twentyRingCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                twentyRingCapsule = true;
                twentyRingCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void extraLifeCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (extraLifeCapsuleToolStripMenuItem.Checked)
            {
                extraLifeCapsule = false;
                extraLifeCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                extraLifeCapsule = true;
                extraLifeCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void powerSneakersCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (powerSneakersCapsuleToolStripMenuItem.Checked)
            {
                powerSneakersCapsule = false;
                powerSneakersCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                powerSneakersCapsule = true;
                powerSneakersCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void powerGaugeRefillCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (powerGaugeRefillCapsuleToolStripMenuItem.Checked)
            {
                powerGaugeRefillCapsule = false;
                powerGaugeRefillCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                powerGaugeRefillCapsule = true;
                powerGaugeRefillCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void invincibilityCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (invincibilityCapsuleToolStripMenuItem.Checked)
            {
                invincibilityCapsule = false;
                invincibilityCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                invincibilityCapsule = true;
                invincibilityCapsuleToolStripMenuItem.Checked = true;
            }
        }

        private void shieldCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (shieldCapsuleToolStripMenuItem.Checked)
            {
                shieldCapsule = false;
                shieldCapsuleToolStripMenuItem.Checked = false;
            }
            else
            {
                shieldCapsule = true;
                shieldCapsuleToolStripMenuItem.Checked = true;
            }
        }
        #endregion

        #region Directory Radio Buttons
        private void customDirButton_CheckedChanged(object sender, EventArgs e)
        {
            if (customDirButton.Checked)
            {
                outputFolderType = 0;
                outputBox.Enabled = true;
                outputButton.Enabled = true;
            }
        }

        private void sourceDirButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sourceDirButton.Checked)
            {
                outputFolderType = 1;
                outputBox.Enabled = false;
                outputButton.Enabled = false;
            }
        }

        private void programDirButton_CheckedChanged(object sender, EventArgs e)
        {
            if (programDirButton.Checked)
            {
                outputFolderType = 2;
                outputBox.Enabled = false;
                outputButton.Enabled = false;
            }
        }
        #endregion

        #region Config Checkboxes
        private void enemiesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (enemiesCheckbox.Checked) { randomEnemies = true; }
            if (!enemiesCheckbox.Checked) { randomEnemies = false; }
        }

        private void charactersCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (charactersCheckbox.Checked) { randomCharacters = true; }
            if (!charactersCheckbox.Checked) { randomCharacters = false; }
        }

        private void itemsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (itemsCheckbox.Checked) { randomItems = true; }
            if (!itemsCheckbox.Checked) { randomItems = false; }
        }

        private void voiceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (voiceCheckbox.Checked) { randomVoices = true; }
            if (!voiceCheckbox.Checked) { randomVoices = false; }
        }

        private void logCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (logCheckbox.Checked) { spoilerLog = true; }
            if (!logCheckbox.Checked) { spoilerLog = false; }
        }

        private void xmlCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (xmlCheckbox.Checked) { keepXML = true; }
            if (!xmlCheckbox.Checked) { keepXML = false; }
        }

        private void folderRandom_CheckedChanged(object sender, EventArgs e)
        {
            filepathBox.Text = "";
            filepath = "";
            if (folderRandom.Checked)
            {
                filepathLabel.Text = "Folder to Randomise:";
                randomiseFolder = true;
            }
            if (!folderRandom.Checked)
            {
                filepathLabel.Text = "XML to Randomise:";
                randomiseFolder = false;
            }
        }
        #endregion

        #region Input Boxes/Buttons
        private void filepathBox_TextChanged(object sender, EventArgs e)
        {
            filepath = filepathBox.Text;
            if (filepathBox.Text == "")
            {
                randomiseButton.Enabled = false;
            }
            else
            {
                randomiseButton.Enabled = true;
            }
        }

        private void filepathButton_Click(object sender, EventArgs e)
        {
            if (!randomiseFolder)
            {
                OpenFileDialog xmlBrowser = new OpenFileDialog();
                xmlBrowser.Title = "Select XML";
                xmlBrowser.Filter = "eXtensible Markup Language file (*.xml)|*.xml|All files (*.*)|*.*";
                xmlBrowser.FilterIndex = 1;
                xmlBrowser.RestoreDirectory = true;
                if (xmlBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = xmlBrowser.FileName;
                    filepathBox.Text = filepath;
                }
            }
            else
            {
                FolderBrowserDialog outputBrowser = new FolderBrowserDialog();
                if (outputBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = outputBrowser.SelectedPath;
                    filepathBox.Text = filepath;
                }
            }
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {
            output = outputBox.Text;
        }

        private void outputButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog outputBrowser = new FolderBrowserDialog();
            if (outputBrowser.ShowDialog() == DialogResult.OK)
            {
                output = outputBrowser.SelectedPath;
                outputBox.Text = output;
            }
        }

        private void seedBox_TextChanged(object sender, EventArgs e)
        {
            rndSeed = seedBox.Text;
        }

        private void seedButton_Click(object sender, EventArgs e)
        {
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }
        #endregion

        private void saveConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog configSave = new SaveFileDialog();
            configSave.Title = "Save Config";
            configSave.Filter = "Randomiser Config (*.s06)|*.s06";
            configSave.FilterIndex = 1;
            configSave.RestoreDirectory = true;
            if (configSave.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(configSave.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.WriteLine("cBiter: " + cBiter);
                    sw.WriteLine("cCrawler: " + cCrawler);
                    sw.WriteLine("cGolem: " + cGolem);
                    sw.WriteLine("cTaker: " + cTaker);

                    sw.WriteLine("cGazer: " + cGazer);
                    sw.WriteLine("cStalker: " + cStalker);
                    sw.WriteLine("cTitan: " + cTitan);
                    sw.WriteLine("cTricker: " + cTricker);

                    sw.WriteLine("eArmor: " + eArmor);
                    sw.WriteLine("eBomber: " + eBomber);
                    sw.WriteLine("eBluster: " + eBluster);
                    sw.WriteLine("eBuster: " + eBuster);
                    sw.WriteLine("eCannon: " + eCannon);
                    sw.WriteLine("eCommander: " + eCommander);
                    sw.WriteLine("eFlyer: " + eFlyer);
                    sw.WriteLine("eGuardian: " + eGuardian);
                    sw.WriteLine("eGunner: " + eGunner);
                    sw.WriteLine("eKeeper: " + eKeeper);
                    sw.WriteLine("eLancer: " + eLancer);
                    sw.WriteLine("eLiner: " + eLiner);
                    sw.WriteLine("eRounder: " + eRounder);
                    sw.WriteLine("eSearcher: " + eSearcher);
                    sw.WriteLine("eStinger: " + eStinger);
                    sw.WriteLine("eSweeper: " + eSweeper);
                    sw.WriteLine("eHunter: " + eHunter);
                    sw.WriteLine("eWalker: " + eWalker);
                    sw.WriteLine("eChaser: " + eChaser);

                    sw.WriteLine("eCerberus: " + eCerberus);
                    sw.WriteLine("eGenesis: " + eGenesis);
                    sw.WriteLine("eWyvern: " + eWyvern);
                    sw.WriteLine("iblisOne: " + iblisOne);
                    sw.WriteLine("iblisTwo: " + iblisTwo);
                    sw.WriteLine("iblisThree: " + iblisThree);
                    sw.WriteLine("mephiles: " + mephiles);
                    sw.WriteLine("solaris: " + solaris);

                    sw.WriteLine("sonic: " + sonic);
                    sw.WriteLine("tails: " + tails);
                    sw.WriteLine("knuckles: " + knuckles);
                    sw.WriteLine("sonicMachSpeed: " + sonicMachSpeed);
                    sw.WriteLine("sonicElise: " + sonicElise);
                    sw.WriteLine("sonicSnowboardWAP: " + sonicSnowboardWAP);
                    sw.WriteLine("sonicSnowboardCSC: " + sonicSnowboardCSC);
                    sw.WriteLine("shadow: " + shadow);
                    sw.WriteLine("rouge: " + rouge);
                    sw.WriteLine("omega: " + omega);
                    sw.WriteLine("silver: " + silver);
                    sw.WriteLine("blaze: " + blaze);
                    sw.WriteLine("amy: " + amy);

                    sw.WriteLine("emptyCapsule: " + emptyCapsule);
                    sw.WriteLine("fiveRingCapsule: " + fiveRingCapsule);
                    sw.WriteLine("tenRingCapsule: " + tenRingCapsule);
                    sw.WriteLine("twentyRingCapsule: " + twentyRingCapsule);
                    sw.WriteLine("extraLifeCapsule: " + extraLifeCapsule);
                    sw.WriteLine("powerSneakersCapsule: " + powerSneakersCapsule);
                    sw.WriteLine("powerGaugeRefillCapsule: " + powerGaugeRefillCapsule);
                    sw.WriteLine("invincibilityCapsule: " + invincibilityCapsule);
                    sw.WriteLine("shieldCapsule: " + shieldCapsule);

                    sw.WriteLine("randomEnemies: " + randomEnemies);
                    sw.WriteLine("randomCharacters: " + randomCharacters);
                    sw.WriteLine("randomItems: " + randomItems);
                    sw.WriteLine("randomVoices: " + randomVoices);
                    sw.WriteLine("spoilerLog: " + spoilerLog);
                    sw.WriteLine("keepXML: " + keepXML);
                    sw.WriteLine("outputFolderType: " + outputFolderType);
                    sw.WriteLine("messageBox: " + messageBox);

                    sw.Close();
                }
            }
        }

        private void loadConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog configLoad = new OpenFileDialog();
            configLoad.Title = "Load Config";
            configLoad.Filter = "Randomiser Config (*.s06)|*.s06";
            configLoad.FilterIndex = 1;
            configLoad.RestoreDirectory = true;
            if (configLoad.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(configLoad.FileName, FileMode.Open))
                using (StreamReader sr = new StreamReader(s))
                {
                    string line;
                    string varName = "";
                    string varValue = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        varName = line.Split(':')[0];
                        varValue = line.Substring(line.IndexOf(":") + 2);
                        switch (varName)
                        {
                            #region Enemies
                            case "cBiter":
                                cBiter = bool.Parse(varValue);
                                cBiterToolStripMenuItem.Checked = cBiter;
                                break;
                            case "cCrawler":
                                cCrawler = bool.Parse(varValue);
                                cCrawlerToolStripMenuItem.Checked = cCrawler;
                                break;
                            case "cGazer":
                                cGazer = bool.Parse(varValue);
                                cGazerToolStripMenuItem.Checked = cGazer;
                                break;
                            case "cGolem":
                                cGolem = bool.Parse(varValue);
                                cGolemToolStripMenuItem.Checked = cGolem;
                                break;
                            case "cStalker":
                                cStalker = bool.Parse(varValue);
                                cStalkerToolStripMenuItem.Checked = cStalker;
                                break;
                            case "cTaker":
                                cTaker = bool.Parse(varValue);
                                cTakerToolStripMenuItem.Checked = cTaker;
                                break;
                            case "cTitan":
                                cTitan = bool.Parse(varValue);
                                cTitanToolStripMenuItem.Checked = cTitan;
                                break;
                            case "cTricker":
                                cTricker = bool.Parse(varValue);
                                cTrickerToolStripMenuItem.Checked = cTricker;
                                break;
                            case "eArmor":
                                eArmor = bool.Parse(varValue);
                                eArmorToolStripMenuItem.Checked = eArmor;
                                break;
                            case "eBomber":
                                eBomber = bool.Parse(varValue);
                                eBomberToolStripMenuItem.Checked = eBomber;
                                break;
                            case "eBluster":
                                eBluster = bool.Parse(varValue);
                                eBlusterToolStripMenuItem.Checked = eBluster;
                                break;
                            case "eBuster":
                                eBuster = bool.Parse(varValue);
                                eBusterToolStripMenuItem.Checked = eBuster;
                                break;
                            case "eCannon":
                                eCannon = bool.Parse(varValue);
                                eCannonToolStripMenuItem.Checked = eCannon;
                                break;
                            case "eCommander":
                                eCommander = bool.Parse(varValue);
                                eCommanderToolStripMenuItem.Checked = eCommander;
                                break;
                            case "eFlyer":
                                eFlyer = bool.Parse(varValue);
                                eFlyerToolStripMenuItem.Checked = eFlyer;
                                break;
                            case "eGuardian":
                                eGuardian = bool.Parse(varValue);
                                eGuardianToolStripMenuItem.Checked = eGuardian;
                                break;
                            case "eGunner":
                                eGunner = bool.Parse(varValue);
                                eGunnerToolStripMenuItem.Checked = eGunner;
                                break;
                            case "eKeeper":
                                eKeeper = bool.Parse(varValue);
                                eKeeperToolStripMenuItem.Checked = eKeeper;
                                break;
                            case "eLancer":
                                eLancer = bool.Parse(varValue);
                                eLancerToolStripMenuItem.Checked = eLancer;
                                break;
                            case "eLiner":
                                eLiner = bool.Parse(varValue);
                                eLinerToolStripMenuItem.Checked = eLiner;
                                break;
                            case "eRounder":
                                eRounder = bool.Parse(varValue);
                                eRounderToolStripMenuItem.Checked = eRounder;
                                break;
                            case "eSearcher":
                                eSearcher = bool.Parse(varValue);
                                eSearcherToolStripMenuItem.Checked = eSearcher;
                                break;
                            case "eStinger":
                                eStinger = bool.Parse(varValue);
                                eStingerToolStripMenuItem.Checked = eStinger;
                                break;
                            case "eSweeper":
                                eSweeper = bool.Parse(varValue);
                                eSweeperToolStripMenuItem.Checked = eSweeper;
                                break;
                            case "eHunter":
                                eHunter = bool.Parse(varValue);
                                eggHunterToolStripMenuItem.Checked = eHunter;
                                break;
                            case "eWalker":
                                eWalker = bool.Parse(varValue);
                                eggWalkerToolStripMenuItem.Checked = eWalker;
                                break;
                            case "eChaser":
                                eChaser = bool.Parse(varValue);
                                eggChaserToolStripMenuItem.Checked = eChaser;
                                break;

                            case "eCerberus":
                                eCerberus = bool.Parse(varValue);
                                eggCerberusToolStripMenuItem.Checked = eCerberus;
                                break;
                            case "eGenesis":
                                eGenesis = bool.Parse(varValue);
                                eggGenesisToolStripMenuItem.Checked = eGenesis;
                                break;
                            case "eWyvern":
                                eWyvern = bool.Parse(varValue);
                                eggWyvernToolStripMenuItem.Checked = eWyvern;
                                break;
                            case "iblisOne":
                                iblisOne = bool.Parse(varValue);
                                iblisPhase1ToolStripMenuItem.Checked = iblisOne;
                                break;
                            case "iblisTwo":
                                iblisTwo = bool.Parse(varValue);
                                iblisPhase2ToolStripMenuItem.Checked = iblisTwo;
                                break;
                            case "iblisThree":
                                iblisThree = bool.Parse(varValue);
                                iblisPhase3ToolStripMenuItem.Checked = iblisThree;
                                break;
                            case "mephiles":
                                mephiles = bool.Parse(varValue);
                                mephilesPhase1ToolStripMenuItem.Checked = mephiles;
                                break;
                            case "solaris":
                                solaris = bool.Parse(varValue);
                                solarisPhase1ToolStripMenuItem.Checked = solaris;
                                break;
                            #endregion

                            #region Characters
                            case "sonic":
                                sonic = bool.Parse(varValue);
                                sonicToolStripMenuItem.Checked = sonic;
                                break;
                            case "tails":
                                tails = bool.Parse(varValue);
                                tailsToolStripMenuItem.Checked = tails;
                                break;
                            case "knuckles":
                                knuckles = bool.Parse(varValue);
                                knucklesToolStripMenuItem.Checked = knuckles;
                                break;
                            case "sonicMachSpeed":
                                sonicMachSpeed = bool.Parse(varValue);
                                sonicMachSpeedToolStripMenuItem.Checked = sonicMachSpeed;
                                break;
                            case "sonicElise":
                                sonicElise = bool.Parse(varValue);
                                sonicAndEliseToolStripMenuItem.Checked = sonicElise;
                                break;
                            case "sonicSnowboardWAP":
                                sonicSnowboardWAP = bool.Parse(varValue);
                                sonicSnowboardWAPMenuItem.Checked = sonicSnowboardWAP;
                                break;
                            case "sonicSnowboardCSC":
                                sonicSnowboardCSC = bool.Parse(varValue);
                                sonicSnowboardCSCMenuItem.Checked = sonicSnowboardCSC;
                                break;
                            case "shadow":
                                shadow = bool.Parse(varValue);
                                shadowToolStripMenuItem.Checked = shadow;
                                break;
                            case "rouge":
                                rouge = bool.Parse(varValue);
                                rougeToolStripMenuItem.Checked = rouge;
                                break;
                            case "omega":
                                omega = bool.Parse(varValue);
                                omegaToolStripMenuItem.Checked = omega;
                                break;
                            case "silver":
                                silver = bool.Parse(varValue);
                                silverToolStripMenuItem.Checked = silver;
                                break;
                            case "blaze":
                                blaze = bool.Parse(varValue);
                                blazeToolStripMenuItem.Checked = blaze;
                                break;
                            case "amy":
                                amy = bool.Parse(varValue);
                                amyToolStripMenuItem.Checked = amy;
                                break;
                            #endregion

                            #region Item Capsules
                            case "emptyCapsule":
                                emptyCapsule = bool.Parse(varValue);
                                emptyCapsuleToolStripMenuItem.Checked = emptyCapsule;
                                break;
                            case "fiveRingCapsule":
                                fiveRingCapsule = bool.Parse(varValue);
                                fiveRingCapsuleToolStripMenuItem.Checked = fiveRingCapsule;
                                break;
                            case "tenRingCapsule":
                                tenRingCapsule = bool.Parse(varValue);
                                tenRingCapsuleToolStripMenuItem.Checked = tenRingCapsule;
                                break;
                            case "twentyRingCapsule":
                                twentyRingCapsule = bool.Parse(varValue);
                                twentyRingCapsuleToolStripMenuItem.Checked = twentyRingCapsule;
                                break;
                            case "extraLifeCapsule":
                                extraLifeCapsule = bool.Parse(varValue);
                                extraLifeCapsuleToolStripMenuItem.Checked = extraLifeCapsule;
                                break;
                            case "powerSneakersCapsule":
                                powerSneakersCapsule = bool.Parse(varValue);
                                powerSneakersCapsuleToolStripMenuItem.Checked = powerSneakersCapsule;
                                break;
                            case "powerGaugeRefillCapsule":
                                powerGaugeRefillCapsule = bool.Parse(varValue);
                                powerGaugeRefillCapsuleToolStripMenuItem.Checked = powerGaugeRefillCapsule;
                                break;
                            case "invincibilityCapsule":
                                invincibilityCapsule = bool.Parse(varValue);
                                invincibilityCapsuleToolStripMenuItem.Checked = invincibilityCapsule;
                                break;
                            case "shieldCapsule":
                                shieldCapsule = bool.Parse(varValue);
                                shieldCapsuleToolStripMenuItem.Checked = shieldCapsule;
                                break;
                            #endregion
                            
                            #region Main Variables
                            case "randomEnemies":
                                randomEnemies = bool.Parse(varValue);
                                enemiesCheckbox.Checked = randomEnemies;
                                break;
                            case "randomCharacters":
                                randomCharacters = bool.Parse(varValue);
                                charactersCheckbox.Checked = randomCharacters;
                                break;
                            case "randomItems":
                                randomItems = bool.Parse(varValue);
                                itemsCheckbox.Checked = randomItems;
                                break;
                            case "randomVoices":
                                randomVoices = bool.Parse(varValue);
                                voiceCheckbox.Checked = randomVoices;
                                break;
                            case "spoilerLog":
                                spoilerLog = bool.Parse(varValue);
                                logCheckbox.Checked = spoilerLog;
                                break;
                            case "keepXML":
                                keepXML = bool.Parse(varValue);
                                xmlCheckbox.Checked = keepXML;
                                break;
                            case "outputFolderType":
                                outputFolderType = int.Parse(varValue);
                                switch (varValue)
                                {
                                    case "0":
                                        customDirButton.Checked = true;
                                        sourceDirButton.Checked = false;
                                        programDirButton.Checked = false;
                                        break;
                                    case "1":
                                        customDirButton.Checked = false;
                                        sourceDirButton.Checked = true;
                                        programDirButton.Checked = false;
                                        break;
                                    case "2":
                                        customDirButton.Checked = false;
                                        sourceDirButton.Checked = false;
                                        programDirButton.Checked = true;
                                        break;
                                    default:
                                        Console.WriteLine("Invalid value in outputFolderType, defaulting to 0");
                                        customDirButton.Checked = true;
                                        sourceDirButton.Checked = false;
                                        programDirButton.Checked = false;
                                        break;
                                }
                                break;
                            case "messageBox":
                                messageBox = bool.Parse(varValue);
                                messageBoxToolStripMenuItem.Checked = messageBox;
                                break;
                                #endregion
                        }
                    }
                }
            }

        }

        private void setExtractorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractSetWindow extractSetForm = new ExtractSetWindow();
            extractSetForm.ShowDialog();
        }

        private void setImporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportSetWindow extractSetForm = new ImportSetWindow();
            extractSetForm.ShowDialog();
        }

        private void loadFromSpoilerLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Loading from Spoiler Log not yet supported.");
        }

        private void randomiseButton_Click(object sender, EventArgs e)
        {
            #region Setup Valid Enemies to pass in
            var validEnemies = new List<string> { };
            if (cBiter) { validEnemies.Add("cBiter"); }
            if (cCrawler) { validEnemies.Add("cCrawler"); }
            if (cGazer) { validEnemies.Add("cGazer"); }
            if (cGolem) { validEnemies.Add("cGolem"); }
            if (cStalker) { validEnemies.Add("cStalker"); }
            if (cTaker) { validEnemies.Add("cTaker"); }
            if (cTitan) { validEnemies.Add("cTitan"); }
            if (cTricker) { validEnemies.Add("cTricker"); }
            if (eArmor) { validEnemies.Add("eArmor"); }
            if (eBomber) { validEnemies.Add("eBomber"); }
            if (eBluster) { validEnemies.Add("eBluster"); }
            if (eBuster) { validEnemies.Add("eBuster"); }
            if (eCannon) { validEnemies.Add("eCannon"); }
            if (eCommander) { validEnemies.Add("eCommander"); }
            if (eFlyer) { validEnemies.Add("eFlyer"); }
            if (eGuardian) { validEnemies.Add("eGuardian"); }
            if (eGunner) { validEnemies.Add("eGunner"); }
            if (eKeeper) { validEnemies.Add("eKeeper"); }
            if (eLancer) { validEnemies.Add("eLancer"); }
            if (eLiner) { validEnemies.Add("eLiner"); }
            if (eRounder) { validEnemies.Add("eRounder"); }
            if (eSearcher) { validEnemies.Add("eSearcher"); }
            if (eStinger) { validEnemies.Add("eStinger"); }
            if (eSweeper) { validEnemies.Add("eSweeper"); }
            if (eHunter) { validEnemies.Add("eHunter"); }
            if (eWalker) { validEnemies.Add("eWalker"); }
            if (eChaser) { validEnemies.Add("eChaser"); }

            if (eCerberus) { validEnemies.Add("eCerberus"); }
            if (eGenesis) { validEnemies.Add("eGenesis"); }
            if (eWyvern) { validEnemies.Add("eWyvern"); }
            if (iblisOne) { validEnemies.Add("firstiblis"); }
            if (iblisTwo) { validEnemies.Add("secondiblis"); }
            if (iblisThree) { validEnemies.Add("thirdiblis"); }
            if (mephiles) { validEnemies.Add("firstmefiress"); }
            if (solaris) { validEnemies.Add("solaris01"); }
            #endregion

            #region Setup Valid Characters to pass in
            var validCharacters = new List<string> { };
            if (sonic) { validCharacters.Add("sonic_new"); }
            if (tails) { validCharacters.Add("tails"); }
            if (knuckles) { validCharacters.Add("knuckles"); }
            if (sonicMachSpeed) { validCharacters.Add("sonic_fast"); }
            if (sonicElise) { validCharacters.Add("princess"); }
            if (sonicSnowboardWAP) { validCharacters.Add("snow_board_wap"); }
            if (sonicSnowboardCSC) { validCharacters.Add("snow_board"); }
            if (shadow) { validCharacters.Add("shadow"); }
            if (rouge) { validCharacters.Add("rouge"); }
            if (omega) { validCharacters.Add("omega"); }
            if (silver) { validCharacters.Add("silver"); }
            if (blaze) { validCharacters.Add("blaze"); }
            if (amy) { validCharacters.Add("amy"); }
            #endregion

            #region Setup Valid Item Capsules to pass in
            var validItems = new List<string> { };
            if (emptyCapsule) { validItems.Add("0"); }
            if (fiveRingCapsule) { validItems.Add("1"); }
            if (tenRingCapsule) { validItems.Add("2"); }
            if (twentyRingCapsule) { validItems.Add("3"); }
            if (extraLifeCapsule) { validItems.Add("4"); }
            if (powerSneakersCapsule) { validItems.Add("5"); }
            if (powerGaugeRefillCapsule) { validItems.Add("6"); }
            if (invincibilityCapsule) { validItems.Add("7"); }
            if (shieldCapsule) { validItems.Add("8"); }
            #endregion

            Randomisation.SetupRandomiser(randomEnemies, validEnemies, randomCharacters, validCharacters, randomItems, validItems, randomVoices, spoilerLog, keepXML, randomiseFolder, outputFolderType, filepath, output, rndSeed);
            if (messageBox) { MessageBox.Show("Randomisation of " + filepath + " complete."); }
        }

        private void messageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (messageBoxToolStripMenuItem.Checked)
            {
                messageBox = false;
                messageBoxToolStripMenuItem.Checked = false;
            }
            else
            {
                messageBox = true;
                messageBoxToolStripMenuItem.Checked = true;
            }
        }
    }
}
