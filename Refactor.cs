using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ScnEdit {
    public partial class Refactor : Form {

        FastColoredTextBox E;
        string S;
        //string ReplaceSymbol;


        public Refactor(Main main) {
            InitializeComponent();
            E = main.CurrentEditor;
            if (E != null) {
                KeyDown += Refactor_KeyDown;
                GetSymbol();
            } else Dispose();
        }

        void Refactor_Shown(object sender, EventArgs e) {
            var main = Owner as Main;
            E = main.CurrentEditor;
        }

        void GetSymbol() {
            var originalSelection = new Range(E, E.Selection.Start, E.Selection.End);
            S = E.SelectWord();
            E.Selection.Normalize();
            var point = E.PlaceToPoint(E.Selection.Start);
            var target = E.PointToScreen(point);
            target.X -= E.Left;
            target.Y -= Height - ClientRectangle.Height - E.Left + 3;
            Location = target;
            Symbol.Text = S;
            var style = E.Selection.Chars.First().style;
            var notApplicableForStyles = new StyleIndex[] {
                EditorSyntax.StyleMap.Command,
                EditorSyntax.StyleMap.Comment,
                EditorSyntax.StyleMap.Keyword0,
                EditorSyntax.StyleMap.Keyword1,
                EditorSyntax.StyleMap.Keyword2,
                EditorSyntax.StyleMap.Keyword3,
                EditorSyntax.StyleMap.Keyword4,
                EditorSyntax.StyleMap.Number
            };
            var applicable = !notApplicableForStyles.Contains(style);
            if (applicable) ShowDialog();
            else { E.Selection = originalSelection; Dispose(); }
        }
        
        void Refactor_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        int FindStart(string text, int i) {
            while (i > 0) { var x = text[i--]; if (x == ' ' || x == ';' || x == '\n') return i + 2; }
            return -1;
        }

        int FindEnd(string text, int i) {
            while (i > 0) { var x = text[i++]; if (x == ' ' || x == ';' || x == '\r' || x == '\n') return i - 1; }
            return -1;
        }

        
    }
}
