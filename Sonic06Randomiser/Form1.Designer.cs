namespace Sonic06Randomiser
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(404, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Randomise";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(115, 12);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(334, 20);
            this.filepathBox.TabIndex = 1;
            this.filepathBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(455, 12);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(24, 22);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "...";
            this.toolTip1.SetToolTip(this.browseButton, "Browse for an XML file to randomise.");
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 100);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(122, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Randomise Enemies";
            this.toolTip1.SetToolTip(this.checkBox1, "Randomise the enemies that spawn in the stage and their behaviour mode.");
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 123);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(148, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Randomise Item Capsules";
            this.toolTip1.SetToolTip(this.checkBox2, "Randomise what Item Capsules will contain.");
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(12, 146);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(133, 17);
            this.checkBox3.TabIndex = 5;
            this.checkBox3.Text = "Randomise Characters";
            this.toolTip1.SetToolTip(this.checkBox3, "Randomise which characters are used in the stage (affects the main spawn point an" +
        "d any points where control is switched to another character).");
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(115, 58);
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(334, 20);
            this.seedBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.seedBox, "String to hash for the randomise to use as a seed.");
            this.seedBox.TextChanged += new System.EventHandler(this.seedBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "XML to Randomise:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Output Directory:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(115, 35);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(334, 20);
            this.outputBox.TabIndex = 9;
            this.toolTip1.SetToolTip(this.outputBox, "Location to store the randomised sets (leave blank to store them in the applicati" +
        "on folder)");
            this.outputBox.TextChanged += new System.EventHandler(this.outputBox_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(455, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 22);
            this.button2.TabIndex = 10;
            this.button2.Text = "...";
            this.toolTip1.SetToolTip(this.button2, "Browse for an output folder.");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Seed:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(12, 169);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(150, 17);
            this.checkBox4.TabIndex = 12;
            this.checkBox4.Text = "Randomise Voice Triggers";
            this.toolTip1.SetToolTip(this.checkBox4, "Randomise what voice lines play at set points in the stage.");
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Enabled = false;
            this.checkBox5.Location = new System.Drawing.Point(168, 145);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(139, 17);
            this.checkBox5.TabIndex = 14;
            this.checkBox5.Text = "Include Alternate States";
            this.toolTip1.SetToolTip(this.checkBox5, "Include alternate character states for Sonic (his snowboards and Mach Speed state" +
        "s) in the randomisation.");
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::Sonic06Randomiser.Properties.Resources.seedIcon;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Cursor = System.Windows.Forms.Cursors.Default;
            this.button3.Location = new System.Drawing.Point(455, 58);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 22);
            this.button3.TabIndex = 13;
            this.toolTip1.SetToolTip(this.button3, "Generate a new random seed.");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(168, 168);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(153, 17);
            this.checkBox6.TabIndex = 15;
            this.checkBox6.Text = "Enable Randomisation Log";
            this.toolTip1.SetToolTip(this.checkBox6, "Output a log file showing what was changed when randomisation is complete.");
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 197);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seedBox);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "SONIC THE HEDGEHOG (2006) Randomiser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
    }
}

