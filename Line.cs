using System;

public class Line {

    public V3D A = null;
    public V3D B = null;

    public double Length { get { return (A - B).Length; } }

    public double DistanceTo(V3D p) {
        var a = this.Length;
        var b = (p - A).Length;
        var c = (p - B).Length;
        var s = 0.5 * (a + b + c);
        return 2 * Math.Sqrt(s * (s - a) * (s - b) * (s - c)) / a;
    }

}