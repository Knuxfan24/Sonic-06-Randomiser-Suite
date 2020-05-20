using Ookii.Dialogs;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    class Dialogs
    {
        /// <summary>
        /// Creates a folder browser from Ookii
        /// </summary>
        public static string FolderBrowser(string text) {
            VistaFolderBrowserDialog browse = new VistaFolderBrowserDialog() {
                Description = text,
                UseDescriptionForTitle = true
            };

            return browse.ShowDialog() == DialogResult.OK ? browse.SelectedPath : string.Empty;
        }

        /// <summary>
        /// Creates a file browser from WINAPI
        /// </summary>
        public static string FileBrowser(string text, string filter) {
            OpenFileDialog browse = new OpenFileDialog() {
                Title = text,
                Filter = filter
            };

            return browse.ShowDialog() == DialogResult.OK ? browse.FileName : string.Empty;
        }
    }
}
