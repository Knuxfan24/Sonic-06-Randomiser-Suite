namespace SONIC_THE_HEDGEHOG__2006__Randomiser_Suite
{
    partial class EnemyConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnemyConfig));
            this.enemyConfigList = new System.Windows.Forms.CheckedListBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.uncheckButton = new System.Windows.Forms.Button();
            this.checkButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // enemyConfigList
            // 
            this.enemyConfigList.FormattingEnabled = true;
            this.enemyConfigList.Items.AddRange(new object[] {
            "Iblis Biter",
            "Iblis Golem",
            "Iblis Taker",
            "Iblis Worm",
            "Mephiles Gazer",
            "Mephiles Stalker",
            "Mephiles Titan",
            "Mephiles Tricker",
            "Egg Armour",
            "Egg Buster",
            "Egg Buster (Egg Gunner)",
            "Egg Buster (Egg Gunner) (Fly)",
            "Egg Bomber",
            "Egg Cannon",
            "Egg Cannon (Fly)",
            "Egg Chaser",
            "Egg Commander",
            "Egg Flyer",
            "Egg Guardian",
            "Egg Gunner",
            "Egg Gunner (Fly)",
            "Egg Hunter",
            "Egg Keeper",
            "Egg Lancer",
            "Egg Lancer(Fly)",
            "Egg Liner",
            "Egg Rounder",
            "Egg Searcher",
            "Egg Stinger",
            "Egg Stinger (Fly)",
            "Egg Sweeper",
            "Egg Walker",
            "Egg Cerberus",
            "Egg Genesis",
            "Egg Wyvern",
            "Iblis (Phase 1)",
            "Iblis (Phase 2)",
            "Iblis (Phase 3)",
            "Mephiles (Phase 1)",
            "Solaris (Phase 1)",
            "Solaris (Phase 2)"});
            this.enemyConfigList.Location = new System.Drawing.Point(12, 12);
            this.enemyConfigList.Name = "enemyConfigList";
            this.enemyConfigList.Size = new System.Drawing.Size(416, 394);
            this.enemyConfigList.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(353, 413);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 414);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // uncheckButton
            // 
            this.uncheckButton.Location = new System.Drawing.Point(93, 414);
            this.uncheckButton.Name = "uncheckButton";
            this.uncheckButton.Size = new System.Drawing.Size(75, 23);
            this.uncheckButton.TabIndex = 3;
            this.uncheckButton.Text = "Uncheck All";
            this.uncheckButton.UseVisualStyleBackColor = true;
            this.uncheckButton.Click += new System.EventHandler(this.UncheckButton_Click);
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(272, 412);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(75, 23);
            this.checkButton.TabIndex = 4;
            this.checkButton.Text = "Check All";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // EnemyConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 449);
            this.Controls.Add(this.checkButton);
            this.Controls.Add(this.uncheckButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.enemyConfigList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EnemyConfig";
            this.Text = "SET Randomiser ~ Enemy Configuration";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox enemyConfigList;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button uncheckButton;
        private System.Windows.Forms.Button checkButton;
    }
}