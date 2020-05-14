using System;
using System.IO;
using HedgeLib.Sets;
using Unify.TabControl;
using System.Windows.Forms;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    public partial class Main : Form
    {
        public static Random rng = new Random();
        public static List<string> Enemies = new List<string>(), Characters = new List<string>();
        public static List<int> Items = new List<int>();

        public Main() {
            InitializeComponent();

            Properties.Settings.Default.SettingsSaving += Default_SettingsSaving;
            LoadSettings();
        }

        private void Default_SettingsSaving(object sender, System.ComponentModel.CancelEventArgs e) => LoadSettings();

        private void LoadSettings() {
            TextBox_ModsDirectory.Text = Properties.Settings.Default.Path_ModsDirectory;
            TextBox_GameExecutable.Text = Properties.Settings.Default.Path_GameExecutable;
            TextBox_RandomisationSeed.Text = rng.Next().ToString();

            SplitContainer_GeneralControls.SplitterWidth =
            SplitContainer_EnemiesControls.SplitterWidth =
            SplitContainer_CharactersControls.SplitterWidth =
            SplitContainer_ItemsControls.SplitterWidth = 1;
        }

        private void UnifyTabControl_Selected(object sender, TabControlEventArgs e) => ((UnifyTabControl)sender).Refresh();

        private void Button_Randomise_Click(object sender, EventArgs e) {
            Enemies.Clear();
            Characters.Clear();
            Items.Clear();

            // Feed seed to Random Number Generator
            rng = new Random(TextBox_RandomisationSeed.Text.GetHashCode());

            string modDirectory = Mods.Create(TextBox_RandomisationSeed.Text);

            // Setup various elements of the Randomisation process
            // Create the Valid Enemy list from CheckedListBox_Enemies 
            foreach (int item in CheckedListBox_Enemies.CheckedIndices)
            {
                switch (item)
                {
                    // Enemies
                    case 0:  Enemies.Add("cBiter");        break;
                    case 1:  Enemies.Add("cGolem");        break;
                    case 2:  Enemies.Add("cTaker");        break;
                    case 3:  Enemies.Add("cCrawler");      break;
                    case 4:  Enemies.Add("cGazer");        break;
                    case 5:  Enemies.Add("cStalker");      break;
                    case 6:  Enemies.Add("cTitan");        break;
                    case 7:  Enemies.Add("cTricker");      break;
                    case 8:  Enemies.Add("eArmor");        break;
                    case 9:  Enemies.Add("eBluster");      break;
                    case 10: Enemies.Add("eBuster");       break;
                    case 11: Enemies.Add("eBuster(Fly)");  break;
                    case 12: Enemies.Add("eBomber");       break;
                    case 13: Enemies.Add("eCannon");       break;
                    case 14: Enemies.Add("eCannon(Fly)");  break;
                    case 15: Enemies.Add("eChaser");       break;
                    case 16: Enemies.Add("eCommander");    break;
                    case 17: Enemies.Add("eFlyer");        break;
                    case 18: Enemies.Add("eGuardian");     break;
                    case 19: Enemies.Add("eGunner");       break;
                    case 20: Enemies.Add("eGunner(Fly)");  break;
                    case 21: Enemies.Add("eHunter");       break;
                    case 22: Enemies.Add("eKeeper");       break;
                    case 23: Enemies.Add("eLancer");       break;
                    case 24: Enemies.Add("eLancer(Fly)");  break;
                    case 25: Enemies.Add("eLiner");        break;
                    case 26: Enemies.Add("eRounder");      break;
                    case 27: Enemies.Add("eSearcher");     break;
                    case 28: Enemies.Add("eStinger");      break;
                    case 29: Enemies.Add("eStinger(Fly)"); break;
                    case 30: Enemies.Add("eSweeper");      break;
                    case 31: Enemies.Add("eWalker");       break;

                    // Bosses
                    case 32: Enemies.Add("eCerberus");     break;
                    case 33: Enemies.Add("eGenesis");      break;
                    case 34: Enemies.Add("eWyvern");       break;
                    case 35: Enemies.Add("firstIblis");    break;
                    case 36: Enemies.Add("secondIblis");   break;
                    case 37: Enemies.Add("thirdIblis");    break;
                    case 38: Enemies.Add("firstmefiress"); break;
                    case 39: Enemies.Add("solaris01");     break;
                    case 40: Enemies.Add("solaris02");     break;
                }
            }

            // Create the Valid Character list from CheckedListBox_Characters
            foreach (int item in CheckedListBox_Characters.CheckedIndices)
            {
                switch (item)
                {
                    case 0:  Characters.Add("sonic_new");      break;
                    case 1:  Characters.Add("sonic_fast");     break;
                    case 2:  Characters.Add("princess");       break;
                    case 3:  Characters.Add("snow_board_wap"); break;
                    case 4:  Characters.Add("snow_board");     break;
                    case 5:  Characters.Add("shadow");         break;
                    case 6:  Characters.Add("silver");         break;
                    case 7:  Characters.Add("tails");          break;
                    case 8:  Characters.Add("knuckles");       break;
                    case 9:  Characters.Add("rouge");          break;
                    case 10: Characters.Add("omega");          break;
                    case 11: Characters.Add("blaze");          break;
                    case 12: Characters.Add("amy");            break;
                }
            }

            // Create the valid Items list from CheckedListBox_Items
            foreach (int item in CheckedListBox_Items.CheckedIndices) Items.Add(item + 1);

            // Unpack ARCs in preperation for randomisation
            foreach (string archive in Paths.CollectGameData(Path.GetDirectoryName(Properties.Settings.Default.Path_GameExecutable))) {
                if (Path.GetFileName(archive).ToLower() == "scripts.arc") {
                    string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    foreach (string setData in Directory.GetFiles(randomArchive, "*.set", SearchOption.AllDirectories)) {
                        Console.WriteLine(setData);
                        S06SetData set = new S06SetData();
                        set.Load(setData);

                        foreach (int item in CheckedListBox_General.CheckedIndices)
                        {
                            switch (item)
                            {
                                case 0: Placement.RandomiseEnemies(set, rng); break;
                                case 1: Placement.RandomiseCharacters(set, rng); break;
                                case 2: Placement.RandomiseItems(set, rng); break;
                                case 3: Placement.RandomiseVoices(set, rng); break;
                                case 4: Placement.RandomisePhysicsProps(set, rng); break;
                            }
                        }

                        set.Save(setData, true);
                    }

                    if (Directory.Exists(modDirectory)) {
                        // Absolute file path (xenon/ps3/win32 and beyond)
                        string filePath = archive.Remove(0, Path.GetDirectoryName(Properties.Settings.Default.Path_GameExecutable).Length).Substring(1);

                        // Absolute file path (from the mod) combined with the game directory
                        string vanillaFilePath = Path.Combine(modDirectory, filePath);

                        // Creates the archive subdirectories
                        string createArchives = Mods.CreateFolderDynamically(modDirectory, "archives", Literal.System(Properties.Settings.Default.Path_GameExecutable));

                        if (createArchives != string.Empty)
                            Archives.RepackARC(randomArchive, Path.Combine(createArchives, Path.GetFileName(archive)));
                    }
                }
            }
        }

        private void Button_RandomisationSeed_Click(object sender, EventArgs e) => TextBox_RandomisationSeed.Text = rng.Next().ToString();

        private void Button_SelectAll_Click(object sender, EventArgs e) {
            if (sender == Button_Placement_General_SelectAll)         for (int i = 0; i < CheckedListBox_General.Items.Count; i++) CheckedListBox_General.SetItemChecked(i, true);
            else if (sender == Button_Placement_Enemies_SelectAll)    for (int i = 0; i < CheckedListBox_Enemies.Items.Count; i++) CheckedListBox_Enemies.SetItemChecked(i, true);
            else if (sender == Button_Placement_Characters_SelectAll) for (int i = 0; i < CheckedListBox_Characters.Items.Count; i++) CheckedListBox_Characters.SetItemChecked(i, true);
            else if (sender == Button_Placement_Items_SelectAll)      for (int i = 0; i < CheckedListBox_Items.Items.Count; i++) CheckedListBox_Items.SetItemChecked(i, true);
        }

        private void Button_DeselectAll_Click(object sender, EventArgs e) {
            if (sender == Button_Placement_General_DeselectAll)         for (int i = 0; i < CheckedListBox_General.Items.Count; i++) CheckedListBox_General.SetItemChecked(i, false);
            else if (sender == Button_Placement_Enemies_DeselectAll)    for (int i = 0; i < CheckedListBox_Enemies.Items.Count; i++) CheckedListBox_Enemies.SetItemChecked(i, false);
            else if (sender == Button_Placement_Characters_DeselectAll) for (int i = 0; i < CheckedListBox_Characters.Items.Count; i++) CheckedListBox_Characters.SetItemChecked(i, false);
            else if (sender == Button_Placement_Items_DeselectAll)      for (int i = 0; i < CheckedListBox_Items.Items.Count; i++) CheckedListBox_Items.SetItemChecked(i, false);
        }

        private void Button_Browse_Click(object sender, EventArgs e) {
            if (sender == Button_ModsDirectory) {
                string browseMods = Dialogs.FolderBrowser("Please select your mods directory...");

                if (browseMods != string.Empty) {
                    Properties.Settings.Default.Path_ModsDirectory = TextBox_ModsDirectory.Text = browseMods;
                    Properties.Settings.Default.Save();
                }
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
