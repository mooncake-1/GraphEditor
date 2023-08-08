using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.GraphStructure
{
    /// <summary>
    /// Represents an undirected graph containing vertices and edges.
    /// </summary>
    public class UndirectedGraph : Graph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedGraph"/> class.
        /// </summary>
        public UndirectedGraph() : base()
        {
        }

        /// <summary>
        /// Adds an undirected edge to the graph between the given vertices with the specified weight.
        /// </summary>
        /// <param name="from">The starting vertex of the edge.</param>
        /// <param name="to">The ending vertex of the edge.</param>
        /// <param name="weight">The weight of the edge.</param>
        public override void AddEdge(Vertex from, Vertex to, int weight)
        {
            if (!_vertices.Contains(from) || !_vertices.Contains(to))
            {
                throw new ArgumentException("Both 'from' and 'to' vertices must be part of the graph.");
            }

            foreach (Edge edge in _edges)
            {
                if ((edge.From == from && edge.To == to) || (edge.From == to && edge.To == from))
                {
                    return;
                }
            }

            _edges.Add(new Edge(from, to, weight, isDirected: false));
        }

        /// <summary>
        /// Gets the neighboring vertices of the given vertex in an undirected graph.
        /// </summary>
        /// <param name="v">The vertex to find neighbors for.</param>
        /// <returns>A list of neighboring vertices.</returns>
        public override List<Vertex> GetNeighbors(Vertex v) =>
            _edges
                .Where(edge => edge.From == v || edge.To == v)
                .Select(edge => edge.From == v ? edge.To : edge.From)
                .ToList();
    }
}
