using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // Generate the path to a temp directory we can use for the Randomisation process.
        public static string TemporaryDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        // Set up the Randomiser.
        public static Random Randomiser = new();

        public MainWindow()
        {
            InitializeComponent();
            GenerateDirectories();
            SetDefaults();
            
            // If this is a debug build, set the seed to WPF Test for the sake of consistent testing.
            // If not, hide the Debug Tab, Release Builds don't need it.
            #if DEBUG
            TextBox_General_Seed.Text = "WPF Test";
            #else
            TabItem_Debug.Visibility = System.Windows.Visibility.Collapsed;
            #endif
        }

        /// <summary>
        /// Create the Voice Packs and XMA Cache Directories if they don't exist.
        /// </summary>
        private void GenerateDirectories()
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
            Helpers.FillCheckedListBox(Properties.Resources.MiscSongs, CheckedList_Misc_Songs);
            Helpers.FillCheckedListBox(Properties.Resources.MiscLanguages, CheckedList_Misc_Languages);

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

        #region Text Box Functions
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
        /// </summary>
        private void GameExecutable_Update(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.GameExecutable = TextBox_General_GameExecutable.Text;
            Properties.Settings.Default.Save();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                case "CheckBox_Event_Voices":
                    CheckBox_Event_Voices_Japanese.IsEnabled = (bool)NewCheckedStatus;
                    CheckBox_Event_Voices_Gameplay.IsEnabled = (bool)NewCheckedStatus;
                    break;

                case "CheckBox_Scene_Light_Direction": CheckBox_Scene_Light_Direction_Enforce.IsEnabled = (bool)NewCheckedStatus; break;

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
                        default: throw new NotImplementedException();
                    }
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
        /// Actual main Randomisation process.
        /// </summary>
        private async void Randomise(object sender, RoutedEventArgs e)
        {
            // Check that our mods directory and game executable actually exist.
            if (!Directory.Exists(TextBox_General_ModsDirectory.Text) || !File.Exists(TextBox_General_GameExecutable.Text))
            {
                MessageBox.Show("Either your Game Executable or Mods Directory don't exist, please check your general settings.",
                                "Sonic '06 Randomiser Suite",
                                MessageBoxButton.OK);
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
                MessageBoxResult check = MessageBox.Show($"A mod with the seed {TextBox_General_Seed.Text} already exists.\nDo you want to replace it?",
                                             "Sonic '06 Randomiser Suite",
                                             MessageBoxButton.YesNo);

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

            // TODO: Reimplement the Wildcard.

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

            List<string> MiscMusic = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Songs);
            List<string> MiscLanguages = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Languages);
            List<string> MiscPatches = Helpers.EnumerateCheckedListBox(CheckedList_Misc_Patches);

            string[] CustomMusic = TextBox_Custom_Music.Text.Split('|');
            List<string> CustomVoxPacks = Helpers.EnumerateCheckedListBox(CheckedList_Custom_Vox);

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

            if (MiscMusic.Count == 0)
                CheckBox_Misc_Music.IsChecked = false;
            if (MiscLanguages.Count == 0)
                CheckBox_Misc_Text.IsChecked = false;
            if (MiscPatches.Count == 0)
                CheckBox_Misc_Patches.IsChecked = false;

            // Object Placement
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
                                await Task.Run(() => ObjectPlacementRandomiser.BossPatch(luaFile, setEnemies, setHints, SetHints));
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

            // Event Randomisation
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

            // Scene Randomisation
            bool? sceneLightAmbient = CheckBox_Scene_Light_Ambient.IsChecked;
            bool? sceneLightMain = CheckBox_Scene_Light_Main.IsChecked;
            bool? sceneLightSub = CheckBox_Scene_Light_Sub.IsChecked;
            bool? sceneLightDirection = CheckBox_Scene_Light_Direction.IsChecked;
            bool? sceneLightDirectionEnforce = CheckBox_Scene_Light_Direction_Enforce.IsChecked;
            bool? sceneFogColour = CheckBox_Scene_Fog_Colour.IsChecked;
            bool? sceneFogDensity = CheckBox_Scene_Fog_Density.IsChecked;
            bool? sceneEnvMaps = CheckBox_Scene_EnvMaps.IsChecked;

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
                            await Task.Run(() => SceneRandomiser.Process(luaFile, sceneLightAmbient, sceneLightMain, sceneLightSub, sceneLightDirection, sceneLightDirectionEnforce, sceneFogColour,
                                                                      sceneFogDensity, sceneEnvMaps, SceneEnvMaps));
                        }
                    }
                }
            }

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
                            if (luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("a_") || luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("b_") ||
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
                            luaFile.Contains("mission"))
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
                await Task.Run(() => MiscellaneousRandomisers.PatchRandomiser(ModDirectory, MiscPatches, miscPatchesWeight));
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

            // Restore Form Visiblity.
            TabControl_Main.Visibility = Visibility.Visible;
            ScrollViewer_ProgressLogger.Visibility = Visibility.Hidden;
            TextBlock_ProgressLogger.Visibility = Visibility.Hidden;
            ProgressBar_ProgressLogger.Visibility = Visibility.Hidden;
            Button_Randomise.IsEnabled = true;
            Button_LoadConfig.IsEnabled = true;
        }
        #endregion

        /// <summary>
        /// Add text to the TextBlock_ProgressLogger element and snap the scroll bar to the bottom.
        /// </summary>
        /// <param name="text">The text to add to the element.</param>
        private void UpdateLogger(string text)
        {
            TextBlock_ProgressLogger.Text += $"{text}\n";
            ScrollViewer_ProgressLogger.ScrollToEnd();
        }
    }
}
