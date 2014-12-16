using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScnEdit {
    
    /// <summary>
    /// Scenery track definition
    /// </summary>
    class ScnTrack {
        
        #region Fields
        
        // definition
        public string Name;
        public string Type;
        public double TrackLength;
        public double TrackWidth;
        public double Friction;
        public double SoundDist;
        public Int32 Quality;
        public byte DamageFlag;
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
        public string Isolated;
        public string Extras;
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
        /// Returns coarse approximated length of the track based on specified length or points distance
        /// </summary>
        /// <returns></returns>
        public int GetCoarseLength() {
            int length = 0;
            if (TrackLength < 0.01) {
                double length12 = Point1.DistanceTo(Point2);
                if (Point3 != null && Point4 != null) {
                    var length34 = Point3.DistanceTo(Point4);
                    length = (int)Math.Round(Math.Sqrt(length12 * length34));
                } else length = (int)Math.Round(length12);
            } else length = (int)Math.Round(TrackLength);
            return length;
        }

        /// <summary>
        /// Parses track match to track object
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public static ScnTrack Parse(Match match, ScnTrackIncludes includes = ScnTrackIncludes.Before) {
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
            track.Type = c[i++].Value;
            track.TrackLength = Double.Parse(c[i++].Value, f);
            track.TrackWidth = Double.Parse(c[i++].Value, f);
            track.Friction = Double.Parse(c[i++].Value, f);
            track.SoundDist = Double.Parse(c[i++].Value, f);
            track.Quality = Int32.Parse(c[i++].Value, f);
            track.DamageFlag = Byte.Parse(c[i++].Value, f);
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
            if (track.Type.ToLowerInvariant() == "switch") {
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
                    case "isolated": track.Isolated = c[++i].Value; break;
                    default: extras.Add(c[i].Value); break;
                }
            }
            if (extras.Count > 0) track.Extras = String.Join(" ", extras);
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
                    Name, Type, ScnNumbers.ToString(new[] { TrackLength, TrackWidth, Friction, SoundDist }),
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
            if (Extras != null) text += "\r\n" + Extras;
            text += "\r\nendtrack";
            if (IncludesAfter != null) text += "\r\n" + IncludesAfter;
            return text;
        }

    }

    /// <summary>
    /// Scenery track list
    /// </summary>
    class ScnTracks : List<ScnTrack> {

        #region Regular expressions

        private const string PatComment = @"\s*//.*$";
        private const string PatIncludeBefore = @"(?:(include[^\r\n]+end)[ \t;]*\r?\n)?";
        private const string PatIncludeAfter = @"(?:\r?\n[ \t;]*(include[^\r\n]+end))?";
        private const string PatTrackBlock = @"\s*node.*?track.*?endtrack\s*";
        private const string PatTrackDef = @"node[\s;]+[^\s;]+[\s;]+[^\s;]+[\s;]+([^\s;]+)[\s;]+track[\s;]+(?:([^\s;]+)+[\s;]+)+?endtrack";
        private static Regex RxComment = new Regex(PatComment, RegexOptions.Compiled | RegexOptions.Multiline);

        #endregion

        /// <summary>
        /// Parses text containing track definitions into scenery track list
        /// </summary>
        /// <param name="text"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static ScnTracks Parse(string text, ScnTrackIncludes includes = ScnTrackIncludes.Before) {
            var tracks = new ScnTracks();
            var clean = RxComment.Replace(text, "");
            var exp = PatTrackDef;
            if (includes.HasFlag(ScnTrackIncludes.Before)) exp = PatIncludeBefore + exp;
            if (includes.HasFlag(ScnTrackIncludes.After)) exp += PatIncludeAfter;
            var rx = new Regex(exp, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match match in rx.Matches(clean)) tracks.Add(ScnTrack.Parse(match, includes));
            return tracks;
        }

        /// <summary>
        /// Sorts tracks as linked list
        /// </summary>
        public new void Sort() {
            var groups = new List<LinkedList<ScnTrack>>();
            var queue = new Queue<ScnTrack>(this);
            var list = new LinkedList<ScnTrack>();
            while (queue.Count > 0) {
                var track = queue.Dequeue();
                if (list.Count < 1) {
                    list.AddFirst(track);
                    continue;
                }
                var added = false;
                LinkedListNode<ScnTrack> current = list.First;
                while (current != null) {
                    if (track.IsEndLinkedTo(current.Value)) {
                        list.AddBefore(current, track);
                        current = current.Next;
                        added = true;
                        break;
                    }
                    if (track.IsStartLinkedTo(current.Value)) {
                        list.AddAfter(current, track);
                        current = current.Next;
                        added = true;
                        break;
                    }
                    current = current.Next;
                }
                if (!added) list.AddLast(track);
            }
            Clear();
            AddRange(list);
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
                bool isNamed = !track.Name.StartsWith("none");
                int prevLength = prev != null ? prev.GetCoarseLength() : 0;
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
