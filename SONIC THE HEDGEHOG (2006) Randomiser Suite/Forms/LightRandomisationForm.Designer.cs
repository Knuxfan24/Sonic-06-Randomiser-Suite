namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class LightRandomisationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LightRandomisationForm));
            this.seedButton = new System.Windows.Forms.Button();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.randomFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.subCheckbox = new System.Windows.Forms.CheckBox();
            this.mainCheckbox = new System.Windows.Forms.CheckBox();
            this.ambientCheckbox = new System.Windows.Forms.CheckBox();
            this.directionCheckbox = new System.Windows.Forms.CheckBox();
            this.fogColourCheckbox = new System.Windows.Forms.CheckBox();
            this.fogDensityCheckbox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // seedButton
            // 
            this.seedButton.BackgroundImage = global::SONIC_THE_HEDGEHOG__2006__Randomiser_Suite.Properties.Resources.seedIcon;
            this.seedButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.seedButton.Location = new System.Drawing.Point(454, 37);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 23);
            this.seedButton.TabIndex = 17;
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.SeedButton_Click);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(128, 38);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(320, 20);
            this.seedBox.TabIndex = 16;
            this.seedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(12, 36);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(110, 23);
            this.seedLabel.TabIndex = 15;
            this.seedLabel.Text = "Randomisation Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(454, 11);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 11;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(128, 12);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(320, 20);
            this.filepathBox.TabIndex = 10;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.Location = new System.Drawing.Point(12, 10);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(110, 23);
            this.filepathLabel.TabIndex = 9;
            this.filepathLabel.Text = "Folder to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // randomFolderCheckbox
            // 
            this.randomFolderCheckbox.AutoSize = true;
            this.randomFolderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.randomFolderCheckbox.Location = new System.Drawing.Point(367, 66);
            this.randomFolderCheckbox.Name = "randomFolderCheckbox";
            this.randomFolderCheckbox.Size = new System.Drawing.Size(111, 17);
            this.randomFolderCheckbox.TabIndex = 48;
            this.randomFolderCheckbox.Text = "Randomise Folder";
            this.toolTip1.SetToolTip(this.randomFolderCheckbox, "Randomise a folder and subfolders instead of one LUA file.");
            this.randomFolderCheckbox.UseVisualStyleBackColor = true;
            this.randomFolderCheckbox.CheckedChanged += new System.EventHandler(this.RandomFolderCheckbox_CheckedChanged);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(403, 177);
            this.randomiseButton.Name = "randomiseButton";
            this.randomiseButton.Size = new System.Drawing.Size(75, 23);
            this.randomiseButton.TabIndex = 45;
            this.randomiseButton.Text = "Randomise";
            this.randomiseButton.UseVisualStyleBackColor = true;
            this.randomiseButton.Click += new System.EventHandler(this.RandomiseButton_Click);
            // 
            // subCheckbox
            // 
            this.subCheckbox.AutoSize = true;
            this.subCheckbox.Location = new System.Drawing.Point(15, 112);
            this.subCheckbox.Name = "subCheckbox";
            this.subCheckbox.Size = new System.Drawing.Size(127, 17);
            this.subCheckbox.TabIndex = 52;
            this.subCheckbox.Text = "Randomise Sub Light";
            this.toolTip1.SetToolTip(this.subCheckbox, "Randomise the colour of the Sub Lighting in the stage.");
            this.subCheckbox.UseVisualStyleBackColor = true;
            // 
            // mainCheckbox
            // 
            this.mainCheckbox.AutoSize = true;
            this.mainCheckbox.Location = new System.Drawing.Point(15, 89);
            this.mainCheckbox.Name = "mainCheckbox";
            this.mainCheckbox.Size = new System.Drawing.Size(131, 17);
            this.mainCheckbox.TabIndex = 51;
            this.mainCheckbox.Text = "Randomise Main Light";
            this.toolTip1.SetToolTip(this.mainCheckbox, "Randomise the colour of the Main Light in the stage.");
            this.mainCheckbox.UseVisualStyleBackColor = true;
            // 
            // ambientCheckbox
            // 
            this.ambientCheckbox.AutoSize = true;
            this.ambientCheckbox.Location = new System.Drawing.Point(15, 66);
            this.ambientCheckbox.Name = "ambientCheckbox";
            this.ambientCheckbox.Size = new System.Drawing.Size(146, 17);
            this.ambientCheckbox.TabIndex = 50;
            this.ambientCheckbox.Text = "Randomise Ambient Light";
            this.toolTip1.SetToolTip(this.ambientCheckbox, "Randomise the colour of the Ambient Lighting in the stage.");
            this.ambientCheckbox.UseVisualStyleBackColor = true;
            // 
            // directionCheckbox
            // 
            this.directionCheckbox.AutoSize = true;
            this.directionCheckbox.Location = new System.Drawing.Point(15, 135);
            this.directionCheckbox.Name = "directionCheckbox";
            this.directionCheckbox.Size = new System.Drawing.Size(150, 17);
            this.directionCheckbox.TabIndex = 53;
            this.directionCheckbox.Text = "Randomise Light Direction";
            this.toolTip1.SetToolTip(this.directionCheckbox, "Randomise the direction the light comes from in the stage.");
            this.directionCheckbox.UseVisualStyleBackColor = true;
            // 
            // fogColourCheckbox
            // 
            this.fogColourCheckbox.AutoSize = true;
            this.fogColourCheckbox.Location = new System.Drawing.Point(15, 158);
            this.fogColourCheckbox.Name = "fogColourCheckbox";
            this.fogColourCheckbox.Size = new System.Drawing.Size(133, 17);
            this.fogColourCheckbox.TabIndex = 54;
            this.fogColourCheckbox.Text = "Randomise Fog Colour";
            this.toolTip1.SetToolTip(this.fogColourCheckbox, "Randomise the colour of the fog in the stage.");
            this.fogColourCheckbox.UseVisualStyleBackColor = true;
            // 
            // fogDensityCheckbox
            // 
            this.fogDensityCheckbox.AutoSize = true;
            this.fogDensityCheckbox.Location = new System.Drawing.Point(15, 181);
            this.fogDensityCheckbox.Name = "fogDensityCheckbox";
            this.fogDensityCheckbox.Size = new System.Drawing.Size(138, 17);
            this.fogDensityCheckbox.TabIndex = 55;
            this.fogDensityCheckbox.Text = "Randomise Fog Density";
            this.toolTip1.SetToolTip(this.fogDensityCheckbox, "Randomise how thick the fog in the stage is.");
            this.fogDensityCheckbox.UseVisualStyleBackColor = true;
            // 
            // LightRandomisationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 209);
            this.Controls.Add(this.fogDensityCheckbox);
            this.Controls.Add(this.fogColourCheckbox);
            this.Controls.Add(this.directionCheckbox);
            this.Controls.Add(this.subCheckbox);
            this.Controls.Add(this.mainCheckbox);
            this.Controls.Add(this.ambientCheckbox);
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
            this.Name = "LightRandomisationForm";
            this.Text = "SONIC THE HEDGEHOG (2006) Light Randomiser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button seedButton;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.CheckBox randomFolderCheckbox;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.CheckBox subCheckbox;
        private System.Windows.Forms.CheckBox mainCheckbox;
        private System.Windows.Forms.CheckBox ambientCheckbox;
        private System.Windows.Forms.CheckBox directionCheckbox;
        private System.Windows.Forms.CheckBox fogColourCheckbox;
        private System.Windows.Forms.CheckBox fogDensityCheckbox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}