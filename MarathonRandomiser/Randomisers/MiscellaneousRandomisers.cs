using Marathon.Formats.Archive;
using Marathon.Formats.Mesh;
using Marathon.Formats.Package;
using Marathon.Formats.Script.Lua.Decompiler.Blocks;
using Marathon.Formats.Text;
using Marathon.Helpers;
using Marathon.IO;
using System;
using System.Linq;

namespace MarathonRandomiser
{
    internal class MiscellaneousRandomisers
    {
        /// <summary>
        /// Randomises the amount of health enemies have, controlled by ScriptParameter.bin.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked enemy.arc containing ScriptParameter.bin</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="minHealth">The minimum value the enemy health can be set to.</param>
        /// <param name="maxHealth">The maximum value the enemy health can be set to.</param>
        /// <param name="includeBosses">Whether or not bosses should also get their health values randomised.</param>
        public static async Task EnemyHealthRandomiser(string archivePath, string corePath, int minHealth, int maxHealth, bool? includeBosses)
        {
            // Load ScriptParameter.bin
            ScriptPackage scriptPackage = new($@"{archivePath}\{corePath}\enemy\ScriptParameter.bin");

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
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        public static async Task UnlockEpisodes(string archivePath, string corePath)
        {
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
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="psi">Whether or not the object's behaviour when grabbed by Silver should be randomised.</param>
        /// <param name="psiNoGrab">Whether or not to allow making a prop not grabbable.</param>
        /// <param name="psiNoDebris">Whether or not to skip randomising the PSI behaviour on a break object.</param>
        /// <param name="debris">Whether or not to randomise what break object spawns from a prop.</param>
        public static async Task PropAttributes(string archivePath, string corePath, bool? psi, bool? psiNoGrab, bool? psiNoDebris, bool? debris)
        {
            // Set up a list of valid break objects.
            List<string> debrisTypes = new();

            // Load the Common Package.
            CommonPackage commonBIN = new($@"{archivePath}\{corePath}\object\Common.bin");

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
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="sonicVH">The path to Sonic's Very Hard Mode Arc.</param>
        /// <param name="shadowVH">The path to Shadow's Very Hard Mode Arc.</param>
        /// <param name="silverVH">The path to Silver's Very Hard Mode Arc.</param>
        /// <param name="townMissions">Whether or not to use Town Missions in the Generated Episode.</param>
        /// <param name="townMissionCount">How many Town Missions should be included in the Generated Episode.</param>
        public static async Task<Dictionary<string, int>> EpisodeGenerator(string archivePath, string corePath, string? sonicVH, string? shadowVH, string? silverVH, bool? townMissions, int townMissionCount)
        {
            Dictionary<string, int> LevelOrder = new();

            // List of events for the sake of picking one to play at the start of each level.
            List<string> events = new() { "e0001", "e0002", "e0003", "e0004", "e0006", "e0007", "e0009", "e0010", "e0011", "e0012", "e0013", "e0014", "e0015", "e0016", "e0017", "e0018", "e0019", "e0021", "e0022", "e0023", "e0024", "e0026", "e0027", "e0028", "e0029", "e0031", "e0102", "e0103", "e0104", "e0105", "e0106", "e0107", "e0108", "e0109", "e0110", "e0111", "e0112", "e0113", "e0114", "e0115", "e0116", "e0117", "e0118", "e0119", "e0120", "e0121", "e0122", "e0125", "e0126", "e0127", "e0128", "e0129", "e0201", "e0202", "e0203", "e0204", "e0205", "e0206", "e0207", "e0208", "e0209", "e0210", "e0211", "e0212", "e0213", "e0214", "e0215", "e0216", "e0217", "e0218", "e0219", "e0221", "e0222", "e0223", "e0224", "e0225", "e0226", "e0227", "e0300", "e0301", "e0302", "e0304", "e1001", "e1002", "e1011", "e1012", "e1031", "e1032", "e1041", "e1052", "e1061", "e1062", "e1071", "e1072", "e1081", "e1082", "e1091", "e1101", "e1111", "e1112", "e1121", "e1122", "e1123", "e1141", "e1151", "e1161", "e1171" };
            
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

            #region Town Missions
            // Shuffle a list of all the used town missions.
            List<string> TownMissions = await Task.Run(() => Helpers.ShuffleList(new() { "1001", "1003", "1004", "1005", "1008", "1010", "1011", "1012", "1013", "1014", "1018", "1019", "1024", "1025", "1027", "1029", "1030", "1031", "1032", "1033", "1103", "1104", "1107", "1108", "1109", "1112", "1114", "1117", "1118", "1119", "1126", "1128", "1130", "1131", "1132", "1201", "1203", "1208", "1211", "1212", "1214", "1215", "1216", "1218", "1219", "1220", "1221", "1226", "1232", "1237", "1238", "1239", "1240" }));
            
            // Trim the list down to the specified count.
            while (TownMissions.Count > townMissionCount)
                TownMissions.RemoveAt(TownMissions.Count - 1);

            // Copy the mission luas.
            foreach (var TownMission in TownMissions)
            {
                // Determine which character this mission is for so we can name the mission lua.
                string hedgehog = "sonic";
                if (TownMission.StartsWith("11"))
                    hedgehog = "shadow";
                if (TownMission.StartsWith("12"))
                    hedgehog = "silver";

                File.Copy($@"{archivePath}\{corePath}\scripts\mission\{TownMission}\mission.lub", $@"{archivePath}\{corePath}\scripts\mission\rando\mission_{hedgehog}_town{TownMission}.lub");
            }
            #endregion

            #region DLC
            // Load DLC archives if needed.
            if (sonicVH != null)
            {
                U8Archive sonicVeryHard = new(sonicVH, ReadMode.CopyToMemory);
                sonicVeryHard.Root.GetFile($"{corePath}/scripts/game.lub").Extract($@"{archivePath}\{corePath}\scripts\game.lub"); // Sonic's very hard modifies game.lub to include tpjC for him.
                await Task.Run(() => Helpers.RendererExtract(sonicVeryHard, archivePath));

                // 1/3 chance to swap in Sonic's Very Hard Mode files.
                // Wave Ocean
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "wvo", archivePath, "4002", "sonic"));

                // Dusty Desert
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "dtd", archivePath, "4007", "sonic"));

                // White Acropolis
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "wap", archivePath, "4012", "sonic"));

                // Crisis City
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "csc", archivePath, "4017", "sonic"));

                // Flame Core
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "flc", archivePath, "4022", "sonic"));

                // Radical Train
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "rct", archivePath, "4027", "sonic"));

                // Tropical Jungle
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "tpj", archivePath, "4032", "sonic"));

                // Kingdom Valley
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "kdv", archivePath, "4037", "sonic"));

                // Aquatic Base
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, sonicVeryHard, "aqa", archivePath, "4042", "sonic"));
            }
            if (shadowVH != null)
            {
                U8Archive shadowVeryHard = new(shadowVH, ReadMode.CopyToMemory);
                await Task.Run(() => Helpers.RendererExtract(shadowVeryHard, archivePath));

                // 1/3 chance to swap in Shadow's Very Hard Mode files.
                // White Acropolis
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "wap", archivePath, "4053", "shadow"));

                // Kingdom Valley
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "kdv", archivePath, "4058", "shadow"));

                // Crisis City
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "csc", archivePath, "4063", "shadow"));

                // Flame Core
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "flc", archivePath, "4068", "shadow"));

                // Radical Train
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "rct", archivePath, "4073", "shadow"));

                // Aquatic Base
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "aqa", archivePath, "4078", "shadow"));

                // Wave Ocean
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "wvo", archivePath, "4083", "shadow"));

                // Dusty Desert
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, shadowVeryHard, "dtd", archivePath, "4088", "shadow"));
            }
            if (silverVH != null)
            {
                U8Archive silverVeryHard = new(silverVH, ReadMode.CopyToMemory);
                await Task.Run(() => Helpers.RendererExtract(silverVeryHard, archivePath));

                // 1/3 chance to swap in Silver's Very Hard Mode files.
                // Crisis City
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "csc", archivePath, "4099", "silver"));

                // Tropical Jungle
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "tpj", archivePath, "4104", "silver"));

                // Dusty Desert
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "dtd", archivePath, "4109", "silver"));

                // White Acropolis
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "wap", archivePath, "4114", "silver"));

                // Radical Train
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "rct", archivePath, "4119", "silver"));

                // Aquatic Base
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "aqa", archivePath, "4124", "silver"));

                // Kingdom Valley
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "kdv", archivePath, "4129", "silver"));

                // Flame Core
                if (MainWindow.Randomiser.Next(0, 3) == 0)
                    await Task.Run(() => Helpers.VeryHardModeExtractor(corePath, silverVeryHard, "flc", archivePath, "4134", "silver"));
            }
            #endregion

            // Get a list of all the mission luas we have.
            List<string> luas = Directory.GetFiles($@"{archivePath}\{corePath}\scripts\mission\rando\", "*.lub").ToList();

            // Determine which mission lua to start on.
            string startLua = luas[MainWindow.Randomiser.Next(luas.Count)];
            LevelOrder.Add(Path.GetFileNameWithoutExtension(startLua), 1);

            // Remove the startLua from the list.
            luas.Remove(startLua);

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
                    // Add a next mission redirect to lastLua.
                    if (lua[i].Contains("MissionClear("))
                    {
                        // If this is a failed mission, point to itself as a hack to restart the mission rather than booting the player out to the menu.
                        if (lua[i].Contains("failed"))
                            lua[i] = $"    SetNextMission(a1, \"scripts/mission/rando/{Path.GetFileNameWithoutExtension(lastLua)}.lua\")\r\n{lua[i]}";

                        // If it's a regular completion, then point to the nextLua instead so we can advance to the next stage.
                        else
                            lua[i] = $"    SetNextMission(a1, \"scripts/mission/rando/{Path.GetFileNameWithoutExtension(nextLua)}.lua\")\r\n{lua[i]}";
                    }

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
            foreach (KeyValuePair<string, int> entry in LevelOrder)
            {
                // Read the filename key seperately so we don't dupe code.
                string fileName = entry.Key;

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

                    // Set the mission text to our MST if it's not a Town Mission one.
                    if (lua[i].Contains("mission_text = ") && !fileName.Contains("_town"))
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
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="LevelOrder">Used to determine what stage is where in order.</param>
        public static async Task RandomEpisodeMST(string archivePath, string corePath, Dictionary<string, int> LevelOrder)
        {
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

            // Add a description for the stage select.
            msg = new()
            {
                Name = "stage_select",
                Text = "Select a Stage!"
            };
            mst.Data.Messages.Add(msg);

            // Write a message for each stage's number to allow the player to keep track of how much of the game is left.
            foreach (KeyValuePair<string, int> entry in LevelOrder)
            {
                msg = new()
                {
                    Name = $"{entry.Key.Replace("mission", "msg")}",
                    Text = $"Stage {entry.Value}/{LevelOrder.Count}"
                };

                // If this is a stage entry, then add it to our custom MST, if not, add it to the approriate Town Mission one.
                if (!entry.Key.Contains("_town"))
                {
                    // Replace stage with boss just to make it obvious on loading that it's a... Well... Boss.
                    if (entry.Key.Contains("eCerberus") || entry.Key.Contains("eGenesis") || entry.Key.Contains("eWyvern") ||
                        entry.Key.Contains("iblis01") || entry.Key.Contains("iblis02") || entry.Key.Contains("iblis03") ||
                        entry.Key.Contains("mephiles01") || entry.Key.Contains("mephiles02") || entry.Key.Contains("solaris") ||
                        entry.Key.EndsWith("_sonic") || entry.Key.EndsWith("_shadow") || entry.Key.EndsWith("_silver"))
                            msg.Text = msg.Text.Replace("Stage", "Boss");

                    mst.Data.Messages.Add(msg);
                }
                else
                {
                    // Replace stage with mission just to make it obvious on loading that it's a Town Mission.
                    msg.Text = msg.Text.Replace("Stage", "Mission");

                    // Load the Town Mission MST based on the hedgehog name.
                    MessageTable missionMST = new($@"{archivePath}\{corePath}\text\english\msg_town_mission_{((string[]?)entry.Key.Split('_'))[1]}.e.mst");
                    
                    // Add our message to it.
                    missionMST.Data.Messages.Add(msg);

                    // Resave it.
                    missionMST.Save();
                }
            }

            // Save our new MST.
            mst.Save($@"{archivePath}\{corePath}\text\english\msg_randomiser.e.mst");
        }

        /// <summary>
        /// Generate the Message Table for our random episode's shop stage select.
        /// </summary>
        /// <param name="archivePath">The path to the extracted text.arc.</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="LevelOrder">Used to determine what stage is where in order.</param>
        public static async Task RandomEpisodeShopMST(string archivePath, string corePath, Dictionary<string, int> LevelOrder)
        {
            // Load the shop MST.
            MessageTable mst = new($@"{archivePath}\{corePath}\text\english\msg_shop.e.mst");

            // Set up a dummy message.
            Message msg;

            // Create the actual shop listing.
            foreach (KeyValuePair<string, int> entry in LevelOrder)
            {
                msg = new()
                {
                    Name = $"msg_shop_24{entry.Value.ToString().PadLeft(2, '0')}",
                    Text = $"Stage {entry.Value}"
                };
                mst.Data.Messages.Add(msg);
            }

            // Create the stage descriptions.
            foreach (KeyValuePair<string, int> entry in LevelOrder)
            {
                // Handle End of the World and Solaris differently.
                if (entry.Key != "mission_eotw" && entry.Key != "mission_solaris")
                {
                    // Split the entry into three.
                    string[] split = entry.Key.Split('_');

                    // Properly capitalise the Hedgehog name.
                    split[1] = Helpers.FirstLetterToUpper(split[1]);

                    // Swap out the stage shorthand for its proper name.
                    // Also swap out the character name for the few stages that don't involve the main hedgehog.
                    switch (split[2])
                    {
                        // Stages.
                        case "aqa": split[2] = "Aquatic Base"; break;
                        case "csc": split[2] = "Crisis City"; break;
                        case "dtd": split[2] = "Dusty Desert"; break;
                        case "flc": split[2] = "Flame Core"; break;
                        case "kdv": split[2] = "Kingdom Valley"; break;
                        case "rct": split[2] = "Radical Train"; break;
                        case "tpj": split[2] = "Tropical Jungle"; if (split[1] == "Shadow") split[1] = "Rouge"; break;
                        case "wap": split[2] = "White Acropolis"; break;
                        case "wvo": split[2] = "Wave Ocean"; if (split[1] == "Silver") split[1] = "Blaze"; break;
                        case "wvoT": split[2] = "Wave Ocean"; split[1] = "Tails"; break;

                        // Eggman Bosses.
                        case "eCerberus": split[2] = "Egg Cerberus"; break;
                        case "eGenesis": split[2] = "Egg Genesis"; break;
                        case "eWyvern": split[2] = "Egg Wyvern"; break;

                        // Iblis Bosses
                        case "iblis01": split[2] = "Iblis Phase 1"; break;
                        case "iblis02": split[2] = "Iblis Phase 2"; break;
                        case "iblis03": split[2] = "Iblis Phase 3"; break;

                        // Mephiles Bosses
                        case "mephiles01": split[2] = "Mephiles Phase 1"; break;
                        case "mephiles02": split[2] = "Mephiles Phase 2"; break;

                        // Character Battles
                        case "silver": split[2] = "Silver The Hedgehog"; break;
                        case "sonic": split[2] = "Sonic The Hedgehog"; break;
                        case "shadow": split[2] = "Shadow The Hedgehog"; break;

                        // Sonic's Town Missions.
                        case "town1001": split[2] = "The winder of a Shoemaker"; break;
                        case "town1003": split[2] = "Shadows of\nEggman's Mechs"; break;
                        case "town1004": split[2] = "Find Pele,\nthe Beloved Dog"; break;
                        case "town1005": split[2] = "The Soleanna Boys'\nChallenge"; break;
                        case "town1008": split[2] = "Mels, the Soleanna\nRunning Legend"; break;
                        case "town1010": split[2] = "The 100 Forgotten Rings"; break;
                        case "town1011": split[2] = "Who is the Captain?"; break;
                        case "town1012": split[2] = "Chase the Fleeing Car!"; break;
                        case "town1013": split[2] = "VS Sonic Man"; break;
                        case "town1014": split[2] = "Aristo's Challenge"; break;
                        case "town1018": split[2] = "The Hotel's Festival\nof Rings"; break;
                        case "town1019": split[2] = "Acrobatic Circus Scout"; break;
                        case "town1024": split[2] = "Destroy all Enemies\nin the New City!"; break;
                        case "town1025": split[2] = "Battle at the Warehouse!"; break;
                        case "town1027": split[2] = "Open the Cave Gate!"; break;
                        case "town1029": split[2] = "Take the Lady-in-Waiting\nto Town!"; break;
                        case "town1030": split[2] = "The Test of Intelligence"; break;
                        case "town1031": split[2] = "The Test of Love"; break;
                        case "town1032": split[2] = "The Test of Courage"; break;
                        case "town1033": split[2] = "The Legend of\nthe Three Musketeers"; break;

                        // Shadow's Town Missions.
                        case "town1103": split[2] = "Request from the\nRimlight Employee"; break;
                        case "town1104": split[2] = "Defeat the Enemies\non the Shopping Street"; break;
                        case "town1107": split[2] = "Motorcycle License Test"; break;
                        case "town1108": split[2] = "Protect the Professor's Car"; break;
                        case "town1109": split[2] = "Car Festival"; break;
                        case "town1112": split[2] = "Agent Test: Intelligence"; break;
                        case "town1114": split[2] = "Lord Regis' Daughter,\nSabrina"; break;
                        case "town1117": split[2] = "Agent Test: Strength"; break;
                        case "town1118": split[2] = "The Mathematician's Training"; break;
                        case "town1119": split[2] = "Stolen Rimlight Information"; break;
                        case "town1126": split[2] = "Buggy Ring Race"; break;
                        case "town1128": split[2] = "The Mystery of the Ghost"; break;
                        case "town1130": split[2] = "Emergency Order:\nCapture the Thieves' Cars!"; break;
                        case "town1131": split[2] = "Destroy the Enemies\ninvading from the Desert!"; break;
                        case "town1132": split[2] = "Save the Archaeologist!"; break;

                        // Silver's Town Missions.
                        case "town1201": split[2] = "Soleanna's Apple Festival"; break;
                        case "town1203": split[2] = "The Training Grounds"; break;
                        case "town1208": split[2] = "Protect Anna\nShe Knows the Secret!"; break;
                        case "town1211": split[2] = "The Stolen Bronze Medals"; break;
                        case "town1212": split[2] = "The Airplane Tournament"; break;
                        case "town1214": split[2] = "Protect the Coastline!"; break;
                        case "town1215": split[2] = "Sonic Man Returns!"; break;
                        case "town1216": split[2] = "The Soleanna\nWater Target Tournament!"; break;
                        case "town1218": split[2] = "Catch the Soleanna Boys!"; break;
                        case "town1219": split[2] = "Raid: Evil Monster"; break;
                        case "town1220": split[2] = "Defeat the Pursuers\nand Escort Elise!"; split[1] = "Amy"; break;
                        case "town1221": split[2] = "Protect the Gate to\nthe Castle Town"; break;
                        case "town1226": split[2] = "Lost Company Property"; break;
                        case "town1232": split[2] = "The Test of Memory"; break;
                        case "town1237": split[2] = "The Test of Friendship"; break;
                        case "town1238": split[2] = "The Test of Heart"; break;
                        case "town1239": split[2] = "Protect the Barrels\nin the Warehouse District!"; break;
                        case "town1240": split[2] = "Get into the station!"; break;

                        default: System.Diagnostics.Debug.WriteLine($"Unhandled stage shorthand '{split[2]}'"); break;
                    }

                    // Write a message for the shop menu.
                    msg = new()
                    {
                        Name = $"msg_shop_25{entry.Value.ToString().PadLeft(2, '0')}",
                        Text = $"{split[2]}\n---\n{split[1]}"
                    };
                    mst.Data.Messages.Add(msg);
                }
                
                // End of the World and Solaris don't have a character tag, so handle them differently.
                else if (entry.Key == "mission_eotw")
                {
                    msg = new()
                    {
                        Name = $"msg_shop_25{entry.Value.ToString().PadLeft(2, '0')}",
                        Text = $"End of the World"
                    };
                    mst.Data.Messages.Add(msg);
                }
                else if (entry.Key == "mission_solaris")
                {
                    msg = new()
                    {
                        Name = $"msg_shop_25{entry.Value.ToString().PadLeft(2, '0')}",
                        Text = $"Solaris"
                    };
                    mst.Data.Messages.Add(msg);
                }
            }

            // Save the edited shop MST.
            mst.Save();
        }

        /// <summary>
        /// Generates the mission lua for the Soleanna Stage Select for the Random Episode.
        /// </summary>
        /// <param name="archivePath">The path to the extracted scripts.arc.</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="LevelOrder">Used to determine what stage is where in order.</param>
        public static async Task GenerateRandomEpisodeTown(string archivePath, string corePath, Dictionary<string, int> LevelOrder)
        {
            // Copy the HUB set.
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\set_rando_hub.set", $@"{archivePath}\{corePath}\scripts\mission\rando\set_rando_hub.set", true);

            // Write the mission lua.
            using (Stream luaCreate = File.Open($@"{archivePath}\{corePath}\scripts\mission\rando\mission_start.lub", FileMode.Create))
            using (StreamWriter luaInfo = new(luaCreate))
            {
                // Mission Header.
                luaInfo.WriteLine("g_mission_information = {");
                luaInfo.WriteLine("  mission_string = \"stage_select\",");
                luaInfo.WriteLine("  mission_area = \"twn/sonic/a\",");
                luaInfo.WriteLine("  mission_terrain = \"stage/twn/a/\",");
                luaInfo.WriteLine("  mission_set_default = \"scripts/mission/rando/set_rando_hub.XML\",");
                luaInfo.WriteLine("  mission_event_start = \"\",");
                luaInfo.WriteLine("  mission_event_end = \"\",");
                luaInfo.WriteLine("  mission_text = \"text/msg_randomiser.mst\",");
                luaInfo.WriteLine("  mission_is_battle = true");
                luaInfo.WriteLine("}");

                // Shop Header.
                luaInfo.WriteLine("g_shop = {");

                // Shop Basics.
                luaInfo.WriteLine("  {");
                luaInfo.WriteLine("    message_first = \"msg_shop_001\",");
                luaInfo.WriteLine("    message_agree = \"msg_shop_005\",");
                luaInfo.WriteLine("    message_buy_item = \"msg_shop_006\",");
                luaInfo.WriteLine("    message_cancel_item = \"msg_shop_007\",");
                luaInfo.WriteLine("    message_second = \"msg_shop_007\",");
                luaInfo.WriteLine("    message_no_money = \"msg_shop_008\",");
                luaInfo.WriteLine("    message_soldout = \"msg_shop_011\",");
                luaInfo.WriteLine("    message_end = \"msg_shop_012\"");
                luaInfo.WriteLine("  },");

                // Shop Entries.
                KeyValuePair<string, int> lastLevel = LevelOrder.Last();
                foreach (KeyValuePair<string, int> entry in LevelOrder)
                {
                    luaInfo.WriteLine("  {");
                    luaInfo.WriteLine($"    name = \"msg_shop_24{entry.Value.ToString().PadLeft(2, '0')}\",");
                    luaInfo.WriteLine($"    price = 0,");
                    luaInfo.WriteLine($"    explain = \"msg_shop_25{entry.Value.ToString().PadLeft(2, '0')}\",");
                    luaInfo.WriteLine($"    event = \"shop_buy_24{entry.Value.ToString().PadLeft(2, '0')}\"");

                    if (entry.Key != lastLevel.Key)
                        luaInfo.WriteLine("  },");
                    else
                        luaInfo.WriteLine("  }");
                }

                // Shop Close.
                luaInfo.WriteLine("}");

                // Town stuff needed to make the game not die.
                luaInfo.WriteLine("g_message_setuped = \"\"");
                luaInfo.WriteLine("g_message_icon = 0");
                luaInfo.WriteLine("g_name_setuped = \"\"");

                // Main Function.
                luaInfo.WriteLine("function main(a1)");

                // Unlock Sonic's Items.
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6000, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6001, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6002, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6004, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6005, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6006, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6007, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6008, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6009, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6010, 1)");

                // Unlock Shadow's Items.
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6012, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6013, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6014, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6015, 1)");

                // Unlock Silver's Items.
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6016, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6017, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6018, 1)");
                luaInfo.WriteLine("  SetGlobalFlag(a1, 6019, 1)");

                // Main Function Close.
                luaInfo.WriteLine("end");

                // on_hint function.
                luaInfo.WriteLine("function on_hint(a1, a2)");
                luaInfo.WriteLine("end");

                // on_goto function.
                luaInfo.WriteLine("function on_goto(a1, a2)");
                luaInfo.WriteLine("end");

                // on_event function.
                luaInfo.WriteLine("function on_event(a1, a2)");
                foreach (KeyValuePair<string, int> entry in LevelOrder)
                {
                    luaInfo.WriteLine($"  if a2 == \"shop_buy_24{entry.Value.ToString().PadLeft(2, '0')}\" then");
                    luaInfo.WriteLine($"    SetNextMission(a1, \"scripts/mission/rando/{entry.Key}.lua\")");
                    luaInfo.WriteLine($"    MissionClear(a1, \"complete\")");
                    luaInfo.WriteLine($"  end");
                }
                luaInfo.WriteLine("end");

                // on_talk_icon function.
                luaInfo.WriteLine("function on_talk_icon(a1, a2)");
                luaInfo.WriteLine("end");

                // on_talk_setup function.
                luaInfo.WriteLine("function on_talk_setup(a1, a2)");
                luaInfo.WriteLine("  if a2 == \"shop\" then");
                luaInfo.WriteLine("    OpenShop(a1, \"g_shop\")");
                luaInfo.WriteLine("    g_message_setuped = \"\"");
                luaInfo.WriteLine("  end");
                luaInfo.WriteLine("end");

                // on_talk_open function.
                luaInfo.WriteLine("function on_talk_open(a1, a2)");
                luaInfo.WriteLine("end");

                // on_talk_close function.
                luaInfo.WriteLine("function on_talk_close(a1, a2)");
                luaInfo.WriteLine("end");

                // on_goal function.
                luaInfo.WriteLine("function on_goal(a1)");
                luaInfo.WriteLine("end");

                luaInfo.Close();
            }
        }

        /// <summary>
        /// Randomises how long various enemy wait timers are.
        /// </summary>
        /// <param name="luaFile">The enemy lua to process.</param>
        /// <param name="min">The minimum waiting time.</param>
        /// <param name="max">The maximum waiting time.</param>
        public static async Task RandomiseEnemyWaitTimes(string luaFile, double min, double max)
        {
            // Decompile this lua file.
            await Task.Run(() => Helpers.LuaDecompile(luaFile));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(luaFile);

            // Loop through each line, if one contains a value we need, split the line and replace the end part with the random function in lua.
            for (int i = 0; i < lua.Length; i++)
            {
                if (lua[i].Contains("DeathBallWaitTime = ") || lua[i].Contains("HoldExplosionWaitTime =") || lua[i].Contains("FootBrokenWait ="))
                {
                    string[] split = lua[i].Split("= ");
                    split[1] = $"math.random() + math.random({min}, {max}),";
                    lua[i] = string.Join("= ", split);
                }

                if (lua[i].Contains("WaitFixed") || lua[i].Contains("WaitLevel") || lua[i].Contains("WaitRotate"))
                {
                    string[] split = lua[i].Split(", ");
                    split[1] = $"math.random() + math.random({min}, {max}))";
                    lua[i] = string.Join(", ", split);
                }

                if (lua[i].Contains("WaitPosAdjustment"))
                {
                    string[] split = lua[i].Split(", ");
                    split[4] = $"math.random() + math.random({min}, {max}))";
                    lua[i] = string.Join(", ", split);
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(luaFile, lua);
        }
    }
}
