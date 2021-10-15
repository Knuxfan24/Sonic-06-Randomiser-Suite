using Marathon.Formats.Audio;
using Marathon.Formats.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Sonic_06_Randomiser_Suite
{
    class Custom
    {
        /// <summary>
        /// Takes the contents of the custom songs textbox, converts them to XMA, copies them to the mod directory, adds them to the music list and bgm.sbk.
        /// Also caches the resulting XMAs if requested.
        /// </summary>
        /// <param name="customSongFiles">The value of the custom songs textbox.</param>
        /// <param name="modsDirectory">The location of the mods.</param>
        /// <param name="seed">The seed being used for this randomisation.</param>
        /// <param name="cache">Whether the XMA cache should be used.</param>
        /// <param name="miscMusic">The list of valid music for the music randomiser, used here to add the custom tracks to it so they can be picked.</param>
        /// <param name="archives">The array of the game archives, used to find sound.arc for the bgm.sbk patching.</param>
        /// <param name="gameExecutable">The location of the game's executable, used as part of the archive extraction in the bgm.sbk patching process.</param>
        /// <returns></returns>
        public static List<string> CustomMusic(string customSongFiles, string modsDirectory, string seed, bool cache, List<string> miscMusic, string[] archives, string gameExecutable)
        {
            // Get list of custom song files.
            string[] customSongs = customSongFiles.Split('|');

            // Create Directories.
            Directory.CreateDirectory($@"{Program.TemporaryDirectory}\tempWavs");
            Directory.CreateDirectory($@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound");
            if(cache)
                Directory.CreateDirectory($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\Cache\\XMA\\");

            // Set the CustomFilesystem value in the mod ini to true.
            string[] configFile = File.ReadAllLines(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini"));
            if (configFile[9] == "CustomFilesystem=\"False\"")
            {
                configFile[9] = "CustomFilesystem=\"True\"";
                File.WriteAllLines(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini"), configFile);
            }

            // Line to add to the end of mod ini.
            string songs = "Custom=\"";

            // Loop through custom songs.
            for (int i = 0; i < customSongs.Length; i++)
            {
                string origName = $"{Path.GetFileNameWithoutExtension(customSongs[i])}.xma";
                int startLoop = 0;
                int endLoop = 0;

                // If this song is already an XMA we can just copy it straight over.
                if (Path.GetExtension(customSongs[i]) == ".xma")
                {
                    System.Console.WriteLine($@"Copying '{customSongs[i]}'.");
                    File.Copy(customSongs[i], $@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\custom{i}.xma");
                }

                // If not, we have to convert it using xmaencode (and potentially vgmstream).
                else
                {
                    // If we're using the cache and an XMA with this name is present in the cache folder, copy it rather than converting anything.
                    if (File.Exists($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\Cache\\XMA\\{origName}") && cache)
                    {
                        System.Console.WriteLine($@"Copying '{customSongs[i]}' from XMA cache.");
                        File.Copy($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\Cache\\XMA\\{origName}", $@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\custom{i}.xma");
                    }

                    // If not, then convert it.
                    else
                    {
                        System.Console.WriteLine($@"Converting and copying '{customSongs[i]}'.");

                        // If this file isn't a WAV, try convert it using vgmstream.
                        if (Path.GetExtension(customSongs[i]) != ".wav")
                        {
                            Process process = new();
                            process.StartInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\ExternalResources\\vgmstream\\vgmstream-cli.exe\"";
                            process.StartInfo.Arguments = $"-i -o \"{Program.TemporaryDirectory}\\tempWavs\\custom{i}.wav\" \"{customSongs[i]}\"";
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.Start();

                            StreamReader sr = process.StandardOutput;
                            string[] output = sr.ReadToEnd().Split("\r\n");
                            process.WaitForExit();

                            // If vgmstream has reported any loop points, then store them.
                            foreach(string value in output)
                            {
                                if(value.StartsWith("loop start"))
                                {
                                    string[] split = value.Split(' ');
                                    startLoop = int.Parse(split[2]);
                                }
                                if(value.StartsWith("loop end"))
                                {
                                    string[] split = value.Split(' ');
                                    endLoop = int.Parse(split[2]);
                                }
                            }

                            // Set the path to the song to our wav for the rest of the function.
                            customSongs[i] = $@"{Program.TemporaryDirectory}\tempWavs\custom{i}.wav";
                        }

                        // Convert WAV file to XMA.
                        using (Process process = new())
                        {
                            process.StartInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\ExternalResources\\xmaencode.exe\"";
                            process.StartInfo.Arguments = $"\"{customSongs[i]}\" /b 64 /t \"{modsDirectory}\\Sonic '06 Randomised ({seed})\\xenon\\sound\\custom{i}.xma\"";
                            process.StartInfo.UseShellExecute = false;
                            process.StartInfo.RedirectStandardOutput = true;
                            process.StartInfo.CreateNoWindow = true;

                            process.Start();
                            process.BeginOutputReadLine();
                            process.WaitForExit();
                        }

                        // Patch the XMA to actually loop from start to end.
                        byte[] xma = File.ReadAllBytes($@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\custom{i}.xma");
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
                        File.WriteAllBytes($@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\custom{i}.xma", xma);

                        // If the cache is enabled, copy the newly converted file to the XMA cache folder.
                        if (cache)
                            File.Copy($@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\custom{i}.xma", $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\Cache\\XMA\\{origName}");
                    }
                }

                // Add this song to the list of valid music for randomisation.
                miscMusic.Add($"custom{i}");

                // Add this song to the string of custom files for the mod configuration ini.
                songs += $"custom{i}.xma,";
            }

            // Add all the songs to the mod configuration ini.
            // Remove the last comma and replace it with a closing quote.
            songs = songs.Remove(songs.LastIndexOf(','));
            songs += "\"";

            // Write the list of custom files to the mod configuration ini.
            using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini")))
            {
                configInfo.WriteLine(songs);
                configInfo.Close();
            }

            // Add all the songs to bgm.sbk in sound.arc
            System.Console.WriteLine($@"Updating bgm.sbk with {customSongs.Length} custom songs.");
            foreach (string archive in archives)
            {
                // Find sound.arc.
                if (Path.GetFileName(archive).ToLower() == "sound.arc")
                {
                    // Unpack sound.arc.
                    string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(gameExecutable));

                    // Load bgm.sbk.
                    SoundBank bgmSBK = new($@"{unpackedArchive}\xenon\sound\bgm.sbk");

                    // Loop through all our custom songs and add them to bgm.sbk so they'll play in game.
                    for (int i = 0; i < customSongs.Length; i++)
                    {
                        Marathon.Formats.Audio.Cue customSongCue = new()
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
            return miscMusic;
        }

        /// <summary>
        /// Takes the contents of the voice packs textbox, extracts them to the mod directory, adds them to the voice list and voice_all_e.sbk.
        /// Also adds the hints to msg_hint.e.mst so they can be displayed in game.
        /// </summary>
        /// <param name="voicePacks">The value of the voice packs textbox.</param>
        /// <param name="modsDirectory">The location of the mods.</param>
        /// <param name="seed">The seed being used for this randomisation.</param>
        /// <param name="setVoice">The list of valid hints for the voice randomiser, used here to add the custom hints to it so they can be picked.</param>
        /// <param name="archives">The array of the game archives, used to find sound.arc for the voice_all_e.sbk patching and text.arc for the msg_hint.e.mst patching.</param>
        /// <param name="gameExecutable">The location of the game's executable, used as part of the archive extraction in the voice_all_e.sbk and msg_hint.e.mst patching processes.</param>
        /// <returns></returns>
        public static List<string> VoicePacks(string voicePacks, string modsDirectory, string seed, List<string> setVoice, string[] archives, string gameExecutable)
        {
            // Split voice list.
            string[] voxList = voicePacks.Split('|');

            // Loop through the voice packs.
            foreach (string voxPack in voxList)
            {
                System.Console.WriteLine($@"Unpacking '{voxPack}'.");
                // Setup a check in chase we already have Custom Files.
                bool alreadyHasCustom = false;
                string sounds = "Custom=\"";

                // Ensure the folders needed for this voice pack exist.
                Directory.CreateDirectory($@"{Program.TemporaryDirectory}\{Path.GetFileNameWithoutExtension(voxPack)}");
                Directory.CreateDirectory($@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\voice\e\");

                // Load the mod configuration ini to see if we already have custom content. If we do, then read the custom files line.
                string[] modConfig = File.ReadAllLines(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini"));
                if(modConfig[9].Contains("True") || modConfig.Length == 11)
                {
                    alreadyHasCustom = true;
                    sounds = modConfig[10].Remove(modConfig[10].LastIndexOf('\"'));
                    sounds += ",";
                }

                // Extract the voice pack zip archive.
                using (Process process = new())
                {
                    process.StartInfo.FileName = $"\"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\ExternalResources\\7z.exe\"";
                    process.StartInfo.Arguments = $"x \"{voxPack}\" -o\"{Program.TemporaryDirectory}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                // If there isn't a messageTable.mst file, abort.
                if (!File.Exists($@"{Program.TemporaryDirectory}\{Path.GetFileNameWithoutExtension(voxPack)}\messageTable.mst"))
                {
                    System.Console.WriteLine($@"'{voxPack}' doesn't appear to be a Sonic '06 Randomiser Suite Voice Pack. Skipping.");
                    continue;
                }

                // Copy XMAs to the Mod Directory and add them to the list of custom files.
                string[] voiceXmas = Directory.GetFiles($@"{Program.TemporaryDirectory}\{Path.GetFileNameWithoutExtension(voxPack)}", "*.xma", SearchOption.TopDirectoryOnly);
                foreach(string voiceXma in voiceXmas)
                {
                    System.Console.WriteLine($@"Copying '{voiceXma}' to '{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\voice\e\{Path.GetFileNameWithoutExtension(voiceXma)}.xma'.");
                    File.Copy(voiceXma, $@"{modsDirectory}\Sonic '06 Randomised ({seed})\xenon\sound\voice\e\{Path.GetFileNameWithoutExtension(voiceXma)}.xma");
                    sounds += $"{Path.GetFileNameWithoutExtension(voiceXma)}.xma,";
                }

                // Add all the sounds to the mod ini.
                sounds = sounds.Remove(sounds.LastIndexOf(','));
                sounds += "\"";

                // If we aren't already using custom files, then write the custom list the same way as the custom music function does.
                if(!alreadyHasCustom)
                {
                    using (StreamWriter configInfo = File.AppendText(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini")))
                    {
                        configInfo.WriteLine(sounds);
                        configInfo.Close();
                    }
                }
                // If not, then patch the expanded list over the top of the existing one.
                else
                {
                    modConfig[10] = sounds;
                    File.WriteAllLines(Path.Combine($@"{modsDirectory}\Sonic '06 Randomised ({seed})", "mod.ini"), modConfig);
                }

                // Add all the voice lines to voice_all_e.sbk in sound.arc
                System.Console.WriteLine($@"Updating voice_all_e.sbk with {voiceXmas.Length} voice lines from '{voxPack}'.");
                foreach (string archive in archives)
                {
                    // Find sound.arc.
                    if (Path.GetFileName(archive).ToLower() == "sound.arc")
                    {
                        // Unpack sound.arc.
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(gameExecutable));

                        // Load voice_all_e.sbk.
                        SoundBank voiceSBK = new($@"{unpackedArchive}\xenon\sound\voice_all_e.sbk");

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
                MessageTable customMST = new($@"{Program.TemporaryDirectory}\{Path.GetFileNameWithoutExtension(voxPack)}\messageTable.mst");

                System.Console.WriteLine($@"Updating msg_hint.e.mst with {voiceXmas.Length} voice lines from '{voxPack}'.");
                foreach (string archive in archives)
                {
                    // Find text.arc.
                    if (Path.GetFileName(archive).ToLower() == "text.arc")
                    {
                        // Unpack text.arc.
                        string unpackedArchive = ArchiveHandler.UnpackArchive(archive, Path.GetDirectoryName(gameExecutable));

                        // Load the original msg_hint.e.mst
                        MessageTable origMST = new($@"{unpackedArchive}\xenon\text\english\msg_hint.e.mst");

                        // Copy each entry from the voice pack's message table to the original game's message table.
                        foreach(Message entry in customMST.Data.Messages)
                        {
                            origMST.Data.Messages.Add(entry);
                            setVoice.Add(entry.Name);
                        }

                        // Save the updated msg_hint.e.mst.
                        origMST.Save();
                    }
                }
            }

            // Return the updated list to be used by the voice line randomiser.
            return setVoice;
        }
    }
}
