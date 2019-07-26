namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class CharacterRandomisationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterRandomisationForm));
            this.jumpCheckbox = new System.Windows.Forms.CheckBox();
            this.movementCheckbox = new System.Windows.Forms.CheckBox();
            this.randomFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.seedButton = new System.Windows.Forms.Button();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.grindCheckbox = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gemCheckbox = new System.Windows.Forms.CheckBox();
            this.abilityCheckbox = new System.Windows.Forms.CheckBox();
            this.modelsCheckbox = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.modelConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // jumpCheckbox
            // 
            this.jumpCheckbox.AutoSize = true;
            this.jumpCheckbox.Location = new System.Drawing.Point(16, 104);
            this.jumpCheckbox.Name = "jumpCheckbox";
            this.jumpCheckbox.Size = new System.Drawing.Size(141, 17);
            this.jumpCheckbox.TabIndex = 62;
            this.jumpCheckbox.Text = "Randomise Jump Height";
            this.toolTip.SetToolTip(this.jumpCheckbox, "Randomise how high the character jumps and how much momentum they carry into it.");
            this.jumpCheckbox.UseVisualStyleBackColor = true;
            // 
            // movementCheckbox
            // 
            this.movementCheckbox.AutoSize = true;
            this.movementCheckbox.Location = new System.Drawing.Point(16, 81);
            this.movementCheckbox.Name = "movementCheckbox";
            this.movementCheckbox.Size = new System.Drawing.Size(166, 17);
            this.movementCheckbox.TabIndex = 61;
            this.movementCheckbox.Text = "Randomise Movement Speed";
            this.toolTip.SetToolTip(this.movementCheckbox, "Randomise how fast the character moves on the ground.");
            this.movementCheckbox.UseVisualStyleBackColor = true;
            // 
            // randomFolderCheckbox
            // 
            this.randomFolderCheckbox.AutoSize = true;
            this.randomFolderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.randomFolderCheckbox.Location = new System.Drawing.Point(368, 81);
            this.randomFolderCheckbox.Name = "randomFolderCheckbox";
            this.randomFolderCheckbox.Size = new System.Drawing.Size(111, 17);
            this.randomFolderCheckbox.TabIndex = 59;
            this.randomFolderCheckbox.Text = "Randomise Folder";
            this.toolTip.SetToolTip(this.randomFolderCheckbox, "Randomise a folder and subfolders instead of one LUA file.");
            this.randomFolderCheckbox.UseVisualStyleBackColor = true;
            this.randomFolderCheckbox.CheckedChanged += new System.EventHandler(this.RandomFolderCheckbox_CheckedChanged);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(405, 193);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(75, 23);
            this.randomiseButton.TabIndex = 58;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.RandomiseButton_Click);
            // 
            // seedButton
            // 
            this.seedButton.BackgroundImage = global::SONIC_THE_HEDGEHOG__2006__Randomiser_Suite.Properties.Resources.seedIcon;
            this.seedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seedButton.Location = new System.Drawing.Point(455, 52);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 23);
            this.seedButton.TabIndex = 57;
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.SeedButton_Click);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(129, 53);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(320, 20);
            this.seedBox.TabIndex = 56;
            this.seedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(13, 51);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(110, 23);
            this.seedLabel.TabIndex = 55;
            this.seedLabel.Text = "Randomisation Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(455, 26);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 54;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(129, 27);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(320, 20);
            this.filepathBox.TabIndex = 53;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.Location = new System.Drawing.Point(13, 25);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(110, 23);
            this.filepathLabel.TabIndex = 52;
            this.filepathLabel.Text = "Folder to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grindCheckbox
            // 
            this.grindCheckbox.AutoSize = true;
            this.grindCheckbox.Location = new System.Drawing.Point(16, 127);
            this.grindCheckbox.Name = "grindCheckbox";
            this.grindCheckbox.Size = new System.Drawing.Size(155, 17);
            this.grindCheckbox.TabIndex = 63;
            this.grindCheckbox.Text = "Randomise Grinding Speed";
            this.toolTip.SetToolTip(this.grindCheckbox, "Randomise how fast this character grinds at.");
            this.grindCheckbox.UseVisualStyleBackColor = true;
            // 
            // gemCheckbox
            // 
            this.gemCheckbox.AutoSize = true;
            this.gemCheckbox.Location = new System.Drawing.Point(16, 197);
            this.gemCheckbox.Name = "gemCheckbox";
            this.gemCheckbox.Size = new System.Drawing.Size(144, 17);
            this.gemCheckbox.TabIndex = 64;
            this.gemCheckbox.Text = "Fix Sonic\'s Action Gauge";
            this.toolTip.SetToolTip(this.gemCheckbox, "Patch the typo that prevents Sonic\'s Action Gauge from draining when using the Ge" +
        "ms.");
            this.gemCheckbox.UseVisualStyleBackColor = true;
            // 
            // abilityCheckbox
            // 
            this.abilityCheckbox.AutoSize = true;
            this.abilityCheckbox.Location = new System.Drawing.Point(16, 150);
            this.abilityCheckbox.Name = "abilityCheckbox";
            this.abilityCheckbox.Size = new System.Drawing.Size(214, 17);
            this.abilityCheckbox.TabIndex = 65;
            this.abilityCheckbox.Text = "Randomise Character Ability Parameters";
            this.toolTip.SetToolTip(this.abilityCheckbox, "Randomise various attributes unique to certain characters (such as Amy\'s Double J" +
        "ump or Sonic\'s Bound Attack)");
            this.abilityCheckbox.UseVisualStyleBackColor = true;
            // 
            // modelsCheckbox
            // 
            this.modelsCheckbox.AutoSize = true;
            this.modelsCheckbox.Location = new System.Drawing.Point(16, 173);
            this.modelsCheckbox.Name = "modelsCheckbox";
            this.modelsCheckbox.Size = new System.Drawing.Size(165, 17);
            this.modelsCheckbox.TabIndex = 66;
            this.modelsCheckbox.Text = "Randomise Character Models";
            this.toolTip.SetToolTip(this.modelsCheckbox, "Randomise what character model the character should use.");
            this.modelsCheckbox.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modelConfigurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(492, 24);
            this.menuStrip1.TabIndex = 67;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // modelConfigurationToolStripMenuItem
            // 
            this.modelConfigurationToolStripMenuItem.Name = "modelConfigurationToolStripMenuItem";
            this.modelConfigurationToolStripMenuItem.Size = new System.Drawing.Size(130, 20);
            this.modelConfigurationToolStripMenuItem.Text = "Model Configuration";
            this.modelConfigurationToolStripMenuItem.Click += new System.EventHandler(this.ModelConfigurationToolStripMenuItem_Click);
            // 
            // CharacterRandomisationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 227);
            this.Controls.Add(this.modelsCheckbox);
            this.Controls.Add(this.abilityCheckbox);
            this.Controls.Add(this.gemCheckbox);
            this.Controls.Add(this.grindCheckbox);
            this.Controls.Add(this.jumpCheckbox);
            this.Controls.Add(this.movementCheckbox);
            this.Controls.Add(this.randomFolderCheckbox);
            this.Controls.Add(this.randomiseButton);
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
            this.Name = "CharacterRandomisationForm";
            this.Text = "SONIC THE HEDGEHOG (2006) Character Attributes Randomiser";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox jumpCheckbox;
        private System.Windows.Forms.CheckBox movementCheckbox;
        private System.Windows.Forms.CheckBox randomFolderCheckbox;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.Button seedButton;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.CheckBox grindCheckbox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox gemCheckbox;
        private System.Windows.Forms.CheckBox abilityCheckbox;
        private System.Windows.Forms.CheckBox modelsCheckbox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modelConfigurationToolStripMenuItem;
    }
}