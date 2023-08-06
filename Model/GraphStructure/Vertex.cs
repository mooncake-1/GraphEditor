using GraphEditor.Model.Algorithms.GraphTraversal;
using System;
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
        private readonly int _id;
        private PointF _position;

        public const float RADIUS = 25;

        public int ID => _id;

        public PointF Position
        {
            get => _position;
            set => _position = value;
        }

        public int DiscoveryTime { get; set; }

        public int FinishTime { get; set; }

        public bool IsHighlighted { get; set; }

        public VertexColor Color { get; set; }

        public Brush FillColor
        {
            get
            {
                switch (Color)
                {
                    case VertexColor.White:
                        return Brushes.LightSkyBlue;
                    case VertexColor.Grey:
                        return Brushes.DeepSkyBlue;
                    case VertexColor.Black:
                        return Brushes.DodgerBlue;
                    default:
                        return Brushes.LightSkyBlue;
                }
            }
        }

        public Vertex(PointF position, int id)
        {
            _id = id;
            _position = position;
            Color = VertexColor.White;
        }
        public override string ToString() => $"{ID}";

        public void Draw(Graphics graphics)
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Brush vertexFillBrush = FillColor;
            Pen vertexBorderPen = Pens.Black;

            DrawFilledCircle(graphics, vertexFillBrush);
            DrawCircleBorder(graphics, vertexBorderPen);

            DrawVertexID(graphics);
        }

        private void DrawFilledCircle(Graphics graphics, Brush fillBrush)
        {
            RectangleF boundingRect = new RectangleF(_position.X - RADIUS, _position.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            graphics.FillEllipse(fillBrush, boundingRect);
        }

        private void DrawCircleBorder(Graphics graphics, Pen borderPen)
        {
            RectangleF boundingRect = new RectangleF(_position.X - RADIUS, _position.Y - RADIUS, 2 * RADIUS, 2 * RADIUS);
            graphics.DrawEllipse(borderPen, boundingRect);
        }

        private void DrawVertexID(Graphics graphics)
        {
            string nodeIdText = _id.ToString();
            SizeF textSize = graphics.MeasureString(nodeIdText, SystemFonts.DefaultFont);
            PointF textPosition = new PointF(_position.X - textSize.Width / 2, _position.Y - textSize.Height / 2);
            graphics.DrawString(nodeIdText, SystemFonts.DefaultFont, Brushes.Black, textPosition);
        }

        public float GetDistance(Vertex otherVertex) =>
            (float)Math.Sqrt(Math.Pow(this._position.X - otherVertex._position.X, 2) +
                Math.Pow(this._position.Y - otherVertex._position.Y, 2));

        public bool Collides(Vertex otherVertex) => GetDistance(otherVertex) < 2 * RADIUS;

        public bool ContainsPoint(PointF point)
        {
            float distanceSquared = (point.X - _position.X) * (point.X - _position.X) + (point.Y - _position.Y) * (point.Y - _position.Y);
            return distanceSquared <= RADIUS * RADIUS;
        }
    }
}
