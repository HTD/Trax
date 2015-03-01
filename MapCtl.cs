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

namespace ScnEdit {
    public partial class MapCtl : UserControl {

        #region Map properties

        [Category("Appearance")]
        public Color TrackColor { get; set; }
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
        public Color DotColor { get; set; }
        [Category("Appearance")]
        public bool ShowGrid { get; set; }
        [Category("Appearance")]
        public bool ShowDots { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new float Scale {
            get {
                return _CurrentScale;
            }
            set {
                _CurrentScale = value;
                Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF MapOffset {
            get {
                return _MapOffset;
            }
            set {
                _MapOffset = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PointF CursorPosition {
            get {
                return _MapPointed;
            }
        }

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ScnTracks Tracks {
            get { if (Splines == null) return null; return Splines.Tracks; }
            set { if (value == null) return; Splines = new TrackSplines(value); _FitScale = _CurrentScale = GetFitScale(); Invalidate(); }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OutFull { get { return _CurrentScale == _FitScale; } }

        #endregion

        #region Map events

        public event EventHandler Loaded;
        public event EventHandler ScaleChanged;
        public event EventHandler OffsetChanged;
        public event EventHandler CursorPositionChanged;

        #endregion

        #region Private fields

        string _Message;
        float _FitScale;
        float _CurrentScale;
        float _GridSize = 100f;
        TrackSplines Splines;
        TrackSpline[] VisibleSplines;
        RectangleF _Bounds;
        RectangleF _Viewport;
        PointF _CtlCenter;
        PointF _MapCenter;
        PointF _MapOffset;
        PointF _MapPointed;
        MapLayer[] _Layers;

        #endregion

        public MapCtl() {
            SuspendLayout();
            Dock = DockStyle.Fill;
            Font = new Font(Font.FontFamily, 15f);
            BackColor = ColorTranslator.FromHtml("#233e3b"); 
            ForeColor = ColorTranslator.FromHtml("#1d3331");
            TrackColor = Color.Black;
            SwitchColor = Color.Orange;
            RoadColor = ColorTranslator.FromHtml("#1d3331");
            RiverColor = ColorTranslator.FromHtml("#2d536f");
            CrossColor = RoadColor;
            GridColor = ColorTranslator.FromHtml("#ddeddd");
            DotColor = ColorTranslator.FromHtml("#ff2200");
            ShowDots = true;
            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            ResumeLayout();
            _FitScale = _CurrentScale = 0.01f;
            _CtlCenter = new PointF(Width / 2, Height / 2);
            BeginLoad();
        }

        private void BeginLoad() {
            Enabled = false;
            _Message = "Loading data...";
            using (var w = new BackgroundWorker()) {
                w.DoWork += Load;
                w.RunWorkerCompleted += LoadingDone;
                w.RunWorkerAsync();
            }
        }

        private new void Load(object sender, DoWorkEventArgs e) {
            try {
                if (!DesignMode) {
                    Splines = new TrackSplines(ScnTracks.Load());
                    _Bounds = Splines.Bounds;
                }
                _Message = null;
            } catch (Exception x) {
                _Message = x.Message;
            }
        }

        private void LoadingDone(object sender, RunWorkerCompletedEventArgs e) {
            if (Splines != null) {
                Enabled = true;
                ResetView();
                if (Loaded != null) Loaded.Invoke(this, EventArgs.Empty);
            }
        }

        private object LayerLock = new Object();

        public void ResetView() {
            lock (LayerLock) {
                _Layers = new[] {
                    new MapLayer(3, RiverColor),
                    new MapLayer(4, CrossColor),
                    new MapLayer(2, RoadColor),
                    new MapLayer(1, SwitchColor),
                    new MapLayer(0, TrackColor)
                };
            }
            _FitScale = _CurrentScale = GetFitScale();
            _MapCenter = new PointF(Splines.Bounds.X + Splines.Bounds.Width / 2, Splines.Bounds.Y + Splines.Bounds.Height / 2);
            _MapOffset = new PointF(0, 0);
            Invalidate();
            if (ScaleChanged != null) ScaleChanged.Invoke(this, EventArgs.Empty);
            if (OffsetChanged != null) OffsetChanged.Invoke(this, EventArgs.Empty);
        }

        private float[] GetTransformation() {
            return new float[] {
                _CurrentScale,
                _CtlCenter.X + (_MapOffset.X - _MapCenter.X) * _CurrentScale,
                _CtlCenter.Y + (_MapOffset.Y - _MapCenter.Y) * _CurrentScale
            };
        }

        private float GetFitScale() {
            var sx = Bounds.Width / _Bounds.Width;
            var sy = Bounds.Height / _Bounds.Height;
            return new[] { sx, sy }.Min();
        }

        private void DrawMap(PaintEventArgs e) {
            if (_Layers == null) return;
            var b = Splines.Bounds;
            var g = e.Graphics;
            var f = true; // OutFull;
            var tr = GetTransformation();
            float ts = tr[0], tx = tr[1], ty = tr[2];
            TrackSpline[] visible = !f ? Splines.GetSubset(ts, _Viewport) : null;
            var ct = f ? Splines.Count : visible.Length;
            lock (LayerLock) {
                foreach (var layer in _Layers) {
                    using (var pen = new Pen(layer.Color)) {
                        for (int i = 0; i < ct; i++) {
                            TrackSpline s = f ? Splines[i] : visible[i];
                            if (s.T == layer.Type) {
                                pen.Width = ts * s.W;
                                if (ts < 0.01 || s.L) {
                                    var p1 = new PointF(s.A.X * ts + tx, s.A.Y * ts + ty);
                                    var p2 = new PointF(s.D.X * ts + tx, s.D.Y * ts + ty);
                                    g.DrawLine(pen, p1, p2);
                                } else {
                                    var p1 = new PointF(s.A.X * ts + tx, s.A.Y * ts + ty);
                                    var p2 = new PointF(s.B.X * ts + tx, s.B.Y * ts + ty);
                                    var p3 = new PointF(s.C.X * ts + tx, s.C.Y * ts + ty);
                                    var p4 = new PointF(s.D.X * ts + tx, s.D.Y * ts + ty);
                                    var b1c = Math.Abs(p1.X - p2.X) >= 0.1f || Math.Abs(p1.Y - p2.Y) > 0.1f;
                                    var b2c = Math.Abs(p3.X - p4.X) >= 0.1f || Math.Abs(p3.Y - p4.Y) > 0.1f;
                                    if (b1c && b2c) g.DrawBezier(pen, p1, p2, p3, p4); else g.DrawLine(pen, p1, p4);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawGrid(PaintEventArgs e) {
            var g = e.Graphics;
            var b = Splines.Bounds;
            var t = GetTransformation();
            var d = 100;
            var s = t[0] * d;
            if (s > 3) {
                var dx_m = _MapOffset.X;
                var dy_m = _MapOffset.Y;
                var dx_i = (int)Math.Floor(dx_m / d);
                var dy_i = (int)Math.Floor(dy_m / d);
                var dx_r = dx_m - (d * dx_i);
                var dy_r = dy_m - (d * dy_i);
                var o = new PointF(dx_r * s / d, dy_r * s / d);
                using (var pen = new Pen(GridColor)) {
                    for (float y = -s; y < Height + s; y += s) g.DrawLine(pen, 0, y + o.Y, Width, y + o.Y);
                    for (float x = -s; x < Width + s; x += s) g.DrawLine(pen, x + o.X, 0, x + o.X, Height);
                }
            }
        }

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
            var f = true; // OutFull;
            var sc = _CurrentScale;
            var xo = b.Left * sc;
            var yo = b.Top * sc;
            var tr = GetTransformation();
            float ts = tr[0], tx = tr[1], ty = tr[2];
            TrackSpline[] visible = !f ? Splines.GetSubset(sc, _Viewport) : null;
            var ct = f ? Splines.Count : visible.Length;
            for (int i = 0; i < ct; i++) {
                TrackSpline s = f ? Splines[i] : visible[i];
                var p = pens[s.T];
                var w = _CurrentScale * s.W * 0.5f;
                var v = w / 2;
                var p1 = new PointF(s.A.X * ts + tx, s.A.Y * ts + ty);
                var p2 = new PointF(s.D.X * ts + tx, s.D.Y * ts + ty);
                g.DrawEllipse(p, p1.X - v, p1.Y - v, w, w);
                g.FillEllipse(dotFill, p1.X - v, p1.Y - v, w, w);
                g.DrawEllipse(p, p2.X - v, p2.Y - v, w, w);
                g.FillEllipse(dotFill, p2.X - v, p2.Y - v, w, w);
            }
            foreach (var p in pens) p.Dispose();
            dotFill.Dispose();
        }

        private void DrawMessage(PaintEventArgs e) {
            if (!String.IsNullOrEmpty(_Message)) e.Graphics.DrawString(_Message, Font, new SolidBrush(ForeColor), new PointF(10f, 10f));
        }

        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            if (Splines != null) {
                if (ShowGrid) DrawGrid(e);
                DrawMap(e);
                if (ShowDots) DrawDots(e);
            } else DrawMessage(e);
        }

        protected override void OnSizeChanged(EventArgs e) {
            if (Splines != null) {
                var outFull = OutFull;
                _FitScale = GetFitScale();
                if (outFull) _CurrentScale = _FitScale;
            }
            _CtlCenter = new PointF(Width / 2, Height / 2);
            base.OnSizeChanged(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e) {
            var s0 = _CurrentScale;
            var s1 = _CurrentScale;
            var sf = _FitScale;
            var c0 = new PointF(e.X - _CtlCenter.X, e.Y - _CtlCenter.Y); // relative control position of cursor (in pixels)
            var m0 = new PointF(c0.X / s0 - _MapOffset.X, c0.Y / s0 - _MapOffset.Y); // relative map position of cursor
            if (e.Delta > 0 && s0 < 30) s1 *= 1.5f;
            if (e.Delta < 0 && s0 > sf) s1 /= 1.5f;
            if (s1 != s0) {
                _CurrentScale = s1;
                _MapOffset = new PointF(c0.X / s1 - m0.X, c0.Y / s1 - m0.Y);
                Invalidate();
                if (ScaleChanged != null) ScaleChanged.Invoke(this, EventArgs.Empty);
            }
        }

        PointF P0;
        bool MoveMapMode;
        protected override void OnMouseDown(MouseEventArgs e) {
            var s = _CurrentScale;
            var c = new PointF(e.X - _CtlCenter.X, e.Y - _CtlCenter.Y);
            P0 = new PointF(c.X / s - _MapOffset.X, c.Y / s - _MapOffset.Y);
            System.Diagnostics.Debug.Print(String.Format("p = ({0} ; {1})", P0.X, P0.Y));
            
            MoveMapMode = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            MoveMapMode = false;
            Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            var s = _CurrentScale;
            var c = new PointF(e.X - _CtlCenter.X, e.Y - _CtlCenter.Y);
            var p = new PointF(c.X / s - _MapOffset.X, c.Y / s - _MapOffset.Y);
            var d = new PointF(p.X - P0.X, p.Y - P0.Y);
            if (MoveMapMode && d != PointF.Empty) {
                Cursor = Cursors.SizeAll;
                _MapOffset = new PointF(_MapOffset.X + d.X, _MapOffset.Y + d.Y);
                if (OffsetChanged != null) OffsetChanged.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
            _MapPointed = new PointF(-p.X - _MapCenter.X, -p.Y - _MapCenter.Y);
            if (CursorPositionChanged != null) CursorPositionChanged.Invoke(this, EventArgs.Empty);
            base.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            var s = _CurrentScale;
            var c = new PointF(e.X - _CtlCenter.X, e.Y - _CtlCenter.Y);
            var p = new PointF(c.X / s - _MapOffset.X - _MapCenter.X, c.Y / s - _MapOffset.Y - _MapCenter.Y);

        }

        struct MapLayer {
            public int Type;
            public Color Color;
            public MapLayer(int type, Color color) { Type = type; Color = color; }
        }

        #region Track splines

        class TrackSplines : List<TrackSpline> {

            RectangleF _Bounds;
            bool _BoundsSet;

            public ScnTracks Tracks;

            public RectangleF Bounds {
                get {
                    if (!_BoundsSet) { _Bounds = GetBounds(); _BoundsSet = true; }
                    return _Bounds;
                }
            }

            public TrackSpline[] GetSubset(float scale, RectangleF bounds) {
                var sc = Count;
                var subset = new List<TrackSpline>(sc / 4);
                for (int i = 0; i < sc; i++) {
                    if (this[i].L) {
                        if (Belongs(this[i].A, scale, bounds) || Belongs(this[i].D, scale, bounds)) subset.Add(this[i]);
                    } else {
                        if (
                            Belongs(this[i].A, scale, bounds) ||
                            Belongs(this[i].I1, scale, bounds) ||
                            Belongs(this[i].I2, scale, bounds) ||
                            Belongs(this[i].I3, scale, bounds) ||
                            Belongs(this[i].D, scale, bounds)
                        ) subset.Add(this[i]);
                    }
                }
                return subset.ToArray();
            }

            public TrackSplines(ScnTracks tracks) {
                Tracks = tracks;
                var tc = Tracks.Count;
                Capacity = (int)Math.Round(1.1 * tc, 0);
                for (int i = 0; i < tc; i++) {
                    var t = Tracks[i];
                    PointF a, b, c, d;
                    a = t.Point1; b = t.Point1 + t.CVec1; c = t.Point2 + t.CVec2; d = t.Point2;
                    if (t.IsSwitch) {
                        Add(new TrackSpline {
                            I = i, T = 1, L = t.CVec1.Zero && t.CVec2.Zero, W = (float)t.TrackWidth,
                            A = a, B = b, C = c, D = d,
                            I1 = Interpolate(0.25f, a, b, c, d), I2 = Interpolate(0.5f, a, b, c, d), I3 = Interpolate(0.75f, a, b, c, d)
                        });
                        a = t.Point3; b = t.Point3 + t.CVec3; c = t.Point4 + t.CVec4; d = t.Point4;
                        Add(new TrackSpline {
                            I = i, T = 1, L = t.CVec3.Zero && t.CVec4.Zero, W = (float)t.TrackWidth,
                            A = a, B = b, C = c, D = d,
                            I1 = Interpolate(0.25f, a, b, c, d), I2 = Interpolate(0.5f, a, b, c, d), I3 = Interpolate(0.75f, a, b, c, d)
                        });
                    } else {
                        int tt = 0;
                        switch (t.TrackType.ToLowerInvariant()) {
                            case "road":  tt = 2; break;
                            case "river": tt = 3; break;
                            case "cross": tt = 4; break;
                            default: tt = 0; break;
                        }
                        Add(new TrackSpline {
                            I = i, T = tt, L = t.CVec1.Zero && t.CVec2.Zero, W = (float)t.TrackWidth,
                            A = a, B = b, C = c, D = d,
                            I1 = Interpolate(0.25f, a, b, c, d), I2 = Interpolate(0.5f, a, b, c, d), I3 = Interpolate(0.75f, a, b, c, d)
                        });
                    }
                }
            }

            private PointF Interpolate(float t, PointF a, PointF b, PointF c, PointF d) {
                return
                    (a == b && c == d)
                        ? new PointF(
                                a.X * (1 - t) + d.X * t,
                                a.Y * (1 - t) + d.Y * t
                            )
                        : new PointF(
                                a.X + 3 * t * (b.X - a.X) + 3 * t * t * (c.X - 2 * b.X + a.X) + t * t * t * (d.X - 3 * c.X + 3 * b.X - a.X),
                                a.Y + 3 * t * (b.Y - a.Y) + 3 * t * t * (c.Y - 2 * b.Y + a.Y) + t * t * t * (d.Y - 3 * c.Y + 3 * b.Y - a.Y)
                            );
            }

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

            private bool Belongs(PointF point, float scale, RectangleF bounds) {
                float x = point.X * scale;
                float y = point.Y * scale;
                return x >= bounds.Left && x <= bounds.Right && y >= bounds.Top && y <= bounds.Bottom;
            }

        }

        struct TrackSpline {
            public int I; // track index
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
        }

        #endregion

    }


}
