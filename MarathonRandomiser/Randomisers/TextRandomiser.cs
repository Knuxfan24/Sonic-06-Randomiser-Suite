using Marathon.Formats.Text;
using System.Linq;

namespace MarathonRandomiser
{
    internal class TextRandomiser
    {
        /// <summary>
        /// Shuffles all the text entries in all the game's message table files around.
        /// </summary>
        /// <param name="mstFiles">Array of all the message table files.</param>
        /// <param name="eventArc">The path to the already unpacked event.arc.</param>
        /// <param name="textArc">The path to the already unpacked text.arc.</param>
        /// <param name="languages">The list of valid language codes to include.</param>
        public static async Task ShuffleText(string[] mstFiles, string eventArc, string textArc, List<string> languages)
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
        public static async Task TextGenerator(string mstFile, string[] wordList, bool? enforce)
        {
            // Load the MST.
            MessageTable mst = new(mstFile);

            // Loop through each Message Entry in this MST.
            foreach (Message? message in mst.Data.Messages)
            {
                // Edit the New Lines, New Text Boxes and Placeholder Calls so they can be preserved.
                message.Text = message.Text.Replace("\n", " \n ");
                message.Text = message.Text.Replace("\f", " \f ");
                message.Text = message.Text.Replace("$", " $ ");

                // Split this string into an array.
                string[] split = message.Text.Split(' ');

                // Loop through and pick a random word for each array entry.
                for (int i = 0; i < split.Length; i++)
                {
                    // Check this isn't empty or a control character.
                    if (split[i] != "\n" && split[i] != "\f" && split[i] != "$" && split[i] != "")
                    {
                        // If we're not enforcing the length of the words, pick any of them.
                        if (enforce == false)
                            split[i] = wordList[MainWindow.Randomiser.Next(wordList.Length)].ToUpper();

                        // If we ARE enforcing the length of the words, then pick until it's the same length or we just give up.
                        else
                        {
                            // Choose a string to start with.
                            string chosenString = wordList[MainWindow.Randomiser.Next(wordList.Length)].ToUpper();

                            // Start with one million attempts.
                            int remainingAttempts = 1000000;

                            // If the string is longer and we have attempts left, try again and deduct an attempt.
                            while(remainingAttempts != 0 && chosenString.Length != split[i].Length)
                            {
                                chosenString = wordList[MainWindow.Randomiser.Next(wordList.Length)].ToUpper();
                                remainingAttempts--;
                            }

                            // If we have no attempts left, give up and print it to the debug output. Else, actually save the edited word.
                            if (remainingAttempts == 0)
                                System.Diagnostics.Debug.WriteLine($"Ran out of attempts to generate a word to replace \"{split[i]}\" for '{message.Name}' in '{mstFile}'.");
                            else
                                split[i] = chosenString;

                        }
                    }
                }

                // Rejoin the string and reverse the earlier edits to avoid redudant spaces.
                message.Text = String.Join(" ", split);
                message.Text = message.Text.Replace(" \n ", "\n");
                message.Text = message.Text.Replace(" \f ", "\f");
                message.Text = message.Text.Replace(" $ ", "$");
            }

            // Save the MST.
            mst.Save();
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
    }
}
