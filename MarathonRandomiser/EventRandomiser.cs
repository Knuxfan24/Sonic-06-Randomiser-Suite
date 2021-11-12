using Marathon.Formats.Event;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class EventRandomiser
    {
        public static async Task Load(string archivePath, bool? lighting, List<string> EventLighting, bool? terrain, List<string> EventTerrain, bool? rotX, bool? rotY, bool? rotZ, bool? posX, bool? posY,
                                      bool? posZ)
        {
            EventPlaybook epb = new($@"{archivePath}\xenon\eventplaybook.epb");

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

                // Save the updated EventPlaybook.epb.
                epb.Save();
            }

        }
        //public static void Load(string archivePath, bool scene, List<string> EventLighting, bool terrain, List<string> EventTerrain, bool rotX, bool rotY, bool rotZ, bool posX, bool posY, bool posZ,
        //                        bool eventShuffle, string ModDirectory, string GameExecutable)
        //{
        //    EventPlaybook epb = new($@"{archivePath}\xenon\eventplaybook.epb");

        //    // Loop through each event present in eventplaybook.epb.
        //    foreach (Event eventEntry in epb.Events)
        //    {
        //        // Pick a random scene lua binary from the list if we're randomising it and this event actually uses one.
        //        if (eventEntry.SceneLua != null && scene)
        //            eventEntry.SceneLua = EventLighting[MainWindow.Randomiser.Next(EventLighting.Count)];

        //        // Pick a random terrain folder path from the list if we're randomising it and this event actually uses one.
        //        if (eventEntry.Terrain != null && terrain)
        //            eventEntry.Terrain = EventTerrain[MainWindow.Randomiser.Next(EventTerrain.Count)];

        //        // Rotation
        //        if (rotX || rotY || rotZ)
        //            Rotation(eventEntry, rotX, rotY, rotZ);

        //        // Position
        //        if (posX || posY || posZ)
        //            Position(eventEntry, posX, posY, posZ);
        //    }

        //    // Shuffle events.
        //    if (eventShuffle)
        //        EventShuffler(epb, ModDirectory, GameExecutable);

        //    // Save the updated EventPlaybook.epb.
        //    epb.Save();
        //}

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
        /// <param name="epb">The EventPlaybook we're working on.</param>
        /// <param name="modsDirectory">The path to the user's mods directory (used to find where to place the WMV files).</param>
        /// <param name="GameExecutable">The path to the user's game directory (used to find the event FMVs).</param>
        /// <param name="seed">The seed being used (used to find where to place the WMV files).</param>
        public static async Task EventShuffler(string archivePath, string ModDirectory, string GameExecutable)
        {
            EventPlaybook epb = new($@"{archivePath}\xenon\eventplaybook.epb");

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
            usedNumbers.Clear();
            string[] fmvs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\xenon\event", "*.wmv", SearchOption.AllDirectories);
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
                    foreach (string fmv in fmvs)
                    {
                        if (fmv.Contains(eventsFMV[index].Folder.Replace('/', '\\')) && epb.Events[i].Name != "e0220")
                        {
                            Directory.CreateDirectory($@"{ModDirectory}\xenon\event\{epb.Events[i].Name}");
                            File.Copy(fmv, $@"{ModDirectory}\xenon\event\{epb.Events[i].Name}\{epb.Events[i].Name}.wmv");
                        }
                    }
                }
            }
        }
    
        public static async Task ShuffleVoiceLines(string GameExecutable, bool? includeJapanese, bool? includeGameplay, bool hasVox, string ModDirectory)
        {
            string neededFolder = "xenon";
            if (GameExecutable.ToLower().EndsWith(".bin"))
                neededFolder = "ps3";

            string[] eventXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{neededFolder}\event", "E*.xma", SearchOption.AllDirectories);
            if (includeJapanese == true)
                eventXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{neededFolder}\event", "*.xma", SearchOption.AllDirectories);

            // Do this so we don't accidentally shuffle EVERY voice line.
            string[] shuffleArray = eventXMAs;

            if (includeGameplay == true)
            {
                string[] gameplayXMAs = Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{neededFolder}\sound\voice\e", "*.xma", SearchOption.AllDirectories);
                if (includeJapanese == true)
                {
                    gameplayXMAs = gameplayXMAs.Concat(Directory.GetFiles($@"{Path.GetDirectoryName(GameExecutable)}\{neededFolder}\sound\voice\j", "*.xma", SearchOption.AllDirectories)).ToArray();
                    eventXMAs = eventXMAs.Concat(gameplayXMAs).ToArray();
                }
                if (hasVox)
                {
                    gameplayXMAs = gameplayXMAs.Concat(Directory.GetFiles($@"{ModDirectory}\xenon\sound\voice\e\", "*.xma", SearchOption.AllDirectories)).ToArray();
                    eventXMAs = eventXMAs.Concat(gameplayXMAs).ToArray();
                }
            }

            List<int> usedNumbers = new();

            for (int i = 0; i < shuffleArray.Length; i++)
            {
                int index = MainWindow.Randomiser.Next(eventXMAs.Length);
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(eventXMAs.Length); }
                    while (usedNumbers.Contains(index));
                }
                usedNumbers.Add(index);

                if (!Directory.Exists($@"{ModDirectory}{Path.GetDirectoryName(shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(GameExecutable), ""))}"))
                    Directory.CreateDirectory($@"{ModDirectory}{Path.GetDirectoryName(shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(GameExecutable), ""))}");

                File.Copy(eventXMAs[index], $@"{ModDirectory}{shuffleArray[i].Substring(0, shuffleArray[i].Length).Replace(Path.GetDirectoryName(GameExecutable), "")}");
            }
        }
    }
}
