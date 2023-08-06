using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphEditor.Model.GraphStructure
{

    public class DirectedGraph : Graph
    {

        public DirectedGraph() : base()
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
                if (edge.From == from && edge.To == to)
                {
                    return;
                }
            }

            _edges.Add(new Edge(from, to, weight, isDirected: true));
        }

        public override List<Vertex> GetNeighbors(Vertex v) =>
            _edges
                .Where(edge => edge.From == v)
                .Select(edge => edge.To)
                .ToList();
    }
}
