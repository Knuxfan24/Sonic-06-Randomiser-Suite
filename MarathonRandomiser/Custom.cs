using Marathon.Formats.Audio;
using Marathon.Formats.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MarathonRandomiser
{
    internal class Custom
    {
        public static List<string> Music(string[] CustomMusic, string ModDirectory, bool? EnableCache, List<string> MiscMusic, string[] archives)
        {
            // Create Directories.
            Directory.CreateDirectory($@"{MainWindow.TemporaryDirectory}\tempWavs");
            Directory.CreateDirectory($@"{ModDirectory}\xenon\sound");

            // Line to add to the end of mod ini.
            string songs = "Custom=\"";

            // Loop through custom songs.
            for (int i = 0; i < CustomMusic.Length; i++)
            {
                string origName = $"{Path.GetFileNameWithoutExtension(CustomMusic[i])}.xma";
                int startLoop = 0;
                int endLoop = 0;

                // If this song is already an XMA we can just copy it straight over.
                if (Path.GetExtension(CustomMusic[i]) == ".xma")
                {
                    File.Copy(CustomMusic[i], $@"{ModDirectory}\xenon\sound\custom{i}.xma");
                }

                // If not, we have to convert it using xmaencode (and potentially vgmstream).
                else
                {
                    // If we're using the cache and an XMA with this name is present in the cache folder, copy it rather than converting anything.
                    if (File.Exists($"{Environment.CurrentDirectory}\\Cache\\XMA\\{origName}") && (bool)EnableCache)
                    {
                        File.Copy($"{Environment.CurrentDirectory}\\Cache\\XMA\\{origName}", $@"{ModDirectory}\xenon\sound\custom{i}.xma");
                    }

                    // If not, then convert it.
                    else
                    {
                        // If this file isn't a WAV, try convert it using vgmstream.
                        if (Path.GetExtension(CustomMusic[i]) != ".wav")
                        {
                            Process process = new();
                            process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\vgmstream\\vgmstream-cli.exe\"";
                            process.StartInfo.Arguments = $"-i -o \"{MainWindow.TemporaryDirectory}\\tempWavs\\custom{i}.wav\" \"{CustomMusic[i]}\"";
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
                            CustomMusic[i] = $@"{MainWindow.TemporaryDirectory}\tempWavs\custom{i}.wav";
                        }

                        // Convert WAV file to XMA.
                        using (Process process = new())
                        {
                            process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\xmaencode.exe\"";
                            process.StartInfo.Arguments = $"\"{CustomMusic[i]}\" /b 64 /t \"{ModDirectory}\\xenon\\sound\\custom{i}.xma\"";
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.CreateNoWindow = true;

                            process.Start();
                            process.BeginOutputReadLine();
                            process.WaitForExit();
                        }

                        // Patch the XMA to actually loop from start to end.
                        byte[] xma = File.ReadAllBytes($@"{ModDirectory}\xenon\sound\custom{i}.xma");
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
                        File.WriteAllBytes($@"{ModDirectory}\xenon\sound\custom{i}.xma", xma);

                        // If the cache is enabled, copy the newly converted file to the XMA cache folder.
                        if ((bool)EnableCache)
                            File.Copy($@"{ModDirectory}\xenon\sound\custom{i}.xma", $"{Environment.CurrentDirectory}\\Cache\\XMA\\{origName}");
                    }
                }

                // Add this song to the list of valid music for randomisation.
                MiscMusic.Add($"custom{i}");

                // Add this song to the string of custom files for the mod configuration ini.
                songs += $"custom{i}.xma,";
            }

            // Add all the songs to the mod configuration ini.
            // Remove the last comma and replace it with a closing quote.
            songs = songs.Remove(songs.LastIndexOf(','));
            songs += "\"";

            // Write the list of custom files to the mod configuration ini.
            using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{ModDirectory}", "mod.ini")))
            {
                configInfo.WriteLine(songs);
                configInfo.Close();
            }

            // Add all the songs to bgm.sbk in sound.arc
            foreach (string archive in archives)
            {
                // Find sound.arc.
                if (Path.GetFileName(archive).ToLower() == "sound.arc")
                {
                    // Unpack sound.arc.
                    string unpackedArchive = Helpers.ArchiveHandler(archive);

                    // Load bgm.sbk.
                    SoundBank bgmSBK = new($@"{unpackedArchive}\xenon\sound\bgm.sbk");

                    // Loop through all our custom songs and add them to bgm.sbk so they'll play in game.
                    for (int i = 0; i < CustomMusic.Length; i++)
                    {
                        Cue customSongCue = new()
                        {
                            Category = 1,
                            Name = $"custom{i}",
                            Radius = 6000,
                            Stream = $"sound/custom{i}.xma",
                            UnknownSingle = 500
                        };
                        bgmSBK.Data.Cues.Add(customSongCue);
                    }

                    // Save the updated bgm.sbk.
                    bgmSBK.Save();
                }
            }

            // Return the updated list to be used by the music randomiser.
            return MiscMusic;
        }

        public static List<string> VoicePacks(List<string> CustomVoxPacks, string ModDirectory, string[] archives, List<string> SetHints)
        {
            Directory.CreateDirectory($@"{ModDirectory}\xenon\sound\voice\e\");

            foreach (string CustomVoxPack in CustomVoxPacks)
            {
                // Setup a check in chase we already have Custom Files.
                bool alreadyHasCustom = false;
                string sounds = "Custom=\"";

                // Ensure the folders needed for this voice pack exist.
                Directory.CreateDirectory($@"{MainWindow.TemporaryDirectory}\tempVox\{CustomVoxPack}");

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
                    process.StartInfo.Arguments = $"x \"{Environment.CurrentDirectory}\\VoicePacks\\{CustomVoxPack}.zip\" -o\"{MainWindow.TemporaryDirectory}\\tempVox\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                // If there isn't a messageTable.mst file, abort.
                if (!File.Exists($@"{MainWindow.TemporaryDirectory}\tempVox\{CustomVoxPack}\messageTable.mst"))
                {
                    continue;
                }

                // Copy XMAs to the Mod Directory and add them to the list of custom files.
                string[] voiceXmas = Directory.GetFiles($@"{MainWindow.TemporaryDirectory}\tempVox\{CustomVoxPack}", "*.xma", SearchOption.TopDirectoryOnly);
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
                    using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{ModDirectory}", "mod.ini")))
                    {
                        configInfo.WriteLine(sounds);
                        configInfo.Close();
                    }
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
                        SoundBank voiceSBK = new($@"{Helpers.ArchiveHandler(archive)}\xenon\sound\voice_all_e.sbk");

                        // Loop through all our custom voice lines and add them to voice_all_e.sbk so they'll play in game.
                        for (int i = 0; i < voiceXmas.Length; i++)
                        {
                            Marathon.Formats.Audio.Cue customSongCue = new()
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
                MessageTable customMST = new($@"{MainWindow.TemporaryDirectory}\tempVox\{CustomVoxPack}\messageTable.mst");

                foreach (string archive in archives)
                {
                    // Find text.arc.
                    if (Path.GetFileName(archive).ToLower() == "text.arc")
                    {
                        // Unpack text.arc and load the original msg_hint.e.mst
                        MessageTable origMST = new($@"{Helpers.ArchiveHandler(archive)}\xenon\text\english\msg_hint.e.mst");

                        // Copy each entry from the voice pack's message table to the original game's message table.
                        foreach (Message entry in customMST.Data.Messages)
                        {
                            origMST.Data.Messages.Add(entry);
                            SetHints.Add(entry.Name);
                        }

                        // Save the updated msg_hint.e.mst.
                        origMST.Save();
                    }
                }
            }

            // Return the updated list to be used by the voice line randomiser.
            return SetHints;
        }
    }
}
