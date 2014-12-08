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

        internal readonly Properties.Settings Settings = Properties.Settings.Default;
        internal string FileToOpen;
        internal ProjectPanel SceneryPanel;
        internal SearchResultsPanel SearchResultsPanel;

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

        private void DisableEdit(bool useWaitCursor = false) {
            DockPanel.Enabled = EditMenu.Enabled = ViewMenu.Enabled = SceneryMenu.Enabled = DebugMenu.Enabled = false;
            if (useWaitCursor) Cursor = Cursors.WaitCursor;
            Application.DoEvents();
        }

        private void EnableEdit() {
            DockPanel.Enabled = EditMenu.Enabled = ViewMenu.Enabled = SceneryMenu.Enabled = DebugMenu.Enabled = true;
            Cursor = Cursors.Default;
            Application.DoEvents();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog.ShowDialog();
        }

        private void OpenFile_FileOk(object sender, CancelEventArgs e) {
            new EditorFile(OpenFileDialog.FileName);
            if (EditorFile.All.Count > 0) { EnableEdit(); Status.Visible = true; }
        }

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

        private void RunMenuItem_Click(object sender, EventArgs e) {
            EditorFile.Run();
        }

        private void SaveMenuItem_Click(object sender, EventArgs e) {
            EditorFile.SaveAll();
        }

        private void RefactorMenuItem_Click(object sender, EventArgs e) {
            if (CurrentFile != null &&
                (CurrentFile.Type == EditorFile.Types.SceneryMain || CurrentFile.Type == EditorFile.Types.SceneryPart)) new Refactor(this);
        }

        public void StatusUpdate_Loading(object sender, EventArgs e) {
            StatusLabel.Text = String.Format(Messages.LoadingIncludes, (sender as EditorFile).FileName);
        }

        private void StatusUpdate_Ready(object sender, EventArgs e) {
            EnableEdit();
            StatusLabel.Text = Messages.Ready;
        }

        private void SceneryNormalizeMenuItem_Click(object sender, EventArgs e) {
            DisableEdit(true);
            Status.Text = Messages.NormalizeInProgress;
            // TODO: Global normalization
            Status.Text = Messages.Ready;
            EnableEdit();
        }

        private void WordWrapMenuItem_Click(object sender, EventArgs e) {
            (sender as ToolStripMenuItem).Checked = CurrentEditor.WordWrap = !CurrentEditor.WordWrap;
            Properties.Settings.Default.WordWrap = CurrentEditor.WordWrap;
            Properties.Settings.Default.Save();
        }
        private void SaveAllMenuItem_Click(object sender, EventArgs e) {
            EditorFile.All.ForEach(i => i.Save());
        }

        private void ExitMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void SceneryUndoMenuItem_Click(object sender, EventArgs e) {
            EditorFile.All.ForEach(i => { while (i.Editor.UndoEnabled) i.Editor.Undo(); });
        }

        private void SceneryRedoMenuItem_Click(object sender, EventArgs e) {
            EditorFile.All.ForEach(i => { while (i.Editor.RedoEnabled) i.Editor.Redo(); });
        }

        private void SceneryReloadMenuItem_Click(object sender, EventArgs e) {
            var mainFile = EditorFile.All.First(i => i.Role == EditorFile.Roles.Main);
            if (mainFile != null) {
                var mainFilePath = mainFile.Path;
                EditorFile.Reset();
                new EditorFile(mainFilePath, EditorFile.Roles.Main);
            }
        }

        private void ResetMenuItem_Click(object sender, EventArgs e) {
            EditorFile.Reset();
            Status.Visible = false;
            DisableEdit();
        }

        private void CommentSelectedMenuItem_Click(object sender, EventArgs e) {
            CurrentEditor.ToggleComment();
        }

        private void FindReferencesMenuItem_Click(object sender, EventArgs e) {
            if (CurrentEditor != null) {
                var symbol = CurrentEditor.SelectWord();
                var pattern = @"(?<=^|[ :;\r\n]+)" + Regex.Escape(symbol) + "(?=[ ;\r\n]+|$)";
                var regex = new Regex(pattern, RegexOptions.Compiled);
                Place p;
                SearchResultsPanel.Reset();
                ProjectFile.All.ForEach(i => {
                    foreach (Match m in regex.Matches(i.Text)) {
                        p = i.Editor.PositionToPlace(m.Index);
                        SearchResultsPanel.Add(new SearchResult {
                            Path = i.Path, File = i.FileName, Fragment = m.Value, Line = p.iLine + 1, Column = p.iChar + 1
                        });
                    }
                });
                SearchResultsPanel.CloseIfEmpty();
            }
        }

    }


}
