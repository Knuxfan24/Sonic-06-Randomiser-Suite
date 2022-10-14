using Marathon.Formats.Archive;
using Marathon.Formats.Mesh.Ninja;
using Marathon.Formats.Package;
using Marathon.Helpers;
using System.Diagnostics;
using System.Linq;

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

            // Get the name of this PKG (surely this way is me having a brainfart?)
            string pkgName = pkgFile[(pkgFile.LastIndexOf('\\') + 1)..];
            pkgName = Path.GetFileNameWithoutExtension(pkgName);

            // Find the player's archive and pkg name after splitting the pkgName.
            foreach (string pkgPart in pkgName.Split('_'))
            {
                foreach (string arc in Directory.GetFiles(Path.GetDirectoryName(GameExecutable), "player_*.arc", SearchOption.AllDirectories))
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
                targetArchive = null;

            // Set up a couple of lists.
            List<string> XNMFiles = new();
            List<int> usedNumbers = new();

            // Load the PKG.
            AssetPackage pkg = new(pkgFile);

            // Loop through the PKGs to find the Motion Category.
            foreach (AssetType? type in pkg.Types)
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

                        // Loop through all the files in the archive.
                        foreach (Marathon.IO.Interfaces.IArchiveFile? xnmFile in arc.Root.GetFiles())
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

                        // Loop through the files in the archive.
                        foreach (Marathon.IO.Interfaces.IArchiveFile? xnmFile in eventArc.Root.GetFiles())
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

        /// <summary>
        /// Removes any submotions in an event XNM if the Randomiser's chance value is met.
        /// </summary>
        /// <param name="archivePath">The path to the extracted event_data.arc.</param>
        /// <param name="chance">How likely it is for the randomiser to actually erase this XNM's animations.</param>
        public static async Task AssertDominance(string archivePath, int chance)
        {
            // Get the XNM files, if we're processing event_data.arc, then only get ones with a _Root name.
            string[] xnmFiles = Directory.GetFiles(archivePath, "*.xnm", SearchOption.AllDirectories);
            if (archivePath.Contains("event_data"))
                xnmFiles = Directory.GetFiles(archivePath, "*_Root.xnm", SearchOption.AllDirectories);

            // Loop through each XNM.
            foreach (string xnmFile in xnmFiles)
            {
                // Skip Silver's style animation and Elise's hair animations, as they crash the game.
                if (Path.GetFileName(xnmFile) == "silver_style_Root.xnm" || Path.GetFileName(xnmFile).Contains("_hair"))
                    continue;

                // Check if the randomiesr generates a number lower than or equal to our chance number. Continue if it does.
                if (MainWindow.Randomiser.Next(0, 101) <= chance)
                {
                    // Load the XNM.
                    NinjaNext xnm = new(xnmFile);

                    // Loop backwards through the submotions and remove ones that aren't animating Node 0, 1 or 2.
                    for (int i = xnm.Data.Motion.SubMotions.Count - 1; i >= 0; i--)
                        if (xnm.Data.Motion.SubMotions[i].NodeIndex is not 0 and not 1 and not 2)
                            xnm.Data.Motion.SubMotions.RemoveAt(i);

                    // Resave the XNM.
                    xnm.Save();
                }
            }
        }

        public static async Task RetargetAnimations(string eventID, string srcCharacter, Dictionary<string, string> models, CriwareFileRef file, bool? enforceNewModel)
        {
            // Store the original file name.
            string originalFile = file.Name;

            // Set up a goto so we can jump back.
            retry:

            // Get a model value.
            int value = MainWindow.Randomiser.Next(models.Count);

            // Set the model name.
            switch (value)
            {
                case 0: file.Name = "enemy/firstmefiress/en_fmef_Root.xno"; break;
                case 1: file.Name = "event/eventobj/eggman/ch_eggman.xno"; break;
                case 2: file.Name = "event/eventobj/evilshadow/evilshadow_Root.xno"; break;
                case 3: file.Name = "event/eventobj/maidmaster/ch_maidmaster.xno"; break;
                case 4: file.Name = "event/eventobj/princess/ev_princess01.xno"; break;
                case 5: file.Name = "event/eventobj/princess_child/ch_childelis.xno"; break;
                case 6: file.Name = "event/eventobj/soleana/ch_soleana.xno"; break;
                case 7: file.Name = "player/amy/amy_Root.xno"; break;
                case 8: file.Name = "player/blaze/blaze_Root.xno"; break;
                case 9: file.Name = "player/knuckles/knuckles_Root.xno"; break;
                case 10: file.Name = "player/omega/omega_Root.xno"; break;
                case 11: file.Name = "player/rouge/rouge_Root.xno"; break;
                case 12: file.Name = "player/shadow/shadow_Root.xno"; break;
                case 13: file.Name = "player/silver/silver_Root.xno"; break;
                case 14: file.Name = "player/sonic_new/sonic_Root.xno"; break;
                case 15: file.Name = "player/supershadow/sshadow_Root.xno"; break;
                case 16: file.Name = "player/supersilver/ssilver_Root.xno"; break;
                case 17: file.Name = "player/supersonic/ssonic_Root.xno"; break;
                case 18: file.Name = "player/tails/tails_Root.xno"; break;
            }

            // If this model is too long for a short space, or need to be a different one than the original by enforcement, then go back.
            if ((originalFile.Length <= 0x1F && file.Name.Length > 0x1F) || (originalFile == file.Name && enforceNewModel == true))
                goto retry;

            // If this model is too short, then pad its name.
            if (originalFile.Length > 0x1F && file.Name.Length <= 0x1F)
                file.Name = file.Name.PadRight(0x21, '@');

            // Do the retargeting stuff based on names.
            switch (srcCharacter)
            {
                default:
                    foreach (var xnmFile in Directory.GetFiles(eventID, $"*_{srcCharacter}*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;

                case "ev_princess01":
                    foreach (var xnmFile in Directory.GetFiles(eventID, "*_princess_Root*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;

                case "ch_eggman":
                    foreach (var xnmFile in Directory.GetFiles(eventID, "*_eggman_Root*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;

                case "evilshadow_Root":
                case "en_fmef_Root":
                    foreach (var xnmFile in Directory.GetFiles(eventID, "*_mefiress_Root*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;

                case "ch_maidmaster":
                    foreach (var xnmFile in Directory.GetFiles(eventID, "*_maidmaster_Root*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;

                case "ch_childelis":
                    foreach (var xnmFile in Directory.GetFiles(eventID, "*_princess_child_Root*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;

                case "ch_soleana":
                    foreach (var xnmFile in Directory.GetFiles(eventID, "*_soleana_Root*.xnm", SearchOption.AllDirectories))
                        Helpers.RetargetAnimation(xnmFile, models.FirstOrDefault(x => x.Key == srcCharacter).Value, models.ElementAt(value).Value);
                    break;
            }
        }
    }
}
