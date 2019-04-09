using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sonic06Randomiser
{
    public partial class LUARandomiserWindow : Form
    {
        //Setup Variables
        #region Music Variables
        bool boss_cerberus = true;
        bool boss_character = true;
        bool boss_iblis = true;
        bool boss_iblis3 = true;
        bool boss_mefires1 = true;
        bool boss_mefires3 = true;
        bool boss_solaris1 = true;
        bool boss_solaris2 = true;
        bool boss_wyvern = true;
        bool extra = true;
        bool menu = true;
        bool result = true;
        bool select = true;
        bool stg_aqa_a = true;
        bool stg_aqa_b = true;
        bool stg_csc_a = true;
        bool stg_csc_b = true;
        bool stg_csc_e = true;
        bool stg_csc_f = true;
        bool stg_dtd_a = true;
        bool stg_dtd_b = true;
        bool stg_end_a = true;
        bool stg_end_b = true;
        bool stg_end_c = true;
        bool stg_end_d = true;
        bool stg_end_e = true;
        bool stg_end_f = true;
        bool stg_end_g = true;
        bool stg_flc_a = true;
        bool stg_flc_b = true;
        bool stg_kdv_a = true;
        bool stg_kdv_b = true;
        bool stg_kdv_c = true;
        bool stg_kdv_d = true;
        bool stg_rct_a = true;
        bool stg_rct_b = true;
        bool stg_tpj_a = true;
        bool stg_tpj_b = true;
        bool stg_tpj_c = true;
        bool stg_twn_a = true;
        bool stg_twn_b = true;
        bool stg_twn_c = true;
        bool stg_twn_shop = true;
        bool stg_wap_a = true;
        bool stg_wap_b = true;
        bool stg_wvo_a = true;
        bool stg_wvo_b = true;
        bool twn_accordion = true;
        bool twn_mission_comical = true;
        bool twn_mission_fast = true;
        bool twn_mission_slow = true;
        #endregion

        #region Main Variables
        bool randomMusic = false;
        bool lightColours = false;
        bool lightDirection = false;
        bool spoilerLog = false;
        bool randomiseFolder = false;
        int outputFolderType = 0; //0 = Custom, 1 = Source, 2 = Program
        string filepath = "";
        string output = "";
        Random rnd = new Random();
        string rndSeed = "";
        bool messageBox = true;
        #endregion

        public LUARandomiserWindow()
        {
            InitializeComponent();
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }

        private void randomiseButton_Click(object sender, EventArgs e)
        {
            var validMusic = new List<string> { };
            if (boss_cerberus) { validMusic.Add("boss_cerberus"); }
            if (boss_character) { validMusic.Add("boss_character"); }
            if (boss_iblis) { validMusic.Add("boss_iblis"); }
            if (boss_iblis3) { validMusic.Add("boss_iblis3"); }
            if (boss_mefires1) { validMusic.Add("boss_mefires1"); }
            if (boss_mefires3) { validMusic.Add("boss_mefires3"); }
            if (boss_solaris1) { validMusic.Add("boss_solaris1"); }
            if (boss_solaris2) { validMusic.Add("boss_solaris2"); }
            if (boss_wyvern) { validMusic.Add("boss_wyvern"); }
            if (extra) { validMusic.Add("extra"); }
            if (menu) { validMusic.Add("menu"); }
            if (result) { validMusic.Add("result"); }
            if (select) { validMusic.Add("select"); }
            if (stg_aqa_a) { validMusic.Add("stg_aqa_a"); }
            if (stg_aqa_b) { validMusic.Add("stg_aqa_b"); }
            if (stg_csc_a) { validMusic.Add("stg_csc_a"); }
            if (stg_csc_b) { validMusic.Add("stg_csc_b"); }
            if (stg_csc_e) { validMusic.Add("stg_csc_e"); }
            if (stg_csc_f) { validMusic.Add("stg_csc_f"); }
            if (stg_dtd_a) { validMusic.Add("stg_dtd_a"); }
            if (stg_dtd_b) { validMusic.Add("stg_dtd_b"); }
            if (stg_end_a) { validMusic.Add("stg_end_a"); }
            if (stg_end_b) { validMusic.Add("stg_end_b"); }
            if (stg_end_c) { validMusic.Add("stg_end_c"); }
            if (stg_end_d) { validMusic.Add("stg_end_d"); }
            if (stg_end_e) { validMusic.Add("stg_end_e"); }
            if (stg_end_f) { validMusic.Add("stg_end_f"); }
            if (stg_end_g) { validMusic.Add("stg_end_g"); }
            if (stg_flc_a) { validMusic.Add("stg_flc_a"); }
            if (stg_flc_b) { validMusic.Add("stg_flc_b"); }
            if (stg_kdv_a) { validMusic.Add("stg_kdv_a"); }
            if (stg_kdv_b) { validMusic.Add("stg_kdv_b"); }
            if (stg_kdv_c) { validMusic.Add("stg_kdv_c"); }
            if (stg_kdv_d) { validMusic.Add("stg_kdv_d"); }
            if (stg_rct_a) { validMusic.Add("stg_rct_a"); }
            if (stg_rct_b) { validMusic.Add("stg_rct_b"); }
            if (stg_tpj_a) { validMusic.Add("stg_tpj_a"); }
            if (stg_tpj_b) { validMusic.Add("stg_tpj_b"); }
            if (stg_tpj_c) { validMusic.Add("stg_tpj_c"); }
            if (stg_twn_a) { validMusic.Add("stg_twn_a"); }
            if (stg_twn_b) { validMusic.Add("stg_twn_b"); }
            if (stg_twn_c) { validMusic.Add("stg_twn_c"); }
            if (stg_twn_shop) { validMusic.Add("stg_twn_shop"); }
            if (stg_wap_a) { validMusic.Add("stg_wap_a"); }
            if (stg_wap_b) { validMusic.Add("stg_wap_b"); }
            if (stg_wvo_a) { validMusic.Add("stg_wvo_a"); }
            if (stg_wvo_b) { validMusic.Add("stg_wvo_b"); }
            if (twn_accordion) { validMusic.Add("twn_accordion"); }
            if (twn_mission_comical) { validMusic.Add("twn_mission_comical"); }
            if (twn_mission_fast) { validMusic.Add("twn_mission_fast"); }
            if (twn_mission_slow) { validMusic.Add("twn_mission_slow"); }
            LUARandomisation.SetupRandomiser(randomMusic, validMusic, lightColours, lightDirection, spoilerLog, randomiseFolder, outputFolderType, filepath, output, rndSeed);
            if (messageBox) { MessageBox.Show("Randomisation of " + filepath + " complete."); }
        }

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
                xmlBrowser.Title = "Select LUB";
                xmlBrowser.Filter = "Decompiled LUA Binary (*.lub)|*.lub|All files (*.*)|*.*";
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

        #region Main Checkboxes
        private void logCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (logCheckbox.Checked) { spoilerLog = true; }
            if (!logCheckbox.Checked) { spoilerLog = false; }
        }

        private void messageBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            messageBox = ((ToolStripMenuItem)sender).Checked;
        }

        private void musicCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            randomMusic = ((CheckBox)sender).Checked;
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
                filepathLabel.Text = "LUB to Randomise:";
                randomiseFolder = false;
            }
        }

        private void lightingCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (lightColoursCheckbox.Checked) { lightColours = true; }
            if (!lightColoursCheckbox.Checked) { lightColours = false; }
        }

        private void lightDirectionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (lightDirectionCheckbox.Checked) { lightDirection = true; }
            if (!lightDirectionCheckbox.Checked) { lightDirection = false; }
        }
        #endregion

        #region Stage Music
        private void stg_aqa_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_aqa_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_aqa_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_aqa_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_csc_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_csc_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_csc_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_csc_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_csc_eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_csc_e = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_csc_fToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_csc_f = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_dtd_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_dtd_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_dtd_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_dtd_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_c = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_d = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_e = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_fToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_f = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_end_gToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_end_g = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_flc_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_flc_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_flc_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_flc_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_kdv_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_kdv_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_kdv_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_kdv_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_kdv_dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_kdv_d = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_kdv_cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_kdv_c = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_rct_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_rct_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_rct_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_rct_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_tpj_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_tpj_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_tpj_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_tpj_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_tpj_cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_tpj_c = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_wap_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_wap_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_wap_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_wap_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_wvo_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_wvo_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_wvo_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_wvo_b = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Boss Music
        private void boss_characterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_character = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_cerberusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_cerberus = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_wyvernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_wyvern = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_iblisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_iblis = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_iblis3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_iblis3 = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_mefires1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_mefires1 = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_mefires3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_mefires3 = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_solaris1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_solaris1 = ((ToolStripMenuItem)sender).Checked;
        }

        private void boss_solaris2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            boss_solaris2 = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Soleanna Music
        private void stg_twn_aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_twn_a = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_twn_bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_twn_b = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_twn_cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_twn_c = ((ToolStripMenuItem)sender).Checked;
        }

        private void twn_accordionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            twn_accordion = ((ToolStripMenuItem)sender).Checked;
        }

        private void stg_twn_shopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stg_twn_shop = ((ToolStripMenuItem)sender).Checked;
        }

        private void twn_mission_slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            twn_mission_slow = ((ToolStripMenuItem)sender).Checked;
        }

        private void twn_mission_comicalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            twn_mission_comical = ((ToolStripMenuItem)sender).Checked;
        }

        private void twn_mission_fastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            twn_mission_fast = ((ToolStripMenuItem)sender).Checked;
        }
        #endregion

        #region Menu Music
        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menu = ((ToolStripMenuItem)sender).Checked;
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            select = ((ToolStripMenuItem)sender).Checked;
        }

        private void extraStripMenuItem_Click(object sender, EventArgs e)
        {
            extra = ((ToolStripMenuItem)sender).Checked;
        }

        private void resultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            result = ((ToolStripMenuItem)sender).Checked;
        }

        #endregion

        #region Config Buttons
        private void saveConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog configSave = new SaveFileDialog();
            configSave.Title = "Save Config";
            configSave.Filter = "LUA Randomiser Config (*.s07)|*.s07";
            configSave.FilterIndex = 1;
            configSave.RestoreDirectory = true;
            if (configSave.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(configSave.FileName, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.WriteLine("boss_cerberus: " + boss_cerberus);
                    sw.WriteLine("boss_character: " + boss_character);
                    sw.WriteLine("boss_iblis: " + boss_iblis);
                    sw.WriteLine("boss_iblis3: " + boss_iblis3);
                    sw.WriteLine("boss_mefires1: " + boss_mefires1);
                    sw.WriteLine("boss_mefires3: " + boss_mefires3);
                    sw.WriteLine("boss_solaris1: " + boss_solaris1);
                    sw.WriteLine("boss_solaris2: " + boss_solaris2);
                    sw.WriteLine("boss_wyvern: " + boss_wyvern);
                    sw.WriteLine("extra: " + extra);
                    sw.WriteLine("menu: " + menu);
                    sw.WriteLine("result: " + result);
                    sw.WriteLine("select: " + select);
                    sw.WriteLine("stg_aqa_a: " + stg_aqa_a);
                    sw.WriteLine("stg_aqa_b: " + stg_aqa_b);
                    sw.WriteLine("stg_csc_a: " + stg_csc_a);
                    sw.WriteLine("stg_csc_b: " + stg_csc_b);
                    sw.WriteLine("stg_csc_e: " + stg_csc_e);
                    sw.WriteLine("stg_csc_f: " + stg_csc_f);
                    sw.WriteLine("stg_dtd_a: " + stg_dtd_a);
                    sw.WriteLine("stg_dtd_b: " + stg_dtd_b);
                    sw.WriteLine("stg_end_a: " + stg_end_a);
                    sw.WriteLine("stg_end_b: " + stg_end_b);
                    sw.WriteLine("stg_end_c: " + stg_end_c);
                    sw.WriteLine("stg_end_d: " + stg_end_d);
                    sw.WriteLine("stg_end_e: " + stg_end_e);
                    sw.WriteLine("stg_end_f: " + stg_end_f);
                    sw.WriteLine("stg_end_g: " + stg_end_g);
                    sw.WriteLine("stg_flc_a: " + stg_flc_a);
                    sw.WriteLine("stg_flc_b: " + stg_flc_b);
                    sw.WriteLine("stg_kdv_a: " + stg_kdv_a);
                    sw.WriteLine("stg_kdv_b: " + stg_kdv_b);
                    sw.WriteLine("stg_kdv_c: " + stg_kdv_c);
                    sw.WriteLine("stg_kdv_d: " + stg_kdv_d);
                    sw.WriteLine("stg_rct_a: " + stg_rct_a);
                    sw.WriteLine("stg_rct_b: " + stg_rct_b);
                    sw.WriteLine("stg_tpj_a: " + stg_tpj_a);
                    sw.WriteLine("stg_tpj_b: " + stg_tpj_b);
                    sw.WriteLine("stg_tpj_c: " + stg_tpj_c);
                    sw.WriteLine("stg_twn_a: " + stg_twn_a);
                    sw.WriteLine("stg_twn_b: " + stg_twn_b);
                    sw.WriteLine("stg_twn_c: " + stg_twn_c);
                    sw.WriteLine("stg_twn_shop: " + stg_twn_shop);
                    sw.WriteLine("stg_wap_a: " + stg_wap_a);
                    sw.WriteLine("stg_wap_b: " + stg_wap_b);
                    sw.WriteLine("stg_wvo_a: " + stg_wvo_a);
                    sw.WriteLine("stg_wvo_b: " + stg_wvo_b);
                    sw.WriteLine("twn_accordion: " + twn_accordion);
                    sw.WriteLine("twn_mission_comical: " + twn_mission_comical);
                    sw.WriteLine("twn_mission_fast: " + twn_mission_fast);
                    sw.WriteLine("twn_mission_slow: " + twn_mission_slow);
                    
                    sw.WriteLine("randomMusic: " + randomMusic);
                    sw.WriteLine("lightColours: " + lightColours);
                    sw.WriteLine("lightDirection: " + lightDirection);
                    sw.WriteLine("spoilerLog: " + spoilerLog);
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
            configLoad.Filter = "LUA Randomiser Config (*.s07)|*.s07";
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
                            #region Music Variables
                            case "boss_cerberus":
                                boss_cerberus = bool.Parse(varValue);
                                boss_cerberusToolStripMenuItem.Checked = boss_cerberus;
                                break;
                            case "boss_character":
                                boss_character = bool.Parse(varValue);
                                boss_characterToolStripMenuItem.Checked = boss_character;
                                break;
                            case "boss_iblis":
                                boss_iblis = bool.Parse(varValue);
                                boss_iblisToolStripMenuItem.Checked = boss_iblis;
                                break;
                            case "boss_iblis3":
                                boss_iblis3 = bool.Parse(varValue);
                                boss_iblis3ToolStripMenuItem.Checked = boss_iblis3;
                                break;
                            case "boss_mefires1":
                                boss_mefires1 = bool.Parse(varValue);
                                boss_mefires1ToolStripMenuItem.Checked = boss_mefires1;
                                break;
                            case "boss_mefires3":
                                boss_mefires3 = bool.Parse(varValue);
                                boss_mefires3ToolStripMenuItem.Checked = boss_mefires3;
                                break;
                            case "boss_solaris1":
                                boss_solaris1 = bool.Parse(varValue);
                                boss_solaris1ToolStripMenuItem.Checked = boss_solaris1;
                                break;
                            case "boss_solaris2":
                                boss_solaris2 = bool.Parse(varValue);
                                boss_solaris2ToolStripMenuItem.Checked = boss_solaris2;
                                break;
                            case "boss_wyvern":
                                boss_wyvern = bool.Parse(varValue);
                                boss_wyvernToolStripMenuItem.Checked = boss_wyvern;
                                break;
                            case "menu":
                                menu = bool.Parse(varValue);
                                menuToolStripMenuItem.Checked = menu;
                                break;
                            case "extra":
                                extra = bool.Parse(varValue);
                                extraStripMenuItem.Checked = extra;
                                break;
                            case "result":
                                result = bool.Parse(varValue);
                                resultToolStripMenuItem.Checked = result;
                                break;
                            case "select":
                                select = bool.Parse(varValue);
                                selectToolStripMenuItem.Checked = select;
                                break;
                            case "stg_aqa_a":
                                stg_aqa_a = bool.Parse(varValue);
                                stg_aqa_aToolStripMenuItem.Checked = stg_aqa_a;
                                break;
                            case "stg_aqa_b":
                                stg_aqa_b = bool.Parse(varValue);
                                stg_aqa_bToolStripMenuItem.Checked = stg_aqa_b;
                                break;
                            case "stg_csc_a":
                                stg_csc_a = bool.Parse(varValue);
                                stg_csc_aToolStripMenuItem.Checked = stg_csc_a;
                                break;
                            case "stg_csc_b":
                                stg_csc_b = bool.Parse(varValue);
                                stg_csc_bToolStripMenuItem.Checked = stg_csc_b;
                                break;
                            case "stg_csc_e":
                                stg_csc_e = bool.Parse(varValue);
                                stg_csc_eToolStripMenuItem.Checked = stg_csc_e;
                                break;
                            case "stg_csc_f":
                                stg_csc_f = bool.Parse(varValue);
                                stg_csc_fToolStripMenuItem.Checked = stg_csc_f;
                                break;
                            case "stg_dtd_a":
                                stg_dtd_a = bool.Parse(varValue);
                                stg_dtd_aToolStripMenuItem.Checked = stg_dtd_a;
                                break;
                            case "stg_dtd_b":
                                stg_dtd_b = bool.Parse(varValue);
                                stg_dtd_bToolStripMenuItem.Checked = stg_dtd_b;
                                break;
                            case "stg_end_a":
                                stg_end_a = bool.Parse(varValue);
                                stg_end_aToolStripMenuItem.Checked = stg_end_a;
                                break;
                            case "stg_end_b":
                                stg_end_b = bool.Parse(varValue);
                                stg_end_bToolStripMenuItem.Checked = stg_end_b;
                                break;
                            case "stg_end_c":
                                stg_end_c = bool.Parse(varValue);
                                stg_end_cToolStripMenuItem.Checked = stg_end_c;
                                break;
                            case "stg_end_d":
                                stg_end_d = bool.Parse(varValue);
                                stg_end_dToolStripMenuItem.Checked = stg_end_d;
                                break;
                            case "stg_end_e":
                                stg_end_e = bool.Parse(varValue);
                                stg_end_eToolStripMenuItem.Checked = stg_end_e;
                                break;
                            case "stg_end_f":
                                stg_end_f = bool.Parse(varValue);
                                stg_end_fToolStripMenuItem.Checked = stg_end_f;
                                break;
                            case "stg_end_g":
                                stg_end_g = bool.Parse(varValue);
                                stg_end_gToolStripMenuItem.Checked = stg_end_g;
                                break;
                            case "stg_flc_a":
                                stg_flc_a = bool.Parse(varValue);
                                stg_flc_aToolStripMenuItem.Checked = stg_flc_a;
                                break;
                            case "stg_flc_b":
                                stg_flc_b = bool.Parse(varValue);
                                stg_flc_bToolStripMenuItem.Checked = stg_flc_b;
                                break;
                            case "stg_kdv_a":
                                stg_kdv_a = bool.Parse(varValue);
                                stg_kdv_aToolStripMenuItem.Checked = stg_kdv_a;
                                break;
                            case "stg_kdv_b":
                                stg_kdv_b = bool.Parse(varValue);
                                stg_kdv_bToolStripMenuItem.Checked = stg_kdv_b;
                                break;
                            case "stg_kdv_c":
                                stg_kdv_c = bool.Parse(varValue);
                                stg_kdv_cToolStripMenuItem.Checked = stg_kdv_c;
                                break;
                            case "stg_kdv_d":
                                stg_kdv_d = bool.Parse(varValue);
                                stg_kdv_dToolStripMenuItem.Checked = stg_kdv_d;
                                break;
                            case "stg_rct_a":
                                stg_rct_a = bool.Parse(varValue);
                                stg_rct_aToolStripMenuItem.Checked = stg_rct_a;
                                break;
                            case "stg_rct_b":
                                stg_rct_b = bool.Parse(varValue);
                                stg_rct_bToolStripMenuItem.Checked = stg_rct_b;
                                break;
                            case "stg_tpj_a":
                                stg_tpj_a = bool.Parse(varValue);
                                stg_tpj_aToolStripMenuItem.Checked = stg_tpj_a;
                                break;
                            case "stg_tpj_b":
                                stg_tpj_b = bool.Parse(varValue);
                                stg_tpj_bToolStripMenuItem.Checked = stg_tpj_b;
                                break;
                            case "stg_tpj_c":
                                stg_tpj_c = bool.Parse(varValue);
                                stg_tpj_cToolStripMenuItem.Checked = stg_tpj_c;
                                break;
                            case "stg_wap_a":
                                stg_wap_a = bool.Parse(varValue);
                                stg_wap_aToolStripMenuItem.Checked = stg_wap_a;
                                break;
                            case "stg_wap_b":
                                stg_wap_b = bool.Parse(varValue);
                                stg_wap_bToolStripMenuItem.Checked = stg_wap_b;
                                break;
                            case "stg_wvo_a":
                                stg_wvo_a = bool.Parse(varValue);
                                stg_wvo_aToolStripMenuItem.Checked = stg_wvo_a;
                                break;
                            case "stg_wvo_b":
                                stg_wvo_b = bool.Parse(varValue);
                                stg_wvo_bToolStripMenuItem.Checked = stg_wvo_b;
                                break;
                            case "stg_twn_a":
                                stg_twn_a = bool.Parse(varValue);
                                stg_twn_aToolStripMenuItem.Checked = stg_twn_a;
                                break;
                            case "stg_twn_b":
                                stg_twn_b = bool.Parse(varValue);
                                stg_twn_bToolStripMenuItem.Checked = stg_twn_b;
                                break;
                            case "stg_twn_c":
                                stg_twn_c = bool.Parse(varValue);
                                stg_twn_cToolStripMenuItem.Checked = stg_twn_c;
                                break;
                            case "stg_twn_shop":
                                stg_twn_shop = bool.Parse(varValue);
                                stg_twn_shopToolStripMenuItem.Checked = stg_twn_shop;
                                break;
                            case "twn_accordion":
                                twn_accordion = bool.Parse(varValue);
                                twn_accordionToolStripMenuItem.Checked = twn_accordion;
                                break;
                            case "twn_mission_comical":
                                twn_mission_comical = bool.Parse(varValue);
                                twn_mission_comicalToolStripMenuItem.Checked = twn_mission_comical;
                                break;
                            case "twn_mission_fast":
                                twn_mission_fast = bool.Parse(varValue);
                                twn_mission_fastToolStripMenuItem.Checked = twn_mission_fast;
                                break;
                            case "twn_mission_slow":
                                twn_mission_slow = bool.Parse(varValue);
                                twn_mission_slowToolStripMenuItem.Checked = twn_mission_slow;
                                break;
                            #endregion

                            #region Main Variables
                            case "randomMusic":
                                randomMusic = bool.Parse(varValue);
                                musicCheckbox.Checked = randomMusic;
                                break;
                            case "lightColours":
                                lightColours = bool.Parse(varValue);
                                lightColoursCheckbox.Checked = lightColours;
                                break;
                            case "lightDirection":
                                lightDirection = bool.Parse(varValue);
                                lightDirectionCheckbox.Checked = lightDirection;
                                break;
                            case "spoilerLog":
                                spoilerLog = bool.Parse(varValue);
                                logCheckbox.Checked = spoilerLog;
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
        #endregion
    }
}
