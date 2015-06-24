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

namespace Trax {
    
    public partial class Map : DockContent {

        public Map() {
            InitializeComponent();
            Text = Messages.TrackMap;
            Ctl.Focus();
            Ctl.Loaded += Ctl_MapLoaded;
            Ctl.ScaleChanged += Ctl_ScaleChanged;
            Ctl.PositionChanged += Ctl_PositionChanged;
            Ctl.CursorPositionChanged += Ctl_CursorPositionChanged;
            ShowGridButton.Checked = Ctl.ShowGrid;
            ShowDotsButton.Checked = Ctl.ShowDots;
            ScaleLabel.Text = String.Empty;
            CursorXLabel.Text = String.Empty;
            CursorZLabel.Text = String.Empty;
            PositionXLabel.Text = String.Empty;
            PositionZLabel.Text = String.Empty;
        }

        void Ctl_MapLoaded(object sender, EventArgs e) {
            CursorXLabel.Text = String.Format("X = {0}", Ctl.CursorPosition.X.ToString("00000.00m"));
            CursorZLabel.Text = String.Format("Z = {0}", Ctl.CursorPosition.Y.ToString("00000.00m"));
        }

        void Ctl_ScaleChanged(object sender, EventArgs e) {
            ScaleLabel.Text = String.Format("100m -> {0}px", (Ctl.Scale * 100).ToString("0.00"));
            PositionXLabel.Text = String.Format("X = {0}", Ctl.Position.X.ToString("00000.00m"));
            PositionZLabel.Text = String.Format("Z = {0}", Ctl.Position.Y.ToString("00000.00m"));
            ResetViewButton.Checked = Ctl.IsOutFull;
        }

        void Ctl_PositionChanged(object sender, EventArgs e) {
            PositionXLabel.Text = String.Format("X = {0}", Ctl.Position.X.ToString("00000.00m"));
            PositionZLabel.Text = String.Format("Z = {0}", Ctl.Position.Y.ToString("00000.00m"));
        }

        void Ctl_CursorPositionChanged(object sender, EventArgs e) {
            CursorXLabel.Text = String.Format("X = {0}", Ctl.CursorPosition.X.ToString("00000.00m"));
            CursorZLabel.Text = String.Format("Z = {0}", Ctl.CursorPosition.Y.ToString("00000.00m"));
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

        private void FindButton_Click(object sender, EventArgs e) {
            Ctl.FindName(FindBox.Text);
        }

        private void FindBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) FindButton_Click(sender, EventArgs.Empty);
        }

    }
}
