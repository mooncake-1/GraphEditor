using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.Algorithms.GraphTraversal
{
    public class BreadthFirstSearch : IGraphTraversal
    {
        private const int HIGHLIGHT_DURATION_MS = 1000;

        public delegate void GraphTraversalEventHandler(object sender, EventArgs e);

        public event EventHandler GraphTraversalEvent;

        private void InitializeGraph(Graph graph)
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.Color = VertexColor.White;
            }

            graph.ClearHighlighting();
        }

        private void HighlightAndSleep(Graph graph, Edge edge = null, Vertex vertex = null)
        {
            if (edge != null)
                graph.HighlightEdge(edge);

            if (vertex != null)
                graph.HighlightVertex(vertex);

            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
            OnGraphTraversalEvent();
        }

        private List<Vertex> ReconstructPath(Dictionary<Vertex, Vertex> parentMap, Vertex destination)
        {
            List<Vertex> path = new List<Vertex>();
            Vertex vertexInPath = destination;

            while (vertexInPath != null)
            {
                path.Insert(0, vertexInPath);
                vertexInPath = parentMap[vertexInPath];
            }

            return path;
        }

        public List<Vertex> TraverseGraph(Graph graph, Vertex startVertex)
        {
            InitializeGraph(graph);

            Queue<Vertex> queue = InitializeTraversal(startVertex);
            List<Vertex> visitedVertices = new List<Vertex> { startVertex };

            while (queue.Count > 0)
            {
                Vertex currentVertex = queue.Dequeue();

                TraverseNeighbors(graph, currentVertex, queue, visitedVertices);

                currentVertex.Color = VertexColor.Black;
                HighlightAndSleep(graph, vertex: currentVertex);
            }

            InitializeGraph(graph);

            return visitedVertices;
        }

        private Queue<Vertex> InitializeTraversal(Vertex startVertex)
        {
            Queue<Vertex> queue = new Queue<Vertex>();
            queue.Enqueue(startVertex);
            startVertex.Color = VertexColor.Grey;

            return queue;
        }

      
        private void TraverseNeighbors(Graph graph, Vertex currentVertex, Queue<Vertex> queue, List<Vertex> visitedVertices)
        {
            foreach (Vertex neighbor in graph.GetNeighbors(currentVertex))
            {
                if (neighbor.Color == VertexColor.White)
                {
                    Edge edgeToNeighbor = graph.GetEdge(currentVertex, neighbor);
                    HighlightAndSleep(graph, edgeToNeighbor);

                    neighbor.Color = VertexColor.Grey;
                    queue.Enqueue(neighbor);
                    visitedVertices.Add(neighbor);
                }
            }
        }

        protected virtual void OnGraphTraversalEvent() => GraphTraversalEvent?.Invoke(this, EventArgs.Empty);

        private bool DestinationReached(Vertex currentVertex, Vertex destination,
            Dictionary<Vertex, Vertex> parentMap, Graph graph, out List<Vertex> path)
        {
            if (currentVertex != destination)
            {
                path = null;
                return false;
            }

            path = ReconstructPath(parentMap, destination);
            InitializeGraph(graph);
            return true;
        }

        private void PrepareNeighbor(Graph graph, Vertex neighbor, Vertex currentVertex,
            Queue<Vertex> queue, Dictionary<Vertex, Vertex> parentMap)
        {
            neighbor.Color = VertexColor.Grey;
            queue.Enqueue(neighbor);
            parentMap[neighbor] = currentVertex;
            HighlightAndSleep(graph, graph.GetEdge(currentVertex, neighbor));
        }

        private bool ProcessNeighbors(Graph graph, Vertex currentVertex, Vertex destination,
            Dictionary<Vertex, Vertex> parentMap, Queue<Vertex> queue)
        {
            foreach (var neighbor in graph.GetNeighbors(currentVertex))
            {
                if (neighbor.Color != VertexColor.White) continue;

                PrepareNeighbor(graph, neighbor, currentVertex, queue, parentMap);

                if (DestinationReached(neighbor, destination, parentMap, graph, out List<Vertex> _))
                {
                    return true;
                }
            }

            currentVertex.Color = VertexColor.Black;
            return false;
        }

        public bool HasPath(Graph graph, Vertex source, Vertex destination, out List<Vertex> path)
        {
            InitializeGraph(graph);
            var parentMap = new Dictionary<Vertex, Vertex> { { source, null } };
            var queue = InitializeTraversal(source);

            while (queue.Any())
            {
                var currentVertex = queue.Dequeue();
                HighlightAndSleep(graph, vertex: currentVertex);

                if (DestinationReached(currentVertex, destination, parentMap, graph, out path))
                    return true;

                if (ProcessNeighbors(graph, currentVertex, destination, parentMap, queue))
                {
                    path = ReconstructPath(parentMap, destination);
                    return true;
                }
            }

            path = null;
            InitializeGraph(graph);
            return false;
        }

        public void HighlightPath(Graph graph, List<Vertex> path)
        {
            graph.ClearHighlighting();

            for (int i = 0; i < path.Count - 1; i++)
            {
                Edge edge = graph.GetEdge(path[i], path[i + 1]);
                if (edge != null)
                {
                    graph.HighlightEdge(edge);
                    System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
                }
            }
        }
    }
}
