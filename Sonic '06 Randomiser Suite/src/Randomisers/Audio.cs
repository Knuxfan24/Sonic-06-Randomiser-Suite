using System;

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
    }
}
