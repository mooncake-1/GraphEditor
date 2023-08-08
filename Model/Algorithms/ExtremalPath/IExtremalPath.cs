using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    /// <summary>
    /// Provides data for the extremal path event.
    /// </summary>
    public class ExtremalPathEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Defines the methods for finding and highlighting extremal paths in a graph.
    /// </summary>
    public interface IExtremalPath
    {
        /// <summary>
        /// Occurs when an extremal path event is triggered.
        /// </summary>
        event EventHandler ExtremalPathEvent;

        /// <summary>
        /// Finds the shortest paths from the source vertex to all other vertices in the graph.
        /// </summary>
        /// <param name="graph">The graph to search for the shortest paths.</param>
        /// <param name="source">The source vertex for the paths.</param>
        /// <returns>A dictionary mapping vertices to lists of vertices representing the shortest paths to them.</returns>
        Dictionary<Vertex, List<Vertex>> FindShortestPath(Graph graph, Vertex source);

        /// <summary>
        /// Highlights the shortest paths that were found in the graph.
        /// </summary>
        /// <param name="graph">The graph containing the shortest paths.</param>
        /// <param name="shortestPaths">A dictionary representing the shortest paths, as returned by <see cref="FindShortestPath"/>.</param>
        void HighlightShortestPaths(Graph graph, Dictionary<Vertex, List<Vertex>> shortestPaths);
    }
}
