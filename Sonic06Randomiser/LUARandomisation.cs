using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sonic06Randomiser
{
    class LUARandomisation
    {
        static public void SetupRandomiser(bool randomMusic, List<string> validMusic, bool lightColours, bool lightDirection, bool spoilerLog, bool randomiseFolder, int outputFolderType, string filepath, string output, string rndSeed)
        {
            string lubName = ""; //Set up lubName because variables suck at being defined in if statements
            Random rnd = new Random();
            string logSeed = rndSeed;
            if (outputFolderType == 0 && output == "") { outputFolderType = 1; } //Default to saving in the program directory if the user doesn't specify a path with Output Folder Type set to custom.

            if (!randomiseFolder)
            {
                #region Load the lub if we're only randomising one
                string file = filepath;
                lubName = filepath.Remove(0, Path.GetDirectoryName(filepath).Length);
                lubName = lubName.Remove(lubName.Length - 4);
                lubName = lubName.Replace("\\", "");
                StreamReader sr = new StreamReader(file);
                #endregion

                #region Set up the Random Number Generation
                if (rndSeed != "")
                {
                    rndSeed = rndSeed + lubName;
                    rnd = new Random(rndSeed.GetHashCode()); //Override RND with seed if the box wasn't blank
                }
                #endregion

                StreamWriter spoiler;
                #region Bodge Spoiler Log
                switch (outputFolderType)
                {
                    case 0: //Custom
                        spoiler = new StreamWriter(output + "\\" + lubName + "_log.txt");
                        break;
                    case 1: //Source
                        spoiler = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                        break;
                    case 2: //Program
                        spoiler = new StreamWriter(lubName + "_log.txt");
                        break;
                    default: //Need this otherwise spoiler becomes invalid for some god forsaken reason
                        spoiler = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                        break;
                }

                spoiler.WriteLine("Seed: " + logSeed);
                spoiler.WriteLine("Randomise Music: " + randomMusic);
                if (randomMusic)
                {
                    spoiler.WriteLine("Valid Music Tracks:");
                    validMusic.ForEach(i => spoiler.Write("{0}, ", i));
                    spoiler.Write(Environment.NewLine);
                }
                spoiler.WriteLine("Randomise Light Colours: " + lightColours);
                spoiler.WriteLine("Randomise Light Direction: " + lightDirection);
                spoiler.Write(Environment.NewLine);
                #endregion

                //Do the randomisation functions here
                string lub = "Not Used";
                ReadLUB(sr, rnd, randomMusic, validMusic, lightColours, lightDirection, spoilerLog, spoiler, randomiseFolder, outputFolderType, filepath, output, rndSeed, lubName, lub);
            }
            else
            {
                #region Load the Folder of lubs
                string[] lubs = Directory.GetFiles(filepath, "*.lub", SearchOption.AllDirectories);
                Console.WriteLine("Found " + lubs.Length + " lub files");
                foreach (string lub in lubs)
                {
                    //Load lub
                    lubName = lub.Remove(0, Path.GetDirectoryName(lub).Length);
                    lubName = lubName.Remove(lubName.Length - 4);
                    lubName = lubName.Replace("\\", "");
                    StreamReader sr = new StreamReader(lub);
                    Console.WriteLine(lubName);
                    #endregion

                    #region Set up the Random Number Generation
                    if (rndSeed != "")
                    {
                        rndSeed = rndSeed + lubName;
                        rnd = new Random(rndSeed.GetHashCode()); //Override RND with seed if the box wasn't blank
                    }
                    #endregion

                    StreamWriter spoiler;
                    #region Bodge Spoiler Log
                    switch (outputFolderType)
                    {
                        case 0: //Custom
                            spoiler = new StreamWriter(output + "\\" + lubName + "_log.txt");
                            break;
                        case 1: //Source
                            spoiler = new StreamWriter(filepath + "\\" + lubName + "_log.txt");
                            break;
                        case 2: //Program
                            spoiler = new StreamWriter(lubName + "_log.txt");
                            break;
                        default: //Need this otherwise spoiler becomes invalid for some god forsaken reason
                            spoiler = new StreamWriter(filepath.Remove(filepath.Length - 4) + "_log.txt");
                            break;
                    }

                    spoiler.WriteLine("Seed: " + logSeed);
                    spoiler.WriteLine("Randomise Enemies: " + randomMusic);
                    if (randomMusic)
                    {
                        spoiler.WriteLine("Valid Music Tracks:");
                        validMusic.ForEach(i => spoiler.Write("{0}, ", i));
                        spoiler.Write(Environment.NewLine);
                    }
                    spoiler.Write(Environment.NewLine);
                    #endregion

                    //Do the randomisation functions here
                    ReadLUB(sr, rnd, randomMusic, validMusic, lightColours, lightDirection, spoilerLog, spoiler, randomiseFolder, outputFolderType, filepath, output, rndSeed, lubName, lub);
                }
            }
        }

        static public void ReadLUB(StreamReader sr, Random rnd, bool randomMusic, List<string> validMusic, bool lightColours, bool lightDirection, bool spoilerLog, StreamWriter spoiler, bool randomiseFolder, int outputFolderType, string filepath, string output, string rndSeed, string lubName, string lub)
        {
            int index = 0; //Int to use as a host for the random number generation
            String line;
            string prevLine = "";
            bool isColour = false;
            float colourValue = 0;
            double colourValueTrue = 0;
            bool isDirection = false;

            bool foundMusic = false;
            bool foundLightColour = false;
            bool foundLightDirection = false;

            int colourCount = 0;
            var lines = new List<string>() { };
            line = sr.ReadLine();

            while (line != null)
            {
                if (line.Contains("Game.PlayBGM") && randomMusic)
                {
                    var lineLog = line.Substring(line.IndexOf('\"') + 1);
                    lineLog = lineLog.Remove(lineLog.Length - 2, 2);
                    spoiler.Write("Stage music (previously '" + lineLog + "') became: '");
                    //Randomise music here
                    index = rnd.Next(validMusic.Count);
                    line = "    Game.PlayBGM(\"" + validMusic[index] + "\")";
                    spoiler.Write(validMusic[index] + "'");
                    spoiler.Write(Environment.NewLine);
                    foundMusic = true;
                }
                if (lightColours)
                {
                    if (prevLine.Contains("Color"))
                    {
                        colourCount = 0;
                        isColour = true;
                    }
                    if (isColour && colourCount < 3)
                    {
                        //To Do: Figure out a way to write the old colours into the Spoiler Log.
                        colourCount++;
                        colourValue = (float)rnd.Next(0, 256) / 255;
                        colourValueTrue = Math.Round(colourValue, 2);
                        line = "      " + colourValueTrue.ToString() + ",";
                        foundLightColour = true;
                        if (colourCount == 3) { isColour = false; }
                    }
                }
                if (lightDirection)
                {
                    if (prevLine.Contains("Position"))
                    {
                        colourCount = 0;
                        isDirection = true;
                    }
                    if (isDirection && colourCount < 3)
                    {
                        //To Do: Figure out a way to write the old colours into the Spoiler Log.
                        colourCount++;
                        colourValue = (float)rnd.Next(-256, 256) / 255;
                        colourValueTrue = Math.Round(colourValue, 6);
                        line = "        " + colourValueTrue.ToString() + ",";
                        foundLightDirection = true;
                        if (colourCount == 3) { isDirection = false; }
                    }
                }
                lines.Add(line);
                prevLine = line;
                line = sr.ReadLine();
            }
            sr.Close();
            if (randomMusic && !foundMusic) { spoiler.WriteLine("Music Randomiser enabled, but no music definition was found."); }
            if (lightColours && !foundLightColour) { spoiler.WriteLine("Light Colour Randomiser enabled, but no light colours were found."); }
            if (lightDirection && !foundLightDirection) { spoiler.WriteLine("Light Direction Randomiser enabled, but no light direction parameters were found."); }
            spoiler.Close();
            lubSave(randomiseFolder, outputFolderType, output, lubName, filepath, spoilerLog, lub, lines);
        }

        static public void lubSave(bool randomiseFolder, int outputFolderType, string output, string lubName, string filepath, bool spoilerLog, string lub, List<string> lines)
        {
            switch (outputFolderType)
            {
                case 0: //Custom
                    File.Delete(output + "\\" + lubName + ".lub");
                    File.WriteAllLines(output + "\\" + lubName + ".lub", lines);
                    if (!spoilerLog) { File.Delete(output + "\\" + lubName + "_log.txt"); }
                    break;
                case 1: //Source
                    if (!randomiseFolder)
                    {
                        File.Delete(filepath.Remove(filepath.Length - 4) + ".set");
                        File.WriteAllLines(filepath.Remove(filepath.Length - 4) + ".lub", lines);
                        if (!spoilerLog) { File.Delete(filepath.Remove(filepath.Length - 4) + "_log.txt"); }
                    }
                    if (randomiseFolder)
                    {
                        File.Delete(Path.GetDirectoryName(lub) + "\\" + lubName + ".set");
                        File.WriteAllLines(Path.GetDirectoryName(lub) + "\\" + lubName + ".lub", lines);
                        if (!spoilerLog) { File.Delete(filepath + "\\" + lubName + "_log.txt"); }
                    }
                    break;
                case 2: //Program
                    File.Delete(lubName + ".set");
                    File.WriteAllLines(lubName + ".lub", lines);
                    if (!spoilerLog) { File.Delete(lubName + "_log.txt"); }
                    break;
            }
        }
    }
}
