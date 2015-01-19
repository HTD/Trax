using System;
using System.Drawing;

/// <summary>
/// 3D vector class
/// </summary>
public class V3D {
    public double X;
    public double Y;
    public double Z;
    public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
    public bool Zero { get { return X == 0 && Y == 0; } }
    public V3D(double x, double y, double z) { X = x; Y = y; Z = z; }
    public override int GetHashCode() { return base.GetHashCode(); }
    public override bool Equals(object obj) {
        return base.Equals(obj as V3D == this);
    }
    public static bool operator ==(V3D a, V3D b) {
        if (ReferenceEquals(b, null)) return ReferenceEquals(a, null);
        return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    }
    public static bool operator !=(V3D a, V3D b) {
        if (ReferenceEquals(b, null)) return !ReferenceEquals(a, null);
        return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    }
    public static bool operator >=(V3D a, V3D b) { return a.Length >= b.Length; }
    public static bool operator <=(V3D a, V3D b) { return a.Length <= b.Length; }
    public static bool operator >(V3D a, V3D b) { return a.Length > b.Length; }
    public static bool operator <(V3D a, V3D b) { return a.Length < b.Length; }
    public static V3D operator +(V3D a, V3D b) { return new V3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
    public static V3D operator -(V3D a, V3D b) { return new V3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
    public static V3D operator -(V3D a) { return new V3D(-a.X, -a.Y, -a.Z); }
    public static V3D operator +(V3D a) { return new V3D(+a.X, +a.Y, +a.Z); }
    public static V3D operator *(V3D a, double k) { return new V3D(k * a.X, k * a.Y, k * a.Z); }
    public static V3D operator *(double k, V3D a) { return new V3D(k * a.X, k * a.Y, k * a.Z); }
    public static V3D operator /(V3D a, double k) { return new V3D(a.X / k, a.Y / k, a.Z / k); }
    public static V3D operator /(double k, V3D a) { return new V3D(k / a.X, k / a.Y, k / a.Z); }
    public double Dot(V3D a) { return X * a.X + Y * a.Y + Z * a.Z; }
    public V3D Cross(V3D a) { return new V3D(Y * a.Z - Z * a.Y, Z * a.X - X * a.Z, X * a.Y - Y * a.X); }
    public static V3D Interpolate(V3D a, V3D b, double t) {
        return new V3D(
            a.X * (1 - t) + b.X * t,
            a.Y * (1 - t) + b.Y * t,
            a.Z * (1 - t) + b.Z * t);
    }
    public static implicit operator PointF(V3D v) { return new PointF(-(float)v.X, -(float)v.Z); }
}