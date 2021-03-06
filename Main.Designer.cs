﻿namespace Trax {
    partial class Main {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.AlignMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NormalizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s2 = new System.Windows.Forms.ToolStripSeparator();
            this.CommentSelectedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.WordWrapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s3 = new System.Windows.Forms.ToolStripSeparator();
            this.ColorSchemeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.s4 = new System.Windows.Forms.ToolStripSeparator();
            this.ClearMarkersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SceneryMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FindObjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FindReferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s5 = new System.Windows.Forms.ToolStripSeparator();
            this.RefactorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s6 = new System.Windows.Forms.ToolStripSeparator();
            this.SceneryUndoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SceneryRedoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.s7 = new System.Windows.Forms.ToolStripSeparator();
            this.SceneryNormalizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NameTracksMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ShowMapMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.StartDebuggingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartWithoutDebuggingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreditsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.TypeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SlashLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LinesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.LineLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ColumnLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ReplaceLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.BrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.MainMenu.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            resources.ApplyResources(this.MainMenu, "MainMenu");
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.EditMenu,
            this.ViewMenu,
            this.SceneryMenu,
            this.DebugMenu,
            this.CreditsMenuItem});
            this.MainMenu.Name = "MainMenu";
            // 
            // FileMenu
            // 
            resources.ApplyResources(this.FileMenu, "FileMenu");
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuItem,
            this.SaveMenuItem,
            this.SaveAllMenuItem,
            this.CloseMenuItem,
            this.s1,
            this.ExitMenuItem});
            this.FileMenu.Name = "FileMenu";
            // 
            // OpenMenuItem
            // 
            resources.ApplyResources(this.OpenMenuItem, "OpenMenuItem");
            this.OpenMenuItem.Name = "OpenMenuItem";
            this.OpenMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // SaveMenuItem
            // 
            resources.ApplyResources(this.SaveMenuItem, "SaveMenuItem");
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
            // 
            // SaveAllMenuItem
            // 
            resources.ApplyResources(this.SaveAllMenuItem, "SaveAllMenuItem");
            this.SaveAllMenuItem.Name = "SaveAllMenuItem";
            this.SaveAllMenuItem.Click += new System.EventHandler(this.SaveAllMenuItem_Click);
            // 
            // CloseMenuItem
            // 
            resources.ApplyResources(this.CloseMenuItem, "CloseMenuItem");
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);
            // 
            // s1
            // 
            resources.ApplyResources(this.s1, "s1");
            this.s1.Name = "s1";
            // 
            // ExitMenuItem
            // 
            resources.ApplyResources(this.ExitMenuItem, "ExitMenuItem");
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // EditMenu
            // 
            resources.ApplyResources(this.EditMenu, "EditMenu");
            this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AlignMenuItem,
            this.NormalizeMenuItem,
            this.s2,
            this.CommentSelectedMenuItem});
            this.EditMenu.Name = "EditMenu";
            // 
            // AlignMenuItem
            // 
            resources.ApplyResources(this.AlignMenuItem, "AlignMenuItem");
            this.AlignMenuItem.Name = "AlignMenuItem";
            // 
            // NormalizeMenuItem
            // 
            resources.ApplyResources(this.NormalizeMenuItem, "NormalizeMenuItem");
            this.NormalizeMenuItem.Name = "NormalizeMenuItem";
            this.NormalizeMenuItem.Click += new System.EventHandler(this.NormalizeMenuItem_Click);
            // 
            // s2
            // 
            resources.ApplyResources(this.s2, "s2");
            this.s2.Name = "s2";
            // 
            // CommentSelectedMenuItem
            // 
            resources.ApplyResources(this.CommentSelectedMenuItem, "CommentSelectedMenuItem");
            this.CommentSelectedMenuItem.Name = "CommentSelectedMenuItem";
            this.CommentSelectedMenuItem.Click += new System.EventHandler(this.CommentSelectedMenuItem_Click);
            // 
            // ViewMenu
            // 
            resources.ApplyResources(this.ViewMenu, "ViewMenu");
            this.ViewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WordWrapMenuItem,
            this.s3,
            this.ColorSchemeMenu,
            this.s4,
            this.ClearMarkersMenuItem});
            this.ViewMenu.Name = "ViewMenu";
            // 
            // WordWrapMenuItem
            // 
            resources.ApplyResources(this.WordWrapMenuItem, "WordWrapMenuItem");
            this.WordWrapMenuItem.Name = "WordWrapMenuItem";
            this.WordWrapMenuItem.Click += new System.EventHandler(this.WordWrapMenuItem_Click);
            // 
            // s3
            // 
            resources.ApplyResources(this.s3, "s3");
            this.s3.Name = "s3";
            // 
            // ColorSchemeMenu
            // 
            resources.ApplyResources(this.ColorSchemeMenu, "ColorSchemeMenu");
            this.ColorSchemeMenu.Name = "ColorSchemeMenu";
            // 
            // s4
            // 
            resources.ApplyResources(this.s4, "s4");
            this.s4.Name = "s4";
            // 
            // ClearMarkersMenuItem
            // 
            resources.ApplyResources(this.ClearMarkersMenuItem, "ClearMarkersMenuItem");
            this.ClearMarkersMenuItem.Name = "ClearMarkersMenuItem";
            this.ClearMarkersMenuItem.Click += new System.EventHandler(this.ClearMarkersMenuItem_Click);
            // 
            // SceneryMenu
            // 
            resources.ApplyResources(this.SceneryMenu, "SceneryMenu");
            this.SceneryMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.FindObjectMenuItem,
            this.FindReferencesMenuItem,
            this.s5,
            this.RefactorMenuItem,
            this.s6,
            this.SceneryUndoMenuItem,
            this.SceneryRedoMenuItem,
            this.s7,
            this.SceneryNormalizeMenuItem,
            this.NameTracksMenuItem,
            this.toolStripMenuItem1,
            this.ShowMapMenuItem});
            this.SceneryMenu.Name = "SceneryMenu";
            // 
            // findToolStripMenuItem
            // 
            resources.ApplyResources(this.findToolStripMenuItem, "findToolStripMenuItem");
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            // 
            // FindObjectMenuItem
            // 
            resources.ApplyResources(this.FindObjectMenuItem, "FindObjectMenuItem");
            this.FindObjectMenuItem.Name = "FindObjectMenuItem";
            // 
            // FindReferencesMenuItem
            // 
            resources.ApplyResources(this.FindReferencesMenuItem, "FindReferencesMenuItem");
            this.FindReferencesMenuItem.Name = "FindReferencesMenuItem";
            this.FindReferencesMenuItem.Click += new System.EventHandler(this.FindReferencesMenuItem_Click);
            // 
            // s5
            // 
            resources.ApplyResources(this.s5, "s5");
            this.s5.Name = "s5";
            // 
            // RefactorMenuItem
            // 
            resources.ApplyResources(this.RefactorMenuItem, "RefactorMenuItem");
            this.RefactorMenuItem.Name = "RefactorMenuItem";
            this.RefactorMenuItem.Click += new System.EventHandler(this.RefactorMenuItem_Click);
            // 
            // s6
            // 
            resources.ApplyResources(this.s6, "s6");
            this.s6.Name = "s6";
            // 
            // SceneryUndoMenuItem
            // 
            resources.ApplyResources(this.SceneryUndoMenuItem, "SceneryUndoMenuItem");
            this.SceneryUndoMenuItem.Name = "SceneryUndoMenuItem";
            this.SceneryUndoMenuItem.Click += new System.EventHandler(this.SceneryUndoMenuItem_Click);
            // 
            // SceneryRedoMenuItem
            // 
            resources.ApplyResources(this.SceneryRedoMenuItem, "SceneryRedoMenuItem");
            this.SceneryRedoMenuItem.Name = "SceneryRedoMenuItem";
            this.SceneryRedoMenuItem.Click += new System.EventHandler(this.SceneryRedoMenuItem_Click);
            // 
            // s7
            // 
            resources.ApplyResources(this.s7, "s7");
            this.s7.Name = "s7";
            // 
            // SceneryNormalizeMenuItem
            // 
            resources.ApplyResources(this.SceneryNormalizeMenuItem, "SceneryNormalizeMenuItem");
            this.SceneryNormalizeMenuItem.Name = "SceneryNormalizeMenuItem";
            this.SceneryNormalizeMenuItem.Click += new System.EventHandler(this.SceneryNormalizeMenuItem_Click);
            // 
            // NameTracksMenuItem
            // 
            resources.ApplyResources(this.NameTracksMenuItem, "NameTracksMenuItem");
            this.NameTracksMenuItem.Name = "NameTracksMenuItem";
            this.NameTracksMenuItem.Click += new System.EventHandler(this.NameTracksMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // ShowMapMenuItem
            // 
            resources.ApplyResources(this.ShowMapMenuItem, "ShowMapMenuItem");
            this.ShowMapMenuItem.Name = "ShowMapMenuItem";
            this.ShowMapMenuItem.Click += new System.EventHandler(this.ShowMapMenuItem_Click);
            // 
            // DebugMenu
            // 
            resources.ApplyResources(this.DebugMenu, "DebugMenu");
            this.DebugMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartDebuggingMenuItem,
            this.StartWithoutDebuggingMenuItem});
            this.DebugMenu.Name = "DebugMenu";
            // 
            // StartDebuggingMenuItem
            // 
            resources.ApplyResources(this.StartDebuggingMenuItem, "StartDebuggingMenuItem");
            this.StartDebuggingMenuItem.Name = "StartDebuggingMenuItem";
            this.StartDebuggingMenuItem.Click += new System.EventHandler(this.RunMenuItem_Click);
            // 
            // StartWithoutDebuggingMenuItem
            // 
            resources.ApplyResources(this.StartWithoutDebuggingMenuItem, "StartWithoutDebuggingMenuItem");
            this.StartWithoutDebuggingMenuItem.Name = "StartWithoutDebuggingMenuItem";
            // 
            // CreditsMenuItem
            // 
            resources.ApplyResources(this.CreditsMenuItem, "CreditsMenuItem");
            this.CreditsMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.CreditsMenuItem.Name = "CreditsMenuItem";
            this.CreditsMenuItem.Click += new System.EventHandler(this.CreditsMenuItem_Click);
            // 
            // MainStatusStrip
            // 
            resources.ApplyResources(this.MainStatusStrip, "MainStatusStrip");
            this.MainStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.TypeLabel,
            this.SizeLabel,
            this.SlashLabel,
            this.LinesLabel,
            this.LineLabel,
            this.ColumnLabel,
            this.SelectionLabel,
            this.ReplaceLabel});
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.SizingGrip = false;
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.ForeColor = System.Drawing.Color.White;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Spring = true;
            // 
            // TypeLabel
            // 
            resources.ApplyResources(this.TypeLabel, "TypeLabel");
            this.TypeLabel.ForeColor = System.Drawing.Color.White;
            this.TypeLabel.Name = "TypeLabel";
            // 
            // SizeLabel
            // 
            resources.ApplyResources(this.SizeLabel, "SizeLabel");
            this.SizeLabel.ForeColor = System.Drawing.Color.White;
            this.SizeLabel.Name = "SizeLabel";
            // 
            // SlashLabel
            // 
            resources.ApplyResources(this.SlashLabel, "SlashLabel");
            this.SlashLabel.ForeColor = System.Drawing.Color.White;
            this.SlashLabel.Name = "SlashLabel";
            // 
            // LinesLabel
            // 
            resources.ApplyResources(this.LinesLabel, "LinesLabel");
            this.LinesLabel.ForeColor = System.Drawing.Color.White;
            this.LinesLabel.Name = "LinesLabel";
            // 
            // LineLabel
            // 
            resources.ApplyResources(this.LineLabel, "LineLabel");
            this.LineLabel.ForeColor = System.Drawing.Color.White;
            this.LineLabel.Name = "LineLabel";
            // 
            // ColumnLabel
            // 
            resources.ApplyResources(this.ColumnLabel, "ColumnLabel");
            this.ColumnLabel.ForeColor = System.Drawing.Color.White;
            this.ColumnLabel.Name = "ColumnLabel";
            // 
            // SelectionLabel
            // 
            resources.ApplyResources(this.SelectionLabel, "SelectionLabel");
            this.SelectionLabel.ForeColor = System.Drawing.Color.White;
            this.SelectionLabel.Name = "SelectionLabel";
            // 
            // ReplaceLabel
            // 
            resources.ApplyResources(this.ReplaceLabel, "ReplaceLabel");
            this.ReplaceLabel.ForeColor = System.Drawing.Color.White;
            this.ReplaceLabel.Name = "ReplaceLabel";
            // 
            // OpenFileDialog
            // 
            resources.ApplyResources(this.OpenFileDialog, "OpenFileDialog");
            this.OpenFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFile_FileOk);
            // 
            // SaveFileDialog
            // 
            resources.ApplyResources(this.SaveFileDialog, "SaveFileDialog");
            // 
            // BrowseFolder
            // 
            resources.ApplyResources(this.BrowseFolder, "BrowseFolder");
            // 
            // DockPanel
            // 
            resources.ApplyResources(this.DockPanel, "DockPanel");
            this.DockPanel.Name = "DockPanel";
            this.DockPanel.ActiveDocumentChanged += new System.EventHandler(this.DockPanel_ActiveDocumentChanged);
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DockPanel);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MainMenu;
            this.Name = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
        private System.Windows.Forms.ToolStripSeparator s1;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DebugMenu;
        private System.Windows.Forms.ToolStripMenuItem StartDebuggingMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog BrowseFolder;
        private System.Windows.Forms.ToolStripMenuItem EditMenu;
        private System.Windows.Forms.ToolStripMenuItem NormalizeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AlignMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SceneryMenu;
        private System.Windows.Forms.ToolStripMenuItem RefactorMenuItem;
        private System.Windows.Forms.ToolStripSeparator s6;
        private System.Windows.Forms.ToolStripMenuItem SceneryUndoMenuItem;
        private System.Windows.Forms.ToolStripSeparator s7;
        private System.Windows.Forms.ToolStripMenuItem SceneryNormalizeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NameTracksMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StartWithoutDebuggingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SceneryRedoMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ViewMenu;
        internal System.Windows.Forms.ToolStripMenuItem WordWrapMenuItem;
        private System.Windows.Forms.ToolStripSeparator s3;
        private System.Windows.Forms.ToolStripMenuItem ColorSchemeMenu;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        internal System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        internal System.Windows.Forms.ToolStripStatusLabel LineLabel;
        internal System.Windows.Forms.ToolStripStatusLabel ColumnLabel;
        internal System.Windows.Forms.ToolStripStatusLabel SelectionLabel;
        internal System.Windows.Forms.ToolStripStatusLabel ReplaceLabel;
        internal System.Windows.Forms.ToolStripStatusLabel SizeLabel;
        internal System.Windows.Forms.ToolStripStatusLabel TypeLabel;
        internal System.Windows.Forms.ToolStripStatusLabel LinesLabel;
        private System.Windows.Forms.ToolStripMenuItem SaveAllMenuItem;
        internal System.Windows.Forms.ToolStripStatusLabel SlashLabel;
        private System.Windows.Forms.ToolStripSeparator s2;
        private System.Windows.Forms.ToolStripMenuItem CommentSelectedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FindObjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FindReferencesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator s5;
        internal WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;
        private System.Windows.Forms.ToolStripSeparator s4;
        private System.Windows.Forms.ToolStripMenuItem ClearMarkersMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ShowMapMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreditsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseMenuItem;
    }
}

