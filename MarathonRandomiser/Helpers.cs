using Marathon.Formats.Archive;
using Marathon.Formats.Audio;
using Marathon.Formats.Script.Lua;
using Marathon.Helpers;
using Marathon.IO;
using Marathon.IO.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MarathonRandomiser
{
    internal class Helpers
    {
        /// <summary>
        /// Fills out the custom CheckedListBox elements from text files containing comma seperated values.
        /// </summary>
        /// <param name="file">Text file containing comma seperated values.</param>
        /// <param name="listBox">The CheckedListBox element to populate.</param>
        public static void FillCheckedListBox(string file, CheckedListBox listBox)
        {
            // Loop through every entry in the text file.
            foreach (string line in file.Split("\r\n"))
            {
                // Split this entry on the comma values.
                string[] split = line.Split(',');

                // Parse this entry into a CheckedListBoxItem.
                CheckedListBoxItem item = new()
                {
                    DisplayName = split[0],
                    Tag = split[1],
                    Checked = bool.Parse(split[2])
                };

                // Add this parsed entry into the CheckedListBox.
                listBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Takes the user's specified patches directory and gets the mlua files from it.
        /// </summary>
        /// <param name="directory">The provided path.</param>
        /// <param name="listBox">The CheckedListBox to fill in.</param>
        public static void FetchPatches(string directory, CheckedListBox listBox)
        {
            // This crashes if the directory doesn't exist, so check that first.
            if (!Directory.Exists(directory))
                return;

            // Clear out the list.
            listBox.Items.Clear();

            // Set up a list of patches that don't fit too well with the Randomiser.
            List<string> Forbidden = new()
            {
                "Disable2xMSAA.mlua",
                "Disable4xMSAA.mlua",
                "DisableCharacterDialogue.mlua",
                "DisableCharacterUpgrades.mlua",
                "DisableHUD.mlua",
                "DisableHintRings.mlua",
                "DisableMusic.mlua",
                "DisableShadows.mlua",
                "DisableTalkWindowInStages.mlua",
                "DoNotCarryElise.mlua",
                "DoNotEnterMachSpeed.mlua",
                "DoNotUseTheSnowboard.mlua",
                "EnableDebugMode.mlua",
                "OmegaBlurFix.mlua",
                "TGS2006Menu.mlua"
            };

            // Loop through and add the patches to the CheckedList_Misc_Patches element
            foreach (string patch in Directory.GetFiles(directory, "*.mlua", SearchOption.TopDirectoryOnly))
            {
                // Read the mlua and split it's second line (contains the title) into a seperate array we can use.
                string[] mlua = File.ReadAllLines(patch);
                string[] split = mlua[1].Split('\"');

                CheckedListBoxItem item = new()
                {
                    DisplayName = split[1],
                    Tag = Path.GetFileName(patch),
                    Checked = true
                };

                // Check if this patch is a forbidden one, if so, uncheck it by default.
                if (Forbidden.Contains(Path.GetFileName(patch)))
                    item.Checked = false;

                listBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Automatically fills in a custom CheckedListBox element based on the Checkboxes in a tab.
        /// </summary>
        /// <param name="element">The element to check through.</param>
        /// <param name="listBox">The CheckedListBox element to populate.</param>
        public static void FillWildcardCheckedListBox(DependencyObject element, CheckedListBox listBox)
        {
            // Loop through each Checkbox in the Tab Grid.
            foreach (CheckBox checkbox in Descendants<CheckBox>(element))
            {
                // Create an item for each.
                CheckedListBoxItem item = new()
                {
                    DisplayName = (string)checkbox.Content,
                    Tag = checkbox.Name,
                    Checked = true
                };

                // If this Checkbox is red (my not recommended colour) don't check it by default.
                if (checkbox.Foreground == Brushes.Firebrick)
                    item.Checked = false;

                // Add this parsed entry into the CheckedListBox.
                listBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Finds matching elements in a WPF Control.
        /// https://social.technet.microsoft.com/wiki/contents/articles/53438.wpf-get-all-controls-of-a-specific-type-using-c.aspx#Base_extension_method
        /// </summary>
        public static IEnumerable<T> Descendants<T>(DependencyObject dependencyItem) where T : DependencyObject
        {
            if (dependencyItem != null)
            {
                for (int index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyItem); index++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyItem, index);

                    if (child is T dependencyObject)
                        yield return dependencyObject;

                    foreach (T childOfChild in Descendants<T>(child))
                        yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// Hacky workaround to fix the custom CheckedListBox element not visually updating if addressed in code.
        /// </summary>
        /// <param name="listBox">The CheckedListBox element to update.</param>
        /// <param name="edit">Whether we're also doing editing first</param>
        /// <param name="checkAll">Whether to check or uncheck the elements if we are editing.</param>
        public static void InvalidateCheckedListBox(CheckedListBox listBox, bool edit = false, bool checkAll = true)
        {
            if (edit)
            {
                foreach (CheckedListBoxItem item in listBox.Items)
                {
                    if (checkAll)
                        item.Checked = true;
                    else
                        item.Checked = false;
                }
            }

            // Get a standalone list of all the items in the target CheckedListBox.
            List<CheckedListBoxItem> listItems = listBox.Items.ToList();

            // Clear out the existing list of items in the target CheckedListBox.
            listBox.Items.Clear();

            // Loop through and add the items back with their updated status.
            foreach (CheckedListBoxItem item in listItems)
                listBox.Items.Add(item);
        }

        /// <summary>
        /// Loops through every item in a CheckedListBox element and return a list of their tags if checked.
        /// </summary>
        /// <param name="listBox">The CheckedListBox element to loop through.</param>
        /// <returns>The string list to return.</returns>
        public static List<string> EnumerateCheckedListBox(CheckedListBox listBox)
        {
            // Set up our string list.
            List<string> list = new();

            // Loop through each item in the CheckedListBox element.
            foreach (CheckedListBoxItem item in listBox.Items)
            {
                // If the item is checked, add its tag value to the list.
                if (item.Checked)
                    list.Add(item.Tag);
            }

            // Return the string list.
            return list;
        }

        /// <summary>
        /// Decompiles a Lua Binary file.
        /// </summary>
        /// <param name="path">Path to the Lua Binary to decompile.</param>
        public static async Task LuaDecompile(string path)
        {
            // Marathon throws an exception if a Lua is already decompiled, so catch any to prevent a program crash.
            try { LuaBinary lub = new(path, true); }
            catch { }
        }

        /// <summary>
        /// Replaces illegal characters from a path with underscores.
        /// </summary>
        /// <param name="text">The string to strip out.</param>
        public static string UseSafeFormattedCharacters(string text)
        {
            return text.Replace(@"\", "_")
                       .Replace("/", "_")
                       .Replace(":", "_")
                       .Replace("*", "_")
                       .Replace("?", "_")
                       .Replace("\"", "_")
                       .Replace("<", "_")
                       .Replace(">", "_")
                       .Replace("|", "_");
        }

        /// <summary>
        /// Unpacks a Sonic '06 U8 Archive to the temporary directory.
        /// </summary>
        /// <param name="archive">The path to the archive to unpack.</param>
        /// <returns>The location of the unpacked archive.</returns>
        public static async Task<string> ArchiveHandler(string archive, bool dontUnpack = false)
        {
            // Determine whether this archive is in a win32, xenon or ps3 directory.
            string archiveLocation = "";
            if (archive.Contains("\\win32"))
                archiveLocation = archive.Remove(archive.IndexOf("\\win32"));
            if (archive.Contains("\\xenon"))
                archiveLocation = archive.Remove(archive.IndexOf("\\xenon"));
            if (archive.Contains("\\ps3"))
                archiveLocation = archive.Remove(archive.IndexOf("\\ps3"));

            // Check if the archive is already extracted to the temporary directory. If not, then unpack it.
            if (!Directory.Exists($@"{MainWindow.TemporaryDirectory}{archive[0..^4].Replace(archiveLocation, "")}") && !dontUnpack)
            {
                U8Archive arc = new(archive, ReadMode.IndexOnly);
                arc.Extract($@"{MainWindow.TemporaryDirectory}{archive[0..^4].Replace(archiveLocation, "")}");
            }
            // Return the path to the unpacked archive as a string.
            return $@"{MainWindow.TemporaryDirectory}{archive[0..^4].Replace(archiveLocation, "")}";
        }

        /// <summary>
        /// Repacks a directory to a U8 Archive. Currently using Optimal compression due to a Marathon bug.
        /// </summary>
        /// <param name="path">The path to the repack.</param>
        /// <param name="savePath">Where to save the packed archive to.</param>
        public static async Task ArchiveHandler(string path, string savePath)
        {
            U8Archive arc = new(path, true, CompressionLevel.Optimal);
            arc.Save(savePath);
        }

        /// <summary>
        /// Writes a hint name at an offset and figure out how many nulls need to be written to pad it to 19 characters.
        /// </summary>
        /// <param name="patchInfo">The streamwriter for our hyrbid patch file.</param>
        /// <param name="offset">Where we're writing to.</param>
        /// <param name="hint">The name of the hint to write here.</param>
        public static void HybridPatchWriter(StreamWriter patchInfo, int offset, string hint)
        {
            patchInfo.WriteLine($"WriteTextBytes(Executable|0x{offset:X}|\"{hint}\")");
            patchInfo.WriteLine($"WriteNullBytes(Executable|0x{offset + hint.Length:X}|{19 - hint.Length})");
        }

        /// <summary>
        /// Fetches my official voice packs from https://github.com/Knuxfan24/Sonic-06-Randomiser-Suite-Voice-Packs.
        /// Based on code demonstration shown here: https://markheath.net/post/list-and-download-github-repo-cs.
        /// </summary>
        /// <returns>A dictionary of the url for each pack and the html decoded filename of each one.</returns>
        public static async Task<Dictionary<string, string>> FetchOfficalVox()
        {
            // Set up our dictionary.
            Dictionary<string, string> packs = new();

            // Get a JSON of the files on the GitHub repo.
            HttpClient? httpClient = new();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("MyApplication", "1"));
            string? repo = "Knuxfan24/Sonic-06-Randomiser-Suite-Voice-Packs";
            string? contentsUrl = $"https://api.github.com/repos/{repo}/contents";
            string? contentsJson = await httpClient.GetStringAsync(contentsUrl);
            JArray? contents = (JArray)JsonConvert.DeserializeObject(contentsJson);

            // Loop through each file.
            foreach (JToken? file in contents)
            {
                // Whether this is a file or a directory, as I don't plan to use directories I can ignore those.
                string? fileType = (string)file["type"];
                if (fileType == "file")
                {
                    // Get the actual url.
                    string? downloadUrl = (string)file["download_url"];

                    // Strip the url down to the file name and decode its HTML.
                    string decodedFilename = Path.GetFileName(HttpUtility.UrlDecode(downloadUrl));

                    // Only add this file to the dictionary if it's a ZIP (so ignore the README basically).
                    if (decodedFilename.EndsWith(".zip"))
                        packs.Add(downloadUrl, decodedFilename);
                }
            }

            // Return our dictionary.
            return packs;
        }

        /// <summary>
        /// Downloads the specified voice pack.
        /// </summary>
        /// <param name="pack">The entry in the dictonary containing the url and file name.</param>
        public static async Task DownloadVox(KeyValuePair<string, string> pack)
        {
            using WebClient? client = new();
            client.DownloadFile(pack.Key, $@"{Environment.CurrentDirectory}\VoicePacks\{pack.Value}");
        }

        /// <summary>
        /// Cleans up any .rnd files in the specified archive.
        /// </summary>
        /// <param name="archivePath">The archive to process.</param>
        public static async Task CleanUpShuffleLeftovers(string archivePath)
        {
            // Delete each file with my .rnd extension.
            foreach (string file in Directory.GetFiles(archivePath, "*.rnd", SearchOption.AllDirectories))
                File.Delete(file);
        }
    
        /// <summary>
        /// Checks if a Lua needs to be processed for a function.
        /// </summary>
        /// <param name="luaPath">The Lua Binary to check.</param>
        /// <param name="neededParameters">The parameter this file needs to contain at least one of.</param>
        /// <returns>Whether or not this file needs processing.</returns>
        public static async Task<bool> NeededLua(string luaPath, List<string> neededParameters)
        {
            // Set up a list of forbidden luas (as I think some cause problems if decompiled).
            List<string> Forbidden = new()
            {
                "actionarea.lub",
                "actionstage.lub",
                "game.lub",
                "standard.lub",
                "sonic.lub",
                "shadow.lub",
                "silver.lub",
                "gameshow_sonic.lub",
                "gameshow_shadow.lub",
                "gameshow_silver.lub",
            };

            // If this lua is a forbidden one, ignore it.
            if (Forbidden.Contains(Path.GetFileName(luaPath)))
                return false;

            // If it's not forbidden, then read it and check for any of the needed parameters.
            string lua = File.ReadAllText(luaPath);
            bool b = neededParameters.Any(lua.Contains);
            return b;
        }

        /// <summary>
        /// Patches the voice_all_e.sbk file to load every voice file.
        /// </summary>
        /// <param name="archivePath">The path to an extracted sound.arc.</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        public static async Task VoiceBankPatcher(string archivePath, string corePath)
        {
            // Load the voice_all_e.sbk file.
            SoundBank voice_all = new($@"{archivePath}\{corePath}\sound\voice_all_e.sbk");

            // Loop through each English sound bank in sound.arc.
            foreach (string sbkFile in Directory.GetFiles($@"{archivePath}\{corePath}\sound", "voice_*_e.sbk"))
            {
                // Ignore this one if it's voice_all_e.sbk.
                if (Path.GetFileName(sbkFile) == "voice_all_e.sbk")
                    continue;

                // Load this sound bank.
                SoundBank sbk = new(sbkFile);

                // Add this sound bank's cues to voice_all_e.sbk.
                voice_all.Data.Cues.AddRange(sbk.Data.Cues);
            }

            // Save our updated voice_all_e.sbk.
            voice_all.Save();
        }

        /// <summary>
        /// Takes a lits of strings and shuffles their order.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <returns>The shuffled list.</returns>
        public static async Task<List<string>> ShuffleList(List<string> list)
        {
            // Set up our list of numbers.
            List<int> usedNumbers = new();

            // Set up our new list.
            List<string> newList = new();

            // Loop through all the messages in the list.
            for (int i = 0; i < list.Count; i++)
            {
                // Pick a random message.
                int index = MainWindow.Randomiser.Next(list.Count);

                // If it's used, keep picking new ones until it's not.
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(list.Count); }
                    while (usedNumbers.Contains(index));
                }

                // Mark the selected numbers as used.
                usedNumbers.Add(index);

                // Add the selected message to the new list.
                newList.Add(list[index]);
            }

            // Return the new list.
            return newList;
        }

        /// <summary>
        /// Looks through an archive and extracts all the render scripts from it.
        /// </summary>
        /// <param name="archive">The loaded archive.</param>
        /// <param name="archivePath">Where to extract the files.</param>
        public static async Task RendererExtract(U8Archive archive, string archivePath)
        {
            foreach (IArchiveFile file in archive.Root.GetFiles())
                if (file.Name.StartsWith("render"))
                    file.Extract($@"{archivePath}\{file.Path}");
        }

        /// <summary>
        /// Automates copying content from Very Hard Mode for use in the Episode Generator.
        /// </summary>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="archive">The Very Hard Mode archive in memory.</param>
        /// <param name="stage">The stage name we need the files of.</param>
        /// <param name="archivePath">The scripts.arc to extract files to.</param>
        /// <param name="missionID">The mission ID we need the files of.</param>
        /// <param name="character">The character we need the files of.</param>
        public static async Task VeryHardModeExtractor(string corePath, U8Archive archive, string stage, string archivePath, string missionID, string character)
        {
            // Extract and replace the area files.
            foreach (IArchiveFile? file in archive.Root.GetFiles())
                if (file.Path.Contains($"{corePath}/scripts/stage/{stage}"))
                    file.Extract($@"{archivePath}\{corePath}\scripts\stage\{stage}\{file.Name}");

            // Replace the mission script.
            archive.Root.GetFile($"{corePath}/scripts/mission/{missionID}/mission.lub").Extract($@"{archivePath}\{corePath}\scripts\mission\rando\mission_{character}_{stage}.lub");

            // Create the directory for the paths and sets.
            Directory.CreateDirectory($@"{archivePath}\{corePath}\scripts\mission\{missionID}");

            // Extract the paths and sets.
            foreach (IArchiveFile? file in archive.Root.GetFiles())
                if (file.Path.Contains(missionID) && file.Name != "mission.lub")
                    file.Extract($@"{archivePath}\{corePath}\scripts\mission\{missionID}\{file.Name}");
        }

        // https://stackoverflow.com/a/4135491
        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
