using System;
using System.IO;
using System.Collections.Generic;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    class Textures
    {
        public static void RandomiseTextures(string folderPath, Random rng) {
            List<string> availableTextures = new List<string>();
            List<int> usedNumbers = new List<int>();
            int index;

            // Extract texture names
            foreach (string ddsData in Directory.GetFiles(folderPath, $"*.dds", SearchOption.AllDirectories)) {
                availableTextures.Add(ddsData);
                File.Move(ddsData, Paths.ReplaceFilename(ddsData, $"temp-{Path.GetFileName(ddsData)}"));
            }

            // Copy textures
            foreach (string ddsData in availableTextures) {
                Console.WriteLine($"Source texture: {ddsData}");

                index = rng.Next(availableTextures.Count);
                if (usedNumbers.Contains(index)) {
                    do { index = rng.Next(availableTextures.Count); }
                    while (usedNumbers.Contains(index));
                }
                usedNumbers.Add(index);

                Console.WriteLine($"Target texture: {availableTextures[index]}");
                File.Move(Paths.ReplaceFilename(ddsData, $"temp-{Path.GetFileName(ddsData)}"), availableTextures[index]);
            }
        }
    }
}
