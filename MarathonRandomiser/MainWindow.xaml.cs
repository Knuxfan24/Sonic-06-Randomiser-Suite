using Ookii.Dialogs.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
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
        public static readonly string GlobalVersionNumber = $"Version 2.1.6";

        #if !DEBUG
        public static readonly string VersionNumber = GlobalVersionNumber;
        #else
        public static readonly string VersionNumber = $"{GlobalVersionNumber}-indev-{File.GetLastAccessTime(Environment.CurrentDirectory):ddMMyy}";
        #endif
        
        // Generate the path to a temp directory we can use for the Randomisation process.
        public static readonly string TemporaryDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        // Set up the Randomiser.
        public static Random Randomiser = new();

        public MainWindow()
        {
            InitializeComponent();
            GenerateDirectories();

            SetDefaults();
            
            // Force culture info 'en-GB' to prevent errors with values altered by language-specific differences.
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-GB");

            // If this is a debug build, set the seed to WPF Test for the sake of consistent testing.
            // If not, hide the Debug Tab, Release Builds don't need it.
            #if DEBUG
            TextBox_General_Seed.Text = "WPF Test";
            Debug.WriteLine($"Current temporary path is: {TemporaryDirectory}.");
            #else
            TabItem_Debug.Visibility = System.Windows.Visibility.Collapsed;
            #endif
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

            // Generate a seed to use.
            TextBox_General_Seed.Text = Randomiser.Next().ToString();
            
            // Fill in the configuration CheckListBox elements.
            Helpers.FillCheckedListBox(Properties.Resources.EnemyTypes, CheckedList_SET_EnemyTypes);
            Helpers.FillCheckedListBox(Properties.Resources.CharacterTypes, CheckedList_SET_Characters);
            Helpers.FillCheckedListBox(Properties.Resources.ItemTypes, CheckedList_SET_ItemCapsules);
            Helpers.FillCheckedListBox(Properties.Resources.CommonPropTypes, CheckedList_SET_CommonProps);
            Helpers.FillCheckedListBox(Properties.Resources.PathPropTypes, CheckedList_SET_PathProps);
            Helpers.FillCheckedListBox(Properties.Resources.VoiceTypes, CheckedList_SET_Hints);
            Helpers.FillCheckedListBox(Properties.Resources.DoorTypes, CheckedList_SET_Doors);
            Helpers.FillCheckedListBox(Properties.Resources.EventLighting, CheckedList_Event_Lighting);
            Helpers.FillCheckedListBox(Properties.Resources.EventTerrain, CheckedList_Event_Terrain);
            Helpers.FillCheckedListBox(Properties.Resources.EnvMaps, CheckedList_Scene_EnvMaps);
            Helpers.FillCheckedListBox(Properties.Resources.SceneSkyboxes, CheckedList_Scene_Skyboxes);
            Helpers.FillCheckedListBox(Properties.Resources.MiscSongs, CheckedList_Misc_Songs);
            Helpers.FillCheckedListBox(Properties.Resources.MiscLanguages, CheckedList_Misc_Languages);

            RefreshVoicePacks();

            // Get all the patch files in the user's Mod Manager data.
            string[] patches = Directory.GetFiles($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Unify\\Patches\\", "*.mlua", SearchOption.TopDirectoryOnly);

            // Loop through and add the patches to the CheckedList_Misc_Patches element
            foreach (string patch in patches)
            {
                // Read the mlua and split it's second line (contains the title) into a seperate array we can use.
                string[] mlua = File.ReadAllLines(patch);
                string[] split = mlua[1].Split('\"');

                CheckedListBoxItem item = new()
                {
                    DisplayName = split[1],
                    Tag = Path.GetFileName(patch),
                    Checked = true
                };

                // Auto uncheck patches which don't fit too well with the Randomiser.
                if (Path.GetFileName(patch) == "EnableDebugMode.mlua" || Path.GetFileName(patch) == "Disable2xMSAA.mlua" || Path.GetFileName(patch) == "Disable4xMSAA.mlua" ||
                    Path.GetFileName(patch) == "DisableCharacterDialogue.mlua" || Path.GetFileName(patch) == "DisableCharacterUpgrades.mlua" || Path.GetFileName(patch) == "DisableHintRings.mlua" ||
                    Path.GetFileName(patch) == "DisableHUD.mlua" || Path.GetFileName(patch) == "DisableMusic.mlua" || Path.GetFileName(patch) == "DisableShadows.mlua" ||
                    Path.GetFileName(patch) == "DisableTalkWindowInStages.mlua" || Path.GetFileName(patch) == "DoNotCarryElise.mlua" || Path.GetFileName(patch) == "DoNotEnterMachSpeed.mlua" ||
                    Path.GetFileName(patch) == "DoNotUseTheSnowboard.mlua" || Path.GetFileName(patch) == "OmegaBlurFix.mlua" || Path.GetFileName(patch) == "TGS2006Menu.mlua")
                {
                    item.Checked = false;
                }

                CheckedList_Misc_Patches.Items.Add(item);
            }
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

            // If the selected executable exists, populate the Texture Randomiser's archives list.
            if (File.Exists(TextBox_General_GameExecutable.Text))
            {
                CheckedList_Textures_Arcs.Items.Clear();
                string[] archives = Directory.GetFiles(Path.GetDirectoryName(TextBox_General_GameExecutable.Text), "*.arc", SearchOption.AllDirectories);

                foreach(string archive in archives)
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
        /// Downloads my voice packs from GitHub.
        /// </summary>
        private async void Custom_FetchVox(object sender, RoutedEventArgs e)
        {
            // Display the Progress Logger elements and disable bottom buttons that shouldn't be useable during the process.
            TextBlock_ProgressLogger.Text = "";
            TabControl_Main.Visibility = Visibility.Hidden;
            TextBlock_ProgressLogger.Visibility = Visibility.Visible;
            ScrollViewer_ProgressLogger.Visibility = Visibility.Visible;
            ProgressBar_ProgressLogger.Visibility = Visibility.Visible;
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
            ScrollViewer_ProgressLogger.Visibility = Visibility.Hidden;
            TextBlock_ProgressLogger.Visibility = Visibility.Hidden;
            ProgressBar_ProgressLogger.Visibility = Visibility.Hidden;
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
        #endregion

        #region Form Helper Functions
        /// <summary>
        /// Disables and enables certain other elements based on the toggled status of a CheckBox
        /// </summary>
        private void Dependency_CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // Get the new state of this CheckBox.
            bool? NewCheckedStatus = ((CheckBox)sender).IsChecked;

            // Get the name of this Checkbox.
            string CheckBoxName = ((CheckBox)sender).Name;

            // Check the name of the Checkbox and carry out the appropriate task(s).
            switch (CheckBoxName)
            {
                case "CheckBox_SET_Enemies": CheckBox_SET_Enemies_NoBosses.IsEnabled = (bool)NewCheckedStatus; break;
                case "CheckBox_SET_Enemies_Behaviour": CheckBox_SET_Enemies_Behaviour_NoEnforce.IsEnabled = (bool)NewCheckedStatus; break;
                case "CheckBox_SET_DrawDistance":
                    Label_SET_DrawDistance_Min.IsEnabled = (bool)NewCheckedStatus;
                    NumericUpDown_SET_DrawDistance_Min.IsEnabled = (bool)NewCheckedStatus;
                    Label_SET_DrawDistance_Max.IsEnabled = (bool)NewCheckedStatus;
                    NumericUpDown_SET_DrawDistance_Max.IsEnabled = (bool)NewCheckedStatus;
                    break;

                case "CheckBox_Event_Voices":
                    CheckBox_Event_Voices_Japanese.IsEnabled = (bool)NewCheckedStatus;
                    CheckBox_Event_Voices_Gameplay.IsEnabled = (bool)NewCheckedStatus;
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
                case "CheckBox_Scene_Light_Direction": CheckBox_Scene_Light_Direction_Enforce.IsEnabled = (bool)NewCheckedStatus; break;

                case "CheckBox_Textures_Textures": CheckBox_Textures_PerArc.IsEnabled = (bool)NewCheckedStatus; break;

                case "CheckBox_Misc_EnemyHealth":
                    Label_Misc_EnemyHealth_Min.IsEnabled = (bool)NewCheckedStatus;
                    NumericUpDown_Misc_EnemyHealth_Min.IsEnabled = (bool)NewCheckedStatus;
                    Label_Misc_EnemyHealth_Max.IsEnabled = (bool)NewCheckedStatus;
                    NumericUpDown_Misc_EnemyHealth_Max.IsEnabled = (bool)NewCheckedStatus;
                    CheckBox_Misc_EnemyHealth_Bosses.IsEnabled = (bool)NewCheckedStatus;
                    break;
                case "CheckBox_Misc_Collision": CheckBox_Misc_Collision_PerFace.IsEnabled = (bool)NewCheckedStatus; break;

                case "CheckBox_Misc_Patches":
                    Label_Misc_Patches_Weight.IsEnabled = (bool)NewCheckedStatus;
                    NumericUpDown_Misc_Patches_Weight.IsEnabled = (bool)NewCheckedStatus;
                    break;

                case "CheckBox_General_Wildcard":
                    // Object Placement Tab.
                    CheckBox_SET_Enemies.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_Enemies_NoBosses.IsEnabled = (bool)!NewCheckedStatus;
                    if (NewCheckedStatus == false)
                    {
                        if (CheckBox_SET_Enemies.IsChecked == true)
                            CheckBox_SET_Enemies_NoBosses.IsEnabled = true;

                        if (CheckBox_SET_Enemies.IsChecked == false)
                            CheckBox_SET_Enemies_NoBosses.IsEnabled = false;
                    }
                    CheckBox_SET_Characters.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_ItemCapsules.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_CommonProps.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_PathProps.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_Hints.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_Doors.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_SET_Cosmetic.IsEnabled = (bool)!NewCheckedStatus;
                    TabControl_ObjectPlacement.IsEnabled = (bool)!NewCheckedStatus;

                    // Event and Scene Tabs.
                    TabItem_Event.IsEnabled = (bool)!NewCheckedStatus;
                    TabItem_Scene.IsEnabled = (bool)!NewCheckedStatus;
                    TabItem_Anim.IsEnabled = (bool)!NewCheckedStatus;

                    // Misc Tab.
                    CheckBox_Misc_Music.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_Misc_Collision.IsEnabled = (bool)!NewCheckedStatus;
                    CheckBox_Misc_Collision_PerFace.IsEnabled = (bool)!NewCheckedStatus;
                    if (NewCheckedStatus == false)
                    {
                        if (CheckBox_Misc_Collision.IsChecked == true)
                            CheckBox_Misc_Collision_PerFace.IsEnabled = true;

                        if (CheckBox_Misc_Collision.IsChecked == false)
                            CheckBox_Misc_Collision_PerFace.IsEnabled = false;
                    }
                    CheckBox_Misc_Text.IsEnabled = (bool)!NewCheckedStatus;
                    TabItem_Misc_Songs.IsEnabled = (bool)!NewCheckedStatus;
                    TabItem_Misc_Languages.IsEnabled = (bool)!NewCheckedStatus;
                    TabControl_Miscellaneous.SelectedIndex = 2;
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

                case "Grid_Textures":
                    Helpers.InvalidateCheckedListBox(CheckedList_Textures_Arcs, true, selectAll);
                    break;

                case "Grid_Miscellaneous":
                    switch (TabControl_Miscellaneous.SelectedIndex)
                    {
                        case 0: Helpers.InvalidateCheckedListBox(CheckedList_Misc_Songs, true, selectAll); break;
                        case 1: Helpers.InvalidateCheckedListBox(CheckedList_Misc_Languages, true, selectAll); break;
                        case 2: Helpers.InvalidateCheckedListBox(CheckedList_Misc_Patches, true, selectAll); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Grid_Custom": Helpers.InvalidateCheckedListBox(CheckedList_Custom_Vox, true, selectAll); break;

                default:
                    throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// Add text to the TextBlock_ProgressLogger element and snap the scroll bar to the bottom.
        /// </summary>
        /// <param name="text">The text to add to the element.</param>
        private void UpdateLogger(string text)
        {
            TextBlock_ProgressLogger.Text += $"{text}\n";
            ScrollViewer_ProgressLogger.ScrollToEnd();
        }
        
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

        #region Debug Functions
        /// <summary>
        /// Opens the temporary directory. Will open the documents folder if the temporary directory does not exist.
        /// </summary>
        private async void Debug_OpenTempDir(object sender, RoutedEventArgs e)
        {
            Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", TemporaryDirectory);
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
                                                  "Microsoft: xmaencode utility.\n" +
                                                  "HandyControl: WPF Form Controls.",
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
                configInfo.WriteLine($"Version=\"{TextBox_General_Seed.Text}\"");
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
            TextBlock_ProgressLogger.Text = "";
            TabControl_Main.Visibility = Visibility.Hidden;
            TextBlock_ProgressLogger.Visibility = Visibility.Visible;
            ScrollViewer_ProgressLogger.Visibility = Visibility.Visible;
            ProgressBar_ProgressLogger.Visibility = Visibility.Visible;
            Button_Randomise.IsEnabled = false;
            Button_LoadConfig.IsEnabled = false;

            // Wildcard Setup
            if (CheckBox_General_Wildcard.IsChecked == true)
            {
                // Save user's config so we can make it seemlessTM.
                SaveConfig(Path.Combine(ModDirectory, "wildcard.bak"));

                // Object Placement Tab.
                WildcardTabRoll(StackPanel_ObjectPlacement, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_EnemyTypes, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_Characters, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_ItemCapsules, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_CommonProps, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_PathProps, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_Hints, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_SET_Doors, (int)NumericUpDown_Wildcard_Weight.Value);

                // Event Tab.
                WildcardTabRoll(StackPanel_Event, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Event_Lighting, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Event_Terrain, (int)NumericUpDown_Wildcard_Weight.Value);

                // Scene Tab.
                WildcardTabRoll(StackPanel_Scene, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Scene_EnvMaps, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Scene_Skyboxes, (int)NumericUpDown_Wildcard_Weight.Value);

                // Animations Tab.
                WildcardTabRoll(StackPanel_Animation, (int)NumericUpDown_Wildcard_Weight.Value);

                // Textures Tab.
                WildcardTabRoll(StackPanel_Textures, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Textures_Arcs, (int)NumericUpDown_Wildcard_Weight.Value);

                // Misc Tab.
                WildcardTabRoll(StackPanel_Misc, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Misc_Songs, (int)NumericUpDown_Wildcard_Weight.Value);
                WildcardCheckedList(CheckedList_Misc_Languages, (int)NumericUpDown_Wildcard_Weight.Value);
            }

            // Enumerate the Checked List Boxes for the user's settings on lists.
            List<string> SetEnemies = Helpers.EnumerateCheckedListBox(CheckedList_SET_EnemyTypes);
            List<string> SetCharacters = Helpers.EnumerateCheckedListBox(CheckedList_SET_Characters);
            List<string> SetItemCapsules = Helpers.EnumerateCheckedListBox(CheckedList_SET_ItemCapsules);
            List<string> SetCommonProps = Helpers.EnumerateCheckedListBox(CheckedList_SET_CommonProps);
            List<string> SetPathProps = Helpers.EnumerateCheckedListBox(CheckedList_SET_PathProps);
            List<string> SetHints = Helpers.EnumerateCheckedListBox(CheckedList_SET_Hints);
            List<string> SetDoors = Helpers.EnumerateCheckedListBox(CheckedList_SET_Doors);

            List<string> EventLighting = Helpers.EnumerateCheckedListBox(CheckedList_Event_Lighting);
            List<string> EventTerrain = Helpers.EnumerateCheckedListBox(CheckedList_Event_Terrain);

            List<string> SceneEnvMaps = Helpers.EnumerateCheckedListBox(CheckedList_Scene_EnvMaps);
            List<string> SceneSkyboxes = Helpers.EnumerateCheckedListBox(CheckedList_Scene_Skyboxes);

            List<string> TexturesArchives = Helpers.EnumerateCheckedListBox(CheckedList_Textures_Arcs);

            List<string> MiscMusic = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Songs);
            List<string> MiscLanguages = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Languages);
            List<string> MiscPatches = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Patches);

            string[] CustomMusic = TextBox_Custom_Music.Text.Split('|');
            List<string> CustomVoxPacks = Helpers.EnumerateCheckedListBox(CheckedList_Custom_Vox);
            
            // Don't do the Custom stuff if we're using a PS3 version
            if (GameExecutable.ToLower().EndsWith(".xex"))
            {
                // Wildcard Custom Overrides
                if (CheckBox_General_Wildcard.IsChecked == true)
                {
                    if (TextBox_Custom_Music.Text.Length != 0)
                        CheckBox_Misc_Music.IsChecked = true;

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
                        MiscMusic.Add($"custom{i}");
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

                    // Insert the patched voice_all_e.sbk file into sound.arc first.
                    foreach (string archive in archives)
                    {
                        if (Path.GetFileName(archive).ToLower() == "sound.arc")
                        {
                            UpdateLogger($"Patching 'voice_all_e.sbk'.");
                            string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\voice_all_e.sbk", $@"{unpackedArchive}\xenon\sound\voice_all_e.sbk", true);
                        }
                    }

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

            if (EventLighting.Count == 0)
                CheckBox_Event_Lighting.IsChecked = false;
            if (EventTerrain.Count == 0)
                CheckBox_Event_Terrain.IsChecked = false;

            if (SceneEnvMaps.Count == 0)
                CheckBox_Scene_EnvMaps.IsChecked = false;
            if (SceneSkyboxes.Count == 0)
                CheckBox_Scene_Skyboxes.IsChecked = false;

            if (MiscMusic.Count == 0)
                CheckBox_Misc_Music.IsChecked = false;
            if (MiscLanguages.Count == 0)
                CheckBox_Misc_Text.IsChecked = false;
            if (MiscPatches.Count == 0)
                CheckBox_Misc_Patches.IsChecked = false;

            #region Object Placement
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
            int setMinDrawDistance = (int)NumericUpDown_SET_DrawDistance_Min.Value;
            int setMaxDrawDistance = (int)NumericUpDown_SET_DrawDistance_Max.Value;

            // Check if we actually need to do SET stuff.
            if (setEnemies == true || setBehaviour == true || setCharacters == true || setItemCapsules == true || setCommonProps == true || setPathProps == true || setHints == true || setDoors == true ||
                setDrawDistance == true || setCosmetic == true)
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
                            await Task.Run(() => ObjectPlacementRandomiser.Process(setFile, setEnemies, setEnemiesNoBosses, setBehaviour, setBehaviourNoEnforce, setCharacters, setItemCapsules, setCommonProps, setPathProps,
                                                                                     setHints, setDoors, setDrawDistance, setCosmetic, SetEnemies, SetCharacters, SetItemCapsules, SetCommonProps, SetPathProps,
                                                                                     SetHints, SetDoors, setMinDrawDistance, setMaxDrawDistance));
                        }

                        // Patch enemy luas if they need patching.
                        if (setEnemies == true && (SetEnemies.Contains("eCerberus") || SetEnemies.Contains("eGenesis") || SetEnemies.Contains("eWyvern") || SetEnemies.Contains("firstIblis") ||
                            SetEnemies.Contains("secondIblis") || SetEnemies.Contains("thirdIblis") || SetEnemies.Contains("firstmefiress") || SetEnemies.Contains("secondmefiress") ||
                            SetEnemies.Contains("solaris01") || SetEnemies.Contains("solaris02")) || setHints == true)
                        {
                            string[] luaFiles = Directory.GetFiles($"{unpackedArchive}\\xenon\\scripts\\enemy", "*.lub", SearchOption.TopDirectoryOnly);
                            foreach (string luaFile in luaFiles)
                            {
                                UpdateLogger($"Patching '{luaFile}'.");
                                await Task.Run(() => ObjectPlacementRandomiser.BossPatch(luaFile, setEnemies, setHints, SetHints, SetEnemies));
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
                            using (Stream patchCreate = File.Open(Path.Combine(ModDirectory, "patch.mlua"), FileMode.Create))
                            using (StreamWriter patchInfo = new(patchCreate))
                            {
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

                        }

                        // Patch stage and mission luas for player_start2 entities
                        if ((setEnemies == true && setEnemiesNoBosses != true) || setCharacters == true)
                        {
                            string[] luaFiles = Directory.GetFiles(unpackedArchive, "*.lub", SearchOption.AllDirectories);
                            foreach (string luaFile in luaFiles)
                            {
                                // Check if we need to actually use this lua file.
                                if (luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("a_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("b_")               ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("c_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("d_")               ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("e_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f_")               ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f1_")              || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f2_")              ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("g_")               || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eCerberus")        ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eGenesis")         || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eWyvern")          ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("firstmefiress")    || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("iblis01")          ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("secondiblis")      || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("secondmefiress")   ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("shadow_vs_silver") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("silver_vs_shadow") ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("solaris_super3")   || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("sonic_vs_silver")  ||
                                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("thirdiblis")       || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("silver_vs_sonic"))
                                {
                                    UpdateLogger($"Randomising player_start2 entities in '{luaFile}'.");
                                    await Task.Run(() => ObjectPlacementRandomiser.LuaPlayerStartRandomiser(luaFile, setCharacters, SetCharacters, setEnemies, setEnemiesNoBosses));
                                }
                            }
                        }
                    }

                    // Patch voice_all_e.sbk if we aren't using any voice packs.
                    if (CustomVoxPacks.Count == 0)
                    {
                        if (Path.GetFileName(archive).ToLower() == "sound.arc")
                        {
                            UpdateLogger($"Patching 'voice_all_e.sbk'.");
                            string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\voice_all_e.sbk", $@"{unpackedArchive}\xenon\sound\voice_all_e.sbk", true);
                        }
                    }
                }
            }
            #endregion

            #region Event Randomisation
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

            // Check if we actually need to do event stuff.
            if (eventLighting == true || eventRotX == true || eventRotY == true || eventRotZ == true || eventPosX == true || eventPosY == true || eventPosZ == true || eventTerrain == true || eventOrder == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "cache.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        if (eventLighting == true || eventRotX == true || eventRotY == true || eventRotZ == true || eventPosX == true || eventPosY == true || eventPosZ == true || eventTerrain == true)
                        {
                            UpdateLogger($"Randomising 'eventplaybook.epb' parameters.");
                            await Task.Run(() => EventRandomiser.Process(unpackedArchive, eventLighting, EventLighting, eventTerrain, EventTerrain, eventRotX, eventRotY, eventRotZ, eventPosX, eventPosY, eventPosZ));
                        }

                        if (eventOrder == true)
                        {
                            UpdateLogger($"Shuffling event order.");
                            await Task.Run(() => EventRandomiser.EventShuffler(unpackedArchive, ModDirectory, GameExecutable));
                        }

                        if (eventVoice == true)
                        {
                            UpdateLogger($"Shuffling event voice files.");
                            await Task.Run(() => EventRandomiser.ShuffleVoiceLines(GameExecutable, eventVoiceJpn, eventVoiceGame, CustomVoxPacks.Count != 0, ModDirectory));
                        }
                    }
                }
            }
            #endregion

            #region Scene Randomisation
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

            // Check if we actually need to do scene randomisation.
            if (sceneLightAmbient == true || sceneLightMain == true || sceneLightSub == true || sceneLightDirection == true || sceneFogColour == true ||
                sceneFogDensity == true || sceneEnvMaps == true)
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
                            await Task.Run(() => SceneRandomiser.Process(luaFile, sceneLightAmbient, sceneLightMain, sceneLightSub, sceneMinLight, sceneLightDirection, sceneLightDirectionEnforce, sceneFogColour,
                                                                      sceneFogDensity, sceneEnvMaps, SceneEnvMaps));
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
                            // Check if we need to actually use this lua file.
                            if (!skyLua.Contains("enemy") && (skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("a_") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("b_") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("c_") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("d_") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("e_") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("f_") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("f1_") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("f2_") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("g_") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("eCerberus") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("eGenesis") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("eWyvern") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("firstmefiress") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("iblis01") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("secondiblis") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("secondmefiress") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("shadow_vs_silver") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("silver_vs_shadow") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("solaris_super3") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("sonic_vs_silver") ||
                            skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("thirdiblis") || skyLua.Substring(skyLua.LastIndexOf('\\') + 1).StartsWith("silver_vs_sonic")))
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
            bool? animGameplay = CheckBox_Anim_Gameplay.IsChecked;
            bool? animEvents = CheckBox_Anim_Events.IsChecked;
            bool? animEventsFace = CheckBox_Anim_Events_Face.IsChecked;
            bool? animEventsCamera = CheckBox_Anim_Cameras.IsChecked;

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
                            await Task.Run(() => AnimationRandomiser.GameplayAnimationRandomiser(pkgFile));
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
            #endregion

            #region Texture Randomisation
            bool? texturesTextures = CheckBox_Textures_Textures.IsChecked;
            bool? texturesPerArc = CheckBox_Textures_PerArc.IsChecked;

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
                    for (int i = 0; i < archivePaths.Count; i++)
                    {
                        UpdateLogger($"Getting textures in '{Path.GetFileName(archivePaths[i])}.arc'.");
                        Textures = await Task.Run(() => TextureRandomiser.FetchTextures(Textures, archivePaths[i]));
                    }

                    List<int> usedNumbers = new();
                    for (int i = 0; i < archivePaths.Count; i++)
                    {
                        UpdateLogger($"Randomising textures in '{Path.GetFileName(archivePaths[i])}.arc'.");
                        usedNumbers = await Task.Run(() => TextureRandomiser.ShuffleTextures(usedNumbers, archivePaths[i], Textures));
                    }
                }

                // If we are, then do them one at a time.
                else
                {
                    foreach (string archive in archives)
                    {
                        if (TexturesArchives.Contains(Path.GetFileName(archive)))
                        {
                            UpdateLogger($"Randomising textures in '{Path.GetFileName(archive)}.arc'.");
                            string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                            await Task.Run(() => TextureRandomiser.PerArchive(unpackedArchive));
                        }
                    }
                }
            }
            #endregion

            #region Misc. Randomisers
            // Music Randomisation
            bool? miscMusic = CheckBox_Misc_Music.IsChecked;

            // Check if we actually need to do music randomisation.
            if (miscMusic == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));

                        string[] luaFiles = Directory.GetFiles(unpackedArchive, "*.lub", SearchOption.AllDirectories);
                        foreach (string luaFile in luaFiles)
                        {
                            // Check if we need to actually use this lua file.
                            if (!luaFile.Contains("enemy") && (luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("a_") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("b_") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("c_") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("d_") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("e_") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f_") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f1_") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("f2_") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("g_") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eCerberus") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eGenesis") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("eWyvern") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("firstmefiress") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("iblis01") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("secondiblis") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("secondmefiress") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("shadow_vs_silver") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("silver_vs_shadow") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("solaris_super3") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("sonic_vs_silver") ||
                            luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("thirdiblis") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("silver_vs_sonic") ||
                            luaFile.Contains("mission")))
                            {
                                UpdateLogger($"Randomising music in '{luaFile}'.");
                                await Task.Run(() => MiscellaneousRandomisers.MusicRandomiser(luaFile, MiscMusic));
                            }
                        }
                    }
                }
            }

            // Enemy Health Randomisation
            bool? miscEnemyHealth = CheckBox_Misc_EnemyHealth.IsChecked;
            bool? miscEnemyHealthBosses = CheckBox_Misc_EnemyHealth_Bosses.IsChecked;
            int miscEnemyHealthMin = (int)NumericUpDown_Misc_EnemyHealth_Min.Value;
            int miscEnemyHealthMax = (int)NumericUpDown_Misc_EnemyHealth_Max.Value;

            // Check if we need to actually do enemy health randomisation.
            if (miscEnemyHealth == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "enemy.arc")
                    {
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        UpdateLogger($"Randomising enemy health values.");
                        await Task.Run(() => MiscellaneousRandomisers.EnemyHealthRandomiser(unpackedArchive, miscEnemyHealthMin, miscEnemyHealthMax, miscEnemyHealthBosses));
                    }
                }
            }

            // Collision Randomisation
            bool? miscCollision = CheckBox_Misc_Collision.IsChecked;
            bool? miscCollisionPerFace = CheckBox_Misc_Collision_PerFace.IsChecked;

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

            // Text Randomisation
            bool? miscText = CheckBox_Misc_Text.IsChecked;

            // Check if we need to actually do text randomisation.
            if (miscText == true)
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

                UpdateLogger($"Shuffling text.");
                await Task.Run(() => MiscellaneousRandomisers.TextRandomiser(eventArc, textArc, MiscLanguages));
            }

            // Patch Randomisation
            bool? miscPatches = CheckBox_Misc_Patches.IsChecked;
            int miscPatchesWeight = (int)NumericUpDown_Misc_Patches_Weight.Value;

            // Check if we need to actually do patch randomisation.
            if (miscPatches == true)
            {
                UpdateLogger($"Randomising Patches.");
                await Task.Run(() => MiscellaneousRandomisers.PatchRandomiser(ModDirectory, MiscPatches, miscPatchesWeight));
            }

            // Auto Unlock Shadow and Silver's Episodes.
            bool? miscUnlock = CheckBox_Misc_AutoUnlock.IsChecked;

            if (miscUnlock == true)
            {
                foreach (string archive in archives)
                {
                    if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                    {
                        UpdateLogger($"Patching first Sonic mission Lua to unlock Shadow and Silver's episodes.");
                        string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                        await Task.Run(() => MiscellaneousRandomisers.UnlockEpisodes(unpackedArchive, GameExecutable));
                    }
                }
            }
            #endregion

            // Delete any temp files in the archives themselves.
            UpdateLogger($"Cleaning up leftover files.");
            foreach (string archive in archives)
            {
                string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive, true));
                if(Directory.Exists(unpackedArchive))
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
            if (CheckBox_General_Wildcard.IsChecked == true)
            {
                SaveConfig(Path.Combine(ModDirectory, "wildcard.log"));
                LoadConfig(Path.Combine(ModDirectory, "wildcard.bak"));
                File.Delete(Path.Combine(ModDirectory, "wildcard.bak"));
            }

            // Restore Form Visiblity.
            TabControl_Main.Visibility = Visibility.Visible;
            ScrollViewer_ProgressLogger.Visibility = Visibility.Hidden;
            TextBlock_ProgressLogger.Visibility = Visibility.Hidden;
            ProgressBar_ProgressLogger.Visibility = Visibility.Hidden;
            Button_Randomise.IsEnabled = true;
            Button_LoadConfig.IsEnabled = true;

            // Show the Randomisation Complete message box.
            HandyControl.Controls.MessageBox.Show("Randomisation Complete!",
                                                  "Sonic '06 Randomiser Suite",
                                                  MessageBoxButton.OK,
                                                  MessageBoxImage.Information);

            // If the user has chosen to enable Auto Unlock, inform them of how to actually make it take.
            if (miscUnlock == true)
            {
                HandyControl.Controls.MessageBox.Show("To enable access to Shadow and Silver's episodes, load Sonic's and save the game from the pause menu.",
                                                      "Sonic '06 Randomiser Suite",
                                                      MessageBoxButton.OK,
                                                      MessageBoxImage.Information);
            }
        }
        #endregion

        #region Config Saving/Loading
        /// <summary>
        /// Saves a configuration ini.
        /// </summary>
        /// <param name="location">The filepath we're saving to.</param>
        private void SaveConfig(string location)
        {
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

            // Textures Block.
            ConfigTabRead(configInfo, "Textures", StackPanel_Textures);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Textures_Arcs);
            configInfo.WriteLine();

            // Misc Block.
            ConfigTabRead(configInfo, "Misc", StackPanel_Misc);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Misc_Songs);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Misc_Languages);
            ConfigCheckedListBoxRead(configInfo, CheckedList_Misc_Patches);
            configInfo.WriteLine();

            // Custom Block.
            configInfo.WriteLine($"[Custom]");
            configInfo.WriteLine($"TextBox_Custom_Music={TextBox_Custom_Music.Text}");
            configInfo.WriteLine($"CheckBox_Custom_Music_XMACache={CheckBox_Custom_Music_XMACache.IsChecked}");
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
                // Try to get the item as both a Checkbox and an NumericUpDown element.
                CheckBox? checkbox = item as CheckBox;
                HandyControl.Controls.NumericUpDown? numeric = item as HandyControl.Controls.NumericUpDown;

                // If it's a Checkbox element, write the name and checked state.
                if (checkbox != null)
                    configInfo.WriteLine($"{checkbox.Name}={checkbox.IsChecked}");

                // If it's a NumericUpDown element, write the name and value.
                if (numeric != null)
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
                return;

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
                element = Grid_ObjectPlacement.FindName(split[0]);
                element = Grid_Event.FindName(split[0]);
                element = Grid_Scene.FindName(split[0]);
                element = Grid_Miscellaneous.FindName(split[0]);
                element = Grid_Custom.FindName(split[0]);

                // If we find this key's name, continue on.
                if (element != null)
                {
                    // Determine this element type.
                    CheckBox? checkbox = element as CheckBox;
                    HandyControl.Controls.NumericUpDown? numeric = element as HandyControl.Controls.NumericUpDown;
                    TextBox? textbox = element as TextBox;
                    CheckedListBox? checkedlist = element as CheckedListBox;
                    
                    // If it's a check box, check it or uncheck it depending on the key value.
                    if (checkbox != null)
                    {
                        checkbox.IsChecked = bool.Parse(split[1]);
                        Dependency_CheckBox_Changed(checkbox, null);
                    }

                    // If it's a numericupdown, set its value depending on the key's.
                    if (numeric != null)
                        numeric.Value = double.Parse(split[1]);

                    // If it's a text box, fill it in with the key's value.
                    if (textbox != null)
                        textbox.Text = split[1];

                    // If it's a checkedlistbox, invalidate the existing list, loop through and check ones that have the tags specified in the list.
                    if (checkedlist != null)
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

                    // If we've failed to find this element, we've buggered something up, throw an exception.
                    if (checkbox == null && numeric == null && textbox == null && checkedlist == null)
                    {
                        throw new NotImplementedException();
                    }
                }

                // If not, we've buggered something up, throw an exception.
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        #endregion

        #region Wildcard
        /// <summary>
        /// Goes through a StackPanel can chooses whether to check or uncheck CheckBox elements within it.
        /// </summary>
        /// <param name="element">The StackPanel to search.</param>
        /// <param name="wildcardWeight">The likelyhood the Wildcard will toggle an option on.</param>
        private static void WildcardTabRoll(DependencyObject element, int wildcardWeight)
        {
            // List of Checkboxes the Wildcard is forbidden from changing.
            List<string> Forbidden = new() { "CheckBox_SET_Enemies_Behaviour", "CheckBox_SET_Enemies_Behaviour_NoEnforce", "CheckBox_SET_DrawDistance", "CheckBox_Misc_EnemyHealth", "CheckBox_Misc_Patches", "CheckBox_Misc_AutoUnlock" };
            
            // Get all the children of this StackPanel element.
            IEnumerable? children = LogicalTreeHelper.GetChildren(element);

            // Loop through each item in this StackPanel element.
            foreach (object? item in children)
            {
                // Try to get the item as a Checkbox element.
                CheckBox? checkbox = item as CheckBox;

                // If the element is a Checkbox and is not forbidden, then roll a number and check the box based on the outcome.
                if (checkbox != null)
                {
                    if (!Forbidden.Contains(checkbox.Name))
                    {
                        if (Randomiser.Next(0, 101) <= wildcardWeight)
                            checkbox.IsChecked = true;
                        else
                            checkbox.IsChecked = false;
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
        #endregion
    }
}
