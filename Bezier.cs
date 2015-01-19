using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Quadratic Bézier curve calculation class
/// </summary>
public class Bezier2 {

    /// <summary>
    /// Start point
    /// </summary>
    public V3D A;
    /// <summary>
    /// Control point
    /// </summary>
    public V3D B;
    /// <summary>
    /// End point
    /// </summary>
    public V3D C;

    public Bezier2(V3D a, V3D b, V3D c) { A = a; B = b; C = c; }

    /// <summary>
    /// Interpolated point at t : 0..1 position
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public V3D P(double t) { return (1 - t) * (1 - t) * A + 2 * t * (1 - t) * B + t * t * C; }

    /// <summary>
    /// Integral calculation by Dave Eberly
    /// See: http://www.gamedev.net/topic/551455-length-of-a-generalized-quadratic-bezier-curve-in-3d/
    /// </summary>
    public double Length {
        get {
            V3D A0 = B - A;
            V3D A1 = A - 2 * B + C;
            if (!A1.Zero) {
                double c = 4 * A1.Dot(A1);
                double b = 8 * A0.Dot(A1);
                double a = 4 * A0.Dot(A0);
                double q = 4 * a * c - b * b;
                double twoCpB = 2 * c + b;
                double sumCBA = c + b + a;
                double mult0 = 0.25 / c;
                double mult1 = q / (8 * Math.Pow(c, 1.5));
                return
                    mult0 * (twoCpB * Math.Sqrt(sumCBA) - b * Math.Sqrt(a)) +
                    mult1 * (Math.Log(2 * Math.Sqrt(c * sumCBA) + twoCpB) - Math.Log(2 * Math.Sqrt(c * a) + b));
            } else return 2 * A0.Length;
        }
    }

}

/// <summary>
/// Cubic Bézier curve calculation class
/// </summary>
public class Bezier3 {

    public static double InterpolationPrecision = 0.001;

    #region Optimization constants

    protected static double Sqrt3 = Math.Sqrt(3);
    protected static double Div18Sqrt3 = 18 / Sqrt3;
    protected static double OneThird = 1 / 3;
    protected static double Sqrt3Div36 = Sqrt3 / 36;

    #endregion

    /// <summary>
    /// Start point
    /// </summary>
    public V3D A;
    /// <summary>
    /// Control point 1
    /// </summary>
    public V3D B;
    /// <summary>
    /// Control point 2
    /// </summary>
    public V3D C;
    /// <summary>
    /// End point
    /// </summary>
    public V3D D;

    public Bezier3(V3D a, V3D b, V3D c, V3D d) { A = a; B = b; C = c; D = d; }

    /// <summary>
    /// Interpolated point at t : 0..1 position
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public V3D P(double t) { return A + 3 * t * (B - A) + 3 * t * t * (C - 2 * B + A) + t * t * t * (D - 3 * C + 3 * B - A); }

    /// <summary>
    /// Control point for mid-point quadratic approximation
    /// </summary>
    private V3D Q { get { return (3 * C - D + 3 * B - A) / 4; } }

    /// <summary>
    /// Mid-point quadratic approximation
    /// </summary>
    public Bezier2 M { get { return new Bezier2(A, Q, D); } }

    /// <summary>
    /// Splits the curve at given position (t : 0..1)
    /// (De Casteljau's algorithm, see: http://caffeineowl.com/graphics/2d/vectorial/bezierintro.html)
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public Bezier3[] SplitAt(double t) {
        V3D a = V3D.Interpolate(A, B, t);
        V3D b = V3D.Interpolate(B, C, t);
        V3D c = V3D.Interpolate(C, D, t);
        V3D m = V3D.Interpolate(a, b, t);
        V3D n = V3D.Interpolate(b, c, t);
        V3D p = P(t);
        return new[] { new Bezier3(A, a, m, p), new Bezier3(p, n, c, D) };
    }

    /// <summary>
    /// The distance between 0 and 1 quadratic aproximations
    /// </summary>
    private double D01 { get { return (D - 3 * C + 3 * B - A).Length / 2; } }

    /// <summary>
    /// Split point for adaptive quadratic approximation
    /// </summary>
    private double Tmax { get { return Math.Pow(Div18Sqrt3 * InterpolationPrecision / D01, OneThird); } }

    /// <summary>
    /// Calculated length of the mid-point quadratic approximation
    /// </summary>
    public double QLength { 
        get {
            if (A == B && C == D) return (A - B).Length;
            return M.Length;
        }
    }

    /// <summary>
    /// Calculated length of adaptive quadratic approximation
    /// </summary>
    public double Length {
        get {
            if (A == B && C == D) return (A - B).Length;
            double tmax = 0;
            Bezier3 segment = this;
            List<Bezier3> segments = new List<Bezier3>();
            while ((tmax = segment.Tmax) < 1) {
                var split = segment.SplitAt(tmax);
                segments.Add(split[0]);
                segment = split[1];
            }
            segments.Add(segment);
            return segments.Sum(s => s.QLength);
        }
    }

}
