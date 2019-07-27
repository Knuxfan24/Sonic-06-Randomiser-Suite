namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class MusicRandomisationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MusicRandomisationForm));
            this.randomFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.seedButton = new System.Windows.Forms.Button();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.musicConfigList = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // randomFolderCheckbox
            // 
            this.randomFolderCheckbox.AutoSize = true;
            this.randomFolderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.randomFolderCheckbox.Location = new System.Drawing.Point(366, 67);
            this.randomFolderCheckbox.Name = "randomFolderCheckbox";
            this.randomFolderCheckbox.Size = new System.Drawing.Size(111, 17);
            this.randomFolderCheckbox.TabIndex = 62;
            this.randomFolderCheckbox.Text = "Randomise Folder";
            this.toolTip1.SetToolTip(this.randomFolderCheckbox, "Randomise a folder and subfolders instead of one LUA file.");
            this.randomFolderCheckbox.UseVisualStyleBackColor = true;
            this.randomFolderCheckbox.CheckedChanged += new System.EventHandler(this.RandomFolderCheckbox_CheckedChanged);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(408, 181);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(75, 23);
            this.randomiseButton.TabIndex = 59;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.RandomiseButton_Click);
            // 
            // seedButton
            // 
            this.seedButton.BackgroundImage = global::SONIC_THE_HEDGEHOG__2006__Randomiser_Suite.Properties.Resources.seedIcon;
            this.seedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seedButton.Location = new System.Drawing.Point(453, 38);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 23);
            this.seedButton.TabIndex = 58;
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.SeedButton_Click);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(127, 39);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(320, 20);
            this.seedBox.TabIndex = 57;
            this.seedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(11, 37);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(110, 23);
            this.seedLabel.TabIndex = 56;
            this.seedLabel.Text = "Randomisation Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(453, 9);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 52;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(127, 10);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(320, 20);
            this.filepathBox.TabIndex = 51;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.Location = new System.Drawing.Point(11, 8);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(110, 23);
            this.filepathLabel.TabIndex = 50;
            this.filepathLabel.Text = "Folder to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(253, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 64;
            this.button1.Text = "Check All";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // musicConfigList
            // 
            this.musicConfigList.FormattingEnabled = true;
            this.musicConfigList.Items.AddRange(new object[] {
            "Aquatic Base - Level 1",
            "Aquatic Base - Level 2",
            "Crisis City - The Flames",
            "Crisis City - Skyscraper",
            "Crisis City - Whirlwind",
            "Crisis City - Tornado",
            "Dusty Desert - Quicksand",
            "Dusty Desert - The Ruins",
            "End of the World - Tails",
            "End of the World - Omega",
            "End of the World - Knuckles",
            "End of the World - Silver",
            "End of the World - Rouge",
            "End of the World - Amy",
            "End of the World - Shadow",
            "Flame Core - Volcano",
            "Flame Core - The Cavern",
            "Kingdom Valley - Wind",
            "Kingdom Valley - The Castle",
            "Kingdom Valley - Lakeside",
            "Kingdom Valley - Water",
            "Radical Train - The Abandoned Mine",
            "Radical Train - The Chase",
            "Tropical Jungle - The Jungle",
            "Tropical Jungle - The Swamp",
            "Tropical Jungle - The Ruins",
            "White Acropolis - Snowy Peak",
            "White Acropolis - The Base",
            "Wave Ocean - The Water\'s Edge",
            "Wave Ocean - The Inlet",
            "Character Battle",
            "Egg Cereberus and Egg Genesis",
            "Egg Wyvern",
            "Iblis",
            "Iblis (Phase 3)",
            "Mephiles",
            "Mephiles (Phase 2)",
            "Solaris",
            "Solaris (Phase 2)",
            "Soleanna Castle Town",
            "Soleanna New City",
            "Soleanna Forest",
            "Accordion Song",
            "Town Mission 1",
            "Town Mission 2",
            "Town Mission 3",
            "Town Mission 4",
            "Main Menu",
            "Multiplayer Character Select",
            "Extras Menu",
            "Results"});
            this.musicConfigList.Location = new System.Drawing.Point(14, 66);
            this.musicConfigList.Name = "musicConfigList";
            this.musicConfigList.Size = new System.Drawing.Size(314, 109);
            this.musicConfigList.TabIndex = 65;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 66;
            this.button2.Text = "Uncheck All";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // MusicRandomisationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 214);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.musicConfigList);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.randomFolderCheckbox);
            this.Controls.Add(this.randomiseButton);
            this.Controls.Add(this.seedButton);
            this.Controls.Add(this.seedBox);
            this.Controls.Add(this.seedLabel);
            this.Controls.Add(this.filepathButton);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.filepathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MusicRandomisationForm";
            this.Text = "SONIC THE HEDGEHOG (2006) Music Randomiser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox randomFolderCheckbox;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.Button seedButton;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox musicConfigList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}