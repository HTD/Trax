using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ScnEdit {

    internal class EditorSyntax : ICustomHighlighter {

        internal static class Styles {
            
            public static Color Background;
            public static Color Carret;
            public static Color LineNumber;
            public static Color ServiceLine;
            public static Color Selection;
            public static TextStyle Default;
            public static TextStyle Comment;
            public static TextStyle Keyword0;
            public static TextStyle Keyword1;
            public static TextStyle Keyword2;
            public static TextStyle Keyword3;
            public static TextStyle Keyword4;
            public static TextStyle Number;
            public static TextStyle Time;
            public static TextStyle Special;
            public static TextStyle Command;
            public static TextStyle Path;
            public static TextStyle SameWord;
            public static TargetStyle SearchResult;
            public static TargetStyle ReplaceResult;

            internal static IEnumerable<TypeInfo> ConfiguredColorSchemes {
                get {
                    var csType = typeof(IColorScheme);
                    return csType.Assembly.DefinedTypes.Where(i => i.ImplementedInterfaces.Contains(csType));
                }
            }

            internal static IEnumerable<TypeInfo> ConfiguredStyleSchemes {
                get {
                    var ssType = typeof(IStyleScheme);
                    return ssType.Assembly.DefinedTypes.Where(i => i.ImplementedInterfaces.Contains(ssType));
                }
            }

            internal static IColorScheme ColorScheme;
            internal static IStyleScheme StyleScheme;

            static Styles() {
                SetColorScheme(Properties.Settings.Default.ColorScheme);
            }

            static IColorScheme GetColorScheme(string colorScheme) {
                var schemeClass = ConfiguredColorSchemes.Where(i => i.Name == "Colors_" + colorScheme).First();
                return (IColorScheme)Activator.CreateInstance(schemeClass);
            }

            static IStyleScheme GetStyleScheme(string styleScheme) {
                var schemeClass = ConfiguredStyleSchemes.Where(i => i.Name == "Styles_" + styleScheme).First();
                return (IStyleScheme)Activator.CreateInstance(schemeClass);
            }

            public static void SetColorScheme(string colorScheme) {
                try { ColorScheme = GetColorScheme(colorScheme); } catch (InvalidOperationException) {
                    MessageBox.Show("Color scheme not found.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                try {
                    if (StyleScheme == null) StyleScheme = GetStyleScheme(Properties.Settings.Default.StyleScheme);
                } catch {
                    MessageBox.Show("Style scheme not found.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                Background = ColorScheme.Background;
                Carret = ColorScheme.Carret;
                LineNumber = ColorScheme.LineNumber;
                ServiceLine = ColorScheme.ServiceLine;
                Selection = ColorScheme.Selection;
                Default = new TextStyle(new SolidBrush(ColorScheme.Text), null, FontStyle.Regular);
                SameWord = new TextStyle(new SolidBrush(ColorScheme.SameWordText), new SolidBrush(ColorScheme.SameWord), StyleScheme.SameWord);
                Comment = new TextStyle(new SolidBrush(ColorScheme.Comment), null, StyleScheme.Comment);
                Keyword0 = new TextStyle(new SolidBrush(ColorScheme.Keyword), null, StyleScheme.Keyword);
                Keyword1 = new TextStyle(new SolidBrush(ColorScheme.Keyword1), null, StyleScheme.Keyword1);
                Keyword2 = new TextStyle(new SolidBrush(ColorScheme.Keyword2), null, StyleScheme.Keyword2);
                Keyword3 = new TextStyle(new SolidBrush(ColorScheme.Keyword3), null, StyleScheme.Keyword3);
                Keyword4 = new TextStyle(new SolidBrush(ColorScheme.Keyword4), null, StyleScheme.Keyword4);
                Number = new TextStyle(new SolidBrush(ColorScheme.Number), null, StyleScheme.Number);
                Time = new TextStyle(new SolidBrush(ColorScheme.Time), null, StyleScheme.Time);
                Special = new TextStyle(new SolidBrush(ColorScheme.Special), null, StyleScheme.Special);
                Command = new TextStyle(new SolidBrush(ColorScheme.Command), null, StyleScheme.Command);
                Path = new TextStyle(new SolidBrush(ColorScheme.Path), null, StyleScheme.Path);
                SearchResult = new TargetStyle(ColorScheme.SearchResultBack, ColorScheme.SearchResultFrame);
                ReplaceResult = new TargetStyle(ColorScheme.ReplaceResultBack, ColorScheme.ReplaceResultFrame);
            }

        }

        internal abstract class StyleMap {
            public const StyleIndex SearchResult = StyleIndex.Style0;
            public const StyleIndex ReplaceResult = StyleIndex.Style1;
            public const StyleIndex SameWord = StyleIndex.Style2;
            public const StyleIndex Special = StyleIndex.Style3;
            public const StyleIndex Command = StyleIndex.Style4;
            public const StyleIndex Comment = StyleIndex.Style5;
            public const StyleIndex Keyword0 = StyleIndex.Style6;
            public const StyleIndex Keyword1 = StyleIndex.Style7;
            public const StyleIndex Keyword2 = StyleIndex.Style8;
            public const StyleIndex Keyword3 = StyleIndex.Style9;
            public const StyleIndex Keyword4 = StyleIndex.Style10;
            public const StyleIndex Time = StyleIndex.Style11;
            public const StyleIndex Path = StyleIndex.Style12;
            public const StyleIndex Number = StyleIndex.Style13;
        }

        private Editor E;

        internal static bool FullAsyncMode;

        internal EditorSyntax(Editor e, bool fullAsyncMode = false) {
            E = e;
            FullAsyncMode = fullAsyncMode;
            switch (E.File.Type) {
                case EditorFile.Types.HTML:
                    E.Language = Language.HTML;
                    break;
                default:
                    E.Language = Language.Custom;
                    E.CustomHighlighter = this;
                    E.CommentPrefix = "//";
                    E.IncludeInWordSelection = @"_-/\.";
                    break;
            }
            E.BackBrush = new SolidBrush(Styles.Background);
            E.BackColor = Styles.Background;
            E.ForeColor = (Styles.Default.ForeBrush as SolidBrush).Color;
            E.PaddingBackColor = Styles.Background;
            E.LineNumberColor = Styles.LineNumber;
            E.IndentBackColor = Styles.Background;
            E.TextAreaBorderColor = Styles.Background;
            E.ServiceLinesColor = Styles.ServiceLine;
            E.CaretColor = Styles.Carret;
            E.DefaultStyle = Styles.Default;
            E.SelectionColor = Styles.Selection;
            E.SyntaxHighlighter.CommentStyle = Styles.Comment;
            E.SyntaxHighlighter.TagBracketStyle = Styles.Comment;
            E.SyntaxHighlighter.TagNameStyle = Styles.Keyword0;
            E.SyntaxHighlighter.AttributeStyle = Styles.Keyword3;
            E.SyntaxHighlighter.AttributeValueStyle = Styles.Keyword2;
            E.SyntaxHighlighter.HtmlEntityStyle = Styles.Keyword4;
            E.SyntaxHighlighter.CommentTagStyle = Styles.Comment;
            E.SyntaxHighlighter.FunctionsStyle = Styles.Comment;
            E.HighlightingRangeType = HighlightingRangeType.VisibleRange;
            if (!E.FileBindingMode) GetStyles();
        }

        public void GetStyles(bool reload = false) {
            if (reload || E.Styles[1] == null) {
                E.ClearStylesBuffer();
                E.AddStyle(Styles.SearchResult);
                E.AddStyle(Styles.ReplaceResult);
                E.AddStyle(Styles.SameWord);
                E.AddStyle(Styles.Special);
                E.AddStyle(Styles.Command);
                E.AddStyle(Styles.Comment);
                E.AddStyle(Styles.Keyword0);
                E.AddStyle(Styles.Keyword1);
                E.AddStyle(Styles.Keyword2);
                E.AddStyle(Styles.Keyword3);
                E.AddStyle(Styles.Keyword4);
                E.AddStyle(Styles.Time);
                E.AddStyle(Styles.Path);
                E.AddStyle(Styles.Number);
            }
        }

        public void HighlightSyntax(Range range) {
            var w = new BackgroundWorker();
            w.DoWork += BackgroundHighlighting;
            w.RunWorkerCompleted += BackgroundHighlightingDone;
            w.RunWorkerAsync(range);
        }

        static object stylesLock = new object();

        private void BackgroundHighlighting(object sender, DoWorkEventArgs e) {
            var range = e.Argument as Range;
            var type = E.File.Type;
            if (!FullAsyncMode && range.tb.InvokeRequired) { range.tb.Invoke(new Action(() => { BackgroundHighlighting(sender, e); })); }
            else {
                if (E.FileBindingMode) lock (stylesLock) GetStyles();
                switch (type) {
                    case ScnEdit.EditorFile.Types.SceneryMain:
                    case ScnEdit.EditorFile.Types.SceneryPart:
                        range.ClearStyle(
                            StyleIndex.Style2 | StyleIndex.Style3 | StyleIndex.Style4 | StyleIndex.Style5 |
                            StyleIndex.Style6 | StyleIndex.Style7 | StyleIndex.Style8 | StyleIndex.Style9 |
                            StyleIndex.Style10 | StyleIndex.Style11 | StyleIndex.Style12 | StyleIndex.Style13
                        );
                        range.SetStyle(StyleMap.Special, new ScnSyntax.Special());
                        range.SetStyle(StyleMap.Command, new ScnSyntax.Command());
                        range.SetStyle(StyleMap.Comment, new ScnSyntax.Comment());
                        range.SetStyle(StyleMap.Keyword0, new ScnSyntax.Keyword1());
                        range.SetStyle(StyleMap.Keyword1, new ScnSyntax.Keyword2());
                        range.SetStyle(StyleMap.Keyword2, new ScnSyntax.Keyword3());
                        range.SetStyle(StyleMap.Keyword3, new ScnSyntax.Keyword4());
                        range.SetStyle(StyleMap.Keyword4, new ScnSyntax.Keyword5());
                        range.SetStyle(StyleMap.Time, new ScnSyntax.Time());
                        range.SetStyle(StyleMap.Path, new ScnSyntax.Path());
                        range.SetStyle(StyleMap.Number, new ScnSyntax.Number());
                        break;
                    case ScnEdit.EditorFile.Types.Timetable:
                        range.ClearStyle(StyleMap.Comment | StyleMap.Time);
                        range.SetStyle(StyleMap.Comment, new ScnSyntax.TimetableFrame());
                        range.SetStyle(StyleMap.Time, new ScnSyntax.TimetableTime());
                        break;
                }
            }
        }

        void BackgroundHighlightingDone(object sender, RunWorkerCompletedEventArgs e) {
            var worker = sender as BackgroundWorker;
            worker.Dispose();
        }

        public void SameWordHighlight(object sender, EventArgs e) {
            E.ClearStyle(StyleMap.SameWord);
            if (E.Selection.IsEmpty) return;
            var text = E.SelectWord();
            if (text.Length == 0 || E.Selection.Text.Contains(' ')) return;
            var ranges = E.GetRanges("\\b" + Regex.Escape(text) + "\\b").ToArray();
            if (ranges.Length > 1) foreach (var r in ranges) r.SetStyle(StyleMap.SameWord);
        }

        public void HintParser(object sender, ToolTipNeededEventArgs e) {
            var ctl = sender as FastColoredTextBox;
            var p = ctl.PlaceToPosition(e.Place);
            foreach (Match m in new ScnSyntax.NodeParams().Matches(ctl.Text))
                MatchParser(e, p, m, "node", NodeDescriptor);
            foreach (Match m in new ScnSyntax.IncludeParams().Matches(ctl.Text))
                MatchParser(e, p, m, "include", IncludeDescriptor);
            foreach (Match m in new ScnSyntax.EventParams().Matches(ctl.Text))
                MatchParser(e, p, m, "event", EventDescriptor);
            foreach (Match m in new ScnSyntax.TrainsetParams().Matches(ctl.Text))
                MatchParser(e, p, m, "trainset", TrainsetDescriptor);
        }

        bool IsComment(int index, int length) {
            return E.GetRange(index, index + length).Chars.First().style.HasFlag(StyleMap.Comment);   
        }

        void MatchParser(ToolTipNeededEventArgs e, int p, Match m, string title = null, ParamDescriptor descriptor = null) {
            if (p >= m.Index && p <= (m.Index + m.Length)) {
                var i = 0;
                var d = "P[{0}]: {1}";
                var v = new List<string>();
                foreach (Capture c in m.Groups[1].Captures) {
                    if (IsComment(c.Index, c.Length)) continue;
                    i++;
                    v.Add(c.Value);
                    if (p >= c.Index && p <= (c.Index + c.Length)) {
                        if (descriptor != null) descriptor(i, ref d, v);
                        e.ToolTipIcon = ToolTipIcon.Info;
                        e.ToolTipTitle = title;
                        e.ToolTipText = String.Format(d, i - 1, c.Value);
                    }
                }
            }
        }

        delegate void ParamDescriptor(int index, ref string description, List<string> values);

        void NodeDescriptor(int i, ref string d, List<string> v) {
            switch (i) {
                case 1: d = "Max distance: {1}"; break;
                case 2: d = "Min distance: {1}"; break;
                case 3: d = "Name: \"{1}\""; break;
                case 4: d = "Type: {1}"; break;
            }
        }

        void IncludeDescriptor(int i, ref string d, List<string> v) {
            if (i == 1) d = "File: \"{1}\"";
        }

        void EventDescriptor(int i, ref string d, List<string> v) {
            switch (i) {
                case 1: d = "Name: \"{1}\""; break;
                case 2: d = "Type: {1}"; break;
                case 3: d = "Delay: {1}s"; break;
            }
        }

        void TrainsetDescriptor(int i, ref string d, List<string> v) {
            switch (i) {
                case 1: d = "Timetable: \"{1}\""; break;
                case 2: d = "Track: \"{1}\""; break;
                case 3: d = "Offset: {1}m"; break;
                case 4: d = "Velocity: {1}km/h"; break;
            }
        }

    }

}
