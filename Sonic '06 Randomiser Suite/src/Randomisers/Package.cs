using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace Sonic_06_Randomiser_Suite
{
    class Package
    {
        /// <summary>
        /// Use the PKGTool to encode/decode the given file.
        /// </summary>
        public static void PKGTool(string filepath)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = Program.pkgtool,
                WorkingDirectory = Path.GetDirectoryName(Program.pkgtool),
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = filepath
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            // Erase the TXT file once encoded as PKG
            if (Path.GetExtension(filepath) == ".txt") File.Delete(filepath);
        }

        //Slightly Messy, could do with a bit of clean up
        public static void PackageAnimationRandomiser(string package, Random rng)
        {
            List<string> availableReferences = new List<string>();
            List<int> usedNumbers = new List<int>();
            int index;

            PKGTool($"{package}.pkg");

            List<string> packageList = File.ReadAllLines($"{package}.txt").ToList(), editedPackage = packageList;
            bool keyFound = false;
            int lineCount = 0;

            foreach (string entry in packageList)
            {
                if (entry == $"\"motion\"" && !entry.EndsWith(";")) keyFound = true;

                if (keyFound)
                {
                    if (entry.StartsWith($"\t\"") && entry.EndsWith(".xnm\";") && !entry.Contains("face_"))
                    {
                        string[] splitEntry = entry.Split('\"');
                        availableReferences.Add(splitEntry[3]);
                    }
                }
                lineCount++;
            }

            keyFound = false;
            lineCount = 0;
            packageList = File.ReadAllLines($"{package}.txt").ToList();

            foreach (string entry in packageList)
            {
                if (entry == $"\"motion\"" && !entry.EndsWith(";")) keyFound = true;

                if (keyFound)
                {
                    if (entry.StartsWith($"\t\"") && entry.EndsWith(".xnm\";") && !entry.Contains("face_"))
                    {
                        string[] splitEntry = entry.Split('\"');

                        index = rng.Next(availableReferences.Count);
                        if (usedNumbers.Contains(index))
                        {
                            do { index = rng.Next(availableReferences.Count); }
                            while (usedNumbers.Contains(index));
                        }
                        usedNumbers.Add(index);
                        splitEntry[3] = availableReferences[index];
                        editedPackage[lineCount] = string.Join("\"", splitEntry);
                    }
                }
                lineCount++;
            }

            File.WriteAllLines($"{package}.txt", editedPackage);
            PKGTool($"{package}.txt");
        }
    }
}
