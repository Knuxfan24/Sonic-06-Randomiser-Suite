using SonicAudioLib.Archives;
using SonicAudioLib.CriMw;
using SonicAudioLib.IO;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MarathonRandomiser
{
    internal class AudioRandomisers
    {
        #region Sonic Audio Tools Functions
        // All taken from the code for CsbEditor: https://github.com/blueskythlikesclouds/SonicAudioTools/blob/master/Source/CsbEditor/Program.cs
        public static async Task CSBUnpack(string csbFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            DataExtractor? extractor = new()
            {
                EnableThreading = false
            };

            string baseDirectory = Path.GetDirectoryName(csbFile);
            string outputDirectoryName = Path.Combine(baseDirectory, Path.GetFileNameWithoutExtension(csbFile));

            CriCpkArchive cpkArchive = null;
            string cpkPath = outputDirectoryName + ".cpk";
            bool found = File.Exists(cpkPath);

            //This should fix "File not found" error in case-sensitive file systems.
            //Add new extensions when necessary.
            foreach (string extension in new string[] { "cpk", "CPK" })
            {
                if (found)
                    break;

                cpkPath = outputDirectoryName + "." + extension;
                found = File.Exists(cpkPath);
            }

            using (CriTableReader reader = CriTableReader.Create(csbFile))
            {
                while (reader.Read())
                {
                    if (reader.GetString("name") == "SOUND_ELEMENT")
                    {
                        long tablePosition = reader.GetPosition("utf");
                        using (CriTableReader sdlReader = CriTableReader.Create(reader.GetSubStream("utf")))
                        {
                            while (sdlReader.Read())
                            {
                                if (sdlReader.GetByte("fmt") != 0)
                                {
                                    throw new Exception("The given CSB file contains an audio file which is not an ADX. Only CSB files with ADXs are supported.");
                                }

                                bool streaming = sdlReader.GetBoolean("stmflg");
                                if (streaming && !found)
                                {
                                    throw new Exception("Cannot find the external .CPK file for this .CSB file. Please ensure that the external .CPK file is stored in the directory where the .CPK file is.");
                                }

                                else if (streaming && found && cpkArchive == null)
                                {
                                    cpkArchive = new CriCpkArchive();
                                    cpkArchive.Load(cpkPath, 4096);
                                }

                                string sdlName = sdlReader.GetString("name");
                                DirectoryInfo destinationPath = new DirectoryInfo(Path.Combine(outputDirectoryName, sdlName));
                                destinationPath.Create();

                                CriAaxArchive aaxArchive = new CriAaxArchive();

                                if (streaming)
                                {
                                    CriCpkEntry cpkEntry = cpkArchive.GetByPath(sdlName);

                                    if (cpkEntry != null)
                                    {
                                        using (Stream cpkSource = File.OpenRead(cpkPath))
                                        using (Stream aaxSource = cpkEntry.Open(cpkSource))
                                        {
                                            aaxArchive.Read(aaxSource);

                                            foreach (CriAaxEntry entry in aaxArchive)
                                            {
                                                extractor.Add(cpkPath,
                                                    Path.Combine(destinationPath.FullName,
                                                    entry.Flag == CriAaxEntryFlag.Intro ? "Intro.adx" : "Loop.adx"),
                                                    cpkEntry.Position + entry.Position, entry.Length);
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    long aaxPosition = sdlReader.GetPosition("data");
                                    using (Stream aaxSource = sdlReader.GetSubStream("data"))
                                    {
                                        aaxArchive.Read(aaxSource);

                                        foreach (CriAaxEntry entry in aaxArchive)
                                        {
                                            extractor.Add(csbFile,
                                                Path.Combine(destinationPath.FullName,
                                                entry.Flag == CriAaxEntryFlag.Intro ? "Intro.adx" : "Loop.adx"),
                                                tablePosition + aaxPosition + entry.Position, entry.Length);
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    }
                }
            }

            extractor.Run();
        }
    
        public static async Task CSBRepack(string csbDirectory)
        {
            string baseDirectory = Path.GetDirectoryName(csbDirectory);
            string csbPath = csbDirectory + ".csb";

            foreach (string extension in new string[] { "csb", "CSB" })
            {
                if (File.Exists(csbPath))
                    break;
                csbPath = csbDirectory + "." + extension;
            }

            if (!File.Exists(csbPath))
            {
                throw new Exception("Cannot find the .CSB file for this directory. Please ensure that the .CSB file is stored in the directory where this directory is.");
            }

            CriCpkArchive cpkArchive = new CriCpkArchive();

            CriTable csbFile = new CriTable();
            csbFile.Load(csbPath, 4096);

            CriRow soundElementRow = csbFile.Rows.First(row => (string)row["name"] == "SOUND_ELEMENT");

            CriTable soundElementTable = new CriTable();
            soundElementTable.Load((byte[])soundElementRow["utf"]);

            List<FileInfo> junks = new List<FileInfo>();

            foreach (CriRow sdlRow in soundElementTable.Rows)
            {
                string sdlName = (string)sdlRow["name"];

                DirectoryInfo sdlDirectory = new DirectoryInfo(Path.Combine(csbDirectory, sdlName));

                if (!sdlDirectory.Exists)
                {
                    throw new Exception($"Cannot find sound element directory for replacement.\nPath attempt: {sdlDirectory.FullName}");
                }

                bool streaming = (byte)sdlRow["stmflg"] != 0;
                uint sampleRate = (uint)sdlRow["sfreq"];
                byte numberChannels = (byte)sdlRow["nch"];

                CriAaxArchive aaxArchive = new CriAaxArchive();
                foreach (FileInfo file in sdlDirectory.GetFiles("*.adx"))
                {
                    CriAaxEntry entry = new CriAaxEntry();
                    if (file.Name.ToLower(CultureInfo.GetCultureInfo("en-US")) == "intro.adx")
                    {
                        entry.Flag = CriAaxEntryFlag.Intro;
                        entry.FilePath = file;
                        aaxArchive.Add(entry);

                        ReadAdx(file, out sampleRate, out numberChannels);
                    }

                    else if (file.Name.ToLower(CultureInfo.GetCultureInfo("en-US")) == "loop.adx")
                    {
                        entry.Flag = CriAaxEntryFlag.Loop;
                        entry.FilePath = file;
                        aaxArchive.Add(entry);

                        ReadAdx(file, out sampleRate, out numberChannels);
                    }
                }

                if (streaming)
                {
                    CriCpkEntry entry = new CriCpkEntry();
                    entry.Name = Path.GetFileName(sdlName);
                    entry.DirectoryName = Path.GetDirectoryName(sdlName);
                    entry.Id = (uint)cpkArchive.Count;
                    entry.FilePath = new FileInfo(Path.GetTempFileName());
                    junks.Add(entry.FilePath);

                    cpkArchive.Add(entry);
                    aaxArchive.Save(entry.FilePath.FullName, 4096);
                }

                else
                {
                    sdlRow["data"] = aaxArchive.Save();
                }

                sdlRow["sfreq"] = sampleRate;
                sdlRow["nch"] = numberChannels;
            }

            soundElementTable.WriterSettings = CriTableWriterSettings.AdxSettings;
            soundElementRow["utf"] = soundElementTable.Save();

            csbFile.WriterSettings = CriTableWriterSettings.AdxSettings;
            csbFile.Save(csbPath, 4096);

            if (cpkArchive.Count > 0)
            {
                string cpkPath = csbDirectory + ".cpk";
                foreach (string extension in new string[] { "cpk", "CPK" })
                {
                    if (File.Exists(csbDirectory + "." + extension))
                    {
                        cpkPath = csbDirectory + "." + extension;
                        break;
                    }
                }

                cpkArchive.Save(cpkPath, 4096);
            }

            foreach (FileInfo junk in junks)
            {
                junk.Delete();
            }
        }
        
        static void ReadAdx(FileInfo fileInfo, out uint sampleRate, out byte numberChannels)
        {
            using (Stream source = fileInfo.OpenRead())
            {
                source.Seek(7, SeekOrigin.Begin);
                numberChannels = DataStream.ReadByte(source);
                sampleRate = DataStream.ReadUInt32BE(source);
            }
        }
        #endregion

        /// <summary>
        /// Randomises the music to play.
        /// </summary>
        /// <param name="luaFile">The lua to process.</param>
        /// <param name="MiscMusic">The list of valid songs.</param>
        /// <returns></returns>
        public static async Task MusicRandomiser(string luaFile, List<string> MiscMusic, string Seed)
        {
            // Decompile this lua binary.
            await Task.Run(() => Helpers.LuaDecompile(luaFile));

            // Read the decompiled lua file into a string array.
            string[] lua = File.ReadAllLines(luaFile);

            // Loop through each line in this lua binary.
            for (int i = 0; i < lua.Length; i++)
            {
                // Search for the two lines that control music playback.
                if (lua[i].Contains("Game.PlayBGM") || lua[i].Contains("mission_bgm"))
                {
                    // Split the line controlling the music playback up based on the quote marks around the song name.
                    string[] song = lua[i].Split('"');

                    // Accordion Song Easter Egg (https://youtu.be/YqjQew7BRRk?t=6016).
                    if (Seed.Contains("Accordion") && lua[i].Contains("mission_bgm"))
                    {
                        // ACCORDIONS.
                        song[1] = "twn_accordion";

                        // Rejoin the split array into one line and add it back to the original lua array.
                        lua[i] = string.Join("\"", song);
                    }

                    // Some things apparently have an empty thing, so don't change those, else, ALL THE ACCORDIONS!
                    else if (song[1] != "")
                    {
                        // Replace the second value in the split array (the one containing the song name) with a song from the list of valid songs.
                        song[1] = MiscMusic[MainWindow.Randomiser.Next(MiscMusic.Count)];

                        // Rejoin the split array into one line and add it back to the original lua array.
                        lua[i] = string.Join("\"", song);
                    }
                }
            }

            // Save the updated lua binary.
            File.WriteAllLines(luaFile, lua);
        }
    
        /// <summary>
        /// Shuffles all the ADX sounds in the CSB files.
        /// </summary>
        /// <param name="Files">The current set of ADX files we're working on, Intro VS Loop and Mono VS Stereo.</param>
        public static async Task ShuffleSoundEffects(List<string> Files)
        {
            // Set up a list of numbers.
            List<int> usedNumbers = new();

            for (int i = 0; i < Files.Count; i++)
            {
                // Pick a random number from the amount of entires in the list of ADX files.
                int index = MainWindow.Randomiser.Next(Files.Count);

                // If the selected number is already used, pick another until it isn't.
                if (usedNumbers.Contains(index))
                {
                    do { index = MainWindow.Randomiser.Next(Files.Count); }
                    while (usedNumbers.Contains(index));
                }

                // Add this number to the usedNumbers list so we can't pull the same ADX file twice.
                usedNumbers.Add(index);

                // As the file can't be picked more than once, move it to the name of the chosen file.
                File.Move($"{Files[index]}.rnd", Files[i]);
            }
        }
    }
}
