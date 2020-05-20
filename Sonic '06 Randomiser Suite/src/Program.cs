using System;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    static class Program
    {
        public static readonly string GlobalVersionNumber = "Version 1.0";

        public static string ApplicationData   = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                             Arctool           = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\arctool.exe",
                             CollisionImporter = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\CollisionImporter.exe",
                             CollisionExporter = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\CollisionExporter.exe",
                             pkgtool           = $"{ApplicationData}\\Sonic_06_Randomiser_Suite\\Tools\\pkgtool.exe",
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

            if (!File.Exists(Arctool))
                File.WriteAllBytes(Arctool, Properties.Resources.arctool);

            if (!File.Exists(CollisionImporter))
                File.WriteAllBytes(CollisionImporter, Properties.Resources.CollisionImporter);

            if (!File.Exists(CollisionExporter))
                File.WriteAllBytes(CollisionExporter, Properties.Resources.CollisionExporter);

            if (!File.Exists(pkgtool))
                File.WriteAllBytes(pkgtool, Properties.Resources.pkgtool);

            if (!File.Exists(unlub))
                File.WriteAllBytes(unlub, Properties.Resources.unlub);
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
