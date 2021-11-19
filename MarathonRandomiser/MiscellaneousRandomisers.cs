using Marathon.Formats.Mesh;
using Marathon.Formats.Package;
using Marathon.Formats.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class MiscellaneousRandomisers
    {
        /// <summary>
        /// Randomises the music to play.
        /// </summary>
        /// <param name="luaFile">The lua to process.</param>
        /// <param name="MiscMusic">The list of valid songs.</param>
        /// <returns></returns>
        public static async Task MusicRandomiser(string luaFile, List<string> MiscMusic)
        {
            // Decompile this lua binary.
            await Task.Run(() => Helpers.LuaDecompile(luaFile));

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
                    song[1] = MiscMusic[MainWindow.Randomiser.Next(MiscMusic.Count)];

                    // Rejoin the split array into one line and add it back to the original lua array.
                    lua[i] = string.Join("\"", song);
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(luaFile, lua);
        }

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
        /// Shuffles all the text entries in all the game's message table files around.
        /// </summary>
        /// <param name="eventArc">The path to the already unpacked event.arc.</param>
        /// <param name="textArc">The path to the already unpacked text.arc.</param>
        /// <param name="languages">The list of valid language codes to include.</param>
        public static async Task TextRandomiser(string eventArc, string textArc, List<string> languages)
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
            using MessageTable list = new();

            // Loop through all the message tables.
            foreach (var mstFile in mstFiles)
            {
                // Load this message table.
                using MessageTable mst = new(mstFile);

                // Copy every message into our list message table for later referal.
                foreach(Message message in mst.Data.Messages)
                    list.Data.Messages.Add(message);
            }

            // Loop through all the message tables again.
            foreach (var mstFile in mstFiles)
            {
                // Load this message table.
                using MessageTable mst = new(mstFile);

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
        /// Sets random patches to be required so the Mod Manager will auto install them with the randomisation.
        /// </summary>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="MiscPatches">The list of valid patch files.</param>
        /// <param name="Weight">The likelyhood for a patch to be enabled.</param>
        /// <returns></returns>
        public static async Task PatchRandomiser(string ModDirectory, List<string> MiscPatches, int Weight)
        {
            // Create the template list of patches to add to the mod configuration ini.
            string patchList = "RequiredPatches=\"";

            // Loop through our list of valid patches.
            foreach(string patch in MiscPatches)
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
        /// <returns></returns>
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
        /// Shuffles the XNMs called for character animations in a PKG file.
        /// </summary>
        /// <param name="pkgFile">The PKG file to process.</param>
        public static async Task AnimationRandomiser(string pkgFile)
        {
            // Set up a couple of lists.
            List<string> XNMFiles = new();
            List<int> usedNumbers = new();

            // Load the PKG.
            AssetPackage pkg = new(pkgFile);

            // Loop through the PKGs to find the Motion Category.
            foreach (var type in pkg.Types)
            {
                if (type.Name == "motion")
                {
                    // Loop through the entries to find the XNM files.
                    foreach (AssetFile? entry in type.Files)
                    {
                        // Only add this entry's XNM if it meets these criteria (as XNMs under these tend to be for faces or Omega's body parts)
                        if (!entry.Name.Contains("face") && entry.Name != "style" && !entry.File.Contains("point.xnm"))
                        {
                            // Add this XNM to the list of valid files for this PKG.
                            XNMFiles.Add(entry.File);
                        }
                    }

                    // Loop through the entries again to pick new XNMs.
                    foreach (AssetFile? entry in type.Files)
                    {
                        // Same criteria check as above.
                        if (!entry.Name.Contains("face") && entry.Name != "style" && !entry.File.Contains("point.xnm"))
                        {
                            // Pick a random number from the amount of entires in the XNM list.
                            int index = MainWindow.Randomiser.Next(XNMFiles.Count);

                            // If the selected number is already used, pick another until it isn't.
                            if (usedNumbers.Contains(index))
                            {
                                do { index = MainWindow.Randomiser.Next(XNMFiles.Count); }
                                while (usedNumbers.Contains(index));
                            }

                            // Add this number to the usedNumbers list so we can't pull the same XNM twice.
                            usedNumbers.Add(index);

                            // Set this entry's XNM to the selected one.
                            entry.File = XNMFiles[index];
                        }
                    }
                }
            }

            // Save the updated PKG.
            pkg.Save();
        }

        /// <summary>
        /// Randomises the XNMs for each event.
        /// </summary>
        /// <param name="archivePath">The path to the extracted event_data.arc.</param>
        /// <param name="character">The character to shuffle the animations of.</param>
        /// <param name="type">The type of animation to shuffle (Root or evf_head).</param>
        /// <returns></returns>
        public static async Task EventAnimationRandomiser(string archivePath, string character, string type)
        {
            // Set up a list of numbers.
            List<int> usedNumbers = new();

            // Get the XNMs that fit our search criteria.
            string[] XNMFiles = Directory.GetFiles(archivePath, $"*_{character}_{type}*.xnm", SearchOption.AllDirectories);

            // Loop through each XNM and rename it to have a .rnd extension.
            for (int i = 0; i < XNMFiles.Length; i++)
            {
                File.Move(XNMFiles[i], $"{XNMFiles[i]}.rnd");
                XNMFiles[i] = $"{XNMFiles[i]}.rnd";
            }

            // Loop through again for the actual randomisation.
            for(int i = 0; i < XNMFiles.Length; i++)
            {
                // Pick a random number from the amount of entires in the XNM list.
                int index = MainWindow.Randomiser.Next(XNMFiles.Length);

                // If the selected number is already used, pick another until it isn't.
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(XNMFiles.Length); }
                    while (usedNumbers.Contains(index));
                }

                // Add this number to the usedNumbers list so we can't pull the same XNM twice.
                usedNumbers.Add(index);

                // Copy this XNM to the name of the chosen one, minus our .rnd addition.
                File.Copy(XNMFiles[i], $"{XNMFiles[index].Remove(XNMFiles[index].LastIndexOf('.'))}");

                // Delete the .rnd version.
                File.Delete(XNMFiles[i]);
            }
        }
    }
}
