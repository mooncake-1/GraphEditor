using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.GraphStructure
{

    /// <summary>
    /// Represents a directed graph containing vertices and edges.
    /// </summary>
    public class DirectedGraph : Graph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectedGraph"/> class.
        /// </summary>
        public DirectedGraph() : base()
        {
        }

        /// <summary>
        /// Adds a directed edge to the graph between the given vertices with the specified weight.
        /// </summary>
        /// <param name="from">The starting vertex of the edge.</param>
        /// <param name="to">The ending vertex of the edge.</param>
        /// <param name="weight">The weight of the edge.</param>
        public override void AddEdge(Vertex from, Vertex to, int weight)
        {
            bool curveFlag = false;
            if (!_vertices.Contains(from) || !_vertices.Contains(to))
            {
                throw new ArgumentException("Both 'from' and 'to' vertices must be part of the graph.");
            }

            foreach (Edge edge in _edges)
            {
                if (edge.From == from && edge.To == to)
                {
                    return;
                }
                if (edge.From == to && edge.To == from) // Check for reverse edge
                {
                    edge.IsCurved = true;
                    curveFlag = true; // A flag we'll set to true
                }
            }

            Edge newEdge = new Edge(from, to, weight, isDirected: true);
            newEdge.IsCurved = curveFlag;
            _edges.Add(newEdge);
        }

        /// <summary>
        /// Gets the neighboring vertices of the given vertex in a directed graph.
        /// </summary>
        /// <param name="v">The vertex to find neighbors for.</param>
        /// <returns>A list of neighboring vertices.</returns>
        public override List<Vertex> GetNeighbors(Vertex v) =>
            _edges
                .Where(edge => edge.From == v)
                .Select(edge => edge.To)
                .ToList();
    }
}
