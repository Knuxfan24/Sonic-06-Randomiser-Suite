using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    class CollisionRandomisation
    {
        static public void Decompile(string filepath, out string convertedColi)
        {
            ProcessStartInfo binSession;
            binSession = new ProcessStartInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "External Software\\s06collision.py"), $"\"{Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileName(filepath))}\"")
            {
                WorkingDirectory = Path.GetDirectoryName(filepath),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var Decode = Process.Start(binSession);
            Decode.WaitForExit();
            Decode.Close();

            convertedColi = $"{Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath))}.obj";
        }
        static public void RotationSwap(string filepath, Random rng)
        {
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                if (line.StartsWith("v "))
                {
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

                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                if (line.StartsWith("vn"))
                {
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

                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void PropertyRandomiser(string filepath, Random rng, bool respectWalls, bool respectWater, bool respectDeath)
        {
            int index;
            string[] editedLua = File.ReadAllLines(filepath);

            int lineNum = 0;
            foreach (string line in editedLua)
            {
                StringBuilder surfaceType = new StringBuilder("00000000", 8);
                if (line.StartsWith("g"))
                {
                    string[] tempLine = line.Split(' ');
                    index = rng.Next(CollisionPropertiesForm.validSurfaces.Count);

                    if (respectWalls)
                    {
                        if (tempLine[1][3].ToString() == "1")
                        {
                            surfaceType.Remove(3, 1);
                            surfaceType.Insert(3, "1");
                        }
                        if (tempLine[1][3].ToString() == "4")
                        {
                            surfaceType.Remove(3, 1);
                            surfaceType.Insert(3, "4");
                        }
                        if (tempLine[1][0].ToString() == "1")
                        {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "1");
                        }
                        if (tempLine[1][0].ToString() == "8")
                        {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "8");
                        }
                        if (tempLine[1][1].ToString() == "8")
                        {
                            surfaceType.Remove(1, 1);
                            surfaceType.Insert(1, "8");
                        }
                        if (tempLine[1][2].ToString() == "2")
                        {
                            surfaceType.Remove(2, 1);
                            surfaceType.Insert(2, "2");
                        }
                        if (tempLine[1][2].ToString() == "8")
                        {
                            surfaceType.Remove(2, 1);
                            surfaceType.Insert(2, "8");
                        }
                    }
                    if (respectWater)
                    {
                        if (tempLine[1][3].ToString() == "8")
                        {
                            surfaceType.Remove(3, 1);
                            surfaceType.Insert(3, "8");
                        }
                        if (tempLine[1][0].ToString() == "4")
                        {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "4");
                        }
                        if (tempLine[1][0].ToString() == "6")
                        {
                            surfaceType.Remove(0, 1);
                            surfaceType.Insert(0, "6");
                        }
                    }
                    if (respectDeath)
                    {
                        if (tempLine[1][2].ToString() == "1")
                        {
                            surfaceType.Remove(2, 1);
                            surfaceType.Insert(2, "1");
                        }
                    }

                    surfaceType.Remove(7, 1);
                    surfaceType.Insert(7, CollisionPropertiesForm.validSurfaces[index]);
                    tempLine[1] = tempLine[1] + "@" + surfaceType.ToString();

                    editedLua[lineNum] = string.Join(" ", tempLine);
                }
                lineNum++;
            }
            File.WriteAllLines(filepath, editedLua);
        }
        static public void Compile(string filepath, string originalFile)
        {
            ProcessStartInfo binSession;
            binSession = new ProcessStartInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "External Software\\s06col.exe"), $"\"{Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileName(filepath))}\"")
            {
                WorkingDirectory = Path.GetDirectoryName(filepath),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            var Compile = Process.Start(binSession);
            Compile.WaitForExit();
            Compile.Close();

            File.Delete(filepath);
        }
    }
}
