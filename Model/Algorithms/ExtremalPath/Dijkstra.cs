using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    public class DistanceComparer : IComparer<Vertex>
    {
        private Dictionary<Vertex, int> distances;
        public DistanceComparer(Dictionary<Vertex, int> distances) => this.distances = distances;
        public int Compare(Vertex x, Vertex y) => distances[x].CompareTo(distances[y]);
    }

    public class Dijkstra : IExtremalPath
    {
        private const int INFINITY = int.MaxValue;
        private const int HIGHLIGHT_DURATION_MS = 1500;

        public delegate void ExtremalPathEventHandler(object sender, EventArgs e);
        public event EventHandler ExtremalPathEvent;

        private Graph graph;
        private Dictionary<Vertex, int> distances;
        private SortedSet<Vertex> vertices;
        private Dictionary<Vertex, Vertex> previous;
        private Dictionary<Vertex, List<Vertex>> shortestPaths;

        public Dictionary<Vertex, List<Vertex>> FindShortestPath(Graph graph, Vertex source)
        {
            Initialize(graph, source);

            while (vertices.Count != 0)
            {
                ProcessCurrentVertex();
            }

            return shortestPaths;
        }

        private void Initialize(Graph graph, Vertex source)
        {
            this.graph = graph;
            this.distances = InitializeDistances(graph.Vertices, source);
            this.vertices = new SortedSet<Vertex>(new DistanceComparer(distances));
            this.previous = new Dictionary<Vertex, Vertex>();
            this.shortestPaths = new Dictionary<Vertex, List<Vertex>>();

            InitializeGraph(graph);
            InitializeVerticesAndPrevious(graph.Vertices, source);
        }

        private void ProcessCurrentVertex()
        {
            Vertex current = vertices.Min;
            vertices.Remove(current);
            current.Color = VertexColor.Grey;

            foreach (var neighbor in graph.GetNeighbors(current))
            {
                ConsiderEdge(current, neighbor);
            }

            current.Color = VertexColor.White;
        }

        private void ConsiderEdge(Vertex current, Vertex neighbor)
        {
            Edge edgeBeingConsidered = graph.GetEdge(current, neighbor);
            edgeBeingConsidered.IsSelected = true;

            if (TryComputeNewDistance(current, neighbor, edgeBeingConsidered.Weight, out int newDistance))
            {
                UpdateDistanceAndPaths(neighbor, newDistance, current);
                HighlightChanges();
            }

            edgeBeingConsidered.IsSelected = false;
        }

        private bool TryComputeNewDistance(Vertex current, Vertex neighbor, int weight, out int newDistance)
        {
            newDistance = distances[current];

            if (newDistance == INFINITY || (int.MaxValue - newDistance) < weight)
            {
                newDistance = INFINITY;
                return false;
            }

            newDistance += weight;
            return newDistance < distances[neighbor];
        }

        private void UpdateDistanceAndPaths(Vertex neighbor, int newDistance, Vertex current)
        {
            distances[neighbor] = newDistance;
            previous[neighbor] = current;
            vertices.Remove(neighbor);
            vertices.Add(neighbor);
            shortestPaths[neighbor] = ReconstructPath(previous, neighbor);
        }

        private void HighlightChanges()
        {
            HighlightShortestPaths(graph, shortestPaths);
            HighlightAndSleep(graph);
        }

        private void InitializeGraph(Graph graph)
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.Color = VertexColor.White;
            }

            graph.ClearHighlighting();
        }

        private Dictionary<Vertex, int> InitializeDistances(IEnumerable<Vertex> vertices, Vertex source)
        {
            var distances = new Dictionary<Vertex, int>();
            foreach (var vertex in vertices)
            {
                distances[vertex] = vertex.Equals(source) ? 0 : INFINITY;
            }
            return distances;
        }

        private void InitializeVerticesAndPrevious(IEnumerable<Vertex> vertices, Vertex source)
        {
            foreach (var vertex in vertices)
            {
                this.vertices.Add(vertex);
                previous[vertex] = null;
            }
        }

        private void HighlightAndSleep(Graph graph)
        {
            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
            OnExtremalPathEvent();
        }

        private List<Vertex> ReconstructPath(Dictionary<Vertex, Vertex> previous, Vertex destination)
        {
            List<Vertex> path = new List<Vertex>();
            for (var vertex = destination; vertex != null; vertex = previous[vertex])
            {
                path.Insert(0, vertex);
            }
            return path;
        }

        public void HighlightShortestPaths(Graph graph, Dictionary<Vertex, List<Vertex>> shortestPaths)
        {
            graph.ClearHighlighting();

            foreach (var path in shortestPaths.Values)
            {
                HighlightPath(graph, path);
            }
        }

        public void HighlightPath(Graph graph, List<Vertex> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Edge edge = graph.GetEdge(path[i], path[i + 1]);
                graph.HighlightEdge(edge);
                graph.HighlightVertex(path[i]);
            }
        }

        protected virtual void OnExtremalPathEvent() => ExtremalPathEvent?.Invoke(this, EventArgs.Empty);
    }
}
