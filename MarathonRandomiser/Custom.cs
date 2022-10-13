using Marathon.Formats.Audio;
using Marathon.Formats.Text;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System.Diagnostics;
using System.Linq;

namespace MarathonRandomiser
{
    internal class Custom
    {
        /// <summary>
        /// Converts and saves custom song XMAs to the randomisation's mod directory.
        /// </summary>
        /// <param name="CustomSong">The filepath to the current song to process.</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="index">The number in the list of custom songs of the one we're processing.</param>
        /// <param name="EnableCache">Whether or not we need to store the generated XMA file.</param>
        public static async Task Music(string CustomSong, string ModDirectory, int index, bool? EnableCache)
        {
            // Update the mod.ini Custom file list.
            await Task.Run(() => Helpers.UpdateCustomFiles($"custom{index}.xma", ModDirectory));

            // Get the name of the file with the extension replaced with .xma (used for the XMA Cache system).
            string origName = $"{Path.GetFileNameWithoutExtension(CustomSong)}.xma";

            // Set up values we'll use for looping.
            int startLoop = 0;
            int endLoop = 0;

            // If this song is already an XMA we can just copy it straight over.
            if (Path.GetExtension(CustomSong) == ".xma")
                File.Copy(CustomSong, $@"{ModDirectory}\xenon\sound\custom{index}.xma");

            // If not, we need to check for it in the cache if we're using it, or convert it.
            else
            {
                // Check if this file exists in the XMA Cache and copy it if the cache is enabled.
                if (File.Exists($@"{Environment.CurrentDirectory}\Cache\XMA\Music\{origName}") && EnableCache == true)
                    File.Copy($@"{Environment.CurrentDirectory}\Cache\XMA\Music\{origName}", $@"{ModDirectory}\xenon\sound\custom{index}.xma");

                // If not, then convert it.
                else
                {
                    // If this file isn't a WAV, try convert it using vgmstream.
                    if (Path.GetExtension(CustomSong) != ".wav")
                    {
                        Process process = new();
                        process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\vgmstream\\vgmstream-cli.exe\"";
                        process.StartInfo.Arguments = $"-o \"{MainWindow.TemporaryDirectory}\\tempWavs\\custom{index}.wav\" -L \"{CustomSong}\"";
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

                        // If we've failed to convert, throw a proper exception.
                        if (output.Length == 1)
                            throw new Exception($"Failed to convert '{CustomSong}', this may be due to a character in the filename or an unsupported filetype?");

                        // Set the path to the song to our wav for the rest of the function.
                        CustomSong = $@"{MainWindow.TemporaryDirectory}\tempWavs\custom{index}.wav";
                    }

                    // Convert WAV file to XMA.
                    await Task.Run(() => Helpers.XMAEncode($@"{CustomSong}", $@"{ModDirectory}\xenon\sound\custom{index}.xma"));

                    // If vgmstream didn't find any loop points, then patch in a start to end loop.
                    if (startLoop == 0 && endLoop == 0)
                    {
                        byte[] xma = File.ReadAllBytes($@"{ModDirectory}\xenon\sound\custom{index}.xma");
                        for (int x = 0; x < xma.Length; x += 4)
                        {
                            // Find the XMA2 Chunk Header
                            if (xma[x] == 0x58 && xma[x + 1] == 0x4D && xma[x + 2] == 0x41 && xma[x + 3] == 0x32)
                            {
                                // Set the part of the file that controls the end loop (0x10 ahead of the XMA2 Chunk Header) to the sample count (0x20 ahead of the XMA2 Chunk Header).
                                xma[x + 0x10] = xma[x + 0x20];
                                xma[(x + 1) + 0x10] = xma[(x + 1) + 0x20];
                                xma[(x + 2) + 0x10] = xma[(x + 2) + 0x20];
                                xma[(x + 3) + 0x10] = xma[(x + 3) + 0x20];
                            }
                        }

                        // Save the updated XMA.
                        File.WriteAllBytes($@"{ModDirectory}\xenon\sound\custom{index}.xma", xma);
                    }

                    // If the cache is enabled, copy the newly converted file to the XMA cache folder.
                    if (EnableCache == true)
                        File.Copy($@"{ModDirectory}\xenon\sound\custom{index}.xma", $@"{Environment.CurrentDirectory}\Cache\XMA\Music\{origName}");
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
        /// Converts and saves custom voice line XMAs to the randomisation's mod directory.
        /// </summary>
        /// <param name="CustomSound">The filepath to the current voice file to process.</param>
        /// <param name="ModDirectory">The path to the randomisation's mod directory.</param>
        /// <param name="index">The number in the list of custom voice files of the one we're processing.</param>
        /// <param name="EnableCache">Whether or not we need to store the generated XMA file.</param>
        public static async Task VoiceLines(string CustomSound, string ModDirectory, int index, bool? EnableCache)
        {
            // Update the mod.ini Custom file list.
            await Task.Run(() => Helpers.UpdateCustomFiles($"custom_hint{index}.xma", ModDirectory));

            // Get the name of the file with the extension replaced with .xma (used for the XMA Cache system).
            string origName = $"{Path.GetFileNameWithoutExtension(CustomSound)}.xma";

            // If this song is already an XMA we can just copy it straight over.
            if (Path.GetExtension(CustomSound) == ".xma")
                File.Copy(CustomSound, $@"{ModDirectory}\xenon\sound\custom_hint{index}.xma");

            // If not, we need to check for it in the cache if we're using it, or convert it.
            else
            {
                // Check if this file exists in the XMA Cache and copy it if the cache is enabled.
                if (File.Exists($@"{Environment.CurrentDirectory}\Cache\XMA\Voice\{origName}") && EnableCache == true)
                    File.Copy($@"{Environment.CurrentDirectory}\Cache\XMA\Voice\{origName}", $@"{ModDirectory}\xenon\sound\voice\e\custom_hint{index}.xma");

                // If not, then convert it.
                else
                {
                    // If this file isn't a WAV, try convert it using vgmstream.
                    if (Path.GetExtension(CustomSound) != ".wav")
                    {
                        Process process = new();
                        process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\vgmstream\\vgmstream-cli.exe\"";
                        process.StartInfo.Arguments = $"-o \"{MainWindow.TemporaryDirectory}\\tempWavs\\custom_hint{index}.wav\" \"{CustomSound}\"";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.Start();

                        StreamReader sr = process.StandardOutput;
                        string[] output = sr.ReadToEnd().Split("\r\n");
                        process.WaitForExit();

                        // If we've failed to convert, throw a proper exception.
                        if (output.Length == 1)
                            throw new Exception($"Failed to convert '{CustomSound}', this may be due to a character in the filename or an unsupported filetype?");

                        // Set the path to the song to our wav for the rest of the function.
                        CustomSound = $@"{MainWindow.TemporaryDirectory}\tempWavs\custom_hint{index}.wav";
                    }

                    // Convert WAV file to XMA.
                    await Task.Run(() => Helpers.XMAEncode($@"{CustomSound}", $@"{ModDirectory}\xenon\sound\voice\e\custom_hint{index}.xma"));

                    // If the cache is enabled, copy the newly converted file to the XMA cache folder.
                    if (EnableCache == true)
                        File.Copy($@"{ModDirectory}\xenon\sound\voice\e\custom_hint{index}.xma", $@"{Environment.CurrentDirectory}\Cache\XMA\Voice\{origName}");
                }
            }
        }

        /// <summary>
        /// Adds the custom hint lines to voice_all_e.sbk so they can play in game.
        /// </summary>
        /// <param name="archivePath">The path to the extracted sound.arc</param>
        /// <param name="count">How many sounds we're adding.</param>
        public static async Task UpdateVoiceTable(string archivePath, int count)
        {
            // Load voice_all_e.sbk.
            SoundBank bgm = new($@"{archivePath}\xenon\sound\voice_all_e.sbk");

            // Loop through all our custom voice lines and add them to voice_all_e.sbk so they'll play in game.
            for (int i = 0; i < count; i++)
            {
                Cue customVoiceCue = new()
                {
                    Category = 0,
                    Name = $"hint_custom{i}",
                    Radius = 6000,
                    Stream = $"sound/voice/e/custom_hint{i}.wma",
                    UnknownSingle = 500
                };
                bgm.Data.Cues.Add(customVoiceCue);
            }

            // Save the updated bgm.sbk.
            bgm.Save();
        }

        /// <summary>
        /// Adds the custom hint lines to msg_hint.e.mst so they can be included in the hint list.
        /// </summary>
        /// <param name="archivePath">The path to the extracted text.arc</param>
        /// <param name="CustomVoices">The custom files we're adding.</param>
        public static async Task UpdateVoiceHints(string archivePath, List<string> CustomVoices)
        {
            // Load msg_hint.e.mst.
            MessageTable mst = new($@"{archivePath}\xenon\\text\english\msg_hint.e.mst");

            // Loop through and create a message entry for each sound with a placeholder string.
            for (int i = 0; i < CustomVoices.Count; i++)
            {
                Message message = new()
                {
                    Name = $"hint_custom{i}",
                    Text = $"${Path.GetFileNameWithoutExtension(CustomVoices[i])}",
                    Placeholders = new[] { $"sound(hint_custom{i})" }
                };
                mst.Data.Messages.Add(message);
            }

            // Save the updated msg_hint.e.mst.
            mst.Save();
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
            // Ensure the folders needed for this voice pack exist.
            Directory.CreateDirectory($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}");

            // Extract the Voice Pack zip.
            using (ZipArchive archive = ZipArchive.Open($@"{Environment.CurrentDirectory}\VoicePacks\{VoxPack}.zip"))
            {
                foreach (ZipArchiveEntry? entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory($@"{MainWindow.TemporaryDirectory}\tempVox", new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }

            // If there isn't a messageTable.mst file, abort and return false so the logger can report it.
            if (!File.Exists($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}\messageTable.mst"))
                return false;

            // Get all the XMAs in this voice pack.
            string[] voiceXmas = Directory.GetFiles($@"{MainWindow.TemporaryDirectory}\tempVox\{VoxPack}", "*.xma", SearchOption.TopDirectoryOnly);
            
            foreach (string voiceXma in voiceXmas)
            {
                // Copy this XMA to the Mod's directory.
                File.Copy(voiceXma, $@"{ModDirectory}\xenon\sound\voice\e\{Path.GetFileNameWithoutExtension(voiceXma)}.xma");

                // Update the mod.ini Custom file list.
                await Task.Run(() => Helpers.UpdateCustomFiles($"{Path.GetFileNameWithoutExtension(voiceXma)}.xma", ModDirectory));
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
                        Cue customVoiceLine = new()
                        {
                            Category = 0,
                            Name = $"{Path.GetFileNameWithoutExtension(voiceXmas[i])}",
                            Radius = 6000,
                            Stream = $"sound/voice/e/{Path.GetFileNameWithoutExtension(voiceXmas[i])}.xma",
                            UnknownSingle = 500
                        };

                        // Debugging message if this sound will cause the voice SBK write to fail and crash.
                        if (customVoiceLine.Name.Length > 32)
                            Debug.WriteLine($"{customVoiceLine.Name} is {customVoiceLine.Name.Length} characters long when 32 is the maximum");

                        voiceSBK.Data.Cues.Add(customVoiceLine);
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
                        entry.Name = $"vox_{entry.Name}";
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
    
        /// <summary>
        /// Converts a non DDS texture to a DDS using texconv.
        /// </summary>
        /// <param name="texture">The filepath to the texture to process.</param>
        /// <returns>The filepath to the converted texture.</returns>
        public static async Task<string> Texture(string texture)
        {
            // Only actually do the conversion if it's not already a DDS.
            if (Path.GetExtension(texture).ToLower() != ".dds")
            {
                using (Process process = new())
                {
                    process.StartInfo.FileName = $"\"{Environment.CurrentDirectory}\\ExternalResources\\texconv.exe\"";
                    process.StartInfo.Arguments = $"-o \"{MainWindow.TemporaryDirectory}\\tempDDS\" \"{texture}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();
                }

                return $"{MainWindow.TemporaryDirectory}\\tempDDS\\{Path.GetFileNameWithoutExtension(texture)}.dds";
            }

            // Just return the filepath if it is already a DDS.
            else
            {
                return texture;
            }
        }
    }
}
