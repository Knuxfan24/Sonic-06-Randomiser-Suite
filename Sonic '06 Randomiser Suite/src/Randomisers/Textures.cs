using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    class Textures
    {
        /// <summary>
        /// Randomises all textures in a directory
        /// </summary>
        public static async void RandomiseTextures(string folderPath, bool customTexturesOnly, bool allowForbidden, Random rng) {
            List<string> forbiddenTags = new List<string>() {
                "_sdw_", "_lm_", "envmap"
            };

            List<string> availableTextures = new List<string>();
            List<int> usedNumbers = new List<int>();
            int index;

            if (allowForbidden) forbiddenTags.Clear();

            // Extract texture names
            foreach (string ddsData in Directory.GetFiles(folderPath, $"*.dds", SearchOption.AllDirectories)) {
                // Add texture names to list if they're not forbidden - only move when necessary
                if (!forbiddenTags.Any(Path.GetFileName(ddsData).Contains)) {
                    availableTextures.Add(ddsData);
                    if (!customTexturesOnly) File.Move(ddsData, Paths.ReplaceFilename(ddsData, $"temp-{Path.GetFileName(ddsData)}"));
                }
            }

            // Copy textures
            if (!customTexturesOnly)
                foreach (string ddsData in availableTextures)
                {
                    Console.WriteLine($"Randomising Texture: {ddsData}");

                    index = rng.Next(availableTextures.Count);
                    if (usedNumbers.Contains(index)) {
                        do { index = rng.Next(availableTextures.Count); }
                        while (usedNumbers.Contains(index));
                    }
                    usedNumbers.Add(index);

                    File.Move(Paths.ReplaceFilename(ddsData, $"temp-{Path.GetFileName(ddsData)}"), availableTextures[index]);
                }

            // Copy custom textures
            foreach (string textureData in Main.Visual_Custom_Textures)
            {
                string texture = availableTextures[rng.Next(availableTextures.Count)];
                Console.WriteLine($"Randomising Custom Texture: {texture}");

                // Copy texture to pre-allocated slot
                File.Copy(textureData, texture, true);
            }
        }
    }
}
