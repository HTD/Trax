using FastColoredTextBoxNS;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ScnEdit {
    
    class Editor : FastColoredTextBox {

        public override string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
                OnSyntaxHighlight(new TextChangedEventArgs(VisibleRange));
            }
        }

        #region Fields

        internal EditorFile File;
        internal Encoding Encoding;
        internal EditorSyntax SyntaxModule;
        internal bool FileBindingMode;
        internal bool FullAsyncMode;

        #endregion

        #region Events

        public event EventHandler FileIODone;

        #endregion

        #region Methods

        public Editor(EditorFile file) {
            MaxHistoryLength = 2048;
            DoubleBuffered = true;
            File = file;
            Dock = DockStyle.Fill;
            WordWrapMode = WordWrapMode.WordWrapControlWidth;
            HighlightingRangeType = HighlightingRangeType.VisibleRange;
            switch (File.Type) {
                case ScnEdit.EditorFile.Types.HTML: Language = Language.HTML; break;
                default: Language = Language.Custom; break;
            }
            Encoding = Encoding.GetEncoding(Properties.Settings.Default.EncodingDefault);
            WordWrap = Properties.Settings.Default.WordWrap;
            if (File.Type == EditorFile.Types.Timetable) IsReplaceMode = true;
            FullAsyncMode = true;
            SyntaxModule = new EditorSyntax(this);
            if (File.Type == EditorFile.Types.SceneryMain || File.Type == EditorFile.Types.SceneryPart) ToolTipNeeded += SyntaxModule.HintParser;
            SelectionChangedDelayed += SyntaxModule.SameWordHighlight;
            IsReplaceModeChanged += Editor_IsReplaceModeChanged;
        }

        public void ReloadScheme() {
            SyntaxModule = null;
            SyntaxModule = new EditorSyntax(this);
            SyntaxModule.GetStyles(true);
            SyntaxModule.HighlightSyntax(VisibleRange);
            File.Container.DocumentMap.BackColor = EditorSyntax.Styles.Background;
        }
        public void LoadFromFile() {
            Encoding = File.EncodingDefault;
            FileBindingMode = !File.AutoDecoding || File.HasHtmlEncoding;
            if (File.TextCache != null) Text = File.TextCache;
            else {
                if (FileBindingMode) OpenBindingFile(File.Path, Encoding);
                else Text = (File as ProjectFile).Text;
            }
            EndUpdate();
            IsChanged = File.IsConverted || File.IsNormalized || File.IsChanged;
            if (File.Container != null) File.Container.UpdateText();
            if (!FileBindingMode) OnSyntaxHighlight(new TextChangedEventArgs(VisibleRange));
            ClearUndo();
            if (File.Role == EditorFile.Roles.Log) {
                ReadOnly = true;
                TextChangedDelayed += ShowLogTail;
            }
            if (File.Container != null) File.Container.UpdateText();
            if (FileIODone != null) FileIODone.Invoke(this, EventArgs.Empty);
        }

        public void SaveToFile() {
            base.SaveToFile(File.Path, Encoding);
            IsChanged = false;
            File.Container.UpdateText(true);
            if (FileIODone != null) FileIODone.Invoke(this, EventArgs.Empty);
        }

        public void CommentSelectedLines() {
            Selection = GetLinesFromSelection();
            SelectedText = new ScnSyntax.NonCommentLine().Replace(SelectedText, "$1//$2");
        }

        public void UncommentSelectedLines() {
            Selection = GetLinesFromSelection();
            SelectedText = new ScnSyntax.CommentLine().Replace(SelectedText, "$1$2");
        }

        public void ToggleComment() {
            if (Language == FastColoredTextBoxNS.Language.Custom) {
                Selection = GetLinesFromSelection();
                if (SelectedText.Trim().StartsWith("//")) UncommentSelectedLines(); else CommentSelectedLines();
            } else CommentSelected();
            OnSyntaxHighlight(new TextChangedEventArgs(Selection));
        }

        public bool SelectionHasStyle(Enum s) {
            if (Selection.IsEmpty) return false;
            return Selection.Chars.First().style.HasFlag(s);
        }

        private Place Mark(int start, int length, StyleIndex style) {
            var p = PositionToPlace(start);
            var q = PositionToPlace(start + length);
            Selection = new Range(this, p, p);
            DoSelectionVisible();
            var target = new Range(this, p, q);
            var timer = new Timer { Interval = 2000 };
            target.SetStyle(style);
            return p;
        }

        private Place Mark(int column, int line, int length, StyleIndex style) {
            var position = PlaceToPosition(new Place(column, line));
            return Mark(position, length, style);
        }

        public Place MarkSearchResult(int column, int line, int length) {
            return Mark(column, line, length, EditorSyntax.StyleMap.SearchResult);
        }

        public Place MarkReplaceResult(int column, int line, int length) {
            return Mark(column, line, length, EditorSyntax.StyleMap.ReplaceResult);
        }

        public void ClearMarkers() {
            ClearStyle(EditorSyntax.StyleMap.SearchResult);
            ClearStyle(EditorSyntax.StyleMap.ReplaceResult);
        }

        #endregion

        #region Helpers

        private static bool IsSpecialKey(Keys k) {
            switch (k) {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:
                case Keys.ShiftKey:
                case Keys.Insert:
                case Keys.Delete:
                    return true;
                default:
                    return false;
            }
        }

        private void ShowLogTail(object sender, EventArgs e) {
            GoEnd();
            TextChangedDelayed -= ShowLogTail;
        }

        private Range GetLinesFromSelection() {
            var range = new Range(this, Selection.Start, Selection.End);
            range.Normalize();
            var startOffset = PlaceToPosition(range.Start);
            var endOffset = PlaceToPosition(range.End);
            for (int i = startOffset; i >= 0; i--) if (i == 0 || Text[i - 1] == '\r' || Text[i - 1] == '\n') { startOffset = i; break; }
            if (endOffset > startOffset) for (int i = endOffset - 1; i < Text.Length; i++) if (Text[i] == '\r' || Text[i] == '\n') { endOffset = i; break; }
            return new Range(this, PositionToPlace(endOffset), PositionToPlace(startOffset));
        }

        #endregion

        #region Event handlers

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape) ClearMarkers();
            if (IsReplaceMode) { // usable replace mode
                if (e.Modifiers == Keys.None || e.Modifiers == Keys.Shift) {
                    var s = Selection;
                    var p = s.Start;
                    var q = s.End;
                    switch (e.KeyCode) {
                        case Keys.Delete:
                            BeginAutoUndo();
                            s.End = new Place(p.iChar + 1, p.iLine);
                            ClearSelected();
                            InsertText(" ");
                            e.Handled = true;
                            EndAutoUndo();
                            break;
                        case Keys.Back:
                            BeginAutoUndo();
                            s.End = new Place(p.iChar - 1, p.iLine);
                            ClearSelected();
                            InsertText(" ");
                            s.Start = new Place(p.iChar - 1, p.iLine);
                            e.Handled = true;
                            EndAutoUndo();
                            break;
                        default:
                            if (!IsSpecialKey(e.KeyCode) && p != q) {
                                BeginAutoUndo();
                                var n = Selection.Clone(); n.Normalize();
                                IsReplaceMode = true;
                                InsertText("".PadLeft(n.End.iChar - n.Start.iChar, ' '));
                                s.Start = n.Start; s.End = n.Start;
                                EndAutoUndo();
                            };
                            break;
                    }
                }
            }
        }

        private void Editor_IsReplaceModeChanged(object sender, EventArgs e) {
            Status.Replace = IsReplaceMode;
        }

        public override void OnSelectionChangedDelayed() {
            base.OnSelectionChangedDelayed();
            var w = new BackgroundWorker();
            w.DoWork += BackgroundStatusUpdate;
            w.RunWorkerCompleted += BackgroundStatusUpdateDone;
            w.RunWorkerAsync();
        }

        void BackgroundStatusUpdate(object sender, DoWorkEventArgs e) {
            if (InvokeRequired) BeginInvoke(new MethodInvoker(() => { BackgroundStatusUpdate(sender, e); }));
            else {
                Status.Line = Selection.Start.iLine + 1;
                Status.Column = Selection.Start.iChar + 1;
                Status.Selection = Selection.Text.Length;
            }
        }

        void BackgroundStatusUpdateDone(object sender, RunWorkerCompletedEventArgs e) {
            (sender as BackgroundWorker).Dispose();
        }

        public override void OnVisibleRangeChangedDelayed() {
            base.OnVisibleRangeChangedDelayed();
            var w = new BackgroundWorker();
            w.DoWork += BackgroundStatusUpdate1;
            w.RunWorkerCompleted += BackgroundStatusUpdate1Done;
            w.RunWorkerAsync();
        }

        void BackgroundStatusUpdate1(object sender, DoWorkEventArgs e) {
            if (InvokeRequired) BeginInvoke(new MethodInvoker(() => { BackgroundStatusUpdate1(sender, e); }));
            else {
                Status.FileName = File.FileName;
                Status.FileSize = Text.Length;
                Status.FileLines = Range.End.iLine + 1;
            }
        }

        void BackgroundStatusUpdate1Done(object sender, RunWorkerCompletedEventArgs e) {
            (sender as BackgroundWorker).Dispose();
        }

        public override void OnTextChangedDelayed(Range changedRange) {
            base.OnSelectionChangedDelayed();
            File.Container.UpdateText();
            if (File.Role == EditorFile.Roles.Log) {
                Selection.Start = new Place(0, Range.End.iLine);
                DoCaretVisible();
            }
        }

        protected override void OnPaint(PaintEventArgs e) { try { base.OnPaint(e); } catch (ArgumentException) { Zoom = Zoom; } }

        public override void Paste() {
            base.Paste();
            OnSyntaxHighlight(new TextChangedEventArgs(Selection));
        }

        public override void Undo() {
            base.Undo();
            Selection = new Range(this);
            OnSyntaxHighlight(new TextChangedEventArgs(VisibleRange));
        }

        public override void Redo() {
            base.Redo();
            Selection = new Range(this);
            OnSyntaxHighlight(new TextChangedEventArgs(VisibleRange));
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (ModifierKeys == Keys.Control) {
                var p = PointToPlace(e.Location);
                var q = new Place(p.iChar + 1, p.iLine);
                var r = new Range(this, p, q);
                var s = r.Chars.FirstOrDefault().style;
                Cursor = s.HasFlag(EditorSyntax.StyleMap.Path) ? Cursors.Hand : Cursors.IBeam;
            } else Cursor = Cursors.IBeam;
        }

        protected override void OnClick(EventArgs e) {
            if (Enabled) EditorSyntax.FullAsyncMode = true;
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control) {
                var p = PointToPlace(e.Location);
                var b = Selection.Clone();
                CaretVisible = false;
                Selection = new Range(this, p, p);
                var w = SelectWord();
                var r = Selection.Clone();
                var s = !r.IsEmpty ? Selection.Chars.First().style : StyleIndex.None;
                Selection = b;
                CaretVisible = true;
                if (s.HasFlag(EditorSyntax.StyleMap.Path)) EditorFile.TryOpen(File, w);
            } else base.OnMouseDown(e);
        }

        #endregion

    }

    class TargetStyle : Style {

        internal Color BackColor { get; set; }
        internal Color FrameColor { get; set; }

        internal TargetStyle(Color backColor, Color frameColor) {
            BackColor = backColor;
            FrameColor = frameColor;
        }

        public override void Draw(System.Drawing.Graphics gr, System.Drawing.Point position, Range range) {
            var selection = new Rectangle(position, GetSizeOfRange(range));
            var frame = selection;
            var pen = new Pen(FrameColor) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
            var brush = new SolidBrush(BackColor);
            frame.Inflate(5, 2);
            gr.DrawRectangle(pen, frame);
            gr.FillRectangle(brush, frame);
        }

    }

}