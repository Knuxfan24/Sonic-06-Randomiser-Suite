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

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    public partial class SetRandomisationForm : Form
    {
        string filepath;
        Random rng = new Random();
        string rngSeed;
        bool randomiseFolder = false;
        public static List<string> validEnemies = new List<string> { "cBiter", "cCrawler", "cGazer", "cGolem", "cStalker", "cTaker", "cTitan", "cTricker", "eArmor", "eBluster", "eBomber",
            "eBuster", "eBuster(Fly)", "eCannon", "eCannon(Fly)", "eChaser", "eCommander", "eFlyer", "eGuardian", "eGunner", "eGunner(Fly)", "eHunter", "eKeeper", "eLancer", "eLancer(Fly)",
            "eLiner", "eRounder", "eSearcher", "eStinger", "eStinger(Fly)", "eSweeper", "eWalker" };
        public static List<string> validCharacters = new List<string> { "sonic_new", "tails", "knuckles", "shadow", "rouge", "omega", "silver", "blaze", "amy" };
        public static List<int> validItems = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        public SetRandomisationForm()
        {
            InitializeComponent();
            filepathLabel.Text = "SET to Randomise:";
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
                setBrowser.Filter = "SONIC THE HEDGEHOG (2006) SET Data (*.set)|*.set";
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

        private void RandomiseButton_Click(object sender, EventArgs e)
        {
            rng = new Random(rngSeed.GetHashCode());
            string setName;

            if (!enemiesCheckbox.Checked && !itemsCheckbox.Checked && !charactersCheckbox.Checked && !voiceCheckbox.Checked && !doorCheckbox.Checked)
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
                string[] setFiles = Directory.GetFiles(filepath, "*.set", SearchOption.AllDirectories);
                int amountOfSets = setFiles.Length;
                foreach (var set in setFiles)
                {
                    setName = set.Remove(0, Path.GetDirectoryName(set).Length);
                    setName = setName.Remove(setName.Length - 4);
                    setName = setName.Replace("\\", "");
                    Console.WriteLine(setName);
                    Program.HedgeLibPatch(set, setName);
                    {
                        if (enemiesCheckbox.Checked) { SetRandomisation.EnemyRandomiser(set, rng); }
                        if (itemsCheckbox.Checked) { SetRandomisation.ItemCapsuleRandomiser(set, rng); }
                        if (charactersCheckbox.Checked) { SetRandomisation.CharacterRandomiser(set, rng); }
                        if (voiceCheckbox.Checked) { SetRandomisation.VoiceRandomiser(set, rng); }
                        if (doorCheckbox.Checked) { SetRandomisation.DoorHack(set); }
                    }
                }
                MessageBox.Show("Randomised " + amountOfSets + " SET files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) SET Randomiser");
            }
            else
            {
                if (!File.Exists(filepath))
                {
                    MessageBox.Show(filepath + " does not appear to exist.");
                    return;
                }
                setName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                setName = setName.Remove(setName.Length - 4);
                setName = setName.Replace("\\", "");
                Program.HedgeLibPatch(filepath, setName);
                if (enemiesCheckbox.Checked) { SetRandomisation.EnemyRandomiser(filepath, rng); }
                if (itemsCheckbox.Checked) { SetRandomisation.ItemCapsuleRandomiser(filepath, rng); }
                if (charactersCheckbox.Checked) { SetRandomisation.CharacterRandomiser(filepath, rng); }
                if (voiceCheckbox.Checked) { SetRandomisation.VoiceRandomiser(filepath, rng); }
                if (doorCheckbox.Checked) { SetRandomisation.DoorHack(filepath); }
                MessageBox.Show("Randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) SET Randomiser");
            }
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
                filepathLabel.Text = "SET to Randomise:";
                randomiseFolder = false;
            }
        }

        private void EnemyConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnemyConfig enemyConfig = new EnemyConfig(validEnemies);
            enemyConfig.ShowDialog();
        }

        private void CharacterConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CharacterConfig characterConfig = new CharacterConfig(validCharacters);
            characterConfig.ShowDialog();
        }

        private void ItemCapsuleConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemConfig itemConfig = new ItemConfig(validItems);
            itemConfig.ShowDialog();
        }
    }
}
