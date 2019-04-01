namespace Sonic06Randomiser
{
    partial class ExtractSetWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractSetWindow));
            this.outputButton = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.customDirButton = new System.Windows.Forms.RadioButton();
            this.programDirButton = new System.Windows.Forms.RadioButton();
            this.sourceDirButton = new System.Windows.Forms.RadioButton();
            this.folderRandom = new System.Windows.Forms.CheckBox();
            this.extractButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(453, 35);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(24, 22);
            this.outputButton.TabIndex = 20;
            this.outputButton.Text = "...";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(103, 35);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(344, 20);
            this.outputBox.TabIndex = 19;
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(10, 38);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(87, 13);
            this.outputLabel.TabIndex = 18;
            this.outputLabel.Text = "Output Directory:";
            this.outputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(453, 12);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 22);
            this.filepathButton.TabIndex = 17;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.filepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(103, 12);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(344, 20);
            this.filepathBox.TabIndex = 16;
            this.filepathBox.TextChanged += new System.EventHandler(this.filepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.AutoSize = true;
            this.filepathLabel.Location = new System.Drawing.Point(10, 15);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(79, 13);
            this.filepathLabel.TabIndex = 15;
            this.filepathLabel.Text = "SET to Extract:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customDirButton
            // 
            this.customDirButton.AutoSize = true;
            this.customDirButton.Checked = true;
            this.customDirButton.Location = new System.Drawing.Point(13, 61);
            this.customDirButton.Name = "customDirButton";
            this.customDirButton.Size = new System.Drawing.Size(144, 17);
            this.customDirButton.TabIndex = 47;
            this.customDirButton.TabStop = true;
            this.customDirButton.Text = "Save in Custom Directory";
            this.customDirButton.UseVisualStyleBackColor = true;
            this.customDirButton.CheckedChanged += new System.EventHandler(this.customDirButton_CheckedChanged);
            // 
            // programDirButton
            // 
            this.programDirButton.AutoSize = true;
            this.programDirButton.Location = new System.Drawing.Point(13, 107);
            this.programDirButton.Name = "programDirButton";
            this.programDirButton.Size = new System.Drawing.Size(148, 17);
            this.programDirButton.TabIndex = 46;
            this.programDirButton.Text = "Save in Program Directory";
            this.programDirButton.UseVisualStyleBackColor = true;
            this.programDirButton.CheckedChanged += new System.EventHandler(this.programDirButton_CheckedChanged);
            // 
            // sourceDirButton
            // 
            this.sourceDirButton.AutoSize = true;
            this.sourceDirButton.Location = new System.Drawing.Point(13, 84);
            this.sourceDirButton.Name = "sourceDirButton";
            this.sourceDirButton.Size = new System.Drawing.Size(143, 17);
            this.sourceDirButton.TabIndex = 45;
            this.sourceDirButton.Text = "Save in Source Directory";
            this.sourceDirButton.UseVisualStyleBackColor = true;
            this.sourceDirButton.CheckedChanged += new System.EventHandler(this.sourceDirButton_CheckedChanged);
            // 
            // folderRandom
            // 
            this.folderRandom.AutoSize = true;
            this.folderRandom.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.folderRandom.Location = new System.Drawing.Point(385, 61);
            this.folderRandom.Name = "folderRandom";
            this.folderRandom.Size = new System.Drawing.Size(91, 17);
            this.folderRandom.TabIndex = 48;
            this.folderRandom.Text = "Extract Folder";
            this.folderRandom.UseVisualStyleBackColor = true;
            this.folderRandom.CheckedChanged += new System.EventHandler(this.folderRandom_CheckedChanged);
            // 
            // extractButton
            // 
            this.extractButton.Enabled = false;
            this.extractButton.Location = new System.Drawing.Point(402, 101);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 23);
            this.extractButton.TabIndex = 49;
            this.extractButton.Text = "Extract";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // ExtractSetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 135);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.folderRandom);
            this.Controls.Add(this.customDirButton);
            this.Controls.Add(this.programDirButton);
            this.Controls.Add(this.sourceDirButton);
            this.Controls.Add(this.outputButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.filepathButton);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.filepathLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ExtractSetWindow";
            this.Text = "SONIC THE HEDGEHOG (2006) Randomiser (Set Extractor)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.RadioButton customDirButton;
        private System.Windows.Forms.RadioButton programDirButton;
        private System.Windows.Forms.RadioButton sourceDirButton;
        private System.Windows.Forms.CheckBox folderRandom;
        private System.Windows.Forms.Button extractButton;
    }
}