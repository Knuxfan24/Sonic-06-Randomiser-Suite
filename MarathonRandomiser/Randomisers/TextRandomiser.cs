﻿using Marathon.Formats.Text;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;

namespace MarathonRandomiser
{
    internal class TextRandomiser
    {
        /// <summary>
        /// Shuffles all the text entries in all the game's message table files around.
        /// </summary>
        /// <param name="mstFiles">Array of all the message table files.</param>
        public static async Task ShuffleText(string[] mstFiles)
        {
            // Set up a list so we can track which messages have already been used.
            List<int> usedNumbers = new();

            // Create a list of all the messages.
            using MessageTable list = new();

            // Loop through all the message tables.
            foreach (string mstFile in mstFiles)
            {
                // Load this message table.
                using MessageTable mst = new(mstFile);

                // Copy every message into our list message table for later referal.
                foreach (Message message in mst.Data.Messages)
                    list.Data.Messages.Add(message);
            }

            // Loop through all the message tables again.
            foreach (string mstFile in mstFiles)
            {
                // Load this message table.
                using MessageTable mst = new(mstFile);

                // Loop through every message in this message table.
                foreach (Message message in mst.Data.Messages)
                {
                    // Pick a random number from the amount of entires in the list message table.
                    int index = MainWindow.Randomiser.Next(list.Data.Messages.Count);

                    // If the selected number is already used, pick another until it isn't.
                    if (usedNumbers.Contains(index))
                    {
                        do { index = MainWindow.Randomiser.Next(list.Data.Messages.Count); }
                        while (usedNumbers.Contains(index));
                    }

                    // Add this number to the usedNumbers list so we can't pull the same message twice.
                    usedNumbers.Add(index);

                    // Copy the selected message's placeholders and text over this one's.
                    message.Placeholders = list.Data.Messages[index].Placeholders;
                    message.Text = list.Data.Messages[index].Text;
                }

                // Save the updated message table.
                mst.Save();
            }
        }

        /// <summary>
        /// Replaces the text entries in an MST with a random word.
        /// </summary>
        /// <param name="mstFile">The path to the MST to process.</param>
        /// <param name="wordList">The list of English words.</param>
        /// <param name="enforce">Whether or not we need to enforce the word length.</param>
        public static async Task TextGenerator(string mstFile, string[] wordList, bool? enforce, bool? vox)
        {
            // Load the MST.
            MessageTable mst = new(mstFile);

            // Loop through each Message Entry in this MST.
            foreach (Message? message in mst.Data.Messages)
            {
                // Skip hints added from the Custom Voice Lines option and Voice Pack ones if they're disabled.
                if (message.Name.Contains("hint_custom") || (message.Name.StartsWith("vox_") && vox == false))
                    continue;

                // Edit the New Lines, New Text Boxes, Placeholder Calls and basic punctuation so they can be preserved.
                message.Text = message.Text.Replace("\n", " \n ");
                message.Text = message.Text.Replace("\f", " \f ");
                message.Text = message.Text.Replace("$", " $ ");
                message.Text = message.Text.Replace(".", " . ");
                message.Text = message.Text.Replace(",", " , ");
                message.Text = message.Text.Replace("?", " ? ");
                message.Text = message.Text.Replace("!", " ! ");
                message.Text = message.Text.Replace("-", " - ");
                message.Text = message.Text.Replace("\"", " \" ");
                message.Text = message.Text.Replace("©", " © ");
                message.Text = message.Text.Replace("…", " … ");
                message.Text = message.Text.Replace(":", " : ");
                message.Text = message.Text.Replace("&", " & ");

                // Split this string into an array.
                string[] split = message.Text.Split(' ');

                // Loop through and pick a random word for each array entry.
                for (int i = 0; i < split.Length; i++)
                {
                    // Check this isn't empty, a control character, basic punctuation or a number.
                    if (split[i] is not "\n" and not "\f" and not "$" and not "" and not "." and not "," and not "?" and not "!" and not "-" and not "\"" and not "©" and not "…" and not ":" and not "&"
                        && !int.TryParse(split[i], out _))
                    {
                        // Check the case of the word.
                        bool isAllUpper = false;
                        bool isAllLower = false;

                        if (split[i].ToLower() == split[i])
                            isAllLower = true;
                        if (split[i].ToUpper() == split[i])
                            isAllUpper = true;

                        // If we're not enforcing the length of the words, pick any of them.
                        if (enforce == false)
                            split[i] = wordList[MainWindow.Randomiser.Next(wordList.Length)];

                        // If we ARE enforcing the length of the words, then pick until it's the same length or we just give up.
                        else
                        {
                            // Choose a string to start with.
                            string chosenString = wordList[MainWindow.Randomiser.Next(wordList.Length)];

                            // Start with one million attempts.
                            int remainingAttempts = 1000000;

                            // If the string is longer and we have attempts left, try again and deduct an attempt.
                            while (remainingAttempts != 0 && chosenString.Length != split[i].Length)
                            {
                                chosenString = wordList[MainWindow.Randomiser.Next(wordList.Length)];
                                remainingAttempts--;
                            }

                            // If we have no attempts left, give up and print it to the debug output. Else, actually save the edited word.
                            if (remainingAttempts == 0)
                                System.Diagnostics.Debug.WriteLine($"Ran out of attempts to generate a word to replace \"{split[i]}\" for '{message.Name}' in '{mstFile}'.");
                            else
                                split[i] = chosenString;
                        }

                        // Change the case of the word/first character.
                        if (isAllLower)
                            split[i] = split[i].ToLower();
                        else if (isAllUpper)
                            split[i] = split[i].ToUpper();
                        else
                            split[i] = char.ToUpper(split[i][0]) + split[i][1..];
                    }
                }

                // Rejoin the string and reverse the earlier edits to avoid redudant spaces.
                message.Text = String.Join(" ", split);
                message.Text = message.Text.Replace(" \n ", "\n");
                message.Text = message.Text.Replace(" \f ", "\f");
                message.Text = message.Text.Replace(" $ ", "$");
                message.Text = message.Text.Replace(" . ", ".");
                message.Text = message.Text.Replace(" , ", ",");
                message.Text = message.Text.Replace(" ? ", "?");
                message.Text = message.Text.Replace(" ! ", "!");
                message.Text = message.Text.Replace(" - ", "-");
                message.Text = message.Text.Replace(" \" ", "\"");
                message.Text = message.Text.Replace(" © ", "©");
                message.Text = message.Text.Replace(" … ", "...");
                message.Text = message.Text.Replace(" : ", ":");
                message.Text = message.Text.Replace(" & ", "&");
            }

            // Save the MST.
            mst.Save();
        }

        /// <summary>
        /// Generates a text to speech audio file for a message table entry.
        /// </summary>
        /// <param name="messageEntry">The message table entry we're processing.</param>
        /// <param name="ModDirectory">The mod directory to place the generated XMAs into.</param>
        public static async Task GenerateTTS(Message messageEntry, string ModDirectory)
        {
            // Keep track of what sound we're one.
            int sounds = 0;

            // Set up the text that needs to be TTS'd.
            string[] messages = messageEntry.Text.Split("\f");
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = messages[i].Replace('\n', ' ');
                messages[i] = messages[i].Replace("$", "");
            }

            // Loop through each placeholder in this message.
            foreach (string placeholder in messageEntry.Placeholders)
            {
                // Only do anything to sound placeholders.
                if (placeholder.Contains("sound"))
                {
                    // Get the name of the sound file.
                    string name = placeholder.Replace("sound(", "");
                    name = name.Replace(")", "");

                    // Initialize a new instance of the SpeechSynthesizer.  
                    SpeechSynthesizer synth = new();

                    // Select a random voice from the installed voices.
                    var voices = synth.GetInstalledVoices();
                    synth.SelectVoice(voices[MainWindow.Randomiser.Next(voices.Count)].VoiceInfo.Name);

                    // Configure the audio output.   
                    synth.SetOutputToWaveFile($@"{MainWindow.TemporaryDirectory}\tempWavs\tts\{name}.wav", new SpeechAudioFormatInfo(48000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

                    // Set up the PromptBuilder.
                    PromptBuilder builder = new();
                    builder.AppendText(messages[sounds]);

                    // Do the actual Text to Speech process.
                    synth.Speak(builder);

                    // Dispose of the now useless SpeechSynthesizer
                    synth.Dispose();

                    // Increment our sound tracker.
                    sounds++;

                    // Normalise the generated WAV. Still feels quiet but oh well...
                    await Task.Run(() => Helpers.WavNormalise($@"{MainWindow.TemporaryDirectory}\tempWavs\tts\{name}.wav"));

                    // Convert WAV file to XMA.
                    await Task.Run(() => Helpers.XMAEncode($@"{MainWindow.TemporaryDirectory}\tempWavs\tts\{name}.wav", $@"{ModDirectory}\xenon\sound\voice\e\{name}.xma"));
                }
            }
        }

        /// <summary>
        /// Randomises button icon placeholders in an MST.
        /// </summary>
        /// <param name="mstFile">The path to the MST to process.</param>
        /// <param name="TextButtons">The list of valid button icons to pick from.</param>
        public static async Task RandomiseButtonIcons(string mstFile, List<string> TextButtons)
        {
            // Load the MST.
            MessageTable mst = new(mstFile);

            // Loop through each Message Entry in this MST.
            foreach (Message? message in mst.Data.Messages)
            {
                // Check this Message Entry actually has placeholders.
                if (message.Placeholders != null)
                {
                    // Loop through the placeholders.
                    for (int i = 0; i < message.Placeholders.Length; i++)
                    {
                        // If this placeholder is a picture one, replace it.
                        if (message.Placeholders[i].Contains("picture"))
                            message.Placeholders[i] = TextButtons[MainWindow.Randomiser.Next(TextButtons.Count)];
                    }
                }
            }

            // Save the MST.
            mst.Save();
        }

        /// <summary>
        /// Randomly colours characters in the messages within an MST.
        /// </summary>
        /// <param name="mstFile">The path to the MST to process.</param>
        /// <param name="textColourWeight">The likelyhood for a character to be coloured.</param>
        public static async Task RandomColours(string mstFile, int textColourWeight)
        {
            // Load the MST.
            MessageTable mst = new(mstFile);

            // Loop through each Message Entry in this MST.
            foreach (Message? message in mst.Data.Messages)
            {
                // Set up a system to handle placeholders.
                List<string> Placeholders = new();
                List<string>? oldPlaceholders = null;
                if (message.Placeholders != null)
                    oldPlaceholders = message.Placeholders.ToList();

                // Set up a string to store our edited message.
                string newMessage = string.Empty;

                // Set up a system to determine whether this enables or disables colours.
                bool isColoured = false;

                // Figure out existing placeholders.
                int placeholderCount = 0;

                // Loop through each character in this message.
                for (int i = 0; i < message.Text.Length; i++)
                {
                    // If this character is a $ and placeholders already exist, then add the approriate placeholder to our new list.
                    if (message.Text[i] == '$')
                    {
                        if (oldPlaceholders != null)
                        {
                            Placeholders.Add(oldPlaceholders[placeholderCount]);
                            placeholderCount++;
                        }
                    }

                    // If not, then roll a number between 0 and 100, if it's smaller than or equal to textColourWeight, then proceed.
                    else if (MainWindow.Randomiser.Next(0, 101) <= textColourWeight)
                    {
                        // Add a new $ placeholder.
                        newMessage += '$';

                        // Add an rgba entry or a color entry depending on whether or not we're starting or stopping the colouration.
                        if (!isColoured)
                            Placeholders.Add($"rgba({MainWindow.Randomiser.Next(0, 256)}, {MainWindow.Randomiser.Next(0, 256)}, {MainWindow.Randomiser.Next(0, 256)})");
                        if (isColoured)
                            Placeholders.Add("color");

                        // Invert our colour status.
                        isColoured = !isColoured;
                    }

                    // Add this character to the new string.
                    newMessage += message.Text[i];
                }

                // If we haven't added a final colour entry, then add it manually.
                if (isColoured)
                {
                    Placeholders.Add("color");
                    newMessage += '$';
                }

                // Save our new message and placeholders over the original ones.
                message.Text = newMessage;
                message.Placeholders = Placeholders.ToArray();
            }

            // Save the MST.
            mst.Save();
        }

        /// <summary>
        /// Replaces ev0026_12_sn in e0026 and e0225 with Lost in Translation's best line.
        /// </summary>
        /// <param name="mstFile">The mst file to edit.</param>
        public static async Task LostInTranslation(string mstFile)
        {
            // Load the MST.
            MessageTable mst = new(mstFile);

            // Loop through each Message Entry in this MST to replace the correct one.
            foreach (Message? message in mst.Data.Messages)
                if (message.Name == "ev0026_12_sn")
                    message.Text = "Thanks for the money";

            // Save the MST.
            mst.Save();
        }
    }
}
