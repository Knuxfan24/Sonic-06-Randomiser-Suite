using System;
using System.Windows.Forms;

namespace Sonic06Randomiser
{
    public partial class Form1 : Form
    {
        string filepath = "";
        string output = "";
        bool randomEnemies = false;
        bool randomItems = false;
        bool randomCharacters = false;
        bool altCharacters = false;
        bool randomVoices = false;
        bool spoilerLog = false;
        Random rnd = new Random();
        string rndSeed = "";

        public Form1()
        {
            InitializeComponent();
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            filepath = filepathBox.Text;
            if (filepathBox.Text == "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select XML";
            fdlg.Filter = "eXtensible Markup Language file (*.xml)|*.xml|All files B (*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                filepath = fdlg.FileName;
                filepathBox.Text = filepath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                randomEnemies = true;
            }
            if (!checkBox1.Checked)
            {
                randomEnemies = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                randomItems = true;
            }
            if (!checkBox2.Checked)
            {
                randomItems = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                randomCharacters = true;
                checkBox5.Enabled = true;
            }
            if (!checkBox3.Checked)
            {
                randomCharacters = false;
                altCharacters = false;
                checkBox5.Checked = false;
                checkBox5.Enabled = false;
            }
        }

        private void seedBox_TextChanged(object sender, EventArgs e)
        {
            rndSeed = seedBox.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.RandomiseFunction(filepath, rndSeed, output, randomEnemies, randomItems, randomCharacters, altCharacters, randomVoices, spoilerLog);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                output = fbd.SelectedPath;
                outputBox.Text = output;
            }
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {
            output = outputBox.Text;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                randomVoices = true;
            }
            if (!checkBox4.Checked)
            {
                randomVoices = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                altCharacters = true;
            }
            if (!checkBox5.Checked)
            {
                altCharacters = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                spoilerLog = true;
            }
            if (!checkBox6.Checked)
            {
                spoilerLog = false;
            }
        }
    }
}
