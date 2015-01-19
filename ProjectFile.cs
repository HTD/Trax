using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ScnEdit {
    
    internal class ProjectFile {

        #region Constants and enumerations

        private const string _SceneryDirectoryName = "scenery";

        internal enum Types { Text, SceneryMain, SceneryPart, Timetable, HTML, CSS }

        internal enum Roles { Any, Main, Include, Timetable, Description, Log }

        #endregion

        #region Properties

        internal static List<ProjectFile> All { get; set; }
        
        internal List<ProjectFile> References { get; set; }

        #endregion

        #region Fields

        internal Types Type;
        internal Roles Role;
        internal string BaseDirectory;
        internal string SceneryDirectory;
        internal string Path;
        internal string Directory;
        internal string FileName;
        internal string RelativeDirectory;
        internal bool IsConverted;
        internal bool IsNormalized;
        internal bool IsChanged;

        public string TextCache;

        #endregion

        #region Properties

        internal static ProjectFile Project {
            get {
                return ProjectFile.All.Find(i => i.Role == Roles.Main);
            }
        }

        internal static List<ProjectFile> ProjectFiles {
            get { return ProjectFile.All; }
        }

        internal bool IsOpen { get { return this is EditorFile; } }
        internal bool HasHtmlEncoding { get { return Type == Types.HTML || Type == Types.CSS; } }
        
        /// <summary>
        /// Encoding set default for current file type
        /// </summary>
        internal Encoding EncodingDefault {
            get {
                if (_EncodingDefault != null) return _EncodingDefault;
                var settings = Properties.Settings.Default;
                return _EncodingDefault = Encoding.GetEncoding(HasHtmlEncoding ? settings.HtmlEncodingDefault : settings.EncodingDefault);
            }
        }
        
        internal bool AutoDecoding {
            get {
                if (_AutoDecodingSet) return _AutoDecoding;
                _AutoDecodingSet = true;
                return _AutoDecoding = Properties.Settings.Default.AutoDecoding;
            }
        }

        internal virtual string Text {
            get {
                using (var stream = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    stream.Close();
                    return TextCache = PreNormalize(
                        (AutoDecoding && !HasHtmlEncoding)
                            ? AutoDecode(EncodingDefault, buffer, out IsConverted)
                            : EncodingDefault.GetString(buffer)
                    );
                }
            }
        }

        internal Editor Editor {
            get {
                var editorFile = EditorFile.All.FirstOrDefault(i => i.Path == this.Path);
                if (editorFile == null) Open();
                editorFile = EditorFile.All.First(i => i.Path == this.Path);
                if (editorFile == null) return null;
                return editorFile.Editor;
            }
        }

        #endregion

        #region Events

        internal event EventHandler ReferencesResolved;

        #endregion

        #region Methods

        /// <summary>
        /// Creates new project file
        /// </summary>
        /// <param name="path">File system path</param>
        /// <param name="role">Project role</param>
        internal ProjectFile(string path, Roles role = Roles.Any) {
            Role = role;
            Path = path.Replace('/', '\\');
            Type = GetFileType(Path);
            if (Role == Roles.Timetable) Type = Types.Timetable;
            if (Type == Types.SceneryMain) Role = Roles.Main;
            var sceneryIndex = path.IndexOf(Properties.Settings.Default.SceneryDirectory, StringComparison.InvariantCultureIgnoreCase);
            var sceneryLength = _SceneryDirectoryName.Length;
            Directory = System.IO.Path.GetDirectoryName(Path);
            FileName = System.IO.Path.GetFileName(Path);
            if (sceneryIndex >= 0 && (Role == Roles.Include || Role == Roles.Timetable || Type == Types.SceneryMain || Type == Types.SceneryPart)) {
                BaseDirectory = Path.Substring(0, sceneryIndex).Trim(new[] { '\\' });
                SceneryDirectory = BaseDirectory + "\\" + Path.Substring(sceneryIndex, sceneryLength);
                RelativeDirectory = Directory.Replace(SceneryDirectory, "");
            }
            if (All != null && All.Exists(i => i.Path == Path)) return;
            if (Type == Types.SceneryMain && Role == Roles.Main) GetScenery(this);
            else {
                if (All == null) All = new List<ProjectFile>();
                All.Add(this);
            }
        }

        /// <summary>
        /// Opens file in editor
        /// </summary>
        internal void Open() { new EditorFile(this); }

        internal static void FindText(string text, bool useRegex = false) {
            var pattern = useRegex ? text : Regex.Escape(text);
            try { FindAll(new Regex(pattern, RegexOptions.Compiled)); } catch (ArgumentException) { }
        }
        
        internal static void FindSymbol(string symbol) {
            var pattern = @"(?<=^|[ :;\r\n]+)" + Regex.Escape(symbol) + "(?=[_ ;\r\n]+|$)";
            FindAll(new Regex(pattern, RegexOptions.Compiled));
        }

        internal static void ReplaceSymbol(string symbol, string replaceText) {
            var pattern = @"(?<=^|[ :;\r\n]+)" + Regex.Escape(symbol) + "(?=[_ ;\r\n]+|$)";
            ReplaceAll(new Regex(pattern, RegexOptions.Compiled), replaceText);
        }

        private static void ProcessAll(Action<ProjectFile> action) {
            var files = ProjectFiles;
            for (int i = 0, n = files.Count; i < n; i++) action(files[i]);
        }

        private static void FindAll(Regex regex) {
            Main.Instance.DisableEdit();
            Status.Text = Messages.Searching;
            Application.DoEvents();
            SearchResultsPanel.Reset();
            ProcessAll(i => {
                foreach (Match m in regex.Matches(i.Text)) {
                    i.Open();
                    var p = i.Editor.PositionToPlace(m.Index);// i.Editor.MarkSearchResult(m.Index, m.Length);
                    SearchResultsPanel.Add(new SearchResult {
                        Path = i.Path, File = i.FileName, Fragment = m.Value, Line = p.iLine + 1, Column = p.iChar + 1
                    });
                }
            });
            SearchResultsPanel.CloseIfEmpty();
            Status.Text = Messages.Ready;
            Application.DoEvents();
            Main.Instance.EnableEdit();
        }

        private static void ReplaceAll(Regex regex, string replaceText) {
            Main.Instance.DisableEdit();
            Status.Text = Messages.Replacing;
            Application.DoEvents();
            SearchResultsPanel.Reset();
            ProcessAll(i => {
                string t;
                foreach (Match m in regex.Matches(t = i.Text)) {
                    var p = i.Editor.PositionToPlace(m.Index);
                    t = t.Substring(0, m.Index) + replaceText + t.Substring(m.Index + m.Length);
                    i.Editor.BeginAutoUndo();
                    i.Editor.Text = t;
                    i.Editor.EndAutoUndo();
                    SearchResultsPanel.Add(new SearchResult {
                        Path = i.Path, File = i.FileName, Fragment = m.Value, Replacement = replaceText, Line = p.iLine + 1, Column = p.iChar + 1
                    });
                }
            });
            SearchResultsPanel.CloseIfEmpty();
            Status.Text = Messages.Ready;
            Application.DoEvents();
            Main.Instance.EnableEdit();
        }

        /// <summary>
        /// Returns file normalized text if applicable, original text otherwise
        /// </summary>
        /// <returns></returns>
        internal string Normalize() {
            var t = this is EditorFile ? (this as EditorFile).Text : (this as ProjectFile).Text;
            if (Type == Types.SceneryMain || Type == Types.SceneryPart) {
                t = new ScnSyntax.HWhiteSpace().Replace(t, " ");
                t = new ScnSyntax.VWhiteSpace().Replace(t, "\r\n");
                t = new ScnSyntax.LineEnd().Replace(t, "");
                t = new ScnSyntax.XVWhiteSpace().Replace(t, "\r\n\r\n");
                t = new ScnSyntax.ExplicitTexExt().Replace(t, "");
                var lines = new ScnSyntax.VWhiteSpace().Split(t);
                var count = lines.Length / 2;
                string[] even = new string[count], odd = new string[count];
                for (int i = 0; i < count; i++) { even[i] = lines[2 * i]; odd[i] = lines[2 * i + 1]; }
                var interleaved = false;
                interleaved |= even.All(i => String.IsNullOrWhiteSpace(i));
                interleaved |= odd.All(i => String.IsNullOrWhiteSpace(i));
                if (interleaved) t = new ScnSyntax.LineInterleave().Replace(t, "\r\n");
                return t.Trim();
            } else return t;
        }

        /// <summary>
        /// Gets all file references associated with main project file
        /// </summary>
        internal void GetReferences() {
            References = new List<ProjectFile>();
            var w = new BackgroundWorker();
            w.DoWork += new DoWorkEventHandler((s, e) => {
                GetReferences(Roles.Include, new ScnSyntax.IncludeSimple());
                GetReferences(Roles.Timetable, new ScnSyntax.Timetable(), new[] { "none", "rozklad" }, null, ".txt");
                GetReferences(Roles.Description, new ScnSyntax.CommandInclude(), null, new[] { ".txt", ".html" });
            });
            if (Role == Roles.Main)
                w.RunWorkerCompleted += new RunWorkerCompletedEventHandler((s, e) => {
                    if (ReferencesResolved != null) ReferencesResolved.Invoke(this, EventArgs.Empty);
                });
            w.RunWorkerAsync();
            w.Dispose();
        }

        /// <summary>
        /// Opens all project's references
        /// </summary>
        internal void OpenReferences() { All.ForEach(i => i.Open()); }

        #endregion

        #region Private

        private static bool _AutoDecoding;
        private static bool _AutoDecodingSet;
        private Encoding _EncodingDefault;

        private string PreNormalize(string text) {
            int initialLength = text.Length;
            var tabString = "".PadLeft(4);
            text = new ScnSyntax.LineEnding().Replace(text, "\r\n");
            text = new ScnSyntax.Tab().Replace(text, tabString);
            IsNormalized = text.Length != initialLength;
            return text;
        }

        private static string AutoDecode(Encoding nonUnicodeDefault, byte[] buffer, out bool u) {
            string s;
            var u8 = new UTF8Encoding(false, true);
            try {
                s = u8.GetString(buffer);
                u = s.Length < buffer.Length;
                return s;
            } catch (DecoderFallbackException) {
                u = false;
                return nonUnicodeDefault.GetString(buffer);
            }
        }

        private static void GetScenery(ProjectFile f) {
            if (f.Role != Roles.Main) throw new InvalidOperationException("Main scenery file expected.");
            if (Main.Instance.TrackMap != null) {
                Main.Instance.TrackMap.Close();
                Main.Instance.TrackMap.Dispose();
                Main.Instance.TrackMap = null;
            }
            All = new List<ProjectFile>();
            All.Add(f);
        }

        private Types GetFileType(string file) {
            var ext = System.IO.Path.GetExtension(file).ToLower();
            if (ext.Length > 0) ext = ext.Substring(1);
            if (ext == Properties.Settings.Default.SceneryMainExtension) return Types.SceneryMain;
            var parts = Regex.Split(Properties.Settings.Default.SceneryPartsExtensions.ToLower(), @"[ ,;\|]");
            if (parts.Any(p => ext == p)) return Types.SceneryPart;
            switch (ext) {
                case "html": return Types.HTML;
                case "css": return Types.CSS;
            }
            return Types.Text;
        }

        private void GetReferences(Roles role, Regex r, string[] ignore = null, string[] allow = null, string defaultExt = null) {
            var input = !Role.HasFlag(Roles.Description) ? new ScnSyntax.Comment().Replace(Text, "") : Text;
            foreach (Match m in r.Matches(input)) {
                if (ignore != null && ignore.Contains(m.Value)) continue;
                var path = ((role == Roles.Description ? BaseDirectory : SceneryDirectory) + "\\" + m.Value).Replace('/', '\\');
                var ext = System.IO.Path.GetExtension(m.Value);
                if (defaultExt != null && ext == "") { ext = defaultExt; path += ext; }
                if (System.IO.File.Exists(path)) {
                    if (allow != null && !allow.Contains(ext)) continue;
                    if (!References.Any(p => p.Path == path)) {
                        var reference = new ProjectFile(path, role);
                        References.Add(reference);
                        reference.GetReferences();
                    }
                }
            }
        }

        #endregion

    }

    internal class ProjectLocation {
        
        public string Path;
        public int Index;
        public int Length;

        public static ProjectLocation FromRange(Range r) {
            var e = r.tb as Editor;
            var p = e.File.Path;
            var i = e.PlaceToPosition(r.Start);
            var l = e.PlaceToPosition(r.End) - i;
            return new ProjectLocation() { Path = p, Index = i, Length = l };
        }

    }

}
