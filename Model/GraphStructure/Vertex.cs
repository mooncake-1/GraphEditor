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

    public class Vertex
    {
        public const float RADIUS = 27.5f;

        public int ID { get; }

        public PointF Position { get; set; }

        public int DiscoveryTime { get; set; }

        public int FinishTime { get; set; }

        public bool IsHighlighted { get; set; }

        public VertexColor Color { get; set; }

        private static readonly Dictionary<VertexColor, Brush> colorMapping = new Dictionary<VertexColor, Brush>
        {
            { VertexColor.White, Brushes.LightSkyBlue },
            { VertexColor.Grey, Brushes.DeepSkyBlue },
            { VertexColor.Black, Brushes.DodgerBlue }
        };

        public Brush FillColor
        {
            get
            {
                colorMapping.TryGetValue(Color, out Brush fillBrush);
                return fillBrush ?? Brushes.LightSkyBlue;
            }
        }

        public Vertex(PointF position, int id)
        {
            ID = id;
            Position = position;
            Color = VertexColor.White;
        }

        public override string ToString() => $"{ID}";

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

        public float GetDistance(Vertex otherVertex) =>
            (float)Math.Sqrt(Math.Pow(this.Position.X - otherVertex.Position.X, 2) +
                Math.Pow(this.Position.Y - otherVertex.Position.Y, 2));

        public bool Collides(Vertex otherVertex) => GetDistance(otherVertex) < 2 * RADIUS;

        public bool ContainsPoint(PointF point)
        {
            float distanceSquared = (point.X - this.Position.X) * (point.X - this.Position.X) + (point.Y - this.Position.Y) * (point.Y - this.Position.Y);
            return distanceSquared <= RADIUS * RADIUS;
        }
    }
}
