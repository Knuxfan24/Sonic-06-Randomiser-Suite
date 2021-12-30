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
                foreach (Message message in mst.Data.Messages)
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
        /// Replaces the text entries in an MST with a random word.
        /// </summary>
        /// <param name="mstFile">The path to the MST to process.</param>
        /// <param name="wordList">The list of English words.</param>
        public static async Task TextGenerator(string mstFile, string[] wordList)
        {
            // Load the MST.
            MessageTable mst = new(mstFile);

            // Loop through each Message Entry in this MST.
            foreach (Message? message in mst.Data.Messages)
            {
                // Edit the New Lines, New Text Boxes and Placeholder Calls so they can be preserved.
                message.Text = message.Text.Replace("\n", " \n ");
                message.Text = message.Text.Replace("\f", " \f ");
                message.Text = message.Text.Replace("$", " $ ");

                // Split this string into an array.
                string[] split = message.Text.Split(' ');

                // Loop through and pick a random word for each array entry.
                // TODO: Option to perserve word length?
                for (int i = 0; i < split.Length; i++)
                    if (split[i] != "\n" && split[i] != "\f" && split[i] != "$")
                        split[i] = wordList[MainWindow.Randomiser.Next(wordList.Length)].ToUpper();

                // Rejoin the string and reverse the earlier edits to avoid redudant spaces.
                message.Text = String.Join(" ", split);
                message.Text = message.Text.Replace(" \n ", "\n");
                message.Text = message.Text.Replace(" \f ", "\f");
                message.Text = message.Text.Replace(" $ ", "$");
            }

            // Save the MST.
            mst.Save();
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
    }
}
