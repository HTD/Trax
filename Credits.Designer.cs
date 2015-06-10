namespace Trax {
    partial class Credits {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Credits));
            this.VersionLabel = new System.Windows.Forms.Label();
            this.CreditsTitlesLabel = new System.Windows.Forms.Label();
            this.CreditsListLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.VersionLabel.ForeColor = System.Drawing.Color.White;
            this.VersionLabel.Location = new System.Drawing.Point(516, 235);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(37, 13);
            this.VersionLabel.TabIndex = 0;
            this.VersionLabel.Text = "v0.8.8";
            // 
            // CreditsTitlesLabel
            // 
            this.CreditsTitlesLabel.AutoSize = true;
            this.CreditsTitlesLabel.BackColor = System.Drawing.Color.Transparent;
            this.CreditsTitlesLabel.ForeColor = System.Drawing.Color.White;
            this.CreditsTitlesLabel.Location = new System.Drawing.Point(396, 9);
            this.CreditsTitlesLabel.Name = "CreditsTitlesLabel";
            this.CreditsTitlesLabel.Size = new System.Drawing.Size(148, 39);
            this.CreditsTitlesLabel.TabIndex = 1;
            this.CreditsTitlesLabel.Text = "Edior code .............................\r\nFast Colored TextBox ............\r\nDock" +
    " Panel Suite ..................";
            // 
            // CreditsListLabel
            // 
            this.CreditsListLabel.AutoSize = true;
            this.CreditsListLabel.BackColor = System.Drawing.Color.Transparent;
            this.CreditsListLabel.ForeColor = System.Drawing.Color.White;
            this.CreditsListLabel.Location = new System.Drawing.Point(540, 9);
            this.CreditsListLabel.Name = "CreditsListLabel";
            this.CreditsListLabel.Size = new System.Drawing.Size(88, 130);
            this.CreditsListLabel.TabIndex = 2;
            this.CreditsListLabel.Text = "Adam Łyskawa\r\nPavel Torgashov\r\nWeiFen Luo\r\nSteve Overton\r\nJulien Bouvrais\r\nMark T" +
    "wombley\r\nDaniel Krebser\r\nKamen Avramov\r\nRyan Rastedt\r\nLex Li";
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.AutoSize = true;
            this.CopyrightLabel.BackColor = System.Drawing.Color.Transparent;
            this.CopyrightLabel.ForeColor = System.Drawing.Color.White;
            this.CopyrightLabel.Location = new System.Drawing.Point(12, 9);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(142, 26);
            this.CopyrightLabel.TabIndex = 3;
            this.CopyrightLabel.Text = "Licensed under MIT License\r\n(c)2015 by Adam Łyskawa";
            // 
            // Credits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(648, 283);
            this.Controls.Add(this.CopyrightLabel);
            this.Controls.Add(this.CreditsListLabel);
            this.Controls.Add(this.CreditsTitlesLabel);
            this.Controls.Add(this.VersionLabel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Credits";
            this.Opacity = 0.85D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credits";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label CreditsTitlesLabel;
        private System.Windows.Forms.Label CreditsListLabel;
        private System.Windows.Forms.Label CopyrightLabel;
    }
}