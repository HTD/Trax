using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ScnEdit {
    
    public partial class Map : DockContent {

        public Map() {
            InitializeComponent();
            Text = Messages.TrackMap;
            Ctl.Focus();
            Ctl.Loaded += Ctl_MapLoaded;
            Ctl.ScaleChanged += Ctl_ScaleChanged;
            Ctl.OffsetChanged += Ctl_OffsetChanged;
            Ctl.CursorPositionChanged += Ctl_CursorPositionChanged;
            Ctl.ShowGrid = true;
            ShowGridButton.Checked = true;
            ScaleLabel.Text = String.Empty;
            XLabel.Text = String.Empty;
            ZLabel.Text = String.Empty;
            XOffsetLabel.Text = String.Empty;
            ZOffsetLabel.Text = String.Empty;
        }

        void Ctl_MapLoaded(object sender, EventArgs e) {
            XLabel.Text = String.Format("X = {0}", Ctl.CursorPosition.X.ToString("00000.00m"));
            ZLabel.Text = String.Format("Z = {0}", Ctl.CursorPosition.Y.ToString("00000.00m"));
        }

        void Ctl_ScaleChanged(object sender, EventArgs e) {
            ScaleLabel.Text = String.Format("100m -> {0}px", (Ctl.Scale * 100).ToString("0.00"));
            XOffsetLabel.Text = String.Format("X = {0}", Ctl.MapOffset.X.ToString("00000.00m"));
            ZOffsetLabel.Text = String.Format("Z = {0}", Ctl.MapOffset.Y.ToString("00000.00m"));
            ResetViewButton.Checked = Ctl.OutFull;
        }

        void Ctl_OffsetChanged(object sender, EventArgs e) {
            XOffsetLabel.Text = String.Format("X = {0}", Ctl.MapOffset.X.ToString("00000.00m"));
            ZOffsetLabel.Text = String.Format("Z = {0}", Ctl.MapOffset.Y.ToString("00000.00m"));
        }

        void Ctl_CursorPositionChanged(object sender, EventArgs e) {
            XLabel.Text = String.Format("X = {0}", Ctl.CursorPosition.X.ToString("00000.00m"));
            ZLabel.Text = String.Format("Z = {0}", Ctl.CursorPosition.Y.ToString("00000.00m"));
        }

        private void ResetViewButton_Click(object sender, EventArgs e) {
            Ctl.ResetView();
        }
        private void ShowGridButton_Click(object sender, EventArgs e) {
            var s = sender as ToolStripButton;
            var c = !s.Checked;
            Ctl.ShowGrid = s.Checked = c;
            Ctl.Invalidate();
        }

        private void ShowDotsButton_Click(object sender, EventArgs e) {
            var s = sender as ToolStripButton;
            var c = !s.Checked;
            Ctl.ShowDots = s.Checked = c;
            Ctl.Invalidate();
        }

    }
}
