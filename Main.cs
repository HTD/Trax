using FastColoredTextBoxNS;
using Lextm.SharpSnmpLib;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ScnEdit {
    
    public partial class Main : Form {

        #region Fields

        internal static Main Instance;

        internal readonly Properties.Settings Settings = Properties.Settings.Default;
        internal string FileToOpen;
        internal ProjectPanel SceneryPanel;
        internal SearchResultsPanel SearchResultsPanel;

        #endregion

        #region Properties

        internal EditorFile CurrentFile {
            get {
                if (DockPanel.Contents.Count < 1) return null;
                if (DockPanel.ActiveContent == null) return null;
                if (!(DockPanel.ActiveContent is EditorContainer)) return null;
                return (DockPanel.ActiveContent as EditorContainer).Editor.File;
            }
        }

        internal Editor CurrentEditor {
            get {
                var cf = CurrentFile;
                return cf != null ? CurrentFile.Editor : null;
            }
        }

        #endregion

        #region Core

        public Main(string[] args) {
            InitializeComponent();
            SuspendLayout();
            (new VS2012ToolStripExtender { VS2012Renderer = new VS2012ToolStripRenderer() }).SetEnableVS2012Style(MainMenu, true);
            Status.Main = this;
            EditorFile.Main = this;
            ProjectPanel.Main = this;
            SearchResultsPanel.Main = this;
            StatusLabel.Text = Messages.Ready;
            if (args.Length > 0) FileToOpen = args[0];
            Instance = this;
            ResumeLayout();
        }

        private void Main_Load(object sender, EventArgs e) {
            Status.Visible = false;
            if (String.IsNullOrWhiteSpace(FileToOpen) && !String.IsNullOrWhiteSpace(Settings.LastOpenFile)) FileToOpen = Settings.LastOpenFile;
            if (FileToOpen != null && System.IO.File.Exists(FileToOpen)) {
                new EditorFile(FileToOpen);
                if (EditorFile.All.Count > 0) EnableEdit();
                Status.Visible = true;
            }
            foreach (TypeInfo t in EditorSyntax.Styles.ConfiguredColorSchemes) {
                var i = ColorSchemeMenu.DropDownItems.Add(t.Name.Replace("Colors_", "").Replace("_", " "), null, Main_SetColorScheme) as ToolStripMenuItem;
                if (t.Name == EditorSyntax.Styles.ColorScheme.GetType().Name) i.Checked = true;
            }
        }

        void Main_SetColorScheme(object sender, EventArgs e) {
            var item = sender as ToolStripMenuItem;
            var schemeName = "";
            if (item.Checked) return;
            EditorSyntax.Styles.SetColorScheme(schemeName = item.Text.Replace(" ", "_"));
            foreach (ToolStripMenuItem i in ColorSchemeMenu.DropDownItems) i.Checked = false;
            DockPanel.BackColor = DockPanel.DockBackColor = EditorSyntax.Styles.ColorScheme.Background;
            EditorFile.All.ForEach(i => { i.Editor.ReloadScheme(); });
            if (SceneryPanel != null) SceneryPanel.ReloadScheme();
            if (SearchResultsPanel != null) SearchResultsPanel.ReloadScheme();
            item.Checked = true;
            Properties.Settings.Default.ColorScheme = schemeName;
            Properties.Settings.Default.Save();
        }

        private void MainDockPanel_ActiveDocumentChanged(object sender, EventArgs e) {
            if (CurrentFile != null) {
                Status.FileName = CurrentFile.FileName;
                Status.Line = CurrentEditor.Selection.Start.iLine + 1;
                Status.Column = CurrentEditor.Selection.Start.iChar + 1;
                Status.Selection = CurrentEditor.Selection.Text.Length;
                Status.FileSize = CurrentEditor.Text.Length;
                Status.FileLines = CurrentEditor.Range.End.iLine + 1;
                Status.FileType = CurrentFile.Type;
                Status.Replace = CurrentEditor.IsReplaceMode;
            }
        }

        private void DockPanel_ActiveDocumentChanged(object sender, EventArgs e) {
            var ed = CurrentEditor;
            if (ed != null) ed.OnVisibleRangeChangedDelayed();
        }

        private void OpenFile_FileOk(object sender, CancelEventArgs e) {
            new EditorFile(OpenFileDialog.FileName);
            if (EditorFile.All.Count > 0) { EnableEdit(); Status.Visible = true; }
        }

        public void DisableEdit(bool useWaitCursor = false) {
            EditorSyntax.FullAsyncMode = false;
            DockPanel.Enabled = EditMenu.Enabled = ViewMenu.Enabled = SceneryMenu.Enabled = DebugMenu.Enabled = false;
            if (useWaitCursor) Cursor = Cursors.WaitCursor;
            Application.DoEvents();
        }

        public void EnableEdit() {
            DockPanel.Enabled = EditMenu.Enabled = ViewMenu.Enabled = SceneryMenu.Enabled = DebugMenu.Enabled = true;
            Cursor = Cursors.Default;
            Application.DoEvents();
            EditorSyntax.FullAsyncMode = true;
        }

        #endregion

        #region Menu

        #region File

        private void OpenMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog.ShowDialog();
        }

        private void SaveMenuItem_Click(object sender, EventArgs e) {
            EditorFile.SaveAll();
        }

        private void SaveAllMenuItem_Click(object sender, EventArgs e) {
            EditorFile.All.ForEach(i => i.Save());
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        #endregion

        #region Edit

        /// <summary>
        /// Script text normalization:
        /// Removes excessive whitespace, indentation, normalizes whitespace characters, removes explicit texture file extensions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NormalizeMenuItem_Click(object sender, EventArgs e) {
            DisableEdit(true);
            CurrentFile.Normalize();
            EnableEdit();
        }

        private void CommentSelectedMenuItem_Click(object sender, EventArgs e) {
            CurrentEditor.ToggleComment();
        }

        #endregion

        #region View

        private void WordWrapMenuItem_Click(object sender, EventArgs e) {
            (sender as ToolStripMenuItem).Checked = CurrentEditor.WordWrap = !CurrentEditor.WordWrap;
            Properties.Settings.Default.WordWrap = CurrentEditor.WordWrap;
            Properties.Settings.Default.Save();
        }

        private void ClearMarkersMenuItem_Click(object sender, EventArgs e) {
            CurrentEditor.ClearMarkers();
        }

        #endregion

        #region Scenery

        private void FindReferencesMenuItem_Click(object sender, EventArgs e) {
            var ed = CurrentEditor;
            if (ed != null) ProjectFile.FindSymbol(ed.SelectWord());
        }

        private void RefactorMenuItem_Click(object sender, EventArgs e) {
            if (CurrentFile != null &&
                (CurrentFile.Type == EditorFile.Types.SceneryMain || CurrentFile.Type == EditorFile.Types.SceneryPart)) new Refactor(this);
        }

        private void SceneryUndoMenuItem_Click(object sender, EventArgs e) {
            EditorFile.All.ForEach(i => { while (i.Editor.UndoEnabled) i.Editor.Undo(); });
        }

        private void SceneryRedoMenuItem_Click(object sender, EventArgs e) {
            EditorFile.All.ForEach(i => { while (i.Editor.RedoEnabled) i.Editor.Redo(); });
        }

        private void SceneryNormalizeMenuItem_Click(object sender, EventArgs e) {
            DisableEdit(true);
            Status.Text = Messages.NormalizeInProgress;
            Application.DoEvents();
            for (int i = 0, n = ProjectFile.All.Count; i < n; i++) {
                var f = ProjectFile.All[i];
                var t = f.Normalize();
                if (f.IsNormalized || t.Length != f.TextCache.Length) {
                    f.TextCache = t;
                    if (f.IsOpen) f.Editor.Text = t;
                    else { f.IsChanged = true; f.Open(); }
                }
            }
            Status.Text = Messages.Ready;
            EnableEdit();
        }

        private void NameTracksMenuItem_Click(object sender, EventArgs e) {
            var ed = CurrentEditor;
            if (ed != null && !String.IsNullOrWhiteSpace(ed.Text)) {
                Status.Text = Messages.ProcessingTracks;
                Application.DoEvents();
                var tracks = ScnTracks.Parse(ed.Text, ed.File.Path);
                if (tracks.Count > 0) {
                    tracks.SortAddNames();
                    ed.BeginAutoUndo();
                    ed.Text = tracks.ReplaceText();
                    ed.EndAutoUndo();
                }
                Status.Text = Messages.Ready;
            }
        }

        private void SceneryReloadMenuItem_Click(object sender, EventArgs e) {
            var mainFile = EditorFile.All.First(i => i.Role == EditorFile.Roles.Main);
            if (mainFile != null) {
                var mainFilePath = mainFile.Path;
                EditorFile.Reset();
                new EditorFile(mainFilePath, EditorFile.Roles.Main);
            }
        }

        #endregion

        #region Debug

        private void RunMenuItem_Click(object sender, EventArgs e) {
            EditorFile.Run();
        }

        #endregion

        #endregion

    }

}