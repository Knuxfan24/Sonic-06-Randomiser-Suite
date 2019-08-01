using Ookii.Dialogs;
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
    public partial class CollisionPropertiesForm : Form
    {
        string filepath;
        Random rng = new Random();
        string rngSeed;
        bool randomiseFolder = false;
        public static List<string> validSurfaces = new List<string> { "0", "1", "2", "3", "5", "6", "8", "9", "A", "E" };

        public CollisionPropertiesForm()
        {
            InitializeComponent();
            filepathLabel.Text = "BIN to Randomise:";
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
                OpenFileDialog binBrowser = new OpenFileDialog();
                binBrowser.Title = "Select Collision BIN";
                binBrowser.Filter = "SONIC THE HEDGEHOG (2006) Collision BIN (collision.bin)|collision.bin";
                binBrowser.FilterIndex = 1;
                binBrowser.RestoreDirectory = true;
                if (binBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = binBrowser.FileName;
                    filepathBox.Text = filepath;
                }
            }
            else
            {
                VistaFolderBrowserDialog binFolderBrowser = new VistaFolderBrowserDialog();
                if (binFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    filepath = binFolderBrowser.SelectedPath;
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
                filepathLabel.Text = "BIN to Randomise:";
                randomiseFolder = false;
            }
        }

        private void RandomiseButton_Click(object sender, EventArgs e)
        {
            string convertedColi;
            rng = new Random(rngSeed.GetHashCode());
            string binName;

            if (randomiseFolder)
            {
                if (!Directory.Exists(filepath))
                {
                    MessageBox.Show(filepath + " does not appear to be a valid folder path.");
                    return;
                }
                string[] binFiles = Directory.GetFiles(filepath, "collision.bin", SearchOption.AllDirectories);
                int amountOfBins = binFiles.Length;
                foreach (var bin in binFiles)
                {
                    binName = bin.Remove(0, Path.GetDirectoryName(bin).Length);
                    binName = binName.Remove(binName.Length - 4);
                    binName = binName.Replace("\\", "");
                    Console.WriteLine("Working on: " + bin);
                    Program.CollisionBackup(bin, binName);
                    CollisionRandomisation.Decompile(bin, out convertedColi);
                    CollisionRandomisation.RotationSwap(convertedColi, rng);
                    CollisionRandomisation.PropertyRandomiser(convertedColi, rng, wallsCheckbox.Checked, waterCheckbox.Checked, deathCheckbox.Checked);
                    CollisionRandomisation.Compile(convertedColi, bin);
                }
                MessageBox.Show("Randomised " + amountOfBins + " Collision files in: " + filepath + ".", "SONIC THE HEDGEHOG (2006) Collision Properties Randomiser");
            }
            else
            {
                if (!File.Exists(filepath))
                {
                    MessageBox.Show(filepath + " does not appear to exist.");
                    return;
                }
                binName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                binName = binName.Remove(binName.Length - 4);
                binName = binName.Replace("\\", "");
                Program.CollisionBackup(filepath, binName);
                CollisionRandomisation.Decompile(filepath, out convertedColi);
                CollisionRandomisation.RotationSwap(convertedColi, rng);
                CollisionRandomisation.PropertyRandomiser(convertedColi, rng, wallsCheckbox.Checked, waterCheckbox.Checked, deathCheckbox.Checked);
                CollisionRandomisation.Compile(convertedColi, filepath);
                MessageBox.Show("Randomised " + filepath + ".", "SONIC THE HEDGEHOG (2006) Collision Properties Randomiser");
            }
        }

        private void SurfaceTypeConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SurfaceConfig surfaceConfig = new SurfaceConfig();
            surfaceConfig.ShowDialog();
        }
    }
}
