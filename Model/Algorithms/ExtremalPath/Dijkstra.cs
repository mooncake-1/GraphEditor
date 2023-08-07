using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    public class Dijkstra : IExtremalPath
    {
        private const int INFINITY = int.MaxValue;
        private const int HIGHLIGHT_DURATION_MS = 1500;

        public event EventHandler ExtremalPathEvent;

        public Dictionary<Vertex, List<Vertex>> FindShortestPath(Graph graph, Vertex source)
        {
            var distances = InitializeDistances(graph.Vertices, source);
            var previous = new Dictionary<Vertex, Vertex>();
            var shortestPaths = new Dictionary<Vertex, List<Vertex>>();

            InitializeGraph(graph);

            var minHeap = new SortedDictionary<int, HashSet<Vertex>>
            {
                { 0, new HashSet<Vertex> { source } }
            };

            foreach (var vertex in graph.Vertices)
            {
                if (vertex != source)
                {
                    minHeap[INFINITY] = minHeap.ContainsKey(INFINITY) ? minHeap[INFINITY] : new HashSet<Vertex>();
                    minHeap[INFINITY].Add(vertex);
                }
                previous[vertex] = null;
            }

            while (minHeap.Count != 0)
            {
                var currentVertex = GetMinAndRemove(minHeap);

                foreach (var neighbor in graph.GetNeighbors(currentVertex))
                {
                    Relax(currentVertex, neighbor, graph, distances, previous, minHeap);

                    shortestPaths[neighbor] = ReconstructPath(previous, neighbor);

                    HighlightShortestPaths(graph, shortestPaths);
                    HighlightAndSleep(graph);
                }
            }

            return shortestPaths;
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

        private Vertex GetMinAndRemove(SortedDictionary<int, HashSet<Vertex>> minHeap)
        {
            var currentDistance = minHeap.First().Key;
            var currentVertex = minHeap.First().Value.First();

            minHeap[currentDistance].Remove(currentVertex);
            if (minHeap[currentDistance].Count == 0)
                minHeap.Remove(currentDistance);

            return currentVertex;
        }

        private void Relax(Vertex u, Vertex v, Graph graph, Dictionary<Vertex, int> distances, Dictionary<Vertex, Vertex> previous, SortedDictionary<int, HashSet<Vertex>> minHeap)
        {
            Edge edge = graph.GetEdge(u, v);
            int weight = edge.Weight;
            int newDistance = distances[u] == INFINITY ? INFINITY : distances[u] + weight;

            if (newDistance < distances[v])
            {
                int oldDistance = distances[v];
                minHeap[oldDistance].Remove(v);
                if (minHeap[oldDistance].Count == 0)
                    minHeap.Remove(oldDistance);

                distances[v] = newDistance;
                previous[v] = u;

                minHeap[newDistance] = minHeap.ContainsKey(newDistance) ? minHeap[newDistance] : new HashSet<Vertex>();
                minHeap[newDistance].Add(v);
            }
        }

        private void InitializeGraph(Graph graph)
        {
            foreach (Vertex vertex in graph.Vertices)
            {
                vertex.Color = VertexColor.White;
            }
            graph.ClearHighlighting();
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
