using System;
using System.Windows.Forms;

namespace Sonic06Randomiser
{
    public partial class MainWindow : Form
    {
        //Setup Variables
        string filepath = "";
        string output = "";
        bool sourceOutput = false;
        bool randomEnemies = false;
        bool randomCharacters = false;
        bool altCharacters = false;
        bool randomItems = false;
        bool randomVoices = false;
        bool spoilerLog = false;
        bool keepXML = false;
        Random rnd = new Random();
        string rndSeed = "";

        public MainWindow()
        {
            //Basic Windows Forms setup as well as generating the first RND Seed.
            InitializeComponent();
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }

        private void filepathBox_TextChanged(object sender, EventArgs e)
        {
            //Setting the filepath variable to the XML based on what is in the box and enabling the button to randomise.
            filepath = filepathBox.Text;
            if (filepathBox.Text == "")
            {
                randomiseButton.Enabled = false;
            }
            else
            {
                randomiseButton.Enabled = true;
            }
        }

        private void filepathButton_Click(object sender, EventArgs e)
        {
            //Selecting the XML and updating the filepath text & variable if one is selected.
            OpenFileDialog xmlBrowser = new OpenFileDialog();
            xmlBrowser.Title = "Select XML";
            xmlBrowser.Filter = "eXtensible Markup Language file (*.xml)|*.xml|All files (*.*)|*.*";
            xmlBrowser.FilterIndex = 1;
            xmlBrowser.RestoreDirectory = true;
            if (xmlBrowser.ShowDialog() == DialogResult.OK)
            {
                filepath = xmlBrowser.FileName;
                filepathBox.Text = filepath;
            }
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {
            //Simply set the output variable to the folder in the text box.
            output = outputBox.Text;
            if (outputBox.Text == "")
            {
                sourceToggle.Enabled = true;
            }
            else
            {
                sourceToggle.Checked = false;
                sourceToggle.Enabled = false;
            }
        }

        private void outputButton_Click(object sender, EventArgs e)
        {
            //Open up a folder browser to select a folder and update the output text & variable if one is selected.
            FolderBrowserDialog outputBrowser = new FolderBrowserDialog();
            if (outputBrowser.ShowDialog() == DialogResult.OK)
            {
                output = outputBrowser.SelectedPath;
                outputBox.Text = output;
            }
        }

        private void seedBox_TextChanged(object sender, EventArgs e)
        {
            //Simply set the rndSeed variable to whatever is in the text box.
            rndSeed = seedBox.Text;
        }

        private void seedButton_Click(object sender, EventArgs e)
        {
            //Randomly generate and update the new seed.
            rndSeed = rnd.Next().ToString();
            seedBox.Text = rndSeed;
        }

        private void enemiesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the randomEnemies variable on or off.
            if (enemiesCheckbox.Checked)
            {
                randomEnemies = true;
            }
            if (!enemiesCheckbox.Checked)
            {
                randomEnemies = false;
            }
        }

        private void charactersCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the randomCharacters variable on or off and enable/disable the Alternate States box.
            if (charactersCheckbox.Checked)
            {
                randomCharacters = true;
                statesCheckbox.Enabled = true;
            }
            if (!charactersCheckbox.Checked)
            {
                randomCharacters = false;
                altCharacters = false;
                statesCheckbox.Checked = false;
                statesCheckbox.Enabled = false;
            }
        }

        private void statesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the altCharacters variable on or off.
            if (statesCheckbox.Checked)
            {
                altCharacters = true;
            }
            if (!statesCheckbox.Checked)
            {
                altCharacters = false;
            }
        }

        private void itemsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the randomItems variable on or off.
            if (itemsCheckbox.Checked)
            {
                randomItems = true;
            }
            if (!itemsCheckbox.Checked)
            {
                randomItems = false;
            }
        }

        private void voiceCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the randomVoices variable on or off.
            if (voiceCheckbox.Checked)
            {
                randomVoices = true;
            }
            if (!voiceCheckbox.Checked)
            {
                randomVoices = false;
            }
        }

        private void logCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the spoilerLog variable on or off.
            if (logCheckbox.Checked)
            {
                spoilerLog = true;
            }
            if (!logCheckbox.Checked)
            {
                spoilerLog = false;
            }
        }

        private void xmlCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the keepXML variable on or off.
            if (xmlCheckbox.Checked)
            {
                keepXML = true;
            }
            if (!xmlCheckbox.Checked)
            {
                keepXML = false;
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            //Bring up the Set Extraction Window and lock input to the main window while it is active
            ExtractSetWindow extractSetForm = new ExtractSetWindow();
            extractSetForm.ShowDialog();
        }

        private void randomiseButton_Click(object sender, EventArgs e)
        {
            //Pass the variables set by the main form to the randomise function and carry out the randomisation.
            Randomisation.Randomise(filepath, rndSeed, output, randomEnemies, randomItems, randomCharacters, altCharacters, randomVoices, spoilerLog, keepXML, sourceOutput);
        }

        private void sourceToggle_CheckedChanged(object sender, EventArgs e)
        {
            //Toggle the sourceOutput variable on or off.
            if (sourceToggle.Checked)
            {
                sourceOutput = true;
            }
            if (!sourceToggle.Checked)
            {
                sourceOutput = false;
            }
        }
    }
}
