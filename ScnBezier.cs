using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnEdit {
    
    /// <summary>
    /// Basic cubic Bézier curve interpolation class
    /// </summary>
    class ScnBezier {

        public const double DefaultPrecision = 0.05; // 5cm

        public ScnPoint A;
        public ScnPoint B;
        public ScnPoint C;
        public ScnPoint D;

        /// <summary>
        /// Returns point of the curve for t = 0..1
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ScnPoint P(double t) {
            double t11 = 1 - t, t12 = t11 * t11, t13 = t12 * t11, t2 = t * t, t3 = t2 * t, _3tt12 = 3 * t * t12, _3t2t11 = 3 * t2 * t11;
            return t13 * A + _3tt12 * B + _3t2t11 * C + t3 * D;
        }

        /// <summary>
        /// Returns interpolated curve length
        /// </summary>
        /// <param name="precision">[m]</param>
        /// <returns></returns>
        public double Length(double precision = DefaultPrecision) {
            double dt = precision / A.DistanceTo(D), length = 0;
            for (double t = dt; t < 1; t += dt) length += new ScnVector(P(t - dt), P(t)).Length;
            return length;
        }
        
    }

}