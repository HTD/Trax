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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Symbol = new System.Windows.Forms.ToolStripTextBox();
            this.ChangeLocal = new System.Windows.Forms.ToolStripButton();
            this.ChangeGlobal = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Symbol,
            this.ChangeLocal,
            this.ChangeGlobal});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // Symbol
            // 
            resources.ApplyResources(this.Symbol, "Symbol");
            this.Symbol.Name = "Symbol";
            // 
            // ChangeLocal
            // 
            this.ChangeLocal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ChangeLocal, "ChangeLocal");
            this.ChangeLocal.Name = "ChangeLocal";
            // 
            // ChangeGlobal
            // 
            this.ChangeGlobal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ChangeGlobal, "ChangeGlobal");
            this.ChangeGlobal.Name = "ChangeGlobal";
            // 
            // Refactor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Refactor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox Symbol;
        private System.Windows.Forms.ToolStripButton ChangeLocal;
        private System.Windows.Forms.ToolStripButton ChangeGlobal;
    }
}