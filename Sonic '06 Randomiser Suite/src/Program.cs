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

            if (!File.Exists(unlub))
                File.WriteAllBytes(unlub, Properties.Resources.unlub);
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
