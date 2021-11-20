using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class TextureRandomiser
    {
        /// <summary>
        /// Fetch all the valid textures in the specified archive and adds them to a list for later use.
        /// </summary>
        /// <param name="Textures">The list of valid textures to update.</param>
        /// <param name="archivePath">The path to the current archive.</param>
        public static async Task<List<string>> FetchTextures(List<string> Textures, string archivePath)
        {
            // Find all the DDS texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds", SearchOption.AllDirectories);

            // Loop through each DDS.
            for (int i = 0; i < ddsFiles.Length; i++)
            {
                // If it's not a shadow, light or cubemap, or a font, then throw a .rnd extension on and add it to the list.
                if (!ddsFiles[i].Contains("_sdw_") && !ddsFiles[i].Contains("_lm_") && !ddsFiles[i].Contains("envmap") && !ddsFiles[i].Contains("suna_cube") && !ddsFiles[i].Contains("cubemap") && !ddsFiles[i].Contains("font"))
                {
                    File.Move(ddsFiles[i], $"{ddsFiles[i]}.rnd");
                    Textures.Add($"{ddsFiles[i]}.rnd");
                }
            }

            // Return the update texture list.
            return Textures;
        }

        /// <summary>
        /// Shuffles textures between archives.
        /// </summary>
        /// <param name="usedNumbers">A list showing what textures have already being used.</param>
        /// <param name="archivePath">The path to the current archive.</param>
        /// <param name="Textures">The list of valid textures.</param>
        public static async Task<List<int>> ShuffleTextures(List<int> usedNumbers, string archivePath, List<string> Textures)
        {
            // Find all the previously determined as valid texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds.rnd", SearchOption.AllDirectories);

            // Loop through the textures.
            for (int i = 0; i < ddsFiles.Length; i++)
            {
                // Pick a random number from the amount of entires in the Textures list.
                int index = MainWindow.Randomiser.Next(Textures.Count);

                // If the selected number is already used, pick another until it isn't.
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(Textures.Count); }
                    while (usedNumbers.Contains(index));
                }

                // Add this number to the usedNumbers list so we can't pull the same DDS file twice.
                usedNumbers.Add(index);

                // Copy this DDS to the name of the chosen one, minus our .rnd addition.
                File.Copy(Textures[index], $"{ddsFiles[i].Remove(ddsFiles[i].LastIndexOf('.'))}");
            }

            // Return our updated list of used numbers.
            return usedNumbers;
        }

        /// <summary>
        /// Randomises textures per archive.
        /// </summary>
        /// <param name="archivePath">The path to the current archive.</param>
        public static async Task PerArchive(string archivePath)
        {
            // Set up a list of numbers.
            List<int> usedNumbers = new();

            // Find all the DDS texture files in this archive.
            string[] ddsFiles = Directory.GetFiles(archivePath, "*.dds", SearchOption.AllDirectories);

            // Set up a new list to fill with valid DDS files.
            List<string> validDDSFiles = new();

            for (int i = 0; i < ddsFiles.Length; i++)
            {
                // If it's not a shadow, light or cubemap, or a font, then throw a .rnd extension on and add it to the list.
                if (!ddsFiles[i].Contains("_sdw_") && !ddsFiles[i].Contains("_lm_") && !ddsFiles[i].Contains("envmap") && !ddsFiles[i].Contains("suna_cube") && !ddsFiles[i].Contains("cubemap") && !ddsFiles[i].Contains("font"))
                {
                    File.Move(ddsFiles[i], $"{ddsFiles[i]}.rnd");
                    validDDSFiles.Add($"{ddsFiles[i]}.rnd");
                }
            }

            // Loop through again for the actual randomisation, based on the new list..
            for (int i = 0; i < validDDSFiles.Count; i++)
            {
                // Pick a random number from the amount of entires in the DDS list.
                int index = MainWindow.Randomiser.Next(validDDSFiles.Count);

                // If the selected number is already used, pick another until it isn't.
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(validDDSFiles.Count); }
                    while (usedNumbers.Contains(index));
                }

                // Add this number to the usedNumbers list so we can't pull the same DDS file twice.
                usedNumbers.Add(index);

                // Copy this DDS to the name of the chosen one, minus our .rnd addition.
                File.Copy(validDDSFiles[i], $"{validDDSFiles[index].Remove(validDDSFiles[index].LastIndexOf('.'))}");
            }
        }
    }
}
