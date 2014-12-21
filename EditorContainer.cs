using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ScnEdit {

    class EditorContainer : DockContent {

        public Editor Editor;
        public Splitter Splitter;
        public DocumentMap DocumentMap;
        int DocumentMapMinimumWidth = 50;
        int DocumentMapInitialWidth = 100;
        float DocumentMapScaleFactor = 0.0015f;
        const string ChangedIndicator = "*";

        //private DockPanel _DockPanel;
        //private DockState _DockState;

        public EditorContainer(Editor editor, DockPanel dock, DockState dockState = DockState.Document) {
            Editor = editor;
            Editor.UndoRedoStateChanged += Editor_UndoRedoStateChanged;
            Editor.TextChangedDelayed += Editor_TextChangedDelayed;
            Splitter = new Splitter() { Dock = DockStyle.Right, BackColor = SystemColors.ControlDarkDark, Width = 4 };
            DocumentMap = new DocumentMap() {
                Target = editor,
                Dock = DockStyle.Right,
                Width = DocumentMapInitialWidth,
                MinimumSize = new Size(DocumentMapMinimumWidth, 0),
                Scale = DocumentMapInitialWidth * DocumentMapScaleFactor,
                BackColor = EditorSyntax.Styles.Background,
                ForeColor = Color.FromArgb(0, 122, 204)
            };
            DocumentMap.DoubleClick += DocumentMap_DoubleClick;
            DocumentMap.MouseWheel += DocumentMap_MouseWheel;
            Splitter.SplitterMoved += Splitter_SplitterMoved;
            Name = Editor.File.FileName;
            ToolTipText = Editor.File.Path;
            Controls.Add(Editor);
            Controls.Add(Splitter);
            Controls.Add(DocumentMap);
            UpdateText(true);
            FormClosing += EditorContainer_FormClosing;
            FormClosed += EditorContainer_FormClosed;
            System.Threading.Thread.Sleep(10);
            dock.Invoke(new Action(() => { Show(dock, dockState); }));
        }

        void DocumentMap_MouseWheel(object sender, MouseEventArgs e) {
            DocumentMap.Scale += e.Delta * 0.1f;
        }

        void DocumentMap_DoubleClick(object sender, EventArgs e) {
            DocumentMap.Width = DocumentMapInitialWidth;
            DocumentMap.Scale = DocumentMap.Width * DocumentMapScaleFactor;
        }

        void Splitter_SplitterMoved(object sender, SplitterEventArgs e) {
            DocumentMap.Scale = DocumentMap.Width * DocumentMapScaleFactor;
        }

        public bool ProceedWithUnsavedChanges() {
            if (Editor.IsChanged) {
                var r = System.Windows.Forms.MessageBox.Show(
                    String.Format(Messages.UnsavedChanges, Editor.File.FileName), "",
                    System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Exclamation
                );
                if (r == System.Windows.Forms.DialogResult.Yes) {
                    Editor.SaveToFile();
                    return true;
                }
                if (r == System.Windows.Forms.DialogResult.Cancel) return false;
            } return true;
        }
        
        void Editor_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e) {
            if (Editor.IsChanged) UpdateText(true);
        }

        void Editor_UndoRedoStateChanged(object sender, EventArgs e) {
            if (!Editor.UndoEnabled) { Editor.IsChanged = false; UpdateText(); }
        }

        void EditorContainer_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e) {
            EditorFile.All.Remove(Editor.File);
            Editor.Dispose();
        }

        void EditorContainer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e) {
            e.Cancel = !ProceedWithUnsavedChanges();
        }

        public void UpdateText(bool force = false) {
            if (IsHandleCreated)
                Invoke(new Action(() => {
                    if (force || Editor.IsChanged) Text = Editor.IsChanged ? (Name + ChangedIndicator) : Name;
                    else Text = Name;
                }));
        }

    }

}