using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Sonic_06_Randomiser_Suite
{
    /// <summary>
    /// All this code is old & hacky. Could do with being rewritten at a later point?
    /// </summary>
    class Collision
    {
        public static void Decode(string filepath, out string convertedColi) {
            ProcessStartInfo binSession = new ProcessStartInfo(Program.CollisionExporter) {
                Arguments = $"\"{Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileName(filepath))}\"",
                WorkingDirectory = Path.GetDirectoryName(filepath),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process Decode = Process.Start(binSession);
            Decode.WaitForExit();
            Decode.Close();

            convertedColi = $"{Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath))}.obj";
        }

        public static void RotationSwap(string filepath) {
            string[] editedObj = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedObj)
            {
                // Vertex
                if (line.StartsWith("v ")) {
                    string[] tempLine = line.Split(' ');
                    var zOld = tempLine[3];

                    tempLine[3] = tempLine[2];
                    if (zOld.Contains("-"))
                        tempLine[2] = zOld.Remove(0, 1);
                    else
                        tempLine[2] = "-" + zOld;

                    editedObj[lineNum] = string.Join(" ", tempLine);
                }

                // Normal
                else if (line.StartsWith("vn")) {
                    string[] tempLine = line.Split(' ');
                    var zOld = tempLine[3];

                    tempLine[3] = tempLine[2];
                    if (zOld.Contains("-"))
                    {
                        tempLine[2] = zOld.Remove(0, 1);
                    }
                    else
                    {
                        tempLine[2] = "-" + zOld;
                    }

                    editedObj[lineNum] = string.Join(" ", tempLine);
                }

                lineNum++;
            }

            File.WriteAllLines(filepath, editedObj);
        }

        public static void PropertyRandomiser(string filepath, Random rng, bool respectWalls, bool respectWater, bool respectDeath) {
            string[] editedObj = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedObj)
            {
                StringBuilder surfaceType = new StringBuilder("00000000", 8);
                if (line.StartsWith("g"))
                {
                    string[] tempLine = line.Split(' ');

                    if (respectWalls)
                    {
                        // Wall
                        if (tempLine[1][3].ToString() == "1") {
                            surfaceType.Remove(3, 1);
                            surfaceType.Insert(3, "1");
                        }

                        // No Stand
                        if (tempLine[1][3].ToString() == "4") {
                            surfaceType.Remove(3, 1);
                            surfaceType.Insert(3, "4");
                        }

                        // Fall if player stops moving
                        if (tempLine[1][0].ToString() == "1") {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "1");
                        }

                        // Climbable Wall
                        if (tempLine[1][0].ToString() == "8") {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "8");
                        }

                        // Unknown
                        if (tempLine[1][1].ToString() == "8") {
                            surfaceType.Remove(1, 1);
                            surfaceType.Insert(1, "8");
                        }

                        // Player Only Collision
                        if (tempLine[1][2].ToString() == "2") {
                            surfaceType.Remove(2, 1);
                            surfaceType.Insert(2, "2");
                        }

                        // Unknown
                        if (tempLine[1][2].ToString() == "8") {
                            surfaceType.Remove(2, 1);
                            surfaceType.Insert(2, "8");
                        }
                    }

                    if (respectWater)
                    {
                        // Water
                        if (tempLine[1][3].ToString() == "8") {
                            surfaceType.Remove(3, 1);
                            surfaceType.Insert(3, "8");
                        }

                        // Water
                        if (tempLine[1][0].ToString() == "4") {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "4");
                        }
                    }

                    if (respectDeath)
                    {
                        // Corner Damage
                        if (tempLine[1][0].ToString() == "2") {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "2");
                        }

                        // Deadly Water
                        if (tempLine[1][0].ToString() == "6") {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "6");
                        }

                        // Damage
                        if (tempLine[1][1].ToString() == "8") {
                            surfaceType.Remove(1, 1);
                            surfaceType.Insert(1, "8");
                        }

                        // Death
                        if (tempLine[1][2].ToString() == "1") {
                            surfaceType.Remove(2, 1);
                            surfaceType.Insert(2, "1");
                        }
                    }

                    surfaceType.Remove(7, 1);
                    surfaceType.Insert(7, Main.Surfaces[rng.Next(Main.Surfaces.Count)]);
                    tempLine[1] = tempLine[1] + "@" + surfaceType.ToString();

                    editedObj[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedObj);
        }

        public static void Encode(string filepath) {
            ProcessStartInfo binSession = new ProcessStartInfo(Program.CollisionImporter) {
                Arguments = $"\"{Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileName(filepath))}\"",
                WorkingDirectory = Path.GetDirectoryName(filepath),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process Encode = Process.Start(binSession);
            Encode.WaitForExit();
            Encode.Close();

            File.Delete(filepath);
        }
    }
}
