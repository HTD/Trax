namespace ScnEdit {
    partial class Refactor {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Refactor));
            this.RefactorToolStrip = new System.Windows.Forms.ToolStrip();
            this.Symbol = new System.Windows.Forms.ToolStripTextBox();
            this.Rename = new System.Windows.Forms.ToolStripButton();
            this.RefactorToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // RefactorToolStrip
            // 
            this.RefactorToolStrip.AllowMerge = false;
            this.RefactorToolStrip.CanOverflow = false;
            resources.ApplyResources(this.RefactorToolStrip, "RefactorToolStrip");
            this.RefactorToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.RefactorToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Symbol,
            this.Rename});
            this.RefactorToolStrip.Name = "RefactorToolStrip";
            this.RefactorToolStrip.Stretch = true;
            this.RefactorToolStrip.TabStop = true;
            // 
            // Symbol
            // 
            this.Symbol.AcceptsReturn = true;
            resources.ApplyResources(this.Symbol, "Symbol");
            this.Symbol.CausesValidation = false;
            this.Symbol.Name = "Symbol";
            this.Symbol.ShortcutsEnabled = false;
            // 
            // Rename
            // 
            this.Rename.AutoToolTip = false;
            this.Rename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.Rename, "Rename");
            this.Rename.Name = "Rename";
            this.Rename.Click += new System.EventHandler(this.Rename_Click);
            // 
            // Refactor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RefactorToolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Refactor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.RefactorToolStrip.ResumeLayout(false);
            this.RefactorToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip RefactorToolStrip;
        private System.Windows.Forms.ToolStripTextBox Symbol;
        private System.Windows.Forms.ToolStripButton Rename;
    }
}