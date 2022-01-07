using Marathon.Formats.Archive;
using Marathon.Formats.Script.Lua;
using Marathon.IO;
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
            // Load text file into string array.
            string[] list = file.Split("\r\n");

            // Loop through every entry in the text file.
            foreach (string line in list)
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
            // Clear out the list.
            listBox.Items.Clear();

            // Fetch the mlua files.
            string[] patches = Directory.GetFiles(directory, "*.mlua", SearchOption.TopDirectoryOnly);

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
            foreach (string patch in patches)
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
            foreach (var checkbox in Descendants<CheckBox>(element))
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
                for (var index = 0; index < VisualTreeHelper.GetChildrenCount(dependencyItem); index++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyItem, index);
                    if (child is T dependencyObject)
                    {
                        yield return dependencyObject;
                    }

                    foreach (var childOfChild in Descendants<T>(child))
                    {
                        yield return childOfChild;
                    }
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
            try
            {
                LuaBinary lub = new(path, true);
            }
            catch
            {

            }
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
        /// <returns></returns>
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
            // Find all the files with a .rnd extension.
            string[] RandomiserFiles = Directory.GetFiles(archivePath, "*.rnd", SearchOption.AllDirectories);

            // Delete each file.
            foreach (string file in RandomiserFiles)
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
    }
}
