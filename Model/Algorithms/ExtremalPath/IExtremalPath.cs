using GraphEditor.Model.GraphStructure;
using System;
using System.Collections.Generic;

namespace GraphEditor.Model.Algorithms.ExtremalPath
{
    public class ExtremalPathEventArgs : EventArgs
    {
    }

    public interface IExtremalPath
    {
        event EventHandler ExtremalPathEvent;

        Dictionary<Vertex, List<Vertex>> FindShortestPath(Graph graph, Vertex source);

        void HighlightShortestPaths(Graph graph, Dictionary<Vertex, List<Vertex>> shortestPaths);
    }
}
