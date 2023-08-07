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

        public override List<Vertex> GetNeighbors(Vertex v) =>
            _edges
                .Where(edge => edge.From == v)
                .Select(edge => edge.To)
                .ToList();
    }
}
