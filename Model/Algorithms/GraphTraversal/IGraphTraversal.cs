using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.GraphTraversal
{
    public class GraphTraversalEventArgs : EventArgs
    {
    }

    public interface IGraphTraversal
    {
        event EventHandler GraphTraversalEvent;

        List<Vertex> TraverseGraph(Graph graph, Vertex startVertex);

        bool HasPath(Graph graph, Vertex source, Vertex destination, out List<Vertex> path);

        void HighlightPath(Graph graph, List<Vertex> path);
    }
}
