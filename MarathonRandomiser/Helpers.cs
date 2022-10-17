using DirectShowLib.DES;
using DirectShowLib;
using Marathon.Formats.Archive;
using Marathon.Formats.Audio;
using Marathon.Formats.Mesh.Ninja;
using Marathon.Formats.Script.Lua;
using Marathon.Formats.Text;
using Marathon.Helpers;
using Marathon.IO;
using Marathon.IO.Interfaces;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;

namespace MarathonRandomiser
{
    public class CriwareAcroarts
    {
        public uint ABRSOffset { get; set; }

        public byte[] Bytes { get; set; }

        public List<CriwareFileRef> Files { get; set; } = new();

        public uint FooterLength { get; set; }

        public byte[] EndBytes { get; set; }
    }

    public class CriwareFileRef
    {
        public uint OffsetPadding { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }
    }

    internal class Helpers
    {
        /// <summary>
        /// Gets the version number from the GitHub.
        /// </summary>
        public static string GetVersionNumber()
        {
            // Set up the HTTPClient.
            HttpClient? httpClient = new();
            httpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("MyApplication", "1"));
            string? repo = "Knuxfan24/Sonic-06-Randomiser-Suite";
            string? contentsUrl = $"https://api.github.com/repos/{repo}/contents";

            // Try to get the list of files in the repo's root as a JSON.
            Task<string> task = Task.Run(() => httpClient.GetStringAsync(contentsUrl));
            try { task.Wait(); }
            catch { return ""; }
            string? contentsJson = task.Result;

            JArray? contents = (JArray)JsonConvert.DeserializeObject(contentsJson);

            // Loop through each file to find version.txt.
            foreach (JToken? file in contents)
            {
                string? fileName = (string)file["name"];

                if (fileName == "version.txt")
                {
                    // Temporarily download the version text file.
                    using WebClient? client = new();
                    client.DownloadFile((string)file["download_url"], $@"{Environment.CurrentDirectory}\version.txt");

                    // Read from it.
                    string ret = File.ReadAllText($@"{Environment.CurrentDirectory}\version.txt");

                    // Delete it.
                    File.Delete($@"{Environment.CurrentDirectory}\version.txt");

                    // Return what we read.
                    return ret;
                }
            }

            // Return an empty string if we haven't found version.txt.
            return "";
        }

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
                "RestoreScrappedBattleActs.mlua",
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
                // Get this file's name.
                string? fileName = (string)file["name"];

                // If it's a zip archive, add it to our dictionary.
                if (Path.GetExtension(fileName) == ".zip")
                    packs.Add((string)file["download_url"], fileName);
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
                return char.ToUpper(str[0]) + str[1..];

            return str.ToUpper();
        }

        /// <summary>
        /// Adds a file to the Custom parameter in mod.ini
        /// </summary>
        /// <param name="file">The name of the file to add to the Custom parameter.</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="CustomFilesystem">Whether we also need to flip the CustomFilesystem flag.</param>
        public static async Task UpdateCustomFiles(string file, string ModDirectory, bool CustomFilesystem = false)
        {
            // Setup a check in chase we already have Custom Files.
            bool alreadyHasCustom = false;

            // Set up the initial Custom line.
            string customLine = $"Custom=\"{file}\"";

            // Load the mod configuration ini to see if we already have custom content. If we do, then read the custom files line and patch our addition onto it.
            string[] modConfig = File.ReadAllLines(Path.Combine($@"{ModDirectory}", "mod.ini"));
            if (modConfig.Length == 11)
            {
                alreadyHasCustom = true;
                customLine = modConfig[10].Remove(modConfig[10].LastIndexOf('\"'));
                customLine += $",{file}\"";
            }

            // If we aren't already using custom files, then write the custom list.
            if (!alreadyHasCustom)
            {
                using StreamWriter configInfo = File.AppendText(Path.Combine($@"{ModDirectory}", "mod.ini"));
                configInfo.WriteLine(customLine);
                configInfo.Close();
            }
            // If not, then patch the expanded list over the top of the existing one.
            else
            {
                modConfig[10] = customLine;
                File.WriteAllLines(Path.Combine($@"{ModDirectory}", "mod.ini"), modConfig);
            }

            // Enable the Custom Filesystem flag.
            if (CustomFilesystem)
            {
                // Update modConfig just to be safe.
                modConfig = File.ReadAllLines(Path.Combine($@"{ModDirectory}", "mod.ini"));

                // Loop through each line in modConfig.
                for (int i = 0; i < modConfig.Length; i++)
                {
                    // If this line is the CustomFilesystem one and is set to false, then set it to true and resave the config.
                    if (modConfig[i] == "CustomFilesystem=\"False\"")
                    {
                        modConfig[i] = "CustomFilesystem=\"True\"";
                        File.WriteAllLines(Path.Combine($@"{ModDirectory}", "mod.ini"), modConfig);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the Main Menu header to show the Randomiser's version number.
        /// </summary>
        /// <param name="archivePath">The path to an extracted text.arc.</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        public static async Task VersionNumberMST(string archivePath, string corePath)
        {
            // Loop through all the msg_mainmenu message tables.
            foreach (string mstFile in Directory.GetFiles($@"{archivePath}\{corePath}\text", "msg_mainmenu.*.mst", SearchOption.AllDirectories))
            {
                // Load this message table.
                MessageTable mst = new(mstFile);

                // Loop through all the messages in this table.
                foreach (Message? message in mst.Data.Messages)
                {
                    // If this is the msg_mainmenu message, then replace its text with the Rando's version number and null out the Placeholders (just in case).
                    if (message.Name == "msg_mainmenu")
                    {
                        message.Text = $"Randomiser Suite {MainWindow.VersionNumber}";
                        message.Placeholders = null;
                    }
                }

                // Resave this message table.
                mst.Save();
            }
        }

        // https://markheath.net/post/normalize-audio-naudio
        public static async Task WavNormalise(string file)
        {
            float max = 0;

            using (var reader = new AudioFileReader(file))
            {
                // find the max peak
                float[] buffer = new float[reader.WaveFormat.SampleRate];
                int read;
                do
                {
                    read = reader.Read(buffer, 0, buffer.Length);
                    for (int n = 0; n < read; n++)
                    {
                        var abs = Math.Abs(buffer[n]);
                        if (abs > max) max = abs;
                    }
                } while (read > 0);
                Console.WriteLine($"Max sample value: {max}");

                if (max == 0 || max > 1.0f)
                    throw new InvalidOperationException("File cannot be normalized");

                // rewind and amplify
                reader.Position = 0;
                reader.Volume = 1.0f / max;

                // write out to a new WAV file
                WaveFileWriter.CreateWaveFile16($@"{Path.GetDirectoryName(file)}\{Path.GetFileNameWithoutExtension(file)}_norm.wav", reader);
            }

            File.Delete(file);
            File.Move($@"{Path.GetDirectoryName(file)}\{Path.GetFileNameWithoutExtension(file)}_norm.wav", file);
        }

        /// <summary>
        /// Uses XMAEncode to convert a WAV file to an XMA file.
        /// </summary>
        /// <param name="inputFile">The path to the WAV file to convert.</param>
        /// <param name="outputFile">Where to save the XMA file.</param>
        public static async Task XMAEncode(string inputFile, string outputFile)
        {
            using (Process process = new())
            {
                process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\xmaencode.exe\"";
                process.StartInfo.Arguments = $"\"{inputFile}\" /b 64 /t \"{outputFile}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
        }

        /// <summary>
        /// Finds a specific XNO in the target archive.
        /// </summary>
        /// <param name="archive">The archive to serach.</param>
        /// <param name="targetFile">The XNO file to find.</param>
        /// <param name="character">The character name to store this under.</param>
        /// <param name="modelLocations">The dictonary of XNOs to add this to.</param>
        public static Dictionary<string, string> FindPlayer(string archive, string targetFile, string character, Dictionary<string, string> modelLocations)
        {
            U8Archive arc = new(archive, ReadMode.IndexOnly);
            foreach (IArchiveFile file in arc.Root.GetFiles())
            {
                if (file.Name == targetFile)
                {
                    file.Extract($@"{MainWindow.TemporaryDirectory}\playerXNOs\{file.Name}");
                    modelLocations.Add(character, $@"{MainWindow.TemporaryDirectory}\playerXNOs\{file.Name}");
                }
            }
            return modelLocations;
        }

        // Fucking terrible hacks to load and save a Criware Acroarts MAB File's file table.
        // This is all awful and really needs to be replaced with a proper implementation in Marathon.
        public static CriwareAcroarts LoadMAB(string mabFile)
        {
            CriwareAcroarts mab = new();

            BinaryReaderEx reader = new(File.OpenRead(mabFile), true);

            reader.JumpTo(0x4);
            uint mrabChunkSize = reader.ReadUInt32();
            reader.JumpTo(0x34);
            mab.ABRSOffset = reader.ReadUInt32() + mrabChunkSize + 0x20;
            reader.JumpTo(0x00);

            mab.Bytes = reader.ReadBytes((int)mab.ABRSOffset);

            uint storedOffset = (uint)reader.BaseStream.Position;
            reader.Offset = storedOffset;

            reader.ReadSignature(4, "ABRS");
            uint dataLength = reader.ReadUInt32(); // Length of the data in this chunk from the first sub(?) chunk.
            uint chunkSize = reader.ReadUInt32();
            uint UnknownUInt32_1 = reader.ReadUInt32(); // Always zero.

            uint UnknownUInt32_2 = reader.ReadUInt32(); // Always hex 77 91 73 25.
            uint OffsetCount = reader.ReadUInt32();
            uint OffsetTableOffset = reader.ReadUInt32(); // At least I think it it's an offset table? Chunk type is POF0, like Ninja's NOF0. Seems more like a BINA Offset Table than a Ninja one.

            for (int i = 0; i < OffsetCount; i++)
            {
                CriwareFileRef file = new();

                uint OffsetEntryOffset = reader.ReadUInt32();
                file.OffsetPadding = reader.ReadUInt32(); // Frequently hex FF FF FF FF, but not always.

                long pos = reader.BaseStream.Position;
                reader.JumpTo(OffsetEntryOffset, true);

                reader.Offset = (uint)reader.BaseStream.Position;
                file.ID = new(reader.ReadChars(0x04)); // Seems to be the first four bytes of the target file?
                uint chunkLength = reader.ReadUInt32(); // Not including chunk header.
                uint chunkOffset = reader.ReadUInt32();
                uint unknownChunkData = reader.ReadUInt32(); // Always hex 00 40 00 00.

                reader.JumpTo(chunkOffset, true);
                file.Name = reader.ReadNullTerminatedString();
                mab.Files.Add(file);

                reader.Offset = storedOffset;
                reader.JumpTo(pos);
            }

            reader.JumpTo(OffsetTableOffset, true);
            reader.JumpAhead(0x4);
            mab.FooterLength = reader.ReadUInt32();
            reader.JumpBehind(0x8);

            mab.EndBytes = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

            reader.Dispose();
            reader.Close();

            return mab;
        }

        public static void SaveMAB(CriwareAcroarts mab, string filepath)
        {
            BinaryWriterEx writer = new(File.Create(filepath), true);
            writer.Offset = mab.ABRSOffset;

            writer.Write(mab.Bytes);

            long arbsSize = writer.BaseStream.Position;
            writer.Write("ABRS");
            long arbsDataSizePos = writer.BaseStream.Position;
            writer.Write("SIZE");
            long arbsSizePos = writer.BaseStream.Position;
            writer.Write("SIZE");
            writer.WriteNulls(0x4);
            writer.Write((byte)0x77);
            writer.Write((byte)0x91);
            writer.Write((byte)0x73);
            writer.Write((byte)0x25);
            writer.Write(mab.Files.Count);
            writer.AddOffset("offsettable");

            for (int i = 0; i < mab.Files.Count; i++)
            {
                writer.AddOffset($"file{i}offset");
                writer.Write(mab.Files[i].OffsetPadding);
            }

            writer.FixPadding(0x10);
            arbsSize = writer.BaseStream.Position - arbsSize;

            long pos = writer.BaseStream.Position;
            writer.BaseStream.Position = arbsSizePos;
            writer.Write((uint)arbsSize);
            writer.BaseStream.Position = pos;

            uint dataSize = mab.FooterLength + 0x10;

            for (int i = 0; i < mab.Files.Count; i++)
            {
                writer.FillOffset($"file{i}offset", true);
                writer.Write(mab.Files[i].ID);
                long sizePos = writer.BaseStream.Position;
                writer.Write("SIZE");
                writer.Write(0x10);
                writer.Write((byte)0x00);
                writer.Write((byte)0x40);
                writer.Write((byte)0x00);
                writer.Write((byte)0x00);

                long chunkSize = writer.BaseStream.Position;
                if (!mab.Files[i].Name.Contains('@'))
                    writer.WriteNullTerminatedString(mab.Files[i].Name);
                else
                {
                    mab.Files[i].Name = mab.Files[i].Name.Replace("@", "");
                    writer.WriteNullPaddedString(mab.Files[i].Name, 0x21);
                }
                writer.FixPadding(0x10);
                chunkSize = writer.BaseStream.Position - chunkSize;

                dataSize += (uint)chunkSize + 0x10;

                pos = writer.BaseStream.Position;
                writer.BaseStream.Position = sizePos;
                writer.Write((uint)chunkSize);
                writer.BaseStream.Position = pos;
            }

            writer.FillOffset("offsettable", true);
            writer.Write(mab.EndBytes);

            writer.BaseStream.Position = arbsDataSizePos;
            writer.Write(dataSize);

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Cheaply retargets an animation from one model to another.
        /// </summary>
        /// <param name="xnm">The animation to retarget.</param>
        /// <param name="srcXNO">The model this animation is originally from.</param>
        /// <param name="tgtXNO">The model this animation should be retargeted onto.</param>
        public static void RetargetAnimation(string xnm, string srcXNO, string tgtXNO)
        {
            // Load the XNM and XNOs.
            NinjaNext anim = new(xnm);
            NinjaNext srcMdl = new(srcXNO);
            NinjaNext tgtMdl = new(tgtXNO);

            // Create a list to store the Sub Motions that use a node that doesn't exist on the target model.
            List<int> Unused = new();

            // Loop through all the Sub Motions in the animation.
            for (int s = 0; s < anim.Data.Motion.SubMotions.Count; s++)
            {
                // Set a flag to tell if we've retargeted this Sub Motion.
                bool retargeted = false;

                // Get the name of the node this Sub Motion is for.
                string nodeName = srcMdl.Data.NodeNameList.NinjaNodeNames[anim.Data.Motion.SubMotions[s].NodeIndex];

                // Loop through the target model's Ninja Node Name List.
                for (int i = 0; i < tgtMdl.Data.NodeNameList.NinjaNodeNames.Count; i++)
                {
                    // If we've found a Node with the same name as the one from the source model, then update the Sub Motion Node Index and set our flag to true.
                    if (tgtMdl.Data.NodeNameList.NinjaNodeNames[i] == nodeName)
                    {
                        anim.Data.Motion.SubMotions[s].NodeIndex = i;
                        retargeted = true;
                    }
                }

                // If our flag is still false, mark this Sub Motion for removal.
                if (!retargeted)
                    Unused.Add(s);
            }

            // Flip the list of Sub Motions to be removed.
            Unused.Reverse();

            // Remove all the Sub Motions that have a Node Index that has been added to our list.
            foreach (int node in Unused)
                anim.Data.Motion.SubMotions.RemoveAt(node);

            // Save the retargeted animation. If a path wasn't specified, just use the original with a .retargeted extension tacked on.
            anim.Save(xnm);
        }

        // https://stackoverflow.com/questions/6215185/getting-length-of-video
        public static async Task<string> GetWMVDuration(string wmvFile)
        {
            var mediaDet = (IMediaDet)new MediaDet();
            DsError.ThrowExceptionForHR(mediaDet.put_Filename(wmvFile));

            // find the video stream in the file
            int index;
            var type = Guid.Empty;
            for (index = 0; index < 1000 && type != MediaType.Video; index++)
            {
                mediaDet.put_CurrentStream(index);
                mediaDet.get_StreamType(out type);
            }

            // retrieve some measurements from the video
            double frameRate;
            mediaDet.get_FrameRate(out frameRate);

            double mediaLength;
            mediaDet.get_StreamLength(out mediaLength);
            var frameCount = (int)(frameRate * mediaLength);
            float duration = (float)(frameCount / frameRate);


            // https://stackoverflow.com/questions/73160913/float-to-hexadecimal-using-bitconverter-in-c-sharp
            byte[] ba = BitConverter.GetBytes(duration - 3f);

            StringBuilder sb = new();

            for (int i = ba.Length - 1; i >= 0; i--)
                sb.Append($"{ba[i].ToString("X2")} ");

            sb.Length--;

            return sb.ToString();
        }
    }

    // https://stackoverflow.com/questions/36845430/persistent-hashcode-for-strings/36845864#36845864
    // Needed as the old GetHashCode() function I used on the NET Framework versions doesn't work for seeding any more as it salts the hash.
    public static class StringExtensionMethods
    {
        public static int GetStableHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
