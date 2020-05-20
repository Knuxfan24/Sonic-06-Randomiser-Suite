using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    public class ListBoxWriter : TextWriter
    {
        private readonly ListBox _list;
        private StringBuilder _content = new StringBuilder();

        public ListBoxWriter(ListBox list) => _list = list;

        public override Encoding Encoding { get { return Encoding.UTF8; } }

        /// <summary>
        /// Writes all console output to a ListBox element
        /// </summary>
        public override void Write(char value) {
            base.Write(value);
            _content.Append(value);

            if (value != '\n') return;

            if (_list.InvokeRequired) {
                try {
                    _list.Invoke(new MethodInvoker(() => _list.Items.Add(_content.ToString())));
                    _list.Invoke(new MethodInvoker(() => _list.SelectedIndex = _list.Items.Count - 1));
                    _list.Invoke(new MethodInvoker(() => _list.SelectedIndex = -1));
                } catch (ObjectDisposedException ex) { Console.WriteLine($"[{DateTime.Now:HH:mm:ss tt}] [Error] Failed to dispose invoker...\n{ex}"); }
            } else {
                _list.Items.Add(_content.ToString());
                _list.SelectedIndex = _list.Items.Count - 1;
                _list.SelectedIndex = -1;
            }

            _content = new StringBuilder();
        }
    }
}
