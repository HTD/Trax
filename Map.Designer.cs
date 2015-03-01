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
            this.ZLabel = new System.Windows.Forms.ToolStripLabel();
            this.XLabel = new System.Windows.Forms.ToolStripLabel();
            this.PositionLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ZOffsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.XOffsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.OffsetLabel = new System.Windows.Forms.ToolStripLabel();
            this.ScaleLabel = new System.Windows.Forms.ToolStripLabel();
            this.ScaleLabelTitle = new System.Windows.Forms.ToolStripLabel();
            this.ResetViewButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowGridButton = new System.Windows.Forms.ToolStripButton();
            this.ShowDotsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectButton = new System.Windows.Forms.ToolStripButton();
            this.Ctl = new ScnEdit.MapCtl();
            this.MapToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapToolStrip
            // 
            this.MapToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MapToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZLabel,
            this.XLabel,
            this.PositionLabel,
            this.toolStripSeparator3,
            this.ZOffsetLabel,
            this.XOffsetLabel,
            this.OffsetLabel,
            this.ScaleLabel,
            this.ScaleLabelTitle,
            this.ResetViewButton,
            this.toolStripSeparator1,
            this.ShowGridButton,
            this.ShowDotsButton,
            this.toolStripSeparator2,
            this.SelectButton});
            this.MapToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MapToolStrip.Name = "MapToolStrip";
            this.MapToolStrip.Size = new System.Drawing.Size(957, 25);
            this.MapToolStrip.TabIndex = 1;
            this.MapToolStrip.Text = "toolStrip1";
            // 
            // ZLabel
            // 
            this.ZLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ZLabel.AutoSize = false;
            this.ZLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.ZLabel.Name = "ZLabel";
            this.ZLabel.Size = new System.Drawing.Size(100, 22);
            this.ZLabel.Text = "Z = +00000,00m";
            this.ZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // XLabel
            // 
            this.XLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.XLabel.AutoSize = false;
            this.XLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(100, 22);
            this.XLabel.Text = "X = +00000,00m";
            this.XLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PositionLabel
            // 
            this.PositionLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.PositionLabel.AutoSize = false;
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(80, 22);
            this.PositionLabel.Text = "Position:";
            this.PositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ZOffsetLabel
            // 
            this.ZOffsetLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ZOffsetLabel.AutoSize = false;
            this.ZOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.ZOffsetLabel.Name = "ZOffsetLabel";
            this.ZOffsetLabel.Size = new System.Drawing.Size(100, 22);
            this.ZOffsetLabel.Text = "Z = +00000,00m";
            this.ZOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // XOffsetLabel
            // 
            this.XOffsetLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.XOffsetLabel.AutoSize = false;
            this.XOffsetLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.XOffsetLabel.Name = "XOffsetLabel";
            this.XOffsetLabel.Size = new System.Drawing.Size(100, 22);
            this.XOffsetLabel.Text = "X = +00000,00m";
            this.XOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OffsetLabel
            // 
            this.OffsetLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.OffsetLabel.AutoSize = false;
            this.OffsetLabel.Name = "OffsetLabel";
            this.OffsetLabel.Size = new System.Drawing.Size(80, 22);
            this.OffsetLabel.Text = "Offset:";
            this.OffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ScaleLabel.AutoSize = false;
            this.ScaleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.ScaleLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(125, 22);
            this.ScaleLabel.Text = "100m -> 100,00px";
            // 
            // ScaleLabelTitle
            // 
            this.ScaleLabelTitle.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ScaleLabelTitle.AutoSize = false;
            this.ScaleLabelTitle.Name = "ScaleLabelTitle";
            this.ScaleLabelTitle.Size = new System.Drawing.Size(80, 22);
            this.ScaleLabelTitle.Text = "Scale:";
            this.ScaleLabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ResetViewButton
            // 
            this.ResetViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ResetViewButton.Image = ((System.Drawing.Image)(resources.GetObject("ResetViewButton.Image")));
            this.ResetViewButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResetViewButton.Name = "ResetViewButton";
            this.ResetViewButton.Size = new System.Drawing.Size(23, 22);
            this.ResetViewButton.Text = "Reset view";
            this.ResetViewButton.Click += new System.EventHandler(this.ResetViewButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ShowGridButton
            // 
            this.ShowGridButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowGridButton.Image = ((System.Drawing.Image)(resources.GetObject("ShowGridButton.Image")));
            this.ShowGridButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowGridButton.Name = "ShowGridButton";
            this.ShowGridButton.Size = new System.Drawing.Size(23, 22);
            this.ShowGridButton.Text = "Show grid";
            this.ShowGridButton.Click += new System.EventHandler(this.ShowGridButton_Click);
            // 
            // ShowDotsButton
            // 
            this.ShowDotsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ShowDotsButton.Image = ((System.Drawing.Image)(resources.GetObject("ShowDotsButton.Image")));
            this.ShowDotsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ShowDotsButton.Name = "ShowDotsButton";
            this.ShowDotsButton.Size = new System.Drawing.Size(23, 22);
            this.ShowDotsButton.Text = "Show dots";
            this.ShowDotsButton.Click += new System.EventHandler(this.ShowDotsButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // SelectButton
            // 
            this.SelectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SelectButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectButton.Image")));
            this.SelectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(23, 22);
            this.SelectButton.Text = "Select mode";
            // 
            // Ctl
            // 
            this.Ctl.BackColor = System.Drawing.Color.White;
            this.Ctl.CrossColor = System.Drawing.Color.LightGray;
            this.Ctl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ctl.DotColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(34)))), ((int)(((byte)(0)))));
            this.Ctl.Enabled = false;
            this.Ctl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Ctl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(51)))), ((int)(((byte)(49)))));
            this.Ctl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.Ctl.Location = new System.Drawing.Point(0, 25);
            this.Ctl.Name = "Ctl";
            this.Ctl.RiverColor = System.Drawing.Color.SteelBlue;
            this.Ctl.RoadColor = System.Drawing.Color.LightGray;
            this.Ctl.ShowDots = false;
            this.Ctl.ShowGrid = false;
            this.Ctl.Size = new System.Drawing.Size(957, 450);
            this.Ctl.SwitchColor = System.Drawing.Color.Black;
            this.Ctl.TabIndex = 0;
            this.Ctl.TrackColor = System.Drawing.Color.Black;
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 475);
            this.Controls.Add(this.Ctl);
            this.Controls.Add(this.MapToolStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Name = "Map";
            this.Text = "Map";
            this.MapToolStrip.ResumeLayout(false);
            this.MapToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapCtl Ctl;
        private System.Windows.Forms.ToolStrip MapToolStrip;
        private System.Windows.Forms.ToolStripLabel ZLabel;
        private System.Windows.Forms.ToolStripLabel XLabel;
        private System.Windows.Forms.ToolStripLabel ScaleLabel;
        private System.Windows.Forms.ToolStripButton ResetViewButton;
        private System.Windows.Forms.ToolStripButton ShowGridButton;
        private System.Windows.Forms.ToolStripButton ShowDotsButton;
        private System.Windows.Forms.ToolStripButton SelectButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel PositionLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel ZOffsetLabel;
        private System.Windows.Forms.ToolStripLabel XOffsetLabel;
        private System.Windows.Forms.ToolStripLabel OffsetLabel;
        private System.Windows.Forms.ToolStripLabel ScaleLabelTitle;
    }
}