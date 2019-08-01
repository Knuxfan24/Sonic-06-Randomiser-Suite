using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Knuxfan24/SONIC-THE-HEDGEHOG-2006-Randomiser-Suite");
        }

        private void LicenseButton_Click(object sender, EventArgs e)
        {
            Licenses licenses = new Licenses();
            licenses.ShowDialog();
        }

        private void KnuxfanLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            knuxfanLinkLabel.LinkVisited = true;
            Process.Start("https://github.com/Knuxfan24");
        }

        private void RadLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            radLinkLabel.LinkVisited = true;
            Process.Start("https://github.com/Radfordhound");
        }

        private void HyperLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            hyperLinkLabel.LinkVisited = true;
            Process.Start("https://github.com/HyperPolygon64");
        }

        private void OokiiLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ookiiLinkLabel.LinkVisited = true;
            Process.Start("http://www.ookii.org/software/dialogs/");
        }
    }
}
