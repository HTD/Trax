using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScnEdit {
    
    /// <summary>
    /// Bézier curve interpolation class using scn type definitions
    /// </summary>
    public class ScnBezier : Bezier3 {

        public ScnBezier(V3D p1, V3D c1, V3D c2, V3D p2) : base(p1, p1 + c1, c2 + p2, p2) { }
        
    }

}