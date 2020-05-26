using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System;

namespace Sonic_06_Randomiser_Suite
{
    class Lua
    {
        /// <summary>
        /// Decompiles a Lua script
        /// </summary>
        public static void Decompile(string _file) {
            string[] readText = File.ReadAllLines(_file); //Read the Lub into an array

            if (readText[0].Contains("LuaP")) {
                using (Process process = new Process()) {
                    process.StartInfo.FileName = "java.exe";
                    process.StartInfo.Arguments = $"-jar \"{Program.unlub}\" \"{_file}\"";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.CreateNoWindow = true;

                    StringBuilder outputBuilder = new StringBuilder();
                    process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.WaitForExit();

                    File.WriteAllText(_file, outputBuilder.ToString());
                }
            }
        }

        public static void ParameterRandomiser(string location, string parameter, float minValue, float maxValue, Random rng) {
            string[] script = File.ReadAllLines(location);
            int lineCount = 0;

            foreach (string line in script) {
                if (line.StartsWith(parameter)) {
                    string[] split = line.Split(' ');
                    split[2] = Math.Round(rng.NextDouble() * (maxValue - minValue) + minValue, 1).ToString();
                    script[lineCount] = string.Join(" ", split);
                    break;
                }
                lineCount++;
            }

            File.WriteAllLines(location, script);
        }
    }
}
