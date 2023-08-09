using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphEditor.Model.GraphStructure
{
    /// <summary>
    /// Represents an edge in a graph with associated properties and methods.
    /// </summary>
    public class Edge
    {
        private const float BorderThickness = 4f;
        private const float FillThickness = 1.75f;
        private const float ArrowLength = 10;
        private const float ArrowWidth = 5;
        private const float TextOffsetLength = 20;

        private readonly Vertex from;
        private readonly Vertex to;
        private int weight;
        private readonly bool isDirected;

        /// <summary>
        /// Gets the starting vertex of the edge.
        /// </summary>
        public Vertex From => from;

        /// <summary>
        /// Gets the ending vertex of the edge.
        /// </summary>
        public Vertex To => to;

        /// <summary>
        /// Gets or sets the weight of the edge.
        /// </summary>
        public int Weight { get => weight; set => weight = value; }

        /// <summary>
        /// Gets a value indicating whether the edge is directed.
        /// </summary>
        public bool IsDirected => isDirected;

        /// <summary>
        /// Gets or sets a value indicating whether the edge is highlighted.
        /// </summary>
        public bool IsHighlighted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the edge is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the edge is curved.
        /// </summary>
        public bool IsCurved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="from">The starting vertex of the edge.</param>
        /// <param name="to">The ending vertex of the edge.</param>
        /// <param name="weight">The weight of the edge.</param>
        /// <param name="isDirected">Indicates whether the edge is directed.</param>
        public Edge(Vertex from, Vertex to, int weight, bool isDirected = false)
        {
            this.from = from ?? throw new ArgumentNullException(nameof(from));
            this.to = to ?? throw new ArgumentNullException(nameof(to));
            this.weight = weight;
            this.isDirected = isDirected;
        }

        /// <summary>
        /// Returns a string representation of the edge.
        /// </summary>
        /// <returns>A string that represents the edge.</returns>
        public override string ToString() => $"{this.From} -> {this.To}";

        /// <summary>
        /// Draws the edge using the provided graphics context.
        /// </summary>
        /// <param name="graphics">The graphics context used for drawing.</param>
        public void Draw(Graphics graphics)
        {
            Color edgeBorderColor = IsSelected ? Color.Red : Color.Black;
            Color edgeFillColor = IsHighlighted ? Color.Green : Color.Black;

            using (Pen borderPen = new Pen(edgeBorderColor, BorderThickness))
            using (Pen fillPen = new Pen(edgeFillColor, FillThickness))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (isDirected)
                {
                    DrawDirectedEdge(graphics, borderPen, fillPen);
                }
                else
                {
                    DrawLine(graphics, borderPen, fillPen, from.Position, to.Position);
                }

                DrawWeightText(graphics);
            }
        }

        private void DrawDirectedEdge(Graphics graphics, Pen borderPen, Pen fillPen)
        {
            PointF midPoint = GetMidPoint();
            PointF unitVector = GetUnitVector();
            PointF arrowP1 = CalculateArrowPoint(midPoint, unitVector, ArrowLength, ArrowWidth);
            PointF arrowP2 = CalculateArrowPoint(midPoint, unitVector, ArrowLength, -ArrowWidth);

            DrawLine(graphics, borderPen, fillPen, from.Position, to.Position);
            graphics.DrawLine(borderPen, midPoint, arrowP1);
            graphics.DrawLine(borderPen, midPoint, arrowP2);
            graphics.DrawLine(fillPen, midPoint, arrowP1);
            graphics.DrawLine(fillPen, midPoint, arrowP2);
        }

        private static void DrawLine(Graphics graphics, Pen borderPen, Pen fillPen, PointF from, PointF to)
        {
            graphics.DrawLine(borderPen, from, to);
            graphics.DrawLine(fillPen, from, to);
        }

        private PointF CalculateArrowPoint(PointF midPoint, PointF unitVector, float length, float width)
        {
            return new PointF(midPoint.X - length * unitVector.X + width * unitVector.Y,
                              midPoint.Y - length * unitVector.Y - width * unitVector.X);
        }

        private PointF GetMidPoint()
            => new PointF((from.Position.X + to.Position.X) / 2, (from.Position.Y + to.Position.Y) / 2);

        private PointF GetUnitVector()
        {
            float length = CalculateLength();
            return new PointF((to.Position.X - from.Position.X) / length, (to.Position.Y - from.Position.Y) / length);
        }

        private float CalculateLength()
            => (float)Math.Sqrt((to.Position.X - from.Position.X) * (to.Position.X - from.Position.X) +
                                (to.Position.Y - from.Position.Y) * (to.Position.Y - from.Position.Y));

        private void DrawWeightText(Graphics graphics)
        {
            PointF textPosition = GetMidPoint();
            float angle = GetAngle();

            float angleInRadians = (angle + 90) * (float)Math.PI / 180;
            PointF offset = new PointF(TextOffsetLength * (float)Math.Cos(angleInRadians), TextOffsetLength * (float)Math.Sin(angleInRadians));

            textPosition = new PointF(textPosition.X + offset.X, textPosition.Y + offset.Y);

            DrawTextWithFont(graphics, textPosition, 0);
        }

        public float GetDistanceToPoint(PointF point)
        {
            PointF vectorAP = new PointF(point.X - from.Position.X, point.Y - from.Position.Y);
            PointF vectorAB = new PointF(to.Position.X - from.Position.X, to.Position.Y - from.Position.Y);
            return CalculateDistance(vectorAP, vectorAB);
        }

        private float CalculateDistance(PointF vectorAP, PointF vectorAB)
        {
            float dotProduct = DotProduct(vectorAP, vectorAB);
            float lengthAB = CalculateLength(vectorAB);
            float distanceAP = CalculateLength(vectorAP);
            float distance = Math.Abs((dotProduct - lengthAB * distanceAP) / lengthAB);
            return (0 <= dotProduct && dotProduct <= lengthAB * lengthAB) ? distance : float.MaxValue;
        }

        private float DotProduct(PointF vectorA, PointF vectorB)
            => vectorA.X * vectorB.X + vectorA.Y * vectorB.Y;

        private float CalculateLength(PointF vector)
            => (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);

        private float GetAngle()
            => (float)(Math.Atan2(to.Position.Y - from.Position.Y, to.Position.X - from.Position.X) * (180 / Math.PI));

        private void DrawTextWithFont(Graphics graphics, PointF textPosition, float angle)
        {
            using (Font font = new Font("Arial", 10))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                graphics.TranslateTransform(textPosition.X, textPosition.Y);
                graphics.RotateTransform(angle);
                graphics.DrawString(weight.ToString(), font, Brushes.Black, 0, 0, sf);
                graphics.ResetTransform();
            }
        }
    }
}
