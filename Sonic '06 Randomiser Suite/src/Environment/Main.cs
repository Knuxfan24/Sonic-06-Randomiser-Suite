using System;
using System.IO;
using System.Linq;
using HedgeLib.Sets;
using System.Drawing;
using Unify.Messenger;
using Unify.TabControl;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    public partial class Main : Form
    {
        public static bool Randomising = false;
        public static Random RNG = new Random();
        public static List<int> Items = new List<int>();
        public static List<string> Enemies    = new List<string>(),
                                   Characters = new List<string>(),
                                   Music      = new List<string>(),
                                   Languages  = new List<string>(),
                                   Areas      = new List<string>(),
                                   Surfaces   = new List<string>();

        /// <summary>
        /// WinForms entry point
        /// </summary>
        public Main() {
            // Initialise form for the designer
            InitializeComponent();

            // Show version number
            Text = $"{Text} ({Program.GlobalVersionNumber})";
            Label_VersionNumber.Text = Program.GlobalVersionNumber;

            // Set console output to ListBox_Logs
            Console.SetOut(new ListBoxWriter(ListBox_Logs));
            
            // Subscribe to simplify saving and loading settings
            Properties.Settings.Default.SettingsSaving += Default_SettingsSaving;
            LoadSettings();

            // Set default randomisation states
            for (int i = 0; i < 32; i++) CheckedListBox_Placement_Enemies.SetItemChecked(i, true);
            CheckedListBox_Placement_Characters.SetItemChecked(0, true);
            for (int i = 5; i < 13; i++) CheckedListBox_Placement_Characters.SetItemChecked(i, true);
            for (int i = 0; i < CheckedListBox_Placement_Items.Items.Count; i++) CheckedListBox_Placement_Items.SetItemChecked(i, true);
            for (int i = 0; i < CheckedListBox_Audio_Music.Items.Count; i++) CheckedListBox_Audio_Music.SetItemChecked(i, true);
            CheckedListBox_Text_Languages.SetItemChecked(0, true);
            for (int i = 0; i < CheckedListBox_Visual_Areas.Items.Count; i++) CheckedListBox_Visual_Areas.SetItemChecked(i, true);
            for (int i = 0; i < CheckedListBox_Collision_Respected.Items.Count; i++) CheckedListBox_Collision_Respected.SetItemChecked(i, true);
            for (int i = 0; i < CheckedListBox_Collision_Surfaces.Items.Count; i++) CheckedListBox_Collision_Surfaces.SetItemChecked(i, true);
            for (int i = 0; i < CheckedListBox_Package_Characters.Items.Count; i++) CheckedListBox_Package_Characters.SetItemChecked(i, true);
        }

        /// <summary>
        /// Loads the settings whenever the settings are saved
        /// </summary>
        private void Default_SettingsSaving(object sender, System.ComponentModel.CancelEventArgs e) => LoadSettings();

        /// <summary>
        /// Applies loaded settings
        /// </summary>
        private void LoadSettings() {
            TextBox_ModsDirectory.Text = Properties.Settings.Default.Path_ModsDirectory;
            TextBox_GameExecutable.Text = Properties.Settings.Default.Path_GameExecutable;
            TextBox_RandomisationSeed.Text = RNG.Next().ToString();
        }

        /// <summary>
        /// Refreshes the software renderer for the custom TabControl
        /// </summary>
        private void UnifyTabControl_Selected(object sender, TabControlEventArgs e) => ((UnifyTabControl)sender).Refresh();

        /// <summary>
        /// Runs the randomisation thread upon clicking
        /// </summary>
        private void Button_Randomise_Click(object sender, EventArgs e) {
            // Reveals the panel (prevents it from overlapping everything in the designer)
            Panel_Inactive.BringToFront();

            // Reveal controls and enable Randomising to tell us if it's still working
            ListBox_Logs.Items.Clear();
            Randomising = Panel_Inactive.Visible = ProgressBar_Randomisation.Visible = true;
            BackgroundWorker bw = new BackgroundWorker() { WorkerReportsProgress = true };

            // Subscribe to required events to keep the progress updated
            bw.DoWork += new DoWorkEventHandler(StartRandomisation);
            bw.ProgressChanged += (bwSender, bwEventArgs) => ProgressBar_Randomisation.Value = bwEventArgs.ProgressPercentage;

            // Begin randomisation
            bw.RunWorkerAsync();

            // Restore controls when complete
            bw.RunWorkerCompleted += (bwSender, bwEventArgs) => {
                // Hide controls and disable Randomising since it's now done
                Randomising = Panel_Inactive.Visible = ProgressBar_Randomisation.Visible = false;
                UnifyMessenger.UnifyMessage.ShowDialog("Sonic '06 has been randomised... Have fun!", "Randomisation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
        }

        /// <summary>
        /// Randomises Sonic '06... duh.
        /// </summary>
        private void StartRandomisation(object sender, DoWorkEventArgs e) {
            // Variables
            string modDirectory = string.Empty; // Stored here to be initialised and checked later
            List<string> getArchiveList = Paths.CollectGameData(Path.GetDirectoryName(Properties.Settings.Default.Path_GameExecutable)).ToList();

            // Clear all lists
            Enemies.Clear();
            Characters.Clear();
            Items.Clear();
            Music.Clear();
            Languages.Clear();
            Areas.Clear();

            // Creates a new seed from TextBox_RandomisationSeed
            RNG = new Random(TextBox_RandomisationSeed.Text.GetHashCode());

            // Create mod directory, also checking if it's beatable to enable a patch - if it's empty, return nothing
            if ((modDirectory = Mods.Create(TextBox_RandomisationSeed.Text, CheckedListBox_Placement_General.GetItemChecked(5))) == string.Empty) return;

            // Define valid lists from CheckedListBox elements 
            Enemies    = Resources.EnumerateEnemiesList(CheckedListBox_Placement_Enemies);
            Characters = Resources.EnumerateCharactersList(CheckedListBox_Placement_Characters);
            Items      = Resources.EnumerateItemsList(CheckedListBox_Placement_Items);
            Music      = Resources.EnumerateMusicList(CheckedListBox_Audio_Music);
            Languages  = Resources.EnumerateLanguagesList(CheckedListBox_Text_Languages);
            Areas      = Resources.EnumerateAreasList(CheckedListBox_Visual_Areas);
            Surfaces  = Resources.EnumerateCollisionList(CheckedListBox_Collision_Surfaces);

            // Sets up the progress bar values
            int getProgress = getArchiveList.Count(),
                arcNumber = 1;

            // Iterate through all archives and modify based on user choice
            foreach (string archive in getArchiveList)
            {
                // Increment progress bar per archive
                ((BackgroundWorker)sender).ReportProgress(100 / getProgress * arcNumber); arcNumber++;

                // Unpacked scripts.arc
                // Randomises a lot of stuff...
                if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                {
                    // If all important CheckedListBox elements for this archive are empty, continue to the next archive
                    if (CheckedListBox_Placement_General.CheckedIndices.Count == 0 &&
                        CheckedListBox_Scene_General.CheckedIndices.Count     == 0 &&
                        CheckedListBox_Audio_General.CheckedIndices.Count     == 0) continue;

                    // Unpack the archive
                    string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    // Placement Randomisation
                    if (CheckedListBox_Placement_General.CheckedIndices.Count != 0)
                    {
                        // Iterate through all SET files
                        foreach (string setData in Directory.GetFiles(randomArchive, "*.set", SearchOption.AllDirectories))
                        {
                            // Write to logs for user feedback
                            Console.WriteLine($"Randomising Placement: {setData}");

                            // Loads the SET data for modification
                            S06SetData set = new S06SetData();
                            set.Load(setData);

                            // Modify based on user choice
                            foreach (int item in CheckedListBox_Placement_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Placement.RandomiseEnemies(set, RNG);      break;
                                    case 1: Placement.RandomiseCharacters(set, RNG);   break;
                                    case 2: Placement.RandomiseItems(set, RNG);        break;
                                    case 3: Placement.RandomiseVoices(set, RNG);       break;
                                    case 4: Placement.RandomisePhysicsProps(set, RNG); break;
                                    case 5: Beatable.DetermineStage(setData, set);     break;
                                }
                            }

                            // Saves the modified SET data
                            set.Save(setData, true);
                        }

                        // Iterate through all Lua scripts and remove parameters from enemies that may be problematic for the player
                        foreach (string enemy in Directory.GetFiles(
                                                 Path.Combine(randomArchive, $"scripts\\{Literal.Core(Properties.Settings.Default.Path_GameExecutable)}\\scripts\\enemy\\"),
                                                 "*.lub", SearchOption.TopDirectoryOnly))
                        {
                            // Blacklisted parameters
                            List<string> blacklistParams = new List<string>() { "CallSetCamera", "FirstMefiress_Warp", "FirstMefiress_RandomWarp" };

                            // Write to logs for user feedback
                            Console.WriteLine($"Patching Enemy: {enemy}");

                            // Decompile Lua script
                            Lua.Decompile(enemy);

                            // Write Lua script back without blacklisted parameters
                            File.WriteAllLines(enemy, File.ReadLines(enemy).Where(x => !blacklistParams.Any(x.Contains)).ToList());
                        }
                    }

                    // Scene Randomisation
                    if (CheckedListBox_Scene_General.CheckedIndices.Count != 0)
                    {
                        // Iterate through all Lua scripts for scene parameters
                        foreach (string lubData in Directory.GetFiles(randomArchive, "scene*.lub", SearchOption.AllDirectories))
                        {
                            // Write to logs for user feedback
                            Console.WriteLine($"Randomising Scene: {lubData}");

                            // Decompile Lua script
                            Lua.Decompile(lubData);

                            // Loads the Lua script into memory
                            string[] editedLub = File.ReadAllLines(lubData);

                            // Modify based on user choice
                            foreach (int item in CheckedListBox_Scene_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Scene.RandomiseLight(editedLub, "Ambient", RNG); break;
                                    case 1: Scene.RandomiseLight(editedLub, "Main", RNG);    break;
                                    case 2: Scene.RandomiseLight(editedLub, "Sub", RNG);     break;
                                    case 3: Scene.RandomiseLightDirection(editedLub, RNG);   break;
                                    case 4: Scene.RandomiseFogColour(editedLub, RNG);        break;
                                    case 5: Scene.RandomiseFogDensity(editedLub, RNG);       break;
                                    case 6: Scene.RandomiseEnvironmentMaps(editedLub, RNG);  break;
                                }
                            }

                            // Writes the modified Lua script
                            File.WriteAllLines(lubData, editedLub);
                        }
                    }

                    // Randomise Music
                    if (CheckedListBox_Audio_General.CheckedIndices.Count != 0)
                    {
                        // If the Music list enumerated nothing, continue to the next statement
                        if (Music.Count == 0) continue;

                        // Whitelisted scripts
                        List<string> whitelistScripts = new List<string>() { "a_", "b_", "c_", "d_", "e_", "f_", "f1_", "f2_", "g_" };

                        // Iterate through all Lua scripts for stage construction
                        foreach (string lubData in Directory.GetFiles(randomArchive, "*.lub", SearchOption.AllDirectories)
                                                   .Where(x => whitelistScripts.Any(Path.GetFileName(x).StartsWith)))
                        {
                            // Write to logs for user feedback
                            Console.WriteLine($"Randomising Audio: {lubData}");

                            // Decompile Lua script
                            Lua.Decompile(lubData);

                            // Loads the Lua script into memory
                            string[] editedLub = File.ReadAllLines(lubData);

                            // Modify based on user choice
                            foreach (int item in CheckedListBox_Audio_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Audio.RandomiseMusic(editedLub, RNG); break;
                                }
                            }

                            // Writes the modified Lua script
                            File.WriteAllLines(lubData, editedLub);
                        }
                    }

                    // Randomise Loading Text
                    if (CheckedListBox_Text_General.CheckedIndices.Count != 0)
                    {
                        // Iterate through all Lua scripts for missions
                        foreach (string lubData in Directory.GetFiles(randomArchive, "mission*.lub", SearchOption.AllDirectories))
                        {
                            // Write to logs for user feedback
                            Console.WriteLine($"Randomising Text: {lubData}");

                            // Decompile Lua script
                            Lua.Decompile(lubData);

                            // Loads the Lua script into memory
                            string[] editedLub = File.ReadAllLines(lubData);

                            // Modify based on user choice
                            foreach (int item in CheckedListBox_Text_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Strings.RandomiseLoadingText(editedLub, RNG); break;
                                }
                            }

                            // Writes the modified Lua script
                            File.WriteAllLines(lubData, editedLub);
                        }
                    }

                    // Repack the archive
                    Archives.CreateModARC(randomArchive, archive, modDirectory);
                }

                // Unpack text.arc
                // Unpack event.arc
                // Randomises all text strings in the MSTs
                else if (Path.GetFileName(archive).ToLower() == "text.arc" || Path.GetFileName(archive).ToLower() == "event.arc")
                {
                    // If the Languages list enumerated nothing, continue to the next statement
                    if (Languages.Count == 0) continue;

                    // Modify based on user choice
                    foreach (int item in CheckedListBox_Text_General.CheckedIndices)
                    {
                        switch (item)
                        {
                            case 1:
                                // Unpack the archive
                                string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                                // Randomise all strings in the MSTs
                                Strings.RandomiseMSTContents(randomArchive, Languages, RNG);

                                // Repack the archive
                                Archives.CreateModARC(randomArchive, archive, modDirectory);
                                break;
                        }
                    }
                }

                else if (Path.GetFileName(archive).ToLower() == "stage.arc") {
                    // Modify based on user choice
                    foreach (int item in CheckedListBox_Collision_General.CheckedIndices)
                    {
                        switch (item)
                        {
                            case 0:
                                // Unpack the archive
                                string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                                // Iterate through all Lua scripts for scene parameters
                                foreach (string binData in Directory.GetFiles(randomArchive, "collision.bin", SearchOption.AllDirectories))
                                {
                                    // Shortened variable to get checked items easier
                                    CheckedListBox @checked = CheckedListBox_Collision_Respected;

                                    // Write to logs for user feedback
                                    Console.WriteLine($"Randomising Collision: {binData}");

                                    // Decode collision binary
                                    Collision.Decode(binData, out string convertedColi);

                                    // Rotate collision axis
                                    Collision.RotationSwap(convertedColi);

                                    // Change collision mesh names in the OBJ
                                    Collision.PropertyRandomiser(convertedColi, RNG, @checked.GetItemChecked(0), @checked.GetItemChecked(1), @checked.GetItemChecked(2));

                                    // Encode collision binary
                                    Collision.Encode(convertedColi);
                                }

                                // Repack the archive
                                Archives.CreateModARC(randomArchive, archive, modDirectory);
                                break;
                        }
                    }
                }

                else if (Path.GetFileName(archive).ToLower() == "player.arc") {
                    // Modify based on user choice
                    foreach (int item in CheckedListBox_Package_General.CheckedIndices)
                    {
                        switch (item)
                        {
                            case 0:
                                // Unpack the archive
                                string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                                // Iterate through all Lua scripts for scene parameters
                                foreach (string pkgData in Directory.GetFiles(randomArchive, "*.pkg", SearchOption.AllDirectories))
                                {
                                    // Write to logs for user feedback
                                    Console.WriteLine($"Randomising Package: {pkgData}");

                                    // Randomise animations in PKG
                                    Package.PackageAnimationRandomiser(pkgData, RNG);
                                }

                                // Repack the archive
                                Archives.CreateModARC(randomArchive, archive, modDirectory);
                                break;
                        }
                    }
                }

                // Unpack object.arc
                // Unpack sprite.arc
                // Unpack stage archives
                // Randomises all textures in the archives
                else if (Path.GetFileName(archive).ToLower() == "object.arc"       ||
                         Path.GetFileName(archive).ToLower() == "sprite.arc"       ||
                         Path.GetFileName(archive).ToLower().StartsWith("player_") ||
                         Path.GetFileName(archive).ToLower().StartsWith("stage_")  ||
                         Path.GetFileNameWithoutExtension(archive).ToLower().EndsWith("data"))
                {
                    // Modify based on user choice
                    foreach (int item in CheckedListBox_Visual_General.CheckedIndices)
                    {
                        switch (item)
                        {
                            case 0 when Areas.Contains(Path.GetFileNameWithoutExtension(archive)):
                            case 1 when Path.GetFileName(archive).ToLower() == "object.arc":
                            case 2 when Path.GetFileName(archive).ToLower() == "sprite.arc":
                            case 3 when Path.GetFileName(archive).ToLower().StartsWith("player_"):
                            case 4 when Path.GetFileName(archive).ToLower().StartsWith("stage_e") || Path.GetFileName(archive).ToLower() == "event_data.arc":
                            case 5 when Path.GetFileName(archive).ToLower() == "particle_data.arc":
                            case 6 when Path.GetFileName(archive).ToLower() == "enemy_data.arc":
                                // Unpack the archive
                                string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                                // Randomise all textures in the archive
                                Textures.RandomiseTextures(randomArchive, RNG);

                                // Repack the archive
                                Archives.CreateModARC(randomArchive, archive, modDirectory);
                                break;
                        }
                    }
                }

                // Unpack sound.arc
                // Replaces the common scene bank with one that contains all voice lines
                else if (Path.GetFileName(archive).ToLower() == "sound.arc") {
                    // Unpack the archive
                    string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
                    string voiceSBK = Path.Combine(randomArchive, $"sound\\{Literal.Core(Properties.Settings.Default.Path_GameExecutable)}\\sound\\voice_all_e.sbk");

                    // Write custom SBK
                    Console.WriteLine($"Patching Scene Bank: {voiceSBK}");
                    File.WriteAllBytes(voiceSBK, Properties.Resources.voice_all_e);

                    // Repack the archive
                    Archives.CreateModARC(randomArchive, archive, modDirectory);
                }
            }
        }

        /// <summary>
        /// Displays the about window upon clicking
        /// </summary>
        private void Button_About_Click(object sender, EventArgs e) {
            UnifyMessenger.UnifyMessage.ShowDialog("Sonic '06 Randomiser Suite\n" +
                                                   $"{Program.GlobalVersionNumber}\n\n" +
                                                   "" +
                                                   "Knuxfan24 - Lead programmer and reverse-engineer\n" +
                                                   "HyperPolygon64 - Designer, optimisation and reverse-engineer\n" +
                                                   "Radfordhound - HedgeLib# and format specifications\n" +
                                                   "GerbilSoft - ArcPackerLib\n" +
                                                   "Shadow LAG - Lua Decompiler\n" +
                                                   "darkhero1337 - Unlock Mid-air Momentum",
                                                   "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Check if the text box controls have been modified
        /// </summary>
        private void TextBox_TextChanged(object sender, EventArgs e) {
            if (!Paths.CheckPathLegitimacy(TextBox_ModsDirectory.Text)) 
                TextBox_ModsDirectory.BackColor = Color.FromArgb(70, 45, 48);
            else
                TextBox_ModsDirectory.BackColor = Color.FromArgb(45, 45, 48);

            if (!Paths.CheckFileLegitimacy(TextBox_GameExecutable.Text))
                TextBox_GameExecutable.BackColor = Color.FromArgb(70, 45, 48);
            else
                TextBox_GameExecutable.BackColor = Color.FromArgb(45, 45, 48);

            if (TextBox_RandomisationSeed.Text == string.Empty)
                TextBox_RandomisationSeed.BackColor = Color.FromArgb(70, 45, 48);
            else
                TextBox_RandomisationSeed.BackColor = Color.FromArgb(45, 45, 48);

            if (!Paths.CheckPathLegitimacy(TextBox_ModsDirectory.Text)  ||
                !Paths.CheckFileLegitimacy(TextBox_GameExecutable.Text) ||
                TextBox_RandomisationSeed.Text == string.Empty)
            {
                Button_Randomise.Enabled = false;
            }
            else
                Button_Randomise.Enabled = true;
        }

        /// <summary>
        /// Displays a warning if the randomiser is still processing
        /// </summary>
        private void Main_FormClosing(object sender, FormClosingEventArgs e) {
            if (Randomising)
                if (UnifyMessenger.UnifyMessage.ShowDialog("A randomisation task is running! Are you sure you want to quit?",
                                                           "Quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    e.Cancel = true;
        }

        /// <summary>
        /// Randomises the seed upon clicking
        /// </summary>
        private void Button_RandomisationSeed_Click(object sender, EventArgs e) => TextBox_RandomisationSeed.Text = RNG.Next().ToString();

        /// <summary>
        /// Select all items in the CheckedListBox depending on sender
        /// </summary>
        private void Button_SelectAll_Click(object sender, EventArgs e) {
            // This looks messy, we know...
            if (sender == Button_Placement_General_SelectAll)         for (int i = 0; i < CheckedListBox_Placement_General.Items.Count; i++)    CheckedListBox_Placement_General.SetItemChecked(i, true);
            else if (sender == Button_Placement_Enemies_SelectAll)    for (int i = 0; i < CheckedListBox_Placement_Enemies.Items.Count; i++)    CheckedListBox_Placement_Enemies.SetItemChecked(i, true);
            else if (sender == Button_Placement_Characters_SelectAll) for (int i = 0; i < CheckedListBox_Placement_Characters.Items.Count; i++) CheckedListBox_Placement_Characters.SetItemChecked(i, true);
            else if (sender == Button_Placement_Items_SelectAll)      for (int i = 0; i < CheckedListBox_Placement_Items.Items.Count; i++)      CheckedListBox_Placement_Items.SetItemChecked(i, true);
            else if (sender == Button_Scene_General_SelectAll)        for (int i = 0; i < CheckedListBox_Scene_General.Items.Count; i++)        CheckedListBox_Scene_General.SetItemChecked(i, true);
            else if (sender == Button_Audio_General_SelectAll)        for (int i = 0; i < CheckedListBox_Audio_General.Items.Count; i++)        CheckedListBox_Audio_General.SetItemChecked(i, true);
            else if (sender == Button_Audio_Music_SelectAll)          for (int i = 0; i < CheckedListBox_Audio_Music.Items.Count; i++)          CheckedListBox_Audio_Music.SetItemChecked(i, true);
            else if (sender == Button_Text_General_SelectAll)         for (int i = 0; i < CheckedListBox_Text_General.Items.Count; i++)         CheckedListBox_Text_General.SetItemChecked(i, true);
            else if (sender == Button_Text_Languages_SelectAll)       for (int i = 0; i < CheckedListBox_Text_Languages.Items.Count; i++)       CheckedListBox_Text_Languages.SetItemChecked(i, true);
            else if (sender == Button_Visual_General_SelectAll)       for (int i = 0; i < CheckedListBox_Visual_General.Items.Count; i++)       CheckedListBox_Visual_General.SetItemChecked(i, true);
            else if (sender == Button_Visual_Areas_SelectAll)         for (int i = 0; i < CheckedListBox_Visual_Areas.Items.Count; i++)         CheckedListBox_Visual_Areas.SetItemChecked(i, true);
            else if (sender == Button_Collision_General_SelectAll)    for (int i = 0; i < CheckedListBox_Collision_General.Items.Count; i++)    CheckedListBox_Collision_General.SetItemChecked(i, true);
            else if (sender == Button_Collision_Respected_SelectAll)  for (int i = 0; i < CheckedListBox_Collision_Respected.Items.Count; i++)  CheckedListBox_Collision_Respected.SetItemChecked(i, true);
            else if (sender == Button_Collision_Surfaces_SelectAll)   for (int i = 0; i < CheckedListBox_Collision_Surfaces.Items.Count; i++)   CheckedListBox_Collision_Surfaces.SetItemChecked(i, true);
            else if (sender == Button_Package_General_SelectAll)      for (int i = 0; i < CheckedListBox_Package_General.Items.Count; i++)      CheckedListBox_Package_General.SetItemChecked(i, true);
            else if (sender == Button_Package_Characters_SelectAll)   for (int i = 0; i < CheckedListBox_Package_Characters.Items.Count; i++)   CheckedListBox_Package_Characters.SetItemChecked(i, true);
        }

        /// <summary>
        /// Deselect all items in the CheckedListBox depending on sender
        /// </summary>
        private void Button_DeselectAll_Click(object sender, EventArgs e) {
            // This also looks messy, we also know...
            if (sender == Button_Placement_General_DeselectAll)         for (int i = 0; i < CheckedListBox_Placement_General.Items.Count; i++)    CheckedListBox_Placement_General.SetItemChecked(i, false);
            else if (sender == Button_Placement_Enemies_DeselectAll)    for (int i = 0; i < CheckedListBox_Placement_Enemies.Items.Count; i++)    CheckedListBox_Placement_Enemies.SetItemChecked(i, false);
            else if (sender == Button_Placement_Characters_DeselectAll) for (int i = 0; i < CheckedListBox_Placement_Characters.Items.Count; i++) CheckedListBox_Placement_Characters.SetItemChecked(i, false);
            else if (sender == Button_Placement_Items_DeselectAll)      for (int i = 0; i < CheckedListBox_Placement_Items.Items.Count; i++)      CheckedListBox_Placement_Items.SetItemChecked(i, false);
            else if (sender == Button_Scene_General_DeselectAll)        for (int i = 0; i < CheckedListBox_Scene_General.Items.Count; i++)        CheckedListBox_Scene_General.SetItemChecked(i, false);
            else if (sender == Button_Audio_General_DeselectAll)        for (int i = 0; i < CheckedListBox_Audio_General.Items.Count; i++)        CheckedListBox_Audio_General.SetItemChecked(i, false);
            else if (sender == Button_Audio_Music_DeselectAll)          for (int i = 0; i < CheckedListBox_Audio_Music.Items.Count; i++)          CheckedListBox_Audio_Music.SetItemChecked(i, false);
            else if (sender == Button_Text_General_DeselectAll)         for (int i = 0; i < CheckedListBox_Text_General.Items.Count; i++)         CheckedListBox_Text_General.SetItemChecked(i, false);
            else if (sender == Button_Text_Languages_DeselectAll)       for (int i = 0; i < CheckedListBox_Text_Languages.Items.Count; i++)       CheckedListBox_Text_Languages.SetItemChecked(i, false);
            else if (sender == Button_Visual_General_DeselectAll)       for (int i = 0; i < CheckedListBox_Visual_General.Items.Count; i++)       CheckedListBox_Visual_General.SetItemChecked(i, false);
            else if (sender == Button_Visual_Areas_DeselectAll)         for (int i = 0; i < CheckedListBox_Visual_Areas.Items.Count; i++)         CheckedListBox_Visual_Areas.SetItemChecked(i, false);
            else if (sender == Button_Collision_General_DeselectAll)    for (int i = 0; i < CheckedListBox_Collision_General.Items.Count; i++)    CheckedListBox_Collision_General.SetItemChecked(i, false);
            else if (sender == Button_Collision_Respected_DeselectAll)  for (int i = 0; i < CheckedListBox_Collision_Respected.Items.Count; i++)  CheckedListBox_Collision_Respected.SetItemChecked(i, false);
            else if (sender == Button_Collision_Surfaces_DeselectAll)   for (int i = 0; i < CheckedListBox_Collision_Surfaces.Items.Count; i++)   CheckedListBox_Collision_Surfaces.SetItemChecked(i, false);
            else if (sender == Button_Package_General_DeselectAll)      for (int i = 0; i < CheckedListBox_Package_General.Items.Count; i++)      CheckedListBox_Package_General.SetItemChecked(i, false);
            else if (sender == Button_Package_Characters_DeselectAll)   for (int i = 0; i < CheckedListBox_Package_Characters.Items.Count; i++)   CheckedListBox_Package_Characters.SetItemChecked(i, false);
        }

        /// <summary>
        /// Browse for content depending on sender
        /// </summary>
        private void Button_Browse_Click(object sender, EventArgs e) {
            // Sender is Button_ModsDirectory
            if (sender == Button_ModsDirectory) {
                string browseMods = Dialogs.FolderBrowser("Please select your mods directory...");

                if (browseMods != string.Empty) {
                    Properties.Settings.Default.Path_ModsDirectory = TextBox_ModsDirectory.Text = browseMods;
                    Properties.Settings.Default.Save();
                }

            // Sender is Button_GameExecutable
            } else if (sender == Button_GameExecutable) {
                string browseGame = Dialogs.FileBrowser("Please select an executable for Sonic '06...",
                                                        "Exectuables (*.xex; *.bin)|*.xex;*.bin|" +
                                                        "Xbox Executable (*.xex)|*.xex|" +
                                                        "PlayStation Executable (*.bin)|*.bin");

                if (browseGame != string.Empty) {
                    Properties.Settings.Default.Path_GameExecutable = TextBox_GameExecutable.Text = browseGame;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
