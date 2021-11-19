using Marathon.Formats.Package;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class AnimationRandomisers
    {
        /// <summary>
        /// Shuffles the XNMs called for character animations in a PKG file.
        /// </summary>
        /// <param name="pkgFile">The PKG file to process.</param>
        public static async Task GameplayAnimationRandomiser(string pkgFile)
        {
            // Set up a couple of lists.
            List<string> XNMFiles = new();
            List<int> usedNumbers = new();

            // Load the PKG.
            AssetPackage pkg = new(pkgFile);

            // Loop through the PKGs to find the Motion Category.
            foreach (var type in pkg.Types)
            {
                if (type.Name == "motion")
                {
                    // Loop through the entries to find the XNM files.
                    foreach (AssetFile? entry in type.Files)
                    {
                        // Only add this entry's XNM if it meets these criteria (as XNMs under these tend to be for faces or Omega's body parts)
                        if (!entry.Name.Contains("face") && entry.Name != "style" && !entry.File.Contains("point.xnm"))
                        {
                            // Add this XNM to the list of valid files for this PKG.
                            XNMFiles.Add(entry.File);
                        }
                    }

                    // Loop through the entries again to pick new XNMs.
                    foreach (AssetFile? entry in type.Files)
                    {
                        // Same criteria check as above.
                        if (!entry.Name.Contains("face") && entry.Name != "style" && !entry.File.Contains("point.xnm"))
                        {
                            // Pick a random number from the amount of entires in the XNM list.
                            int index = MainWindow.Randomiser.Next(XNMFiles.Count);

                            // If the selected number is already used, pick another until it isn't.
                            if (usedNumbers.Contains(index))
                            {
                                do { index = MainWindow.Randomiser.Next(XNMFiles.Count); }
                                while (usedNumbers.Contains(index));
                            }

                            // Add this number to the usedNumbers list so we can't pull the same XNM twice.
                            usedNumbers.Add(index);

                            // Set this entry's XNM to the selected one.
                            entry.File = XNMFiles[index];
                        }
                    }
                }
            }

            // Save the updated PKG.
            pkg.Save();
        }

        /// <summary>
        /// Randomises the XNMs for each event.
        /// </summary>
        /// <param name="archivePath">The path to the extracted event_data.arc.</param>
        /// <param name="character">The character to shuffle the animations of.</param>
        /// <param name="type">The type of animation to shuffle (Root or evf_head).</param>
        /// <param name="camera">Whether or not we're randomising cameras instead.</param>
        /// <returns></returns>
        public static async Task EventAnimationRandomiser(string archivePath, string character, string type, bool camera = false)
        {
            // Set up a list of numbers.
            List<int> usedNumbers = new();

            // Get the XNMs that fit our search criteria.
            string[] XNMFiles = Directory.GetFiles(archivePath, $"*_{character}_{type}*.xnm", SearchOption.AllDirectories);

            if (camera)
                XNMFiles = Directory.GetFiles(archivePath, "*.xnd", SearchOption.AllDirectories);

            // Loop through each XNM and rename it to have a .rnd extension.
            for (int i = 0; i < XNMFiles.Length; i++)
            {
                File.Move(XNMFiles[i], $"{XNMFiles[i]}.rnd");
                XNMFiles[i] = $"{XNMFiles[i]}.rnd";
            }

            // Loop through again for the actual randomisation.
            for (int i = 0; i < XNMFiles.Length; i++)
            {
                // Pick a random number from the amount of entires in the XNM list.
                int index = MainWindow.Randomiser.Next(XNMFiles.Length);

                // If the selected number is already used, pick another until it isn't.
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(XNMFiles.Length); }
                    while (usedNumbers.Contains(index));
                }

                // Add this number to the usedNumbers list so we can't pull the same XNM twice.
                usedNumbers.Add(index);

                // Copy this XNM to the name of the chosen one, minus our .rnd addition.
                File.Copy(XNMFiles[i], $"{XNMFiles[index].Remove(XNMFiles[index].LastIndexOf('.'))}");

                // Delete the .rnd version.
                File.Delete(XNMFiles[i]);
            }
        }
    }
}
