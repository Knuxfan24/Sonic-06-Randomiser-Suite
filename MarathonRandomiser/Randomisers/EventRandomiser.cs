using Marathon.Formats.Event;
using System.Linq;

namespace MarathonRandomiser
{
    internal class EventRandomiser
    {
        /// <summary>
        /// Process and randomise elements in eventplaybook.epb.
        /// </summary>
        /// <param name="archivePath">The path to the extracted cache.arc.</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="lighting">Whether or not we're randomising the scene lua to use.</param>
        /// <param name="EventLighting">The list of valid scene luas.</param>
        /// <param name="terrain">Whether or not we're randomising event terrain.</param>
        /// <param name="EventTerrain">The list of valid terrain paths.</param>
        /// <param name="rotX">Whether or not we're randomising the X rotation for the events.</param>
        /// <param name="rotY">Whether or not we're randomising the Y rotation for the events.</param>
        /// <param name="rotZ">Whether or not we're randomising the Z rotation for the events.</param>
        /// <param name="posX">Whether or not we're randomising the X position for the events.</param>
        /// <param name="posY">Whether or not we're randomising the Y position for the events.</param>
        /// <param name="posZ">Whether or not we're randomising the Z position for the events.</param>
        public static async Task Process(string archivePath, string corePath, bool? lighting, List<string> EventLighting, bool? terrain, List<string> EventTerrain,
                                         bool? rotX, bool? rotY, bool? rotZ, bool? posX, bool? posY, bool? posZ)
        {
            // Load eventplaybook.epb.
            EventPlaybook epb = new($@"{archivePath}\{corePath}\eventplaybook.epb");

            // Loop through each event present in eventplaybook.epb.
            foreach (Event eventEntry in epb.Events)
            {
                // Pick a random scene lua binary from the list if we're randomising it and this event actually uses one.
                if (eventEntry.SceneLua != null && lighting == true)
                    eventEntry.SceneLua = EventLighting[MainWindow.Randomiser.Next(EventLighting.Count)];

                // Pick a random terrain folder path from the list if we're randomising it and this event actually uses one.
                if (eventEntry.Terrain != null && terrain == true)
                    eventEntry.Terrain = EventTerrain[MainWindow.Randomiser.Next(EventTerrain.Count)];

                // Rotation
                if (rotX == true || rotY == true || rotZ == true)
                    Rotation(eventEntry, rotX, rotY, rotZ);

                // Position
                if (posX == true || posY == true || posZ == true)
                    Position(eventEntry, posX, posY, posZ);

                // Save the updated eventplaybook.epb.
                epb.Save();
            }

        }

        /// <summary>
        /// Randomises an event's rotation values.
        /// </summary>
        /// <param name="eventEntry">The event to edit.</param>
        /// <param name="rotX">Whether we should randomise the rotation on the X axis.</param>
        /// <param name="rotY">Whether we should randomise the rotation on the Y axis.</param>
        /// <param name="rotZ">Whether we should randomise the rotation on the Z axis.</param>
        static void Rotation(Event eventEntry, bool? rotX, bool? rotY, bool? rotZ)
        {
            // Get original rotation values.
            float rotationX = eventEntry.Rotation.X;
            float rotationY = eventEntry.Rotation.Y;
            float rotationZ = eventEntry.Rotation.Z;

            // Randomise the rotation values if required.
            if (rotX == true)
                rotationX = MainWindow.Randomiser.Next(-180, 181);
            if (rotY == true)
                rotationY = MainWindow.Randomiser.Next(-180, 181);
            if (rotZ == true)
                rotationZ = MainWindow.Randomiser.Next(-180, 181);

            // Build a Vector3 out of the rotation values and save it over the original values.
            eventEntry.Rotation = new(rotationX, rotationY, rotationZ);
        }

        /// <summary>
        /// Randomises an event's positional values.
        /// </summary>
        /// <param name="eventEntry">The event to edit.</param>
        /// <param name="posX">Whether we should randomise the position on the X axis.</param>
        /// <param name="posY">Whether we should randomise the position on the Y axis.</param>
        /// <param name="posZ">Whether we should randomise the position on the Z axis.</param>
        static void Position(Event eventEntry, bool? posX, bool? posY, bool? posZ)
        {
            // Get original position values.
            float positionX = eventEntry.Position.X;
            float positionY = eventEntry.Position.Y;
            float positionZ = eventEntry.Position.Z;

            // Randomise the position values if required.
            if (posX == true)
                positionX = MainWindow.Randomiser.Next(-50000, 50001);
            if (posY == true)
                positionY = MainWindow.Randomiser.Next(-50000, 50001);
            if (posZ == true)
                positionZ = MainWindow.Randomiser.Next(-50000, 50001);

            // Build a Vector3 out of the position values and save it over the original values.
            eventEntry.Position = new(positionX, positionY, positionZ);
        }

        /// <summary>
        /// Randomises which events play when.
        /// </summary>
        /// <param name="archivePath">The path to the extracted cache.arc.</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="GameExecutable">The path to the game executable (so we know what platform we're working on).</param>
        /// <param name="skipFMVs">Whether or not to skip shuffling the prerendered scenes.</param>
        public static async Task EventShuffler(string archivePath, string corePath, string ModDirectory, string GameExecutable, bool? skipFMVs)
        {
            // Load eventplaybook.epb.
            EventPlaybook epb = new($@"{archivePath}\{corePath}\eventplaybook.epb");

            // Set up a list so we can track which events have already been used.
            List<int> usedNumbers = new();

            // Create two lists of the events.
            List<Event> events = new();
            List<Event> eventsFMV = new();

            // Get all the events.
            for (int i = 0; i < epb.Events.Count; i++)
            {
                // Have to create a new event entry because C# is dumb and references the original one if I just add that to the list.
                Event newEvent = new()
                {
                    Name                 = epb.Events[i].Name,
                    Folder               = epb.Events[i].Folder,
                    Terrain              = epb.Events[i].Terrain,
                    SceneLua             = epb.Events[i].SceneLua,
                    SoundBank            = epb.Events[i].SoundBank,
                    ParticleContainer    = epb.Events[i].ParticleContainer,
                    SubtitleMessageTable = epb.Events[i].SubtitleMessageTable,
                    EventLength          = epb.Events[i].EventLength,
                    Position             = epb.Events[i].Position,
                    Rotation             = epb.Events[i].Rotation
                };

                // Determine whether this event is an FMV or not.
                if (epb.Events[i].Terrain != null)
                    events.Add(newEvent);
                else
                    eventsFMV.Add(newEvent);
            }

            // None FMVs
            for (int i = 0; i < epb.Events.Count; i++)
            {
                if (epb.Events[i].Terrain != null)
                {
                    // Pick a random number from the amount of entires in the standard events table.
                    int index = MainWindow.Randomiser.Next(events.Count);

                    // If the selected number is already used, pick another until it isn't.
                    if (usedNumbers.Contains(index))
                    {
                        do { index = MainWindow.Randomiser.Next(events.Count); }
                        while (usedNumbers.Contains(index));
                    }
                    usedNumbers.Add(index);

                    // Set the values for this event to that of our selected one.
                    epb.Events[i].Folder               = events[index].Folder;
                    epb.Events[i].Terrain              = events[index].Terrain;
                    epb.Events[i].SceneLua             = events[index].SceneLua;
                    epb.Events[i].SoundBank            = events[index].SoundBank;
                    epb.Events[i].ParticleContainer    = events[index].ParticleContainer;
                    epb.Events[i].SubtitleMessageTable = events[index].SubtitleMessageTable;
                    epb.Events[i].EventLength          = events[index].EventLength;
                    epb.Events[i].Position             = events[index].Position;
                    epb.Events[i].Rotation             = events[index].Rotation;
                }
            }

            // FMVs (this shit doesn't work right and dupes an FMV because Aquatic Base can go fuck itself).
            if (skipFMVs == false)
            {
                // Determine what the file extension for the FMVs are.
                string filetype = "wmv";
                if (corePath == "ps3")
                    filetype = "pam";

                usedNumbers.Clear();
                for (int i = 0; i < epb.Events.Count; i++)
                {
                    if (epb.Events[i].Terrain == null)
                    {
                        // Pick a random number from the amount of entires in the fvm event table.
                        int index = MainWindow.Randomiser.Next(eventsFMV.Count);

                        // If the selected number is already used, pick another until it isn't.
                        if (usedNumbers.Contains(index))
                        {
                            do { index = MainWindow.Randomiser.Next(eventsFMV.Count); }
                            while (usedNumbers.Contains(index));
                        }
                        usedNumbers.Add(index);

                        // Set the values for this event to that of our selected one.
                        epb.Events[i].Folder               = eventsFMV[index].Folder;
                        epb.Events[i].Terrain              = eventsFMV[index].Terrain;
                        epb.Events[i].SceneLua             = eventsFMV[index].SceneLua;
                        epb.Events[i].SoundBank            = eventsFMV[index].SoundBank;
                        epb.Events[i].ParticleContainer    = eventsFMV[index].ParticleContainer;
                        epb.Events[i].SubtitleMessageTable = eventsFMV[index].SubtitleMessageTable;
                        epb.Events[i].EventLength          = eventsFMV[index].EventLength;
                        epb.Events[i].Position             = eventsFMV[index].Position;
                        epb.Events[i].Rotation             = eventsFMV[index].Rotation;

                        // Handle the WMV files (has a bug that dupes the Aquatic Base Past FMV because of how its set up).
                        foreach (string fmv in Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\event", $"*{filetype}", SearchOption.AllDirectories))
                        {
                            if (fmv.Contains(eventsFMV[index].Folder.Replace('/', '\\')) && epb.Events[i].Name != "e0220")
                            {
                                Directory.CreateDirectory($@"{ModDirectory}\{corePath}\event\{epb.Events[i].Name}");
                                File.Copy(fmv, $@"{ModDirectory}\{corePath}\event\{epb.Events[i].Name}\{epb.Events[i].Name}.{filetype}");
                            }
                        }
                    }
                }
            }

            // Save the updated eventplaybook.epb
            epb.Save();
        }

        /// <summary>
        /// Replaces the XMA files used in cutscenes with other ones.
        /// </summary>
        /// <param name="GameExecutable">The path to the game executable (so we can get the original cutscene XMAs).</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="includeJapanese">Whether or not we include Japanese voice lines in the shuffle.</param>
        /// <param name="includeGameplay">Whether or not we include voice lines used in gameplay in the shuffle.</param>
        /// <param name="hasVox">Whether we have an voice packs to take into account.</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        public static async Task ShuffleVoiceLines(string GameExecutable, string corePath, bool? includeJapanese, bool? includeGameplay, bool hasVox, string ModDirectory)
        {
            // Figure out if we need to use XMAs or AT3s.
            string fileType = "xma";
            if (corePath == "ps3")
                fileType = "at3";

            // Get the English cutscene XMAs.
            string[] eventXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\event", $"E*.{fileType}", SearchOption.AllDirectories);

            // If we're including Japanese lines, invalidate the old array and replace it with one that includes both English and Japanese.
            if (includeJapanese == true)
                eventXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\event", $"*.{fileType}", SearchOption.AllDirectories);

            // Do this so we don't accidentally shuffle EVERY voice line.
            string[] shuffleArray = eventXMAs;

            // If we're including gameplay lines, then add them in.
            if (includeGameplay == true)
            {
                // Set up a list of gameplay XMAs.
                string[] gameplayXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\sound\voice\e", $"*.{fileType}", SearchOption.AllDirectories);

                // If we're including Japanese ones, merge the existing array with one containing the Japanese lines instead.
                if (includeJapanese == true)
                    gameplayXMAs = gameplayXMAs.Concat(Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\sound\voice\j", $"*.{fileType}", SearchOption.AllDirectories)).ToArray();

                // If we have any voice packs, then pick up their XMAs from the randomisation's mod directory and merge them into the existing array.
                if (hasVox)
                    gameplayXMAs = gameplayXMAs.Concat(Directory.GetFiles($@"{ModDirectory}\xenon\sound\voice\e\", $"*.{fileType}", SearchOption.AllDirectories)).ToArray();

                // Merge the event and gameplay arrays into one.
                eventXMAs = eventXMAs.Concat(gameplayXMAs).ToArray();
            }

            // Store all the indicies we've used already so we can't pick the same xma twice.
            List<int> usedNumbers = new();

            // Loop through everything in shuffleArray (so only the cutscene XMAs).
            for (int i = 0; i < shuffleArray.Length; i++)
            {
                // Pick a number based on the amount of XMAs we have, if it's already used, pick a new one.
                int index = MainWindow.Randomiser.Next(eventXMAs.Length);
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(eventXMAs.Length); }
                    while (usedNumbers.Contains(index));
                }
                usedNumbers.Add(index);

                // Create the directory that this XMA resides in for the mod.
                if (!Directory.Exists($@"{ModDirectory}{Path.GetDirectoryName(shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(GameExecutable), ""))}"))
                    Directory.CreateDirectory($@"{ModDirectory}{Path.GetDirectoryName(shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(GameExecutable), ""))}");

                // Copy the selected XMA to this one's position in the mod.
                File.Copy(eventXMAs[index], $@"{ModDirectory}{shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(GameExecutable), "")}");
            }
        }

        /// <summary>
        /// Replaces the music track files for events with files from the game, including any custom music the user has added.
        /// </summary>
        /// <param name="GameExecutable">The path to the game executable (so we can get the original game music and the event songs).</param>
        /// <param name="corePath">The platform path (ps3 or xenon)</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="hasCustomMusic">Whether or not any custom music is included.</param>
        /// <param name="nullSTRMusic">Whether or not the music has chosen to stop songs from overlapping in a single cutscene.</param>
        public static async Task RandomiseEventMusic(string GameExecutable, string corePath, string ModDirectory, bool hasCustomMusic, bool? nullSTRMusic)
        {
            // Determine which audio format we need to look for.
            string fileType = ".xma";
            if (corePath == "ps3")
                fileType = ".at3";

            // Create the event directory in the mod's sound folder.
            Directory.CreateDirectory($@"{ModDirectory}\{corePath}\sound\event");

            // Get the list of valid songs.
            string[] songs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\sound", $"*{fileType}", SearchOption.TopDirectoryOnly);
            if (hasCustomMusic)
                songs = songs.Concat(Directory.GetFiles($@"{ModDirectory}\{corePath}\sound", $"*{fileType}", SearchOption.TopDirectoryOnly).ToArray()).ToArray();

            // Loop through all the sound files in the game's event folder and copy a random song to the mod's version.
            foreach (string file in Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{corePath}\sound\event", $"*{fileType}", SearchOption.TopDirectoryOnly))
                File.Copy(songs[MainWindow.Randomiser.Next(songs.Length)], $@"{ModDirectory}\{corePath}\sound\event\{Path.GetFileName(file)}");

            // Replace the three additional tracks in The Time Space Rift with blank files to prevent music overlapping.
            if (nullSTRMusic == true)
            {
                File.Create($@"{ModDirectory}\{corePath}\sound\event\e0304_3195{fileType}");
                File.Create($@"{ModDirectory}\{corePath}\sound\event\e0304_9120{fileType}");
                File.Create($@"{ModDirectory}\{corePath}\sound\event\e0304_11940{fileType}");
            }
        }
    }
}
