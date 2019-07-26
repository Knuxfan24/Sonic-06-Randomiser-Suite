using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    public partial class MusicRandomisationForm : Form
    {
        string filepath;
        Random rng = new Random();
        string rngSeed;
        bool randomiseFolder = false;
        List<string> validMusic = new List<string> { };
        public MusicRandomisationForm()
        {
            InitializeComponent();
            filepathLabel.Text = "LUA to Randomise:";
            rngSeed = rng.Next().ToString();
            seedBox.Text = rngSeed;

            for (int i = 0; i < musicConfigList.Items.Count; i++)
            {
                musicConfigList.SetItemChecked(i, true);
            }
        }

        private void FilepathBox_TextChanged(object sender, EventArgs e)
        {
            filepath = filepathBox.Text;
        }

        private void FilepathButton_Click(object sender, EventArgs e)
        {
            if (!randomiseFolder)
            {
                OpenFileDialog setBrowser = new OpenFileDialog();
                setBrowser.Title = "Select SET Data";
                setBrowser.Filter = "SONIC THE HEDGEHOG (2006) Stage LUA (*.lub)|a_*.lub;b_*.lub;c_*.lub;d_*.lub;e_*.lub;f_*.lub;f1_*.lub;f2_*.lub;g_*.lub";
                setBrowser.FilterIndex = 1;
                setBrowser.RestoreDirectory = true;
                if (setBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = setBrowser.FileName;
                    filepathBox.Text = filepath;
                }
            }
            else
            {
                FolderBrowserDialog setFolderBrowser = new FolderBrowserDialog();
                if (setFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = setFolderBrowser.SelectedPath;
                    filepathBox.Text = filepath;
                }
            }
        }

        private void SeedBox_TextChanged(object sender, EventArgs e)
        {
            rngSeed = seedBox.Text;
        }

        private void SeedButton_Click(object sender, EventArgs e)
        {
            rngSeed = rng.Next().ToString();
            seedBox.Text = rngSeed;
        }

        private void RandomFolderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            filepathBox.Text = "";
            if (randomFolderCheckbox.Checked)
            {
                filepathLabel.Text = "Folder to Randomise:";
                randomiseFolder = true;
            }
            if (!randomFolderCheckbox.Checked)
            {
                filepathLabel.Text = "LUA to Randomise:";
                randomiseFolder = false;
            }
        }

        private void RandomiseButton_Click(object sender, EventArgs e)
        {
            rng = new Random(rngSeed.GetHashCode());
            string luaName;

            if (musicConfigList.CheckedIndices.Count == 0)
            {
                MessageBox.Show("At least one selection must be checked.", "Music Randomiser");
                return;
            }

            foreach (int item in musicConfigList.CheckedIndices)
            {
                switch (item)
                {
                    case 0: validMusic.Add("stg_aqa_a"); break;
                    case 1: validMusic.Add("stg_aqa_b"); break;
                    case 2: validMusic.Add("stg_csc_a"); break;
                    case 3: validMusic.Add("stg_csc_b"); break;
                    case 4: validMusic.Add("stage_csc_e"); break;
                    case 5: validMusic.Add("stg_csc_f"); break;
                    case 6: validMusic.Add("stg_dtd_a"); break;
                    case 7: validMusic.Add("stg_dtd_b"); break;
                    case 8: validMusic.Add("stg_end_a"); break;
                    case 9: validMusic.Add("stg_end_b"); break;
                    case 10: validMusic.Add("stg_end_c"); break;
                    case 11: validMusic.Add("stg_end_d"); break;
                    case 12: validMusic.Add("stg_end_e"); break;
                    case 13: validMusic.Add("stg_end_f"); break;
                    case 14: validMusic.Add("stg_end_g"); break;
                    case 15: validMusic.Add("stg_flc_a"); break;
                    case 16: validMusic.Add("stg_flc_b"); break;
                    case 17: validMusic.Add("stg_kdv_a"); break;
                    case 18: validMusic.Add("stg_kdv_b"); break;
                    case 19: validMusic.Add("stg_kdv_c"); break;
                    case 20: validMusic.Add("stg_kdv_d"); break;
                    case 21: validMusic.Add("stg_rct_a"); break;
                    case 22: validMusic.Add("stg_rct_b"); break;
                    case 23: validMusic.Add("stg_tpj_a"); break;
                    case 24: validMusic.Add("stg_tpj_b"); break;
                    case 25: validMusic.Add("stg_tpj_c"); break;
                    case 26: validMusic.Add("stg_wap_a"); break;
                    case 27: validMusic.Add("stg_wap_b"); break;
                    case 28: validMusic.Add("stg_wvo_a"); break;
                    case 29: validMusic.Add("stg_wvo_b"); break;

                    case 30: validMusic.Add("boss_character"); break;
                    case 31: validMusic.Add("boss_cerberus"); break;
                    case 32: validMusic.Add("boss_wyvern"); break;
                    case 33: validMusic.Add("boss_iblis01"); break;
                    case 34: validMusic.Add("boss_iblis03"); break;
                    case 35: validMusic.Add("boss_mefiless01"); break;
                    case 36: validMusic.Add("boss_mefiless02"); break;
                    case 37: validMusic.Add("boss_solaris1"); break;
                    case 38: validMusic.Add("boss_solaris2"); break;

                    case 39: validMusic.Add("stg_twn_a"); break;
                    case 40: validMusic.Add("stg_twn_b"); break;
                    case 41: validMusic.Add("stg_twn_c"); break;
                    case 42: validMusic.Add("twn_accordion"); break;
                    case 43: validMusic.Add("stg_twn_shop"); break;
                    case 44: validMusic.Add("twn_mission_slow"); break;
                    case 45: validMusic.Add("twn_mission_comical"); break;
                    case 46: validMusic.Add("twn_mission_fast"); break;

                    case 47: validMusic.Add("mainmenu"); break;
                    case 48: validMusic.Add("select"); break;
                    case 49: validMusic.Add("extra"); break;
                    case 50: validMusic.Add("result"); break;
                }
            }

            if (randomiseFolder)
            {
                if (!Directory.Exists(filepath))
                {
                    MessageBox.Show(filepath + " does not appear to be a valid folder path.");
                    return;
                }
                string[] luaFilesA = Directory.GetFiles(filepath, "a_*.lub", SearchOption.AllDirectories);
                string[] luaFilesB = Directory.GetFiles(filepath, "b_*.lub", SearchOption.AllDirectories);
                string[] luaFilesC = Directory.GetFiles(filepath, "c_*.lub", SearchOption.AllDirectories);
                string[] luaFilesD = Directory.GetFiles(filepath, "d_*.lub", SearchOption.AllDirectories);
                string[] luaFilesE = Directory.GetFiles(filepath, "e_*.lub", SearchOption.AllDirectories);
                string[] luaFilesF = Directory.GetFiles(filepath, "f_*.lub", SearchOption.AllDirectories);
                string[] luaFilesF1 = Directory.GetFiles(filepath, "f1_*.lub", SearchOption.AllDirectories);
                string[] luaFilesF2 = Directory.GetFiles(filepath, "f2_*.lub", SearchOption.AllDirectories);
                string[] luaFilesG = Directory.GetFiles(filepath, "g_*.lub", SearchOption.AllDirectories);
                var luaList = new List<string> { };
                luaList.AddRange(luaFilesA);
                luaList.AddRange(luaFilesB);
                luaList.AddRange(luaFilesC);
                luaList.AddRange(luaFilesD);
                luaList.AddRange(luaFilesE);
                luaList.AddRange(luaFilesF);
                luaList.AddRange(luaFilesF1);
                luaList.AddRange(luaFilesF2);
                luaList.AddRange(luaFilesG);
                string[] luaFiles = luaList.ToArray();
                int amountOfLuas = luaFiles.Length;
                int compiledLuas = 0;
                foreach (var lua in luaFiles)
                {
                    luaName = lua.Remove(0, Path.GetDirectoryName(lua).Length);
                    luaName = luaName.Remove(luaName.Length - 4);
                    luaName = luaName.Replace("\\", "");
                    Console.WriteLine(luaName);
                    string[] readText = File.ReadAllLines(lua);
                    if (readText[0].Contains("LuaP"))
                    {
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = "/C java.exe -jar \"" + AppDomain.CurrentDomain.BaseDirectory + "External Software\\unlub.jar\" \"" + lua + "\" > \"" + filepath + luaName + ".lua\"";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        File.Delete(lua);
                        File.Move((filepath + luaName + ".lua"), lua);
                        compiledLuas++;
                    }
                    Program.LuaBackup(lua, luaName);
                    MusicRandomisation.MusicRandomiser(lua, rng, validMusic);
                }
                if (compiledLuas == 0)
                {
                    MessageBox.Show("Randomised " + amountOfLuas + " Music LUA files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Music Randomiser");
                }
                else
                {
                    MessageBox.Show("Decompiled " + compiledLuas + " compiled LUA files and randomised " + amountOfLuas + " Music LUA files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Music Randomiser");
                }
            }
            else
            {
                if (!File.Exists(filepath))
                {
                    MessageBox.Show(filepath + " does not appear to exist.");
                    return;
                }
                bool compiledLua = false;
                luaName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                luaName = luaName.Remove(luaName.Length - 4);
                luaName = luaName.Replace("\\", "");
                string[] readText = File.ReadAllLines(filepath);
                if (readText[0].Contains("LuaP"))
                {
                    string output = filepath.Remove((filepath.Length - (luaName.Length + 4)), luaName.Length + 4);
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C java.exe -jar \"" + AppDomain.CurrentDomain.BaseDirectory + "External Software\\unlub.jar\" \"" + filepath + "\" > \"" + output + luaName + ".lua\"";
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    File.Delete(filepath);
                    File.Move((output + luaName + ".lua"), filepath);
                    compiledLua = true;
                }
                Program.LuaBackup(filepath, luaName);
                MusicRandomisation.MusicRandomiser(filepath, rng, validMusic);
                if (!compiledLua)
                {
                    MessageBox.Show("Randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Music Randomiser");
                }
                else
                {
                    MessageBox.Show("Decompiled and randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Music Randomiser");
                }
            }
        }
    }
}
