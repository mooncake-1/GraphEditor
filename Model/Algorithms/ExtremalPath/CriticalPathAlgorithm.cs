using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    public class CriticalPathAlgorithm : IExtremalPath
    {
        private const int INFINITY = int.MaxValue;
        private const int HIGHLIGHT_DURATION_MS = 1500;

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
            var previous = new Dictionary<Vertex, Vertex>();
            var shortestPaths = new Dictionary<Vertex, List<Vertex>>();

            InitializeGraph(graph);

            List<Vertex> topologicalOrder = TopologicalSort(graph);

            foreach (var u in topologicalOrder)
            {
                u.Color = VertexColor.Grey;
                foreach (var v in graph.GetNeighbors(u))
                {
                    Relax(u, v, graph, distances, previous);
                    shortestPaths[v] = ReconstructPath(previous, v);
                    HighlightShortestPaths(graph, shortestPaths);
                    HighlightAndSleep(graph);
                }
                u.Color = VertexColor.White;
            }

            return shortestPaths;
        }

        private List<Vertex> TopologicalSort(Graph graph)
        {
            Stack<Vertex> stack = new Stack<Vertex>();
            HashSet<Vertex> visited = new HashSet<Vertex>();

            foreach (var vertex in graph.Vertices)
            {
                if (!visited.Contains(vertex))
                {
                    TopologicalSortUtil(vertex, visited, stack, graph);
                }
            }

            return stack.ToList();
        }

        private void TopologicalSortUtil(Vertex vertex, HashSet<Vertex> visited, Stack<Vertex> stack, Graph graph)
        {
            visited.Add(vertex);
            vertex.Color = VertexColor.Grey;
            HighlightAndSleep(graph);

            foreach (var neighbor in graph.GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    TopologicalSortUtil(neighbor, visited, stack, graph);
                }
            }
            vertex.Color = VertexColor.White;
            stack.Push(vertex);
        }

        private void Relax(Vertex u, Vertex v, Graph graph, Dictionary<Vertex, int> distances, Dictionary<Vertex, Vertex> previous)
        {
            int weight = graph.GetEdge(u, v).Weight;
            if (distances[u] != INFINITY && distances[u] + weight < distances[v])
            {
                distances[v] = distances[u] + weight;
                previous[v] = u;
            }
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
