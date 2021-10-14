using Marathon.Formats.Mesh;
using Marathon.Formats.Package;
using Marathon.Formats.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SonicNextRandomiser.Randomisers
{
    class MiscellaneousRandomiser
    {
        /// <summary>
        /// Randomises the music used in stages and missions.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked scripts.arc containing the mission and area lua binaries.</param>
        /// <param name="songs">The list of valid songs.</param>
        public static void MusicRandomiser(string archivePath, List<string> songs)
        {
            // Get a list of all the lua binaries in scripts.arc
            string[] luaFiles = Directory.GetFiles(archivePath, "*.lub", SearchOption.AllDirectories);

            // Loop through the lua binaries.
            foreach (string luaFile in luaFiles)
            {
                // Ignore any lua binaries that are not in a stage folder or don't contain a mission name.
                if (!luaFile.Contains("\\stage\\") && !luaFile.Contains("mission"))
                    continue;

                // Minimise the number of unnecessary lua decompilations.
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
                    luaFile.Substring(luaFile.LastIndexOf('\\') + 1).StartsWith("thirdiblis")       || luaFile.Contains("mission"))
                {
                    // Decompile this lua binary.
                    System.Console.WriteLine($@"Randomising music in '{luaFile}'.");
                    Helpers.DecompileLua(luaFile);

                    // Read the decompiled lua file into a string array.
                    string[] lua = File.ReadAllLines(luaFile);

                    // Loop through each line in this lua binary.
                    for (int i = 0; i < lua.Length; i++)
                    {
                        // Search for the two lines that control music playback.
                        if (lua[i].Contains("Game.PlayBGM") || lua[i].Contains("mission_bgm"))
                        {
                            // Split the line controlling the music playback up based on the quote marks around the song name.
                            string[] song = lua[i].Split('"');

                            // Replace the second value in the split array (the one containing the song name) with a song from the list of valid songs.
                            song[1] = songs[MainWindow.Randomiser.Next(songs.Count)];

                            // Rejoin the split array into one line and add it back to the original lua array.
                            lua[i] = string.Join("\"", song);
                        }
                    }

                    // Save the updated lua binary.
                    File.WriteAllLines(luaFile, lua);
                }
            }
        }

        /// <summary>
        /// Randomises the amount of health enemies have, controlled by ScriptParameter.bin.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked enemy.arc containing ScriptParameter.bin</param>
        /// <param name="minHealth">The minimum value the enemy health can be set to.</param>
        /// <param name="maxHealth">The maximum value the enemy health can be set to.</param>
        /// <param name="includeBosses">Whether or not bosses should also get their health values randomised.</param>
        public static void EnemyHealthRandomiser(string archivePath, int minHealth, int maxHealth, bool includeBosses)
        {
            // Load ScriptParameter.bin
            System.Console.WriteLine($@"Randomising health values in '{archivePath}\xenon\enemy\ScriptParameter.bin'.");
            ScriptPackage scriptPackage = new($@"{archivePath}\xenon\enemy\ScriptParameter.bin");

            // Loop through every enemy parameter in ScriptParameter.bin
            foreach (ScriptParameter parameter in scriptPackage.Parameters)
            {
                // If bosses are disabled, then ignore anything related to them (except for the Mephiles Kyozoress enemy that shows up in Mephiles Phase 2).
                if (!includeBosses && (parameter.Name == "firstIblis" || parameter.Name == "secondIblis_sonic" || parameter.Name == "secondIblis_shadow" || parameter.Name == "thirdIblis" ||
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
        /// <param name="archivePath">The path to the already unpacked stage.arc containing all the collision binary files.</param>
        /// <param name="perFace">Whether the collision should be randomised per face rather than per type.</param>
        public static void SurfaceRandomiser(string archivePath, bool perFace)
        {
            // Predetermine what each type will be.
            string concrete  = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string water     = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string wood      = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string metal     = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string grass     = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string sand      = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string snow      = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string dirt      = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string glass     = MainWindow.Randomiser.Next(0, 16).ToString("X");
            string metalEcho = MainWindow.Randomiser.Next(0, 16).ToString("X");

            // Get a list of all the collision.bin files in stage.arc.
            string[] collisionFiles = Directory.GetFiles(archivePath, "collision.bin", SearchOption.AllDirectories);

            // Loop through all the collision.bin files.
            foreach(string collisionFile in collisionFiles)
            {
                // Load this collision file.
                System.Console.WriteLine($@"Randomising surface flags in '{collisionFile}'.");
                Collision collision = new(collisionFile);

                // Loop through this collision file's faces.
                foreach (CollisionFace face in collision.Data.Faces)
                {
                    // Parse the hex value of this face's collision tag into a string.
                    string hexValue = face.Flags.ToString("X").PadLeft(8, '0');

                    // If we're randomising the collision by face rather than type, then replace the last value in the string with a random hex value from 0 to F.
                    if (perFace)
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
        }

        /// <summary>
        /// Shuffles all the text entries in all the game's message table files around.
        /// </summary>
        /// <param name="eventArc">The path to the already unpacked event.arc.</param>
        /// <param name="textArc">The path to the already unpacked text.arc.</param>
        /// <param name="languages">The list of valid language codes to include.</param>
        public static void TextRandomiser(string eventArc, string textArc, List<string> languages)
        {
            // Get a list of all the Message Tables in both archives according to the chosen languages.
            string[] mstFiles = Array.Empty<string>();
            foreach (string language in languages)
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

            // Set up a list so we can track which messages have already been used.
            List<int> usedNumbers = new();

            // Create a list of all the messages.
            MessageTable list = new();

            // Loop through all the message tables.
            foreach (var mstFile in mstFiles)
            {
                // Load this message table.
                MessageTable mst = new(mstFile);

                // Copy every message into our list message table for later referal.
                foreach(Message message in mst.Data.Messages)
                    list.Data.Messages.Add(message);
            }

            // Loop through all the message tables again.
            foreach (var mstFile in mstFiles)
            {
                // Load this message table.
                System.Console.WriteLine($@"Shuffling text in '{mstFile}'.");
                MessageTable mst = new(mstFile);

                // Loop through every message in this message table.
                foreach (Message message in mst.Data.Messages)
                {
                    // Pick a random number from the amount of entires in the list message table.
                    int index = MainWindow.Randomiser.Next(list.Data.Messages.Count);

                    // If the selected number is already used, pick another until it isn't.
                    if (usedNumbers.Contains(index))
                    {
                        do { index = MainWindow.Randomiser.Next(list.Data.Messages.Count); }
                        while (usedNumbers.Contains(index));
                    }

                    // Add this number to the usedNumbers list so we can't pull the same message twice.
                    usedNumbers.Add(index);

                    // Copy the selected message's placeholders and text over this one's.
                    message.Placeholders = list.Data.Messages[index].Placeholders;
                    message.Text = list.Data.Messages[index].Text;
                }

                // Save the updated message table.
                mst.Save();
            }
        }

        /// <summary>
        /// Randomly forces on patches from the user's patch directory.
        /// </summary>
        /// <param name="modsDirectory">The location of the mods.</param>
        /// <param name="seed">The seed being used for this randomisation.</param>
        public static void PatchRandomiser(string modsDirectory, string seed)
        {
            System.Console.WriteLine($@"Randomising patches.");

            // Get a list of all of the user's patches.
            string[] patches = Directory.GetFiles($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Unify\\Patches\\", "*.mlua", SearchOption.TopDirectoryOnly);

            // Create the template list of patches to add to the mod configuration ini.
            string patchList = "RequiredPatches=\"";

            // Loop through all the patches found.
            foreach(string patch in patches)
            {
                // Remove certain patches which don't fit too well with the Randomiser.
                if (!patch.Contains("EnableDebugMode.mlua") && !patch.Contains("Disable2xMSAA.mlua") && !patch.Contains("Disable4xMSAA.mlua") && !patch.Contains("DisableCharacterDialogue.mlua") &&
                    !patch.Contains("DisableCharacterUpgrades.mlua") && !patch.Contains("DisableHintRings.mlua") && !patch.Contains("DisableHUD.mlua") && !patch.Contains("DisableMusic.mlua") &&
                    !patch.Contains("DisableShadows.mlua") && !patch.Contains("DisableTalkWindowInStages.mlua") && !patch.Contains("DoNotCarryElise.mlua") && !patch.Contains("DoNotEnterMachSpeed.mlua") &&
                    !patch.Contains("DoNotUseTheSnowboard.mlua") && !patch.Contains("OmegaBlurFix.mlua") && !patch.Contains("TGS2006Menu.mlua"))
                {
                    // Toss a coin to see if we should actually use this patch, if so, add the name of its mlua file to the list.
                    if (MainWindow.Randomiser.Next(0, 2) == 1)
                        patchList += $"{Path.GetFileName(patch)},";
                }
            }

            // Add all the patches to the mod configuration ini.
            // Remove the last comma and replace it with a closing quote.
            patchList = patchList.Remove(patchList.LastIndexOf(','));
            patchList += "\"";

            // Write the list of required patches (as well as the [Patches] ini block) to the mod configuration ini.
            using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini")))
            {
                configInfo.WriteLine();
                configInfo.WriteLine("[Patches]");
                configInfo.WriteLine(patchList);

                configInfo.Close();
            }
        }
    }
}
