namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class CollisionPropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollisionPropertiesForm));
            this.randomFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.seedButton = new System.Windows.Forms.Button();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.deathCheckbox = new System.Windows.Forms.CheckBox();
            this.waterCheckbox = new System.Windows.Forms.CheckBox();
            this.wallsCheckbox = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.surfaceTypeConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // randomFolderCheckbox
            // 
            this.randomFolderCheckbox.AutoSize = true;
            this.randomFolderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.randomFolderCheckbox.Location = new System.Drawing.Point(367, 81);
            this.randomFolderCheckbox.Name = "randomFolderCheckbox";
            this.randomFolderCheckbox.Size = new System.Drawing.Size(111, 17);
            this.randomFolderCheckbox.TabIndex = 56;
            this.randomFolderCheckbox.Text = "Randomise Folder";
            this.randomFolderCheckbox.UseVisualStyleBackColor = true;
            this.randomFolderCheckbox.CheckedChanged += new System.EventHandler(this.RandomFolderCheckbox_CheckedChanged);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(403, 123);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(75, 23);
            this.randomiseButton.TabIndex = 55;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.RandomiseButton_Click);
            // 
            // seedButton
            // 
            this.seedButton.BackgroundImage = global::SONIC_THE_HEDGEHOG__2006__Randomiser_Suite.Properties.Resources.seedIcon;
            this.seedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seedButton.Location = new System.Drawing.Point(454, 52);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 23);
            this.seedButton.TabIndex = 54;
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.SeedButton_Click);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(128, 53);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(320, 20);
            this.seedBox.TabIndex = 53;
            this.seedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(12, 51);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(110, 23);
            this.seedLabel.TabIndex = 52;
            this.seedLabel.Text = "Randomisation Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(454, 26);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 51;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(128, 27);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(320, 20);
            this.filepathBox.TabIndex = 50;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.Location = new System.Drawing.Point(12, 25);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(110, 23);
            this.filepathLabel.TabIndex = 49;
            this.filepathLabel.Text = "Folder to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // deathCheckbox
            // 
            this.deathCheckbox.AutoSize = true;
            this.deathCheckbox.Checked = true;
            this.deathCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deathCheckbox.Location = new System.Drawing.Point(15, 127);
            this.deathCheckbox.Name = "deathCheckbox";
            this.deathCheckbox.Size = new System.Drawing.Size(133, 17);
            this.deathCheckbox.TabIndex = 59;
            this.deathCheckbox.Text = "Respect Death Planes";
            this.toolTip1.SetToolTip(this.deathCheckbox, "Don\'t remove the death plane tag from areas of collision that previously had it.");
            this.deathCheckbox.UseVisualStyleBackColor = true;
            // 
            // waterCheckbox
            // 
            this.waterCheckbox.AutoSize = true;
            this.waterCheckbox.Location = new System.Drawing.Point(15, 104);
            this.waterCheckbox.Name = "waterCheckbox";
            this.waterCheckbox.Size = new System.Drawing.Size(98, 17);
            this.waterCheckbox.TabIndex = 58;
            this.waterCheckbox.Text = "Respect Water";
            this.toolTip1.SetToolTip(this.waterCheckbox, "Don\'t remove the water tags from areas of collision that previously had it.");
            this.waterCheckbox.UseVisualStyleBackColor = true;
            // 
            // wallsCheckbox
            // 
            this.wallsCheckbox.AutoSize = true;
            this.wallsCheckbox.Checked = true;
            this.wallsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wallsCheckbox.Location = new System.Drawing.Point(15, 81);
            this.wallsCheckbox.Name = "wallsCheckbox";
            this.wallsCheckbox.Size = new System.Drawing.Size(95, 17);
            this.wallsCheckbox.TabIndex = 57;
            this.wallsCheckbox.Text = "Respect Walls";
            this.toolTip1.SetToolTip(this.wallsCheckbox, "Don\'t remove the wall tags from areas of collision that previously had it.");
            this.wallsCheckbox.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.surfaceTypeConfigurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(490, 24);
            this.menuStrip1.TabIndex = 60;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // surfaceTypeConfigurationToolStripMenuItem
            // 
            this.surfaceTypeConfigurationToolStripMenuItem.Name = "surfaceTypeConfigurationToolStripMenuItem";
            this.surfaceTypeConfigurationToolStripMenuItem.Size = new System.Drawing.Size(163, 20);
            this.surfaceTypeConfigurationToolStripMenuItem.Text = "Surface Type Configuration";
            this.surfaceTypeConfigurationToolStripMenuItem.Click += new System.EventHandler(this.SurfaceTypeConfigurationToolStripMenuItem_Click);
            // 
            // CollisionPropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 158);
            this.Controls.Add(this.deathCheckbox);
            this.Controls.Add(this.waterCheckbox);
            this.Controls.Add(this.wallsCheckbox);
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
            this.MaximizeBox = false;
            this.Name = "CollisionPropertiesForm";
            this.Text = "SONIC THE HEDGEHOG (2006) Collision Properties Randomiser";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.CheckBox deathCheckbox;
        private System.Windows.Forms.CheckBox waterCheckbox;
        private System.Windows.Forms.CheckBox wallsCheckbox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem surfaceTypeConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}