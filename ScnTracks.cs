using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ScnEdit {
    
    /// <summary>
    /// Scenery track definition
    /// </summary>
    class ScnTrack : ScnVectorObject<ScnTrack> {
        
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
        public ScnPoint Point1;
        public double Roll1;
        public ScnPoint CVec1;
        public ScnPoint CVec2;
        public ScnPoint Point2;
        public double Roll2;
        public double Radius1;
        // geometry (switch)
        public ScnPoint Point3;
        public double? Roll3;
        public ScnPoint CVec3;
        public ScnPoint CVec4;
        public ScnPoint Point4;
        public double? Roll4;
        public double? Radius2;
        // optional
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

        /// <summary>
        /// Returns true if this track's start is linked to given track end
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool IsStartLinkedTo(ScnTrack track) {
            if (track == null) return false;
            return
                Point1.IsLinkedTo(track.Point2) ||
                Point1.IsLinkedTo(track.Point4) ||
                (Point3 != null && Point3.IsLinkedTo(track.Point2)) ||
                (Point3 != null && Point3.IsLinkedTo(track.Point4));
        }

        /// <summary>
        /// Returns true if this track's end is linked to given track start
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool IsEndLinkedTo(ScnTrack track) {
            if (track == null) return false;
            return
                Point2.IsLinkedTo(track.Point1) ||
                Point2.IsLinkedTo(track.Point3) ||
                (Point4 != null && Point4.IsLinkedTo(track.Point1)) ||
                (Point4 != null && Point4.IsLinkedTo(track.Point3));
        }

        /// <summary>
        /// Returns true if this track is linked to given track
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public bool IsLinkedTo(ScnTrack track) {
            return IsStartLinkedTo(track) || IsEndLinkedTo(track);
        }

        /// <summary>
        /// Calculates track length
        /// </summary>
        /// <param name="n">1 for alternative track in switch</param>
        /// <returns></returns>
        public double GetLength(int n) {
            if (n == 0) {
                if (CVec1 == null || CVec2 == null || (CVec1.Zero && CVec2.Zero)) return Point1.DistanceTo(Point2);
                return new ScnBezier { A = Point1, B = Point1 + CVec1, C = Point2 + CVec2, D = Point2 }.Length();
            } else {
                if (CVec3 == null || CVec4 == null || (CVec3.Zero && CVec4.Zero)) return Point3.DistanceTo(Point4);
                return new ScnBezier { A = Point3, B = Point3 + CVec3, C = Point4 + CVec4, D = Point4 }.Length();
            }
        }

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
            track.TrackType = c[i++].Value;
            track.TrackLength = Double.Parse(c[i++].Value, f);
            track.TrackWidth = Double.Parse(c[i++].Value, f);
            track.Friction = Double.Parse(c[i++].Value, f);
            track.SoundDist = Double.Parse(c[i++].Value, f);
            track.Quality = Int32.Parse(c[i++].Value, f);
            track.DamageFlag = Int32.Parse(c[i++].Value, f);
            track.Environment = c[i++].Value;
            track.Visibility = c[i++].Value;
            if (track.Visibility.ToLowerInvariant() == "vis") {
                track.Tex1 = c[i++].Value;
                track.TexLength = Double.Parse(c[i++].Value, f);
                track.Tex2 = c[i++].Value;
                track.TexHeight = Double.Parse(c[i++].Value, f);
                track.TexWidth = Double.Parse(c[i++].Value, f);
                track.TexSlope = Double.Parse(c[i++].Value, f);
            }
            track.Point1 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
            track.Roll1 = Double.Parse(c[i++].Value, f);
            track.CVec1 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
            track.CVec2 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
            track.Point2 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
            track.Roll2 = Double.Parse(c[i++].Value, f);
            track.Radius1 = Double.Parse(c[i++].Value, f);
            if (track.TrackType.ToLowerInvariant() == "switch") {
                track.Point3 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
                track.Roll3 = Double.Parse(c[i++].Value, f);
                track.CVec3 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
                track.CVec4 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
                track.Point4 = ScnPoint.Parse(c[i++].Value, c[i++].Value, c[i++].Value);
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
            track.Vectors =
                (track.Point3 == null && track.Point4 != null)
                ? new ScnVector[] { new ScnVector(track.Point1, track.Point2) }
                : new ScnVector[] { new ScnVector(track.Point1, track.Point2), new ScnVector(track.Point3, track.Point4) };
            track.SourceIndex = match.Index;
            track.SourceLength = match.Length;
            track.TrackLength = track.GetLength(0);
            return track;
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
    class ScnTracks : ScnVectorObjects<ScnTrack> {

        #region Private

        #region Regular expressions

        private const string PatComment = @"\s*//.*$";
        private const string PatXvs = @"(?:\r?\n){3,}";
        private const string PatIncludeBefore = @"(?:(include[^\r\n]+end)[ \t;]*\r?\n)?";
        private const string PatIncludeAfter = @"(?:\r?\n[ \t;]*(include[^\r\n]+end))?";
        private const string PatTrackBlock = @"node.*?track.*?endtrack";
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

        private void Parse() {
            Match blockMatch, detailsMatch;
            int start = 0, length = Source.Length;
            while ((blockMatch = BlockRegex.Match(Source, start)).Success && start < length) {
                var clean = RxComment.Replace(blockMatch.Value, "");
                detailsMatch = DataRegex.Match(clean);
                var track = ScnTrack.Parse(detailsMatch, SourcePath, SourceIncludes);
                track.SourceIndex = blockMatch.Index;
                track.SourceLength = blockMatch.Length;
                Add(track);
                start = blockMatch.Index + blockMatch.Length;
            }
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
                while (current != null) {
                    if (t.IsEndLinkedTo(current.Value)) {
                        l.AddBefore(current, t);
                        current = current.Next;
                        added = true;
                        break;
                    }
                    if (t.IsStartLinkedTo(current.Value)) {
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
                bool isLinkedToPrev = track.IsLinkedTo(prev);
                bool isNamed = track.Name != "none";
                int prevLength = prev != null ? (int)prev.TrackLength : 0;
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
