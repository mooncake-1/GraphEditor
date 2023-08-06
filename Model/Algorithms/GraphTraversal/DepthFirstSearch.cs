using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.GraphTraversal
{
    public class DepthFirstSearch : IGraphTraversal
    {
        private const int HIGHLIGHT_DURATION_MS = 750;

        public delegate void GraphTraversalEventHandler(object sender, EventArgs e);

        public event EventHandler GraphTraversalEvent;

        private Dictionary<Vertex, Vertex> previousVertices;

        public List<Vertex> TraverseGraph(Graph graph, Vertex startVertex)
        {
            List<Vertex> visitedVertices = new List<Vertex>();

            InitializeSearch(graph, startVertex);
            int time = 0;

            Visit(graph, startVertex, ref time, visitedVertices);

            graph.ClearHighlighting();

            return visitedVertices;
        }

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

        public void HighlightPath(Graph graph, List<Vertex> path)
        {
            graph.ClearHighlighting();

            for (int i = 0; i < path.Count - 1; i++)
            {
                HighlightEdge(graph, graph.GetEdge(path[i], path[i + 1]));
            }
        }

        private void InitializeSearch(Graph graph, Vertex startVertex)
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.Color = VertexColor.White;
            }

            graph.ClearHighlighting();
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

        protected virtual void OnGraphTraversalEvent() => GraphTraversalEvent?.Invoke(this, EventArgs.Empty);
    }
}
