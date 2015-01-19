using System;
using System.Collections.Generic;
using System.Linq;

namespace ScnEdit {
    
    /// <summary>
    /// Generic scenery vector object
    /// </summary>
    public class ScnVectorObject<T> {

        public V3D[] Points = null;
        public Line[] Lines = null;
        public ScnBezier[] Curves = null;

        public ScnVectorObject() { }

        public string ScnType;
        public string SourcePath;
        public int SourceIndex;
        public int SourceLength;

        public double DistanceTo(V3D p) {
            throw new NotImplementedException();
        }

    }

    /// <summary>
    /// Generic scenery vector object list with search by coordinates capability
    /// </summary>
    public class ScnVectorObjects<T> : List<T> where T : ScnVectorObject<T> {

        /// <summary>
        /// Returns scenery vector objects in proximity of given point
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public ScnVectorObjects<T> FindInProximity(V3D p, double distance = 10) {
            return (ScnVectorObjects<T>)this.Where(v => v.DistanceTo(p) < distance).ToList();
        }

    }

}
