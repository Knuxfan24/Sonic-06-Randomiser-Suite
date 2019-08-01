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
    public partial class MainWindow : Form
    {
        string filepath;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetButton_Click(object sender, EventArgs e)
        {
            SetRandomisationForm setRandomisation = new SetRandomisationForm();
            setRandomisation.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            setRandomisation.Show();
        }

        void otherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void LightButton_Click(object sender, EventArgs e)
        {
            LightRandomisationForm lightRandomisation = new LightRandomisationForm();
            lightRandomisation.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            lightRandomisation.Show();
        }

        private void CharacterButton_Click(object sender, EventArgs e)
        {
            CharacterRandomisationForm characterRandomisation = new CharacterRandomisationForm();
            characterRandomisation.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            characterRandomisation.Show();
        }

        private void MusicButton_Click(object sender, EventArgs e)
        {
            MusicRandomisationForm musicRandomisation = new MusicRandomisationForm();
            musicRandomisation.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            musicRandomisation.Show();
        }

        private void FilepathBox_TextChanged(object sender, EventArgs e)
        {
            filepath = filepathBox.Text;
        }

        private void FilepathButton_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog FolderBrowser = new VistaFolderBrowserDialog();
            if (FolderBrowser.ShowDialog() == DialogResult.OK)
            {
                filepath = FolderBrowser.SelectedPath;
                filepathBox.Text = filepath;
            }
        }

        private void CleanupButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(filepath))
            {
                MessageBox.Show(filepath + " does not appear to be a valid folder path.");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Search for and restore all backups in selected folder and subfolders?", "Randomiser Cleanup", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string backupName;
                string[] backupFiles = Directory.GetFiles(filepath, "*.s06back", SearchOption.AllDirectories);
                int amountOfBackups = backupFiles.Length;
                foreach (var file in backupFiles)
                {
                    backupName = file.Remove(0, Path.GetDirectoryName(file).Length);
                    backupName = backupName.Remove(backupName.Length - 8);
                    backupName = backupName.Replace("\\", "");
                    Console.WriteLine("Cleaning up: " + file);
                    if (File.Exists(file.Remove(file.Length - 8)))
                    {
                        File.Delete(file.Remove(file.Length - 8));
                    }
                    File.Move(file, file.Remove(file.Length - 8));
                }
                MessageBox.Show("Cleared up " + amountOfBackups + " modified files in " + filepath + ".");
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void MissionButton_Click(object sender, EventArgs e)
        {
            MissionRandomisationForm missionRandomisation = new MissionRandomisationForm();
            missionRandomisation.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            missionRandomisation.Show();
        }

        private void CollisionButton_Click(object sender, EventArgs e)
        {
            CollisionPropertiesForm collisionProperties = new CollisionPropertiesForm();
            collisionProperties.FormClosed += new FormClosedEventHandler(otherForm_FormClosed);
            this.Hide();
            collisionProperties.Show();
        }
    }
}
