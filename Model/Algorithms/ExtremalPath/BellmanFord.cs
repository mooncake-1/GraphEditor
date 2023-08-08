﻿using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    public class BellmanFord : IExtremalPath
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
