using System.Collections.Generic;
using System.Drawing;
using System;
using System.Linq;

namespace GraphEditor.Model.GraphStructure
{
    public abstract class Graph
    {
        protected List<Vertex> _vertices;
        protected List<Edge> _edges;

     
        public Graph()
        {
            _vertices = new List<Vertex>();
            _edges = new List<Edge>();
        }

       
        public List<Vertex> Vertices { get => _vertices; }

        public List<Edge> Edges { get => _edges; }

        public void AddVertex(Vertex vertex)
        {
            foreach (Vertex v in _vertices)
            {
                if (v.Collides(vertex))
                {
                    return;
                }
            }
            _vertices.Add(vertex);
        }

        public int GetEdgeOnWeightTextPosition(PointF mousePosition)
        {
            const float thresholdDistance = 5;

            foreach (Edge edge in Edges)
            {
                PointF textPosition = new PointF((edge.From.Position.X + edge.To.Position.X) / 2, (edge.From.Position.Y + edge.To.Position.Y) / 2);
                float distance = (float)Math.Sqrt((mousePosition.X - textPosition.X) * (mousePosition.X - textPosition.X) +
                                                  (mousePosition.Y - textPosition.Y) * (mousePosition.Y - textPosition.Y));

                if (distance < thresholdDistance)
                {
                    return Edges.IndexOf(edge);
                }
            }

            return -1;
        }

        public abstract void AddEdge(Vertex from, Vertex to, int weight);

        public abstract List<Vertex> GetNeighbors(Vertex v);
        public bool IsValidIndex<T>(int index, List<T> list) => index >= 0 && index < list.Count;

        public void RemoveItemAtIndex<T>(int index, List<T> list) => list?.RemoveAt(index);

        public void RemoveVertex(int vertexIndex)
        {
            RemoveItemAtIndex(vertexIndex, _vertices);

            if (IsValidIndex(vertexIndex, _vertices))
            {
                Vertex vertexToRemove = _vertices[vertexIndex];
                _edges.RemoveAll(edge => edge.From == vertexToRemove || edge.To == vertexToRemove);
            }
        }

        public void RemoveEdge(int edgeIndex) => RemoveItemAtIndex(edgeIndex, _edges);

        public void Clear()
        {
            _vertices.Clear();
            _edges.Clear();
        }

        public int GetVertexOnPosition(PointF position) => _vertices.FindLastIndex(vertex => vertex.ContainsPoint(position));

        public Edge GetEdge(Vertex from, Vertex to)
        {
            Edge directedEdge = _edges.FirstOrDefault(edge => edge.From == from && edge.To == to);
            if (directedEdge != null)
                return directedEdge;

            Edge undirectedEdge = _edges.FirstOrDefault(edge => edge.From == to && edge.To == from);
            return undirectedEdge;
        }

        public void Draw(Graphics graphics)
        {
            _edges.ForEach(edge => edge.Draw(graphics));
            _vertices.ForEach(vertex => vertex.Draw(graphics));
        }

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

        public void HighlightVertex(Vertex vertex) => vertex.IsHighlighted = true;

        public void HighlightEdge(Edge edge) => edge.IsHighlighted = true;

        private void ClearEdgeHighlighting() => _edges.ForEach(edge => edge.IsHighlighted = false);

        private void ClearVertexHighlighting() => _vertices.ForEach(vertex => vertex.IsHighlighted = false);

        public void ClearHighlighting()
        {
            ClearVertexHighlighting();
            ClearEdgeHighlighting();
        }
    }
}
