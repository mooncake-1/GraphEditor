using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.GraphTraversal
{
    /// <summary>
    /// Provides data for the graph traversal event.
    /// </summary>
    public class GraphTraversalEventArgs : EventArgs
    {
    }

    /// <summary>
    /// Defines the methods for graph traversal operations.
    /// </summary>
    public interface IGraphTraversal
    {
        /// <summary>
        /// Occurs when a graph traversal event is triggered.
        /// </summary>
        event EventHandler GraphTraversalEvent;

        /// <summary>
        /// Traverses the graph starting from the given vertex.
        /// </summary>
        /// <param name="graph">The graph to traverse.</param>
        /// <param name="startVertex">The starting vertex for the traversal.</param>
        /// <returns>A list of vertices in the order they were visited.</returns>
        List<Vertex> TraverseGraph(Graph graph, Vertex startVertex);

        /// <summary>
        /// Determines if there is a path between the source and destination vertices in the graph.
        /// </summary>
        /// <param name="graph">The graph to search for the path.</param>
        /// <param name="source">The source vertex of the path.</param>
        /// <param name="destination">The destination vertex of the path.</param>
        /// <param name="path">The list of vertices that form the path, if found.</param>
        /// <returns><c>true</c> if there is a path, otherwise <c>false</c>.</returns>
        bool HasPath(Graph graph, Vertex source, Vertex destination, out List<Vertex> path);

        /// <summary>
        /// Highlights the vertices and edges that form the given path in the graph.
        /// </summary>
        /// <param name="graph">The graph containing the path.</param>
        /// <param name="path">The list of vertices that form the path to highlight.</param>
        void HighlightPath(Graph graph, List<Vertex> path);
    }
}
