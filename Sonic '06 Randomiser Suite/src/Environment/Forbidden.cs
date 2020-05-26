using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sonic_06_Randomiser_Suite
{
    public partial class Forbidden : Form
    {
        string getSender;

        public Forbidden(object sender)
        {
            InitializeComponent();
            MaximumSize = new Size(int.MaxValue, 151);
            getSender = ((Button)sender).Name;

            if (getSender == "Button_Visual_General_Forbidden")
                TextBox_Forbidden.Text = Properties.Settings.Default.Forbidden_Textures;
            else if (getSender == "Button_Package_General_Forbidden")
                TextBox_Forbidden.Text = Properties.Settings.Default.Forbidden_Animations;
        }

        private void Button_Forbidden_Confirm_Click(object sender, EventArgs e)
        {
            if (getSender == "Button_Visual_General_Forbidden")
                Properties.Settings.Default.Forbidden_Textures = TextBox_Forbidden.Text;
            else if (getSender == "Button_Package_General_Forbidden")
                Properties.Settings.Default.Forbidden_Animations = TextBox_Forbidden.Text;

            Properties.Settings.Default.Save();
            Close();
        }

        private void Button_Forbidden_Default_Click(object sender, EventArgs e)
        {
            if (getSender == "Button_Visual_General_Forbidden")
                TextBox_Forbidden.Text = Properties.Settings.Default.Forbidden_Textures = Properties.Resources.Forbidden_Textures_Default;
            else if (getSender == "Button_Package_General_Forbidden")
                TextBox_Forbidden.Text = Properties.Settings.Default.Forbidden_Animations = Properties.Resources.Forbidden_Animations_Default;
        }
    }
}
