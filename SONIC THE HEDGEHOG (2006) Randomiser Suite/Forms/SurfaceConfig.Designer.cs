namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class SurfaceConfig
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
            this.checkButton = new System.Windows.Forms.Button();
            this.uncheckButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.surfaceConfigList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(272, 412);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.TabIndex = 14;
            this.checkButton.Text = "Check All";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // uncheckButton
            // 
            this.uncheckButton.Location = new System.Drawing.Point(93, 414);
            this.uncheckButton.Name = "uncheckButton";
            this.uncheckButton.Size = new System.Drawing.Size(75, 23);
            this.uncheckButton.TabIndex = 13;
            this.uncheckButton.Text = "Uncheck All";
            this.uncheckButton.UseVisualStyleBackColor = true;
            this.uncheckButton.Click += new System.EventHandler(this.UncheckButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 414);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(353, 413);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // surfaceConfigList
            // 
            this.surfaceConfigList.FormattingEnabled = true;
            this.surfaceConfigList.Items.AddRange(new object[] {
            "Stone",
            "Shallow Water",
            "Wood",
            "Metal",
            "Grass",
            "Sand",
            "Snow",
            "Dirt",
            "Glass?",
            "Echoey Metal"});
            this.surfaceConfigList.Location = new System.Drawing.Point(12, 12);
            this.surfaceConfigList.Name = "surfaceConfigList";
            this.surfaceConfigList.Size = new System.Drawing.Size(416, 394);
            this.surfaceConfigList.TabIndex = 10;
            // 
            // SurfaceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 449);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.uncheckButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.surfaceConfigList);
            this.Name = "SurfaceConfig";
            this.Text = "SurfaceTypesConfig";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Button uncheckButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckedListBox surfaceConfigList;
    }
}