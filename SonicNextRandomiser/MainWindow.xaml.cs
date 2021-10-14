using Ookii.Dialogs.Wpf;
using SonicNextRandomiser.Randomisers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SonicNextRandomiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Noire.NoireWindow
    {
        public static string TemporaryDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
        public static Random Randomiser = new();

        public MainWindow()
        {
            InitializeComponent();
            SetDefaults();

            // Fill out the CheckedListBox controls with the embedded text files.
            Helpers.FillOutCheckList(Properties.Resources.EnemyTypes, CheckedList_SET_EnemyTypes);
            Helpers.FillOutCheckList(Properties.Resources.CharacterTypes, CheckedList_SET_Characters);
            Helpers.FillOutCheckList(Properties.Resources.ItemTypes, CheckedList_SET_ItemCapsules);
            Helpers.FillOutCheckList(Properties.Resources.CommonPropTypes, CheckedList_SET_CommonProps);
            Helpers.FillOutCheckList(Properties.Resources.PathPropTypes, CheckedList_SET_PathProps);
            Helpers.FillOutCheckList(Properties.Resources.VoiceTypes, CheckedList_SET_Hints);
            Helpers.FillOutCheckList(Properties.Resources.DoorTypes, CheckedList_SET_Doors);
            Helpers.FillOutCheckList(Properties.Resources.EventLighting, CheckedList_Event_Lighting);
            Helpers.FillOutCheckList(Properties.Resources.EventTerrain, CheckedList_Event_Terrain);
            Helpers.FillOutCheckList(Properties.Resources.EnvMaps, CheckedList_Scene_EnvMaps);
            Helpers.FillOutCheckList(Properties.Resources.MiscSongs, CheckedList_Misc_Songs);
            Helpers.FillOutCheckList(Properties.Resources.MiscLanguages, CheckedList_Misc_Languages);
        }

        private void SetDefaults()
        {
            TextBox_General_ModsDirectory.Text  = Properties.Settings.Default.modsDirectory;
            TextBox_General_GameExecutable.Text = Properties.Settings.Default.gameExecutable;
            TextBox_General_Seed.Text           = Randomiser.Next().ToString();
        }

        private void Button_General_ModsDirectory_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog FolderBrowser = new()
            {
                Description = "Select Mods Directory",
                UseDescriptionForTitle = true
            };

            if (FolderBrowser.ShowDialog() == true)
                TextBox_General_ModsDirectory.Text = FolderBrowser.SelectedPath;
        }

        private void Button_General_GameExecutable_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Game Executable",
                Multiselect = false,
                Filter = "Xbox 360 Executable|default.xex|PlayStation 3 Executable|EBOOT.BIN"
            };

            if(OpenFileDialog.ShowDialog() == true)
                TextBox_General_GameExecutable.Text = OpenFileDialog.FileName;
        }

        private void Button_General_Seed_Click(object sender, RoutedEventArgs e)
        {
            TextBox_General_Seed.Text = Randomiser.Next().ToString();
        }

        private void TextBox_General_ModsDirectory_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.modsDirectory = TextBox_General_ModsDirectory.Text;
            Properties.Settings.Default.Save();
        }

        private void TextBox_General_GameExecutable_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.gameExecutable = TextBox_General_GameExecutable.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Custom_Music_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Songs",
                Multiselect = true,
                Filter = "All Supported Types|*.wav;*.mp3;*.m4a;*.xma|Waveform Audio File|*.wav|MP3 Audio File|*.mp3|MPEG4 Audio File|*.m4a|Xbox Media Audio File|*.xma"
            };

            // If the selections are valid, add them to the list of text in the custom music textbox.
            if (OpenFileDialog.ShowDialog() == true)
            {
                // Don't erase the box, just add a seperator.
                if (TextBox_Custom_Music.Text.Length != 0)
                    TextBox_Custom_Music.Text += "|";

                // Add selected files to the text box.
                for (int i = 0; i < OpenFileDialog.FileNames.Length; i++)
                    TextBox_Custom_Music.Text += $"{OpenFileDialog.FileNames[i]}|";

                // Remove the extra comma added at the end.
                TextBox_Custom_Music.Text = TextBox_Custom_Music.Text.Remove(TextBox_Custom_Music.Text.LastIndexOf('|'));
            }
        }
    }
}
