using Marathon.Formats.Archive;
using Marathon.Formats.Mesh.Ninja;
using Marathon.Formats.Package;
using Marathon.Helpers;

namespace MarathonRandomiser
{
    internal class AnimationRandomiser
    {
        /// <summary>
        /// Shuffles the XNMs called for character animations in a PKG file.
        /// TODO: Consider refactoring this, it's a bit sloppy right now.
        /// </summary>
        /// <param name="pkgFile">The PKG file to process.</param>
        /// <param name="GameExecutable">Location of the game executable (used to find the player archives).</param>
        /// <param name="useAll">Whether or not we're using the XNMs rather than the entries in the PKGs.</param>
        /// <param name="useEvents">Whether or not to include the event animations too.</param>
        public static async Task GameplayAnimationRandomiser(string pkgFile, string GameExecutable, bool? useAll = false, bool? useEvents = false)
        {
            string targetArchive = null;
            string neededPart = null;

            // Get a list of all the player archives.
            string[] win32Arcs = Directory.GetFiles(Path.GetDirectoryName(GameExecutable), "player_*.arc", SearchOption.AllDirectories);

            // Get the name of this PKG (surely this way is me having a brainfart?)
            string pkgName = pkgFile[(pkgFile.LastIndexOf('\\') + 1)..];
            pkgName = Path.GetFileNameWithoutExtension(pkgName);

            // Split the name of this PKG to figure out what we're looking for.
            string[] pkgParts = pkgName.Split('_');

            // Find the archive and pkg name.
            foreach (string pkgPart in pkgParts)
            {
                foreach (string arc in win32Arcs)
                {
                    if (arc.Contains($"_{pkgPart}"))
                    {
                        targetArchive = arc;
                        neededPart = pkgPart;
                    }
                }
            }

            // Special case for the snowboards to use Sonic's archive.
            if (pkgFile.Contains("snow_board"))
                targetArchive = $@"{Path.GetDirectoryName(GameExecutable)}\win32\archives\player_sonic.arc";

            // Exclude Elise for now.
            if (Path.GetFileName(pkgFile) == "princess.pkg" || Path.GetFileName(pkgFile) == "princess_hair.pkg" || Path.GetFileName(pkgFile) == "princess_princess.pkg")
                targetArchive = null;
            

            // If we're not using all, then just invalidate targetArchive.
            if (useAll == false)
            {
                targetArchive = null;
            }

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
                    // If we're not using all the XNMs (or we haven't found an archive for them) then find them from the PKG entries.
                    if (targetArchive == null)
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
                    }

                    // If we are using all the XNMs, then find them from the player's archive.
                    else if (Path.GetFileName(pkgFile) != "princess_hair.pkg")
                    {
                        // Load the archive.
                        U8Archive arc = new(targetArchive, Marathon.IO.ReadMode.IndexOnly);

                        // Get a list of all the files in the archive.
                        IEnumerable<Marathon.IO.Interfaces.IArchiveFile>? arcFiles = arc.Root.GetFiles();

                        // Loop through the files.
                        foreach (Marathon.IO.Interfaces.IArchiveFile? xnmFile in arcFiles)
                        {
                            // Check if this is an XNM and it's not one we're classing as forbidden.
                            if (Path.GetExtension(xnmFile.Name) == ".xnm" && !xnmFile.Name.Contains("_style") && !xnmFile.Name.Contains("_Head") && !xnmFile.Name.Contains("_face") && !xnmFile.Name.Contains("_point"))
                            {
                                // Add it to the valid files for the PKG in a probably stupid way.
                                string probablyDumbWayToGetThePlayerXNMPath = xnmFile.Path[(xnmFile.Path.IndexOf('/') + 1)..];
                                XNMFiles.Add(probablyDumbWayToGetThePlayerXNMPath[(probablyDumbWayToGetThePlayerXNMPath.IndexOf('/') + 1)..]);
                            }
                        }
                    }

                    // If we're using Event XMAs, then include them too.
                    if (useEvents == true && neededPart != null)
                    {
                        // Load event.arc.
                        U8Archive eventArc = new($@"{Path.GetDirectoryName(GameExecutable)}\win32\archives\event_data.arc", Marathon.IO.ReadMode.IndexOnly);

                        // Get a list of all the files in the archive.
                        IEnumerable<Marathon.IO.Interfaces.IArchiveFile>? arcFiles = eventArc.Root.GetFiles();

                        // Loop through the files.
                        foreach (Marathon.IO.Interfaces.IArchiveFile? xnmFile in arcFiles)
                        {
                            if (Path.GetExtension(xnmFile.Name) == ".xnm" && xnmFile.Name.Contains(neededPart) && xnmFile.Name.Contains("_Root"))
                            {
                                // Add it to the valid files for the PKG in a probably stupid way.
                                string probablyDumbWayToGetThePlayerXNMPath = xnmFile.Path[(xnmFile.Path.IndexOf('/') + 1)..];
                                XNMFiles.Add(probablyDumbWayToGetThePlayerXNMPath[(probablyDumbWayToGetThePlayerXNMPath.IndexOf('/') + 1)..]);
                            }
                        }
                    }

                    // Loop through the entries again to pick new XNMs.
                    foreach (AssetFile? entry in type.Files)
                    {
                        // Same criteria check as above.
                        if (!entry.Name.Contains("face") && entry.Name != "style" && !entry.File.Contains("point.xnm"))
                        {
                            if (usedNumbers.Count == XNMFiles.Count)
                                usedNumbers.Clear();

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
    

        /// <summary>
        /// Randomises the Framerate a Ninja Motion runs at.
        /// </summary>
        /// <param name="motionFile">The Ninja Motion file to edit.</param>
        /// <param name="minFPS">How low the FPS value can be.</param>
        /// <param name="maxFPS">How high the FPS value can be.</param>
        public static async Task AnimationFramerateRandomiser(string motionFile, int minFPS, int maxFPS)
        {
            // Load the Ninja Motion File.
            NinjaNext motion = new(motionFile);

            // Generate a new framerate value between our two numbers.
            motion.Data.Motion.Framerate = MainWindow.Randomiser.Next(minFPS, maxFPS);

            // Resave the Ninja Motion File.
            motion.Save();
        }
    }
}
