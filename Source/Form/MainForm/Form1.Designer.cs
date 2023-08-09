namespace GraphEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.graphTraversalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depthFirstSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traverseGraphToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkPathToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.breadthFirstSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traverseGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extremalPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dijkstraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findShortestPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bellmanFordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criticalPathAlgorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findShortestPathToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphTraversalToolStripMenuItem,
            this.extremalPathToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 461);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(7, 4, 0, 4);
            this.MainMenu.Size = new System.Drawing.Size(582, 27);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // graphTraversalToolStripMenuItem
            // 
            this.graphTraversalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.depthFirstSearchToolStripMenuItem,
            this.breadthFirstSearchToolStripMenuItem});
            this.graphTraversalToolStripMenuItem.Name = "graphTraversalToolStripMenuItem";
            this.graphTraversalToolStripMenuItem.Size = new System.Drawing.Size(99, 19);
            this.graphTraversalToolStripMenuItem.Text = "Graph Traversal";
            this.graphTraversalToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.graphTraversalToolStripMenuItem.Click += new System.EventHandler(this.graphTraversalToolStripMenuItem_Click);
            // 
            // depthFirstSearchToolStripMenuItem
            // 
            this.depthFirstSearchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.traverseGraphToolStripMenuItem1,
            this.checkPathToolStripMenuItem1});
            this.depthFirstSearchToolStripMenuItem.Name = "depthFirstSearchToolStripMenuItem";
            this.depthFirstSearchToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.depthFirstSearchToolStripMenuItem.Text = "Depth First Search";
            this.depthFirstSearchToolStripMenuItem.Click += new System.EventHandler(this.depthFirstSearchToolStripMenuItem_Click);
            // 
            // traverseGraphToolStripMenuItem1
            // 
            this.traverseGraphToolStripMenuItem1.Name = "traverseGraphToolStripMenuItem1";
            this.traverseGraphToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.traverseGraphToolStripMenuItem1.Text = "Traverse graph";
            this.traverseGraphToolStripMenuItem1.Click += new System.EventHandler(this.traverseGraphToolStripMenuItem1_Click);
            // 
            // checkPathToolStripMenuItem1
            // 
            this.checkPathToolStripMenuItem1.Name = "checkPathToolStripMenuItem1";
            this.checkPathToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.checkPathToolStripMenuItem1.Text = "Check path";
            this.checkPathToolStripMenuItem1.Click += new System.EventHandler(this.checkPathToolStripMenuItem1_Click);
            // 
            // breadthFirstSearchToolStripMenuItem
            // 
            this.breadthFirstSearchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.traverseGraphToolStripMenuItem,
            this.checkPathToolStripMenuItem});
            this.breadthFirstSearchToolStripMenuItem.Name = "breadthFirstSearchToolStripMenuItem";
            this.breadthFirstSearchToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.breadthFirstSearchToolStripMenuItem.Text = "Breadth First Search";
            this.breadthFirstSearchToolStripMenuItem.Click += new System.EventHandler(this.breadthFirstSearchToolStripMenuItem_Click);
            // 
            // traverseGraphToolStripMenuItem
            // 
            this.traverseGraphToolStripMenuItem.Name = "traverseGraphToolStripMenuItem";
            this.traverseGraphToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.traverseGraphToolStripMenuItem.Text = "Traverse graph";
            this.traverseGraphToolStripMenuItem.Click += new System.EventHandler(this.traverseGraphToolStripMenuItem_Click);
            // 
            // checkPathToolStripMenuItem
            // 
            this.checkPathToolStripMenuItem.Name = "checkPathToolStripMenuItem";
            this.checkPathToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.checkPathToolStripMenuItem.Text = "Check path";
            this.checkPathToolStripMenuItem.Click += new System.EventHandler(this.checkPathToolStripMenuItem_Click);
            // 
            // extremalPathToolStripMenuItem
            // 
            this.extremalPathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dijkstraToolStripMenuItem,
            this.bellmanFordToolStripMenuItem,
            this.criticalPathAlgorithmToolStripMenuItem});
            this.extremalPathToolStripMenuItem.Name = "extremalPathToolStripMenuItem";
            this.extremalPathToolStripMenuItem.Size = new System.Drawing.Size(92, 19);
            this.extremalPathToolStripMenuItem.Text = "Extremal Path";
            // 
            // dijkstraToolStripMenuItem
            // 
            this.dijkstraToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findShortestPathToolStripMenuItem});
            this.dijkstraToolStripMenuItem.Name = "dijkstraToolStripMenuItem";
            this.dijkstraToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.dijkstraToolStripMenuItem.Text = "Dijkstra";
            this.dijkstraToolStripMenuItem.Click += new System.EventHandler(this.dijkstraToolStripMenuItem_Click);
            // 
            // findShortestPathToolStripMenuItem
            // 
            this.findShortestPathToolStripMenuItem.Name = "findShortestPathToolStripMenuItem";
            this.findShortestPathToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.findShortestPathToolStripMenuItem.Text = "Find shortest path";
            this.findShortestPathToolStripMenuItem.Click += new System.EventHandler(this.findShortestPathToolStripMenuItem_Click);
            // 
            // bellmanFordToolStripMenuItem
            // 
            this.bellmanFordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shortestToolStripMenuItem});
            this.bellmanFordToolStripMenuItem.Name = "bellmanFordToolStripMenuItem";
            this.bellmanFordToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.bellmanFordToolStripMenuItem.Text = "Bellman-Ford";
            // 
            // shortestToolStripMenuItem
            // 
            this.shortestToolStripMenuItem.Name = "shortestToolStripMenuItem";
            this.shortestToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.shortestToolStripMenuItem.Text = "Find shortest path";
            this.shortestToolStripMenuItem.Click += new System.EventHandler(this.shortestToolStripMenuItem_Click);
            // 
            // criticalPathAlgorithmToolStripMenuItem
            // 
            this.criticalPathAlgorithmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findShortestPathToolStripMenuItem1});
            this.criticalPathAlgorithmToolStripMenuItem.Name = "criticalPathAlgorithmToolStripMenuItem";
            this.criticalPathAlgorithmToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.criticalPathAlgorithmToolStripMenuItem.Text = "Critical Path Algorithm";
            // 
            // findShortestPathToolStripMenuItem1
            // 
            this.findShortestPathToolStripMenuItem1.Name = "findShortestPathToolStripMenuItem1";
            this.findShortestPathToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.findShortestPathToolStripMenuItem1.Text = "Find shortest path";
            this.findShortestPathToolStripMenuItem1.Click += new System.EventHandler(this.findShortestPathToolStripMenuItem1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 488);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Mistral", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "Graph Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem graphTraversalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breadthFirstSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depthFirstSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traverseGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem traverseGraphToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem checkPathToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem extremalPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dijkstraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findShortestPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bellmanFordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shortestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criticalPathAlgorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findShortestPathToolStripMenuItem1;
    }
}

