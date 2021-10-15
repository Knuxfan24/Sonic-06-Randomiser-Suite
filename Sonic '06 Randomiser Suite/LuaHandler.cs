using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Sonic_06_Randomiser_Suite
{
    class LuaHandler
    {
        /// <summary>
        /// Uses unlub to decompile a lua binary.
        /// </summary>
        /// <param name="luaFile">The path to the lua file to decompile</param>
        public static void Decompile(string luaFile)
        {
            // Read the lua file into a string array.
            string[] readText = File.ReadAllLines(luaFile);

            // Check if the file starts with the string "LuaP". While not exclusive to compiled lua binaries, it's a good enough indication that it needs decompiling.
            if (readText[0].Contains("LuaP"))
            {
                // Run unlub.jar through a Java process to decompile it.
                using (Process process = new())
                {
                    process.StartInfo.FileName = "java.exe";
                    process.StartInfo.Arguments = $"-jar \"{Environment.CurrentDirectory}\\ExternalResources\\unlub.jar\" \"{luaFile}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    // Grab unlub's output so we can write it back to a .lub file.
                    StringBuilder outputBuilder = new StringBuilder();
                    process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();

                    // Write unlub's output to the original file passed to this function.
                    File.WriteAllText(luaFile, outputBuilder.ToString());
                }
            }
        }
    }
}
