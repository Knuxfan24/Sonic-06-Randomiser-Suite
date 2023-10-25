using Marathon.Formats.Mesh;
using Marathon.Formats.Package;

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
                    split[1] = $"math.random() * ({max} - {min}) + {min}";
                    lua[i] = string.Join("= ", split);
                }

                if (lua[i].Contains("WaitFixed") || lua[i].Contains("WaitLevel") || lua[i].Contains("WaitRotate"))
                {
                    string[] split = lua[i].Split(", ");
                    split[1] = $"math.random() * ({max} - {min}) + {min})";
                    lua[i] = string.Join(", ", split);
                }

                if (lua[i].Contains("WaitPosAdjustment"))
                {
                    string[] split = lua[i].Split(", ");
                    split[4] = $"math.random() * ({max} - {min}) + {min})";
                    lua[i] = string.Join(", ", split);
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(luaFile, lua);
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
        /// Randomly selects a Sega Logo and Title Screen from the External Resources.
        /// </summary>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="SegaLogos">The list of Sega logo file paths.</param>
        /// <param name="TitleLogos">The list of title screen file paths.</param>
        public static async Task<bool> IntroLogos(string ModDirectory, List<string> SegaLogos, List<string> TitleLogos, bool? canUseOriginal)
        {
            // Roll a number with a max of one higher than the amount of logos.
            int value = MainWindow.Randomiser.Next(SegaLogos.Count + 1);

            // Set whether the original Sega logos can be selected or not.
            if (canUseOriginal == false)
                value = MainWindow.Randomiser.Next(SegaLogos.Count);

            // If we're higher, then leave the '06 Sega logo intact.
            if (value != SegaLogos.Count)
            {
                // Create the sound directory just in case.
                Directory.CreateDirectory($@"{ModDirectory}\xenon\sound");

                // Copy a random Sega logo.
                File.Copy(SegaLogos[value], $@"{ModDirectory}\xenon\sound\HD_SEGA.wmv", true);
            }

            // Roll a number with a max of one higher than the amount of title screens.
            value = MainWindow.Randomiser.Next(TitleLogos.Count + 1);

            // Set whether the original title screen can be selected or not.
            if (canUseOriginal == false)
                value = MainWindow.Randomiser.Next(TitleLogos.Count);

            // If we're higher, then leave the '06 title screen intact.
            if (value != TitleLogos.Count)
            {
                // Create the sound directory just in case.
                Directory.CreateDirectory($@"{ModDirectory}\xenon\sound");

                // Copy a random Sega logo.
                File.Copy(TitleLogos[value], $@"{ModDirectory}\xenon\sound\title_loop_GBn.wmv", true);

                // Tell the rando that we've replaced the title screen and need to edit the XNCP and Hybrid Patch.
                return true;
            }

            // Tell the rando that we haven't replaced the title screen and can leave the XNCP alone.
            return false;
        }

        /// <summary>
        /// Replaces all the item capsule contents textures with the same question mark icon.
        /// </summary>
        /// <param name="archivePath">The path to the extracted object.arc.</param>
        public static async Task HideItemCapsules(string archivePath)
        {
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_baria_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_gageup_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_lvup_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_muteki_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_ring5_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_ring10_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_ring20_dflu_r.dds", true);
            File.Copy($@"{Environment.CurrentDirectory}\ExternalResources\item_capsule_mystery.dds", $@"{archivePath}\win32\object\Common\itembox\cmn_speed_dflu_r.dds", true);
        }
    }
}
