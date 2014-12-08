using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScnEdit {
    
    internal static class Status {

        private const int k = 1024;
        private const int M = 1024 * 1024;
        private const int G = 1024 * 1024 * 1024;

        private static string DynamicSize(int size) {
            if (size < k) return size.ToString() + "B";
            if (size < M) return Math.Round(size / (double)k, 3).ToString() + "kB";
            if (size < G) return Math.Round(size / (double)M, 3).ToString() + "MB";
            return Math.Round(size / (double)G, 3).ToString() + "GB";
        }

        internal static Main Main;
        internal static string FileName { set { Main.Text = String.IsNullOrEmpty(value) ? "Scenery Editor" : (value + " :: ScnEdit"); } }
        internal static int FileSize { set { Main.SizeLabel.Text = DynamicSize(value); } }
        internal static int FileLines { set { Main.LinesLabel.Text = String.Format("{0} {1}", value, Messages.LinesUnit); } }
        internal static EditorFile.Types FileType { set { Main.TypeLabel.Text = value.ToString(); } }
        internal static string Text { set { Main.StatusLabel.Text = value; } }
        internal static int Line { set { Main.LineLabel.Text = String.Format("{0} {1}", Messages.LineSymbol, value); } }
        internal static int Column { set { Main.ColumnLabel.Text = String.Format("{0} {1}", Messages.ColumnSymbol, value); } }
        internal static int Selection { set { Main.SelectionLabel.Text = String.Format("{0} {1}", Messages.SelectionSymbol, value); } }
        internal static bool Replace { set { Main.ReplaceLabel.Text = value ? Messages.ReplaceSymbol : Messages.InsertSymbol; } }

        internal static bool Visible {
            set {
                Main.SizeLabel.Visible
                    = Main.SlashLabel.Visible
                    = Main.LinesLabel.Visible
                    = Main.TypeLabel.Visible
                    = Main.LineLabel.Visible
                    = Main.LineLabel.Visible
                    = Main.ColumnLabel.Visible
                    = Main.SelectionLabel.Visible
                    = Main.ReplaceLabel.Visible = value;
                Main.StatusLabel.Text = value ? Messages.Ready : Messages.Ready1;
            }
        }

    }

}
