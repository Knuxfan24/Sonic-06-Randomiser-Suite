using Marathon.IO.Formats.Archives;
using System.IO;

namespace Sonic_06_Randomiser_Suite
{
    class ArchiveHandler
    {
        /// <summary>
        /// Unpacks a U8 Archive to a folder.
        /// </summary>
        /// <param name="archive">The archive to unpack.</param>
        /// <param name="archivelocation">The location the archive is in (to grab the xenon/ps3/win32 path).</param>
        /// <returns></returns>
        public static string UnpackArchive(string archive, string archivelocation)
        {
            // Check if the archive is already extracted to the temporary directory. If not, then unpack it.
            if (!Directory.Exists($@"{Program.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(archivelocation, "")}"))
            {
                System.Console.WriteLine($@"Unpacking '{archive}'.");
                U8Archive arc = new(archive);
                arc.Extract($@"{Program.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(archivelocation, "")}");
            }

            // Return the path to the unpacked archive as a string.
            return $@"{Program.TemporaryDirectory}{archive.Substring(0, archive.Length - 4).Replace(archivelocation, "")}";
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
    }
}
