using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.GraphTraversal
{
    /// <summary>
    /// Implements the Depth-First Search (DFS) algorithm for traversing graphs.
    /// </summary>
    public class DepthFirstSearch : IGraphTraversal
    {
        private const int HIGHLIGHT_DURATION_MS = 750;

        /// <summary>
        /// Occurs when a graph traversal event is triggered.
        /// </summary>
        public event EventHandler GraphTraversalEvent;

        private Dictionary<Vertex, Vertex> previousVertices;

        private void InitializeSearch(Graph graph, Vertex startVertex)
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.Color = VertexColor.White;
            }

            graph.ClearHighlighting();
        }

        private void HighlightEdge(Graph graph, Edge edge)
        {
            graph.HighlightEdge(edge);
            OnGraphTraversalEvent();
            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
        }

        private void MarkVertexAsVisited(Graph graph, Vertex vertex, ref int time)
        {
            vertex.Color = VertexColor.Grey;
            time++;
            vertex.DiscoveryTime = time;
            graph.HighlightVertex(vertex);
            OnGraphTraversalEvent();
            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
        }

        private void FinishVisit(Graph graph, Vertex vertex, ref int time)
        {
            vertex.Color = VertexColor.Black;
            time++;
            vertex.FinishTime = time;
            graph.HighlightVertex(vertex);
            OnGraphTraversalEvent();
            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
        }

        private void Visit(Graph graph, Vertex vertex, ref int time, List<Vertex> visitedVertices)
        {
            MarkVertexAsVisited(graph, vertex, ref time);

            foreach (Vertex neighbor in graph.GetNeighbors(vertex))
            {
                if (neighbor.Color == VertexColor.White)
                {
                    HighlightEdge(graph, graph.GetEdge(vertex, neighbor));
                    Visit(graph, neighbor, ref time, visitedVertices);
                }
            }

            FinishVisit(graph, vertex, ref time);
            visitedVertices.Add(vertex);
        }

        private bool Visit(Graph graph, Vertex source, Vertex destination, ref int time, List<Vertex> visitedVertices)
        {
            MarkVertexAsVisited(graph, source, ref time);

            foreach (Vertex neighbor in graph.GetNeighbors(source))
            {
                if (neighbor.Color == VertexColor.White)
                {
                    HighlightEdge(graph, graph.GetEdge(source, neighbor));
                    previousVertices[neighbor] = source;

                    if (neighbor == destination || Visit(graph, neighbor, destination, ref time, visitedVertices))
                    {
                        visitedVertices.Insert(0, source);
                        foreach (var vertex in graph.Vertices)
                        {
                            vertex.Color = VertexColor.White;
                        }
                        return true;
                    }
                }
            }

            FinishVisit(graph, source, ref time);

            return false;
        }

        /// <summary>
        /// Traverses the graph starting from the given vertex using the Depth-First Search (DFS) algorithm.
        /// </summary>
        /// <param name="graph">The graph to traverse.</param>
        /// <param name="startVertex">The starting vertex for the traversal.</param>
        /// <returns>A list of vertices in the order they were visited.</returns>
        public List<Vertex> TraverseGraph(Graph graph, Vertex startVertex)
        {
            List<Vertex> visitedVertices = new List<Vertex>();

            InitializeSearch(graph, startVertex);
            int time = 0;

            Visit(graph, startVertex, ref time, visitedVertices);

            graph.ClearHighlighting();

            return visitedVertices;
        }

        /// <summary>
        /// Determines if there is a path between the source and destination vertices in the graph using the Depth-First Search (DFS) algorithm.
        /// </summary>
        /// <param name="graph">The graph to search for the path.</param>
        /// <param name="source">The source vertex of the path.</param>
        /// <param name="destination">The destination vertex of the path.</param>
        /// <param name="path">The list of vertices that form the path, if found.</param>
        /// <returns><c>true</c> if there is a path, otherwise <c>false</c>.</returns>
        public bool HasPath(Graph graph, Vertex source, Vertex destination, out List<Vertex> path)
        {
            path = new List<Vertex>();

            if (source == destination)
            {
                path.Add(source);
                return true;
            }

            InitializeSearch(graph, source);
            previousVertices = new Dictionary<Vertex, Vertex>();
            Stack<Vertex> stack = new Stack<Vertex>();

            stack.Push(source);
            previousVertices[source] = null;

            int time = 0;
            bool hasPath = Visit(graph, source, destination, ref time, path);

            return hasPath;
        }

        /// <summary>
        /// Highlights the vertices and edges that form the given path in the graph.
        /// </summary>
        /// <param name="graph">The graph containing the path.</param>
        /// <param name="path">The list of vertices that form the path to highlight.</param>
        public void HighlightPath(Graph graph, List<Vertex> path)
        {
            graph.ClearHighlighting();

            for (int i = 0; i < path.Count - 1; i++)
            {
                HighlightEdge(graph, graph.GetEdge(path[i], path[i + 1]));
            }
        }

        protected virtual void OnGraphTraversalEvent() => GraphTraversalEvent?.Invoke(this, EventArgs.Empty);
    }
}
