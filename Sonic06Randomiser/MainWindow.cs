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
            cBiter = ((ToolStripMenuItem)sender).Checked;
        }

        private void cCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cCrawler = ((ToolStripMenuItem)sender).Checked;
        }

        private void cGolemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cGolem = ((ToolStripMenuItem)sender).Checked;
        }

        private void cTakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cTaker = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Mephiles Monsters
        private void cGazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cGazer = ((ToolStripMenuItem)sender).Checked;
        }

        private void cStalkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cStalker = ((ToolStripMenuItem)sender).Checked;
        }

        private void cTitanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cTitan = ((ToolStripMenuItem)sender).Checked;
        }

        private void cTrickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cTricker = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Eggman Robots
        private void eArmorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eArmor = ((ToolStripMenuItem)sender).Checked;
        }

        private void eBomberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eBomber = ((ToolStripMenuItem)sender).Checked;
        }

        private void eBlusterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eBluster = ((ToolStripMenuItem)sender).Checked;
        }

        private void eBusterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eBuster = ((ToolStripMenuItem)sender).Checked;
        }

        private void eCannonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eCannon = ((ToolStripMenuItem)sender).Checked;
        }

        private void eCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eCommander = ((ToolStripMenuItem)sender).Checked;
        }

        private void eFlyerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eFlyer = ((ToolStripMenuItem)sender).Checked;
        }

        private void eGuardianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eGuardian = ((ToolStripMenuItem)sender).Checked;
        }

        private void eGunnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eGunner = ((ToolStripMenuItem)sender).Checked;
        }

        private void eKeeperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eKeeper = ((ToolStripMenuItem)sender).Checked;
        }

        private void eLancerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eLancer = ((ToolStripMenuItem)sender).Checked;
        }

        private void eLinerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eLiner = ((ToolStripMenuItem)sender).Checked;
        }

        private void eRounderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eRounder = ((ToolStripMenuItem)sender).Checked;
        }

        private void eSearcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSearcher = ((ToolStripMenuItem)sender).Checked;
        }

        private void eStingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eStinger = ((ToolStripMenuItem)sender).Checked;
        }

        private void eSweeperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eSweeper = ((ToolStripMenuItem)sender).Checked;
        }

        private void eggWalkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eWalker = ((ToolStripMenuItem)sender).Checked;
        }

        private void eggHunterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eHunter = ((ToolStripMenuItem)sender).Checked;
        }

        private void eggChaserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eChaser = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Bosses
        private void eggCerberusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            eCerberus = ((ToolStripMenuItem)sender).Checked;
        }

        private void eggGenesisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            eGenesis = ((ToolStripMenuItem)sender).Checked;
        }

        private void eggWyvernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            eWyvern = ((ToolStripMenuItem)sender).Checked;
        }

        private void iblisPhase1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            iblisOne = ((ToolStripMenuItem)sender).Checked;
        }

        private void iblisPhase2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            iblisTwo = ((ToolStripMenuItem)sender).Checked;
        }

        private void iblisPhase3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            iblisThree = ((ToolStripMenuItem)sender).Checked;
        }

        private void mephilesPhase1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            mephiles = ((ToolStripMenuItem)sender).Checked;
        }

        private void solarisPhase1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!bossWarningShown)
            {
                MessageBox.Show("Bosses can cause more stability problems than usual, especially on real hardware, take caution in using them.");
                bossWarningShown = true;
            }
            solaris = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #endregion

        #region Character Checkboxes
        private void sonicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonic = ((ToolStripMenuItem)sender).Checked;
        }

        private void tailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tails = ((ToolStripMenuItem)sender).Checked;
        }

        private void knucklesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            knuckles = ((ToolStripMenuItem)sender).Checked;
        }

        private void sonicMachSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonicMachSpeed = ((ToolStripMenuItem)sender).Checked;
        }

        private void sonicAndEliseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sonicElise = ((ToolStripMenuItem)sender).Checked;
        }

        private void sonicSnowboardWAPMenuItem_Click(object sender, EventArgs e)
        {
            sonicSnowboardWAP = ((ToolStripMenuItem)sender).Checked;
        }

        private void sonicSnowboardCSCMenuItem_Click(object sender, EventArgs e)
        {
            sonicSnowboardCSC = ((ToolStripMenuItem)sender).Checked;
        }

        private void shadowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shadow = ((ToolStripMenuItem)sender).Checked;
        }

        private void rougeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rouge = ((ToolStripMenuItem)sender).Checked;
        }

        private void omegaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            omega = ((ToolStripMenuItem)sender).Checked;
        }

        private void silverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            silver = ((ToolStripMenuItem)sender).Checked;
        }

        private void blazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blaze = ((ToolStripMenuItem)sender).Checked;
        }

        private void amyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amy = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Item Capsule Checkboxes
        private void emptyCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emptyCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void fiveRingCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fiveRingCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void tenRingCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tenRingCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void twentyRingCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            twentyRingCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void extraLifeCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extraLifeCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void powerSneakersCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            powerSneakersCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void powerGaugeRefillCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            powerGaugeRefillCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void invincibilityCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            invincibilityCapsule = ((ToolStripMenuItem)sender).Checked;
        }

        private void shieldCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shieldCapsule = ((ToolStripMenuItem)sender).Checked;
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
                xmlBrowser.Filter = "Extracted XML or Original Set (*.xml, *.set)|*.xml;*.set|eXtensible Markup Language file (*.xml)|*.xml|SONIC THE HEDGEHOG (2006) SET file (*.set)|*.set|All files (*.*)|*.*";
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

            SetRandomisation.SetupRandomiser(randomEnemies, validEnemies, randomCharacters, validCharacters, randomItems, validItems, randomVoices, spoilerLog, keepXML, randomiseFolder, outputFolderType, filepath, output, rndSeed);
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

        private void lUARandomiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LUARandomiserWindow LUARandomiserWindow = new LUARandomiserWindow();
            LUARandomiserWindow.ShowDialog();
        }
    }
}
