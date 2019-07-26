namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class MissionRandomisationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissionRandomisationForm));
            this.scoreCheckbox = new System.Windows.Forms.CheckBox();
            this.missionCheckbox = new System.Windows.Forms.CheckBox();
            this.randomFolderCheckbox = new System.Windows.Forms.CheckBox();
            this.randomiseButton = new System.Windows.Forms.Button();
            this.seedButton = new System.Windows.Forms.Button();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // scoreCheckbox
            // 
            this.scoreCheckbox.AutoSize = true;
            this.scoreCheckbox.Location = new System.Drawing.Point(15, 88);
            this.scoreCheckbox.Name = "scoreCheckbox";
            this.scoreCheckbox.Size = new System.Drawing.Size(209, 17);
            this.scoreCheckbox.TabIndex = 62;
            this.scoreCheckbox.Text = "Randomise Score Ranks and Bonuses";
            this.toolTip1.SetToolTip(this.scoreCheckbox, "Randomise the Time and Ring Bonuses for the stage. Alongside the rank requirement" +
        "s.");
            this.scoreCheckbox.UseVisualStyleBackColor = true;
            // 
            // missionCheckbox
            // 
            this.missionCheckbox.AutoSize = true;
            this.missionCheckbox.Location = new System.Drawing.Point(15, 65);
            this.missionCheckbox.Name = "missionCheckbox";
            this.missionCheckbox.Size = new System.Drawing.Size(181, 17);
            this.missionCheckbox.TabIndex = 61;
            this.missionCheckbox.Text = "Randomise Load Screen Mission";
            this.toolTip1.SetToolTip(this.missionCheckbox, "Randomise the Mission Text that appears on the Loading Screen.");
            this.missionCheckbox.UseVisualStyleBackColor = true;
            // 
            // randomFolderCheckbox
            // 
            this.randomFolderCheckbox.AutoSize = true;
            this.randomFolderCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.randomFolderCheckbox.Location = new System.Drawing.Point(367, 65);
            this.randomFolderCheckbox.Name = "randomFolderCheckbox";
            this.randomFolderCheckbox.Size = new System.Drawing.Size(111, 17);
            this.randomFolderCheckbox.TabIndex = 59;
            this.randomFolderCheckbox.Text = "Randomise Folder";
            this.toolTip1.SetToolTip(this.randomFolderCheckbox, "Randomise a folder and subfolders instead of one LUA file.");
            this.randomFolderCheckbox.UseVisualStyleBackColor = true;
            this.randomFolderCheckbox.CheckedChanged += new System.EventHandler(this.RandomFolderCheckbox_CheckedChanged);
            // 
            // randomiseButton
            // 
            this.randomiseButton.Location = new System.Drawing.Point(403, 88);
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
            this.seedButton.Location = new System.Drawing.Point(454, 36);
            this.seedButton.Name = "seedButton";
            this.seedButton.Size = new System.Drawing.Size(24, 23);
            this.seedButton.TabIndex = 57;
            this.seedButton.UseVisualStyleBackColor = true;
            this.seedButton.Click += new System.EventHandler(this.SeedButton_Click);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(128, 37);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(320, 20);
            this.seedBox.TabIndex = 56;
            this.seedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // seedLabel
            // 
            this.seedLabel.Location = new System.Drawing.Point(12, 35);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(110, 23);
            this.seedLabel.TabIndex = 55;
            this.seedLabel.Text = "Randomisation Seed:";
            this.seedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(454, 10);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 54;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(128, 11);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(320, 20);
            this.filepathBox.TabIndex = 53;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.Location = new System.Drawing.Point(12, 9);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(110, 23);
            this.filepathLabel.TabIndex = 52;
            this.filepathLabel.Text = "Folder to Randomise:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MissionRandomisationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 122);
            this.Controls.Add(this.scoreCheckbox);
            this.Controls.Add(this.missionCheckbox);
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
            this.Name = "MissionRandomisationForm";
            this.Text = "SONIC THE HEDGEHOG (2006) Mission Randomiser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox scoreCheckbox;
        private System.Windows.Forms.CheckBox missionCheckbox;
        private System.Windows.Forms.CheckBox randomFolderCheckbox;
        private System.Windows.Forms.Button randomiseButton;
        private System.Windows.Forms.Button seedButton;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}