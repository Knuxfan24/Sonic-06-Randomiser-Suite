namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class SetRandomisationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetRandomisationForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.enemyConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemCapsuleConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filepathButton = new System.Windows.Forms.Button();
            this.seedLabel = new System.Windows.Forms.Label();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.seedButton = new System.Windows.Forms.Button();
            this.enemiesCheckbox = new System.Windows.Forms.CheckBox();
            this.charactersCheckbox = new System.Windows.Forms.CheckBox();
            this.itemsCheckbox = new System.Windows.Forms.CheckBox();
            this.voiceCheckbox = new System.Windows.Forms.CheckBox();
            this.doorCheckbox = new System.Windows.Forms.CheckBox();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.randomFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.physicsCheckbox = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enemyConfigurationToolStripMenuItem,
            this.characterConfigurationToolStripMenuItem,
            this.itemCapsuleConfigurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(492, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // enemyConfigurationToolStripMenuItem
            // 
            this.enemyConfigurationToolStripMenuItem.Name = "enemyConfigurationToolStripMenuItem";
            this.enemyConfigurationToolStripMenuItem.Size = new System.Drawing.Size(132, 20);
            this.enemyConfigurationToolStripMenuItem.Text = "Enemy Configuration";
            this.enemyConfigurationToolStripMenuItem.Click += new System.EventHandler(this.EnemyConfigurationToolStripMenuItem_Click);
            // 
            // characterConfigurationToolStripMenuItem
            // 
            this.characterConfigurationToolStripMenuItem.Name = "characterConfigurationToolStripMenuItem";
            this.characterConfigurationToolStripMenuItem.Size = new System.Drawing.Size(147, 20);
            this.characterConfigurationToolStripMenuItem.Text = "Character Configuration";
            this.characterConfigurationToolStripMenuItem.Click += new System.EventHandler(this.CharacterConfigurationToolStripMenuItem_Click);
            // 
            // itemCapsuleConfigurationToolStripMenuItem
            // 
            this.itemCapsuleConfigurationToolStripMenuItem.Name = "itemCapsuleConfigurationToolStripMenuItem";
            this.itemCapsuleConfigurationToolStripMenuItem.Size = new System.Drawing.Size(165, 20);
            this.itemCapsuleConfigurationToolStripMenuItem.Text = "Item Capsule Configuration";
            this.itemCapsuleConfigurationToolStripMenuItem.Click += new System.EventHandler(this.ItemCapsuleConfigurationToolStripMenuItem_Click);
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(456, 27);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 2;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(14, 52);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(110, 23);
            this.seedLabel.TabIndex = 6;
            this.seedLabel.Text = "Randomisation Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(130, 28);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(320, 20);
            this.filepathBox.TabIndex = 1;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(130, 54);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(320, 20);
            this.seedBox.TabIndex = 7;
            this.seedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.Location = new System.Drawing.Point(14, 26);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(110, 23);
            this.filepathLabel.TabIndex = 0;
            this.filepathLabel.Text = "Folder to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // seedButton
            // 
            this.seedButton.BackgroundImage = global::SONIC_THE_HEDGEHOG__2006__Randomiser_Suite.Properties.Resources.seedIcon;
            this.seedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seedButton.Location = new System.Drawing.Point(456, 53);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 23);
            this.seedButton.TabIndex = 8;
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.SeedButton_Click);
            // 
            // enemiesCheckbox
            // 
            this.enemiesCheckbox.AutoSize = true;
            this.enemiesCheckbox.Location = new System.Drawing.Point(14, 82);
            this.enemiesCheckbox.Name = "enemiesCheckbox";
            this.enemiesCheckbox.Size = new System.Drawing.Size(122, 17);
            this.enemiesCheckbox.TabIndex = 12;
            this.enemiesCheckbox.Text = "Randomise Enemies";
            this.toolTip1.SetToolTip(this.enemiesCheckbox, "Randomise the enemies that appear in the stage and their behaviour.");
            this.enemiesCheckbox.UseVisualStyleBackColor = true;
            // 
            // charactersCheckbox
            // 
            this.charactersCheckbox.AutoSize = true;
            this.charactersCheckbox.Location = new System.Drawing.Point(14, 105);
            this.charactersCheckbox.Name = "charactersCheckbox";
            this.charactersCheckbox.Size = new System.Drawing.Size(133, 17);
            this.charactersCheckbox.TabIndex = 13;
            this.charactersCheckbox.Text = "Randomise Characters";
            this.toolTip1.SetToolTip(this.charactersCheckbox, "Randomise which characters are played as in the stage.");
            this.charactersCheckbox.UseVisualStyleBackColor = true;
            // 
            // itemsCheckbox
            // 
            this.itemsCheckbox.AutoSize = true;
            this.itemsCheckbox.Location = new System.Drawing.Point(14, 128);
            this.itemsCheckbox.Name = "itemsCheckbox";
            this.itemsCheckbox.Size = new System.Drawing.Size(148, 17);
            this.itemsCheckbox.TabIndex = 14;
            this.itemsCheckbox.Text = "Randomise Item Capsules";
            this.toolTip1.SetToolTip(this.itemsCheckbox, "Randomise the contents of all Item Capsules in the stage.");
            this.itemsCheckbox.UseVisualStyleBackColor = true;
            // 
            // voiceCheckbox
            // 
            this.voiceCheckbox.AutoSize = true;
            this.voiceCheckbox.Location = new System.Drawing.Point(14, 151);
            this.voiceCheckbox.Name = "voiceCheckbox";
            this.voiceCheckbox.Size = new System.Drawing.Size(150, 17);
            this.voiceCheckbox.TabIndex = 15;
            this.voiceCheckbox.Text = "Randomise Voice Triggers";
            this.toolTip1.SetToolTip(this.voiceCheckbox, "Randomise the hints that should be played on Voice Triggers in the stage.");
            this.voiceCheckbox.UseVisualStyleBackColor = true;
            // 
            // doorCheckbox
            // 
            this.doorCheckbox.AutoSize = true;
            this.doorCheckbox.Checked = true;
            this.doorCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doorCheckbox.Location = new System.Drawing.Point(14, 197);
            this.doorCheckbox.Name = "doorCheckbox";
            this.doorCheckbox.Size = new System.Drawing.Size(151, 17);
            this.doorCheckbox.TabIndex = 16;
            this.doorCheckbox.Text = "Remove Doors and Cages";
            this.toolTip1.SetToolTip(this.doorCheckbox, "Remove all Doors and Cages from the stage to work around HedgeLib\'s grouping issu" +
        "e.");
            this.doorCheckbox.UseVisualStyleBackColor = true;
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(405, 193);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(75, 23);
            this.randomiseButton.TabIndex = 18;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.RandomiseButton_Click);
            // 
            // randomFolderCheckbox
            // 
            this.randomFolderCheckbox.AutoSize = true;
            this.randomFolderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.randomFolderCheckbox.Location = new System.Drawing.Point(369, 82);
            this.randomFolderCheckbox.Name = "randomFolderCheckbox";
            this.randomFolderCheckbox.Size = new System.Drawing.Size(111, 17);
            this.randomFolderCheckbox.TabIndex = 42;
            this.randomFolderCheckbox.Text = "Randomise Folder";
            this.toolTip1.SetToolTip(this.randomFolderCheckbox, "Randomise a folder and subfolders instead of one SET file.");
            this.randomFolderCheckbox.UseVisualStyleBackColor = true;
            this.randomFolderCheckbox.CheckedChanged += new System.EventHandler(this.RandomFolderCheckbox_CheckedChanged);
            // 
            // physicsCheckbox
            // 
            this.physicsCheckbox.AutoSize = true;
            this.physicsCheckbox.Location = new System.Drawing.Point(14, 174);
            this.physicsCheckbox.Name = "physicsCheckbox";
            this.physicsCheckbox.Size = new System.Drawing.Size(148, 17);
            this.physicsCheckbox.TabIndex = 43;
            this.physicsCheckbox.Text = "Randomise Physics Props";
            this.toolTip1.SetToolTip(this.physicsCheckbox, "Randomise what Physics Props spawn throughout the stage.");
            this.physicsCheckbox.UseVisualStyleBackColor = true;
            // 
            // SetRandomisationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 228);
            this.Controls.Add(this.physicsCheckbox);
            this.Controls.Add(this.randomFolderCheckbox);
            this.Controls.Add(this.randomiseButton);
            this.Controls.Add(this.doorCheckbox);
            this.Controls.Add(this.voiceCheckbox);
            this.Controls.Add(this.itemsCheckbox);
            this.Controls.Add(this.charactersCheckbox);
            this.Controls.Add(this.enemiesCheckbox);
            this.Controls.Add(this.seedButton);
            this.Controls.Add(this.seedBox);
            this.Controls.Add(this.seedLabel);
            this.Controls.Add(this.filepathButton);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.filepathLabel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "SetRandomisationForm";
            this.Text = "SONIC THE HEDGEHOG (2006) SET Randomiser";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.Button seedButton;
        private System.Windows.Forms.CheckBox enemiesCheckbox;
        private System.Windows.Forms.CheckBox charactersCheckbox;
        private System.Windows.Forms.CheckBox itemsCheckbox;
        private System.Windows.Forms.CheckBox voiceCheckbox;
        private System.Windows.Forms.CheckBox doorCheckbox;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.CheckBox randomFolderCheckbox;
        private System.Windows.Forms.ToolStripMenuItem enemyConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem characterConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemCapsuleConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox physicsCheckbox;
    }
}

