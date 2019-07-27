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
    public partial class CharacterRandomisationForm : Form
    {
        string filepath;
        Random rng = new Random();
        string rngSeed;
        bool randomiseFolder = false;
        public static List<string> validModels = new List<string> { "\"player/amy\"", "\"player/blaze\"", "\"player/tails\"", "\"player/knuckles\"", "\"player/omega\"", "\"player/princess_princess\"", "\"player/rouge\"", "\"player/shadow\"", "\"player/silver\"", "\"player/snow_board\"", "\"player/sonic_new\"", "\"player/supershadow\"", "\"player/supersilver\"", "\"player/supersonic\"" };

        public CharacterRandomisationForm()
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
                OpenFileDialog setBrowser = new OpenFileDialog();
                setBrowser.Title = "Select SET Data";
                setBrowser.Filter = "SONIC THE HEDGEHOG (2006) Character LUA (*.lub)|*.lub";
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

            if (!movementCheckbox.Checked && !jumpCheckbox.Checked && !grindCheckbox.Checked && !abilityCheckbox.Checked && !modelsCheckbox.Checked && !gemCheckbox.Checked)
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
                string[] luaFiles = Directory.GetFiles(filepath, "*.lub", SearchOption.AllDirectories);
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
                    }
                    Program.LuaBackup(lua, luaName);
                    if (movementCheckbox.Checked) { CharacterRandomisation.MovementSpeedRandomiser(lua, rng); }
                    if (jumpCheckbox.Checked) { CharacterRandomisation.JumpRandomiser(lua, rng); }
                    if (grindCheckbox.Checked) { CharacterRandomisation.GrindSpeedRandomiser(lua, rng); }
                    if (abilityCheckbox.Checked) { CharacterRandomisation.CharacterAbilityRandomiser(lua, rng); }
                    if (modelsCheckbox.Checked)
                    {
                        if (lua.Contains("omega.lub") || lua.Contains("silver.lub"))
                        {
                            if (!lua.Contains("select") && !lua.Contains("super"))
                            {
                                if (lua.Contains("omega.lub")){ MessageBox.Show("Skipped the Character Model Randomisation on " + lua + " to avoid potential crashes relating to changing Omega's model in game."); }
                                if (lua.Contains("silver.lub")) { MessageBox.Show("Skipped the Character Model Randomisation on " + lua + " to avoid potential crashes relating to changing Silver's model in game."); }
                            }
                            else
                            {
                                CharacterRandomisation.ModelRandomiser(lua, rng);
                            }
                        }
                        else
                        {
                            CharacterRandomisation.ModelRandomiser(lua, rng);
                        }
                    }
                    if (gemCheckbox.Checked) { CharacterRandomisation.GemPatch(lua, rng); }
                }
                if (compiledLuas == 0)
                {
                    MessageBox.Show("Randomised " + amountOfLuas + " Character LUA files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Character Attributes Randomiser");
                }
                else
                {
                    MessageBox.Show("Decompiled " + compiledLuas + " compiled LUA files and randomised " + amountOfLuas + " Character LUA files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Character Attributes Randomiser");
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
                }
                Program.LuaBackup(filepath, luaName);
                if (movementCheckbox.Checked) { CharacterRandomisation.MovementSpeedRandomiser(filepath, rng); }
                if (jumpCheckbox.Checked) { CharacterRandomisation.JumpRandomiser(filepath, rng); }
                if (grindCheckbox.Checked) { CharacterRandomisation.GrindSpeedRandomiser(filepath, rng); }
                if (abilityCheckbox.Checked) { CharacterRandomisation.CharacterAbilityRandomiser(filepath, rng); }
                if (modelsCheckbox.Checked) { CharacterRandomisation.ModelRandomiser(filepath, rng); }
                if (gemCheckbox.Checked) { CharacterRandomisation.GemPatch(filepath, rng); }
                if (!compiledLua)
                {
                    MessageBox.Show("Randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Character Attributes Randomiser");
                }
                else
                {
                    MessageBox.Show("Decompiled and randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Character Attributes Randomiser");
                }
            }
        }

        private void ModelConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelConfig modelConfig = new ModelConfig();
            modelConfig.ShowDialog();
        }
    }
}
