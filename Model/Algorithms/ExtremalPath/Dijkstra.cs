using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

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

        private void HighlightAndSleep(Graph graph)
        {
            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
            OnExtremalPathEvent();
        }

        public Dictionary<Vertex, List<Vertex>> FindShortestPath(Graph graph, Vertex source)
        {
            var distances = InitializeDistances(graph.Vertices, source);
            var vertices = new SortedSet<Vertex>(new DistanceComparer(distances));
            var previous = new Dictionary<Vertex, Vertex>();
            var shortestPaths = new Dictionary<Vertex, List<Vertex>>();

            foreach (var vertex in graph.Vertices)
            {
                vertices.Add(vertex);
                previous[vertex] = null;
            }

            InitializeGraph(graph);

            while (vertices.Count != 0)
            {
                Vertex current = vertices.Min;
                vertices.Remove(current);
                current.Color = VertexColor.Grey;

                foreach (var neighbor in graph.GetNeighbors(current))
                {
                    Edge edgeBeingConsidered = graph.GetEdge(current, neighbor);
                    edgeBeingConsidered.IsSelected = true; 

                    int edgeWeight = edgeBeingConsidered.Weight;
                    int newDistance = distances[current] + edgeWeight;
                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        previous[neighbor] = current;
                        vertices.Remove(neighbor);
                        vertices.Add(neighbor);
                        shortestPaths[neighbor] = ReconstructPath(previous, neighbor);

                        HighlightShortestPaths(graph, shortestPaths);
                        HighlightAndSleep(graph);
                    }

                    edgeBeingConsidered.IsSelected = false; 
                }

                current.Color = VertexColor.White;
            }

            return shortestPaths;
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
