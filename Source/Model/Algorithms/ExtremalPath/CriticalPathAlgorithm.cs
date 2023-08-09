using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    /// <summary>
    /// Implements the methods for finding and highlighting extremal paths in a graph using the Critical Path Algorithm.
    /// </summary>
    public class CriticalPathAlgorithm : IExtremalPath
    {
        private const int INFINITY = int.MaxValue;
        private const int HIGHLIGHT_DURATION_MS = 1500;

        /// <summary>
        /// Occurs when an extremal path event is triggered.
        /// </summary>
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

        /// <summary>
        /// Updates the shortest path if a shorter path is found.
        /// </summary>
        /// <param name="u">The starting vertex.</param>
        /// <param name="v">The ending vertex.</param>
        /// <param name="graph">The graph containing the vertices.</param>
        /// <param name="distances">The current distances dictionary.</param>
        /// <param name="previous">The previous vertices dictionary.</param>
        private void Relax(Vertex u, Vertex v, Graph graph, Dictionary<Vertex, int> distances, Dictionary<Vertex, Vertex> previous)
        {
            Edge edge = graph.GetEdge(u, v);
            edge.IsSelected = true;

            if (distances[u] == INFINITY || (int.MaxValue - distances[u]) < edge.Weight)
            {
                return;
            }

            int newDistance = checked(distances[u] + edge.Weight);

            if (newDistance < distances[v])
            {
                distances[v] = newDistance;
                previous[v] = u;
            }

            edge.IsSelected = false;
        }

        /// <summary>
        /// Reconstructs the path from the previous vertices dictionary.
        /// </summary>
        /// <param name="previous">The previous vertices dictionary.</param>
        /// <param name="destination">The destination vertex.</param>
        /// <returns>A list of vertices representing the path.</returns>
        private List<Vertex> ReconstructPath(Dictionary<Vertex, Vertex> previous, Vertex destination)
        {
            List<Vertex> path = new List<Vertex>();
            int maxIterations = previous.Count;
            int currentIteration = 0;

            for (var vertex = destination; vertex != null; vertex = previous[vertex])
            {
                path.Insert(0, vertex);

                currentIteration++;
                if (currentIteration > maxIterations)
                {
                    break;
                }
            }

            return path;
        }

        /// <summary>
        /// Performs a topological sort on the graph.
        /// </summary>
        /// <param name="graph">The graph to sort.</param>
        /// <returns>A list of vertices in topological order.</returns>
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

        /// <summary>
        /// Utility method for the topological sort.
        /// </summary>
        /// <param name="vertex">The current vertex.</param>
        /// <param name="visited">The set of visited vertices.</param>
        /// <param name="stack">The stack to store the sorted vertices.</param>
        /// <param name="graph">The graph to sort.</param>
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

        /// <summary>
        /// Finds the shortest paths from the source vertex to all other vertices in the graph using a topological sort.
        /// </summary>
        /// <param name="graph">The graph to search for the shortest paths.</param>
        /// <param name="source">The source vertex for the paths.</param>
        /// <returns>A dictionary mapping vertices to lists of vertices representing the shortest paths to them.</returns>
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

        /// <summary>
        /// Highlights the shortest paths that were found in the graph.
        /// </summary>
        /// <param name="graph">The graph containing the shortest paths.</param>
        /// <param name="shortestPaths">A dictionary representing the shortest paths, as returned by <see cref="FindShortestPath"/>.</param>
        public void HighlightShortestPaths(Graph graph, Dictionary<Vertex, List<Vertex>> shortestPaths)
        {
            graph.ClearHighlighting();

            foreach (var path in shortestPaths.Values)
            {
                HighlightPath(graph, path);
            }
        }

        /// <summary>
        /// Highlights the specified path in the graph.
        /// </summary>
        /// <param name="graph">The graph containing the path.</param>
        /// <param name="path">The list of vertices that form the path to highlight.</param>
        public void HighlightPath(Graph graph, List<Vertex> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Edge edge = graph.GetEdge(path[i], path[i + 1]);
                graph.HighlightEdge(edge);
                graph.HighlightVertex(path[i]);
            }
        }

        /// <summary>
        /// Raises the <see cref="ExtremalPathEvent"/>.
        /// </summary>
        protected virtual void OnExtremalPathEvent() => ExtremalPathEvent?.Invoke(this, EventArgs.Empty);
    }
}
