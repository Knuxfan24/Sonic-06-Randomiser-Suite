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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractSetWindow));
            this.filepathLabel = new System.Windows.Forms.Label();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathButton = new System.Windows.Forms.Button();
            this.outputLabel = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.extractButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.sourceToggle = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // filepathLabel
            // 
            this.filepathLabel.AutoSize = true;
            this.filepathLabel.Location = new System.Drawing.Point(20, 15);
            this.filepathLabel.Name = "filepathLabel";
            this.filepathLabel.Size = new System.Drawing.Size(79, 13);
            this.filepathLabel.TabIndex = 9;
            this.filepathLabel.Text = "SET to Extract:";
            this.filepathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(105, 12);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(344, 20);
            this.filepathBox.TabIndex = 10;
            this.filepathBox.TextChanged += new System.EventHandler(this.filepathBox_TextChanged);
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(455, 12);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 22);
            this.filepathButton.TabIndex = 11;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.filepathButton_Click);
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.Location = new System.Drawing.Point(12, 38);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(87, 13);
            this.outputLabel.TabIndex = 12;
            this.outputLabel.Text = "Output Directory:";
            this.outputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(105, 35);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(344, 20);
            this.outputBox.TabIndex = 13;
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(455, 35);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(24, 22);
            this.outputButton.TabIndex = 14;
            this.outputButton.Text = "...";
            this.toolTip.SetToolTip(this.outputButton, "Browse for an output folder.");
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.outputButton_Click);
            // 
            // extractButton
            // 
            this.extractButton.Enabled = false;
            this.extractButton.Location = new System.Drawing.Point(404, 84);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(75, 23);
            this.extractButton.TabIndex = 16;
            this.extractButton.Text = "Extract";
            this.toolTip.SetToolTip(this.extractButton, "Extract the chosen SET file to the chosen directory.");
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // sourceToggle
            // 
            this.sourceToggle.AutoSize = true;
            this.sourceToggle.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sourceToggle.Location = new System.Drawing.Point(335, 61);
            this.sourceToggle.Name = "sourceToggle";
            this.sourceToggle.Size = new System.Drawing.Size(144, 17);
            this.sourceToggle.TabIndex = 27;
            this.sourceToggle.Text = "Save in Source Directory";
            this.sourceToggle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.sourceToggle, "Save in the same folder as the SET if no Output Directory is specified.");
            this.sourceToggle.UseVisualStyleBackColor = true;
            this.sourceToggle.CheckedChanged += new System.EventHandler(this.sourceToggle_CheckedChanged);
            // 
            // ExtractSetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 115);
            this.Controls.Add(this.sourceToggle);
            this.Controls.Add(this.extractButton);
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

        private System.Windows.Forms.Label filepathLabel;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox sourceToggle;
    }
}