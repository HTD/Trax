using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ScnEdit {
    
    class ProjectPanel : DockContent {

        private static Font _defaultFont = new Font("MS Reference Sans Serif", 8);

        internal static Main Main;
        internal ProjectTree Tree;
        internal TreeNode Root;
        internal ProjectFile Project;

        internal ProjectPanel(ProjectFile file) {
            SuspendLayout();
            Font = _defaultFont;
            CloseButton = false;
            CloseButtonVisible = false;
            Project = file;
            Text = Messages.Scenery;
            Tree = new ProjectTree() { Name = Text, Dock = DockStyle.Fill, LabelEdit = true };
            Root = Tree.Nodes.Add(Project.FileName, Project.FileName, 0);
            Root.Tag = Project;
            Project.GetReferences();
            Project.ReferencesResolved += file_ReferencesResolved;
            ReloadScheme();
            Controls.Add(Tree);
            DockPanel = Main.DockPanel;
            DockPanel.DockRightPortion = 300;
            Show(DockPanel, DockState.DockRightAutoHide);
            Tree.Sort();
            ResumeLayout();
        }

        public void ReloadScheme() {
            Tree.BackColor = EditorSyntax.Styles.ColorScheme.Background;            
            Tree.ForeColor = EditorSyntax.Styles.ColorScheme.ProjectText;
            Tree.SelectionBackColor = EditorSyntax.Styles.ColorScheme.ProjectSelection;
            Tree.FileIconColor = EditorSyntax.Styles.ColorScheme.ProjectIcon;
        }

        void file_ReferencesResolved(object sender, EventArgs e) {
            Project.References.ForEach(i => AddReference(i, Root));
            Root.Expand();
            Tree.Refresh();
        }

        void AddReference(ProjectFile reference, TreeNode node = null) {
            var refNode = node.Nodes.Add(reference.FileName, reference.FileName, 1);
            refNode.Tag = reference;
            if (reference.References != null && reference.References.Count > 0) {
                reference.References.ForEach(i => AddReference(i, refNode));
                refNode.Expand();
            }
        }

    }

    class ProjectTree : TreeView {

        #region Properties

        public Color SelectionBackColor { get; set; }
        public Color FileIconColor { get; set; }

        #endregion

        #region Double buffering VooDoo

        protected const int TV_FIRST = 0x1100;
        protected const int TVM_SETBKCOLOR = TV_FIRST + 29;
        protected const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
        protected const int TVS_EX_DOUBLEBUFFER = 0x0004;

        [DllImport("user32.dll")]
        protected static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        protected static bool IsWinXP {
            get {
                OperatingSystem OS = Environment.OSVersion;
                return (OS.Platform == PlatformID.Win32NT) &&
                    ((OS.Version.Major > 5) || ((OS.Version.Major == 5) && (OS.Version.Minor == 1)));
            }
        }

        private void UpdateExtendedStyles() {
            int Style = 0;
            if (DoubleBuffered) Style |= TVS_EX_DOUBLEBUFFER;
            if (Style != 0) SendMessage(Handle, TVM_SETEXTENDEDSTYLE, TVS_EX_DOUBLEBUFFER, Style);
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            UpdateExtendedStyles();
            if (!IsWinXP) SendMessage(Handle, TVM_SETBKCOLOR, 0, ColorTranslator.ToWin32(BackColor));
        }

        #endregion

        #region Shapes

        protected int[] PSCollapsed = new[] { 0, 0, 4, 4, 0, 8 };
        protected int[] PSExpanded = new[] { -2, 7, 4, 7, 4, 1 };
        protected int[] PSFileIcon1 = new[] { 0, 0, 8, 0, 11, 3, 11, 13, 0, 13 };
        protected int[] PSFileIcon2 = new[] { 1, 1, 7, 1, 10, 4, 10, 12, 1, 12 };
        protected int[] PSFileIcon3 = new[] { 7, 1, 7, 4, 10, 4 };

        protected static Point[] Poly(Point offset, int[] coords) {
            if (coords.Length % 2 > 0) throw new ArgumentException();
            var n = coords.Length / 2;
            var points = new Point[n];
            for (int i = 0; i < n; i++) points[i] = new Point(coords[2 * i] + offset.X, coords[2 * i + 1] + offset.Y);
            return points;
        }

        protected void DrawCollapsed(Graphics gr, Point position, bool solid = false) {
            var poly = Poly(position, PSCollapsed);
            using (var pen = new Pen(ForeColor)) gr.DrawPolygon(pen, poly);
            if (solid) using (var brush = new SolidBrush(ForeColor)) gr.FillPolygon(brush, poly);
        }

        protected void DrawExpanded(Graphics gr, Point position, bool solid = false) {
            var poly = Poly(position, PSExpanded);
            using (var pen = new Pen(ForeColor)) gr.DrawPolygon(pen, poly);
            if (solid) using (var brush = new SolidBrush(ForeColor)) gr.FillPolygon(brush, poly);
        }

        protected void DrawFileIcon(Graphics gr, Point position) {
            var shape = new[] { Poly(position, PSFileIcon1), Poly(position, PSFileIcon2), Poly(position, PSFileIcon3) };
            using (var pen = new Pen(BackColor)) gr.DrawPolygon(pen, shape[0]);
            using (var brush = new SolidBrush(BackColor)) gr.FillPolygon(brush, shape[0]);
            using (var pen = new Pen(FileIconColor)) gr.DrawPolygon(pen, shape[1]);
            using (var pen = new Pen(FileIconColor)) gr.DrawPolygon(pen, shape[2]);
            using (var brush = new SolidBrush(FileIconColor)) gr.FillPolygon(brush, shape[2]);
        }

        #endregion

        #region Initialization and drawing

        public ProjectTree() {
            DrawMode = TreeViewDrawMode.OwnerDrawAll;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            ForeColor = SystemColors.WindowText;
            BackColor = SystemColors.Window;
            FileIconColor = SystemColors.Highlight;
            SelectionBackColor = ColorTranslator.FromHtml("#007acc"); // VS2012 selection
            ImageList = new ImageList();
            ImageList.Images.Add(new Bitmap(16, 16));
        }

        protected override void OnParentChanged(EventArgs e) {
            base.OnParentChanged(e);
            Font = Parent.Font;
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e) {
            e.DrawDefault = false;
            var g = e.Graphics;
            var n = e.Node;
            var f = e.State.HasFlag(TreeNodeStates.Focused);
            var b = n.Bounds; //  original bounds
            var l = b.Location; // original location
            var h = b.Height;
            using (var bg = new SolidBrush(e.State.HasFlag(TreeNodeStates.Focused) ? SelectionBackColor : BackColor)) g.FillRectangle(bg, e.Bounds);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            l.Offset(-32, (h - 8) / 2 - 1);
            if (n.IsExpanded) DrawExpanded(g, l, f); else DrawCollapsed(g, l, f);
            l.Offset(16, -2);
            DrawFileIcon(g, l);
            l = b.Location;
            l.Offset(0, (h - TextRenderer.MeasureText("Mg", Font).Height) / 2);
            using (var brush = new SolidBrush(ForeColor)) g.DrawString(n.Text, Font, brush, l);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Opens the project
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNodeMouseDoubleClick(TreeNodeMouseClickEventArgs e) { (e.Node.Tag as ProjectFile).Open(); }

        /// <summary>
        /// Disables expand / collapse on double click
        /// </summary>
        /// <param name="m"></param>
        protected override void DefWndProc(ref Message m) { if (m.Msg != 515) base.DefWndProc(ref m); }

        #endregion

    }

}