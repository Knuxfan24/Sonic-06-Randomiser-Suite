namespace Sonic06Randomiser
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.filepathLabel = new System.Windows.Forms.Label();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathButton = new System.Windows.Forms.Button();
            this.outputLabel = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.seedLabel = new System.Windows.Forms.Label();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedButton = new System.Windows.Forms.Button();
            this.enemiesCheckbox = new System.Windows.Forms.CheckBox();
            this.charactersCheckbox = new System.Windows.Forms.CheckBox();
            this.itemsCheckbox = new System.Windows.Forms.CheckBox();
            this.voiceCheckbox = new System.Windows.Forms.CheckBox();
            this.statesCheckbox = new System.Windows.Forms.CheckBox();
            this.logCheckbox = new System.Windows.Forms.CheckBox();
            this.xmlCheckbox = new System.Windows.Forms.CheckBox();
            this.extractButton = new System.Windows.Forms.Button();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.sourceToggle = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // filepathLabel
            // 
            this.filepathLabel.AutoSize = true;
            this.filepathLabel.Location = new System.Drawing.Point(9, 15);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(100, 13);
            this.filepathLabel.TabIndex = 8;
            this.filepathLabel.Text = "XML to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(115, 12);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(334, 20);
            this.filepathBox.TabIndex = 9;
            this.filepathBox.TextChanged += new System.EventHandler(this.filepathBox_TextChanged);
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(455, 12);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 22);
            this.filepathButton.TabIndex = 10;
            this.filepathButton.Text = "...";
            this.toolTip.SetToolTip(this.filepathButton, "Browse for an XML file to randomise.");
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.filepathButton_Click);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(22, 38);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(87, 13);
            this.outputLabel.TabIndex = 11;
            this.outputLabel.Text = "Output Directory:";
            this.outputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(115, 35);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(334, 20);
            this.outputBox.TabIndex = 12;
            this.toolTip.SetToolTip(this.outputBox, "Location to store the randomised sets (leave blank to store them in the applicati" +
        "on folder)");
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(455, 35);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(24, 22);
            this.outputButton.TabIndex = 13;
            this.outputButton.Text = "...";
            this.toolTip.SetToolTip(this.outputButton, "Browse for an output folder.");
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Location = new System.Drawing.Point(74, 61);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(35, 13);
            this.seedLabel.TabIndex = 14;
            this.seedLabel.Text = "Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(115, 58);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(334, 20);
            this.seedBox.TabIndex = 15;
            this.toolTip.SetToolTip(this.seedBox, "String to hash for the randomise to use as a seed.");
            this.seedBox.TextChanged += new System.EventHandler(this.seedBox_TextChanged);
            // 
            // seedButton
            // 
            this.seedButton.BackgroundImage = global::Sonic06Randomiser.Properties.Resources.shuffleIcon;
            this.seedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seedButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.seedButton.Location = new System.Drawing.Point(455, 58);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 22);
            this.seedButton.TabIndex = 16;
            this.toolTip.SetToolTip(this.seedButton, "Generate a new random seed.");
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.seedButton_Click);
            // 
            // enemiesCheckbox
            // 
            this.enemiesCheckbox.AutoSize = true;
            this.enemiesCheckbox.Location = new System.Drawing.Point(12, 100);
            this.enemiesCheckbox.Name = "enemiesCheckbox";
            this.enemiesCheckbox.Size = new System.Drawing.Size(122, 17);
            this.enemiesCheckbox.TabIndex = 17;
            this.enemiesCheckbox.Text = "Randomise Enemies";
            this.toolTip.SetToolTip(this.enemiesCheckbox, "Randomise the enemies that spawn in the stage and their behaviour mode.");
            this.enemiesCheckbox.UseVisualStyleBackColor = true;
            this.enemiesCheckbox.CheckedChanged += new System.EventHandler(this.enemiesCheckbox_CheckedChanged);
            // 
            // charactersCheckbox
            // 
            this.charactersCheckbox.AutoSize = true;
            this.charactersCheckbox.Location = new System.Drawing.Point(12, 123);
            this.charactersCheckbox.Name = "charactersCheckbox";
            this.charactersCheckbox.Size = new System.Drawing.Size(133, 17);
            this.charactersCheckbox.TabIndex = 18;
            this.charactersCheckbox.Text = "Randomise Characters";
            this.toolTip.SetToolTip(this.charactersCheckbox, "Randomise which characters are used in the stage (affects the main spawn point an" +
        "d any points where control is switched to another character).");
            this.charactersCheckbox.UseVisualStyleBackColor = true;
            this.charactersCheckbox.CheckedChanged += new System.EventHandler(this.charactersCheckbox_CheckedChanged);
            // 
            // itemsCheckbox
            // 
            this.itemsCheckbox.AutoSize = true;
            this.itemsCheckbox.Location = new System.Drawing.Point(12, 146);
            this.itemsCheckbox.Name = "itemsCheckbox";
            this.itemsCheckbox.Size = new System.Drawing.Size(148, 17);
            this.itemsCheckbox.TabIndex = 19;
            this.itemsCheckbox.Text = "Randomise Item Capsules";
            this.toolTip.SetToolTip(this.itemsCheckbox, "Randomise what Item Capsules will contain.");
            this.itemsCheckbox.UseVisualStyleBackColor = true;
            this.itemsCheckbox.CheckedChanged += new System.EventHandler(this.itemsCheckbox_CheckedChanged);
            // 
            // voiceCheckbox
            // 
            this.voiceCheckbox.AutoSize = true;
            this.voiceCheckbox.Location = new System.Drawing.Point(12, 169);
            this.voiceCheckbox.Name = "voiceCheckbox";
            this.voiceCheckbox.Size = new System.Drawing.Size(150, 17);
            this.voiceCheckbox.TabIndex = 20;
            this.voiceCheckbox.Text = "Randomise Voice Triggers";
            this.toolTip.SetToolTip(this.voiceCheckbox, "Randomise what voice lines play at set points in the stage.");
            this.voiceCheckbox.UseVisualStyleBackColor = true;
            this.voiceCheckbox.CheckedChanged += new System.EventHandler(this.voiceCheckbox_CheckedChanged);
            // 
            // statesCheckbox
            // 
            this.statesCheckbox.AutoSize = true;
            this.statesCheckbox.Enabled = false;
            this.statesCheckbox.Location = new System.Drawing.Point(168, 123);
            this.statesCheckbox.Name = "statesCheckbox";
            this.statesCheckbox.Size = new System.Drawing.Size(139, 17);
            this.statesCheckbox.TabIndex = 21;
            this.statesCheckbox.Text = "Include Alternate States";
            this.toolTip.SetToolTip(this.statesCheckbox, "Include alternate character states for Sonic (his snowboards and Mach Speed state" +
        "s) in the randomisation.");
            this.statesCheckbox.UseVisualStyleBackColor = true;
            this.statesCheckbox.CheckedChanged += new System.EventHandler(this.statesCheckbox_CheckedChanged);
            // 
            // logCheckbox
            // 
            this.logCheckbox.AutoSize = true;
            this.logCheckbox.Location = new System.Drawing.Point(168, 146);
            this.logCheckbox.Name = "logCheckbox";
            this.logCheckbox.Size = new System.Drawing.Size(153, 17);
            this.logCheckbox.TabIndex = 22;
            this.logCheckbox.Text = "Enable Randomisation Log";
            this.toolTip.SetToolTip(this.logCheckbox, "Output a log file showing what was changed when randomisation is complete.");
            this.logCheckbox.UseVisualStyleBackColor = true;
            this.logCheckbox.CheckedChanged += new System.EventHandler(this.logCheckbox_CheckedChanged);
            // 
            // xmlCheckbox
            // 
            this.xmlCheckbox.AutoSize = true;
            this.xmlCheckbox.Location = new System.Drawing.Point(168, 168);
            this.xmlCheckbox.Name = "xmlCheckbox";
            this.xmlCheckbox.Size = new System.Drawing.Size(109, 17);
            this.xmlCheckbox.TabIndex = 23;
            this.xmlCheckbox.Text = "Keep Edited XML";
            this.toolTip.SetToolTip(this.xmlCheckbox, "Don\'t remove the XML created and converted into a SET data by the randomiser.");
            this.xmlCheckbox.UseVisualStyleBackColor = true;
            this.xmlCheckbox.CheckedChanged += new System.EventHandler(this.xmlCheckbox_CheckedChanged);
            // 
            // extractButton
            // 
            this.extractButton.Location = new System.Drawing.Point(404, 136);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 23);
            this.extractButton.TabIndex = 24;
            this.extractButton.Text = "Extract Set";
            this.toolTip.SetToolTip(this.extractButton, "Open up the Set Extractor to enable extracting of original SET files.");
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Enabled = false;
            this.randomiseButton.Location = new System.Drawing.Point(404, 165);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(75, 23);
            this.randomiseButton.TabIndex = 25;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.randomiseButton_Click);
            // 
            // sourceToggle
            // 
            this.sourceToggle.AutoSize = true;
            this.sourceToggle.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sourceToggle.Location = new System.Drawing.Point(335, 84);
            this.sourceToggle.Name = "sourceToggle";
            this.sourceToggle.Size = new System.Drawing.Size(144, 17);
            this.sourceToggle.TabIndex = 26;
            this.sourceToggle.Text = "Save in Source Directory";
            this.sourceToggle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.sourceToggle, "Save in the same folder as the XML if no Output Directory is specified.");
            this.sourceToggle.UseVisualStyleBackColor = true;
            this.sourceToggle.CheckedChanged += new System.EventHandler(this.sourceToggle_CheckedChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 197);
            this.Controls.Add(this.sourceToggle);
            this.Controls.Add(this.randomiseButton);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.xmlCheckbox);
            this.Controls.Add(this.logCheckbox);
            this.Controls.Add(this.statesCheckbox);
            this.Controls.Add(this.voiceCheckbox);
            this.Controls.Add(this.itemsCheckbox);
            this.Controls.Add(this.charactersCheckbox);
            this.Controls.Add(this.enemiesCheckbox);
            this.Controls.Add(this.seedButton);
            this.Controls.Add(this.seedBox);
            this.Controls.Add(this.seedLabel);
            this.Controls.Add(this.outputButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.filepathButton);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.filepathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "SONIC THE HEDGEHOG (2006) Randomiser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Button seedButton;
        private System.Windows.Forms.CheckBox enemiesCheckbox;
        private System.Windows.Forms.CheckBox charactersCheckbox;
        private System.Windows.Forms.CheckBox itemsCheckbox;
        private System.Windows.Forms.CheckBox voiceCheckbox;
        private System.Windows.Forms.CheckBox statesCheckbox;
        private System.Windows.Forms.CheckBox logCheckbox;
        private System.Windows.Forms.CheckBox xmlCheckbox;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox sourceToggle;
    }
}