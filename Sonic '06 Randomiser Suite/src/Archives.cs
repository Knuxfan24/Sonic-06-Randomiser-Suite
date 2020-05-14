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
            Directory.CreateDirectory(tempPath); // Create temporary location
            File.Copy(arc, Path.Combine(tempPath, Path.GetFileName(arc))); // Copy archive to temporary location

            // Extracts the archive in the temporary location
            ProcessStartInfo unpack = new ProcessStartInfo() {
                FileName = Program.Arctool,
                Arguments = $"-d \"{Path.Combine(tempPath, Path.GetFileName(arc))}\"",
                WorkingDirectory = Path.GetDirectoryName(Program.Arctool),
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
    }
}
