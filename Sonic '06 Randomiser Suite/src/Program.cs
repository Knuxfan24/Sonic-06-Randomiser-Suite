using System;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    static class Program
    {
        public static readonly string GlobalVersionNumber = "Version 1.0";

        public static string ApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                             Arctool         = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\arctool.exe";

        [STAThread]

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            #region Write required pre-requisites to the Tools directory
            if (!Directory.Exists($"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\"))
                Directory.CreateDirectory($"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\");

            if (!File.Exists(Arctool))
                File.WriteAllBytes(Arctool, Properties.Resources.arctool);
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
