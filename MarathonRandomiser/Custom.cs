using Marathon.Formats.Audio;
using Marathon.Formats.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MarathonRandomiser
{
    internal class Custom
    {
        /// <summary>
        /// Converts and saves custom XMAs to the randomisation's mod directory.
        /// </summary>
        /// <param name="CustomSong">The filepath to the current song to process.</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="index">The number in the list of custom songs of the one we're processing.</param>
        /// <param name="EnableCache">Whether or not we need to store the generated XMA file.</param>
        public static async Task Music(string CustomSong, string ModDirectory, int index, bool? EnableCache)
        {
            // Get the name of the file with the extension replaced with .xma (used for the XMA Cache system).
            string origName = $"{Path.GetFileNameWithoutExtension(CustomSong)}.xma";

            // Set up values we'll use for looping.
            int startLoop = 0;
            int endLoop = 0;

            // If this song is already an XMA we can just copy it straight over.
            if (Path.GetExtension(CustomSong) == ".xma")
            {
                File.Copy(CustomSong, $@"{ModDirectory}\xenon\sound\custom{index}.xma");
            }

            // If not, we need to check for it in the cache if we're using it, or convert it.
            else
            {
                // Check if this file exists in the XMA Cache and copy it if the cache is enabled.
                if (File.Exists($@"{Environment.CurrentDirectory}\Cache\XMA\{origName}") && EnableCache == true)
                {
                    File.Copy($@"{Environment.CurrentDirectory}\Cache\XMA\{origName}", $@"{ModDirectory}\xenon\sound\custom{index}.xma");
                }

                // If not, then convert it.
                else
                {
                    // If this file isn't a WAV, try convert it using vgmstream.
                    if (Path.GetExtension(CustomSong) != ".wav")
                    {
                        Process process = new();
                        process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\vgmstream\\vgmstream-cli.exe\"";
                        process.StartInfo.Arguments = $"-i -o \"{MainWindow.TemporaryDirectory}\\tempWavs\\custom{index}.wav\" \"{CustomSong}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.Start();

                        StreamReader sr = process.StandardOutput;
                        string[] output = sr.ReadToEnd().Split("\r\n");
                        process.WaitForExit();

                        // If vgmstream has reported any loop points, then store them.
                        foreach (string value in output)
                        {
                            if (value.StartsWith("loop start"))
                            {
                                string[] split = value.Split(' ');
                                startLoop = int.Parse(split[2]);
                            }
                            if (value.StartsWith("loop end"))
                            {
                                string[] split = value.Split(' ');
                                endLoop = int.Parse(split[2]);
                            }
                        }

                        // Set the path to the song to our wav for the rest of the function.
                        CustomSong = $@"{MainWindow.TemporaryDirectory}\tempWavs\custom{index}.wav";
                    }

                    // Convert WAV file to XMA.
                    using (Process process = new())
                    {
                        process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\xmaencode.exe\"";
                        process.StartInfo.Arguments = $"\"{CustomSong}\" /b 64 /t \"{ModDirectory}\\xenon\\sound\\custom{index}.xma\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.CreateNoWindow = true;

                        process.Start();
                        process.BeginOutputReadLine();
                        process.WaitForExit();
                    }

                    // Patch the XMA to actually loop from start to end.
                    byte[] xma = File.ReadAllBytes($@"{ModDirectory}\xenon\sound\custom{index}.xma");
                    for (int x = 0; x < xma.Length; x += 4)
                    {
                        // Find the XMA2 Chunk Header
                        if (xma[x] == 0x58 && xma[x + 1] == 0x4D && xma[x + 2] == 0x41 && xma[x + 3] == 0x32)
                        {
                            // If we haven't fetched any loop points, then just add a start to end loop.
                            if (startLoop == 0 && endLoop == 0)
                            {
                                // Set the part of the file that controls the end loop (0x10 ahead of the XMA2 Chunk Header) to the sample count (0x20 ahead of the XMA2 Chunk Header).
                                xma[x + 0x10] = xma[x + 0x20];
                                xma[(x + 1) + 0x10] = xma[(x + 1) + 0x20];
                                xma[(x + 2) + 0x10] = xma[(x + 2) + 0x20];
                                xma[(x + 3) + 0x10] = xma[(x + 3) + 0x20];
                            }

                            // If we DO have loop points, then add them.
                            else
                            {
                                // Make a byte array out of the values.
                                byte[] startBytes = BitConverter.GetBytes(startLoop);
                                Array.Reverse(startBytes);
                                byte[] endBytes = BitConverter.GetBytes(endLoop);
                                Array.Reverse(endBytes);

                                // Start Loop Bytes
                                xma[x + 0xC] = startBytes[0];
                                xma[(x + 1) + 0xC] = startBytes[1];
                                xma[(x + 2) + 0xC] = startBytes[2];
                                xma[(x + 3) + 0xC] = startBytes[3];

                                // End Loop Bytes
                                xma[x + 0x10] = endBytes[0];
                                xma[(x + 1) + 0x10] = endBytes[1];
                                xma[(x + 2) + 0x10] = endBytes[2];
                                xma[(x + 3) + 0x10] = endBytes[3];

                            }
                        }
                    }
                    // Save the updated XMA.
                    File.WriteAllBytes($@"{ModDirectory}\xenon\sound\custom{index}.xma", xma);

                    // If the cache is enabled, copy the newly converted file to the XMA cache folder.
                    if (EnableCache == true)
                        File.Copy($@"{ModDirectory}\xenon\sound\custom{index}.xma", $@"{Environment.CurrentDirectory}\Cache\XMA\{origName}");
                }
            }
        }

        /// <summary>
        /// Adds all the custom songs to bgm.sbk so they can play in game.
        /// </summary>
        /// <param name="archivePath">The path to the extracted sound.arc</param>
        /// <param name="count">How many songs we're adding.</param>
        public static async Task UpdateBGMSoundBank(string archivePath, int count)
        {
            // Load bgm.sbk.
            SoundBank bgm = new($@"{archivePath}\xenon\sound\bgm.sbk");

            // Loop through all our custom songs and add them to bgm.sbk so they'll play in game.
            for (int i = 0; i < count; i++)
            {
                Cue customSongCue = new()
                {
                    Category = 1,
                    Name = $"custom{i}",
                    Radius = 6000,
                    Stream = $"sound/custom{i}.xma",
                    UnknownSingle = 500
                };
                bgm.Data.Cues.Add(customSongCue);
            }

            // Save the updated bgm.sbk.
            bgm.Save();
        }

        /// <summary>
        /// Unpacks and copies over the content needed for Voice Packs to the randomisation's mod directory.
        /// </summary>
        /// <param name="VoxPack">The filepath to the current voice pack to process.</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="archives">The filepaths to the game's archives (needed to find sound.arc and text.arc so we can update voice_all_e.sbk and msg_hint.e.mst).</param>
        /// <param name="setHints">The list of voice lines valid for Object Placement randomisation (needed so we can update it so the custom voices can be included).</param>
        /// <returns>A boolean used by the ProgressLogger to report if the process was aborted.</returns>
        public static async Task<bool> VoicePacks(string VoxPack, string ModDirectory, string[] archives, List<string> setHints)
        {
            // Setup a check in chase we already have Custom Files (as the Custom Music function always runs before this one).
            bool alreadyHasCustom = false;
            string sounds = "Custom=\"";

            // Ensure the folders needed for this voice pack exist.
            Directory.CreateDirectory($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}");

            // Load the mod configuration ini to see if we already have custom content. If we do, then read the custom files line.
            string[] modConfig = File.ReadAllLines(Path.Combine($@"{ModDirectory}", "mod.ini"));
            if (modConfig[9].Contains("True") || modConfig.Length == 11)
            {
                alreadyHasCustom = true;
                sounds = modConfig[10].Remove(modConfig[10].LastIndexOf('\"'));
                sounds += ",";
            }

            // Extract the voice pack zip archive.
            using (Process process = new())
            {
                process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\7z.exe\"";
                process.StartInfo.Arguments = $"x \"{Environment.CurrentDirectory}\\VoicePacks\\{VoxPack}.zip\" -o\"{MainWindow.TemporaryDirectory}\\tempVox\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }

            // If there isn't a messageTable.mst file, abort and return false so the logger can report it.
            if (!File.Exists($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}\messageTable.mst"))
                return false;

            // Copy XMAs to the Mod Directory and add them to the list of custom files.
            string[] voiceXmas = Directory.GetFiles($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}", "*.xma", SearchOption.TopDirectoryOnly);
            foreach (string voiceXma in voiceXmas)
            {
                File.Copy(voiceXma, $@"{ModDirectory}\xenon\sound\voice\e\{Path.GetFileNameWithoutExtension(voiceXma)}.xma");
                sounds += $"{Path.GetFileNameWithoutExtension(voiceXma)}.xma,";
            }

            // Add all the sounds to the mod ini.
            sounds = sounds.Remove(sounds.LastIndexOf(','));
            sounds += "\"";

            // If we aren't already using custom files, then write the custom list the same way as the custom music function does.
            if (!alreadyHasCustom)
            {
                using StreamWriter configInfo = File.AppendText(Path.Combine($@"{ModDirectory}", "mod.ini"));
                configInfo.WriteLine(sounds);
                configInfo.Close();
            }
            // If not, then patch the expanded list over the top of the existing one.
            else
            {
                modConfig[10] = sounds;
                File.WriteAllLines(Path.Combine($@"{ModDirectory}", "mod.ini"), modConfig);
            }

            // Add all the voice lines to voice_all_e.sbk in sound.arc
            foreach (string archive in archives)
            {
                // Find sound.arc.
                if (Path.GetFileName(archive).ToLower() == "sound.arc")
                {
                    // Unpack sound.arc and load voice_all_e.sbk.
                    string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                    SoundBank voiceSBK = new($@"{unpackedArchive}\xenon\sound\voice_all_e.sbk");

                    // Loop through all our custom voice lines and add them to voice_all_e.sbk so they'll play in game.
                    for (int i = 0; i < voiceXmas.Length; i++)
                    {
                        Cue customSongCue = new()
                        {
                            Category = 0,
                            Name = $"{Path.GetFileNameWithoutExtension(voiceXmas[i])}",
                            Radius = 6000,
                            Stream = $"sound/voice/e/{Path.GetFileNameWithoutExtension(voiceXmas[i])}.xma",
                            UnknownSingle = 500
                        };
                        voiceSBK.Data.Cues.Add(customSongCue);
                    }

                    // Save the updated voice_all_e.sbk.
                    voiceSBK.Save();
                }
            }

            // Add all the hints in this voice pack's messageTable.mst file to msg_hint.e.mst so they can display in game.
            // Load this voice pack's messageTable.mst file.
            MessageTable customMST = new($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}\messageTable.mst");

            foreach (string archive in archives)
            {
                // Find text.arc.
                if (Path.GetFileName(archive).ToLower() == "text.arc")
                {
                    // Unpack text.arc and load the original msg_hint.e.mst
                    string unpackedArchive = await Task.Run(() => Helpers.ArchiveHandler(archive));
                    MessageTable origMST = new($@"{unpackedArchive}\xenon\text\english\msg_hint.e.mst");

                    // Copy each entry from the voice pack's message table to the original game's message table.
                    foreach (Message entry in customMST.Data.Messages)
                    {
                        origMST.Data.Messages.Add(entry);
                        setHints.Add(entry.Name);
                    }

                    // Save the updated msg_hint.e.mst.
                    origMST.Save();
                }
            }

            // Return true to confirm it succeeded so the Logger doesn't need to report a failure.
            return true;
        }
    }
}
