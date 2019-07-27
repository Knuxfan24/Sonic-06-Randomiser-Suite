using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    class MusicRandomisation
    {
        static public void MusicRandomiser(string filepath, Random rng, List<string> validMusic)
        {
            string[] editedLua = File.ReadAllLines(filepath);
            int index;
            int lineNum = 0;

            foreach (string line in editedLua)
            {
                if (line.Contains("Game.PlayBGM"))
                {
                    string[] tempLine = line.Split('"');
                    index = rng.Next(validMusic.Count);
                    tempLine[1] = validMusic[index];
                    editedLua[lineNum] = string.Join("\"", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
    }
}
