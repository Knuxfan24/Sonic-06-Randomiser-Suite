using System;
using System.Windows.Forms;

namespace Sonic06Randomiser
{
    public partial class ExtractSetWindow : Form
    {
        //Setup Variables
        string extractFilepath = "";
        string extractOutput = "";
        bool sourceOutput = false;

        public ExtractSetWindow()
        {
            InitializeComponent();
        }

        private void filepathBox_TextChanged(object sender, EventArgs e)
        {
            //Setting the filepath variable to the SET based on what is in the box and enabling the button to extract.
            extractFilepath = filepathBox.Text;
            if (filepathBox.Text == "")
            {
                extractButton.Enabled = false;
            }
            else
            {
                extractButton.Enabled = true;
            }
        }

        private void filepathButton_Click(object sender, EventArgs e)
        {
            //Selecting the SET and updating the filepath text & variable if one is selected.
            OpenFileDialog setBrowser = new OpenFileDialog();
            setBrowser.Title = "Select XML";
            setBrowser.Filter = "SONIC THE HEDGEHOG (2006) SET file (*.set)|*.set|All files (*.*)|*.*";
            setBrowser.FilterIndex = 1;
            setBrowser.RestoreDirectory = true;
            if (setBrowser.ShowDialog() == DialogResult.OK)
            {
                extractFilepath = setBrowser.FileName;
                filepathBox.Text = extractFilepath;
            }
        }

        private void outputBox_TextChanged(object sender, EventArgs e)
        {
            //Simply set the output variable to the folder in the text box.
            extractOutput = outputBox.Text;
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
                extractOutput = outputBrowser.SelectedPath;
                outputBox.Text = extractOutput;
            }
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            //Pass the variables set by the extraction form to the extract function and carry out the extraction.
            SetExtract.Extract(extractFilepath, extractOutput, sourceOutput);
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
