using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System;

namespace GraphEditor.Model.GraphStructure
{
    /// <summary>
    /// Represents a graph containing vertices and edges.
    /// </summary>
    public abstract class Graph
    {
        private const float ThresholdDistance = 5;

        protected readonly List<Vertex> _vertices;
        protected readonly List<Edge> _edges;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
            _vertices = new List<Vertex>();
            _edges = new List<Edge>();
        }

        /// <summary>
        /// Gets the vertices contained within the graph.
        /// </summary>
        public List<Vertex> Vertices => _vertices;

        /// <summary>
        /// Gets the edges contained within the graph.
        /// </summary>
        public List<Edge> Edges => _edges;

        /// <summary>
        /// Adds a vertex to the graph if it doesn't collide with an existing vertex.
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        public void AddVertex(Vertex vertex)
        {
            if (_vertices.Any(v => v.Collides(vertex)))
            {
                return;
            }
            _vertices.Add(vertex);
        }

        /// <summary>
        /// Finds the index of the edge based on the mouse position.
        /// </summary>
        /// <param name="mousePosition">The mouse position.</param>
        /// <returns>The index of the edge if found, otherwise -1.</returns>
        public int GetEdgeOnWeightTextPosition(PointF mousePosition)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                Edge edge = Edges[i];
                PointF textPosition = new PointF((edge.From.Position.X + edge.To.Position.X) / 2, (edge.From.Position.Y + edge.To.Position.Y) / 2);
                float distanceSquared = CalculateDistanceSquared(mousePosition, textPosition);

                if (distanceSquared < ThresholdDistance * ThresholdDistance)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Adds an edge to the graph between the given vertices with the specified weight.
        /// </summary>
        /// <param name="from">The starting vertex of the edge.</param>
        /// <param name="to">The ending vertex of the edge.</param>
        /// <param name="weight">The weight of the edge.</param>
        public abstract void AddEdge(Vertex from, Vertex to, int weight);

        /// <summary>
        /// Gets the neighboring vertices of the given vertex.
        /// </summary>
        /// <param name="v">The vertex to find neighbors for.</param>
        /// <returns>A list of neighboring vertices.</returns>
        public abstract List<Vertex> GetNeighbors(Vertex v);

        /// <summary>
        /// Removes the vertex at the specified index from the graph.
        /// </summary>
        /// <param name="vertexIndex">The index of the vertex to remove.</param>
        public void RemoveVertex(int vertexIndex)
        {
            if (vertexIndex < 0 || vertexIndex >= _vertices.Count)
            {
                return;
            }

            Vertex vertexToRemove = _vertices[vertexIndex];
            _edges.RemoveAll(edge => edge.From == vertexToRemove || edge.To == vertexToRemove);
            _vertices.RemoveAt(vertexIndex);
        }

        /// <summary>
        /// Removes an item at a specific index from a given list.
        /// </summary>
        /// <typeparam name="T">The type of items in the list.</typeparam>
        /// <param name="index">The index of the item to remove.</param>
        /// <param name="list">The list from which to remove the item.</param>
        private void RemoveItemAtIndex<T>(int index, List<T> list)
        {
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes the edge at the specified index from the graph.
        /// </summary>
        /// <param name="edgeIndex">The index of the edge to remove.</param>
        public void RemoveEdge(int edgeIndex) => RemoveItemAtIndex(edgeIndex, _edges);

        /// <summary>
        /// Clears all vertices and edges from the graph.
        /// </summary>
        public void Clear()
        {
            _vertices.Clear();
            _edges.Clear();
        }

        /// <summary>
        /// Finds the index of the vertex that contains the specified position.
        /// </summary>
        /// <param name="position">The position to search for.</param>
        /// <returns>The index of the vertex if found, otherwise -1.</returns>
        public int GetVertexOnPosition(PointF position) => _vertices.FindIndex(vertex => vertex.ContainsPoint(position));

        /// <summary>
        /// Retrieves an edge between the given vertices.
        /// </summary>
        /// <param name="from">The starting vertex of the edge.</param>
        /// <param name="to">The ending vertex of the edge.</param>
        /// <returns>The edge if found, otherwise null.</returns>
        public Edge GetEdge(Vertex from, Vertex to)
        {
            Edge directedEdge = _edges.FirstOrDefault(edge => edge.From == from && edge.To == to);
            return directedEdge ?? _edges.FirstOrDefault(edge => edge.From == to && edge.To == from);
        }

        /// <summary>
        /// Draws the vertices and edges of the graph.
        /// </summary>
        /// <param name="graphics">The graphics object used for drawing.</param>
        public void Draw(Graphics graphics)
        {
            _edges.ForEach(edge => edge.Draw(graphics));
            _vertices.ForEach(vertex => vertex.Draw(graphics));
        }

        /// <summary>
        /// Finds the index of the edge containing the specified position.
        /// </summary>
        /// <param name="position">The position to search for.</param>
        /// <returns>The index of the edge if found, otherwise -1.</returns>
        public int GetEdgeOnPosition(PointF position)
        {
            int index = _edges.FindLastIndex(edge => IsPointOnEdge(position, edge));
            return index != -1 ? index : -1;
        }

        private bool IsPointOnEdge(PointF point, Edge edge)
        {
            float distance = DistancePointToLine(point, edge.From.Position, edge.To.Position);
            return distance < Vertex.RADIUS; 
        }


        private float DistancePointToLine(PointF point, PointF lineStart, PointF lineEnd)
        {
            float lineLengthSquared = CalculateDistanceSquared(lineStart, lineEnd);

            if (lineLengthSquared == 0)
            {
                return CalculateEuclideanDistance(point, lineStart);
            }

            float t = Math.Max(0, Math.Min(1, CalculateProjectionFactor(point, lineStart, lineEnd, lineLengthSquared)));

            PointF closestPoint = CalculateClosestPointOnLine(point, lineStart, lineEnd, t);

            return CalculateEuclideanDistance(point, closestPoint);
        }


        private float CalculateDistanceSquared(PointF point1, PointF point2)
        {
            float dx = point2.X - point1.X;
            float dy = point2.Y - point1.Y;
            return dx * dx + dy * dy;
        }

        private float CalculateEuclideanDistance(PointF point1, PointF point2)
        {
            float distanceSquared = CalculateDistanceSquared(point1, point2);
            return (float)Math.Sqrt(distanceSquared);
        }

        private float CalculateProjectionFactor(PointF point, PointF lineStart, PointF lineEnd, float lineLengthSquared)
        {
            float t = ((point.X - lineStart.X) * (lineEnd.X - lineStart.X) + (point.Y - lineStart.Y) * (lineEnd.Y - lineStart.Y)) / lineLengthSquared;
            return t;
        }

        private PointF CalculateClosestPointOnLine(PointF point, PointF lineStart, PointF lineEnd, float t)
        {
            float closestPointX = lineStart.X + t * (lineEnd.X - lineStart.X);
            float closestPointY = lineStart.Y + t * (lineEnd.Y - lineStart.Y);
            return new PointF(closestPointX, closestPointY);
        }

        /// <summary>
        /// Highlights the specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to highlight.</param>
        public void HighlightVertex(Vertex vertex) => vertex.IsHighlighted = true;

        /// <summary>
        /// Highlights the specified edge.
        /// </summary>
        /// <param name="edge">The edge to highlight.</param>
        public void HighlightEdge(Edge edge) => edge.IsHighlighted = true;

        /// <summary>
        /// Clears the highlighting from all edges within the graph.
        /// </summary>
        private void ClearEdgeHighlighting() => _edges.ForEach(edge => edge.IsHighlighted = false);

        /// <summary>
        /// Clears the highlighting from all vertices within the graph.
        /// </summary>
        private void ClearVertexHighlighting() => _vertices.ForEach(vertex => vertex.IsHighlighted = false);

        /// <summary>
        /// Clears the highlighting from all vertices and edges within the graph.
        /// </summary>
        public void ClearHighlighting()
        {
            ClearVertexHighlighting();
            ClearEdgeHighlighting();
        }
    }
}