using Ookii.Dialogs;
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
    public partial class LightRandomisationForm : Form
    {
        string filepath;
        Random rng = new Random();
        string rngSeed;
        bool randomiseFolder = false;

        public LightRandomisationForm()
        {
            InitializeComponent();
            filepathLabel.Text = "LUA to Randomise:";
            rngSeed = rng.Next().ToString();
            seedBox.Text = rngSeed;
        }

        private void FilepathBox_TextChanged(object sender, EventArgs e)
        {
            filepath = filepathBox.Text;
        }

        private void FilepathButton_Click(object sender, EventArgs e)
        {
            if (!randomiseFolder)
            {
                OpenFileDialog luaBrowser = new OpenFileDialog();
                luaBrowser.Title = "Select Scene LUA";
                luaBrowser.Filter = "SONIC THE HEDGEHOG (2006) Scene LUA (*.lub)|scene*.lub";
                luaBrowser.FilterIndex = 1;
                luaBrowser.RestoreDirectory = true;
                if (luaBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = luaBrowser.FileName;
                    filepathBox.Text = filepath;
                }
            }
            else
            {
                VistaFolderBrowserDialog luaFolderBrowser = new VistaFolderBrowserDialog();
                if (luaFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = luaFolderBrowser.SelectedPath;
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

            if (!ambientCheckbox.Checked && !mainCheckbox.Checked && !subCheckbox.Checked && !directionCheckbox.Checked && !fogColourCheckbox.Checked && !fogDensityCheckbox.Checked)
            {
                MessageBox.Show("No randomisation selected.");
                return;
            }

            if (randomiseFolder)
            {
                if (!Directory.Exists(filepath))
                {
                    MessageBox.Show(filepath + " does not appear to be a valid folder path.");
                    return;
                }
                string[] luaFiles = Directory.GetFiles(filepath, "scene*.lub", SearchOption.AllDirectories);
                int amountOfLuas = luaFiles.Length;
                int compiledLuas = 0;
                foreach (var lua in luaFiles)
                {
                    luaName = lua.Remove(0, Path.GetDirectoryName(lua).Length);
                    luaName = luaName.Remove(luaName.Length - 4);
                    luaName = luaName.Replace("\\", "");
                    Console.WriteLine("Working on: " + lua);
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
                    }
                    Program.LuaBackup(lua, luaName);
                    if (ambientCheckbox.Checked) { LightRandomisation.AmbientRandomiser(lua, rng); }
                    if (mainCheckbox.Checked) { LightRandomisation.MainRandomiser(lua, rng); }
                    if (subCheckbox.Checked) { LightRandomisation.SubRandomiser(lua, rng); }
                    if (directionCheckbox.Checked) { LightRandomisation.DirectionRandomiser(lua, rng); }
                    if (fogColourCheckbox.Checked) { LightRandomisation.FogColourRandomiser(lua, rng); }
                    if (fogDensityCheckbox.Checked) { LightRandomisation.FogDensityRandomiser(lua, rng); }
                }
                if (compiledLuas == 0)
                {
                    MessageBox.Show("Randomised " + amountOfLuas + " Scene LUA files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Lighting Randomiser");
                }
                else
                {
                    MessageBox.Show("Decompiled " + compiledLuas + " compiled LUA files and randomised " + amountOfLuas + " Scene LUA files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Lighting Randomiser");
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
                    startInfo.Arguments = "/C java.exe -jar \"" + AppDomain.CurrentDomain.BaseDirectory + "External Software\\unlub.jar\" \"" + filepath +  "\" > \"" + output + luaName + ".lua\"";
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    File.Delete(filepath);
                    File.Move((output + luaName + ".lua"), filepath);
                }
                Program.LuaBackup(filepath, luaName);
                if (ambientCheckbox.Checked) { LightRandomisation.AmbientRandomiser(filepath, rng); }
                if (ambientCheckbox.Checked) { LightRandomisation.MainRandomiser(filepath, rng); }
                if (ambientCheckbox.Checked) { LightRandomisation.SubRandomiser(filepath, rng); }
                if (ambientCheckbox.Checked) { LightRandomisation.DirectionRandomiser(filepath, rng); }
                if (fogColourCheckbox.Checked) { LightRandomisation.FogColourRandomiser(filepath, rng); }
                if (fogDensityCheckbox.Checked) { LightRandomisation.FogDensityRandomiser(filepath, rng); }
                if (!compiledLua)
                {
                    MessageBox.Show("Randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Lighting Randomiser");
                }
                else
                {
                    MessageBox.Show("Decompiled and randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Lighting Randomiser");
                }
            }
        }
    }
}
