using System;
using System.IO;
using System.Linq;
using HedgeLib.Sets;
using Unify.Messenger;
using Unify.TabControl;
using System.Windows.Forms;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    public partial class Main : Form
    {
        public static Random rng = new Random();
        public static List<int> Items = new List<int>();
        public static List<string> Enemies = new List<string>(),
                                   Characters = new List<string>(),
                                   Music = new List<string>(),
                                   Languages = new List<string>(),
                                   Areas = new List<string>();

        public Main() {
            InitializeComponent();

            Label_VersionNumber.Text = Program.GlobalVersionNumber;
            Properties.Settings.Default.SettingsSaving += Default_SettingsSaving;
            LoadSettings();
        }

        private void Default_SettingsSaving(object sender, System.ComponentModel.CancelEventArgs e) => LoadSettings();

        private void LoadSettings() {
            TextBox_ModsDirectory.Text = Properties.Settings.Default.Path_ModsDirectory;
            TextBox_GameExecutable.Text = Properties.Settings.Default.Path_GameExecutable;
            TextBox_RandomisationSeed.Text = rng.Next().ToString();
        }

        private void UnifyTabControl_Selected(object sender, TabControlEventArgs e) => ((UnifyTabControl)sender).Refresh();

        private void Button_Randomise_Click(object sender, EventArgs e) {
            Enemies.Clear();
            Characters.Clear();
            Items.Clear();
            Music.Clear();
            Languages.Clear();
            Areas.Clear();

            // Feed seed to Random Number Generator
            rng = new Random(TextBox_RandomisationSeed.Text.GetHashCode());

            // Create mod directory - if it's empty, return nothing.
            string modDirectory = Mods.Create(TextBox_RandomisationSeed.Text);
            if (modDirectory == string.Empty) return;

            // Setup various elements of the Randomisation process
            // Create the Valid Enemy list from CheckedListBox_Placement_Enemies 
            foreach (int item in CheckedListBox_Placement_Enemies.CheckedIndices)
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

            // Create the Valid Character list from CheckedListBox_Placement_Characters
            foreach (int item in CheckedListBox_Placement_Characters.CheckedIndices)
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

            // Create the valid Items list from CheckedListBox_Placement_Items
            foreach (int item in CheckedListBox_Placement_Items.CheckedIndices) Items.Add(item + 1);

            // Create the Valid Music list from CheckedListBox_Audio_Music
            foreach (int item in CheckedListBox_Audio_Music.CheckedIndices)
            {
                switch (item)
                {
                    // Stages
                    case 0:  Music.Add("stg_wvo_a");           break;
                    case 1:  Music.Add("stg_wvo_b");           break;
                    case 2:  Music.Add("stg_dtd_a");           break;
                    case 3:  Music.Add("stg_dtd_b");           break;
                    case 4:  Music.Add("stg_wap_a");           break;
                    case 5:  Music.Add("stg_wap_b");           break;
                    case 6:  Music.Add("stg_csc_a");           break;
                    case 7:  Music.Add("stg_csc_b");           break;
                    case 8:  Music.Add("stage_csc_e");         break;
                    case 9:  Music.Add("stg_csc_f");           break;
                    case 10: Music.Add("stg_flc_a");           break;
                    case 11: Music.Add("stg_flc_b");           break;
                    case 12: Music.Add("stg_rct_a");           break;
                    case 13: Music.Add("stg_rct_b");           break;
                    case 14: Music.Add("stg_tpj_a");           break;
                    case 15: Music.Add("stg_tpj_b");           break;
                    case 16: Music.Add("stg_tpj_c");           break;
                    case 17: Music.Add("stg_kdv_a");           break;
                    case 18: Music.Add("stg_kdv_b");           break;
                    case 19: Music.Add("stg_kdv_c");           break;
                    case 20: Music.Add("stg_kdv_d");           break;
                    case 21: Music.Add("stg_aqa_a");           break;
                    case 22: Music.Add("stg_aqa_b");           break;
                    case 23: Music.Add("stg_end_a");           break;
                    case 24: Music.Add("stg_end_b");           break;
                    case 25: Music.Add("stg_end_c");           break;
                    case 26: Music.Add("stg_end_d");           break;
                    case 27: Music.Add("stg_end_e");           break;
                    case 28: Music.Add("stg_end_f");           break;
                    case 29: Music.Add("stg_end_g");           break;

                    // Bosses
                    case 30: Music.Add("boss_iblis01");        break;
                    case 31: Music.Add("boss_iblis03");        break;
                    case 32: Music.Add("boss_mefiless01");     break;
                    case 33: Music.Add("boss_mefiless02");     break;
                    case 34: Music.Add("boss_character");      break;
                    case 35: Music.Add("boss_cerberus");       break;
                    case 36: Music.Add("boss_wyvern");         break;
                    case 37: Music.Add("boss_solaris1");       break;
                    case 38: Music.Add("boss_solaris2");       break;

                    // Town
                    case 39: Music.Add("stg_twn_a");           break;
                    case 40: Music.Add("stg_twn_b");           break;
                    case 41: Music.Add("stg_twn_c");           break;
                    case 42: Music.Add("stg_twn_shop");        break;
                    case 43: Music.Add("twn_mission_slow");    break;
                    case 44: Music.Add("twn_mission_comical"); break;
                    case 45: Music.Add("twn_mission_fast");    break;
                    case 46: Music.Add("twn_accordion");       break;
                    
                    // Miscellaneous
                    case 47: Music.Add("result");              break;
                    case 48: Music.Add("mainmenu");            break;
                    case 49: Music.Add("select");              break;
                    case 50: Music.Add("extra");               break;
                }
            }

            // Create the Valid Area list from CheckedListBox_Textures_Areas
            foreach (int item in CheckedListBox_Textures_Areas.CheckedIndices)
            {
                switch (item)
                {
                    // Stages
                    case 0:  Areas.Add("stage_wvo_a"); break;
                    case 1:  Areas.Add("stage_wvo_b"); break;
                    case 2:  Areas.Add("stage_dtd_a"); break;
                    case 3:  Areas.Add("stage_dtd_b"); break;
                    case 4:  Areas.Add("stage_wap_a"); break;
                    case 5:  Areas.Add("stage_wap_b"); break;
                    case 6:  Areas.Add("stage_csc_a"); break;
                    case 7:  Areas.Add("stage_csc_b"); break;
                    case 8:  Areas.Add("stage_csc_e"); break;
                    case 9:  Areas.Add("stage_csc_f"); break;
                    case 10: Areas.Add("stage_flc_a"); break;
                    case 11: Areas.Add("stage_flc_b"); break;
                    case 12: Areas.Add("stage_rct_a"); break;
                    case 13: Areas.Add("stage_rct_b"); break;
                    case 14: Areas.Add("stage_tpj_a"); break;
                    case 15: Areas.Add("stage_tpj_b"); break;
                    case 16: Areas.Add("stage_tpj_c"); break;
                    case 17: Areas.Add("stage_kdv_a"); break;
                    case 18: Areas.Add("stage_kdv_b"); break;
                    case 19: Areas.Add("stage_kdv_c"); break;
                    case 20: Areas.Add("stage_kdv_d"); break;
                    case 21: Areas.Add("stage_aqa_a"); break;
                    case 22: Areas.Add("stage_aqa_b"); break;

                    // Bosses
                    case 23: Areas.Add("stage_csc_iblis01"); break;
                    case 24: Areas.Add("stage_boss_iblis02"); break;
                    case 25: Areas.Add("stage_boss_iblis03"); break;
                    case 26: Areas.Add("stage_boss_mefi01"); break;
                    case 27: Areas.Add("stage_boss_mefi02"); break;
                    case 28: Areas.Add("stage_boss_rct"); break;
                    case 29: Areas.Add("stage_boss_dr1_dtd"); break;
                    case 30: Areas.Add("stage_boss_dr1_wap"); break;
                    case 31: Areas.Add("stage_boss_dr2"); break;
                    case 32: Areas.Add("stage_boss_dr3"); break;
                    case 33: Areas.Add("stage_boss_solaris"); break;

                    // Town
                    case 34: Areas.Add("stage_twn_a"); break;
                    case 35: Areas.Add("stage_twn_b"); break;
                    case 36: Areas.Add("stage_twn_c"); break;
                    case 37: Areas.Add("stage_twn_d"); break;
                }
            }

            // Unpack ARCs in preperation for randomisation
            foreach (string archive in Directory.GetFiles(Path.GetDirectoryName(Properties.Settings.Default.Path_GameExecutable), "*.arc", SearchOption.AllDirectories)) {
                Console.WriteLine(archive);
                // Miscellaneous Game Data
                if (Path.GetFileName(archive).ToLower() == "scripts.arc")
                {
                    // Unpack the archive
                    string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    // Placement Randomisation
                    if (CheckedListBox_Placement_General.CheckedIndices.Count != 0)
                    {
                        foreach (string setData in Directory.GetFiles(randomArchive, "*.set", SearchOption.AllDirectories))
                        {
                            Console.WriteLine(setData);
                            S06SetData set = new S06SetData();
                            set.Load(setData);

                            foreach (int item in CheckedListBox_Placement_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Placement.RandomiseEnemies(set, rng); break;
                                    case 1: Placement.RandomiseCharacters(set, rng); break;
                                    case 2: Placement.RandomiseItems(set, rng); break;
                                    case 3: Placement.RandomiseVoices(set, rng); break;
                                    case 4: Placement.RandomisePhysicsProps(set, rng); break;
                                    case 5: Beatable.DetermineStage(setData, set); break;
                                }
                            }

                            set.Save(setData, true);
                        }
                    }

                    // Scene Randomisation
                    if (CheckedListBox_Scene_General.CheckedIndices.Count != 0)
                    {
                        foreach (string lubData in Directory.GetFiles(randomArchive, "scene*.lub", SearchOption.AllDirectories))
                        {
                            Console.WriteLine(lubData);
                            Lua.Decompile(lubData);
                            string[] editedLub = File.ReadAllLines(lubData);

                            foreach (int item in CheckedListBox_Scene_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Scene.RandomiseLight(editedLub, "Ambient", rng); break;
                                    case 1: Scene.RandomiseLight(editedLub, "Main", rng); break;
                                    case 2: Scene.RandomiseLight(editedLub, "Sub", rng); break;
                                    case 3: Scene.RandomiseLightDirection(editedLub, rng); break;
                                    case 4: Scene.RandomiseFogColour(editedLub, rng); break;
                                    case 5: Scene.RandomiseFogDensity(editedLub, rng); break;
                                    case 6: Scene.RandomiseEnvironmentMaps(editedLub, rng); break;
                                }
                            }

                            File.WriteAllLines(lubData, editedLub);
                        }
                    }

                    // Audio Randomisation
                    if (CheckedListBox_Audio_General.CheckedIndices.Count != 0)
                    {
                        foreach (string lubData in Directory.GetFiles(randomArchive, "*.lub", SearchOption.AllDirectories)
                            .Where(x => Path.GetFileName(x).StartsWith("a_") || Path.GetFileName(x).StartsWith("b_") || Path.GetFileName(x).StartsWith("c_") ||
                                        Path.GetFileName(x).StartsWith("d_") || Path.GetFileName(x).StartsWith("e_") || Path.GetFileName(x).StartsWith("f_") ||
                                        Path.GetFileName(x).StartsWith("f1_") || Path.GetFileName(x).StartsWith("f2_") || Path.GetFileName(x).StartsWith("g_")))
                        {
                            Console.WriteLine(lubData);
                            Lua.Decompile(lubData);
                            string[] editedLub = File.ReadAllLines(lubData);

                            foreach (int item in CheckedListBox_Audio_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Audio.RandomiseMusic(editedLub, rng); break;
                                }
                            }

                            File.WriteAllLines(lubData, editedLub);
                        }
                    }

                    // Text Randomisation
                    if (CheckedListBox_Text_General.GetItemChecked(0))
                    {
                        foreach (string lubData in Directory.GetFiles(randomArchive, "mission*.lub", SearchOption.AllDirectories))
                        {
                            Console.WriteLine(lubData);
                            Lua.Decompile(lubData);
                            string[] editedLub = File.ReadAllLines(lubData);

                            foreach (int item in CheckedListBox_Text_General.CheckedIndices)
                            {
                                switch (item)
                                {
                                    case 0: Strings.RandomiseLoadingText(editedLub, rng); break;
                                }
                            }

                            File.WriteAllLines(lubData, editedLub);
                        }
                    }

                    // Repack the archive
                    Archives.CreateModARC(randomArchive, archive, modDirectory);
                }

                // Text Data
                else if (Path.GetFileName(archive).ToLower() == "text.arc" ||
                         Path.GetFileName(archive).ToLower() == "event.arc")
                {
                    // Unpack the archive
                    string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    // Text Randomisation
                    if (CheckedListBox_Text_General.GetItemChecked(1))
                    {
                        foreach (int item in CheckedListBox_Text_Languages.CheckedIndices)
                        {
                            switch (item)
                            {
                                case 0: Languages.Add(".e"); break;
                                case 1: Languages.Add(".f"); break;
                                case 2: Languages.Add(".g"); break;
                                case 3: Languages.Add(".i"); break;
                                case 4: Languages.Add(".j"); break;
                                case 5: Languages.Add(".s"); break;
                            }
                        }

                        Strings.RandomiseMSTContents(randomArchive, Languages, rng);
                    }

                    // Repack the archive
                    Archives.CreateModARC(randomArchive, archive, modDirectory);
                }

                // Texture Randomisation
                if ((Areas.Contains(Path.GetFileNameWithoutExtension(archive)) && CheckedListBox_Textures_General.GetItemChecked(0)) ||
                    (Path.GetFileName(archive).ToLower() == "object.arc"       && CheckedListBox_Textures_General.GetItemChecked(1)) ||
                    (Path.GetFileName(archive).ToLower() == "sprite.arc"       && CheckedListBox_Textures_General.GetItemChecked(2)))
                {
                    // Unpack the archive
                    string randomArchive = Archives.UnpackARC(archive, Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    // Randomise the textures
                    Textures.RandomiseTextures(randomArchive, rng);

                    // Repack the archive
                    Archives.CreateModARC(randomArchive, archive, modDirectory);
                }
            }
        }

        private void Button_About_Click(object sender, EventArgs e) {
            UnifyMessenger.UnifyMessage.ShowDialog("Sonic '06 Randomiser Suite\n" +
                                                   $"{Program.GlobalVersionNumber}\n\n" +
                                                   "" +
                                                   "Knuxfan24 - Lead programmer and reverse-engineer\n" +
                                                   "HyperPolygon64 - UI stuff and slaved away at code",
                                                   "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Button_RandomisationSeed_Click(object sender, EventArgs e) => TextBox_RandomisationSeed.Text = rng.Next().ToString();

        private void Button_SelectAll_Click(object sender, EventArgs e) {
            if (sender == Button_Placement_General_SelectAll)         for (int i = 0; i < CheckedListBox_Placement_General.Items.Count; i++)    CheckedListBox_Placement_General.SetItemChecked(i, true);
            else if (sender == Button_Placement_Enemies_SelectAll)    for (int i = 0; i < CheckedListBox_Placement_Enemies.Items.Count; i++)    CheckedListBox_Placement_Enemies.SetItemChecked(i, true);
            else if (sender == Button_Placement_Characters_SelectAll) for (int i = 0; i < CheckedListBox_Placement_Characters.Items.Count; i++) CheckedListBox_Placement_Characters.SetItemChecked(i, true);
            else if (sender == Button_Placement_Items_SelectAll)      for (int i = 0; i < CheckedListBox_Placement_Items.Items.Count; i++)      CheckedListBox_Placement_Items.SetItemChecked(i, true);
            else if (sender == Button_Scene_General_SelectAll)        for (int i = 0; i < CheckedListBox_Scene_General.Items.Count; i++)        CheckedListBox_Scene_General.SetItemChecked(i, true);
            else if (sender == Button_Audio_General_SelectAll)        for (int i = 0; i < CheckedListBox_Audio_General.Items.Count; i++)        CheckedListBox_Audio_General.SetItemChecked(i, true);
            else if (sender == Button_Audio_Music_SelectAll)          for (int i = 0; i < CheckedListBox_Audio_Music.Items.Count; i++)          CheckedListBox_Audio_Music.SetItemChecked(i, true);
            else if (sender == Button_Text_General_SelectAll)         for (int i = 0; i < CheckedListBox_Text_General.Items.Count; i++)         CheckedListBox_Text_General.SetItemChecked(i, true);
            else if (sender == Button_Text_Languages_SelectAll)       for (int i = 0; i < CheckedListBox_Text_Languages.Items.Count; i++)       CheckedListBox_Text_Languages.SetItemChecked(i, true);
            else if (sender == Button_Textures_General_SelectAll)     for (int i = 0; i < CheckedListBox_Textures_General.Items.Count; i++)     CheckedListBox_Textures_General.SetItemChecked(i, true);
            else if (sender == Button_Textures_Areas_SelectAll)       for (int i = 0; i < CheckedListBox_Textures_Areas.Items.Count; i++)       CheckedListBox_Textures_Areas.SetItemChecked(i, true);
        }

        private void Button_DeselectAll_Click(object sender, EventArgs e) {
            if (sender == Button_Placement_General_DeselectAll)         for (int i = 0; i < CheckedListBox_Placement_General.Items.Count; i++)    CheckedListBox_Placement_General.SetItemChecked(i, false);
            else if (sender == Button_Placement_Enemies_DeselectAll)    for (int i = 0; i < CheckedListBox_Placement_Enemies.Items.Count; i++)    CheckedListBox_Placement_Enemies.SetItemChecked(i, false);
            else if (sender == Button_Placement_Characters_DeselectAll) for (int i = 0; i < CheckedListBox_Placement_Characters.Items.Count; i++) CheckedListBox_Placement_Characters.SetItemChecked(i, false);
            else if (sender == Button_Placement_Items_DeselectAll)      for (int i = 0; i < CheckedListBox_Placement_Items.Items.Count; i++)      CheckedListBox_Placement_Items.SetItemChecked(i, false);
            else if (sender == Button_Scene_General_DeselectAll)        for (int i = 0; i < CheckedListBox_Scene_General.Items.Count; i++)        CheckedListBox_Scene_General.SetItemChecked(i, false);
            else if (sender == Button_Audio_General_DeselectAll)        for (int i = 0; i < CheckedListBox_Audio_General.Items.Count; i++)        CheckedListBox_Audio_General.SetItemChecked(i, false);
            else if (sender == Button_Audio_Music_DeselectAll)          for (int i = 0; i < CheckedListBox_Audio_Music.Items.Count; i++)          CheckedListBox_Audio_Music.SetItemChecked(i, false);
            else if (sender == Button_Text_General_DeselectAll)         for (int i = 0; i < CheckedListBox_Text_General.Items.Count; i++)         CheckedListBox_Text_General.SetItemChecked(i, false);
            else if (sender == Button_Text_Languages_DeselectAll)       for (int i = 0; i < CheckedListBox_Text_Languages.Items.Count; i++)       CheckedListBox_Text_Languages.SetItemChecked(i, false);
            else if (sender == Button_Textures_General_DeselectAll)     for (int i = 0; i < CheckedListBox_Textures_General.Items.Count; i++)     CheckedListBox_Textures_General.SetItemChecked(i, false);
            else if (sender == Button_Textures_Areas_DeselectAll)       for (int i = 0; i < CheckedListBox_Textures_Areas.Items.Count; i++)       CheckedListBox_Textures_Areas.SetItemChecked(i, false);
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
