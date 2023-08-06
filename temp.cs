using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GraphEditor.Model.Algorithms.GraphTraversal;
using GraphEditor.Model.GraphStructure;

namespace GraphEditor
{
    public partial class Form1 : Form
    {
        #region Fields
        private Graph graph;
        private PictureBox pictureBox;
        private Vertex startVertex;
        private PointF tempEndPoint;
        private int vertexCounter = 1;
        private const float threshold = 10;
        #endregion

        #region Initialization
        public Form1()
        {
            InitializeComponent();
            ShowGraphTypeSelection();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void ShowGraphTypeSelection()
        {
            using (GraphTypeSelectionForm graphTypeForm = new GraphTypeSelectionForm())
            {
                if (graphTypeForm.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }

                if (graphTypeForm.IsDirectedGraph)
                {
                    graph = new DirectedGraph();
                }
                else
                {
                    graph = new UndirectedGraph();
                }

                InitializePictureBox();
                InitializeEvents();
            }
        }

        private void InitializePictureBox()
        {
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.MistyRose
            };

            this.Controls.Add(pictureBox);
            pictureBox.Paint += pictureBox_Paint;
        }

        private void InitializeEvents()
        {
            pictureBox.MouseClick += pictureBox_MouseClick;
            pictureBox.MouseDown += pictureBox_MouseDown;
            pictureBox.MouseMove += pictureBox_MouseMove;
            pictureBox.MouseUp += pictureBox_MouseUp;
        }
        #endregion

        #region PictureBoxEvents
        private void pictureBox_Paint(object sender, PaintEventArgs e) => graph.Draw(e.Graphics);

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HandleLeftClick(e.Location);
            }
            else if (e.Button == MouseButtons.Right)
            {
                HandleRightClick(e.Location);
            }
        }

        private void HandleLeftClick(Point location)
        {
            int vertexIndex = graph.GetVertexOnPosition(location);
            if (vertexIndex != -1)
            {
                return;
            }

            int edgeIndex = graph.GetEdgeOnPosition(location);
            if (edgeIndex != -1)
            {
                HandleEdgeSelection(edgeIndex, location);
            }
            else
            {
                AddNewVertex(location);
            }
        }

        private void HandleRightClick(Point location)
        {
            int vertexIndex = graph.GetVertexOnPosition(location);
            int edgeIndex = graph.GetEdgeOnPosition(location);

            if (vertexIndex != -1)
            {
                graph.RemoveVertex(vertexIndex);
            }
            else if (edgeIndex != -1)
            {
                graph.RemoveEdge(edgeIndex);
            }

            pictureBox.Refresh();
        }

        private void HandleEdgeSelection(int edgeIndex, Point location)
        {
            Edge selectedEdge = graph.Edges[edgeIndex];
            float distanceToEdge = selectedEdge.GetDistanceToPoint(location);

            if (distanceToEdge < threshold)
            {
                ModifyEdgeWeight(selectedEdge);
            }
        }

        private void ModifyEdgeWeight(Edge selectedEdge)
        {
            string weightInput = InputDialog.ShowDialog("Modify Edge Weight", "Enter the new weight for the edge:", selectedEdge.Weight.ToString());
            if (int.TryParse(weightInput, out int newWeight))
            {
                selectedEdge.Weight = newWeight;
                pictureBox.Refresh();
            }
        }

        private void AddNewVertex(PointF position)
        {
            Vertex newVertex = new Vertex(position, vertexCounter++);
            graph.AddVertex(newVertex);
            pictureBox.Refresh();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            int vertexIndex = graph.GetVertexOnPosition(e.Location);
            if (vertexIndex == -1) return;
            startVertex = graph.Vertices[vertexIndex];
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && startVertex != null)
            {
                tempEndPoint = e.Location;
                pictureBox.Refresh();
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || startVertex == null) return;

            int vertexIndex = graph.GetVertexOnPosition(e.Location);
            if (vertexIndex != -1)
            {
                Vertex endVertex = graph.Vertices[vertexIndex];
                int weight = 1;
                graph.AddEdge(startVertex, endVertex, weight);
            }

            startVertex = null;
            tempEndPoint = PointF.Empty;
            pictureBox.Refresh();
        }
        #endregion

        #region GraphTraversals
        private void breadthFirstSearchToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void DFSAlgorithm_GraphTraversalEvent(object sender, EventArgs e) => RefreshPictureBox();

        private void depthFirstSearchToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void BFSAlgorithm_GraphTraversalEvent(object sender, EventArgs e) => RefreshPictureBox();

        private void RefreshPictureBox()
        {
            pictureBox.Refresh();
            Application.DoEvents();
        }
        #endregion

        #region GraphTraversals
        private void graphTraversalToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void TraverseGraph_BFS(object sender, EventArgs e) => TraverseGraph<BreadthFirstSearch>();

        private void TraverseGraph_DFS(object sender, EventArgs e) => TraverseGraph<DepthFirstSearch>();

        private void CheckPath_BFS(object sender, EventArgs e) => CheckPath<BreadthFirstSearch>();

        private void CheckPath_DFS(object sender, EventArgs e) => CheckPath<DepthFirstSearch>();

        private void TraverseGraph<T>() where T : IGraphTraversal, new()
        {
            using (StartVertexSelectionForm startVertexForm = new StartVertexSelectionForm(graph.Vertices))
            {
                if (startVertexForm.ShowDialog() != DialogResult.OK) return;

                Vertex selectedStartVertex = startVertexForm.SelectedVertex;
                T traversalAlgorithm = new T();
                traversalAlgorithm.GraphTraversalEvent += RefreshPictureBox;
                List<Vertex> traversalResult = traversalAlgorithm.TraverseGraph(graph, selectedStartVertex);
            }
        }

        private void CheckPath<T>() where T : IGraphTraversal, new()
        {
            using (VertexSelectionForm vertexSelectionForm = new VertexSelectionForm(graph.Vertices))
            {
                if (vertexSelectionForm.ShowDialog() != DialogResult.OK) return;

                Vertex startVertex = vertexSelectionForm.StartVertex;
                Vertex destinationVertex = vertexSelectionForm.DestinationVertex;

                T traversalAlgorithm = new T();
                traversalAlgorithm.GraphTraversalEvent += RefreshPictureBox;

                if (!traversalAlgorithm.HasPath(graph, startVertex, destinationVertex, out List<Vertex> path))
                {
                    MessageBox.Show("No path exists.");
                    return;
                }

                foreach (Vertex vertex in path)
                {
                    vertex.IsHighlighted = true;
                }
            }
        }
        #endregion
    }
}
