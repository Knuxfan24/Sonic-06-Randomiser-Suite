global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.IO;
global using System.Threading.Tasks;
using Marathon.Formats.Archive;
using Marathon.Helpers;
using Marathon.IO;
using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MarathonRandomiser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Version Number.
        public static readonly string GlobalVersionNumber = $"Version 2.1.16";

        #if !DEBUG
        public static readonly string VersionNumber = GlobalVersionNumber;
        #else
        public static readonly string VersionNumber = $"{GlobalVersionNumber}-indev-{File.GetLastAccessTime(Environment.CurrentDirectory):ddMMyy}";
        #endif
        
        // Generate the path to a temp directory we can use for the Randomisation process.
        public static readonly string TemporaryDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        // Set up the Randomiser.
        public static Random Randomiser = new();

        // Binding data for the Progress Logger.
        public static readonly DependencyProperty ProgressLoggerProperty = DependencyProperty.Register
        (
            nameof(ProgressLogger),
            typeof(ObservableCollection<string>),
            typeof(MainWindow),
            new PropertyMetadata(new ObservableCollection<string>())
        );
        public ObservableCollection<string> ProgressLogger
        {
            get => (ObservableCollection<string>)GetValue(ProgressLoggerProperty);
            set => SetValue(ProgressLoggerProperty, value);
        }

        /// <summary>
        /// Main logic for the Application.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            GenerateDirectories();
            SetDefaults();
            
            // Force culture info 'en-GB' to prevent errors with values altered by language-specific differences.
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-GB");

            // If this is a debug build, set the seed to WPF Test for the sake of consistent testing and list the Temporary Directory path.
            #if DEBUG
            TextBox_General_Seed.Text = "Development Testing";
            Debug.WriteLine($"Current temporary path is: {TemporaryDirectory}.");
            #endif

            DataContext = this;
        }

        /// <summary>
        /// Create the Voice Packs and XMA Cache Directories if they don't exist.
        /// </summary>
        private static void GenerateDirectories()
        {
            // Create the Voice Packs directory.
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\VoicePacks"))
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\VoicePacks");

            // Create the XMA Cache Directory.
            if (!Directory.Exists($@"{Environment.CurrentDirectory}\Cache\XMA"))
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Cache\XMA");
        }
        
        /// <summary>
        /// Set up various default values.
        /// </summary>
        private void SetDefaults()
        {
            // Include the version number in the title bar.
            Title = $"Sonic '06 Randomiser Suite ({VersionNumber})";

            // Load consistent settings.
            TextBox_General_ModsDirectory.Text = Properties.Settings.Default.ModsDirectory;
            TextBox_General_GameExecutable.Text = Properties.Settings.Default.GameExecutable;
            
            // Handle an invalid patch directory.
            if (TextBox_General_Patches.Text != $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Unify\\Patches\\" && !Directory.Exists(Properties.Settings.Default.PatchDirectory))
            {
                TextBox_General_Patches.Text = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Unify\\Patches\\";
                Debug.WriteLine("Patch Directory was invalid, restored default localappdata path.");
            }
            else
            {
                TextBox_General_Patches.Text = Properties.Settings.Default.PatchDirectory;
            }

            // Saved DLC locations.
            TextBox_Misc_SonicVHLocation.Text = Properties.Settings.Default.SonicVeryHard;
            TextBox_Misc_ShadowVHLocation.Text = Properties.Settings.Default.ShadowVeryHard;
            TextBox_Misc_SilverVHLocation.Text = Properties.Settings.Default.SilverVeryHard;

            // Generate a seed to use.
            TextBox_General_Seed.Text = Randomiser.Next().ToString();
            
            // Fill in the configuration CheckListBox elements.
            Helpers.FillCheckedListBox(Properties.Resources.SETEnemies, CheckedList_SET_EnemyTypes);
            Helpers.FillCheckedListBox(Properties.Resources.SETCharacters, CheckedList_SET_Characters);
            Helpers.FillCheckedListBox(Properties.Resources.SETItemCapsules, CheckedList_SET_ItemCapsules);
            Helpers.FillCheckedListBox(Properties.Resources.SETCommonProps, CheckedList_SET_CommonProps);
            Helpers.FillCheckedListBox(Properties.Resources.SETPathProps, CheckedList_SET_PathProps);
            Helpers.FillCheckedListBox(Properties.Resources.SETHints, CheckedList_SET_Hints);
            Helpers.FillCheckedListBox(Properties.Resources.SETDoors, CheckedList_SET_Doors);
            Helpers.FillCheckedListBox(Properties.Resources.SETParticles, CheckedList_SET_Particles);
            Helpers.FillCheckedListBox(Properties.Resources.SETObjectTypes, CheckedList_SET_ObjectShuffle);

            Helpers.FillCheckedListBox(Properties.Resources.EventLighting, CheckedList_Event_Lighting);
            Helpers.FillCheckedListBox(Properties.Resources.EventTerrain, CheckedList_Event_Terrain);

            Helpers.FillCheckedListBox(Properties.Resources.SceneEnvMaps, CheckedList_Scene_EnvMaps);
            Helpers.FillCheckedListBox(Properties.Resources.SceneSkyboxes, CheckedList_Scene_Skyboxes);

            Helpers.FillCheckedListBox(Properties.Resources.AudioSongs, CheckedList_Audio_Songs);
            Helpers.FillCheckedListBox(Properties.Resources.AudioCSBs, CheckedList_Audio_SFX);
            
            Helpers.FillCheckedListBox(Properties.Resources.TextLanguages, CheckedList_Text_Languages);
            Helpers.FillCheckedListBox(Properties.Resources.TextButtonIcons, CheckedList_Text_Buttons);

            Helpers.FetchPatches(TextBox_General_Patches.Text, CheckedList_Misc_Patches);

            RefreshVoicePacks();

            // Fill out the CheckedListBoxes for the Wildcard.
            Helpers.FillWildcardCheckedListBox(Grid_ObjectPlacement, CheckedList_Wildcard_SET);
            Helpers.FillWildcardCheckedListBox(Grid_Event, CheckedList_Wildcard_Event);
            Helpers.FillWildcardCheckedListBox(Grid_Scene, CheckedList_Wildcard_Scene);
            Helpers.FillWildcardCheckedListBox(Grid_Animations, CheckedList_Wildcard_Animations);
            Helpers.FillWildcardCheckedListBox(Grid_Models, CheckedList_Wildcard_Models);
            Helpers.FillWildcardCheckedListBox(Grid_Textures, CheckedList_Wildcard_Textures);
            Helpers.FillWildcardCheckedListBox(Grid_Audio, CheckedList_Wildcard_Audio);
            Helpers.FillWildcardCheckedListBox(Grid_Text, CheckedList_Wildcard_Text);
            Helpers.FillWildcardCheckedListBox(Grid_XNCP, CheckedList_Wildcard_XNCP);
            Helpers.FillWildcardCheckedListBox(Grid_Miscellaneous, CheckedList_Wildcard_Miscellaneous);
        }

        #region Text Box/Button Functions
        /// <summary>
        /// Opens a Folder Browser to select our Mods Directory.
        /// </summary>
        private void ModsDirectory_Browse(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog FolderBrowser = new()
            {
                Description = "Select Mods Directory",
                UseDescriptionForTitle = true
            };

            if (FolderBrowser.ShowDialog() == true)
                TextBox_General_ModsDirectory.Text = FolderBrowser.SelectedPath;
        }

        /// <summary>
        /// Saves the Mods Directory setting when the value changes.
        /// </summary>
        private void ModsDirectory_Update(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.ModsDirectory = TextBox_General_ModsDirectory.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Opens a File Browser to select our Game Executable.
        /// </summary>
        private void GameExecutable_Browse(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Game Executable",
                Multiselect = false,
                Filter = "Xbox 360 Executable|default.xex|PlayStation 3 Executable|EBOOT.BIN"
            };

            if (OpenFileDialog.ShowDialog() == true)
                TextBox_General_GameExecutable.Text = OpenFileDialog.FileName;
        }

        /// <summary>
        /// Saves the Game Executable setting when the value changes.
        /// Also kills the Custom Tab if we're using the PS3 version.
        /// Also fills in the archives list on the Texture Randomiser tab.
        /// </summary>
        private void GameExecutable_Update(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.GameExecutable = TextBox_General_GameExecutable.Text;
            Properties.Settings.Default.Save();

            if (TextBox_General_GameExecutable.Text.ToLower().EndsWith(".bin"))
                TabItem_Custom.IsEnabled = false;
            else
                TabItem_Custom.IsEnabled = true;

            // If the selected executable exists, populate the Texture Randomiser and Model Randomiser's archives lists.
            if (File.Exists(TextBox_General_GameExecutable.Text))
            {
                CheckedList_Textures_Arcs.Items.Clear();
                string[] archives = Directory.GetFiles(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "*.arc", SearchOption.AllDirectories);

                foreach(string archive in archives)
                {
                    // Load a list of the files in this archive.
                    U8Archive arc = new(archive, ReadMode.IndexOnly);
                    IEnumerable<Marathon.IO.Interfaces.IArchiveFile> arcFiles = arc.Root.GetFiles();

                    // Determine if we have textures in this archive.
                    bool hasTexture = false;
                    foreach (var file in arcFiles)
                    {
                        if (Path.GetExtension(file.Name) == ".dds")
                        {
                            hasTexture = true;
                            break;
                        }
                    }

                    // Determine if we have models in this archive.
                    bool hasModel = false;
                    foreach (var file in arcFiles)
                    {
                        if (Path.GetExtension(file.Name) == ".xno")
                        {
                            hasModel = true;
                            break;
                        }
                    }

                    // Texture Randomiser List.
                    if (hasTexture)
                    {
                        CheckedListBoxItem item = new()
                        {
                            DisplayName = Path.GetFileName(archive),
                            Tag = Path.GetFileName(archive),
                            Checked = false
                        };

                        // Auto check it if it's a stage archive.
                        if (Path.GetFileName(archive).StartsWith("stage_"))
                            item.Checked = true;

                        CheckedList_Textures_Arcs.Items.Add(item);
                    }

                    // Model Randomiser List.
                    if (hasModel)
                    {
                        CheckedListBoxItem item = new()
                        {
                            DisplayName = Path.GetFileName(archive),
                            Tag = Path.GetFileName(archive),
                            Checked = false
                        };

                        CheckedList_Models_Arcs.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Generate and fill in a new seed.
        /// </summary>
        private void Seed_Reroll(object sender, RoutedEventArgs e)
        {
            TextBox_General_Seed.Text = Randomiser.Next().ToString();
        }

        /// <summary>
        /// Opens a Folder Browser to select our Patches Directory.
        /// </summary>
        private void PatchDirectory_Browse(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog FolderBrowser = new()
            {
                Description = "Select Patches Directory",
                UseDescriptionForTitle = true
            };

            if (FolderBrowser.ShowDialog() == true)
                TextBox_General_Patches.Text = FolderBrowser.SelectedPath;
        }

        /// <summary>
        /// Saves the Patches Directory setting when the value changes.
        /// </summary>
        private void PatchDirectory_Update(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(TextBox_General_Patches.Text))
                Helpers.FetchPatches(TextBox_General_Patches.Text, CheckedList_Misc_Patches);

            Properties.Settings.Default.PatchDirectory = TextBox_General_Patches.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Opens a File Browser to select one or more custom songs.
        /// </summary>
        private void CustomMusic_Browse(object sender, EventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Songs",
                Multiselect = true,
                Filter = "All Types|*.*"
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

        /// <summary>
        /// Opens a File Browser to select one or more custom songs.
        /// </summary>
        private void CustomTextures_Browse(object sender, EventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Textures",
                Multiselect = true,
                Filter = "Supported Types|*.bmp;*.jpg;*.jpeg;*.png;*.dds;*.tga;*.hdr;*.tif;*.tiff;*.wdp;*.hdp;*.jxr;*.ppm;*.pfm|" +
                "BMP|*.bmp|JPEG|*.jpg;*.jpeg|PNG|*.png|DDS|*.dds|TGA|*.tga|HDR|*.hdr|TIFF|*.tif;*.tiff|WDP|*.wdp|HDP|*.hdp|JXR|*.jxr|PPM|*.ppm|PFM|*.pfm"
            };

            // If the selections are valid, add them to the list of text in the custom textures textbox.
            if (OpenFileDialog.ShowDialog() == true)
            {
                // Don't erase the box, just add a seperator.
                if (TextBox_Custom_Textures.Text.Length != 0)
                    TextBox_Custom_Textures.Text += "|";

                // Add selected files to the text box.
                for (int i = 0; i < OpenFileDialog.FileNames.Length; i++)
                    TextBox_Custom_Textures.Text += $"{OpenFileDialog.FileNames[i]}|";

                // Remove the extra comma added at the end.
                TextBox_Custom_Textures.Text = TextBox_Custom_Textures.Text.Remove(TextBox_Custom_Textures.Text.LastIndexOf('|'));
            }
        }
        /// <summary>
        /// Downloads my voice packs from GitHub.
        /// </summary>
        private async void Custom_FetchVox(object sender, RoutedEventArgs e)
        {
            // Display the Progress Logger elements and disable bottom buttons that shouldn't be useable during the process.
            ProgressLogger.Clear();
            TabControl_Main.Visibility = Visibility.Hidden;
            ProgressBar_ProgressLogger.Visibility = Visibility.Visible;
            ListView_ProgressLogger.Visibility = Visibility.Visible;
            Button_Randomise.IsEnabled = false;
            Button_LoadConfig.IsEnabled = false;
            UpdateLogger($"Fetching official voice packs from GitHub.");

            // Get the packs.
            Dictionary<string, string> packs = await Task.Run(() => Helpers.FetchOfficalVox());

            // Loop through the packs and download them.
            foreach (KeyValuePair<string, string> pack in packs)
            {
                UpdateLogger($"Downloading '{Path.GetFileNameWithoutExtension(pack.Value)}' voice pack.");
                await Task.Run(() => Helpers.DownloadVox(pack));
            }

            // Restore Form Visiblity.
            TabControl_Main.Visibility = Visibility.Visible;
            ProgressBar_ProgressLogger.Visibility = Visibility.Hidden;
            ListView_ProgressLogger.Visibility = Visibility.Hidden;
            Button_Randomise.IsEnabled = true;
            Button_LoadConfig.IsEnabled = true;

            // Refresh the Voice Packs List.
            RefreshVoicePacks();
        }

        /// <summary>
        /// Simply call the Refresh Voice Packs function.
        /// </summary>
        private void Custom_RefreshVox(object sender, RoutedEventArgs e)
        {
            RefreshVoicePacks();
        }

        /// <summary>
        /// Opens a File Browser to select the archive containing Sonic's Very Hard Mode content.
        /// </summary>
        private void SonicVHLocation_Browse(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Very Hard Mode ~ Sonic Archive",
                Multiselect = false,
                Filter = "Supported Types|*.arc"
            };

            if (OpenFileDialog.ShowDialog() == true)
                TextBox_Misc_SonicVHLocation.Text = OpenFileDialog.FileName;
        }

        /// <summary>
        /// Saves the Sonic Very Hard Mode location setting when the value changes.
        /// </summary>
        private void SonicVHLocation_Update(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.SonicVeryHard = TextBox_Misc_SonicVHLocation.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Opens a File Browser to select the archive containing Shadow's Very Hard Mode content.
        /// </summary>
        private void ShadowVHLocation_Browse(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Very Hard Mode ~ Shadow Archive",
                Multiselect = false,
                Filter = "Supported Types|*.arc"
            };

            if (OpenFileDialog.ShowDialog() == true)
                TextBox_Misc_ShadowVHLocation.Text = OpenFileDialog.FileName;
        }

        /// <summary>
        /// Saves the Shadow Very Hard Mode location setting when the value changes.
        /// </summary>
        private void ShadowVHLocation_Update(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.ShadowVeryHard = TextBox_Misc_ShadowVHLocation.Text;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Opens a File Browser to select the archive containing Silver's Very Hard Mode content.
        /// </summary>
        private void SilverVHLocation_Browse(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Very Hard Mode ~ Silver Archive",
                Multiselect = false,
                Filter = "Supported Types|*.arc"
            };

            if (OpenFileDialog.ShowDialog() == true)
                TextBox_Misc_SilverVHLocation.Text = OpenFileDialog.FileName;
        }

        /// <summary>
        /// Saves the Silver Very Hard Mode location setting when the value changes.
        /// </summary>
        private void SilverVHLocation_Update(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.SilverVeryHard = TextBox_Misc_SilverVHLocation.Text;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Form Helper Functions
        /// <summary>
        /// Disables and enables certain other elements based on the toggled status of a CheckBox
        /// </summary>
        private void Dependency_CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // Get the new state of this CheckBox.
            bool NewCheckedStatus = (bool)((CheckBox)sender).IsChecked;

            // Get the name of this Checkbox.
            string CheckBoxName = ((CheckBox)sender).Name;

            // Check the name of the Checkbox and carry out the appropriate task(s).
            switch (CheckBoxName)
            {
                case "CheckBox_SET_Enemies":
                    CheckBox_SET_Enemies_NoBosses.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_SET_Enemies_Behaviour":
                    CheckBox_SET_Enemies_Behaviour_NoEnforce.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_SET_DrawDistance":
                    Label_SET_DrawDistance_Min.IsEnabled = NewCheckedStatus;
                    NumericUpDown_SET_DrawDistance_Min.IsEnabled = NewCheckedStatus;
                    Label_SET_DrawDistance_Max.IsEnabled = NewCheckedStatus;
                    NumericUpDown_SET_DrawDistance_Max.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_SET_Jumpboards":
                    Label_SET_Jumpboards_Chance.IsEnabled = NewCheckedStatus;
                    NumericUpDown_SET_Jumpboards_Chance.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Event_Voices":
                    CheckBox_Event_Voices_Japanese.IsEnabled = NewCheckedStatus;
                    CheckBox_Event_Voices_Gameplay.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_Event_Order":
                    CheckBox_Event_SkipFMVs.IsEnabled = NewCheckedStatus;
                    CheckBox_Event_SkipFMVs.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Scene_Light_Ambient":
                case "CheckBox_Scene_Light_Main":
                case "CheckBox_Scene_Light_Sub":
                    if(NewCheckedStatus == true)
                    {
                        Label_Scene_MinStrength.IsEnabled = true;
                        NumericUpDown_Scene_MinStrength.IsEnabled = true;
                    }
                    else if(CheckBox_Scene_Light_Ambient.IsChecked == false && CheckBox_Scene_Light_Main.IsChecked == false && CheckBox_Scene_Light_Sub.IsChecked == false)
                    {
                        Label_Scene_MinStrength.IsEnabled = false;
                        NumericUpDown_Scene_MinStrength.IsEnabled = false;
                    }
                    break;
                case "CheckBox_Scene_Light_Direction":
                    CheckBox_Scene_Light_Direction_Enforce.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Anim_Gameplay":
                    CheckBox_Anim_GameplayUseAll.IsEnabled = NewCheckedStatus;
                    CheckBox_Anim_GameplayUseEvents.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_Anim_Framerate":
                    Label_Anim_Framerate_Min.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Anim_Framerate_Min.IsEnabled = NewCheckedStatus;
                    Label_Anim_Framerate_Max.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Anim_Framerate_Max.IsEnabled = NewCheckedStatus;
                    CheckBox_Anim_Framerate_NoLights.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Models_MaterialColour":
                    CheckBox_Models_MaterialDiffuse.IsEnabled = NewCheckedStatus;
                    CheckBox_Models_MaterialAmbient.IsEnabled = NewCheckedStatus;
                    CheckBox_Models_MaterialSpecular.IsEnabled = NewCheckedStatus;
                    CheckBox_Models_MaterialEmissive.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Textures_Textures":
                    CheckBox_Textures_PerArc.IsEnabled = NewCheckedStatus;
                    CheckBox_Textures_AllowDupes.IsEnabled = NewCheckedStatus;
                    CheckBox_Textures_OnlyCustom.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Text_Generate":
                    CheckBox_Text_Generate_Enforce.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_XNCP_Colours":
                    CheckBox_XNCP_Colours_Same.IsEnabled = NewCheckedStatus;
                    CheckBox_XNCP_Colours_Alpha.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_XNCP_Scale":
                    Label_XNCP_Scale_Min.IsEnabled = NewCheckedStatus;
                    NumericUpDown_XNCP_Scale_Min.IsEnabled = NewCheckedStatus;
                    Label_XNCP_Scale_Max.IsEnabled = NewCheckedStatus;
                    NumericUpDown_XNCP_Scale_Max.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Text_Colour":
                    Label_Text_Colour_Weight.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Text_Colour_Weight.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Misc_EnemyHealth":
                    Label_Misc_EnemyHealth_Min.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Misc_EnemyHealth_Min.IsEnabled = NewCheckedStatus;
                    Label_Misc_EnemyHealth_Max.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Misc_EnemyHealth_Max.IsEnabled = NewCheckedStatus;
                    CheckBox_Misc_EnemyHealth_Bosses.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_Misc_PropPSIBehaviour":
                    CheckBox_Misc_PropPSIBehaviour_NoGrab.IsEnabled = NewCheckedStatus;
                    CheckBox_Misc_PropPSIBehaviour_NoDebris.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_Misc_Collision":
                    CheckBox_Misc_Collision_PerFace.IsEnabled = NewCheckedStatus;
                    break;
                case "CheckBox_Misc_Patches":
                    Label_Misc_Patches_Weight.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Misc_Patches_Weight.IsEnabled = NewCheckedStatus;
                    break;

                case "CheckBox_Wildcard_Enable":
                    TabItem_SET.IsEnabled = !NewCheckedStatus;
                    TabItem_Event.IsEnabled = !NewCheckedStatus;
                    TabItem_Scene.IsEnabled = !NewCheckedStatus;
                    TabItem_Anim.IsEnabled = !NewCheckedStatus;
                    TabItem_Models.IsEnabled = !NewCheckedStatus;
                    TabItem_Textures.IsEnabled = !NewCheckedStatus;
                    TabItem_Audio.IsEnabled = !NewCheckedStatus;
                    TabItem_Text.IsEnabled = !NewCheckedStatus;
                    TabItem_XNCP.IsEnabled = !NewCheckedStatus;
                    TabItem_Misc.IsEnabled = !NewCheckedStatus;

                    Label_Wildcard_Weight.IsEnabled = NewCheckedStatus;
                    NumericUpDown_Wildcard_Weight.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_SET.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Event.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Scene.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Animations.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Models.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Textures.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Audio.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Text.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_XNCP.IsEnabled = NewCheckedStatus;
                    CheckBox_Wildcard_Miscellaneous.IsEnabled = NewCheckedStatus;
                    break;
            }
        }

        /// <summary>
        /// Checks and unchecks every element in a CheckedListBox control.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown if the Grid Name or Selected Tab Index doesn't exist in the list.</exception>
        private void CheckedListBox_SelectionToggle(object sender, RoutedEventArgs e)
        {
            // Check if the button that called this event was a Select All one, doing it this way means I don't have to name them or check a large list of button names.
            bool selectAll = (string)((Button)sender).Content == "Select All";

            // Get the grid element this button was a part of.
            var buttonParent = ((Button)sender).Parent;

            // Check the name of the parent grid element.
            // In most cases, we then check for the relevant tab control's selected index to determine the next action.
            switch (((Grid)buttonParent).Name)
            {
                case "Grid_ObjectPlacement":
                    switch (TabControl_ObjectPlacement.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_SET_EnemyTypes, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_SET_Characters, true, selectAll); break;
                        case 2: Helpers.InvalidateCheckedListBox(CheckedList_SET_ItemCapsules, true, selectAll); break;
                        case 3: Helpers.InvalidateCheckedListBox(CheckedList_SET_CommonProps, true, selectAll); break;
                        case 4: Helpers.InvalidateCheckedListBox(CheckedList_SET_PathProps, true, selectAll); break;
                        case 5: Helpers.InvalidateCheckedListBox(CheckedList_SET_Hints, true, selectAll); break;
                        case 6: Helpers.InvalidateCheckedListBox(CheckedList_SET_Doors, true, selectAll); break;
                        case 7: Helpers.InvalidateCheckedListBox(CheckedList_SET_Particles, true, selectAll); break;
                        case 8: Helpers.InvalidateCheckedListBox(CheckedList_SET_ObjectShuffle, true, selectAll); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Event":
                    switch (TabControl_Event.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Event_Lighting, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_Event_Terrain, true, selectAll); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Scene":
                    switch (TabControl_Scene.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Scene_EnvMaps, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_Scene_Skyboxes, true, selectAll); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Models":
                    Helpers.InvalidateCheckedListBox(CheckedList_Models_Arcs, true, selectAll);
                    break;

                case "Grid_Textures":
                    Helpers.InvalidateCheckedListBox(CheckedList_Textures_Arcs, true, selectAll);
                    break;

                case "Grid_Audio":
                    switch (TabControl_Audio.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Audio_Songs, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_Audio_SFX, true, selectAll); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Text":
                    switch (TabControl_Text.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Text_Languages, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_Text_Buttons, true, selectAll); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Miscellaneous":
                    switch (TabControl_Miscellaneous.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Misc_Patches, true, selectAll); break;
                        case 1:
                            CheckBox_Misc_SonicVH.IsChecked = selectAll;
                            CheckBox_Misc_ShadowVH.IsChecked = selectAll;
                            CheckBox_Misc_SilverVH.IsChecked = selectAll;
                            break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Custom": Helpers.InvalidateCheckedListBox(CheckedList_Custom_Vox, true, selectAll); break;

                case "Grid_Wildcard":
                    switch (TabControl_Wildcard.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_SET, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Event, true, selectAll); break;
                        case 2: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Scene, true, selectAll); break;
                        case 3: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Animations, true, selectAll); break;
                        case 4: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Models, true, selectAll); break;
                        case 5: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Textures, true, selectAll); break;
                        case 6: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Audio, true, selectAll); break;
                        case 7: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Text, true, selectAll); break;
                        case 8: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_XNCP, true, selectAll); break;
                        case 9: Helpers.InvalidateCheckedListBox(CheckedList_Wildcard_Miscellaneous, true, selectAll); break;
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Add text as an item to the ListView_ProgressLogger element and ensure it's in view.
        /// </summary>
        /// <param name="text">The text to add to the element.</param>
        private void UpdateLogger(string text)
        {
            // Silly Maxis Joke Checker, only allowed if Easter Egg Seeds are on.
            if (CheckBox_General_NoFun.IsChecked == false)
            {
                // Setup a boolean to check if we've already dumped the Reticulating splines message in.
                bool alreadyMadeTheJoke = false;

                // Loop through to check for the Reticulating splines message, if we find it, then flag the boolean.
                foreach (string entry in ProgressLogger)
                    if (entry == "Reticulating splines")
                        alreadyMadeTheJoke = true;

                // If we haven't already made the joke, then proceed.
                if (!alreadyMadeTheJoke)
                {
                    // Roll a number, if it's 24, then add Reticulating splines instead of the intended message.
                    if (Randomiser.Next(0, 1001) == 24)
                    {
                        ProgressLogger.Add("Reticulating splines");
                        ListView_ProgressLogger.ScrollIntoView(ListView_ProgressLogger.Items[ListView_ProgressLogger.Items.Count - 1]);
                        return;
                    }
                }
            }

            ProgressLogger.Add(text);
            ListView_ProgressLogger.ScrollIntoView(ListView_ProgressLogger.Items[ListView_ProgressLogger.Items.Count - 1]);
        }
        
        /// <summary>
        /// Updates the list of found Voice Packs.
        /// </summary>
        private void RefreshVoicePacks()
        {
            // Clear out existing ones to be safe.
            CheckedList_Custom_Vox.Items.Clear();

            // Get all the voice pack zip files in the Voice Packs directory.
            string[] voicePacks = Directory.GetFiles($@"{Environment.CurrentDirectory}\VoicePacks", "*.zip", SearchOption.TopDirectoryOnly);

            // Loop through and add the name of each pack to the CheckedList_Custom_Vox element.
            foreach (string voicePack in voicePacks)
            {
                CheckedListBoxItem item = new()
                {
                    DisplayName = Path.GetFileNameWithoutExtension(voicePack),
                    Tag = Path.GetFileNameWithoutExtension(voicePack),
                    Checked = false
                };
                CheckedList_Custom_Vox.Items.Add(item);
            }
        }
        #endregion

        #region Bottom Buttons
        /// <summary>
        /// Opens the GitHub wiki for the Randomiser in the user's default Web Browser.
        /// </summary>
        private void Button_Documentation(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = @"https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite/wiki",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        /// <summary>
        /// Opens the About Message Box
        /// </summary>
        private void Button_About(object sender, RoutedEventArgs e)
        {
            HandyControl.Controls.MessageBox.Show($"Sonic '06 Randomiser Suite ({VersionNumber}) Credits:\n\n" +
                                                  "Knuxfan24: Development, Marathon.\n" +
                                                  "HyperBE32: Marathon.\n" +
                                                  "Sajid: Marathon Lua Decompilation.\n" +
                                                  "ShadowLAG: Original Lua Decompilation Source.\n" +
                                                  "vgmstream: Audio Conversion.\n" +
                                                  "Microsoft: xmaencode and texconv utilities.\n" +
                                                  "HandyControl: WPF Form Controls.\n" +
                                                  "Skyth: Sonic Audio Tools.\n" +
                                                  "dwyl: Plain Text List of English Words.\n" +
                                                  "crash5band: XNCPLib.",
                                                  "Sonic '06 Randomiser Suite",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Information);
        }

        /// <summary>
        /// Opens a File Browser to select where to save a config ini to.
        /// </summary>
        private void SaveConfig_Button(object sender, RoutedEventArgs e)
        {
            VistaSaveFileDialog configSaveBrowser = new()
            {
                Filter = "Randomiser Config (*.ini)|*.ini",
                RestoreDirectory = true
            };

            // Run the SaveConfig funciton if the file location specified is valid.
            if (configSaveBrowser.ShowDialog() == true)
                SaveConfig(configSaveBrowser.FileName);
        }

        /// <summary>
        /// Opens a File Browser to select a config ini to load.
        /// </summary>
        private void LoadConfig_Button(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog OpenFileDialog = new()
            {
                Title = "Select Configuration INI",
                Multiselect = false,
                Filter = "Randomiser Config (*.ini)|*.ini|Wildcard Log (*.log)|*.log"
            };

            if (OpenFileDialog.ShowDialog() == true)
                LoadConfig(OpenFileDialog.FileName);
        }
        #endregion

        #region Config Saving/Loading
        /// <summary>
        /// Saves a configuration ini.
        /// </summary>
        /// <param name="location">The filepath we're saving to.</param>
        private void SaveConfig(string location)
        {
            // Append a .ini if that or .log isn't present. Thanks Winderps.
            if (!location.EndsWith(".ini") && !location.EndsWith(".log"))
                location += ".ini";

            // Set up our StreamWriter.
            StreamWriter configInfo = new(File.Open(location, FileMode.Create));

            // Basic Header, used to identify in the loading process.
            configInfo.WriteLine($"[Sonic '06 Randomiser Suite Configuration File]");
            configInfo.WriteLine();

            // General Block.
            configInfo.WriteLine($"[General]");
            configInfo.WriteLine($"TextBox_General_ModsDirectory={TextBox_General_ModsDirectory.Text}");
            configInfo.WriteLine($"TextBox_General_GameExecutable={TextBox_General_GameExecutable.Text}");
            configInfo.WriteLine($"TextBox_General_Seed={TextBox_General_Seed.Text}");
            configInfo.WriteLine();

            // Object Placement Block.
            ConfigTabRead(configInfo, "Object Placement", StackPanel_ObjectPlacement);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_EnemyTypes);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_Characters);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_ItemCapsules);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_CommonProps);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_PathProps);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_Hints);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_Doors);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_Particles);
            ConfigCheckedListBoxRead(configInfo, CheckedList_SET_ObjectShuffle);
            configInfo.WriteLine();

            // Event Block.
            ConfigTabRead(configInfo, "Event", StackPanel_Event);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Event_Lighting);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Event_Terrain);
            configInfo.WriteLine();

            // Scene Block.
            ConfigTabRead(configInfo, "Scene", StackPanel_Scene);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Scene_EnvMaps);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Scene_Skyboxes);
            configInfo.WriteLine();

            // Animation Block.
            ConfigTabRead(configInfo, "Animations", StackPanel_Animation);
            configInfo.WriteLine();

            // Models Block.
            ConfigTabRead(configInfo, "Models", StackPanel_Models);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Models_Arcs);
            configInfo.WriteLine();

            // Textures Block.
            ConfigTabRead(configInfo, "Textures", StackPanel_Textures);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Textures_Arcs);
            configInfo.WriteLine();

            // Audio Block.
            ConfigTabRead(configInfo, "Audio", StackPanel_Audio);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Audio_Songs);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Audio_SFX);
            configInfo.WriteLine();

            // Text Block.
            ConfigTabRead(configInfo, "Text", StackPanel_Text);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Text_Languages);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Text_Buttons);
            configInfo.WriteLine();

            // User Interface Block.
            ConfigTabRead(configInfo, "UI", StackPanel_XNCP);
            configInfo.WriteLine();

            // Misc Block.
            ConfigTabRead(configInfo, "Misc", StackPanel_Misc);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Misc_Patches);
            configInfo.WriteLine($"TextBox_Misc_SonicVHLocation={TextBox_Misc_SonicVHLocation.Text}");
            configInfo.WriteLine($"{CheckBox_Misc_SonicVH.Name}={CheckBox_Misc_SonicVH.IsChecked}");
            configInfo.WriteLine($"TextBox_Misc_ShadowVHLocation={TextBox_Misc_ShadowVHLocation.Text}");
            configInfo.WriteLine($"{CheckBox_Misc_ShadowVH.Name}={CheckBox_Misc_ShadowVH.IsChecked}");
            configInfo.WriteLine($"TextBox_Misc_SilverVHLocation={TextBox_Misc_SilverVHLocation.Text}");
            configInfo.WriteLine($"{CheckBox_Misc_SilverVH.Name}={CheckBox_Misc_SilverVH.IsChecked}");
            configInfo.WriteLine();

            // Custom Block.
            configInfo.WriteLine($"[Custom]");
            configInfo.WriteLine($"TextBox_Custom_Music={TextBox_Custom_Music.Text}");
            configInfo.WriteLine($"CheckBox_Custom_Music_XMACache={CheckBox_Custom_Music_XMACache.IsChecked}");
            configInfo.WriteLine($"TextBox_Custom_Textures={TextBox_Custom_Textures.Text}");
            ConfigCheckedListBoxRead(configInfo, CheckedList_Custom_Vox);

            // End Write.
            configInfo.Close();
        }

        /// <summary>
        /// Reads all the check box states and numeric up down reels from a tab and adds them to the config writer.
        /// </summary>
        /// <param name="configInfo">The StreamWriter from the main save function.</param>
        /// <param name="sectionHeader">The section header to write.</param>
        /// <param name="element">The StackPanel we're looking through.</param>
        private static void ConfigTabRead(StreamWriter configInfo, string sectionHeader, DependencyObject element)
        {
            // Write the header for this section of the ini.
            configInfo.WriteLine($"[{sectionHeader}]");

            // Get all the children of this StackPanel element.
            IEnumerable? children = LogicalTreeHelper.GetChildren(element);

            // Loop through each item in this StackPanel element.
            foreach (object? item in children)
            { 
                // If this is a Checkbox element, write the name and checked state.
                if (item is CheckBox checkbox)
                    configInfo.WriteLine($"{checkbox.Name}={checkbox.IsChecked}");

                // If this is a NumericUpDown element, write the name and value.
                if (item is HandyControl.Controls.NumericUpDown numeric)
                    configInfo.WriteLine($"{numeric.Name}={numeric.Value}");
            }
        }

        /// <summary>
        /// Reads data from a CheckedListBox element to add to the configuration ini.
        /// </summary>
        /// <param name="configInfo">The StreamWriter from the main save function.</param>
        /// <param name="listBox">The CheckedListBox to parse.</param>
        private static void ConfigCheckedListBoxRead(StreamWriter configInfo, CheckedListBox listBox)
        {
            // Set up the key.
            string typeList = $"{listBox.Name}=";

            // Loop through the CheckedListBox element.
            for (int i = 0; i < listBox.Items.Count; i++)
            {
                // If this element is checked, add its tag to the list.
                if (listBox.Items[i].Checked)
                    typeList += $"{listBox.Items[i].Tag},";
            }

            // Remove the last comma.
            if (typeList.Contains(','))
                typeList = typeList.Remove(typeList.LastIndexOf(','));

            // Write this list to the ini.
            configInfo.WriteLine(typeList);
        }

        /// <summary>
        /// Loads and updates settings from a configuration ini.
        /// </summary>
        /// <param name="location">The config ini to load.</param>
        /// <exception cref="NotImplementedException">Thrown if we just cannot find element or its type.</exception>
        private void LoadConfig(string location)
        {
            // Read the config file into a string array.
            string[] config = File.ReadAllLines(location);

            // Check that the first thing in the config file is our header. If not, abort.
            if (config[0] != "[Sonic '06 Randomiser Suite Configuration File]")
            {
                HandyControl.Controls.MessageBox.Show($"'{location}' does not appear to be a valid configuration file.",
                                                      "Sonic '06 Randomiser Suite",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Error);
                return;
            }

            // Loop through each line in the string array.
            foreach (string setting in config)
            {
                // Ignore comment values (currently don't write any, but could do), empty lines and section tags.
                if (setting.StartsWith(';') || setting == "" || setting.StartsWith('[') || setting.EndsWith(']'))
                    continue;

                // Split this line so we can get the key and the value(s).
                var split = setting.Split('=');

                // Search for this key's name.
                object element = Grid_General.FindName(split[0]);

                // If we find this key's name, continue on.
                if (element != null)
                {                    
                    // If this element is a check box, check it or uncheck it depending on the key value.
                    if (element is CheckBox checkbox)
                    {
                        checkbox.IsChecked = bool.Parse(split[1]);
                        Dependency_CheckBox_Changed(checkbox, null);
                    }

                    // If this element is a numericupdown, set its value depending on the key's.
                    if (element is HandyControl.Controls.NumericUpDown numeric)
                        numeric.Value = double.Parse(split[1]);

                    // If this element is a text box, fill it in with the key's value.
                    if (element is TextBox textbox)
                        textbox.Text = split[1];

                    // If this element is a checkedlistbox, invalidate the existing list, loop through and check ones that have the tags specified in the list.
                    if (element is CheckedListBox checkedlist)
                    {
                        string[] checkedlistValues = split[1].Split(',');
                        Helpers.InvalidateCheckedListBox(checkedlist, true, false);

                        foreach(string value in checkedlistValues)
                        {
                            foreach (var item in checkedlist.Items)
                            {
                                if (item.Tag == value)
                                    item.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Wildcard
        /// <summary>
        /// Scans through a Tab and checks elements based on the Wildcard weight and wether the element is valid.
        /// </summary>
        /// <param name="element">The element (usually a Grid) to process.</param>
        /// <param name="wildcardWeight">The weight value of the Wildcard.</param>
        /// <param name="listBox">The Wildcard CheckedListBox for this tab.</param>
        /// <param name="disabled">Whether this option is enabled at all.</param>
        private static void WildcardTabCheckboxes(DependencyObject element, int wildcardWeight, CheckedListBox listBox, bool disabled = false)
        {
            // Loop through all the CheckBoxes in our element.
            foreach (var checkbox in Helpers.Descendants<CheckBox>(element))
            {
                // Set it to false by default.
                checkbox.IsChecked = false;

                if (!disabled)
                {
                    // Scan through the Wildcard Elements to see if we need to try enable this element.
                    foreach (var item in listBox.Items)
                    {
                        if (item.Tag == checkbox.Name && item.Checked == true)
                        {
                            // Roll a number, if it's less than or equal to the Wildcard's weight then enable it.
                            if (Randomiser.Next(0, 101) <= wildcardWeight)
                                checkbox.IsChecked = true;
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// Loops through a CheckedListBox element and enables or disables the values within it.
        /// </summary>
        /// <param name="listbox">The CheckedListBox element to process.</param>
        /// <param name="wildcardWeight">The likelyhood the Wildcard will toggle an option on.</param>
        private static void WildcardCheckedList(CheckedListBox listbox, int wildcardWeight)
        {
            // Uncheck everything in the CheckedListBox.
            Helpers.InvalidateCheckedListBox(listbox, true, false);

            // Loop through each item in the CheckedListBox, roll a number to determine what to do with it.
            foreach (CheckedListBoxItem? item in listbox.Items)
            {
                if (Randomiser.Next(0, 101) <= wildcardWeight)
                    item.Checked = true;
            }
        }

        /// <summary>
        /// Randomly generate a number for a NumericUpDown element, if the min and max values are not specified then generate a float between 0 and 1.
        /// </summary>
        /// <param name="updown">The element to generate a value for.</param>
        /// <param name="min">The minimum valid number, if a float is passed in then a floating point number between the two will be returned instead of a whole number.</param>
        /// <param name="max">The maximum valid number, if a float is passed in then a floating point number between the two will be returned instead of a whole number.</param>
        private static void WildcardNumericUpDown(HandyControl.Controls.NumericUpDown updown, int min, int max)
        {
            updown.Value = Randomiser.Next(min, max);
        }
        private static void WildcardNumericUpDown(HandyControl.Controls.NumericUpDown updown)
        {
            updown.Value = Randomiser.NextDouble();
        }
        private static void WildcardNumericUpDown(HandyControl.Controls.NumericUpDown updown, float min, float max)
        {
            double range = max - min;
            updown.Value = (Randomiser.NextDouble() * range) + min;
        }
        #endregion

        /// <summary>
        /// Actual main Randomisation process.
        /// </summary>
        private async void Randomise(object sender, RoutedEventArgs e)
        {
            // Check that our mods directory and game executable actually exist.
            if (!Directory.Exists(TextBox_General_ModsDirectory.Text) || !File.Exists(TextBox_General_GameExecutable.Text))
            {
                HandyControl.Controls.MessageBox.Show("Either your Game Executable or Mods Directory don't exist, please check your general settings.",
                                                      "Sonic '06 Randomiser Suite",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Error);
                return;
            }

            // Set up a new Randomiser variable with the new seed.
            Randomiser = new Random(TextBox_General_Seed.Text.GetHashCode());

            // Get a list of all the archives based on the location of the game executable.
            string[] archives = Directory.GetFiles($@"{Path.GetDirectoryName(TextBox_General_GameExecutable.Text)}", "*.arc", SearchOption.AllDirectories);

            // Set up our variables for the Mod Directory and Game Executable.
            string ModDirectory = $@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({Helpers.UseSafeFormattedCharacters(TextBox_General_Seed.Text)})";
            string GameExecutable = TextBox_General_GameExecutable.Text;
            string Seed = TextBox_General_Seed.Text;
            bool? DisableEasterEggs = CheckBox_General_NoFun.IsChecked;

            // Determine if we need a xenon folder or a ps3 folder.
            string corePath = "xenon";
            if (GameExecutable.ToLower().EndsWith(".bin"))
                corePath = "ps3";

            // Create Mod Directory (prompting the user if they want to delete it first or cancel if it already exists.)
            if (Directory.Exists(ModDirectory))
            {
                MessageBoxResult check = HandyControl.Controls.MessageBox.Show($"A mod with the seed {TextBox_General_Seed.Text} already exists.\nDo you want to replace it?",
                                                                               "Sonic '06 Randomiser Suite",
                                                                               MessageBoxButton.YesNo,
                                                                               MessageBoxImage.Exclamation);

                if (check == MessageBoxResult.Yes)
                    Directory.Delete(ModDirectory, true);

                if (check == MessageBoxResult.No)
                    return;
            }
            Directory.CreateDirectory(ModDirectory);

            // Write mod configuration ini.
            using (Stream configCreate = File.Open(Path.Combine(ModDirectory, "mod.ini"), FileMode.Create))
            using (StreamWriter configInfo = new(configCreate))
            {
                configInfo.WriteLine("[Details]");
                configInfo.WriteLine($"Title=\"Sonic '06 Randomised ({TextBox_General_Seed.Text})\"");
                configInfo.WriteLine($"Version=\"{GlobalVersionNumber.Replace("Version ", "")}\"");
                configInfo.WriteLine($"Date=\"{DateTime.Now:dd/MM/yyyy}\"");
                configInfo.WriteLine($"Author=\"Sonic '06 Randomiser Suite\"");

                if (TextBox_General_GameExecutable.Text.ToLower().EndsWith(".xex"))
                    configInfo.WriteLine($"Platform=\"Xbox 360\"");

                if (TextBox_General_GameExecutable.Text.ToLower().EndsWith(".bin"))
                    configInfo.WriteLine($"Platform=\"PlayStation 3\"");

                configInfo.WriteLine("\n[Filesystem]");
                configInfo.WriteLine($"Merge=\"False\"");
                configInfo.WriteLine($"CustomFilesystem=\"False\"");

                configInfo.Close();
            }

            // Display the Progress Logger elements and disable bottom buttons that shouldn't be useable during the process.
            ProgressLogger.Clear();
            TabControl_Main.Visibility = Visibility.Hidden;
            ListView_ProgressLogger.Visibility = Visibility.Visible;
            ProgressBar_ProgressLogger.Visibility = Visibility.Visible;
            Button_Randomise.IsEnabled = false;
            Button_LoadConfig.IsEnabled = false;

            // Wildcard Setup
            if (CheckBox_Wildcard_Enable.IsChecked == true)
            {
                // Save user's config so we can make it seemlessTM.
                SaveConfig(Path.Combine(ModDirectory, "wildcard.bak"));

                // Disable all values first.
                WildcardTabCheckboxes(Grid_ObjectPlacement, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_SET, true);
                WildcardTabCheckboxes(Grid_Event, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Event, true);
                WildcardTabCheckboxes(Grid_Scene, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Scene, true);
                WildcardTabCheckboxes(Grid_Animations, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Animations, true);
                WildcardTabCheckboxes(Grid_Models, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Models, true);
                WildcardTabCheckboxes(Grid_Textures, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Textures, true);
                WildcardTabCheckboxes(Grid_Audio, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Audio, true);
                WildcardTabCheckboxes(Grid_Text, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Text, true);
                WildcardTabCheckboxes(Grid_XNCP, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_XNCP, true);
                WildcardTabCheckboxes(Grid_Miscellaneous, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Miscellaneous, true);

                // Go through and configure elements based on the Wildcard.
                if (CheckBox_Wildcard_SET.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_ObjectPlacement, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_SET);
                    WildcardCheckedList(CheckedList_SET_EnemyTypes, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_Characters, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_ItemCapsules, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_CommonProps, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_PathProps, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_Hints, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_Doors, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_SET_Particles, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardNumericUpDown(NumericUpDown_SET_DrawDistance_Min, 0, 10000);
                    WildcardNumericUpDown(NumericUpDown_SET_DrawDistance_Max, (int)NumericUpDown_SET_DrawDistance_Min.Value, 10000);
                    WildcardCheckedList(CheckedList_SET_ObjectShuffle, (int)NumericUpDown_Wildcard_Weight.Value);
                }

                if (CheckBox_Wildcard_Event.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Event, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Event);
                    WildcardCheckedList(CheckedList_Event_Lighting, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_Event_Terrain, (int)NumericUpDown_Wildcard_Weight.Value);
                }

                if (CheckBox_Wildcard_Scene.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Scene, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Scene);
                    WildcardCheckedList(CheckedList_Scene_EnvMaps, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_Scene_Skyboxes, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardNumericUpDown(NumericUpDown_Scene_MinStrength);
                }

                if (CheckBox_Wildcard_Animations.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Animations, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Animations);
                }

                if (CheckBox_Wildcard_Models.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Models, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Models);
                    WildcardCheckedList(CheckedList_Models_Arcs, (int)NumericUpDown_Wildcard_Weight.Value);
                }

                if (CheckBox_Wildcard_Textures.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Textures, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Textures);
                    WildcardCheckedList(CheckedList_Textures_Arcs, (int)NumericUpDown_Wildcard_Weight.Value);
                }

                if (CheckBox_Wildcard_Audio.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Audio, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Audio);
                    WildcardCheckedList(CheckedList_Audio_Songs, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_Audio_SFX, (int)NumericUpDown_Wildcard_Weight.Value);
                }

                if (CheckBox_Wildcard_Text.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Text, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Text);
                    WildcardCheckedList(CheckedList_Text_Languages, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardCheckedList(CheckedList_Text_Buttons, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardNumericUpDown(NumericUpDown_Text_Colour_Weight, 0, 100);
                }

                if (CheckBox_Wildcard_XNCP.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_XNCP, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Animations);
                    WildcardNumericUpDown(NumericUpDown_XNCP_Scale_Min, 0.25f, 1.75f);
                    WildcardNumericUpDown(NumericUpDown_XNCP_Scale_Max, 0.25f, 1.75f);
                }

                if (CheckBox_Wildcard_Miscellaneous.IsChecked == true)
                {
                    WildcardTabCheckboxes(Grid_Miscellaneous, (int)NumericUpDown_Wildcard_Weight.Value, CheckedList_Wildcard_Miscellaneous);
                    WildcardCheckedList(CheckedList_Misc_Patches, (int)NumericUpDown_Wildcard_Weight.Value);
                    WildcardNumericUpDown(NumericUpDown_Misc_EnemyHealth_Min, 0, 100);
                    WildcardNumericUpDown(NumericUpDown_Misc_EnemyHealth_Max, (int)NumericUpDown_Misc_EnemyHealth_Min.Value, 100);
                    WildcardNumericUpDown(NumericUpDown_Misc_Patches_Weight, 0, 100);
                }
            }

            // Enumerate the Checked List Boxes for the user's settings on lists.
            List<string> SetEnemies = Helpers.EnumerateCheckedListBox(CheckedList_SET_EnemyTypes);
            List<string> SetCharacters = Helpers.EnumerateCheckedListBox(CheckedList_SET_Characters);
            List<string> SetItemCapsules = Helpers.EnumerateCheckedListBox(CheckedList_SET_ItemCapsules);
            List<string> SetCommonProps = Helpers.EnumerateCheckedListBox(CheckedList_SET_CommonProps);
            List<string> SetPathProps = Helpers.EnumerateCheckedListBox(CheckedList_SET_PathProps);
            List<string> SetHints = Helpers.EnumerateCheckedListBox(CheckedList_SET_Hints);
            List<string> SetDoors = Helpers.EnumerateCheckedListBox(CheckedList_SET_Doors);
            List<string> SetParticleBanks = Helpers.EnumerateCheckedListBox(CheckedList_SET_Particles);
            List<string> SetShuffleBlacklist = Helpers.EnumerateCheckedListBox(CheckedList_SET_ObjectShuffle);

            List<string> EventLighting = Helpers.EnumerateCheckedListBox(CheckedList_Event_Lighting);
            List<string> EventTerrain = Helpers.EnumerateCheckedListBox(CheckedList_Event_Terrain);

            List<string> SceneEnvMaps = Helpers.EnumerateCheckedListBox(CheckedList_Scene_EnvMaps);
            List<string> SceneSkyboxes = Helpers.EnumerateCheckedListBox(CheckedList_Scene_Skyboxes);

            List<string> ModelsArchives = Helpers.EnumerateCheckedListBox(CheckedList_Models_Arcs);

            List<string> TexturesArchives = Helpers.EnumerateCheckedListBox(CheckedList_Textures_Arcs);

            List<string> AudioMusic = Helpers.EnumerateCheckedListBox(CheckedList_Audio_Songs);
            List<string> AudioCSBs = Helpers.EnumerateCheckedListBox(CheckedList_Audio_SFX);

            List<string> TextLanguages = Helpers.EnumerateCheckedListBox(CheckedList_Text_Languages);
            List<string> TextButtons = Helpers.EnumerateCheckedListBox(CheckedList_Text_Buttons);

            List<string> MiscPatches = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Patches);

            string[] CustomMusic = TextBox_Custom_Music.Text.Split('|');
            string[] CustomTextures = TextBox_Custom_Textures.Text.Split('|');
            List<string> CustomVoxPacks = Helpers.EnumerateCheckedListBox(CheckedList_Custom_Vox);

            // Don't do the Custom Audio stuff if we're using a PS3 version
            if (GameExecutable.ToLower().EndsWith(".xex"))
            {
                // Wildcard Custom Overrides
                if (CheckBox_Wildcard_Enable.IsChecked == true)
                {
                    if (TextBox_Custom_Music.Text.Length != 0)
                        CheckBox_Audio_Music.IsChecked = true;

                    if (TextBox_Custom_Textures.Text.Length != 0)
                        CheckBox_Textures_Textures.IsChecked = true;

                    if (CustomVoxPacks.Count > 0)
                        CheckBox_SET_Hints.IsChecked = true;
                }

                // Custom Music
                if (TextBox_Custom_Music.Text.Length != 0)
                {
                    // Get the status of the XMA Cache Checkbox.
                    bool? EnableCache = CheckBox_Custom_Music_XMACache.IsChecked;

                    // Create the directories for the process.
                    Directory.CreateDirectory($@"{TemporaryDirectory}\tempWavs");
                    Directory.CreateDirectory($@"{ModDirectory}\xenon\sound");

                    // Set up the string for the custom files in the mod.ini
                    string songs = "Custom=\"";

                    // Loops through the custom songs and process them.
                    for (int i = 0; i < CustomMusic.Length; i++)
                    {
                        UpdateLogger($"Importing: '{CustomMusic[i]}' as custom music.");
                        await Task.Run(() => Custom.Music(CustomMusic[i], ModDirectory, i, EnableCache));
                        songs += $"custom{i}.xma,";
                        AudioMusic.Add($"custom{i}");
                    }

                    // Add all the songs to the mod configuration ini.
                    // Remove the last comma and replace it with a closing quote.
                    songs = songs.Remove(songs.LastIndexOf(','));
                    songs += "\"";

                    // Write the list of custom files to the mod configuration ini.
                    using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{ModDirectory}", "mod.ini")))
                    {
                        configInfo.WriteLine(songs);
                        configInfo.Close();
                    }

                    // Add all the songs to bgm.sbk in sound.arc
                    foreach (string archive in archives)
                    {
                        // Find sound.arc.
                        if (Path.GetFileName(archive).ToLower() == "sound.arc")
                        {
                            UpdateLogger($"Updating 'bgm.sbk' with {CustomMusic.Length} custom songs.");
                            string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            await Task.Run(() => Custom.UpdateBGMSoundBank(unpackedArchive, CustomMusic.Length));
                        }
                    }
                }

                // Voice Packs
                if (CustomVoxPacks.Count > 0)
                {
                    // Create voice directory.
                    Directory.CreateDirectory($@"{ModDirectory}\xenon\sound\voice\e\");

                    // Process the selected voice packs.
                    for (int i = 0; i < CustomVoxPacks.Count; i++)
                    {
                        UpdateLogger($"Processing '{CustomVoxPacks[i]}' voice pack.");
                        bool success = await Task.Run(() => Custom.VoicePacks(CustomVoxPacks[i], ModDirectory, archives, SetHints));
                        if (!success)
                            UpdateLogger($"Ignored '{CustomVoxPacks[i]}' as it does not appear to be a voice pack.");
                    }
                }
            }

            // Custom Textures.
            List<string> CustomTextureFiles = new();
            if (TextBox_Custom_Textures.Text.Length != 0)
            {
                // Create the temp directory to save converted DDS files into.
                Directory.CreateDirectory($@"{TemporaryDirectory}\tempDDS");

                for (int i = 0; i < CustomTextures.Length; i++)
                {
                    UpdateLogger($"Importing '{CustomTextures[i]}' as a custom texture.");
                    CustomTextureFiles.Add(await Task.Run(() => Custom.Texture(CustomTextures[i])));
                }
            }

            // Disable options if they have nothing to pick from.
            if (SetEnemies.Count == 0)
                CheckBox_SET_Enemies.IsChecked = false;
            if (SetCharacters.Count == 0)
                CheckBox_SET_Characters.IsChecked = false;
            if (SetItemCapsules.Count == 0)
                CheckBox_SET_ItemCapsules.IsChecked = false;
            if (SetCommonProps.Count == 0)
                CheckBox_SET_CommonProps.IsChecked = false;
            if (SetPathProps.Count == 0)
                CheckBox_SET_PathProps.IsChecked = false;
            if (SetHints.Count == 0)
                CheckBox_SET_Hints.IsChecked = false;
            if (SetDoors.Count == 0)
                CheckBox_SET_Doors.IsChecked = false;
            if (SetParticleBanks.Count == 0)
                CheckBox_SET_Particles.IsChecked = false;

            if (EventLighting.Count == 0)
                CheckBox_Event_Lighting.IsChecked = false;
            if (EventTerrain.Count == 0)
                CheckBox_Event_Terrain.IsChecked = false;

            if (SceneEnvMaps.Count == 0)
                CheckBox_Scene_EnvMaps.IsChecked = false;
            if (SceneSkyboxes.Count == 0)
                CheckBox_Scene_Skyboxes.IsChecked = false;

            if (ModelsArchives.Count == 0)
            {
                CheckBox_Models_VertexColour.IsChecked = false;
                CheckBox_Models_MaterialColour.IsChecked = false;
            }

            if (TexturesArchives.Count == 0)
                CheckBox_Textures_Textures.IsChecked = false;

            if (AudioMusic.Count == 0)
                CheckBox_Audio_Music.IsChecked = false;
            if (AudioCSBs.Count == 0)
                CheckBox_Audio_SFX.IsChecked = false;

            if (TextLanguages.Count == 0)
            {
                CheckBox_Text_Shuffle.IsChecked = false;
                CheckBox_Text_Generate.IsChecked = false;
                CheckBox_Text_Buttons.IsChecked = false;
            }
            if (TextButtons.Count == 0)
                CheckBox_Text_Buttons.IsChecked = false;

            if (MiscPatches.Count == 0)
                CheckBox_Misc_Patches.IsChecked = false;

            #region Random Episode Generation
            // This has to be done up here or the Very Hard mode content won't be randomised.
            bool? miscRandomEpisode = CheckBox_Misc_RandomEpisode.IsChecked;

            // Set up a level order for later.
            Dictionary<string, int> LevelOrder = new();

            // Check if the Random Episode needs to be generated.
            if (miscRandomEpisode == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        // Definie DLC paths.
                        string? sonicVH = null;
                        string? shadowVH = null;
                        string? silverVH = null;

                        if (Path.GetExtension(TextBox_Misc_SonicVHLocation.Text) == ".arc" && CheckBox_Misc_SonicVH.IsChecked == true)
                            sonicVH = TextBox_Misc_SonicVHLocation.Text;
                        if (Path.GetExtension(TextBox_Misc_ShadowVHLocation.Text) == ".arc" && CheckBox_Misc_ShadowVH.IsChecked == true)
                            shadowVH = TextBox_Misc_ShadowVHLocation.Text;
                        if (Path.GetExtension(TextBox_Misc_SilverVHLocation.Text) == ".arc" && CheckBox_Misc_SilverVH.IsChecked == true)
                            silverVH = TextBox_Misc_SilverVHLocation.Text;

                        UpdateLogger($"Generating random episode.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        LevelOrder = await Task.Run(() => MiscellaneousRandomisers.EpisodeGenerator(unpackedArchive, GameExecutable, sonicVH, shadowVH, silverVH));
                    }
                }
            }
            #endregion

            #region Object Placement
            // Set up values.
            bool? setEnemies = CheckBox_SET_Enemies.IsChecked;
            bool? setEnemiesNoBosses = CheckBox_SET_Enemies_NoBosses.IsChecked;
            bool? setBehaviour = CheckBox_SET_Enemies_Behaviour.IsChecked;
            bool? setBehaviourNoEnforce = CheckBox_SET_Enemies_Behaviour_NoEnforce.IsChecked;
            bool? setCharacters = CheckBox_SET_Characters.IsChecked;
            bool? setItemCapsules = CheckBox_SET_ItemCapsules.IsChecked;
            bool? setCommonProps = CheckBox_SET_CommonProps.IsChecked;
            bool? setPathProps = CheckBox_SET_PathProps.IsChecked;
            bool? setHints = CheckBox_SET_Hints.IsChecked;
            bool? setDoors = CheckBox_SET_Doors.IsChecked;
            bool? setDrawDistance = CheckBox_SET_DrawDistance.IsChecked;
            bool? setCosmetic = CheckBox_SET_Cosmetic.IsChecked;
            bool? setParticles = CheckBox_SET_Particles.IsChecked;
            bool? setJumpboards = CheckBox_SET_Jumpboards.IsChecked;
            int setMinDrawDistance = (int)NumericUpDown_SET_DrawDistance_Min.Value;
            int setMaxDrawDistance = (int)NumericUpDown_SET_DrawDistance_Max.Value;
            int setJumpboardsChance = (int)NumericUpDown_SET_Jumpboards_Chance.Value;
            bool? setTransform = CheckBox_SET_PlacementShuffle.IsChecked;

            // Check if we actually need to do SET stuff.
            if (setEnemies == true || setBehaviour == true || setCharacters == true || setItemCapsules == true || setCommonProps == true || setPathProps == true || setHints == true || setDoors == true||
                setDrawDistance == true || setCosmetic == true || setParticles == true || setJumpboards == true || setTransform == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        // Get a list of all the set files in scripts.arc.
                        string[] setFiles = Directory.GetFiles(unpackedArchive, "*.set", SearchOption.AllDirectories);

                        // Process each set file.
                        foreach (string setFile in setFiles)
                        {
                            UpdateLogger($"Randomising: '{setFile}'.");
                            await Task.Run(() => ObjectPlacementRandomiser.Process(setFile, setEnemies, setEnemiesNoBosses, setBehaviour, setBehaviourNoEnforce, setCharacters, setItemCapsules,
                                                                                   setCommonProps, setPathProps, setHints, setDoors, setDrawDistance, setCosmetic, setParticles, setJumpboards, SetEnemies,
                                                                                   SetCharacters, SetItemCapsules, SetCommonProps, SetPathProps, SetHints, SetDoors, SetParticleBanks, setMinDrawDistance,
                                                                                   setMaxDrawDistance, setJumpboardsChance, setTransform, SetShuffleBlacklist));
                        }

                        // Patch enemy luas if they need patching.
                        if (setEnemies == true && (SetEnemies.Contains("eCerberus") || SetEnemies.Contains("eGenesis") || SetEnemies.Contains("eWyvern") || SetEnemies.Contains("firstiblis") ||
                            SetEnemies.Contains("secondiblis") || SetEnemies.Contains("thirdiblis") || SetEnemies.Contains("firstmefiress") || SetEnemies.Contains("secondmefiress") ||
                            SetEnemies.Contains("solaris01") || SetEnemies.Contains("solaris02")) || setHints == true)
                        {
                            string[] luaFiles = Directory.GetFiles($"{unpackedArchive}\\{corePath}\\scripts\\enemy", "*.lub", SearchOption.TopDirectoryOnly);
                            foreach (string luaFile in luaFiles)
                            {
                                // Skip Mephiles Phase 2 for patching as decompiling his Lua breaks the fight.
                                if (Path.GetFileNameWithoutExtension(luaFile) != "secondmefiress")
                                {
                                    UpdateLogger($"Patching '{luaFile}'.");
                                    await Task.Run(() => ObjectPlacementRandomiser.BossPatch(luaFile, setEnemies, setHints, SetHints, SetEnemies));
                                }
                            }
                        }

                        // The Egg Cerberus is dumb and does its hints lines in the executable instead.
                        // Also changes the Radical Train train lines too because why not?
                        // Offsets are different on the PS3, so this is a 360 only thing.
                        if (setHints == true && TextBox_General_GameExecutable.Text.ToLower().EndsWith(".xex"))
                        {
                            UpdateLogger($"Writing Hybrid Patch for hardcoded voice lines.");

                            // Determine what hint strings to use, as this is an XEX patch, we only have 19 characters free.
                            List<string> hintsToUse = new();
                            for (int i = 0; i < 20; i++)
                            {
                                string hint = SetHints[Randomiser.Next(SetHints.Count)];
                                do { hint = SetHints[Randomiser.Next(SetHints.Count)]; }
                                while (hint.Length > 19);
                                hintsToUse.Add(hint);
                            }

                            // Write the actual Hybrid Patch
                            using Stream patchCreate = File.Open(Path.Combine(ModDirectory, "patch.mlua"), FileMode.Create);
                            using StreamWriter patchInfo = new(patchCreate);
                            patchInfo.WriteLine("--[Patch]--");
                            patchInfo.WriteLine($"Title(\"Sonic '06 Randomised ({TextBox_General_Seed.Text})\")");
                            patchInfo.WriteLine($"Author(\"Sonic '06 Randomiser Suite\")");
                            patchInfo.WriteLine($"Platform(\"Xbox 360\")");

                            patchInfo.WriteLine("\n--[Functions]--");
                            patchInfo.WriteLine($"DecryptExecutable()");
                            patchInfo.WriteLine($"--Radical Train Voice Lines--");
                            Helpers.HybridPatchWriter(patchInfo, 0x1B300, hintsToUse[0]);
                            Helpers.HybridPatchWriter(patchInfo, 0x1B314, hintsToUse[1]);
                            Helpers.HybridPatchWriter(patchInfo, 0x1B328, hintsToUse[2]);
                            Helpers.HybridPatchWriter(patchInfo, 0x1B33C, hintsToUse[3]);
                            Helpers.HybridPatchWriter(patchInfo, 0x1B350, hintsToUse[4]);
                            patchInfo.WriteLine($"\n--Egg Cerberus Voice Lines--");
                            Helpers.HybridPatchWriter(patchInfo, 0x288A4, hintsToUse[5]);
                            Helpers.HybridPatchWriter(patchInfo, 0x288B8, hintsToUse[6]);
                            Helpers.HybridPatchWriter(patchInfo, 0x288CC, hintsToUse[7]);
                            Helpers.HybridPatchWriter(patchInfo, 0x288E0, hintsToUse[8]);
                            Helpers.HybridPatchWriter(patchInfo, 0x288F4, hintsToUse[9]);
                            Helpers.HybridPatchWriter(patchInfo, 0x28908, hintsToUse[10]);
                            Helpers.HybridPatchWriter(patchInfo, 0x2891C, hintsToUse[11]);
                            Helpers.HybridPatchWriter(patchInfo, 0x28930, hintsToUse[12]);
                            Helpers.HybridPatchWriter(patchInfo, 0x28944, hintsToUse[13]);
                            Helpers.HybridPatchWriter(patchInfo, 0x28958, hintsToUse[14]);
                            Helpers.HybridPatchWriter(patchInfo, 0x2896C, hintsToUse[15]);
                            Helpers.HybridPatchWriter(patchInfo, 0x28980, hintsToUse[16]);
                            Helpers.HybridPatchWriter(patchInfo, 0x28994, hintsToUse[17]);
                            Helpers.HybridPatchWriter(patchInfo, 0x289A8, hintsToUse[18]);
                            Helpers.HybridPatchWriter(patchInfo, 0x289BC, hintsToUse[19]);

                            patchInfo.Close();

                        }

                        // Patch stage and mission luas for player_start2 entities
                        if ((setEnemies == true && setEnemiesNoBosses != true) || setCharacters == true)
                        {
                            string[] luaFiles = Directory.GetFiles(unpackedArchive, "*.lub", SearchOption.AllDirectories);
                            foreach (string luaFile in luaFiles)
                            {
                                if (await Task.Run(() => Helpers.NeededLua(luaFile, new List<string>() { "SetPlayer" })))
                                {
                                    UpdateLogger($"Randomising player_start2 entities in '{luaFile}'.");
                                    await Task.Run(() => ObjectPlacementRandomiser.LuaPlayerStartRandomiser(luaFile, setCharacters, SetCharacters, setEnemies, setEnemiesNoBosses));
                                }
                            }
                        }

                        // Patch stage luas to load all the particle banks.
                        if (setParticles == true)
                        {
                            string[] luaFiles = Directory.GetFiles(unpackedArchive, "*.lub", SearchOption.AllDirectories);
                            foreach (string luaFile in luaFiles)
                            {
                                if (await Task.Run(() => Helpers.NeededLua(luaFile, new List<string>() { "AddComponent" })))
                                {
                                    UpdateLogger($"Patching particle bank loading in '{luaFile}'.");
                                    await Task.Run(() => ObjectPlacementRandomiser.ParticlePatch(luaFile));
                                }
                            }
                        }
                    }

                    if (setTransform == true && (!SetShuffleBlacklist.Contains("eventbox") || !SetShuffleBlacklist.Contains("eventcylinder") || !SetShuffleBlacklist.Contains("eventsphere")))
                    {
                        if (Path.GetFileName(archive).ToLower() == "object.arc")
                        {
                            UpdateLogger($"Adding Event Box Indicator Files.");
                            string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            await Task.Run(() => ObjectPlacementRandomiser.PathObjPatcher(unpackedArchive, corePath));
                        }
                    }
                }
            }
            #endregion

            #region Event Randomisation
            // Set up values.
            bool? eventLighting = CheckBox_Event_Lighting.IsChecked;
            bool? eventRotX = CheckBox_Event_XRotation.IsChecked;
            bool? eventRotY = CheckBox_Event_YRotation.IsChecked;
            bool? eventRotZ = CheckBox_Event_ZRotation.IsChecked;
            bool? eventPosX = CheckBox_Event_XPosition.IsChecked;
            bool? eventPosY = CheckBox_Event_YPosition.IsChecked;
            bool? eventPosZ = CheckBox_Event_ZPosition.IsChecked;
            bool? eventVoice = CheckBox_Event_Voices.IsChecked;
            bool? eventVoiceJpn = CheckBox_Event_Voices_Japanese.IsChecked;
            bool? eventVoiceGame = CheckBox_Event_Voices_Gameplay.IsChecked;
            bool? eventTerrain = CheckBox_Event_Terrain.IsChecked;
            bool? eventOrder = CheckBox_Event_Order.IsChecked;
            bool? eventSkipFMVs = CheckBox_Event_SkipFMVs.IsChecked;

            // Check if we actually need to do event stuff.
            if (eventLighting == true || eventRotX == true || eventRotY == true || eventRotZ == true || eventPosX == true || eventPosY == true || eventPosZ == true || eventTerrain == true ||
                eventOrder == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "cache.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        // Main eventplaybook parameter randomisation.
                        if (eventLighting == true || eventRotX == true || eventRotY == true || eventRotZ == true || eventPosX == true || eventPosY == true || eventPosZ == true || eventTerrain == true)
                        {
                            UpdateLogger($"Randomising 'eventplaybook.epb' parameters.");
                            await Task.Run(() => EventRandomiser.Process(unpackedArchive, corePath, eventLighting, EventLighting, eventTerrain, EventTerrain, eventRotX, eventRotY, eventRotZ, eventPosX, eventPosY,
                                                                         eventPosZ));
                        }

                        // Event Order Shuffling.
                        if (eventOrder == true)
                        {
                            UpdateLogger($"Shuffling event order.");
                            await Task.Run(() => EventRandomiser.EventShuffler(unpackedArchive, corePath, ModDirectory, GameExecutable, eventSkipFMVs));
                        }

                        // Event Voice Line Shuffling.
                        if (eventVoice == true)
                        {
                            UpdateLogger($"Shuffling event voice files.");
                            await Task.Run(() => EventRandomiser.ShuffleVoiceLines(GameExecutable, corePath, eventVoiceJpn, eventVoiceGame, CustomVoxPacks.Count != 0, ModDirectory));
                        }
                    }
                }
            }
            #endregion

            #region Scene Randomisation
            // Set up values.
            bool? sceneLightAmbient = CheckBox_Scene_Light_Ambient.IsChecked;
            bool? sceneLightMain = CheckBox_Scene_Light_Main.IsChecked;
            bool? sceneLightSub = CheckBox_Scene_Light_Sub.IsChecked;
            double sceneMinLight = NumericUpDown_Scene_MinStrength.Value;
            bool? sceneLightDirection = CheckBox_Scene_Light_Direction.IsChecked;
            bool? sceneLightDirectionEnforce = CheckBox_Scene_Light_Direction_Enforce.IsChecked;
            bool? sceneFogColour = CheckBox_Scene_Fog_Colour.IsChecked;
            bool? sceneFogDensity = CheckBox_Scene_Fog_Density.IsChecked;
            bool? sceneEnvMaps = CheckBox_Scene_EnvMaps.IsChecked;
            bool? sceneSkyboxes = CheckBox_Scene_Skyboxes.IsChecked;
            bool? sceneNoBloom = CheckBox_Scene_NoBloom.IsChecked;

            // Check if we actually need to do scene randomisation.
            if (sceneLightAmbient == true || sceneLightMain == true || sceneLightSub == true || sceneLightDirection == true || sceneFogColour == true || sceneFogDensity == true || sceneEnvMaps == true || sceneNoBloom == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        string[] sceneLuas = Directory.GetFiles(unpackedArchive, "scene*.lub", SearchOption.AllDirectories);
                        foreach (string luaFile in sceneLuas)
                        {
                            UpdateLogger($"Randomising scene parameters in '{luaFile}'.");
                            await Task.Run(() => SceneRandomiser.Process(luaFile, sceneLightAmbient, sceneLightMain, sceneLightSub, sceneMinLight, sceneLightDirection, sceneLightDirectionEnforce,
                                                                         sceneFogColour, sceneFogDensity, sceneEnvMaps, SceneEnvMaps, sceneNoBloom));
                        }
                    }
                }
            }

            // Check if we need to do skybox randomisation.
            if (sceneSkyboxes == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        string[] skyLuas = Directory.GetFiles(unpackedArchive, "*.lub", SearchOption.AllDirectories);
                        foreach (string skyLua in skyLuas)
                        {
                            if (await Task.Run(() => Helpers.NeededLua(skyLua, new List<string>() { "AddComponent" })))
                            {
                                UpdateLogger($"Randomising skybox in '{skyLua}'.");
                                await Task.Run(() => SceneRandomiser.SkyboxRandomisation(skyLua, SceneSkyboxes));
                            }
                        }
                    }
                }
            }
            #endregion

            #region Animation Randomisers
            // Set up values.
            bool? animGameplay = CheckBox_Anim_Gameplay.IsChecked;
            bool? animGameplayUseAll = CheckBox_Anim_GameplayUseAll.IsChecked;
            bool? animGameplayUseEvents = CheckBox_Anim_GameplayUseEvents.IsChecked;
            bool? animEvents = CheckBox_Anim_Events.IsChecked;
            bool? animEventsFace = CheckBox_Anim_Events_Face.IsChecked;
            bool? animEventsCamera = CheckBox_Anim_Cameras.IsChecked;
            bool? animFramerate = CheckBox_Anim_Framerate.IsChecked;
            int animMinFramerate = (int)NumericUpDown_Anim_Framerate_Min.Value;
            int animMaxFramerate = (int)NumericUpDown_Anim_Framerate_Max.Value;
            bool? animFramerateNoLights = CheckBox_Anim_Framerate_NoLights.IsChecked;

            // Gameplay.
            if (animGameplay == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "player.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        string[] pkgFiles = Directory.GetFiles(unpackedArchive, "*.pkg", SearchOption.AllDirectories);

                        foreach (string pkgFile in pkgFiles)
                        {
                            UpdateLogger($"Randomising animations in '{pkgFile}'.");
                            await Task.Run(() => AnimationRandomiser.GameplayAnimationRandomiser(pkgFile, GameExecutable, animGameplayUseAll, animGameplayUseEvents));
                        }
                    }
                }
            }

            // Events
            if (animEvents == true || animEventsFace == true || animEventsCamera == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "event_data.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        // Main Event Animations.
                        if (animEvents == true)
                        {
                            UpdateLogger($"Shuffling event animations.");
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "amy", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "blaze", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "eggman", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "knuckles", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "mefiress", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "omega", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "princess", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "princess_child", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "rouge", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "shadow", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "silver", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "sonic", "Root"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "tails", "Root"));
                        }

                        // Facial Animations.
                        if (animEventsFace == true)
                        {
                            UpdateLogger($"Shuffling event facial animations.");
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "amy", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "blaze", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "eggman", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "knuckles", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "mefiress", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "omega", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "princess", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "princess_child", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "rouge", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "shadow", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "silver", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "sonic", "evf_head"));
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "tails", "evf_head"));
                        }

                        if (animEventsCamera == true)
                        {
                            UpdateLogger($"Shuffling event cameras.");
                            await Task.Run(() => AnimationRandomiser.EventAnimationRandomiser(unpackedArchive, "", "", true));
                        }
                    }
                }
            }

            // Framerate
            if (animFramerate == true)
            {
                foreach (string archive in archives)
                {
                    U8Archive arc = new(archive, ReadMode.IndexOnly);
                    IEnumerable<Marathon.IO.Interfaces.IArchiveFile>? arcFiles = arc.Root.GetFiles();
                    foreach (var file in arcFiles)
                    {
                        if (Path.GetExtension(file.Name) == ".xnm" || Path.GetExtension(file.Name) == ".xnv" || Path.GetExtension(file.Name) == ".xni" || Path.GetExtension(file.Name) == ".xnf")
                        {
                            string archivePath = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            string[] motionFiles = Directory.GetFiles(archivePath, "*.xnm", SearchOption.AllDirectories);
                            motionFiles = motionFiles.Concat(Directory.GetFiles(archivePath, "*.xnv", SearchOption.AllDirectories)).ToArray();
                            if (animFramerateNoLights == false) { motionFiles = motionFiles.Concat(Directory.GetFiles(archivePath, "*.xni", SearchOption.AllDirectories)).ToArray(); }
                            motionFiles = motionFiles.Concat(Directory.GetFiles(archivePath, "*.xnf", SearchOption.AllDirectories)).ToArray();
                            foreach (string motionFile in motionFiles)
                            {
                                UpdateLogger($"Randomising framerate in '{motionFile}'.");
                                await Task.Run(() => AnimationRandomiser.AnimationFramerateRandomiser(motionFile, animMinFramerate, animMaxFramerate));
                            }
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Model Randomisers
            // Set up values.
            bool? modelsVertexColours = CheckBox_Models_VertexColour.IsChecked;
            bool? modelsMaterialColours = CheckBox_Models_MaterialColour.IsChecked;
            bool? modelsMaterialDiffuse = CheckBox_Models_MaterialDiffuse.IsChecked;
            bool? modelsMaterialAmbient = CheckBox_Models_MaterialAmbient.IsChecked;
            bool? modelsMaterialSpecular = CheckBox_Models_MaterialSpecular.IsChecked;
            bool? modelsMaterialEmissive = CheckBox_Models_MaterialEmissive.IsChecked;

            // Check if we need to do vertex colour randomisation.
            if (modelsVertexColours == true)
            {
                foreach (string archive in archives)
                {
                    if (ModelsArchives.Contains(Path.GetFileName(archive)))
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        string[] xnoFiles = Directory.GetFiles(unpackedArchive, "*.xno", SearchOption.AllDirectories);
                        foreach (string xnoFile in xnoFiles)
                        {
                            UpdateLogger($"Randomising vertex colours in '{xnoFile}'.");
                            await Task.Run(() => ModelRandomisers.RandomiseVertexColours(xnoFile));
                        }
                    }
                }
            }

            // Check if we need to do material colour randomisation.
            // If none of the sub options are selected, then don't bother, otherwise we'd just be loading and saving an XNO for no reason.
            if (modelsMaterialColours == true && (modelsMaterialDiffuse == true || modelsMaterialAmbient == true || modelsMaterialSpecular == true || modelsMaterialEmissive == true))
            {
                foreach (string archive in archives)
                {
                    if (ModelsArchives.Contains(Path.GetFileName(archive)))
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        string[] xnoFiles = Directory.GetFiles(unpackedArchive, "*.xno", SearchOption.AllDirectories);
                        foreach (string xnoFile in xnoFiles)
                        {
                            UpdateLogger($"Randomising material colours in '{xnoFile}'.");
                            await Task.Run(() => ModelRandomisers.RandomiseMaterialColours(xnoFile, modelsMaterialDiffuse, modelsMaterialAmbient, modelsMaterialSpecular, modelsMaterialEmissive));
                        }
                    }
                }
            }
            #endregion

            #region Texture Randomisation
            // Set up values.
            bool? texturesTextures = CheckBox_Textures_Textures.IsChecked;
            bool? texturesPerArc = CheckBox_Textures_PerArc.IsChecked;
            bool? texturesAllowDupes = CheckBox_Textures_AllowDupes.IsChecked;
            bool? texturesOnlyCustom = CheckBox_Textures_OnlyCustom.IsChecked;
            bool? texturesDelete = CheckBox_Textures_DeleteStages.IsChecked;

            // Dupes are NEEDED if we're only using custom textures.
            if (texturesOnlyCustom == true)
                texturesAllowDupes = true;

            // Check if we're actually doing texture randomisation.
            if (texturesTextures == true)
            {
                // If we're not doing it per arc, then process every single one of valid ones.
                if (texturesPerArc == false)
                {
                    UpdateLogger($"Unpacking archives for Texture Randomisation.");
                    List<string> archivePaths = new();

                    foreach (string archive in archives)
                    {
                        if (TexturesArchives.Contains(Path.GetFileName(archive)))
                            archivePaths.Add(await Task.Run(() => Helpers.ArchiveHandler(archive)));
                    }

                    List<string> Textures = new();

                    // Add our custom textures if we have any.
                    foreach (string custom in CustomTextureFiles)
                        Textures.Add(custom);

                    // If we're not only using custom textures, then fetch all the other textures.
                    if (texturesOnlyCustom == false)
                    {
                        for (int i = 0; i < archivePaths.Count; i++)
                        {
                            UpdateLogger($"Getting textures in '{Path.GetFileName(archivePaths[i])}'.");
                            Textures = await Task.Run(() => TextureRandomiser.FetchTextures(Textures, archivePaths[i]));
                        }
                    }

                    // Randomise the textures.
                    List<int> usedNumbers = new();
                    for (int i = 0; i < archivePaths.Count; i++)
                    {
                        UpdateLogger($"Randomising textures in '{Path.GetFileName(archivePaths[i])}'.");
                        usedNumbers = await Task.Run(() => TextureRandomiser.ShuffleTextures(usedNumbers, archivePaths[i], Textures, texturesAllowDupes));
                    }
                }

                // If we are, then do them one at a time.
                else
                {
                    foreach (string archive in archives)
                    {
                        if (TexturesArchives.Contains(Path.GetFileName(archive)))
                        {
                            UpdateLogger($"Randomising textures in '{Path.GetFileName(archive)}'.");
                            string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            await Task.Run(() => TextureRandomiser.PerArchive(unpackedArchive, CustomTextureFiles, texturesAllowDupes, texturesOnlyCustom));
                        }
                    }
                }
            }

            // Delete stage textures if we need to.
            if (texturesDelete == true)
            {
                foreach (string archive in archives)
                {
                    if (archive.Contains("stage_"))
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        UpdateLogger($"Deleting textures from '{Path.GetFileName(archive)}'.");
                        await Task.Run(() => TextureRandomiser.DeleteTextures(unpackedArchive));
                    }
                }
            }
            #endregion

            #region Audio Randomisers
            // Set up values.
            bool? audioMusic = CheckBox_Audio_Music.IsChecked;
            bool? audioSFX = CheckBox_Audio_SFX.IsChecked;

            // Check if we actually need to do music randomisation.
            if (audioMusic == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        string[] luaFiles = Directory.GetFiles(unpackedArchive, "*.lub", SearchOption.AllDirectories);
                        foreach (string luaFile in luaFiles)
                        {
                            if (await Task.Run(() => Helpers.NeededLua(luaFile, new List<string>() { "PlayBGM", "mission_bgm" })))
                            {
                                UpdateLogger($"Randomising music in '{luaFile}'.");
                                await Task.Run(() => AudioRandomisers.MusicRandomiser(luaFile, AudioMusic, Seed, DisableEasterEggs));
                            }
                        }
                    }
                }
            }

            // Check if we actually need to do sound effect randomisation.
            if (audioSFX == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "sound.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        // Find the CSBs and unpack the ones the user has chosen to randomise.
                        string[] csbFiles = Directory.GetFiles($"{unpackedArchive}\\common\\sound", "*.csb", SearchOption.TopDirectoryOnly);
                        foreach (string csbFile in csbFiles)
                        {
                            if (AudioCSBs.Contains(Path.GetFileName(csbFile)))
                            {
                                UpdateLogger($"Getting sounds in '{Path.GetFileName(csbFile)}'.");
                                await Task.Run(() => AudioRandomisers.CSBUnpack(csbFile));
                            }
                        }

                        // Get all the ADXs
                        string[] introADXFilesProcess = Directory.GetFiles($"{unpackedArchive}\\common\\sound", "Intro.adx", SearchOption.AllDirectories);
                        string[] loopADXFilesProcess = Directory.GetFiles($"{unpackedArchive}\\common\\sound", "Loop.adx", SearchOption.AllDirectories);
                        List<string> introADXFilesStereo = new();
                        List<string> introADXFilesMono = new();
                        List<string> loopADXFilesStereo = new();
                        List<string> loopADXFilesMono = new();

                        // Sort the ADX files based on whether they're stereo or mono.
                        foreach (string introADXFile in introADXFilesProcess)
                        {
                            using (BinaryReader reader = new(File.OpenRead(introADXFile)))
                            {
                                reader.BaseStream.Position = 0x07;

                                if (reader.ReadByte() == 1)
                                    introADXFilesMono.Add(introADXFile);

                                else
                                    introADXFilesStereo.Add(introADXFile);
                            }
                            File.Move(introADXFile, $"{introADXFile}.rnd");
                        }

                        foreach (string loopADXFile in loopADXFilesProcess)
                        {
                            using (BinaryReader reader = new(File.OpenRead(loopADXFile)))
                            {
                                reader.BaseStream.Position = 0x07;

                                if (reader.ReadByte() == 1)
                                    loopADXFilesMono.Add(loopADXFile);

                                else
                                    loopADXFilesStereo.Add(loopADXFile);
                            }
                            File.Move(loopADXFile, $"{loopADXFile}.rnd");
                        }

                        // Shuffle the ADX files.
                        UpdateLogger($"Shuffling sounds.");
                        await Task.Run(() => AudioRandomisers.ShuffleSoundEffects(introADXFilesStereo));
                        await Task.Run(() => AudioRandomisers.ShuffleSoundEffects(introADXFilesMono));
                        await Task.Run(() => AudioRandomisers.ShuffleSoundEffects(loopADXFilesStereo));
                        await Task.Run(() => AudioRandomisers.ShuffleSoundEffects(loopADXFilesMono));

                        // Repack the CSB
                        foreach (string csbFile in csbFiles)
                        {
                            if (AudioCSBs.Contains(Path.GetFileName(csbFile)))
                            {
                                UpdateLogger($"Repacking '{Path.GetFileName(csbFile)}'.");
                                await Task.Run(() => AudioRandomisers.CSBRepack($"{Path.GetDirectoryName(csbFile)}\\{Path.GetFileNameWithoutExtension(csbFile)}"));
                                Directory.Delete($"{Path.GetDirectoryName(csbFile)}\\{Path.GetFileNameWithoutExtension(csbFile)}", true);
                            }
                        }
                    }
                }
            }

            // Amogus Easter Egg Seed
            if (Seed.Contains("Amogus") && DisableEasterEggs == false)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "sound.arc")
                    {
                        UpdateLogger($"Making sound effects sus.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => AudioRandomisers.CSBUnpack($@"{unpackedArchive}\common\sound\enemy_monster_common.csb"));
                        File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\susIblis.adx", $@"{unpackedArchive}\common\sound\enemy_monster_common\04_monster_transfer.aif\Intro.adx", true);
                        await Task.Run(() => AudioRandomisers.CSBRepack($"{Path.GetDirectoryName($@"{unpackedArchive}\common\sound\enemy_monster_common.csb")}\\{Path.GetFileNameWithoutExtension($@"{unpackedArchive}\common\sound\enemy_monster_common.csb")}"));
                        Directory.Delete($"{Path.GetDirectoryName($@"{unpackedArchive}\common\sound\enemy_monster_common.csb")}\\{Path.GetFileNameWithoutExtension($@"{unpackedArchive}\common\sound\enemy_monster_common.csb")}", true);

                        await Task.Run(() => AudioRandomisers.CSBUnpack($@"{unpackedArchive}\common\sound\enemy_robot_common.csb"));
                        File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\susRobot.adx", $@"{unpackedArchive}\common\sound\enemy_robot_common\04_robot_transfer.aif.aif\Intro.adx", true);
                        await Task.Run(() => AudioRandomisers.CSBRepack($"{Path.GetDirectoryName($@"{unpackedArchive}\common\sound\enemy_robot_common.csb")}\\{Path.GetFileNameWithoutExtension($@"{unpackedArchive}\common\sound\enemy_robot_common.csb")}"));
                        Directory.Delete($"{Path.GetDirectoryName($@"{unpackedArchive}\common\sound\enemy_robot_common.csb")}\\{Path.GetFileNameWithoutExtension($@"{unpackedArchive}\common\sound\enemy_robot_common.csb")}", true);
                    }
                }
            }
            #endregion

            #region Text Randomisers
            bool? textButtons = CheckBox_Text_Buttons.IsChecked;
            bool? textGenerate = CheckBox_Text_Generate.IsChecked;
            bool? textGenerateEnforce = CheckBox_Text_Generate_Enforce.IsChecked;
            bool? textColour = CheckBox_Text_Colour.IsChecked;
            int textColourWeight = (int)NumericUpDown_Text_Colour_Weight.Value;
            bool? textShuffle = CheckBox_Text_Shuffle.IsChecked;

            // Check if we need to actually do text randomisation.
            if (textShuffle == true || textGenerate == true || textButtons == true || textColour == true)
            {
                // Set up placeholder strings for the locations of event.arc and text.arc
                string eventArc = "";
                string textArc = "";

                // Get event.arc and text.arc, as we need both for Text Randomisation.
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "event.arc")
                        eventArc = await Task.Run(() => Helpers.ArchiveHandler(archive));

                    if (Path.GetFileName(archive).ToLower() == "text.arc")
                        textArc = await Task.Run(() => Helpers.ArchiveHandler(archive));
                }

                // Determine which MSTs we need to edit based on language settings.
                string[] mstFiles = Array.Empty<string>();
                foreach (string language in TextLanguages)
                {
                    if (language == "e")
                        mstFiles = mstFiles.Concat(Directory.GetFiles(eventArc, "*.e.mst", SearchOption.AllDirectories).Concat(Directory.GetFiles(textArc, "*.e.mst", SearchOption.AllDirectories)).ToArray()).ToArray();

                    if (language == "f")
                        mstFiles = mstFiles.Concat(Directory.GetFiles(eventArc, "*.f.mst", SearchOption.AllDirectories).Concat(Directory.GetFiles(textArc, "*.f.mst", SearchOption.AllDirectories)).ToArray()).ToArray();

                    if (language == "g")
                        mstFiles = mstFiles.Concat(Directory.GetFiles(eventArc, "*.g.mst", SearchOption.AllDirectories).Concat(Directory.GetFiles(textArc, "*.g.mst", SearchOption.AllDirectories)).ToArray()).ToArray();

                    if (language == "i")
                        mstFiles = mstFiles.Concat(Directory.GetFiles(eventArc, "*.i.mst", SearchOption.AllDirectories).Concat(Directory.GetFiles(textArc, "*.i.mst", SearchOption.AllDirectories)).ToArray()).ToArray();

                    if (language == "j")
                        mstFiles = mstFiles.Concat(Directory.GetFiles(eventArc, "*.j.mst", SearchOption.AllDirectories).Concat(Directory.GetFiles(textArc, "*.j.mst", SearchOption.AllDirectories)).ToArray()).ToArray();

                    if (language == "s")
                        mstFiles = mstFiles.Concat(Directory.GetFiles(eventArc, "*.s.mst", SearchOption.AllDirectories).Concat(Directory.GetFiles(textArc, "*.s.mst", SearchOption.AllDirectories)).ToArray()).ToArray();
                }

                // Randomise Button Icons if we need to.
                // We do this first so we don't need to go through event.arc's MSTs.
                if (textButtons == true)
                {
                    // Loop through and process each MST.
                    foreach (string mstFile in mstFiles)
                    {
                        // Ignore this MST if it's in event.arc, as they never have placeholders.
                        if (!mstFile.Contains(eventArc))
                        {
                            UpdateLogger($"Randomising button icons in '{mstFile}'.");
                            await Task.Run(() => TextRandomiser.RandomiseButtonIcons(mstFile, TextButtons));
                        }
                    }
                }

                // Generate new random strings if we need to.
                if (textGenerate == true)
                {
                    // Parse the list of English Words from https://github.com/dwyl/english-words into an array.
                    string[] wordList = Properties.Resources.TextEnglishWords.Split("\r\n");

                    // Loop through and process each MST.
                    foreach (string mstFile in mstFiles)
                    {
                        UpdateLogger($"Generating random text for '{mstFile}'.");
                        await Task.Run(() => TextRandomiser.TextGenerator(mstFile, wordList, textGenerateEnforce));
                    }
                }

                // Randomly colour parts of the text.
                if (textColour == true)
                {
                    // Loop through and process each MST.
                    foreach (string mstFile in mstFiles)
                    {
                        UpdateLogger($"Randomly colouring text in '{mstFile}'.");
                        await Task.Run(() => TextRandomiser.RandomColours(mstFile, textColourWeight));
                    }
                }

                // Shuffle all the text in the MSTs if we need to.
                if (textShuffle == true)
                {
                    UpdateLogger($"Shuffling text.");
                    await Task.Run(() => TextRandomiser.ShuffleText(mstFiles, eventArc, textArc, TextLanguages));
                }
            }
            #endregion

            #region UI Randomisers
            bool? xncpColours = CheckBox_XNCP_Colours.IsChecked;
            bool? xncpColoursSame = CheckBox_XNCP_Colours_Same.IsChecked;
            bool? xncpColoursAlpha = CheckBox_XNCP_Colours_Alpha.IsChecked;
            bool? xncpScale = CheckBox_XNCP_Scale.IsChecked;
            double xncpScaleMin = NumericUpDown_XNCP_Scale_Min.Value;
            double xncpScaleMax = NumericUpDown_XNCP_Scale_Max.Value;
            bool? xncpZIndex = CheckBox_XNCP_ZIndex.IsChecked;
            bool? xncpTextureIndicies = CheckBox_XNCP_Textures.IsChecked;

            // Check if we need to actually do any XNCP stuff.
            if (xncpColours == true || xncpScale == true || xncpZIndex == true || xncpTextureIndicies == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "sprite.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        string[] xncpFiles = Directory.GetFiles(unpackedArchive, "*.xncp", SearchOption.AllDirectories);

                        // loading_english.xncp embeds the texture, which XNCPLib seems to fail with, so replace it with the italian one.
                        UpdateLogger($"Patching now loading xncp.");
                        string italianXNCP = Array.Find(xncpFiles, file => file.EndsWith("loading_italian.xncp"));
                        string englishXNCP = Array.Find(xncpFiles, file => file.EndsWith("loading_english.xncp"));
                        File.Copy(italianXNCP, englishXNCP, true);
                        await Task.Run(() => XNCPRandomisation.NowLoadingHack(englishXNCP));

                        foreach (string xncpFile in xncpFiles)
                        {
                            // Skip black_out.xncp as XNCPLib doesn't know what the hell to do with it.
                            if (Path.GetFileName(xncpFile) != "black_out.xncp")
                            {
                                UpdateLogger($"Randomising: '{xncpFile}'.");
                                await Task.Run(() => XNCPRandomisation.Process(xncpFile, xncpColours, xncpColoursSame, xncpColoursAlpha, xncpScale, xncpScaleMin, xncpScaleMax, xncpZIndex, xncpTextureIndicies));
                            }
                        }
                    }
                }
            }
            #endregion

            #region Misc. Randomisers
            // Set up values.
            bool? miscEnemyHealth = CheckBox_Misc_EnemyHealth.IsChecked;
            bool? miscEnemyHealthBosses = CheckBox_Misc_EnemyHealth_Bosses.IsChecked;
            int miscEnemyHealthMin = (int)NumericUpDown_Misc_EnemyHealth_Min.Value;
            int miscEnemyHealthMax = (int)NumericUpDown_Misc_EnemyHealth_Max.Value;
            bool? miscCollision = CheckBox_Misc_Collision.IsChecked;
            bool? miscCollisionPerFace = CheckBox_Misc_Collision_PerFace.IsChecked;
            bool? miscPatches = CheckBox_Misc_Patches.IsChecked;
            int miscPatchesWeight = (int)NumericUpDown_Misc_Patches_Weight.Value;
            bool? miscUnlock = CheckBox_Misc_AutoUnlock.IsChecked;
            bool? miscPropPSI = CheckBox_Misc_PropPSIBehaviour.IsChecked;
            bool? miscPropPSINoGrab = CheckBox_Misc_PropPSIBehaviour_NoGrab.IsChecked;
            bool? miscPropPSINoDebris = CheckBox_Misc_PropPSIBehaviour_NoDebris.IsChecked;
            bool? miscPropDebris = CheckBox_Misc_PropDebris.IsChecked;

            // Check if we need to actually do enemy health randomisation.
            if (miscEnemyHealth == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "enemy.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        UpdateLogger($"Randomising enemy health values.");
                        await Task.Run(() => MiscellaneousRandomisers.EnemyHealthRandomiser(unpackedArchive, corePath, miscEnemyHealthMin, miscEnemyHealthMax, miscEnemyHealthBosses));
                    }
                }
            }

            // Check if we need to actually to do collision randomisation.
            if (miscCollision == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "stage.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        string[] collisionFiles = Directory.GetFiles(unpackedArchive, "collision.bin", SearchOption.AllDirectories);
                        foreach (string collisionFile in collisionFiles)
                        {
                            UpdateLogger($"Randomising collision surface tags in '{collisionFile}'.");
                            await Task.Run(() => MiscellaneousRandomisers.SurfaceRandomiser(collisionFile, miscCollisionPerFace));
                        }
                    }
                }
            }

            // Check if we need to actually do patch randomisation.
            if (miscPatches == true)
            {
                UpdateLogger($"Randomising Patches.");
                await Task.Run(() => MiscellaneousRandomisers.PatchRandomiser(ModDirectory, MiscPatches, miscPatchesWeight));
            }

            // Check if we need to modify Sonic's first mission lua.
            if (miscUnlock == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        UpdateLogger($"Patching first Sonic mission Lua to unlock Shadow and Silver's episodes.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => MiscellaneousRandomisers.UnlockEpisodes(unpackedArchive, corePath));
                    }
                }
            }

            // Check if we need to modify Common.bin
            if (miscPropPSI == true || miscPropDebris == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "object.arc")
                    {
                        UpdateLogger($"Randomising prop attributes.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => MiscellaneousRandomisers.PropAttributes(unpackedArchive, corePath, miscPropPSI, miscPropPSINoGrab, miscPropPSINoDebris, miscPropDebris));
                    }
                }
            }

            // Make the Random Episode's MST and HUB.
            // We do this down here so the Text and SET Randomisers can't interfere with it.
            if (miscRandomEpisode == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "text.arc")
                    {
                        UpdateLogger($"Generating random episode message table.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => MiscellaneousRandomisers.RandomEpisodeMST(unpackedArchive, corePath, LevelOrder));

                        UpdateLogger($"Creating stage select hub.");
                        await Task.Run(() => MiscellaneousRandomisers.RandomEpisodeShopMST(unpackedArchive, corePath, LevelOrder));
                    }
                }

                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => MiscellaneousRandomisers.GenerateRandomEpisodeTown(unpackedArchive, corePath, LevelOrder));
                    }
                }
            }
            #endregion

            // Patch voice_all_e.sbk if we've done something to need it.
            foreach (string archive in archives)
            {
                if (setHints == true || (setEnemies == true && (SetEnemies.Contains("eCerberus") || SetEnemies.Contains("eGenesis") || SetEnemies.Contains("eWyvern") ||
                    SetEnemies.Contains("firstiblis") || SetEnemies.Contains("secondiblis") || SetEnemies.Contains("thirdiblis") || SetEnemies.Contains("firstmefiress") ||
                    SetEnemies.Contains("secondmefiress") || SetEnemies.Contains("solaris01") || SetEnemies.Contains("solaris02"))) || textShuffle == true)
                {
                    if (Path.GetFileName(archive).ToLower() == "sound.arc")
                    {
                        UpdateLogger($"Patching 'voice_all_e.sbk' to allow all voice lines to play in any stage.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => Helpers.VoiceBankPatcher(unpackedArchive, corePath));
                    }
                }
            }

            // Delete any temp files in the archives themselves.
            UpdateLogger($"Cleaning up leftover files.");
            foreach (string archive in archives)
            {
                string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive, true));
                if (Directory.Exists(unpackedArchive))
                    await Task.Run(() => Helpers.CleanUpShuffleLeftovers(unpackedArchive));
            }

            // Repack all the used archives (kinda don't like how this is done right now).
            foreach (string archive in archives)
            {
                if (Directory.Exists($@"{TemporaryDirectory}{archive[0..^4].Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}"))
                {
                    string saveDir = $@"{TextBox_General_ModsDirectory.Text}\Sonic '06 Randomised ({Helpers.UseSafeFormattedCharacters(TextBox_General_Seed.Text)}){archive[0..^4].Replace(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "")}";
                    saveDir = saveDir.Remove(saveDir.LastIndexOf('\\'));

                    Directory.CreateDirectory(saveDir);
                    UpdateLogger($"Repacking '{Path.GetFileName(archive)}'.");
                    await Task.Run(() => Helpers.ArchiveHandler($@"{TemporaryDirectory}{archive[0..^4].Replace(Path.GetDirectoryName(GameExecutable), "")}", $@"{saveDir}\{Path.GetFileName(archive)}"));
                }
            }

            // Delete the temp directory, as they quickly get large, esepecially when custom XMAs are involved.
            if (Directory.Exists(TemporaryDirectory))
                Directory.Delete(TemporaryDirectory, true);

            // Clean up the Wildcard's settings tampering.
            if (CheckBox_Wildcard_Enable.IsChecked == true)
            {
                SaveConfig(Path.Combine(ModDirectory, "wildcard.log"));
                LoadConfig(Path.Combine(ModDirectory, "wildcard.bak"));
                File.Delete(Path.Combine(ModDirectory, "wildcard.bak"));
            }

            // Restore Form Visiblity.
            TabControl_Main.Visibility = Visibility.Visible;
            ListView_ProgressLogger.Visibility = Visibility.Hidden;
            ProgressBar_ProgressLogger.Visibility = Visibility.Hidden;
            Button_Randomise.IsEnabled = true;
            Button_LoadConfig.IsEnabled = true;

            // Show the Randomisation Complete message box.
            HandyControl.Controls.MessageBox.Show("Randomisation Complete!",
                                                  "Sonic '06 Randomiser Suite",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Information);

            // Give a note about the Disable Camera Events patch if using the object shuffler.
            if (setTransform == true)
            {
                HandyControl.Controls.MessageBox.Show("While not required, using the Disable Camera Events patch may make object shuffling more enjoyable.",
                                                      "Sonic '06 Randomiser Suite",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Information);
            }

            // If the user has chosen to enable Auto Unlock, inform them of how to actually make it take.
            if (miscUnlock == true)
            {
                HandyControl.Controls.MessageBox.Show("To enable access to Shadow and Silver's episodes, load Sonic's and save the game from the pause menu.",
                                                      "Sonic '06 Randomiser Suite",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Information);
            }

            // Easter Egg seed message boxes.
            if (DisableEasterEggs == false)
            {
                if (Seed.Contains('π'))
                {
                    HandyControl.Controls.MessageBox.Show("I don't like pies...",
                                                          "Sonic '06 Randomiser Suite",
                                                          MessageBoxButton.OK,
                                                          MessageBoxImage.Question);
                }

                if (Seed.Contains("Accordion"))
                {
                    HandyControl.Controls.MessageBox.Show("That's a great stuff there, accordion man.\nNow play me the Polkamon!",
                                                          "Sonic '06 Randomiser Suite",
                                                          MessageBoxButton.OK,
                                                          MessageBoxImage.Question);
                }

                if (Seed.Contains("Amogus"))
                {
                    HandyControl.Controls.MessageBox.Show("When the imposter is sus...",
                                                          "Sonic '06 Randomiser Suite",
                                                          MessageBoxButton.OK,
                                                          MessageBoxImage.Question);
                }
            }
        }
    }
}
