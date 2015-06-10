namespace ScnEdit {
    partial class Map {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Map));
            this.MapToolStrip = new System.Windows.Forms.ToolStrip();
            this.CursorZLabel = new System.Windows.Forms.ToolStripLabel();
            this.CursorXLabel = new System.Windows.Forms.ToolStripLabel();
            this.CursorLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.PositionZLabel = new System.Windows.Forms.ToolStripLabel();
            this.PositionXLabel = new System.Windows.Forms.ToolStripLabel();
            this.PositionLabel = new System.Windows.Forms.ToolStripLabel();
            this.ScaleLabel = new System.Windows.Forms.ToolStripLabel();
            this.ScaleLabelTitle = new System.Windows.Forms.ToolStripLabel();
            this.ResetViewButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowGridButton = new System.Windows.Forms.ToolStripButton();
            this.ShowDotsButton = new System.Windows.Forms.ToolStripButton();
            this.Ctl = new ScnEdit.MapCtl();
            this.MapToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapToolStrip
            // 
            this.MapToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MapToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CursorZLabel,
            this.CursorXLabel,
            this.CursorLabel,
            this.toolStripSeparator3,
            this.PositionZLabel,
            this.PositionXLabel,
            this.PositionLabel,
            this.ScaleLabel,
            this.ScaleLabelTitle,
            this.ResetViewButton,
            this.toolStripSeparator1,
            this.ShowGridButton,
            this.ShowDotsButton});
            resources.ApplyResources(this.MapToolStrip, "MapToolStrip");
            this.MapToolStrip.Name = "MapToolStrip";
            // 
            // CursorZLabel
            // 
            this.CursorZLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.CursorZLabel, "CursorZLabel");
            this.CursorZLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.CursorZLabel.Name = "CursorZLabel";
            // 
            // CursorXLabel
            // 
            this.CursorXLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.CursorXLabel, "CursorXLabel");
            this.CursorXLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.CursorXLabel.Name = "CursorXLabel";
            // 
            // CursorLabel
            // 
            this.CursorLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.CursorLabel, "CursorLabel");
            this.CursorLabel.Name = "CursorLabel";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // PositionZLabel
            // 
            this.PositionZLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.PositionZLabel, "PositionZLabel");
            this.PositionZLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.PositionZLabel.Name = "PositionZLabel";
            // 
            // PositionXLabel
            // 
            this.PositionXLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.PositionXLabel, "PositionXLabel");
            this.PositionXLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.PositionXLabel.Name = "PositionXLabel";
            // 
            // PositionLabel
            // 
            this.PositionLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.PositionLabel, "PositionLabel");
            this.PositionLabel.Name = "PositionLabel";
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.ScaleLabel, "ScaleLabel");
            this.ScaleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.ScaleLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ScaleLabel.Name = "ScaleLabel";
            // 
            // ScaleLabelTitle
            // 
            this.ScaleLabelTitle.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.ScaleLabelTitle, "ScaleLabelTitle");
            this.ScaleLabelTitle.Name = "ScaleLabelTitle";
            // 
            // ResetViewButton
            // 
            this.ResetViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ResetViewButton, "ResetViewButton");
            this.ResetViewButton.Name = "ResetViewButton";
            this.ResetViewButton.Click += new System.EventHandler(this.ResetViewButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // ShowGridButton
            // 
            this.ShowGridButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ShowGridButton, "ShowGridButton");
            this.ShowGridButton.Name = "ShowGridButton";
            this.ShowGridButton.Click += new System.EventHandler(this.ShowGridButton_Click);
            // 
            // ShowDotsButton
            // 
            this.ShowDotsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.ShowDotsButton, "ShowDotsButton");
            this.ShowDotsButton.Name = "ShowDotsButton";
            this.ShowDotsButton.Click += new System.EventHandler(this.ShowDotsButton_Click);
            // 
            // Ctl
            // 
            this.Ctl.BackColor = System.Drawing.Color.White;
            this.Ctl.CrossColor = System.Drawing.Color.LightGray;
            this.Ctl.DetailTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(68)))));
            resources.ApplyResources(this.Ctl, "Ctl");
            this.Ctl.DotColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(34)))), ((int)(((byte)(0)))));
            this.Ctl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(51)))), ((int)(((byte)(49)))));
            this.Ctl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.Ctl.GridZeroColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.Ctl.Name = "Ctl";
            this.Ctl.RiverColor = System.Drawing.Color.SteelBlue;
            this.Ctl.RoadColor = System.Drawing.Color.LightGray;
            this.Ctl.SelectedTrackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(34)))), ((int)(((byte)(0)))));
            this.Ctl.ShowDots = false;
            this.Ctl.ShowGrid = false;
            this.Ctl.SwitchColor = System.Drawing.Color.Black;
            this.Ctl.TrackColor = System.Drawing.Color.Black;
            // 
            // Map
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Ctl);
            this.Controls.Add(this.MapToolStrip);
            this.Name = "Map";
            this.MapToolStrip.ResumeLayout(false);
            this.MapToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapCtl Ctl;
        private System.Windows.Forms.ToolStrip MapToolStrip;
        private System.Windows.Forms.ToolStripLabel CursorZLabel;
        private System.Windows.Forms.ToolStripLabel CursorXLabel;
        private System.Windows.Forms.ToolStripLabel ScaleLabel;
        private System.Windows.Forms.ToolStripButton ResetViewButton;
        private System.Windows.Forms.ToolStripButton ShowGridButton;
        private System.Windows.Forms.ToolStripButton ShowDotsButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel CursorLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel PositionZLabel;
        private System.Windows.Forms.ToolStripLabel PositionXLabel;
        private System.Windows.Forms.ToolStripLabel PositionLabel;
        private System.Windows.Forms.ToolStripLabel ScaleLabelTitle;
    }
}