using Marathon.IO.Formats.Archives;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace SonicNextRandomiser
{
    internal class Helpers
    {
        /// <summary>
        /// Fills out the custom CheckedListBox elements from text files containing comma seperated values.
        /// </summary>
        /// <param name="file">Text file containing comma seperated values.</param>
        /// <param name="listBox">The CheckedListBox element to populate.</param>
        public static void FillOutCheckList(string file, CheckedListBox listBox)
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
        /// Parses the tag value for the checked CheckBox elements in a specified CheckedListBox element into a string list.
        /// </summary>
        /// <param name="list">The CheckedListBox element to parse.</param>
        /// <returns>A string list containing the tag values in the checked CheckBox elements.</returns>
        public static List<string> CheckListParse(CheckedListBox list)
        {
            // Create the list to return.
            List<string> result = new();

            // Loop through the items in the CheckedListBox and add their tag value to the string list if they are checked.
            foreach (CheckedListBoxItem item in list.Items)
                if (item.Checked)
                    result.Add(item.Tag);

            // Return the list.
            return result;
        }

        /// <summary>
        /// Replaces any characters that are illegal in Windows filenames with an underscore.
        /// </summary>
        /// <param name="text">The string to replace.</param>
        /// <returns>The string with any illegal characters replaced with an underscore.</returns>
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
        /// Unpacks a U8 Archive to a folder.
        /// </summary>
        /// <param name="archive">The archive to unpack.</param>
        /// <param name="archivelocation">The location the archive is in (to grab the xenon/ps3/win32 path).</param>
        /// <returns></returns>
        public static string UnpackArchive(string archive, string archivelocation)
        {
            // Check if the archive is already extracted to the temporary directory. If not, then unpack it.
            if (!Directory.Exists($@"{MainWindow.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(archivelocation, "")}"))
            {
                System.Console.WriteLine($@"Unpacking '{archive}'.");
                U8Archive arc = new(archive);
                arc.Extract($@"{MainWindow.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(archivelocation, "")}");
            }

            // Return the path to the unpacked archive as a string.
            return $@"{MainWindow.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(archivelocation, "")}";
        }

        /// <summary>
        /// Repacks a folder to an Uncompressed U8 Archive.
        /// </summary>
        /// <param name="path">The folder to pack.</param>
        /// <param name="folderStructure">The location to save the packed archive to.</param>
        public static void RepackArchive(string path, string folderStructure)
        {
            System.Console.WriteLine($@"Repacking '{path}.arc'.");

            // Ensure the folder we want to place the archive in actually exists.
            if (!Directory.Exists(folderStructure.Remove(folderStructure.LastIndexOf('\\') + 1)))
                Directory.CreateDirectory(folderStructure.Remove(folderStructure.LastIndexOf('\\') + 1));

            // Create and save the archive.
            U8Archive arc = new()
            {
                CompressionLevel = System.IO.Compression.CompressionLevel.NoCompression,
                ArchiveStreamMode = ArchiveStreamMode.CopyToMemory
            };

            ArchiveDirectory root = new()
            {
                IsRoot = true
            };

            arc.AddDirectory(path, root, includeSubDirectories: true);
            arc.Data.Add(root);
            arc.Save(folderStructure + ".arc");
        }

        /// <summary>
        /// Uses unlub to decompile a lua binary.
        /// </summary>
        /// <param name="luaFile">The path to the lua file to decompile</param>
        public static void DecompileLua(string luaFile)
        {
            // Read the lua file into a string array.
            string[] readText = File.ReadAllLines(luaFile);

            // Check if the file starts with the string "LuaP". While not exclusive to compiled lua binaries, it's a good enough indication that it needs decompiling.
            if (readText[0].Contains("LuaP"))
            {
                // Run unlub.jar through a Java process to decompile it.
                using (Process process = new())
                {
                    process.StartInfo.FileName = "java.exe";
                    process.StartInfo.Arguments = $"-jar \"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\ExternalResources\\unlub.jar\" \"{luaFile}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    // Grab unlub's output so we can write it back to a .lub file.
                    StringBuilder outputBuilder = new StringBuilder();
                    process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();

                    // Write unlub's output to the original file passed to this function.
                    File.WriteAllText(luaFile, outputBuilder.ToString());
                }
            }
        }
    }
}
