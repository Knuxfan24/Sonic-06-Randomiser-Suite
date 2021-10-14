using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    static class Program
    {
        // Generate the path to a temp directory we can use for the Randomisation process.
        public static string TemporaryDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Force culture info 'en-GB' to prevent errors with values altered by language-specific differences.
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-GB");

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Main());

        }
    }

    internal sealed class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
    }
}
