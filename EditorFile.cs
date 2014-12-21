using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ScnEdit {
    
    internal class EditorFile : ProjectFile {

        #region Static part

        #region Fields

        internal static Main Main;
        internal static new List<EditorFile> All = new List<EditorFile>();

        #endregion

        #region Events

        public static event EventHandler BatchStart;
        public static event EventHandler BatchEnd;
        public static event EventHandler ReadingFile;
        public static event EventHandler ReadingFileDone;
        public static event EventHandler WritingFile;
        public static event EventHandler WritingFileDone;
        public static event EventHandler ProcessingFile;
        public static event EventHandler ProcessingFileDone;

        #endregion

        #region Methods

        public static void Reset() {
            All.ForEach(i => {
                i.Editor.Dispose();
                i.Container.DocumentMap.Dispose();
                i.Container.Splitter.Dispose();
                i.Container.Dispose();
            });
            All.Clear();
            var s = Properties.Settings.Default;
            s.LastOpenFile = null;
            s.Save();
        }

        /// <summary>
        /// Tries to open a relative filename within the current project
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool TryOpen(ProjectFile context, string name) {
            string b = context.BaseDirectory, s = context.SceneryDirectory, n = name, l = "\\", i = ".inc", t = ".txt";
            var searchPath = new string[] {
                    b + l + n,
                    b + l + n + i,
                    b + l + n + t,
                    s + l + n,
                    s + l + n + i,
                    s + l + n + t,
                };
            foreach (var path in searchPath) {
                if (System.IO.File.Exists(path)) {
                    new EditorFile(path);
                    return true;
                }
            }
            return false;
        }

        public static void SaveAll() {
            if (BatchStart != null) BatchStart.Invoke(All, EventArgs.Empty);
            All.ForEach(m => m.Save());
            if (BatchEnd != null) BatchEnd.Invoke(All, EventArgs.Empty);
        }

        public static bool ProceedWithUnsavedChanges() {
            if (All.Count < 1) return true;
            foreach (EditorFile i in All) if (!i.Container.ProceedWithUnsavedChanges()) return false;
            return true;
        }

        public static void Run(bool debug = true, bool andExit = false) {
            if (All.Count < 1) return;
            var main = All.First(i => i.Type == Types.SceneryMain);
            if (main != null && !String.IsNullOrWhiteSpace(main.BaseDirectory) && ProceedWithUnsavedChanges()) {
                var simExe = String.Format("{0}\\{1}", main.BaseDirectory, System.IO.Path.GetFileName(Properties.Settings.Default.SimExe));
                if (System.IO.File.Exists(simExe)) {
                    var m = (new ScnSyntax.FirstActiveDynamic()).Match(main.Editor.Text);
                    if (m.Success) {
                        System.IO.Directory.SetCurrentDirectory(main.BaseDirectory);
                        var i = new ProcessStartInfo();
                        i.FileName = simExe;
                        i.Arguments = String.Format("-s {0} -v {1}", main.FileName, m.Value);
                        i.WindowStyle = ProcessWindowStyle.Minimized;
                        var p = Process.Start(i);
                        if (debug && !andExit) {
                            p.WaitForExit();
                            var logPath = String.Format("{0}\\{1}", main.BaseDirectory, "log.txt");
                            if (System.IO.File.Exists(logPath)) new EditorFile(logPath, Roles.Log);
                        }
                        if (andExit) Application.Exit();
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Fields

        internal new Editor Editor;
        internal EditorContainer Container;
        

        #endregion

        #region Properties

        internal override string Text { get { return Editor != null && !String.IsNullOrEmpty(Editor.Text) ? Editor.Text : base.Text; } }



        #endregion

        #region Constructors

        public EditorFile(string path, Roles role = Roles.Any) : base(path, role) {
            Open(DockState.Document);
        }

        public EditorFile(ProjectFile file) : base(file.Path, file.Role) {
            if (file.TextCache != null) TextCache = file.TextCache;
            IsChanged = file.IsChanged;
            Open(DockState.Document);
            var projectIndex = ProjectFile.All.FindIndex(i => i.Path == Path);
            if (projectIndex >= 0) ProjectFile.All[projectIndex] = this;

        }

        #endregion

        #region Internal methods

        internal new void Normalize() {
            if (Type == Types.SceneryMain || Type == Types.SceneryPart) {
                if (ProcessingFile != null) ProcessingFile.Invoke(this, EventArgs.Empty);
                Application.DoEvents();
                Editor.BeginAutoUndo();
                Editor.Text = base.Normalize();
                Editor.OnSyntaxHighlight(new TextChangedEventArgs(Editor.VisibleRange));
                if (ProcessingFileDone != null) ProcessingFileDone.Invoke(this, EventArgs.Empty);
                Application.DoEvents();
            }
        }

        internal void Save() {
            if (Editor.IsChanged) {
                if (WritingFile != null) WritingFile.Invoke(this, EventArgs.Empty);
                Application.DoEvents();
                Editor.SaveToFile();
                if (WritingFileDone != null) WritingFileDone.Invoke(this, EventArgs.Empty);
                Application.DoEvents();
            }
        }

        #endregion

        #region Private methods

        private void Open(DockState dockState = DockState.Document) {
            var existing = All.FirstOrDefault(i => i.Path == this.Path);
            if (existing != null) {
                existing.Container.Activate();
                return;
            }
            Editor = new Editor(this) { Font = Properties.Settings.Default.Font };
            if (ReadingFile != null) ReadingFile.Invoke(this, EventArgs.Empty);
            Application.DoEvents();
            if (Role == Roles.Main) {
                if (Main.SceneryPanel != null) Main.SceneryPanel.Dispose();
                Reset();
                Main.SceneryPanel = new ProjectPanel(this);
            }
            Editor.LoadFromFile();
            (Main.WordWrapMenuItem as ToolStripMenuItem).Checked = Editor.WordWrap;
            Container = new EditorContainer(Editor, Main.DockPanel, dockState);
            Container.UpdateText();
            if (ReadingFileDone != null) ReadingFileDone.Invoke(this, EventArgs.Empty);
            Application.DoEvents();
            if (All.Count < 1) {
                var s = Properties.Settings.Default;
                s.LastOpenFile = Path;
                s.Save();
            }
            All.Add(this);
            Status.FileName = FileName;
        }

        



        #endregion
    
    }

}