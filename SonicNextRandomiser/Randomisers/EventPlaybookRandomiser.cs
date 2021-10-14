using Marathon.Formats.Event;
using System.Collections.Generic;
using System.IO;

namespace SonicNextRandomiser.Randomisers
{
    class EventPlaybookRandomiser
    {
        /// <summary>
        /// The main entry block for the EventPlaybook Randomisation, all the other functions in this file are accessed through here.
        /// If a function to randomise something about an event would have only been one line, then it is also handled in this function instead of a seperate one.
        /// </summary>
        /// <param name="archivePath">The path to the already unpacked cache.arc containing eventplaybook.epb</param>
        /// <param name="scene">Whether a random scene file should be picked to replace each cutscene's normal one.</param>
        /// <param name="sceneLuas">The list of valid Scene Luas for the scene shuffle.</param>
        /// <param name="rotX">Whether the scene's rotation on the X Axis should be randomised between -180 and 180.</param>
        /// <param name="rotY">Whether the scene's rotation on the Y Axis should be randomised between -180 and 180.</param>
        /// <param name="rotZ">Whether the scene's rotation on the Z Axis should be randomised between -180 and 180.</param>
        /// <param name="posX">Whether the scene's position on the X Axis should be randomised between -50000 and 50000.</param>
        /// <param name="posY">Whether the scene's position on the Y Axis should be randomised between -50000 and 50000.</param>
        /// <param name="posZ">Whether the scene's position on the Z Axis should be randomised between -50000 and 50000.</param>
        /// <param name="terrain">Whether a random stage terrain folder should be picked to replace each cutscene's normal one.</param>
        /// <param name="eventShuffle">Whether or not we should shuffle the order events play in.</param>
        /// <param name="modsDirectory"></param>
        /// <param name="gameExecutable"></param>
        /// <param name="seed"></param>
        public static void Load(string archivePath, bool scene, List<string> sceneLuas, bool rotX, bool rotY, bool rotZ, bool posX, bool posY, bool posZ, bool terrain, List<string> terrainFolders, bool eventShuffle, string modsDirectory, string gameExecutable, string seed)
        {
            // Load eventplaybook.epb
            System.Console.WriteLine($@"Randomising '{archivePath}\xenon\eventplaybook.epb'.");
            EventPlaybook epb = new($@"{archivePath}\xenon\eventplaybook.epb");

            // Loop through each event present in eventplaybook.epb.
            foreach (Event eventEntry in epb.Events)
            {
                // Pick a random scene lua binary from the list if we're randomising it and this event actually uses one.
                if (eventEntry.SceneLua != null && scene)
                    eventEntry.SceneLua = sceneLuas[MainWindow.Randomiser.Next(sceneLuas.Count)];

                // Pick a random terrain folder path from the list if we're randomising it and this event actually uses one.
                if (eventEntry.Terrain != null && terrain)
                    eventEntry.Terrain = terrainFolders[MainWindow.Randomiser.Next(terrainFolders.Count)];

                // Rotation
                if (rotX || rotY || rotZ)
                    Rotation(eventEntry, rotX, rotY, rotZ);

                // Position
                if (posX || posY || posZ)
                    Position(eventEntry, posX, posY, posZ);
            }

            // Shuffle events.
            if (eventShuffle)
                EventShuffler(epb, modsDirectory, gameExecutable, seed);

            // Save the updated EventPlaybook.epb.
            epb.Save();
        }

        /// <summary>
        /// Randomises an event's rotation values.
        /// </summary>
        /// <param name="eventEntry">The event to edit.</param>
        /// <param name="rotX">Whether we should randomise the rotation on the X axis.</param>
        /// <param name="rotY">Whether we should randomise the rotation on the Y axis.</param>
        /// <param name="rotZ">Whether we should randomise the rotation on the Z axis.</param>
        static void Rotation(Event eventEntry, bool rotX, bool rotY, bool rotZ)
        {
            // Get original rotation values.
            float rotationX = eventEntry.Rotation.X;
            float rotationY = eventEntry.Rotation.Y;
            float rotationZ = eventEntry.Rotation.Z;

            // Randomise the rotation values if required.
            if (rotX)
                rotationX = MainWindow.Randomiser.Next(-180, 181);
            if (rotY)
                rotationY = MainWindow.Randomiser.Next(-180, 181);
            if (rotZ)
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
        static void Position(Event eventEntry, bool posX, bool posY, bool posZ)
        {
            // Get original position values.
            float positionX = eventEntry.Position.X;
            float positionY = eventEntry.Position.Y;
            float positionZ = eventEntry.Position.Z;

            // Randomise the position values if required.
            if (posX)
                positionX = MainWindow.Randomiser.Next(-50000, 50001);
            if (posY)
                positionY = MainWindow.Randomiser.Next(-50000, 50001);
            if (posZ)
                positionZ = MainWindow.Randomiser.Next(-50000, 50001);

            // Build a Vector3 out of the position values and save it over the original values.
            eventEntry.Position = new(positionX, positionY, positionZ);
        }

        /// <summary>
        /// Randomises which events play when.
        /// </summary>
        /// <param name="epb">The EventPlaybook we're working on.</param>
        /// <param name="modsDirectory">The path to the user's mods directory (used to find where to place the WMV files).</param>
        /// <param name="gameExecutable">The path to the user's game directory (used to find the event FMVs).</param>
        /// <param name="seed">The seed being used (used to find where to place the WMV files).</param>
        static void EventShuffler(EventPlaybook epb, string modsDirectory, string gameExecutable, string seed)
        {
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
            string[] fmvs = Directory.GetFiles($@"{Path.GetDirectoryName(gameExecutable)}\xenon\event", "*.wmv", SearchOption.AllDirectories);
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
                            Directory.CreateDirectory($@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\event\{epb.Events[i].Name}");
                            File.Copy(fmv, $@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\event\{epb.Events[i].Name}\{epb.Events[i].Name}.wmv");
                        }
                    }
                }
            }
        }
    }
}
