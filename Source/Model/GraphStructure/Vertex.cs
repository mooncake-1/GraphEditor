using GraphEditor.Model.Algorithms.GraphTraversal;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphEditor.Model.GraphStructure
{
    public enum VertexColor
    {
        White,
        Grey,
        Black,
    }

    /// <summary>
    /// Represents a vertex in a graph with associated properties and methods.
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// The radius of the vertex.
        /// </summary>
        public const float RADIUS = 27.5f;

        /// <summary>
        /// Gets the unique identifier for the vertex.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        public PointF Position { get; set; }

        /// <summary>
        /// Gets or sets the discovery time of the vertex in the context of graph traversal.
        /// </summary>
        public int DiscoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the finish time of the vertex in the context of graph traversal.
        /// </summary>
        public int FinishTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vertex is highlighted.
        /// </summary>
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// Gets or sets the color of the vertex, used to represent its state in graph traversal algorithms.
        /// </summary>
        public VertexColor Color { get; set; }

        private static readonly Dictionary<VertexColor, Brush> colorMapping = new Dictionary<VertexColor, Brush>
    {
        { VertexColor.White, Brushes.LightSkyBlue },
        { VertexColor.Grey, Brushes.DeepSkyBlue },
        { VertexColor.Black, Brushes.DodgerBlue }
    };

        /// <summary>
        /// Gets the fill color of the vertex based on its <see cref="VertexColor"/>.
        /// </summary>
        public Brush FillColor
        {
            get
            {
                colorMapping.TryGetValue(Color, out Brush fillBrush);
                return fillBrush ?? Brushes.LightSkyBlue;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class with the specified position and identifier.
        /// </summary>
        /// <param name="position">The position of the vertex.</param>
        /// <param name="id">The unique identifier of the vertex.</param>
        public Vertex(PointF position, int id)
        {
            ID = id;
            Position = position;
            Color = VertexColor.White;
        }

        /// <summary>
        /// Returns a string representation of the vertex, including its identifier.
        /// </summary>
        /// <returns>A string that represents the vertex.</returns>
        public override string ToString() => $"{ID}";

        /// <summary>
        /// Draws the vertex using the provided graphics context.
        /// </summary>
        /// <param name="graphics">The graphics context used for drawing.</param>
        public void Draw(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Brush vertexFillBrush = FillColor;
            Pen vertexBorderPen = Pens.Black;
            RectangleF boundingRect = new RectangleF(this.Position.X - RADIUS, this.Position.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            DrawFilledCircle(graphics, vertexFillBrush, boundingRect);
            DrawCircleBorder(graphics, vertexBorderPen, boundingRect);
            DrawVertexID(graphics);
        }

        private void DrawFilledCircle(Graphics graphics, Brush fillBrush, RectangleF boundingRect)
            => graphics.FillEllipse(fillBrush, boundingRect);

        private void DrawCircleBorder(Graphics graphics, Pen borderPen, RectangleF boundingRect)
            => graphics.DrawEllipse(borderPen, boundingRect);

        private void DrawVertexID(Graphics graphics)
        {
            string nodeIdText = ID.ToString();
            SizeF textSize = graphics.MeasureString(nodeIdText, SystemFonts.DefaultFont);
            PointF textPosition = new PointF(this.Position.X - textSize.Width / 2, this.Position.Y - textSize.Height / 2);
            graphics.DrawString(nodeIdText, SystemFonts.DefaultFont, Brushes.Black, textPosition);
        }

        /// <summary>
        /// Determines whether this vertex collides with another vertex.
        /// </summary>
        /// <param name="otherVertex">The other vertex.</param>
        /// <returns><c>true</c> if the vertices collide; otherwise, <c>false</c>.</returns>
        public bool Collides(Vertex otherVertex)
            => GetDistanceSquared(otherVertex) < (2 * RADIUS) * (2 * RADIUS);

        private float GetDistanceSquared(Vertex otherVertex)
            => (this.Position.X - otherVertex.Position.X) * (this.Position.X - otherVertex.Position.X)
               + (this.Position.Y - otherVertex.Position.Y) * (this.Position.Y - otherVertex.Position.Y);


        /// <summary>
        /// Determines whether the specified point is contained within this vertex.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns><c>true</c> if the point is contained within this vertex; otherwise, <c>false</c>.</returns>
        public bool ContainsPoint(PointF point)
        {
            float distanceSquared = (point.X - this.Position.X) * (point.X - this.Position.X) + (point.Y - this.Position.Y) * (point.Y - this.Position.Y);
            return distanceSquared <= RADIUS * RADIUS;
        }
    }
}
