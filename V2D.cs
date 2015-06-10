using System;
using System.Drawing;

/// <summary>
/// 2D vector class
/// </summary>
public class V2D {
    public double X;
    public double Y;
    public double Length { get { return Math.Sqrt(X * X + Y * Y); } }
    public bool Zero { get { return X == 0 && Y == 0; } }
    public V2D() { }
    public V2D(double x, double y) { X = x; Y = y; }
    public override int GetHashCode() { return base.GetHashCode(); }
    public override bool Equals(object obj) {
        return base.Equals(obj as V2D == this);
    }
    public static bool operator ==(V2D a, V2D b) {
        if (ReferenceEquals(b, null)) return ReferenceEquals(a, null);
        return a.X == b.X && a.Y == b.Y;
    }
    public static bool operator !=(V2D a, V2D b) {
        if (ReferenceEquals(b, null)) return !ReferenceEquals(a, null);
        return a.X != b.X || a.Y != b.Y;
    }
    public static bool operator >=(V2D a, V2D b) { return a.Length >= b.Length; }
    public static bool operator <=(V2D a, V2D b) { return a.Length <= b.Length; }
    public static bool operator >(V2D a, V2D b) { return a.Length > b.Length; }
    public static bool operator <(V2D a, V2D b) { return a.Length < b.Length; }
    public static V2D operator +(V2D a, V2D b) { return new V2D(a.X + b.X, a.Y + b.Y); }
    public static V2D operator -(V2D a, V2D b) { return new V2D(a.X - b.X, a.Y - b.Y); }
    public static V2D operator -(V2D a) { return new V2D(-a.X, -a.Y); }
    public static V2D operator +(V2D a) { return new V2D(+a.X, +a.Y); }
    public static V2D operator *(V2D a, double k) { return new V2D(k * a.X, k * a.Y); }
    public static V2D operator *(double k, V2D a) { return new V2D(k * a.X, k * a.Y); }
    public static V2D operator /(V2D a, double k) { return new V2D(a.X / k, a.Y / k); }
    public static V2D operator /(double k, V2D a) { return new V2D(k / a.X, k / a.Y); }
    public double Dot(V2D a) { return X * a.X + Y * a.Y; }
    public static V2D Interpolate(V2D a, V2D b, double t) { return new V2D(a.X * (1 - t) + b.X * t, a.Y * (1 - t) + b.Y * t); }
    public static double LineToPointDistance(V2D a, V2D b, V2D c) {
        var d = (b - a).Length;
        var d2 = d * d;
        if (d2 == 0d) return (c - a).Length;
        var t = ((c - a).Dot(b - a)) / d2;
        if (t < 0d) return (c - a).Length;
        else if (t > 1d) return (c - b).Length;
        var e = a + t * (b - a);
        return (e - c).Length;
    }
    public static implicit operator PointF(V2D v) { return new PointF(-(float)v.X, -(float)v.Y); }
    public static implicit operator V2D(PointF p) { return new V2D(-p.X, -p.Y); }
}