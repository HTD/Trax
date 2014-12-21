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
    
    /// <summary>
    /// Global symbol replace form
    /// </summary>
    public partial class Refactor : Form {

        FastColoredTextBox E;
        Main M;
        string S;
        string R;

        /// <summary>
        /// Creates global replace dialog
        /// </summary>
        /// <param name="main"></param>
        public Refactor(Main main) {
            M = main;
            E = M.CurrentEditor;
            InitializeComponent();
            Width = Symbol.Width + Rename.Width + 4;
            if (E != null) {
                PreviewKeyDown += Refactor_PreviewKeyDown;
                Symbol.KeyDown += Symbol_KeyDown;
                GetSymbol();
                Symbol.Focus();
            } else Dispose();
        }

        /// <summary>
        /// Gets symbol from current editor selection and decides if it's applicable
        /// </summary>
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

        /// <summary>
        /// Handles form's preview keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refactor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Escape: if (!IsDisposed) Close(); break;
                case Keys.Enter: if (!IsDisposed) Close(); break;
            }
        }
        
        /// <summary>
        /// Handles keys pressed in text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Symbol_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Escape: Close(); e.Handled = true; break;
                case Keys.Enter: R = Symbol.Text; DoReplace(S, R); e.Handled = true; break;
            }
        }

        /// <summary>
        /// Handles rename click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rename_Click(object sender, EventArgs e) {
            DoReplace(S, Symbol.Text);
        }

        /// <summary>
        /// Performs replace
        /// </summary>
        /// <param name="original"></param>
        /// <param name="updated"></param>
        private void DoReplace(string original, string updated) {
            E.BeginInvoke(new MethodInvoker(() => { ProjectFile.ReplaceSymbol(original, updated); }));
            if (!IsDisposed) Close();
        }

    }

}
