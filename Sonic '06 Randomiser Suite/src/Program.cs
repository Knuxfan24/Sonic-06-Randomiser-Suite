using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Sonic_06_Randomiser_Suite
{
    static class Program
    {
        public static readonly string GlobalVersionNumber = "Version 1.0";

        public static string ApplicationData   = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                             arctool           = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\arctool.exe",
                             unlub             = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\unlub.jar";

        [STAThread]

        /// <summary>
        /// Main entry point
        /// </summary>
        static void Main() {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            #region Write required pre-requisites to the Tools directory
            if (!Directory.Exists($"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\"))
                Directory.CreateDirectory($"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\");

            if (!File.Exists(arctool))
                File.WriteAllBytes(arctool, Properties.Resources.arctool);

            if (!File.Exists(unlub))
                File.WriteAllBytes(unlub, Properties.Resources.unlub);
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }

    public static class ProcessAsyncHelper
    {
        public static async Task<ProcessResult> ExecuteShellCommand(string command, string arguments, string working, int timeout) {
            var result = new ProcessResult();

            using (var process = new Process()) {
                // If you run bash-script on Linux it is possible that ExitCode can be 255.
                // To fix it you can try to add '#!/bin/bash' header to the script.

                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = working;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                var outputBuilder = new StringBuilder();
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) => {
                    // The output stream has been closed i.e. the process has terminated
                    if (e.Data == null) {
                        outputCloseEvent.SetResult(true);
                    } else {
                        outputBuilder.AppendLine(e.Data);
                    }
                };

                var errorBuilder = new StringBuilder();
                var errorCloseEvent = new TaskCompletionSource<bool>();

                process.ErrorDataReceived += (s, e) => {
                    // The error stream has been closed i.e. the process has terminated
                    if (e.Data == null) {
                        errorCloseEvent.SetResult(true);
                    } else {
                        errorBuilder.AppendLine(e.Data);
                    }
                };

                bool isStarted;

                try { isStarted = process.Start(); }
                catch (Exception error) {
                    // Usually it occurs when an executable file is not found or is not executable

                    result.Completed = true;
                    result.ExitCode = -1;
                    result.Error = error.Message;

                    isStarted = false;
                }

                if (isStarted) {
                    // Reads the output stream first and then waits because deadlocks are possible
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Creates task to wait for process exit using timeout
                    var waitForExit = WaitForExitAsync(process, timeout);

                    // Create task to wait for process exit and closing all output streams
                    var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                    // Waits process completion and then checks it was not completed by timeout
                    if (await Task.WhenAny(Task.Delay(timeout), processTask) == processTask && waitForExit.Result) {
                        result.Completed = true;
                        result.ExitCode = process.ExitCode;

                        // Adds process output if it was completed with error
                        result.Output = outputBuilder.ToString();
                        result.Error = errorBuilder.ToString();
                    } else {
                        try {
                            // Kill hung process
                            process.Kill();
                        }
                        catch { }
                    }
                }
            }

            return result;
        }

        private static Task<bool> WaitForExitAsync(Process process, int timeout) { return Task.Run(() => process.WaitForExit(timeout)); }

        public struct ProcessResult {
            public bool Completed;
            public int? ExitCode;
            public string Output;
            public string Error;
        }
    }
}
