namespace Sonic06Randomiser
{
    partial class ImportSetWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportSetWindow));
            this.importButton = new System.Windows.Forms.Button();
            this.folderRandom = new System.Windows.Forms.CheckBox();
            this.customDirButton = new System.Windows.Forms.RadioButton();
            this.programDirButton = new System.Windows.Forms.RadioButton();
            this.sourceDirButton = new System.Windows.Forms.RadioButton();
            this.outputButton = new System.Windows.Forms.Button();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.filepathButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // importButton
            // 
            this.importButton.Enabled = false;
            this.importButton.Location = new System.Drawing.Point(403, 100);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(75, 23);
            this.importButton.TabIndex = 60;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // folderRandom
            // 
            this.folderRandom.AutoSize = true;
            this.folderRandom.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.folderRandom.Location = new System.Drawing.Point(391, 60);
            this.folderRandom.Name = "folderRandom";
            this.folderRandom.Size = new System.Drawing.Size(87, 17);
            this.folderRandom.TabIndex = 59;
            this.folderRandom.Text = "Import Folder";
            this.folderRandom.UseVisualStyleBackColor = true;
            this.folderRandom.CheckedChanged += new System.EventHandler(this.folderRandom_CheckedChanged);
            // 
            // customDirButton
            // 
            this.customDirButton.AutoSize = true;
            this.customDirButton.Checked = true;
            this.customDirButton.Location = new System.Drawing.Point(14, 60);
            this.customDirButton.Name = "customDirButton";
            this.customDirButton.Size = new System.Drawing.Size(144, 17);
            this.customDirButton.TabIndex = 58;
            this.customDirButton.TabStop = true;
            this.customDirButton.Text = "Save in Custom Directory";
            this.customDirButton.UseVisualStyleBackColor = true;
            this.customDirButton.CheckedChanged += new System.EventHandler(this.customDirButton_CheckedChanged);
            // 
            // programDirButton
            // 
            this.programDirButton.AutoSize = true;
            this.programDirButton.Location = new System.Drawing.Point(14, 106);
            this.programDirButton.Name = "programDirButton";
            this.programDirButton.Size = new System.Drawing.Size(148, 17);
            this.programDirButton.TabIndex = 57;
            this.programDirButton.Text = "Save in Program Directory";
            this.programDirButton.UseVisualStyleBackColor = true;
            this.programDirButton.CheckedChanged += new System.EventHandler(this.programDirButton_CheckedChanged);
            // 
            // sourceDirButton
            // 
            this.sourceDirButton.AutoSize = true;
            this.sourceDirButton.Location = new System.Drawing.Point(14, 83);
            this.sourceDirButton.Name = "sourceDirButton";
            this.sourceDirButton.Size = new System.Drawing.Size(143, 17);
            this.sourceDirButton.TabIndex = 56;
            this.sourceDirButton.Text = "Save in Source Directory";
            this.sourceDirButton.UseVisualStyleBackColor = true;
            this.sourceDirButton.CheckedChanged += new System.EventHandler(this.sourceDirButton_CheckedChanged);
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(454, 34);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(24, 22);
            this.outputButton.TabIndex = 55;
            this.outputButton.Text = "...";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(104, 34);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(344, 20);
            this.outputBox.TabIndex = 54;
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(11, 37);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(87, 13);
            this.outputLabel.TabIndex = 53;
            this.outputLabel.Text = "Output Directory:";
            this.outputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(454, 11);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 22);
            this.filepathButton.TabIndex = 52;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.filepathButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(104, 11);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(344, 20);
            this.filepathBox.TabIndex = 51;
            this.filepathBox.TextChanged += new System.EventHandler(this.filepathBox_TextChanged);
            // 
            // filepathLabel
            // 
            this.filepathLabel.AutoSize = true;
            this.filepathLabel.Location = new System.Drawing.Point(11, 14);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(76, 13);
            this.filepathLabel.TabIndex = 50;
            this.filepathLabel.Text = "XML to Import:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImportSetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 135);
            this.Controls.Add(this.importButton);
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
            this.Name = "ImportSetWindow";
            this.Text = "SONIC THE HEDGEHOG (2006) Randomiser (Set Importer)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.CheckBox folderRandom;
        private System.Windows.Forms.RadioButton customDirButton;
        private System.Windows.Forms.RadioButton programDirButton;
        private System.Windows.Forms.RadioButton sourceDirButton;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Label filepathLabel;
    }
}