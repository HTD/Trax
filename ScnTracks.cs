using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScnEdit {
    
    /// <summary>
    /// Scenery track definition
    /// </summary>
    public class ScnTrack : ScnVectorObject<ScnTrack> {

        const double LinkDistance = 0.01;

        #region Fields
        
        // definition
        public string Name;
        public string TrackType;
        public double TrackLength;
        public double TrackWidth;
        public double Friction;
        public double SoundDist;
        public int Quality;
        public int DamageFlag;
        public string Environment;
        public string Visibility;
        // optional for visible
        public string Tex1;
        public double? TexLength;
        public string Tex2;
        public double? TexHeight;
        public double? TexWidth;
        public double? TexSlope;
        // geometry
        public V3D Point1;
        public double Roll1;
        public V3D CVec1;
        public V3D CVec2;
        public V3D Point2;
        public double Roll2;
        public double Radius1;
        // geometry (switch)
        public V3D Point3;
        public double? Roll3;
        public V3D CVec3;
        public V3D CVec4;
        public V3D Point4;
        public double? Roll4;
        public double? Radius2;
        // optional
        public double? TrackLength2;
        public double? Velocity;
        public string Event0;
        public string Event1;
        public string Event2;
        public string Eventall0;
        public string Eventall1;
        public string Eventall2;
        public string Isolated;
        public string Extras;
        // editor specific
        public string IncludesBefore;
        public string IncludesAfter;

        #endregion

        #region Properties

        public bool IsSwitch { get { return Point3 != null; } }
        public int SwitchState { get; set; }
        public V3D P1 { get { return SwitchState == 0 ? Point1 : Point3; } }
        public V3D P2 { get { return SwitchState == 0 ? Point2 : Point4; } }

        #endregion

        /// <summary>
        /// Track empty constructor
        /// </summary>
        public ScnTrack() { }

        /// <summary>
        /// Track created from lexer node
        /// </summary>
        /// <param name="path"></param>
        /// <param name="buffer"></param>
        /// <param name="node"></param>
        public ScnTrack(string path, byte[] buffer, ScnNodeLexer.Node node) {
            int i = 0, block = 0;
            string xname = null;
            List<string> extras = new List<string>();
            foreach (var value in node.Values(buffer)) {
                switch (block) {
                    case 0: // common properties
                        switch (i++) {
                            case 0: TrackType = GetLCString(value); break;
                            case 1: TrackLength = (float)value; break;
                            case 2: TrackWidth = (float)value; break;
                            case 3: Friction = (float)value; break;
                            case 4: SoundDist = (float)value; break;
                            case 5: Quality = (int)(float)value; break;
                            case 6: DamageFlag = (int)(float)value; break;
                            case 7: Environment = GetLCString(value); break;
                            case 8: Visibility = GetLCString(value);
                                block = Visibility == "vis" ? 1 : 2;
                                i = 0;
                                break;
                        }
                        break;
                    case 1: // fields for visible tracks
                        switch (i++) {
                            case 0: Tex1 = GetString(value);  break;
                            case 1: TexLength = (float?)value; break;
                            case 2: Tex2 = GetString(value); break;
                            case 3: TexHeight = (float?)value; break;
                            case 4: TexWidth = (float?)value; break;
                            case 5: TexSlope = (float?)value;
                                block++;
                                i = 0;
                                break;
                        }
                        break;
                    case 2: // track vectors and parameters
                        switch (i++) {
                            case 0: Point1 = new V3D { X = (float)value }; break;
                            case 1: Point1.Y = (float)value; break;
                            case 2: Point1.Z = (float)value; break;
                            case 3: Roll1 = (float)value; break;
                            case 4: CVec1 = new V3D { X = (float)value }; break;
                            case 5: CVec1.Y = (float)value; break;
                            case 6: CVec1.Z = (float)value; break;
                            case 7: CVec2 = new V3D { X = (float)value }; break;
                            case 8: CVec2.Y = (float)value; break;
                            case 9: CVec2.Z = (float)value; break;
                            case 10: Point2 = new V3D { X = (float)value }; break;
                            case 11: Point2.Y = (float)value; break;
                            case 12: Point2.Z = (float)value; break;
                            case 13: Roll2 = (float)value; break;
                            case 14: Radius1 = (float)value;
                                block = (TrackType == "switch" || TrackType == "cross") ? 3 : 4;
                                i = 0;
                                break;
                        }
                        break;
                    case 3: // switch vectors and parameters
                        switch (i++) {
                            case 0: Point3 = new V3D { X = (float)value }; break;
                            case 1: Point3.Y = (float)value; break;
                            case 2: Point3.Z = (float)value; break;
                            case 3: Roll3 = (float)value; break;
                            case 4: CVec3 = new V3D { X = (float)value }; break;
                            case 5: CVec3.Y = (float)value; break;
                            case 6: CVec3.Z = (float)value; break;
                            case 7: CVec4 = new V3D { X = (float)value }; break;
                            case 8: CVec4.Y = (float)value; break;
                            case 9: CVec4.Z = (float)value; break;
                            case 10: Point4 = new V3D { X = (float)value }; break;
                            case 11: Point4.Y = (float)value; break;
                            case 12: Point4.Z = (float)value; break;
                            case 13: Roll4 = (float)value; break;
                            case 14: Radius2 = (float)value;
                                block++;
                                i = 0;
                                break;
                        }
                        break;
                    case 4: // extras
                        if (i++ % 2 == 0) xname = GetLCString(value);
                        else {
                            switch (xname) {
                                case "velocity": Velocity = (float)value; break;
                                case "event0": Event0 = (string)value; break;
                                case "event1": Event1 = (string)value; break;
                                case "event2": Event2 = (string)value; break;
                                case "eventall0": Eventall0 = (string)value; break;
                                case "eventall1": Eventall1 = (string)value; break;
                                case "eventall2": Eventall2 = (string)value; break;
                                case "isolated": Isolated = (string)value; break;
                                default: extras.Add(xname); extras.Add(GetString(value)); break;
                            }
                        }
                        break;
                }
            }
            if (extras.Count > 0) Extras = String.Join(" ", extras);
            ScnType = "track";
            SourcePath = path;
            SourceIndex = node.SourceIndex;
            SourceLength = node.SourceLength;
        }

        /// <summary>
        /// Gets string value from object which could not necessarily be boxed string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetString(object value) { return value == null ? null : value.ToString(); }
        
        /// <summary>
        /// Gets lower case string value from object which could not necessarily be boxed string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetLCString(object value) { return value == null ? null : value.ToString().ToLowerInvariant(); }

        /// <summary>
        /// Parses track match to track object
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static ScnTrack Parse(Match match, string path = null, ScnTrackIncludes includes = ScnTrackIncludes.Before) {
            var track = new ScnTrack();
            int g = 1;
            if (includes == ScnTrackIncludes.Before) track.IncludesBefore = match.Groups[g++].Value;
            if (track.IncludesBefore != null && track.IncludesBefore.Length < 1) track.IncludesBefore = null;
            track.Name = match.Groups[g++].Value;
            var c = match.Groups[g++].Captures;
            if (includes == ScnTrackIncludes.After) track.IncludesAfter = match.Groups[g++].Value;
            if (track.IncludesAfter != null && track.IncludesAfter.Length < 1) track.IncludesAfter = null;
            var f = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
            int i = 0;
            track.TrackType = c[i++].Value.ToLowerInvariant();
            track.TrackLength = Double.Parse(c[i++].Value, f);
            track.TrackWidth = Double.Parse(c[i++].Value, f);
            track.Friction = Double.Parse(c[i++].Value, f);
            track.SoundDist = Double.Parse(c[i++].Value, f);
            track.Quality = Int32.Parse(c[i++].Value, f);
            track.DamageFlag = Int32.Parse(c[i++].Value, f);
            track.Environment = c[i++].Value.ToLowerInvariant();
            track.Visibility = c[i++].Value.ToLowerInvariant();
            if (track.Visibility == "vis") {
                track.Tex1 = c[i++].Value;
                track.TexLength = Double.Parse(c[i++].Value, f);
                track.Tex2 = c[i++].Value;
                track.TexHeight = Double.Parse(c[i++].Value, f);
                track.TexWidth = Double.Parse(c[i++].Value, f);
                track.TexSlope = Double.Parse(c[i++].Value, f);
            }
            track.Point1 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
            track.Roll1 = Double.Parse(c[i++].Value, f);
            track.CVec1 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
            track.CVec2 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
            track.Point2 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
            track.Roll2 = Double.Parse(c[i++].Value, f);
            track.Radius1 = Double.Parse(c[i++].Value, f);
            if (track.TrackType == "switch") {
                track.Point3 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
                track.Roll3 = Double.Parse(c[i++].Value, f);
                track.CVec3 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
                track.CVec4 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
                track.Point4 = new V3D(Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f), Double.Parse(c[i++].Value, f));
                track.Roll4 = Double.Parse(c[i++].Value, f);
                track.Radius2 = Double.Parse(c[i++].Value, f);
            }
            var extras = new List<string>();
            for (var count = c.Count; i < count; i++) {
                switch (c[i].Value.ToLowerInvariant()) {
                    case "velocity": track.Velocity = Double.Parse(c[++i].Value, f); break;
                    case "event0": track.Event0 = c[++i].Value; break;
                    case "event1": track.Event1 = c[++i].Value; break;
                    case "event2": track.Event2 = c[++i].Value; break;
                    case "eventall0": track.Eventall0 = c[++i].Value; break;
                    case "eventall1": track.Eventall1 = c[++i].Value; break;
                    case "eventall2": track.Eventall2 = c[++i].Value; break;
                    case "isolated": track.Isolated = c[++i].Value; break;
                    default: extras.Add(c[i].Value); break;
                }
            }
            if (extras.Count > 0) track.Extras = String.Join(" ", extras);
            track.ScnType = "track";
            track.SourcePath = path;
            track.SourceIndex = match.Index;
            track.SourceLength = match.Length;
            return track;
        }

        private bool IsLinked(V3D p1, V3D p2) {
            if (p1 == null || p2 == null) return false;
            return (p1 - p2).Length < LinkDistance;
        }


        /// <summary>
        /// Returns true if this track's start is linked to given track end
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool IsStartLinkedTo(ScnTrack track, out bool switchEnd) {
            switchEnd = false;
            if (track == null) return false;
            bool mainEnd = IsLinked(Point1, track.Point2) || IsLinked(Point3, track.Point2);
            switchEnd = IsLinked(Point1, track.Point4) || IsLinked(Point3, track.Point4);
            return mainEnd || switchEnd;
        }

        /// <summary>
        /// Returns true if this track's end is linked to given track start
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool IsEndLinkedTo(ScnTrack track, out bool switchEnd) {
            switchEnd = false;
            if (track == null) return false;
            bool mainEnd = IsLinked(Point2, track.Point1) || IsLinked(Point4, track.Point1);
            switchEnd = IsLinked(Point2, track.Point3) || IsLinked(Point4, track.Point3);
            return mainEnd || switchEnd;
        }

        /// <summary>
        /// Returns true if this track is linked to given track
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool IsLinkedTo(ScnTrack track, out bool switchEnd) {
            return IsStartLinkedTo(track, out switchEnd) || IsEndLinkedTo(track, out switchEnd);
        }

        /// <summary>
        /// Sets Curves field of ScnVectorObject base
        /// </summary>
        public void GetCurves() {
            Curves =
                (IsSwitch)
                    ? new ScnBezier[] {
                        new ScnBezier(Point1, CVec1, CVec2, Point2),
                        new ScnBezier(Point3, CVec3, CVec4, Point4)
                    }
                    : new ScnBezier[] { new ScnBezier(Point1, CVec1, CVec2, Point2) };
        }

        /// <summary>
        /// Calculates track length
        /// </summary>
        /// <param name="n">1 for alternative track in switch</param>
        /// <returns></returns>
        public double GetLength(int? switchState = null) {
            if ((switchState != null ? switchState : SwitchState) == 0) {
                if (CVec1 == null || CVec2 == null || (CVec1.Zero && CVec2.Zero)) return Math.Round((Point1 - Point2).Length, 9);
                return Math.Round(new ScnBezier(Point1, CVec1, CVec2, Point2).QLength, 9);
            } else {
                if (CVec3 == null || CVec4 == null || (CVec3.Zero && CVec4.Zero)) return Math.Round((Point3 - Point4).Length, 9);
                return Math.Round(new ScnBezier(Point3, CVec3, CVec4, Point4).QLength, 9);
            }
        }

        /// <summary>
        /// Calculates track lenghts (main and swith if applicable)
        /// </summary>
        public void GetLengths() {
            TrackLength = GetLength(0);
            if (IsSwitch) TrackLength2 = GetLength(1);
        }

        /// <summary>
        /// Returns track's text representation for debugging purpose
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Name + " : " + (
                (Point3 != null && Point4 != null)
                    ? String.Format("{0} -> {1} x {2} -> {3}", Point1, Point2, Point3, Point4)
                    : String.Format("{0} -> {1}", Point1, Point2)
                );
        }

        public string AsText() {
            string text = IncludesBefore != null ? (IncludesBefore + "\r\n") : "";
            string basePart =
                String.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "node -1 0 {0} track {1} {2} {3} {4} {5} {6}",
                    Name, TrackType, ScnNumbers.ToString(new[] { TrackLength, TrackWidth, Friction, SoundDist }),
                    Quality, DamageFlag, Environment.ToLowerInvariant(), Visibility.ToLowerInvariant()
                );
            text += basePart;
            string visiblePart = null;
            if (Tex1 != null && Tex2 != null)
                visiblePart =
                    String.Format(
                        "{0} {1} {2} {3}",
                        Tex1, ScnNumbers.ToString(TexLength), Tex2, ScnNumbers.ToString(new[] { TexHeight, TexWidth, TexSlope })
                    );
            string trackPart =
                String.Format(
                    "{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}",
                    ScnNumbers.ToString(new[] { Point1.X, Point1.Y, Point1.Z, Roll1 }),
                    ScnNumbers.ToString(new[] { CVec1.X, CVec1.Y, CVec1.Z }),
                    ScnNumbers.ToString(new[] { CVec2.X, CVec2.Y, CVec2.Z }),
                    ScnNumbers.ToString(new[] { Point2.X, Point2.Y, Point2.Z, Roll2 }),
                    ScnNumbers.ToString(Radius1)
                );
            string switchPart = null;
            if (Point3 != null && Point4 != null)
                switchPart =
                    String.Format(
                        "{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}",
                        ScnNumbers.ToString(new[] { Point3.X, Point3.Y, Point3.Z, Roll3 }),
                        ScnNumbers.ToString(new[] { CVec3.X, CVec3.Y, CVec3.Z }),
                        ScnNumbers.ToString(new[] { CVec4.X, CVec4.Y, CVec4.Z }),
                        ScnNumbers.ToString(new[] { Point4.X, Point4.Y, Point4.Z, Roll4 }),
                        ScnNumbers.ToString(Radius2)
                    );
            
            if (visiblePart != null) text += "\r\n" + visiblePart;
            text += "\r\n" + trackPart;
            if (switchPart != null) text += "\r\n" + switchPart;
            if (TrackLength2 != null) text += String.Format("\r\ntracklength {0}", ScnNumbers.ToString(TrackLength2));
            if (Velocity != null) text += String.Format("\r\nvelocity {0}", ScnNumbers.ToString(Velocity));
            if (Event0 != null) text += String.Format("\r\nevent0 {0}", Event0);
            if (Event1 != null) text += String.Format("\r\nevent1 {0}", Event1);
            if (Event2 != null) text += String.Format("\r\nevent2 {0}", Event2);
            if (Eventall0 != null) text += String.Format("\r\neventall0 {0}", Eventall0);
            if (Eventall1 != null) text += String.Format("\r\neventall0 {0}", Eventall1);
            if (Eventall2 != null) text += String.Format("\r\neventall0 {0}", Eventall2);
            if (Extras != null) text += "\r\n" + Extras;
            text += "\r\nendtrack";
            if (IncludesAfter != null) text += "\r\n" + IncludesAfter;
            return text;
        }

    }

    /// <summary>
    /// Scenery track list
    /// </summary>
    public class ScnTracks : ScnVectorObjects<ScnTrack> {

        #region Private

        #region Regular expressions

        private const string PatComment = @"\s*//.*$";
        private const string PatXvs = @"(?:\r?\n){3,}";
        private const string PatIncludeBefore = @"(?:(include[^\r\n]+end)[ \t;]*\r?\n)?";
        private const string PatIncludeAfter = @"(?:\r?\n[ \t;]*(include[^\r\n]+end))?";
        private const string PatTrackBlock = @"(?<!// *)node.*?track.*?(?<!// *)endtrack";
        private const string PatTrackDef = @"node[\s;]+[^\s;]+[\s;]+[^\s;]+[\s;]+([^\s;]+)[\s;]+track[\s;]+(?:([^\s;]+)+[\s;]+)+?endtrack";
        private static Regex RxComment = new Regex(PatComment, RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex RxXvs = new Regex(PatXvs, RegexOptions.Compiled);

        private Regex _BlockRegex;
        private Regex BlockRegex {
            get {
                return _BlockRegex ?? (
                    _BlockRegex = (
                        new Regex(
                            (SourceIncludes.HasFlag(ScnTrackIncludes.Before) ? PatIncludeBefore : "") +
                            PatTrackBlock +
                            (SourceIncludes.HasFlag(ScnTrackIncludes.After) ? PatIncludeAfter : ""),
                            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase
                        )
                    )
                );
            }
        }

        private Regex _DataRegex;
        private Regex DataRegex {
            get {
                return _DataRegex ?? (
                    _DataRegex = (
                        new Regex(
                            (SourceIncludes.HasFlag(ScnTrackIncludes.Before) ? PatIncludeBefore : "") +
                            PatTrackDef +
                            (SourceIncludes.HasFlag(ScnTrackIncludes.After) ? PatIncludeAfter : ""),
                            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase
                        )
                    )
                );
            }
        }

        #endregion

        private ScnTrackIncludes SourceIncludes;
        private int[][] SourceFragments;

        #region Track finder

        private bool IsSeparator(char c) { return c == ' ' || c == ';' || c == '\t' || c == '\r' || c == '\n'; }

        private int FindStart(int length) {
            int x = 0, y = -1;
            while (y < 0 && x < length) {
                y = Source.IndexOf("track", x);
                if (y > 0) {
                    if (!IsSeparator(Source[y - 1]) || !IsSeparator(Source[y + 5])) { x = y + 5; y = -1; continue; }
                    x = y;
                } else return -1;
            }
            if (x > 0) x = Source.LastIndexOf("node", x);
            if (SourceIncludes.HasFlag(ScnTrackIncludes.Before)) {
                y = Source.LastIndexOf("include", 0, x);
                if (y > -1) x = y;
            }
            return x;
        }

        private int FindEnd(int start, int length) {
            int x = start, y = -1;
            while (y < 0 && length - x > start) {
                y = Source.LastIndexOf("endtrack", length - 1, length - x);
                if (y > 0) {
                    if (!IsSeparator(Source[y - 1]) || ((y + 8 < length) && !IsSeparator(Source[y + 8]))) { x = y + 1; y = -1; continue; }
                    x = y;
                } else return length - 1;
            }
            x += 7;
            if (SourceIncludes.HasFlag(ScnTrackIncludes.After)) {
                y = Source.IndexOf("include", x);
                bool allSeparators = true;
                if (y >= x) {
                    for (int i = 0; i < y - x; i++) if (!IsSeparator(Source[i])) allSeparators = false;
                    if (allSeparators) x = Source.IndexOf("end", x);
                }
            }
            return x > 0 ? x : (length - 1);
        }

        #endregion

        private void Parse() {
            try {
                var lexer = new ScnNodeLexer(Source, "track");
                if (lexer.Nodes != null)
                    foreach (var node in lexer.Nodes)
                        Add(new ScnTrack(SourcePath, lexer.Buffer, node));
            } catch (Exception x) {
                System.Windows.Forms.MessageBox.Show(x.Message + "\r\n\r\n" + x.StackTrace);
            }
            
            ////System.Diagnostics.Debug.Print(lexer.Nodes.Length.ToString());

            //Match blockMatch, detailsMatch;
            //int length = Source.Length, start = FindStart(length);
            //if (start < 0) return;
            //int end = FindEnd(start, length);
            //length = end - start + 1;
            //var sample = Source.Substring(start, length);
            //System.Diagnostics.Debug.Print(sample);
            //while (start < length && (blockMatch = BlockRegex.Match(Source, start)).Success) {
            //    var clean = RxComment.Replace(blockMatch.Value, "");
            //    detailsMatch = DataRegex.Match(clean);
            //    var track = ScnTrack.Parse(detailsMatch, SourcePath, SourceIncludes);
            //    track.SourceIndex = blockMatch.Index;
            //    track.SourceLength = blockMatch.Length;
            //    Add(track);
            //    start = blockMatch.Index + blockMatch.Length;
            //}
        }

        private void GetSourceFragments() {
            if (Count < 1) return;
            int a = 0, b = 0, c = 0, d = 0;
            SourceFragments = new int[Count][];
            SourceFragments[0] = new int[] { 0, this[0].SourceIndex };
            for (int i = 0, ct = Count; i < ct; i++) {
                if (i == 0) {
                    c = this[i].SourceIndex;
                    d = this[i].SourceLength;
                    a = c + d;
                    continue;
                }
                b = this[i].SourceIndex;
                SourceFragments[i] = new int[] { a, b - a };
                a = b + this[i].SourceLength;
            }
            a = this[Count - 1].SourceIndex;
            b = a + this[Count - 1].SourceLength;
            SourceFragments[Count - 1] = new int[] { b, Source.Length - 1 - b };
        }

        #endregion

        public string Source;
        public string SourcePath;

        public static ScnTracks Parse(string text, string path = null, ScnTrackIncludes includes = ScnTrackIncludes.Before) {
            var tracks = new ScnTracks();
            tracks.Source = text;
            tracks.SourcePath = path;
            tracks.SourceIncludes = includes;
            tracks.Parse();
            tracks.GetSourceFragments();
            return tracks;
        }

        public static ScnTracks Load() {
            var tracks = new ScnTracks();
            ProjectFile.All.ForEach(f => {
                if (f.Type == ProjectFile.Types.SceneryPart || f.Type == ProjectFile.Types.SceneryMain) {
                    var set = ScnTracks.Parse(f.Text, f.Path, ScnTrackIncludes.Ignore);
                    if (set.Count > 0) tracks.AddRange(set);
                }
            });
            return tracks;
        }

        /// <summary>
        /// Gets a rectangle containing all tracks
        /// </summary>
        /// <returns></returns>
        public RectangleF GetBounds() {
            double x = 0, left = 0, top = 0, right = 0, bottom = 0;
            foreach (var track in this) {
                if ((x = track.Point1.X) < left) left = x;
                if ((x = track.Point2.X) < left) left = x;
                if ((x = track.Point1.X) > right) right = x;
                if ((x = track.Point2.X) > right) right = x;
                if ((x = track.Point1.Z) < top) top = x;
                if ((x = track.Point2.Z) < top) top = x;
                if ((x = track.Point1.Z) > bottom) bottom = x;
                if ((x = track.Point2.Z) > bottom) bottom = x;
                if (track.IsSwitch) {
                    if ((x = track.Point3.X) < left) left = x;
                    if ((x = track.Point4.X) < left) left = x;
                    if ((x = track.Point3.X) > right) right = x;
                    if ((x = track.Point4.X) > right) right = x;
                    if ((x = track.Point3.Z) < top) top = x;
                    if ((x = track.Point4.Z) < top) top = x;
                    if ((x = track.Point3.Z) > bottom) bottom = x;
                    if ((x = track.Point4.Z) > bottom) bottom = x;
                }
            }
            return new RectangleF((float)left, (float)top, (float)(right - left), (float)(bottom - top));
        }

        public float GetFitScale(RectangleF mapBounds, Rectangle displayBounds) {
            var x = (float)displayBounds.Width / mapBounds.Width;
            var y = (float)displayBounds.Height / mapBounds.Height;
            return new[] { x, y }.Min();
        }

        public ScnTracks GetVisible(float scale, RectangleF viewport) {
            var visible = new ScnTracks();
            return visible;
        }

        /// <summary>
        /// Sorts tracks as linked list
        /// </summary>
        public new void Sort() {
            var q = new Queue<ScnTrack>(this);
            var l = new LinkedList<ScnTrack>();
            while (q.Count > 0) {
                var t = q.Dequeue();
                if (l.Count < 1) {
                    l.AddFirst(t);
                    continue;
                }
                var added = false;
                var current = l.First;
                var switchEnd = false;
                while (current != null) {
                    if (t.IsEndLinkedTo(current.Value, out switchEnd)) {
                        l.AddBefore(current, t);
                        current = current.Next;
                        added = true;
                        break;
                    }
                    if (t.IsStartLinkedTo(current.Value, out switchEnd)) {
                        l.AddAfter(current, t);
                        current = current.Next;
                        added = true;
                        break;
                    }
                    current = current.Next;
                }
                if (!added) l.AddLast(t);
            }
            Clear();
            AddRange(l);
        }

        /// <summary>
        /// Adds unique and meaningful names for unnamed tracks
        /// </summary>
        public void AddNames() {
            int trackIndex = 0, switchIndex = 0, distIndex = 0;
            string baseName = null;
            string prevBaseName = null;
            ScnTrack prev = null;
            this.ForEach(track => {
                bool isSwitch = track.Point3 != null;
                bool isPrevSwitchEnd = false;
                bool isLinkedToPrev = track.IsLinkedTo(prev, out isPrevSwitchEnd);
                bool isNamed = track.Name != "none";
                int prevLength = prev != null ? (int)Math.Round(prev.GetLength(isPrevSwitchEnd ? 1 : 0)) : 0;
                if (!isLinkedToPrev) {
                    if (isSwitch) switchIndex++; else trackIndex++;
                }
                baseName = isNamed ? track.Name : (isSwitch ? ("s" + switchIndex.ToString()) : ("t" + trackIndex.ToString()));
                if (isLinkedToPrev) {
                    distIndex += prevLength;
                    if (!isNamed && distIndex > 0) track.Name = prevBaseName + "_" + distIndex.ToString();
                } else {
                    distIndex = 0;
                    if (!isNamed) track.Name = baseName;
                }
                prev = track;
                if (!isLinkedToPrev || isNamed) prevBaseName = baseName;
            });
        }

        /// <summary>
        /// Adds unique and meaningful names for unnamed tracks, but first sorts tracks as linked list
        /// </summary>
        public void SortAddNames() {
            Sort();
            AddNames();
        }

        public string ReplaceText() {
            if (Count < 1) return Source;
            var b = new StringBuilder();
            foreach (var f in SourceFragments) if (f[1] > 0) b.Append(Source.Substring(f[0], f[1]));
            var oc = RxXvs.Replace(b.ToString(), "\r\n\r\n").Trim();
            return Source = (
                oc.Length > 0
                    ? ("// Tracks:\r\n\r\n" + AsText() + "\r\n\r\n// Original content:\r\n\r\n" + oc)
                    : AsText()
            );
        }

        /// <summary>
        /// Returns scenery track list as text
        /// </summary>
        /// <returns></returns>
        public string AsText() {
            return String.Join("\r\n\r\n", this.ConvertAll<string>(i => i.AsText()));
        }

    }

    public enum ScnTrackIncludes {
        Ignore, Before, After
    }

}
