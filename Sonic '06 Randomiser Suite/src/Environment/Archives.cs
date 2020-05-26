using System;
using System.IO;
using ArcPackerLib;
using System.Diagnostics;

namespace Sonic_06_Randomiser_Suite
{
    class Archives
    {
        /// <summary>
        /// Extracts an archive to a temporary location.
        /// </summary>
        public static string UnpackARC(string arc, string tempPath) {
            Console.WriteLine($"Unpacking Archive: {arc}");
            Directory.CreateDirectory(tempPath); // Create temporary location
            File.Copy(arc, Path.Combine(tempPath, Path.GetFileName(arc))); // Copy archive to temporary location

            // Extracts the archive in the temporary location
            ProcessStartInfo unpack = new ProcessStartInfo() {
                FileName = Program.arctool,
                Arguments = $"-d \"{Path.Combine(tempPath, Path.GetFileName(arc))}\"",
                WorkingDirectory = Path.GetDirectoryName(Program.arctool),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var Unpack = Process.Start(unpack);
            Unpack.WaitForExit();
            Unpack.Close();

            return tempPath;
        }

        /// <summary>
        /// Repacks an archive from a temporary location.
        /// </summary>
        public static void RepackARC(string arc, string output) {
            Console.WriteLine($"Repacking Archive: {output}");
            ArcPacker repack = new ArcPacker();
            repack.WriteArc(output, Path.Combine(arc, Path.GetFileNameWithoutExtension(output)));

            // Erases temporary repack data
            try {
                DirectoryInfo tempData = new DirectoryInfo(arc);
                if (Directory.Exists(arc)) {
                    foreach (FileInfo file in tempData.GetFiles()) file.Delete();
                    foreach (DirectoryInfo directory in tempData.GetDirectories()) directory.Delete(true);
                    Directory.Delete(arc);
                }
            } catch { }
        }

        /// <summary>
        /// Creates an archive for the randomiser
        /// </summary>
        public static void CreateModARC(string srcArchive, string origArchive, string modDirectory) {
            if (Directory.Exists(modDirectory)) {
                // Absolute file path (xenon/ps3/win32 and beyond)
                string filePath = origArchive.Remove(0, Path.GetDirectoryName(Properties.Settings.Default.Path_GameExecutable).Length).Substring(1);

                // Creates the archive subdirectories
                string createArchives = Path.Combine(modDirectory, filePath);
                Directory.CreateDirectory(Path.GetDirectoryName(createArchives));

                if (createArchives != string.Empty)
                    RepackARC(srcArchive, createArchives);
            }
        }
    }
}
