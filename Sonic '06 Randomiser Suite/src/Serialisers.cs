using System;
using System.IO;
using System.Linq;
using Unify.Messenger;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sonic_06_Randomiser_Suite.Serialisers
{
    class Mods
    {
        public static string Create(string seed) {
            string safeTitle = Literal.UseSafeFormattedCharacters($"Sonic '06 Randomised ({seed})");
            string newPath = Path.Combine(Properties.Settings.Default.Path_ModsDirectory, safeTitle);

            if (Directory.Exists(newPath)) {
                DialogResult overwrite = UnifyMessenger.UnifyMessage.ShowDialog($"The seed '{seed}' has already been generated. Do you want to overwrite it?",
                                                                                "I/O Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (overwrite == DialogResult.No) return string.Empty;
            }

            Directory.CreateDirectory(newPath);

            using (Stream configCreate = File.Open(Path.Combine(newPath, "mod.ini"), FileMode.Create))
                using (StreamWriter configInfo = new StreamWriter(configCreate)) {
                    configInfo.WriteLine("[Details]");
                    configInfo.WriteLine($"Title=\"Sonic '06 Randomised\"");
                    configInfo.WriteLine($"Version=\"{seed}\"");
                    configInfo.WriteLine($"Date=\"{DateTime.Now}\"");
                    configInfo.WriteLine($"Author=\"Sonic '06 Randomiser Suite\"");
                    configInfo.WriteLine($"Platform=\"{Literal.System(Properties.Settings.Default.Path_GameExecutable)}\"");

                    configInfo.WriteLine("\n[Filesystem]");
                    configInfo.WriteLine($"Merge=\"False\"");
                    configInfo.WriteLine($"CustomFilesystem=\"False\"");

                    configInfo.Close();
                }

            return newPath;
        }

        public static string CreateFolderDynamically(string modDirectory, string extensions, string system) {
            if (system == "Xbox 360") {
                Directory.CreateDirectory(Path.Combine(modDirectory, $"xenon\\{extensions}"));
                return Path.Combine(modDirectory, $"xenon\\{extensions}");
            } else if (system == "PlayStation 3") {
                Directory.CreateDirectory(Path.Combine(modDirectory, $"ps3\\{extensions}"));
                return Path.Combine(modDirectory, $"ps3\\{extensions}");
            } else
                return string.Empty;
        }
    }

    class Literal
    {
        /// <summary>
        /// Translates a file extension to 'Xbox 360' or 'PlayStation 3'
        /// </summary>
        public static string System(string path) {
            if (Path.GetExtension(path).ToLower() == ".xex") return "Xbox 360";
            if (Path.GetExtension(path).ToLower() == ".bin") return "PlayStation 3";
            else return "unspecified";
        }

        /// <summary>
        /// Removes illegal characters from the path in a cleaner format.
        /// </summary>
        public static string UseSafeFormattedCharacters(string text) {
            return text.Replace(@"\", "")
                       .Replace("/",  "-")
                       .Replace(":",  "-")
                       .Replace("*",  "")
                       .Replace("?",  "")
                       .Replace("\"", "'")
                       .Replace("<",  "")
                       .Replace(">",  "")
                       .Replace("|",  "");
        }
    }

    class Resources
    {
        /// <summary>
        /// Splits a line broken resource into a string array.
        /// </summary>
        /// <param name="document">Text resource.</param>
        public static string[] ParseLineBreaks(string document) {
            return document.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None );
        }
    }

    class Paths
    {
        /// <summary>
        /// Returns the first directory of a path.
        /// </summary>
        public static string GetRootFolder(string path) {
            while (true) {
                string temp = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(temp))
                    break;
                path = temp;
            }
            return path;
        }

        /// <summary>
        /// Returns the full path without an extension.
        /// </summary>
        public static string GetPathWithoutExtension(string path) {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        /// <summary>
        /// Returns the folder containing the file.
        /// </summary>
        public static string GetContainingFolder(string path) {
            return Path.GetFileName(Path.GetDirectoryName(path));
        }

        /// <summary>
        /// Returns if the directory is empty.
        /// </summary>
        public static bool IsDirectoryEmpty(string path) {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        /// <summary>
        /// Checks if the path is valid and exists.
        /// </summary>
        public static bool CheckPathLegitimacy(string path) {
            if (Directory.Exists(path) && path != string.Empty) return true;
            else return false;
        }

        /// <summary>
        /// Checks if the path is valid and exists.
        /// </summary>
        public static bool CheckFileLegitimacy(string path) {
            if (File.Exists(path) && path != string.Empty) return true;
            else return false;
        }

        /// <summary>
        /// Returns a new path with the specified filename.
        /// </summary>
        public static string ReplaceFilename(string path, string newFile) {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(newFile));
        }

        /// <summary>
        /// Collects the general file structure for '06 data.
        /// </summary>
        public static List<string> CollectGameData(string path) {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(s => Path.GetExtension(s) == ".arc").ToList();
        }

        /// <summary>
        /// Compares two strings to check if one is a subdirectory of the other.
        /// </summary>
        public static bool IsSubdirectory(string candidate, string other) {
            var isChild = false;

            try {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (candidateInfo.Parent != null) {
                    if (candidateInfo.Parent.FullName == otherInfo.FullName) {
                        isChild = true;
                        break;
                    } else candidateInfo = candidateInfo.Parent;
                }
            } catch (Exception ex) {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss tt}] [Error] Failed to check directories...\n{ex}");
            }

            return isChild;
        }
    }
}
