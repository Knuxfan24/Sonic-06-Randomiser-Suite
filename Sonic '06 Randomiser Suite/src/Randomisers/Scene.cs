using System;
using Sonic_06_Randomiser_Suite.Serialisers;

namespace Sonic_06_Randomiser_Suite
{
    class Scene
    {
        /// <summary>
        /// Randomises all scene parameters for the specified light type in scene parameters
        /// </summary>
        public static void RandomiseLight(string[] editedLub, string lightType, Random rng) {
            int lineNum = 0;

            foreach (string line in editedLub)
            {
                if (line.Contains(lightType + " = {") && !line.Contains("}"))
                {
                    // Not a light preset
                    if (editedLub[lineNum + 1].Contains("Color"))
                    {
                        // Change the next four numbers to a random number from 0 to 1, rounded to 2 decimal points
                        editedLub[lineNum + 2] = $"      {Math.Round(rng.NextDouble(), 2)},";
                        editedLub[lineNum + 3] = $"      {Math.Round(rng.NextDouble(), 2)},";
                        editedLub[lineNum + 4] = $"      {Math.Round(rng.NextDouble(), 2)},";
                        editedLub[lineNum + 5] = $"      {Math.Round(rng.NextDouble(), 2)}";
                    }

                    // Light preset
                    else if (editedLub[lineNum + 2].Contains("Color"))
                    {
                        // Change the next four numbers to a random number from 0 to 1, rounded to 2 decimal points
                        editedLub[lineNum + 3] = $"      {Math.Round(rng.NextDouble(), 2)},";
                        editedLub[lineNum + 4] = $"      {Math.Round(rng.NextDouble(), 2)},";
                        editedLub[lineNum + 5] = $"      {Math.Round(rng.NextDouble(), 2)},";
                        editedLub[lineNum + 6] = $"      {Math.Round(rng.NextDouble(), 2)}";
                    }
                }
                lineNum++;
            }
        }

        /// <summary>
        /// Randomises light direction in scene parameters
        /// </summary>
        public static void RandomiseLightDirection(string[] editedLub, Random rng) {
            int lineNum = 0;

            foreach (string line in editedLub)
            {
                if (line.Contains("Direction_3dsmax = {") && !line.Contains("}"))
                {
                    // Change the next three numbers to a random number from -1 to 1, rounded to 6 decimal points
                    editedLub[lineNum + 2] = $"      {Math.Round(rng.NextDouble() * (1 - -1) + -1, 6)},";
                    editedLub[lineNum + 3] = $"      {Math.Round(rng.NextDouble() * (1 - -1) + -1, 6)},";
                    editedLub[lineNum + 4] = $"      {Math.Round(rng.NextDouble() * (1 - -1) + -1, 6)}";
                }
                lineNum++;
            }
        }

        /// <summary>
        /// Randomises fog colour in scene parameters
        /// </summary>
        public static void RandomiseFogColour(string[] editedLub, Random rng) {
            int lineNum = 0;

            foreach (string line in editedLub)
            {
                if (line.Contains("BRay = {") && !line.Contains("}"))
                {
                    // Change the next three numbers to a random number from 0 to 0.25, rounded to 2 decimal points
                    editedLub[lineNum + 1] = $"    {Math.Round(rng.NextDouble() * (0.25 - 0) + 0, 2)},";
                    editedLub[lineNum + 2] = $"    {Math.Round(rng.NextDouble() * (0.25 - 0) + 0, 2)},";
                    editedLub[lineNum + 3] = $"    {Math.Round(rng.NextDouble() * (0.25 - 0) + 0, 2)},";
                }
                lineNum++;
            }
        }

        /// <summary>
        /// Randomises fog density in scene parameters
        /// </summary>
        public static void RandomiseFogDensity(string[] editedLub, Random rng) {
            int lineNum = 0;

            foreach (string line in editedLub)
            {
                if (line.Contains("BRay = {") && !line.Contains("}"))
                {
                    editedLub[lineNum + 4] = $"    {rng.NextDouble() * (0.0010 - 0.000085) + 0.000085}"; // Set the 4th line from this to a number between 0.000085 and 0.0010
                }
                lineNum++;
            }
        }

        /// <summary>
        /// Randomises environment maps in scene parameters
        /// </summary>
        public static void RandomiseEnvironmentMaps(string[] editedLub, Random rng) {
            string[] Cubemaps = Resources.ParseLineBreaks(Properties.Resources.S06Cubemaps);
            int lineNum = 0;

            foreach (string line in editedLub)
            {
                if (line.Contains("EnvMap = {") && !line.Contains("}"))
                {
                    editedLub[lineNum + 1] = Cubemaps[rng.Next(Cubemaps.Length)]; // Set the next line to an entry in the Cubemaps Text File
                }
                lineNum++;
            }
        }
    }
}
