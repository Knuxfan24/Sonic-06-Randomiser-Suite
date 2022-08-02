using Marathon.Formats.Mesh;
using Marathon.Formats.Package;
using Marathon.Formats.Text;
using System.Linq;

namespace MarathonRandomiser
{
    internal class MiscellaneousRandomisers
    {
        /// <summary>
        /// Randomises the amount of health enemies have, controlled by ScriptParameter.bin.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked enemy.arc containing ScriptParameter.bin</param>
        /// <param name="minHealth">The minimum value the enemy health can be set to.</param>
        /// <param name="maxHealth">The maximum value the enemy health can be set to.</param>
        /// <param name="includeBosses">Whether or not bosses should also get their health values randomised.</param>
        public static async Task EnemyHealthRandomiser(string archivePath, int minHealth, int maxHealth, bool? includeBosses)
        {
            // Load ScriptParameter.bin
            ScriptPackage scriptPackage = new($@"{archivePath}\xenon\enemy\ScriptParameter.bin");

            // Loop through every enemy parameter in ScriptParameter.bin
            foreach (ScriptParameter parameter in scriptPackage.Parameters)
            {
                // If bosses are disabled, then ignore anything related to them (except for the Mephiles Kyozoress enemy that shows up in Mephiles Phase 2).
                if (includeBosses == false && (parameter.Name == "firstIblis" || parameter.Name == "secondIblis_sonic" || parameter.Name == "secondIblis_shadow" || parameter.Name == "thirdIblis" ||
                    parameter.Name == "firstmefiress_shadow" || parameter.Name == "firstmefiress_omega" || parameter.Name == "secondmefiress_shadow" || parameter.Name == "kyozoress" ||
                    parameter.Name == "eCerberus_sonic" || parameter.Name == "eCerberus_shadow" || parameter.Name == "eGenesis_sonic" || parameter.Name == "eGenesis_silver" ||
                    parameter.Name == "eGenesis_wing" || parameter.Name == "eGenesisSpotLight" || parameter.Name == "eWyvern" || parameter.Name == "eWyvernOption" || parameter.Name == "eWyvernEggman" ||
                    parameter.Name == "solaris01" || parameter.Name == "solaris02"))
                    continue;

                else
                {
                    // Check this enemy parameter actually has a health value before changing it.
                    if (parameter.Health != -1)
                        parameter.Health = MainWindow.Randomiser.Next(minHealth, maxHealth + 1);
                }
            }

            // Save the updated ScriptParameter.bin.
            scriptPackage.Save();
        }

        /// <summary>
        /// Randomises the final nibble of the collision files to change their surface types.
        /// </summary>
        /// <param name="collisionFile">The filepath to the collision.bin file we're processing.</param>
        /// <param name="perFace">Whether the collision should be randomised per face rather than per type.</param>
        public static async Task SurfaceRandomiser(string collisionFile, bool? perFace)
        {
            // Predetermine what each type will be.
            string concrete = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string water = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string wood = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string metal = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string grass = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string sand = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string snow = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string dirt = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string glass = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string metalEcho = MainWindow.Randomiser.Next(0, 16).ToString("X");

            // Load this collision file.
            Collision collision = new(collisionFile);

            // Loop through this collision file's faces.
            foreach (CollisionFace face in collision.Data.Faces)
            {
                // Parse the hex value of this face's collision tag into a string.
                string hexValue = face.Flags.ToString("X").PadLeft(8, '0');

                // If we're randomising the collision by face rather than type, then replace the last value in the string with a random hex value from 0 to F.
                if (perFace == true)
                    hexValue = hexValue.Remove(hexValue.Length - 1, 1) + MainWindow.Randomiser.Next(0, 16).ToString("X");

                // If not, then check what the last value in the string is and apply the approriate, predetermined value.
                else
                {
                    switch (hexValue[7])
                    {
                        case '0':
                        case '4':
                        case '7':
                        case 'B':
                        case 'C':
                        case 'D':
                        case 'F':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + concrete;
                            break;

                        case '1':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + water;
                            break;

                        case '2':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + wood;
                            break;

                        case '3':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + metal;
                            break;

                        case '5':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + grass;
                            break;

                        case '6':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + sand;
                            break;

                        case '8':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + snow;
                            break;

                        case '9':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + dirt;
                            break;

                        case 'A':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + glass;
                            break;

                        case 'E':
                            hexValue = hexValue.Remove(hexValue.Length - 1, 1) + metalEcho;
                            break;
                    }
                }

                // Convert the hex string back into a hex number and apply it to this face's flags.
                face.Flags = uint.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
            }

            // Save the updated collison.bin.
            collision.Save();
        }

        /// <summary>
        /// Sets random patches to be required so the Mod Manager will auto install them with the randomisation.
        /// </summary>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="MiscPatches">The list of valid patch files.</param>
        /// <param name="Weight">The likelyhood for a patch to be enabled.</param>
        public static async Task PatchRandomiser(string ModDirectory, List<string> MiscPatches, int Weight)
        {
            // Create the template list of patches to add to the mod configuration ini.
            string patchList = "RequiredPatches=\"";

            // Loop through our list of valid patches.
            foreach (string patch in MiscPatches)
            {
                // Roll a number between 0 and 100, if it's smaller than or equal to Weight, then add the patch's name to the list to write to mod.ini.
                if (MainWindow.Randomiser.Next(0, 101) <= Weight)
                    patchList += $"{Path.GetFileName(patch)},";
            }

            // Add all the patches to the mod configuration ini.
            // Remove the last comma and replace it with a closing quote.
            patchList = patchList.Remove(patchList.LastIndexOf(','));
            patchList += "\"";

            // Write the list of required patches (as well as the [Patches] ini block) to the mod configuration ini.
            using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{ModDirectory}", "mod.ini")))
            {
                configInfo.WriteLine();
                configInfo.WriteLine("[Patches]");
                configInfo.WriteLine(patchList);

                configInfo.Close();
            }
        }

        /// <summary>
        /// Patches the first mission lua in Sonic's story to instantly unlock Shadow and Silver's episodes.
        /// </summary>
        /// <param name="archivePath">The path to the extracted scripts.arc.</param>
        /// <param name="GameExecutable">The filepath of the game executable (used to determine the path).</param>
        public static async Task UnlockEpisodes(string archivePath, string GameExecutable)
        {
            // Determine if we need a xenon folder or a ps3 folder.
            string corePath = "xenon";
            if (GameExecutable.ToLower().EndsWith(".bin"))
                corePath = "ps3";

            // Decompile the 0001 mission lua.
            await Task.Run(() => Helpers.LuaDecompile($@"{archivePath}\{corePath}\scripts\mission\0001\mission.lub"));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines($@"{archivePath}\{corePath}\scripts\mission\0001\mission.lub");

            // Loop through each line in this lua binary.
            for (int i = 0; i < lua.Length; i++)
            {
                // Search for the opening cutscene's ending event and set the global flags used after Silver's boss and after Crisis City.
                if (lua[i] == "  elseif _ARG_1_ == \"e0000_end\" then")
                {
                    lua[i] += "\n    SetGlobalFlag(_ARG_0_, 50, 1)\n    SetGlobalFlag(_ARG_0_, 51, 1)";
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines($@"{archivePath}\{corePath}\scripts\mission\0001\mission.lub", lua);
        }
    
        /// <summary>
        /// Edits attributes in Common.bin to change how physics props behave.
        /// </summary>
        /// <param name="archivePath">The path to the extracted object.arc.</param>
        /// <param name="psi">Whether or not the object's behaviour when grabbed by Silver should be randomised.</param>
        /// <param name="psiNoGrab">Whether or not to allow making a prop not grabbable.</param>
        /// <param name="psiNoDebris">Whether or not to skip randomising the PSI behaviour on a break object.</param>
        /// <param name="debris">Whether or not to randomise what break object spawns from a prop.</param>
        public static async Task PropAttributes(string archivePath, bool? psi, bool? psiNoGrab, bool? psiNoDebris, bool? debris)
        {
            // Set up a list of valid break objects.
            List<string> debrisTypes = new();

            // Load the Common Package.
            CommonPackage commonBIN = new($@"{archivePath}\xenon\object\Common.bin");

            // Build up a list of valid debris stuff
            foreach (CommonObject? entry in commonBIN.Objects)
                if (entry.BreakObject != "")
                    if (!debrisTypes.Contains(entry.BreakObject))
                        debrisTypes.Add(entry.BreakObject);

            // Loop through each object for the actual randomisation.
            foreach (CommonObject? entry in commonBIN.Objects)
            {
                // Randomise the PsiBehaviour if we need to.
                if (psi == true)
                {
                    // Skip this object if it's in the debris list and we're not randomising debris PsiBehaviour.
                    if (psiNoDebris == true && debrisTypes.Contains(entry.PropName))
                        continue;

                    // Generate a number for the PsiBehaviour depending on whether we're allowed to generate a 0 or not.
                    if (psiNoGrab == true)
                        entry.PsiBehaviour = (uint)MainWindow.Randomiser.Next(1, 3);
                    else
                        entry.PsiBehaviour = (uint)MainWindow.Randomiser.Next(0, 3);
                }

                // Pick a new break object if we're randomising debris and this object calls for one.
                if (debris == true && entry.BreakObject != "")
                    entry.BreakObject = debrisTypes[MainWindow.Randomiser.Next(debrisTypes.Count)];
            }

            // Save the updated Common.bin
            commonBIN.Save();
        }

        /// <summary>
        /// Generates a random episode setup.
        /// </summary>
        /// <param name="archivePath">The path to the extracted scripts.arc.</param>
        /// <param name="GameExecutable">The filepath of the game executable (used to determine the path).</param>
        public static async Task<Dictionary<string, int>> EpisodeGenerator(string archivePath, string GameExecutable)
        {
            Dictionary<string, int> LevelOrder = new();

            // List of events for the sake of picking one to play at the start of each level.
            List<string> events = new() { "e0001", "e0002", "e0003", "e0004", "e0006", "e0007", "e0009", "e0010", "e0011", "e0012", "e0013", "e0014", "e0015", "e0016", "e0017", "e0018", "e0019", "e0021", "e0022", "e0023", "e0024", "e0026", "e0027", "e0028", "e0029", "e0031", "e0102", "e0103", "e0104", "e0105", "e0106", "e0107", "e0108", "e0109", "e0110", "e0111", "e0112", "e0113", "e0114", "e0115", "e0116", "e0117", "e0118", "e0119", "e0120", "e0121", "e0122", "e0125", "e0126", "e0127", "e0128", "e0129", "e0201", "e0202", "e0203", "e0204", "e0205", "e0206", "e0207", "e0208", "e0209", "e0210", "e0211", "e0212", "e0213", "e0214", "e0215", "e0216", "e0217", "e0218", "e0219", "e0221", "e0222", "e0223", "e0224", "e0225", "e0226", "e0227", "e0300", "e0301", "e0302", "e0304", "e1001", "e1002", "e1011", "e1012", "e1031", "e1032", "e1041", "e1052", "e1061", "e1062", "e1071", "e1072", "e1081", "e1082", "e1091", "e1101", "e1111", "e1112", "e1121", "e1122", "e1123", "e1141", "e1151", "e1161", "e1171" };
            
            // Determine if we need a xenon folder or a ps3 folder.
            string corePath = "xenon";
            if (GameExecutable.ToLower().EndsWith(".bin"))
                corePath = "ps3";

            // Create the Randomised Episode scripts directory.
            Directory.CreateDirectory($@"{archivePath}\{corePath}\scripts\mission\rando");

            #region Copy Sonic's Luas
            // Wave Ocean
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2001.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wvo.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2002.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wvo.lub");

            // Egg Cerberus
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2011.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_eCerberus.lub");

            // Dusty Desert
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2021.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_dtd.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2022.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_dtd.lub");

            // Silver
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2031.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_silver.lub");

            // White Acropolis
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2041.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wap.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2042.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wap.lub");

            // Crisis City
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2051.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_csc.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2052.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_csc.lub");

            // Flame Core
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2061.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_flc.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2062.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_flc.lub");

            // Iblis Phase 2
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2071.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_iblis02.lub");

            // Radical Train
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2081.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_rct.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2082.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_rct.lub");

            // Egg Genesis
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_2091.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_eGenesis.lub");

            // Tropical Jungle
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20A1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_tpj.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20A2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_tpj.lub");

            // Wave Ocean (Tails)
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20B1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wvoT.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20B2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wvoT.lub");

            // Kingdom Valley
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20C1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_kdv.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20C2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_kdv.lub");

            // Aquatic Base
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20D1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_aqa.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20D2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_aqa.lub");

            // Egg Wyvern
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2000\mission_20E1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_eWyvern.lub");
            #endregion

            #region Copy Shadow's Luas
            // White Acropolis
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2101.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_wap.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2102.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_wap.lub");

            // Egg Cerberus
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2111.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_eCerberus.lub");

            // Kingdom Valley
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2121.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_kdv.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2122.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_kdv.lub");

            // Crisis City
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2131.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_csc.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2132.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_csc.lub");

            // Flame Core
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2141.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_flc.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2142.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_flc.lub");

            // Iblis Phase 2
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2151.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_iblis02.lub");

            // Tropical Jungle
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2161.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_tpj.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2162.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_tpj.lub");

            // Mephiles Phase 1
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2171.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_mephiles01.lub");

            // Radical Train
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2181.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_rct.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2182.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_rct.lub");

            // Silver
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_2191.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_silver.lub");

            // Aquatic Base
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21A1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_aqa.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21A2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_aqa.lub");

            // Wave Ocean
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21B1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_wvo.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21B2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_wvo.lub");

            // Dusty Desrt
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21C1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_dtd.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21C2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_dtd.lub");

            // Mephiles Phase 2
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2100\mission_21D1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_mephiles02.lub");
            #endregion

            #region Copy Silver's Luas
            // Crisis City
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2201.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_csc.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2202.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_csc.lub");

            // Iblis Phase 1
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2211.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_iblis01.lub");

            // Tropical Jungle
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2221.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_tpj.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2222.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_tpj.lub");

            // Wave Ocean
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2231.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_wvo.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2232.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_wvo.lub");

            // Dusty Desert
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2241.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_dtd.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2242.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_dtd.lub");

            // Sonic
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2251.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_sonic.lub");

            // White Acropolis
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2261.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_wap.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2262.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_wap.lub");

            // Egg Genesis
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2271.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_eGenesis.lub");

            // Radical Train
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2281.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_rct.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2282.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_rct.lub");

            // Shadow
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_2291.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_shadow.lub");

            // Aquatic Base
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22A1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_aqa.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22A2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_aqa.lub");

            // Kingdom Valley
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22B1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_kdv.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22B2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_kdv.lub");

            // Flame Core
            if (MainWindow.Randomiser.Next(0, 2) == 0)
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22C1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_flc.lub");
            else
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22C2.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_flc.lub");

            // Iblis Phase 3
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2200\mission_22D1.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_iblis03.lub");
            #endregion

            #region Copy End of the World Luas
            // End of the World
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2300\mission_2301.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_eotw.lub");

            // Solaris
            File.Copy($@"{archivePath}\{corePath}\scripts\mission\2300\mission_2311.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_solaris.lub");
            #endregion

            #region Copy Team Attack Amigo (if it exists)
            if (File.Exists($@"{archivePath}\{corePath}\download\0030.lub"))
                File.Copy($@"{archivePath}\{corePath}\scripts\mission\4545\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_taa.lub");
            #endregion

            /* Disable the Very Hard support as it breaks the other sets by replacing their area luas (and Sonic's fucks game.lub, screwing over Team Attack Amigo and vice versa)
             * If I'm to readd this, the DLC arcs will need to be handled seperately.
            #region 1/3 chance to swap in Sonic's Very Hard Luas (if they exist)

            if (File.Exists($@"{archivePath}\{corePath}\download\0003.lub"))
            {
                // Wave Ocean
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4002\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wvo.lub", true);

                // Dusty Desert
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4007\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_dtd.lub", true);

                // White Acropolis
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4012\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_wap.lub", true);

                // Crisis City
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4017\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_csc.lub", true);

                // Flame Core
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4022\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_flc.lub", true);

                // Radical Train
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4027\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_rct.lub", true);

                // Tropical Jungle
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4032\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_tpj.lub", true);

                // Kingdom Valley
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4037\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_kdv.lub", true);

                // Aquatic Base
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4042\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_sonic_aqa.lub", true);
            }
            #endregion

            #region 1/3 chance to swap in Shadow's Very Hard Luas (if they exist)

            if (File.Exists($@"{archivePath}\{corePath}\download\0004.lub"))
            {
                // White Acropolis
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4053\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_wap.lub", true);

                // Kingdom Valley
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4058\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_kdv.lub", true);

                // Crisis City
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4063\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_csc.lub", true);

                // Flame Core
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4068\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_flc.lub", true);

                // Radical Train
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4073\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_rct.lub", true);

                // Aquatic Base
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4078\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_aqa.lub", true);

                // Wave Ocean
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4083\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_wvo.lub", true);

                // Dusty Desert
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4088\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_shadow_dtd.lub", true);
            }
            #endregion

            #region 1/3 chance to swap in Silver's Very Hard Luas (if they exist)

            if (File.Exists($@"{archivePath}\{corePath}\download\0005.lub"))
            {
                // Crisis City
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4099\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_csc.lub", true);

                // Tropical Jungle
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4104\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_tpj.lub", true);

                // Dusty Desert
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4109\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_dtd.lub", true);

                // White Acropolis
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4114\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_wap.lub", true);

                // Radical Train
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4119\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_rct.lub", true);

                // Aquatic Base
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4124\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_aqa.lub", true);

                // Kingdom Valley
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4129\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_kdv.lub", true);

                // Flame Core
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    File.Copy($@"{archivePath}\{corePath}\scripts\mission\4134\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_silver_flc.lub", true);
            }
            #endregion
            */

            // Get a list of all the mission luas we have.
            List<string> luas = Directory.GetFiles($@"{archivePath}\{corePath}\scripts\mission\rando\", "*.lub").ToList();

            // Determine which mission lua to start on.
            string startLua = luas[MainWindow.Randomiser.Next(luas.Count)];
            LevelOrder.Add(Path.GetFileNameWithoutExtension(startLua), 1);

            // Remove the startLua from the list.
            luas.Remove(startLua);

            // Rename the start lua.
            File.Move(startLua, $@"{archivePath}\{corePath}\scripts\mission\rando\mission_start.lub");

            // Update the variable for the new name.
            startLua = $@"{archivePath}\{corePath}\scripts\mission\rando\mission_start.lub";

            // Create lastLua with the same value.
            string lastLua = startLua;
            int levelCount = 1;

            // Loop through and add every mission to the "episode".
            while (luas.Count > 0)
            {
                if (levelCount != 1)
                    LevelOrder.Add(Path.GetFileNameWithoutExtension(lastLua), levelCount);

                levelCount++;

                // Decompile the Lua file we need to add a mission redirect to.
                await Task.Run(() => Helpers.LuaDecompile(lastLua));

                // Load the decompiled lua into a string array for editing.
                string[] lua = File.ReadAllLines(lastLua);

                // Pick what lua will be the next one and remove it from the list.
                string nextLua = luas[MainWindow.Randomiser.Next(luas.Count)];
                luas.Remove(nextLua);

                for (int i = 0; i < lua.Length; i++)
                {
                    // Add a next mission redirect to lastLua, pointing to nextLua
                    if (lua[i].Contains("MissionClear("))
                        lua[i] = $"    SetNextMission(a1, \"scripts/mission/rando/{Path.GetFileNameWithoutExtension(nextLua)}.lua\")\r\n{lua[i]}";

                    // Select a random cutscene to start the mission with.
                    if (lua[i].Contains("mission_event_start = "))
                    {
                        // Determine if we need to add a comma.
                        bool hasComma = lua[i].Contains(',');

                        // Select a random event.
                        lua[i] = $"  mission_event_start = \"{events[MainWindow.Randomiser.Next(events.Count)]}\"";

                        // Add the comma if we need to.
                        if (hasComma)
                            lua[i] += ",";
                    }
                }

                // Save the updated lua binary.
                File.WriteAllLines(lastLua, lua);

                // Set the lastLua to the nextLua.
                lastLua = nextLua;
            }
            LevelOrder.Add(Path.GetFileNameWithoutExtension(lastLua), levelCount);

            // Make startLua unlock all the player upgrades.
            await Task.Run(() => Helpers.LuaDecompile(startLua));

            // Load the decompiled lua into a string array for editing.
            string[] mission = File.ReadAllLines(startLua);

            // Find the main function and strap all the global flag toggles to it.
            for (int i = 0; i < mission.Length; i++)
            {
                if (mission[i].StartsWith("function main("))
                {
                    // Unlock Sonic's items (including the gems).
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6000, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6001, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6002, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6004, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6005, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6006, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6007, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6008, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6009, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6010, 1)";

                    // Unlock Shadow's items.
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6012, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6013, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6014, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6015, 1)";

                    // Unlock Silver's items.
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6016, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6017, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6018, 1)";
                    mission[i] += $"\r\n  SetGlobalFlag(a1, 6019, 1)";
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(startLua, mission);

            // Create the DLC directory.
            Directory.CreateDirectory($@"{archivePath}\{corePath}\download");

            // Copy the DLC entry.
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\DLC_RandomEpisode.lub", $@"{archivePath}\{corePath}\download\DLC_RandomEpisode.lub", true);

            // Read the DLC entry.
            string dlc = File.ReadAllText($@"{archivePath}\{corePath}\download\DLC_RandomEpisode.lub");

            // Generate a list of select luas we can choose from.
            List<string> selectLuas = new() { "select_amy", "select_blaze", "select_knuckles", "select_omega", "select_rouge", "select_shadow", "select_silver", "select_sonic", "select_tails" };

            // Replace the placeholder with a random select lua.
            dlc = dlc.Replace("character_placeholder", selectLuas[MainWindow.Randomiser.Next(selectLuas.Count)]);

            // Resave the DLC entry.
            File.WriteAllText($@"{archivePath}\{corePath}\download\DLC_RandomEpisode.lub", dlc);

            // Set all the stage messages.
            foreach (var entry in LevelOrder)
            {
                // Read the filename key seperately so we don't dupe code.
                string fileName = entry.Key;

                // First entry doesn't have the same name as its lua, handle that.
                if (entry.Value == 1)
                    fileName = "mission_start";

                // Decompile the Lua file (needed so the last one doesn't brick itself)
                await Task.Run(() => Helpers.LuaDecompile($@"{archivePath}\{corePath}\scripts\mission\rando\{fileName}.lub"));

                // Read the lua.
                string[] lua = File.ReadAllLines($@"{archivePath}\{corePath}\scripts\mission\rando\{fileName}.lub");

                for (int i = 0; i < lua.Length; i++)
                {
                    // Set the mission string.
                    if (lua[i].Contains("mission_string = "))
                    {
                        // Determine if we need to add a comma.
                        bool hasComma = lua[i].Contains(',');

                        // Select the approriate mission string.
                        lua[i] = $"  mission_string = \"{entry.Key.Replace("mission", "msg")}\"";

                        // Add the comma if we need to.
                        if (hasComma)
                            lua[i] += ",";
                    }

                    // Set the mission text to our MST.
                    if (lua[i].Contains("mission_text = "))
                    {
                        // Determine if we need to add a comma.
                        bool hasComma = lua[i].Contains(',');

                        lua[i] = $"  mission_text = \"text/msg_randomiser.mst\"";

                        // Add the comma if we need to.
                        if (hasComma)
                            lua[i] += ",";
                    }
                }

                // Resave the updated Lua.
                File.WriteAllLines($@"{archivePath}\{corePath}\scripts\mission\rando\{fileName}.lub", lua);
            }

            return LevelOrder;
        }

        /// <summary>
        /// Generate the Message Table for our random episode.
        /// </summary>
        /// <param name="archivePath">The path to the extracted text.arc.</param>
        /// <param name="GameExecutable">The filepath of the game executable (used to determine the path).</param>
        public static async Task RandomEpisodeMST(string archivePath, string GameExecutable, Dictionary<string, int> LevelOrder)
        {
            // Determine if we need a xenon folder or a ps3 folder.
            string corePath = "xenon";
            if (GameExecutable.ToLower().EndsWith(".bin"))
                corePath = "ps3";

            // Create the MST for the random episode.
            MessageTable mst = new();
            mst.Data.Name = "msg_randomiser";

            // Set up a dummy message.
            Message msg;

            // Add the episode name.
            msg = new()
            {
                Name = "episode_name",
                Text = "GENERATED EPISODE"
            };
            mst.Data.Messages.Add(msg);

            // Write a message for each stage's number to allow the player to keep track of how much of the game is left.
            foreach (var entry in LevelOrder)
            {
                msg = new()
                {
                    Name = $"{entry.Key.Replace("mission", "msg")}",
                    Text = $"Stage {entry.Value}/{LevelOrder.Count}"
                };
                mst.Data.Messages.Add(msg);
            }

            // Save our new MST.
            mst.Save($@"{archivePath}\{corePath}\text\english\msg_randomiser.e.mst");
        }
    }
}
