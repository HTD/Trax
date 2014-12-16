using System;
namespace ScnEdit {

    /// <summary>
    /// Scenery spatial point
    /// </summary>
    public class ScnPoint {

        /// <summary>
        /// A distance below which the points are considered linked
        /// </summary>
        private const double LinkDistance = 0.01;

        /// <summary>
        /// Horizontal coordinate on map
        /// </summary>
        public double X;

        /// <summary>
        /// Height on map
        /// </summary>
        public double Y;

        /// <summary>
        /// Vertical coordinate on map
        /// </summary>
        public double Z;

        /// <summary>
        /// Parses coordinates given as strings to scenery point object
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static ScnPoint Parse(string x, string y, string z) {
            var f = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
            return new ScnPoint {
                X = Double.Parse(x, f),
                Y = Double.Parse(y, f),
                Z = Double.Parse(z, f),
            };
        }

        /// <summary>
        /// Returns distance from this to given point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public double DistanceTo(ScnPoint p) {
            return Math.Sqrt((p.X - X) * (p.X - X) + (p.Y - Y) * (p.Y - Y) + (p.Z - Z) * (p.Z - Z));
        }

        /// <summary>
        /// Returns true if given point is considered linked to this point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsLinkedTo(ScnPoint p) {
            if (p == null) return false;
            return DistanceTo(p) < LinkDistance;
        }

        /// <summary>
        /// Returns point's text representation for debugging purpose
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return String.Format(System.Globalization.CultureInfo.InvariantCulture, "[{0}, {1}]", X, Z); 
        }

    }

}
