using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Trax {
    
    /// <summary>
    /// Control for displaying a map of the scenery
    /// </summary>
    public partial class MapCtl : UserControl {

        #region Properties

        #region Colors

        [Category("Appearance")]
        public Color TrackColor { get; set; }
        [Category("Appearance")]
        public Color SelectedTrackColor { get; set; }
        [Category("Appearance")]
        public Color DetailTextColor { get; set; }
        [Category("Appearance")]
        public Color SwitchColor { get; set; }
        [Category("Appearance")]
        public Color RoadColor { get; set; }
        [Category("Appearance")]
        public Color RiverColor { get; set; }
        [Category("Appearance")]
        public Color CrossColor { get; set; }
        [Category("Appearance")]
        public Color GridColor { get; set; }
        [Category("Appearance")]
        public Color GridZeroColor { get; set; }
        [Category("Appearance")]
        public Color DotColor { get; set; }

        #endregion

        #region Options

        [Category("Appearance")]
        public bool ShowGrid { get; set; }
        [Category("Appearance")]
        public bool ShowDots { get; set; }

        #endregion

        #region State

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new float Scale {
            get {
                return CurrentScale;
            }
            set {
                CurrentScale = value;
                Invalidate();
                if (ScaleChanged != null) ScaleChanged.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF Position {
            get {
                return new PointF(Offset.X - MapCenter.X, Offset.Y - MapCenter.Y);
            }
            set { 
                Offset = new PointF(value.X + MapCenter.X, value.Y + MapCenter.Y);
                Invalidate();
                if (PositionChanged != null) PositionChanged.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsOutFull { get { return CurrentScale == FitScale; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF CursorPosition {
            get {
                return MapPointed;
            }
        }

        #endregion

        #region Data

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ScnTrackCollection Tracks {
            get { if (Splines == null) return null; return Splines.Tracks; }
            set { if (value == null) return; Splines = new SplineCollection(value); FitScale = CurrentScale = GetFitScale(); Invalidate(); }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SelectionClass Selection { get; set; }

        #endregion

        #region Private

        private RectangleF VisibleArea {
            get {
                if (Overscan == 0) return DisplayToMap(ClientRectangle);
                else {
                    var v = ClientRectangle;
                    v.Inflate(Overscan, Overscan);
                    return DisplayToMap(v);
                }
            }
        }
        
        private List<Spline> VisibleSplines {
            get {
                if (IsOutFull) return Splines;
                return Splines.Where(s => s.HasPointIn(VisibleArea)).ToList();
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler Loaded;
        public event EventHandler ScaleChanged;
        public event EventHandler PositionChanged;
        public event EventHandler CursorPositionChanged;

        #endregion

        #region Private fields

        private Transformation T = new Transformation();
        private SplineCollection Splines;
        private RectangleF MapArea;
        private int Overscan = 5;
        private float GridSize = 100f;
        private float FitScale;
        private float CurrentScale;
        private PointF CtlCenter;
        private PointF MapCenter;
        private PointF Offset;
        private PointF MapPointed;
        private Layer[] Layers;
        private object LayerLock = new Object();
        private string Message;

        #endregion

        /// <summary>
        /// Initializes map control instance
        /// </summary>
        public MapCtl() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            Font = new Font(Font.FontFamily, 9f);
            BackColor = ColorTranslator.FromHtml("#233e3b"); 
            ForeColor = ColorTranslator.FromHtml("#1d3331");
            TrackColor = Color.Black;
            SelectedTrackColor = ColorTranslator.FromHtml("#ff2200");
            DetailTextColor = ColorTranslator.FromHtml("#ff0044");
            SwitchColor = Color.Orange;
            RoadColor = ColorTranslator.FromHtml("#1d3331");
            RiverColor = ColorTranslator.FromHtml("#2d536f");
            CrossColor = RoadColor;
            GridColor = ColorTranslator.FromHtml("#ddeddd");
            GridZeroColor = ColorTranslator.FromHtml("#bbcccc");
            DotColor = ColorTranslator.FromHtml("#ff2200");
            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            ResumeLayout();
            FitScale = CurrentScale = 0.01f;
            CtlCenter = new PointF(Width / 2, Height / 2);
            Selection = new SelectionClass(this);
            BeginLoad();
        }

        #region Loading

        /// <summary>
        /// Begins track data loading
        /// </summary>
        private void BeginLoad() {
            Enabled = false;
            Message = "Loading data...";
            using (var w = new BackgroundWorker()) {
                w.DoWork += Load;
                w.RunWorkerCompleted += LoadingDone;
                w.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Loads track data in the background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private new void Load(object sender, DoWorkEventArgs e) {
            try {
                if (!DesignMode) {
                    Tracks = ScnTrackCollection.Load();
                    MapArea = Splines.Bounds;
                }
                Message = null;
            } catch (Exception x) {
                Message = x.Message;
            }
        }

        /// <summary>
        /// Completes track data loading and initializes control view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadingDone(object sender, RunWorkerCompletedEventArgs e) {
            if (Splines != null) {
                Enabled = true;
                ResetView();
                if (Loaded != null) Loaded.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Resets the map view to default value (zoomed out full, centered)
        /// </summary>
        public void ResetView() {
            lock (LayerLock) {
                Layers = new[] {
                    new Layer(3, RiverColor),
                    new Layer(4, CrossColor),
                    new Layer(2, RoadColor),
                    new Layer(1, SwitchColor),
                    new Layer(0, TrackColor)
                };
            }
            FitScale = CurrentScale = GetFitScale();
            MapCenter = new PointF(Splines.Bounds.X + Splines.Bounds.Width / 2, Splines.Bounds.Y + Splines.Bounds.Height / 2);
            Offset = new PointF(0, 0);
            Invalidate();
            if (ScaleChanged != null) ScaleChanged.Invoke(this, EventArgs.Empty);
            if (PositionChanged != null) PositionChanged.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates visible map transformation data
        /// </summary>
        private void UpdateTransformation() {
            T.Scale = CurrentScale;
            T.X = CtlCenter.X + (Offset.X - MapCenter.X) * CurrentScale;
            T.Y = CtlCenter.Y + (Offset.Y - MapCenter.Y) * CurrentScale;
        }

        /// <summary>
        /// Translates map coordinates to display point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private PointF MapToDisplay(float x, float y) {
            return new PointF(x * T.Scale + T.X, y * T.Scale + T.Y);
        }

        /// <summary>
        /// Translates map point to display point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private PointF MapToDisplay(PointF p) {
            return MapToDisplay(p.X, p.Y);
        }

        /// <summary>
        /// Translates map rectangle to display rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private RectangleF MapToDisplay(RectangleF r) {
            var a = MapToDisplay(r.Left, r.Top);
            var b = MapToDisplay(r.Right, r.Bottom);
            return new RectangleF(a.X, a.Y, b.X - a.X, b.Y - a.Y);
        }

        /// <summary>
        /// Translates display coordinates to map point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private PointF DisplayToMap(float x, float y) {
            return new PointF((x - T.X) / T.Scale, (y - T.Y) / T.Scale);
        }

        /// <summary>
        /// Translates display point to map point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private PointF DisplayToMap(PointF p) {
            return DisplayToMap(p.X, p.Y);
        }

        /// <summary>
        /// Translates display rectangle to map rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        private RectangleF DisplayToMap(RectangleF r) {
            var a = DisplayToMap(r.Left, r.Top);
            var b = DisplayToMap(r.Right, r.Bottom);
            return new RectangleF(a.X, a.Y, b.X - a.X, b.Y - a.Y);
        }

        /// <summary>
        /// Returns scale allowing whole map to fit in current control bounds
        /// </summary>
        /// <returns></returns>
        private float GetFitScale() {
            var sx = Bounds.Width / MapArea.Width;
            var sy = Bounds.Height / MapArea.Height;
            return new[] { sx, sy }.Min();
        }

        #endregion

        #region Drawing

        SizeF CharSize;

        /// <summary>
        /// Draws tracks
        /// </summary>
        /// <param name="e"></param>
        private void DrawTracks(PaintEventArgs e) {
            if (Layers == null) return;
            lock (LayerLock) {
                foreach (var layer in Layers) {
                    using (var pen = new Pen(layer.Color)) {
                        VisibleSplines.ForEach(s => {
                            if (s.T == layer.Type) {
                                pen.Width = T.Scale * s.W;
                                if (T.Scale < 0.01 || s.L)
                                    e.Graphics.DrawLine(pen, MapToDisplay(s.A), MapToDisplay(s.D));
                                else {
                                    var p1 = MapToDisplay(s.A);
                                    var p2 = MapToDisplay(s.B);
                                    var p3 = MapToDisplay(s.C);
                                    var p4 = MapToDisplay(s.D);
                                    var b1c = Math.Abs(p1.X - p2.X) >= 0.1f || Math.Abs(p1.Y - p2.Y) > 0.1f;
                                    var b2c = Math.Abs(p3.X - p4.X) >= 0.1f || Math.Abs(p3.Y - p4.Y) > 0.1f;
                                    if (b1c && b2c) e.Graphics.DrawBezier(pen, p1, p2, p3, p4); else e.Graphics.DrawLine(pen, p1, p4);
                                }
                            }
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Draws selected spline (active selected or specified explicitly)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="spline"></param>
        private void DrawSelectedSpline(PaintEventArgs e, Spline spline) {
            if (spline != null) {
                var track = spline.Track;
                var lineWidth = T.Scale * spline.W;
                using (var pen = new Pen(SelectedTrackColor) { Width = 1f + lineWidth })
                    if (T.Scale < 0.01 || spline.L)
                        e.Graphics.DrawLine(pen, MapToDisplay(spline.A), MapToDisplay(spline.D));
                    else {
                        var p1 = MapToDisplay(spline.A);
                        var p2 = MapToDisplay(spline.B);
                        var p3 = MapToDisplay(spline.C);
                        var p4 = MapToDisplay(spline.D);
                        var b1c = Math.Abs(p1.X - p2.X) >= 0.1f || Math.Abs(p1.Y - p2.Y) > 0.1f;
                        var b2c = Math.Abs(p3.X - p4.X) >= 0.1f || Math.Abs(p3.Y - p4.Y) > 0.1f;
                        if (b1c && b2c) e.Graphics.DrawBezier(pen, p1, p2, p3, p4); else e.Graphics.DrawLine(pen, p1, p4);
                    }
            }
        }

        /// <summary>
        /// Draws selected spline ends as dots (active selected or specified explicitly)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="spline"></param>
        private void DrawSelectedSplineEnds(PaintEventArgs e, Spline spline) {
            if (spline != null) {
                var track = spline.Track;
                var lineWidth = T.Scale * spline.W;
                var a = MapToDisplay(spline.A);
                var d = MapToDisplay(spline.D);
                var r = CharSize.Height / 2;
                if (lineWidth * 0.7f > r) r = lineWidth * 0.7f;
                
                using (var brush = new SolidBrush(SelectedTrackColor)) {
                    e.Graphics.FillEllipse(brush, a.X - r, a.Y - r, 2 * r, 2 * r);
                    e.Graphics.FillEllipse(brush, d.X - r, d.Y - r, 2 * r, 2 * r);
                }
            }
        }

        /// <summary>
        /// Draws selected spline info including ends number descriptions (active selected or specified explicitly)
        /// </summary>
        /// <param name="e"></param>
        /// <param name="spline"></param>
        private void DrawSelectedTrackInfo(PaintEventArgs e, Spline spline) {
            if (spline != null) {
                var track = spline.Track;
                var lineWidth = T.Scale * spline.W;
                // Line ends description:
                var a = MapToDisplay(spline.A);
                var d = MapToDisplay(spline.D);
                var indexA = 0;
                var indexD = 0;
                if (spline.A == (PointF)track.Point1) indexA = 1;
                else if (spline.A == (PointF)track.Point2) indexA = 2;
                else if (track.Point3 != null && spline.A == (PointF)track.Point3) indexA = 3;
                else if (track.Point4 != null && spline.A == (PointF)track.Point4) indexA = 4;
                if (spline.D == (PointF)track.Point1) indexD = 1;
                else if (spline.D == (PointF)track.Point2) indexD = 2;
                else if (track.Point3 != null && spline.D == (PointF)track.Point3) indexD = 3;
                else if (track.Point4 != null && spline.D == (PointF)track.Point4) indexD = 4;
                var a1 = new PointF(a.X - 0.55f * CharSize.Width, a.Y - 0.55f * CharSize.Height);
                var d1 = new PointF(d.X - 0.55f * CharSize.Width, d.Y - 0.55f * CharSize.Height);
                using (var brush = new SolidBrush(BackColor)) {
                    e.Graphics.DrawString(indexA.ToString(), Font, brush, a1);
                    e.Graphics.DrawString(indexD.ToString(), Font, brush, d1);
                }
                // Track info:
                var infoHeaderPosition = new PointF(Width - CharSize.Width * 60, Height - CharSize.Height * 8);
                var infoPosition = new PointF(Width - CharSize.Width * 60, Height - CharSize.Height * 7);
                var events = new StringBuilder();
                if (track.Event0 != null) { events.Append("Event0 = "); events.AppendLine(track.Event0); }
                if (track.Event1 != null) { events.Append("Event1 = "); events.AppendLine(track.Event1); }
                if (track.Event2 != null) { events.Append("Event2 = "); events.AppendLine(track.Event2); }
                if (track.Eventall0 != null) { events.Append("Eventall0 = "); events.AppendLine(track.Eventall0); }
                if (track.Eventall1 != null) { events.Append("Eventall1 = "); events.AppendLine(track.Eventall1); }
                if (track.Eventall2 != null) { events.Append("Eventall2 = "); events.AppendLine(track.Eventall2); }
                if (track.Velocity != null) { events.Append("Velocity = "); events.AppendLine(track.Velocity.ToString()); }
                var bold = new Font(Font, FontStyle.Bold);
                var header = Messages.TrackSelected;
                var info = String.Format(
                    Messages.TrackInfo,
                    track.Name ?? "none", track.SourcePath, track.SourceIndex, track.Quality, events.ToString()
                );
                using (var brush = new SolidBrush(DetailTextColor)) {
                    e.Graphics.DrawString(header, bold, brush, infoHeaderPosition);
                    e.Graphics.DrawString(info, Font, brush, infoPosition);
                }
            }
        }

        /// <summary>
        /// Draws selected elements
        /// </summary>
        /// <param name="e"></param>
        private void DrawSelection(PaintEventArgs e) {
            if (!Selection.Empty)
                Selection.ForEach(
                    a => {
                        if (a is Spline) {
                            var spline = a as Spline;
                            DrawSelectedSpline(e, spline);
                            DrawSelectedSplineEnds(e, spline);
                        }
                    },
                    b => {
                        if (b is Spline) {
                            var spline = b as Spline;
                            DrawSelectedSpline(e, spline);
                            DrawSelectedSplineEnds(e, spline);
                            DrawSelectedTrackInfo(e, spline);
                        }
                    }
                );
        }

        /// <summary>
        /// Draws map grid starting at map (0, 0) point
        /// </summary>
        /// <param name="e"></param>
        private void DrawGrid(PaintEventArgs e) {
            var d = GridSize; // grid division size (in meters)
            var m = Splines.Bounds; // full map rectangle
            var v = VisibleArea; // visible map rectangle
            var pen = new Pen(GridColor);
            var pen0 = new Pen(GridZeroColor, T.Scale);
            var p = pen0;
            for (
                    float x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                    x1 > m.Left || x2 < m.Right || y1 > m.Top || y2 < m.Bottom;
                    x1 -= d, x2 += d, y1 -= d, y2 += d
                ) {
                if (y1 > m.Top) e.Graphics.DrawLine(p, MapToDisplay(m.Left, y1), MapToDisplay(m.Right, y1));
                if (y2 != y1 && y2 < m.Bottom) e.Graphics.DrawLine(p, MapToDisplay(m.Left, y2), MapToDisplay(m.Right, y2));
                if (x1 > m.Left) e.Graphics.DrawLine(p, MapToDisplay(x1, m.Top), MapToDisplay(x1, m.Bottom));
                if (x2 != x1 && x2 < m.Right) e.Graphics.DrawLine(p, MapToDisplay(x2, m.Top), MapToDisplay(x2, m.Bottom));
                p = pen;
            }
            pen0.Dispose();
            pen.Dispose();
        }

        /// <summary>
        /// Draws dots marking track ends
        /// </summary>
        /// <param name="e"></param>
        private void DrawDots(PaintEventArgs e) {
            var pens = new[] {
                new Pen(TrackColor),
                new Pen(SwitchColor),
                new Pen(RoadColor),
                new Pen(RiverColor),
                new Pen(CrossColor)
            };
            var dotFill = new SolidBrush(DotColor);
            var b = Splines.Bounds;
            var g = e.Graphics;
            var sc = CurrentScale;
            var xo = b.Left * sc;
            var yo = b.Top * sc;
            VisibleSplines.ForEach(s => {
                var p = pens[s.T];
                var w = CurrentScale * s.W * 0.5f;
                var v = w / 2;
                var p1 = MapToDisplay(s.A);
                var p2 = MapToDisplay(s.D);
                g.DrawEllipse(p, p1.X - v, p1.Y - v, w, w);
                g.FillEllipse(dotFill, p1.X - v, p1.Y - v, w, w);
                g.DrawEllipse(p, p2.X - v, p2.Y - v, w, w);
                g.FillEllipse(dotFill, p2.X - v, p2.Y - v, w, w);
            });
            foreach (var p in pens) p.Dispose();
            dotFill.Dispose();
        }

        /// <summary>
        /// Draws control's message
        /// </summary>
        /// <param name="e"></param>
        private void DrawMessage(PaintEventArgs e) {
            if (!String.IsNullOrEmpty(Message)) e.Graphics.DrawString(Message, Font, new SolidBrush(ForeColor), new PointF(10f, 10f));
        }

        /// <summary>
        /// OnPaint event handler
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            var cs = e.Graphics.MeasureString("1234", Font);
            CharSize = new SizeF(cs.Width / 4, cs.Height);
            if (Splines != null) {
                UpdateTransformation();
                if (ShowGrid) DrawGrid(e);
                DrawTracks(e);
                if (ShowDots) DrawDots(e);
                DrawSelection(e);
            } else DrawMessage(e);
        }

        #endregion

        #region Event handling

        /// <summary>
        /// Handles control's Resize event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e) {
            if (Splines != null) {
                var outFull = IsOutFull;
                FitScale = GetFitScale();
                if (outFull) CurrentScale = FitScale;
            }
            CtlCenter = new PointF(Width / 2, Height / 2);
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Handles control's MouseWheel event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e) {
            var s0 = CurrentScale;
            var s1 = CurrentScale;
            var sf = FitScale;
            var c0 = new PointF(e.X - CtlCenter.X, e.Y - CtlCenter.Y); // relative control position of cursor (in pixels)
            var m0 = new PointF(c0.X / s0 - Offset.X, c0.Y / s0 - Offset.Y); // relative map position of cursor
            if (e.Delta > 0 && s0 < 30) s1 *= 1.5f;
            if (e.Delta < 0 && s0 > sf) s1 /= 1.5f;
            if (s1 != s0) {
                CurrentScale = s1;
                Offset = new PointF(c0.X / s1 - m0.X, c0.Y / s1 - m0.Y);
                Invalidate();
                if (ScaleChanged != null) ScaleChanged.Invoke(this, EventArgs.Empty);
            }
        }
        
        /// <summary>
        /// Point on the map when the mouse button was pressed
        /// </summary>
        PointF P0;
        /// <summary>
        /// After the mouse button is pressed and before it's released the map is in the move mode
        /// </summary>
        bool MoveMode;
        /// <summary>
        /// True, if the map was moved between mouse button was pressed and released
        /// </summary>
        bool MapMoved;
        
        /// <summary>
        /// Handles control's MouseDown event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e) {
            var s = CurrentScale;
            var c = new PointF(e.X - CtlCenter.X, e.Y - CtlCenter.Y);
            P0 = new PointF(c.X / s - Offset.X, c.Y / s - Offset.Y);
            System.Diagnostics.Debug.Print(String.Format("p = ({0} ; {1})", P0.X, P0.Y));
            MoveMode = true;
            MapMoved = false;
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles control's MouseUp event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e) {
            MoveMode = false;
            Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Handles control's MouseMove event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e) {
            var s = CurrentScale;
            var c = new PointF(e.X - CtlCenter.X, e.Y - CtlCenter.Y);
            var p = new PointF(c.X / s - Offset.X, c.Y / s - Offset.Y);
            var d = new PointF(p.X - P0.X, p.Y - P0.Y);
            if (MoveMode && d != PointF.Empty) {
                Cursor = Cursors.SizeAll;
                Offset = new PointF(Offset.X + d.X, Offset.Y + d.Y);
                if (PositionChanged != null) PositionChanged.Invoke(this, EventArgs.Empty);
                Invalidate();
                MapMoved = true;
            }
            MapPointed = new PointF(-p.X - MapCenter.X, -p.Y - MapCenter.Y);
            if (CursorPositionChanged != null) CursorPositionChanged.Invoke(this, EventArgs.Empty);
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles control's MouseClick event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e) {
            if (!MapMoved) {
                if (!Control.ModifierKeys.HasFlag(Keys.Shift)) Selection.Clear();
                if (!Control.ModifierKeys.HasFlag(Keys.Control)) {
                    // Track clicks:
                    Selection.Add(Splines.GetNearest(CursorPosition, 0, VisibleArea));
                } else {
                    // Signal clicks:
                }

                base.OnMouseClick(e);
            }

        }

        /// <summary>
        /// Handles control's DoubleClick event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoubleClick(EventArgs e) {
            MoveMode = false;
            MapMoved = false;
            if (!Control.ModifierKeys.HasFlag(Keys.Control)) {
                if (!Selection.Empty) {
                    FindCurrentSelectionInFiles();
                } else base.OnDoubleClick(e);
            } else {
                base.OnDoubleClick(e);
            }
        }

        #endregion

        #region Code interaction

        /// <summary>
        /// Finds current selection in scenery files
        /// </summary>
        private void FindCurrentSelectionInFiles() {
            if (Selection.Current is Spline) { // Track double clicks:
                ProjectFile.FindTrack((Selection.Current as Spline).Track);
            } else { // Signal double clicks:

            }
        }

        internal void FindCodeFragmentOnMap(EditorFile file) {
            var text = file.Text;
            var offset = file.Editor.SelectionStart;
        }

        #endregion

        #region Internal data structures

        struct Transformation {
            public float Scale;
            public float X;
            public float Y;
        }

        struct Layer {
            public int Type;
            public Color Color;
            public Layer(int type, Color color) { Type = type; Color = color; }
        }

        public class SelectionClass {

            private int Index = -1;
            private List<object> Data = new List<object>();
            private Control Ctl;

            public bool Empty { get { return Data.Count < 1; } }

            public object Last { get { return Data.Count > 0 ? Data[Data.Count - 1] : null; } }

            public object Current { get { return Data[Index]; } }

            public SelectionClass(Control ctl) { Ctl = ctl; }

            public void Add(object item) {
                Index = Data.IndexOf(item);
                if (Index < 0) {
                    Data.Add(item);
                    Index = Data.Count - 1;
                }
                Ctl.Invalidate();
            }

            public void Remove(object item) {
                var current = Current;
                Data.Remove(item);
                Index = Data.IndexOf(current);
                Ctl.Invalidate();
            }

            public void Clear() { Data.Clear(); Index = -1; Ctl.Invalidate(); }

            public void ForEach(Action<object> action) { Data.ForEach(action); }

            public void ForEach(Action<object> actionForAllButCurrent, Action<object> actionForCurrent) {
                for (int i = 0; i < Data.Count; i++) if (i != Index) actionForAllButCurrent(Data[i]);
                actionForCurrent(Data[Index]);
            }

        }

        #region Splines

        /// <summary>
        /// A single, 2D, floating point track segment for drawing and on screen manipulation
        /// ALL COORDINATES ARE REVERSE TERRAIN COORDINATES
        /// </summary>
        class Spline {

            #region Fields

            public ScnTrack Track; // track reference
            public int T; // track type: 0 - normal, 1 - switch, 2 - road, 3 - river, 4 - cross
            public bool L; // true for straight line
            public float W; // track width
            public PointF A; // spline start point
            public PointF B; // spline control point 1
            public PointF C; // spline control point 2
            public PointF D; // spline end point
            public PointF I1; // interpolated point 1
            public PointF I2; // interpolated point 2
            public PointF I3; // interpolated point 3

            #endregion

            /// <summary>
            /// Returns true if any spline point is contained within specified rectangle
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public bool HasPointIn(RectangleF r) {
                return r.Contains(A) || r.Contains(I1) || r.Contains(I2) || r.Contains(I3) || r.Contains(D);
            }

            /// <summary>
            /// Exact or interpolated distance to specified point (exact if the spline is a straight line)
            /// </summary>
            /// <param name="point"></param>
            /// <returns></returns>
            public float DistanceToPoint(PointF point) {
                var a = (V2D)(this.A);
                var b = (V2D)(this.D);
                var c = new V2D(point.X, point.Y);
                if (this.L) return (float)V2D.LineToPointDistance(a, b, c);
                else {
                    var d = -1f;
                    var e = 0f;
                    var i1 = (V2D)this.I1;
                    var i2 = (V2D)this.I2;
                    var i3 = (V2D)this.I3;
                    d = (float)V2D.LineToPointDistance(a, i1, c);
                    e = (float)V2D.LineToPointDistance(i1, i2, c); if (e < d) d = e;
                    e = (float)V2D.LineToPointDistance(i2, i3, c); if (e < d) d = e;
                    e = (float)V2D.LineToPointDistance(i3, b, c); if (e < d) d = e;
                    return d;
                }
            }

            /// <summary>
            /// Creates TrackSpline object from ScnTrack
            /// </summary>
            /// <param name="track"></param>
            /// <param name="switchState"></param>
            /// <returns></returns>
            public static Spline FromTrack(ScnTrack track, bool switchState = false) {
                var p1 = !switchState;
                var spline = new Spline {
                    A = p1 ? track.Point1 : track.Point3,
                    B = p1 ? track.Point1 + track.CVec1 : track.Point3 + track.CVec3,
                    C = p1 ? track.Point2 + track.CVec2 : track.Point4 + track.CVec4,
                    D = p1 ? track.Point2 : track.Point4,
                };
                var type = 0;
                switch (track.TrackType.ToLowerInvariant()) {
                    case "normal": type = 0; break;
                    case "switch": type = 1; break;
                    case "road": type = 2; break;
                    case "river": type = 3; break;
                    case "cross": type = 4; break;
                    default: type = 0; break;
                }
                spline.T = type;
                spline.W = (float)track.TrackWidth;
                spline.L = p1 ? track.CVec1.Zero && track.CVec2.Zero : track.CVec3.Zero && track.CVec4.Zero;
                spline.I1 = spline.Interpolate(0.25f);
                spline.I2 = spline.Interpolate(0.5f);
                spline.I3 = spline.Interpolate(0.75f);
                spline.Track = track;
                return spline;
            }

            /// <summary>
            /// Interpolates a point on spline
            /// </summary>
            /// <param name="t">0..1</param>
            /// <returns></returns>
            private PointF Interpolate(float t) {
                return
                    (A == B && C == D)
                        ? new PointF( // parametric straight equation
                                A.X * (1 - t) + D.X * t,
                                A.Y * (1 - t) + D.Y * t
                            )
                        : new PointF( // parametric cubic Bezier equation
                                A.X + 3 * t * (B.X - A.X) + 3 * t * t * (C.X - 2 * B.X + A.X) + t * t * t * (D.X - 3 * C.X + 3 * B.X - A.X),
                                A.Y + 3 * t * (B.Y - A.Y) + 3 * t * t * (C.Y - 2 * B.Y + A.Y) + t * t * t * (D.Y - 3 * C.Y + 3 * B.Y - A.Y)
                            );
            }

        }

        /// <summary>
        /// A list of track splines for drawing and on screen manipulation
        /// </summary>
        class SplineCollection : List<Spline> {
            
            /// <summary>
            /// Temporary Bounds rectangle
            /// </summary>
            RectangleF _Bounds;
            
            /// <summary>
            /// True if _Bounds are calculated
            /// </summary>
            bool _BoundsSet;

            /// <summary>
            /// Source track list
            /// </summary>
            public ScnTrackCollection Tracks;

            /// <summary>
            /// Gets the smallest rectangle defining the area where all track splines are contained in
            /// IN REVERSE TERRAIN COORDINATES
            /// </summary>
            public RectangleF Bounds {
                get {
                    if (!_BoundsSet) { _Bounds = GetBounds(); _BoundsSet = true; }
                    return _Bounds;
                }
            }

            /// <summary>
            /// Creates track spline list instance for the track list specified
            /// </summary>
            /// <param name="tracks"></param>
            public SplineCollection(ScnTrackCollection tracks) {
                var tc = tracks.Count;
                Capacity = (int)Math.Round(1.1 * tc, 0);
                tracks.ForEach(t => { Add(Spline.FromTrack(t)); if (t.IsSwitch) Add(Spline.FromTrack(t, true)); });
                Tracks = tracks;
            }

            /// <summary>
            /// Returns the smallest rectangle all splines are contained in
            /// </summary>
            /// <returns>IN INVERSE TERRAIN COORDINATES</returns>
            private RectangleF GetBounds() {
                float x = 0, left = 0, top = 0, right = 0, bottom = 0;
                foreach (var spline in this) {
                    if ((x = spline.A.X)  < left) left = x;
                    if ((x = spline.I1.X) < left) left = x;
                    if ((x = spline.I2.X) < left) left = x;
                    if ((x = spline.I3.X) < left) left = x;
                    if ((x = spline.D.X)  < left) left = x;
                    if ((x = spline.A.X)  > right) right = x;
                    if ((x = spline.I1.X) > right) right = x;
                    if ((x = spline.I2.X) > right) right = x;
                    if ((x = spline.I3.X) > right) right = x;
                    if ((x = spline.D.X)  > right) right = x;
                    if ((x = spline.A.Y)  < top) top = x;
                    if ((x = spline.I1.Y) < top) top = x;
                    if ((x = spline.I2.Y) < top) top = x;
                    if ((x = spline.I3.Y) < top) top = x;
                    if ((x = spline.D.Y)  < top) top = x;
                    if ((x = spline.A.Y)  > bottom) bottom = x;
                    if ((x = spline.I1.Y) > bottom) bottom = x;
                    if ((x = spline.I2.Y) > bottom) bottom = x;
                    if ((x = spline.I3.Y) > bottom) bottom = x;
                    if ((x = spline.D.Y)  > bottom) bottom = x;
                }
                return new RectangleF(left, top, right - left, bottom - top);
            }

            /// <summary>
            /// Returns the track nearest to the specified point if the distance is within specified range
            /// </summary>
            /// <param name="point"></param>
            /// <param name="range"></param>
            /// <returns></returns>
            public Spline GetNearest(PointF point, float range, RectangleF within) {
                int count = Count, index = 0;
                float d;
                float distance = -1f;
                for (var i = 0; i < count; i++) {
                    if (within == RectangleF.Empty || this[i].HasPointIn(within)) {
                        d = this[i].DistanceToPoint(point);
                        if (distance < 0f || d < distance) { distance = d; index = i; }
                    }
                }
                var spline = this[index];
                if (range == 0f) range = spline.W * 2;
                return range < 0 || distance <= range ? spline : null;
            }

            /// <summary>
            /// Returns the track nearest to the specified point if the distance is within specified range
            /// </summary>
            /// <param name="point"></param>
            /// <param name="range"></param>
            /// <returns></returns>
            public Spline GetNearest(PointF point, float range = 0f) {
                return GetNearest(point, range, RectangleF.Empty);
            }
            

        }


        #endregion

        #endregion

    }

}