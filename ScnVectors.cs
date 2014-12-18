using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnEdit {
    
    /// <summary>
    /// Generic scenery vector object
    /// </summary>
    class ScnVectorObject<T> {

        public ScnVector[] Vectors;

        public ScnVectorObject() { }
        public ScnVectorObject(ScnVector[] vectors) { Vectors = vectors; }

        public string ScnType;
        public string SourcePath;
        public int SourceIndex;
        public int SourceLength;

        public double DistanceTo(ScnPoint x0) {
            var n = Vectors.Length;
            var d = new double[n];
            for (int i = 0; i < n; i++) d[i] = Vectors[i].DistanceTo(x0);
            return d.Min();
        }

    }

    /// <summary>
    /// Generic scenery vector object list with search by coordinates capability
    /// </summary>
    class ScnVectorObjects<T> : List<T> where T : ScnVectorObject<T> {

        /// <summary>
        /// Returns scenery vector objects in proximity of given point
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public ScnVectorObjects<T> FindInProximity(ScnPoint x0, double distance = 10) {
            return (ScnVectorObjects<T>)this.Where(v => v.DistanceTo(x0) < distance).ToList();
        }


    }
    
    /// <summary>
    /// Generic scenery vector
    /// </summary>
    class ScnVector {

        public ScnPoint P1;
        public ScnPoint P2;
        public ScnPoint D { get { return new ScnPoint { X = P2.X - P1.X, Y = P2.Y - P1.Y, Z = P2.Z - P1.Z }; } }
        public ScnVector U {
            get {
                var d = D;
                var k = 1 / Math.Sqrt(d.X * d.X + d.Y * d.Y + d.Z * d.Z);
                return new ScnVector(new ScnPoint(), k * d);
            }
        }

        public double Length { get { return P1.DistanceTo(P2); } }

        #region Operators

        public static ScnVector operator +(ScnVector a, ScnVector b) {
            return new ScnVector(a.P1 + b.P1, a.P2 + b.P2);
        }

        public static ScnVector operator -(ScnVector a, ScnVector b) {
            return new ScnVector(a.P1 - b.P1, a.P2 - b.P2);
        }

        public static ScnVector operator *(double k, ScnVector v) {
            return new ScnVector(v.P1, k * v.P2);
        }

        public static ScnVector operator *(ScnVector v, double k) {
            return new ScnVector(v.P1, k * v.P2);
        }

        public static ScnVector operator /(double k, ScnVector v) {
            return new ScnVector(v.P1, k / v.P2);
        }

        public static ScnVector operator /(ScnVector v, double k) {
            return new ScnVector(v.P1, k / v.P2);
        }

        #endregion

        public ScnVector(ScnPoint x1, ScnPoint x2) { P1 = x1; P2 = x2; }

        public double DistanceTo(ScnPoint x0) {
            var a = this.Length;
            var b = new ScnVector(x0, P1).Length;
            var c = new ScnVector(x0, P2).Length;
            var s = 0.5 * (a + b + c);
            return 2 * Math.Sqrt(s * (s - a) * (s - b) * (s - c)) / a;
        }

    }

}