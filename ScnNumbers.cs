using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ScnEdit {
    
    public static class ScnNumbers {

        public static CultureInfo IC = CultureInfo.InvariantCulture;
        public const string NF = "0.############";

        public static double Parse(string s) {
            return System.Double.Parse(s, IC);
        }

        public static string ToString(double d) {
            return d.ToString(NF);
        }

        public static string ToString(double? d) {
            if (d == null) return "";
            return ((double)d).ToString(NF, IC);
        }

        public static string ToString(double[] d) {
            int l = d.Length;
            string[] s = new string[l];
            for (int i = 0; i < l; i++) s[i] = d[i].ToString(NF, IC);
            return String.Join(" ", s);
        }

        public static string ToString(double?[] d) {
            int l = d.Length;
            string[] s = new string[l];
            for (int i = 0; i < l; i++) s[i] = ToString(d[i]);
            return String.Join(" ", s);
        }

    }
}
