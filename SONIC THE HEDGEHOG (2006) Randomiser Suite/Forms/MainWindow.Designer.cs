namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.setButton = new System.Windows.Forms.Button();
            this.lightButton = new System.Windows.Forms.Button();
            this.characterButton = new System.Windows.Forms.Button();
            this.musicButton = new System.Windows.Forms.Button();
            this.cleanupButton = new System.Windows.Forms.Button();
            this.filepathBox = new System.Windows.Forms.TextBox();
            this.filepathButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.missionButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(12, 29);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(404, 23);
            this.setButton.TabIndex = 0;
            this.setButton.Text = "SET Randomiser";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // lightButton
            // 
            this.lightButton.Location = new System.Drawing.Point(12, 58);
            this.lightButton.Name = "lightButton";
            this.lightButton.Size = new System.Drawing.Size(404, 23);
            this.lightButton.TabIndex = 1;
            this.lightButton.Text = "Light Randomiser";
            this.lightButton.UseVisualStyleBackColor = true;
            this.lightButton.Click += new System.EventHandler(this.LightButton_Click);
            // 
            // characterButton
            // 
            this.characterButton.Location = new System.Drawing.Point(12, 166);
            this.characterButton.Name = "characterButton";
            this.characterButton.Size = new System.Drawing.Size(404, 23);
            this.characterButton.TabIndex = 2;
            this.characterButton.Text = "Character Attributes Randomiser";
            this.characterButton.UseVisualStyleBackColor = true;
            this.characterButton.Click += new System.EventHandler(this.CharacterButton_Click);
            // 
            // musicButton
            // 
            this.musicButton.Location = new System.Drawing.Point(12, 87);
            this.musicButton.Name = "musicButton";
            this.musicButton.Size = new System.Drawing.Size(404, 23);
            this.musicButton.TabIndex = 3;
            this.musicButton.Text = "Music Randomiser";
            this.musicButton.UseVisualStyleBackColor = true;
            this.musicButton.Click += new System.EventHandler(this.MusicButton_Click);
            // 
            // cleanupButton
            // 
            this.cleanupButton.Location = new System.Drawing.Point(12, 224);
            this.cleanupButton.Name = "cleanupButton";
            this.cleanupButton.Size = new System.Drawing.Size(59, 22);
            this.cleanupButton.TabIndex = 4;
            this.cleanupButton.Text = "Clean Up";
            this.cleanupButton.UseVisualStyleBackColor = true;
            this.cleanupButton.Click += new System.EventHandler(this.CleanupButton_Click);
            // 
            // filepathBox
            // 
            this.filepathBox.Location = new System.Drawing.Point(77, 226);
            this.filepathBox.Name = "filepathBox";
            this.filepathBox.Size = new System.Drawing.Size(309, 20);
            this.filepathBox.TabIndex = 5;
            this.filepathBox.TextChanged += new System.EventHandler(this.FilepathBox_TextChanged);
            // 
            // filepathButton
            // 
            this.filepathButton.Location = new System.Drawing.Point(392, 224);
            this.filepathButton.Name = "filepathButton";
            this.filepathButton.Size = new System.Drawing.Size(24, 23);
            this.filepathButton.TabIndex = 53;
            this.filepathButton.Text = "...";
            this.filepathButton.UseVisualStyleBackColor = true;
            this.filepathButton.Click += new System.EventHandler(this.FilepathButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 253);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(404, 23);
            this.button2.TabIndex = 55;
            this.button2.Text = "About";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // missionButton
            // 
            this.missionButton.Location = new System.Drawing.Point(12, 116);
            this.missionButton.Name = "missionButton";
            this.missionButton.Size = new System.Drawing.Size(404, 23);
            this.missionButton.TabIndex = 56;
            this.missionButton.Text = "Mission Randomiser";
            this.missionButton.UseVisualStyleBackColor = true;
            this.missionButton.Click += new System.EventHandler(this.MissionButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(136, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 57;
            this.label1.Text = "scripts.arc Randomisation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(136, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "player.arc Randomisation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 292);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.missionButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.filepathButton);
            this.Controls.Add(this.filepathBox);
            this.Controls.Add(this.cleanupButton);
            this.Controls.Add(this.musicButton);
            this.Controls.Add(this.characterButton);
            this.Controls.Add(this.lightButton);
            this.Controls.Add(this.setButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "SONIC THE HEDGEHOG (2006) Randomiser Suite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.Button lightButton;
        private System.Windows.Forms.Button characterButton;
        private System.Windows.Forms.Button musicButton;
        private System.Windows.Forms.Button cleanupButton;
        private System.Windows.Forms.TextBox filepathBox;
        private System.Windows.Forms.Button filepathButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button missionButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}