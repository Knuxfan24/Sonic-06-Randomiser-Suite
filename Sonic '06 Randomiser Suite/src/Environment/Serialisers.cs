using System;
using System.IO;
using System.Linq;
using Unify.Messenger;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sonic_06_Randomiser_Suite.Serialisers
{
    class Mods
    {
        /// <summary>
        /// Creates a mod folder and INI for Sonic '06 Mod Manager
        /// </summary>
        public static string Create(string seed, bool beatable) {
            // Create folder name that's safe for character limitations
            string safeTitle = Literal.UseSafeFormattedCharacters($"Sonic '06 Randomised ({Paths.Truncate(seed, 150)})");

            string newPath = Path.Combine(Properties.Settings.Default.Path_ModsDirectory, safeTitle);

            if (Directory.Exists(newPath)) {
                DialogResult overwrite = UnifyMessenger.UnifyMessage.ShowDialog($"The seed '{seed}' has already been generated. Do you want to overwrite it?",
                                                                                "Mod Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (overwrite == DialogResult.No) return string.Empty;
            }

            Directory.CreateDirectory(newPath);

            using (Stream configCreate = File.Open(Path.Combine(newPath, "mod.ini"), FileMode.Create))
                using (StreamWriter configInfo = new StreamWriter(configCreate)) {
                    configInfo.WriteLine("[Details]");
                    configInfo.WriteLine($"Title=\"Sonic '06 Randomised\"");
                    configInfo.WriteLine($"Version=\"{seed}\"");
                    configInfo.WriteLine($"Date=\"{DateTime.Now:dd/MM/yyyy}\"");
                    configInfo.WriteLine($"Author=\"Sonic '06 Randomiser Suite\"");
                    configInfo.WriteLine($"Platform=\"{Literal.System(Properties.Settings.Default.Path_GameExecutable)}\"");

                    if (beatable) configInfo.WriteLine($"[Patches]\nRequiredPatches=\"UnlockMidairMomentum.mlua\"");

                    configInfo.WriteLine("\n[Filesystem]");
                    configInfo.WriteLine($"Merge=\"False\"");
                    configInfo.WriteLine($"CustomFilesystem=\"False\"");

                    configInfo.Close();
                }

            File.WriteAllText(Path.Combine(newPath, "license.txt"), Properties.Resources.License);

            return newPath;
        }

        /// <summary>
        /// Creates the required directories for archives based on the version of the game.
        /// </summary>
        public static string CreateFolderDynamically(string modDirectory, string extensions, string system) {
            if (system == "Xbox 360") {
                Directory.CreateDirectory(Path.Combine(modDirectory, $"xenon\\{extensions}"));
                return Path.Combine(modDirectory, $"xenon\\{extensions}");
            } else if (system == "PlayStation 3") {
                Directory.CreateDirectory(Path.Combine(modDirectory, $"ps3\\{extensions}"));
                return Path.Combine(modDirectory, $"ps3\\{extensions}");
            } else
                return string.Empty;
        }
    }

    class Literal
    {
        /// <summary>
        /// Translates a file extension to 'Xbox 360' or 'PlayStation 3'
        /// </summary>
        public static string System(string path) {
            if (Path.GetExtension(path).ToLower() == ".xex") return "Xbox 360";
            if (Path.GetExtension(path).ToLower() == ".bin") return "PlayStation 3";
            else return "unspecified";
        }

        /// <summary>
        /// Translates a file extension to 'xenon' or 'ps3'
        /// </summary>
        public static string Core(string path) {
            if (Path.GetExtension(path).ToLower() == ".xex") return "xenon";
            else if (Path.GetExtension(path).ToLower() == ".bin") return "ps3";
            else return "core";
        }

        /// <summary>
        /// Renames the 'core' folder to the appropriate system root.
        /// </summary>
        public static string CoreReplace(string path) {
            string system = System(Properties.Settings.Default.Path_GameExecutable);

            if (Paths.GetRootFolder(path) == "core") {
                string[] splitPath = path.Split('\\');

                for (int i = 0; i < splitPath.Length; i++) {
                    if (splitPath[i] == "core" && system == "Xbox 360") {
                        splitPath[i] = "xenon";
                        return string.Join("\\", splitPath);
                    }
                    else if (splitPath[i] == "core" && system == "PlayStation 3") {
                        splitPath[i] = "ps3";
                        return string.Join("\\", splitPath);
                    }
                }

                return string.Join("\\", splitPath);
            } else
                return path;
        }

        /// <summary>
        /// Removes illegal characters from the path in a cleaner format.
        /// </summary>
        public static string UseSafeFormattedCharacters(string text) {
            return text.Replace(@"\", "")
                       .Replace("/",  "-")
                       .Replace(":",  "-")
                       .Replace("*",  "")
                       .Replace("?",  "")
                       .Replace("\"", "'")
                       .Replace("<",  "")
                       .Replace(">",  "")
                       .Replace("|",  "");
        }
    }

    class Resources
    {
        /// <summary>
        /// Splits a line broken resource into a string array.
        /// </summary>
        /// <param name="document">Text resource.</param>
        public static string[] ParseLineBreaks(string document) {
            return document.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None );
        }

        /// <summary>
        /// Enumerates the valid enemies selected by the user
        /// </summary>
        public static List<string> EnumerateEnemiesList(CheckedListBox items)
        {
            List<string> Enemies = new List<string>();

            foreach (int item in items.CheckedIndices)
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

            return Enemies;
        }

        /// <summary>
        /// Enumerates the valid characters selected by the user
        /// </summary>
        public static List<string> EnumerateCharactersList_Placement(CheckedListBox items)
        {
            List<string> Characters = new List<string>();

            foreach (int item in items.CheckedIndices)
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

            return Characters;
        }

        /// <summary>
        /// Enumerates the valid characters selected by the user
        /// </summary>
        public static List<string> EnumerateExtendedCharactersList(CheckedListBox items, string resource)
        {
            List<string> Characters = new List<string>();

            foreach (int item in items.CheckedIndices)
            {
                if (items.Name == "CheckedListBox_Package_Characters")
                {
                    switch (item)
                    {
                        case 0:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("sonic") && !file.Contains("sonic_fast")) Characters.Add(file);
                            break;
                        case 1:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("sonic_fast")) Characters.Add(file);
                            break;
                        case 2:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("princess") || file.Contains("elise")) Characters.Add(file);
                            break;
                        case 3:
                            Characters.Add("snow_board.pkg");
                            break;
                        case 4:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("shadow")) Characters.Add(file);
                            break;
                        case 5:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("silver")) Characters.Add(file);
                            break;
                        case 6:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("tails")) Characters.Add(file);
                            break;
                        case 7:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("knuckles")) Characters.Add(file);
                            break;
                        case 8:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("rouge")) Characters.Add(file);
                            break;
                        case 9:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("omega")) Characters.Add(file);
                            break;
                        case 10:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("blaze")) Characters.Add(file);
                            break;
                        case 11:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("amy")) Characters.Add(file);
                            break;
                    }
                }
                else if (items.Name == "CheckedListBox_Lua_Characters")
                {
                    switch (item)
                    {
                        case 0:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("sonic") && !file.Contains("sonic_fast")) Characters.Add(file);
                            break;
                        case 1:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("sonic_fast")) Characters.Add(file);
                            break;
                        case 2:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("princess") || file.Contains("elise")) Characters.Add(file);
                            break;
                        case 3:
                            Characters.Add("snow_board_wap.lub");
                            break;
                        case 4:
                            Characters.Add("snow_board.lub");
                            break;
                        case 5:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("shadow")) Characters.Add(file);
                            break;
                        case 6:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("silver")) Characters.Add(file);
                            break;
                        case 7:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("tails")) Characters.Add(file);
                            break;
                        case 8:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("knuckles")) Characters.Add(file);
                            break;
                        case 9:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("rouge")) Characters.Add(file);
                            break;
                        case 10:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("omega")) Characters.Add(file);
                            break;
                        case 11:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("blaze")) Characters.Add(file);
                            break;
                        case 12:
                            foreach (string file in ParseLineBreaks(resource))
                                if (file.Contains("amy")) Characters.Add(file);
                            break;
                    }
                }
            }

            return Characters;
        }

        /// <summary>
        /// Enumerates the valid items selected by the user
        /// </summary>
        public static List<int> EnumerateItemsList(CheckedListBox items)
        {
            List<int> Items = new List<int>();

            foreach (int item in items.CheckedIndices) Items.Add(item + 1);

            return Items;
        }

        /// <summary>
        /// Enumerates the valid music selected by the user
        /// </summary>
        public static List<string> EnumerateMusicList(CheckedListBox items)
        {
            List<string> Music = new List<string>();

            foreach (int item in items.CheckedIndices)
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

            return Music;
        }

        /// <summary>
        /// Enumerates the valid areas selected by the user
        /// </summary>
        public static List<string> EnumerateAreasList(CheckedListBox items)
        {
            List<string> Areas = new List<string>();

            foreach (int item in items.CheckedIndices)
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

            return Areas;
        }

        /// <summary>
        /// Enumerates the valid languages selected by the user
        /// </summary>
        public static List<string> EnumerateLanguagesList(CheckedListBox items)
        {
            List<string> Languages = new List<string>();

            foreach (int item in items.CheckedIndices)
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

            return Languages;
        }

        /// <summary>
        /// Enumerates the valid collision surface tags selected by the user
        /// </summary>
        public static List<string> EnumerateCollisionList(CheckedListBox items)
        {
            List<string> Collision = new List<string>();

            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    case 0: Collision.Add("0"); break;
                    case 1: Collision.Add("1"); break;
                    case 2: Collision.Add("2"); break;
                    case 3: Collision.Add("3"); break;
                    case 4: Collision.Add("5"); break;
                    case 5: Collision.Add("6"); break;
                    case 6: Collision.Add("8"); break;
                    case 7: Collision.Add("9"); break;
                    case 8: Collision.Add("A"); break;
                    case 9: Collision.Add("E"); break;
                }
            }

            return Collision;
        }

        /// <summary>
        /// Enumerates the valid Lua parameters selected by the user
        /// </summary>
        public static List<string> EnumerateParameterList(CheckedListBox items)
        {
            List<string> Parameters = new List<string>();

            foreach (int item in items.CheckedIndices)
            {
                switch (item)
                {
                    case 0: Parameters.Add("c_walk_speed_max"); break;
                    case 1: Parameters.Add("c_run_speed_max"); break;
                    case 2: Parameters.Add("l_jump_hight"); break;
                    case 3: Parameters.Add("c_custom_action_machspeed_acc"); break;
                    case 4: Parameters.Add("c_custom_action_slow_bias"); break;
                }
            }

            return Parameters;
        }
    }

    class Paths
    {
        /// <summary>
        /// Truncates a string to make it safe for paths
        /// </summary>
        public static string Truncate(string value, int maxLength) {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : $"{value.Substring(0, maxLength)}...";
        }

        /// <summary>
        /// Returns the first directory of a path.
        /// </summary>
        public static string GetRootFolder(string path) {
            while (true) {
                string temp = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(temp))
                    break;
                path = temp;
            }
            return path;
        }

        /// <summary>
        /// Returns the full path without an extension.
        /// </summary>
        public static string GetPathWithoutExtension(string path) {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// Returns the folder containing the file.
        /// </summary>
        public static string GetContainingFolder(string path) {
            return Path.GetFileName(Path.GetDirectoryName(path));
        }

        /// <summary>
        /// Returns if the directory is empty.
        /// </summary>
        public static bool IsDirectoryEmpty(string path) {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Checks if the path is valid and exists.
        /// </summary>
        public static bool CheckPathLegitimacy(string path) {
            if (Directory.Exists(path) && path != string.Empty) return true;
            else return false;
        }

        /// <summary>
        /// Checks if the path is valid and exists.
        /// </summary>
        public static bool CheckFileLegitimacy(string path) {
            if (File.Exists(path) && path != string.Empty) return true;
            else return false;
        }

        /// <summary>
        /// Returns a new path with the specified filename.
        /// </summary>
        public static string ReplaceFilename(string path, string newFile) {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(newFile));
        }

        /// <summary>
        /// Collects the general file structure for '06 data.
        /// </summary>
        public static List<string> CollectGameData(string path) {
            return Directory.GetFiles(path, "*.arc", SearchOption.AllDirectories).ToList();
        }

        /// <summary>
        /// Compares two strings to check if one is a subdirectory of the other.
        /// </summary>
        public static bool IsSubdirectory(string candidate, string other) {
            var isChild = false;

            try {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (candidateInfo.Parent != null) {
                    if (candidateInfo.Parent.FullName == otherInfo.FullName) {
                        isChild = true;
                        break;
                    } else candidateInfo = candidateInfo.Parent;
                }
            } catch (Exception ex) {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss tt}] [Error] Failed to check directories...\n{ex}");
            }

            return isChild;
        }
    }
}
