using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    /// <summary>
    /// Implements the Bellman-Ford algorithm to find the shortest paths from a source vertex to all other vertices in a graph.
    /// </summary>
    public class BellmanFord : IExtremalPath
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

        private void HighlightAndSleep()
        {
            System.Threading.Thread.Sleep(HIGHLIGHT_DURATION_MS);
            OnExtremalPathEvent();
        }

        private void Relax(Vertex vertex, Vertex neighbor, Dictionary<Vertex, int> distances,
                           Dictionary<Vertex, Vertex> previous, Graph graph, Dictionary<Vertex, List<Vertex>> shortestPaths)
        {
            Edge edge = graph.GetEdge(vertex, neighbor);

            if (distances[vertex] == INFINITY || (int.MaxValue - distances[vertex]) < edge.Weight)
            {
                return;
            }

            int newDistance = checked(distances[vertex] + edge.Weight);
            edge.IsSelected = true;
            if (newDistance < distances[neighbor])
            {
                distances[neighbor] = newDistance;
                previous[neighbor] = vertex;
                shortestPaths[neighbor] = ReconstructPath(previous, neighbor);
                HighlightShortestPaths(graph, shortestPaths);
                HighlightAndSleep();
            }
            edge.IsSelected = false;
        }

        private void CheckForNegativeCycle(Vertex vertex, Vertex neighbor, Dictionary<Vertex, int> distances, Graph graph)
        {
            Edge edge = graph.GetEdge(vertex, neighbor);
            int newDistance = distances[vertex] + edge.Weight;  
            if (newDistance < distances[neighbor])
            {
                throw new InvalidOperationException("Graph contains a negative-weight cycle");
            }
        }

        /// <summary>
        /// Finds the shortest paths from the source vertex to all other vertices in the given graph.
        /// </summary>
        /// <param name="graph">The graph to search for the shortest paths.</param>
        /// <param name="source">The source vertex for the paths.</param>
        /// <returns>A dictionary mapping vertices to lists of vertices representing the shortest paths to them.</returns>
        public Dictionary<Vertex, List<Vertex>> FindShortestPath(Graph graph, Vertex source)
        {
            var distances = InitializeDistances(graph.Vertices, source);
            var previous = new Dictionary<Vertex, Vertex>();
            var shortestPaths = new Dictionary<Vertex, List<Vertex>>();

            foreach (var vertex in graph.Vertices)
            {
                previous[vertex] = null;
            }

            InitializeGraph(graph);

            for (int i = 0; i < graph.Vertices.Count - 1; i++)
            {
                foreach (var vertex in graph.Vertices)
                {
                    if (distances[vertex] == INFINITY) continue;

                    foreach (var neighbor in graph.GetNeighbors(vertex))
                    {
                        Relax(vertex, neighbor, distances, previous, graph, shortestPaths);
                    }
                }
            }

            foreach (var vertex in graph.Vertices)
            {
                if (distances[vertex] == INFINITY) continue;

                foreach (var neighbor in graph.GetNeighbors(vertex))
                {
                    CheckForNegativeCycle(vertex, neighbor, distances, graph);
                }
            }

            return shortestPaths;
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

            int maxIterations = previous.Count; // safeguard
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
