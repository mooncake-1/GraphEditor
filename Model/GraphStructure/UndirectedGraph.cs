using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.GraphStructure
{
    public class UndirectedGraph : Graph
    {
        public UndirectedGraph() : base()
        {
        }

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

        public override List<Vertex> GetNeighbors(Vertex v) =>
            _edges
                .Where(edge => edge.From == v || edge.To == v)
                .Select(edge => edge.From == v ? edge.To : edge.From)
                .ToList();
    }
}
