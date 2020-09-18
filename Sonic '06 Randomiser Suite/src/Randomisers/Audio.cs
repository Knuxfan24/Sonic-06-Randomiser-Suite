using Sonic_06_Randomiser_Suite.Serialisers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VGAudio.Containers.Adx;
using VGAudio.Formats;
using VGAudio.Formats.CriAdx;

namespace Sonic_06_Randomiser_Suite
{
    class Audio
    {
        /// <summary>
        /// Randomises all music in Lua scripts
        /// </summary>
        public static void RandomiseMusic(string[] editedLub, Random rng) {
            int lineNum = 0;

            foreach (string line in editedLub)
            {
                if (line.Contains("Game.PlayBGM"))
                {
                    string[] tempLine = line.Split('"');
                    tempLine[1] = Main.Lua_Music[rng.Next(Main.Lua_Music.Count)];
                    editedLub[lineNum] = string.Join("\"", tempLine);
                }
                lineNum++;
            }
        }

        public static void EditCSB(string argument)
        {
            Process tempProcess = new Process();
            tempProcess.StartInfo.FileName = Program.CsbEditor;
            tempProcess.StartInfo.Arguments = argument;
            tempProcess.StartInfo.CreateNoWindow = true;
            tempProcess.StartInfo.UseShellExecute = false;
            tempProcess.Start();
            tempProcess.WaitForExit();
        }

        public static void RandomiseSoundEffects(string randomArchive, string type, Random rng)
        {
            List<string> availableMonoSounds = new List<string>();
            List<string> availableStereoSounds = new List<string>();
            List<int> usedMonoNumbers = new List<int>();
            List<int> usedStereoNumbers = new List<int>();

            foreach (string adxData in Directory.GetFiles($"{randomArchive}\\sound\\common\\sound", $"{type}.adx", SearchOption.AllDirectories))
            {
                try
                {
                    AudioData audio = new AdxReader().Read(File.ReadAllBytes(adxData));
                    int channelCount = audio.GetFormat<CriAdxFormat>().ChannelCount;

                    if (channelCount == 1) { availableMonoSounds.Add(adxData); }
                    if (channelCount == 2) { availableStereoSounds.Add(adxData); }

                    File.Move(adxData, Paths.ReplaceFilename(adxData, $"temp-{Path.GetFileName(adxData)}"));
                }
                catch
                {
                }
            }
            foreach (string adxData in availableMonoSounds)
            {
                int index = rng.Next(availableMonoSounds.Count);
                if (usedMonoNumbers.Contains(index))
                {
                    do { index = rng.Next(availableMonoSounds.Count); }
                    while (usedMonoNumbers.Contains(index));
                }
                usedMonoNumbers.Add(index);

                File.Move(Paths.ReplaceFilename(adxData, $"temp-{Path.GetFileName(adxData)}"), availableMonoSounds[index]);
            }
            foreach (string adxData in availableStereoSounds)
            {
                int index = rng.Next(availableStereoSounds.Count);
                if (usedStereoNumbers.Contains(index))
                {
                    do { index = rng.Next(availableStereoSounds.Count); }
                    while (usedStereoNumbers.Contains(index));
                }
                usedStereoNumbers.Add(index);

                File.Move(Paths.ReplaceFilename(adxData, $"temp-{Path.GetFileName(adxData)}"), availableStereoSounds[index]);
            }
        }
    }
}
