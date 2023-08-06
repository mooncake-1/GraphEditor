using System;
using System.Drawing;

namespace GraphEditor.Model.GraphStructure
{
    public class Edge
    {
        private readonly Vertex _from;
        private readonly Vertex _to;
        private int _weight;
        private readonly bool _isDirected;

        public Vertex From { get => _from; }
        public Vertex To { get => _to; }
        public int Weight { get => _weight; set => _weight = value; }
        public bool IsDirected { get => _isDirected; }
        public bool IsHighlighted { get; set; }

        public bool IsSelected { get; set; }

        public Edge(Vertex from, Vertex to, int weight, bool isDirected = false)
        {
            _from = from;
            _to = to;
            _weight = weight;
            _isDirected = isDirected;
        }

        public override string ToString() => $"{this.From} -> {this.To}";

        public void Draw(Graphics graphics)
        {
            Color edgeBorderColor = IsSelected ? Color.Red : Color.Black;
            Color edgeFillColor = IsHighlighted ? Color.Green : Color.Black;

            Pen borderPen = new Pen(edgeBorderColor, 4); 
            Pen fillPen = new Pen(edgeFillColor, 2);    

            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (_isDirected)
            {
                DrawDirectedEdge(graphics, borderPen, fillPen);
            }
            else
            {
                DrawUndirectedEdge(graphics, borderPen, fillPen);
            }

            DrawWeightText(graphics);
        }

        private void DrawDirectedEdge(Graphics graphics, Pen borderPen, Pen fillPen)
        {
            PointF midPoint = GetMidPoint();
            PointF unitVector = GetUnitVector();
            DrawEdgeWithArrow(graphics, borderPen, midPoint, unitVector);
            DrawEdgeWithArrow(graphics, fillPen, midPoint, unitVector);
        }

        private void DrawUndirectedEdge(Graphics graphics, Pen borderPen, Pen fillPen)
        {
            graphics.DrawLine(borderPen, _from.Position, _to.Position); 
            graphics.DrawLine(fillPen, _from.Position, _to.Position);   
        }

        private void DrawEdgeWithArrow(Graphics graphics, Pen pen, PointF midPoint, PointF unitVector)
        {
            PointF arrowP1 = new PointF(midPoint.X - 10 * unitVector.X + 5 * unitVector.Y,
                                        midPoint.Y - 10 * unitVector.Y - 5 * unitVector.X);
            PointF arrowP2 = new PointF(midPoint.X - 10 * unitVector.X - 5 * unitVector.Y,
                                        midPoint.Y - 10 * unitVector.Y + 5 * unitVector.X);

            graphics.DrawLine(pen, _from.Position, _to.Position);
            graphics.DrawLine(pen, midPoint, arrowP1);
            graphics.DrawLine(pen, midPoint, arrowP2);
        }



        private PointF GetMidPoint()
            => new PointF((_from.Position.X + _to.Position.X) / 2, (_from.Position.Y + _to.Position.Y) / 2);

        private PointF GetUnitVector()
        {
            float length = CalculateLength();
            return new PointF((_to.Position.X - _from.Position.X) / length, (_to.Position.Y - _from.Position.Y) / length);
        }

        private float CalculateLength()
            => (float)Math.Sqrt((_to.Position.X - _from.Position.X) * (_to.Position.X - _from.Position.X) +
                                (_to.Position.Y - _from.Position.Y) * (_to.Position.Y - _from.Position.Y));

        private void DrawWeightText(Graphics graphics)
        {
            PointF textPosition = GetMidPoint();
            float angle = GetAngle();

            float angleInRadians = (angle + 90) * (float)Math.PI / 180;
            PointF offset = new PointF(20 * (float)Math.Cos(angleInRadians), 20 * (float)Math.Sin(angleInRadians));

            textPosition = new PointF(textPosition.X + offset.X, textPosition.Y + offset.Y);

            DrawTextWithFont(graphics, textPosition, 0);
        }

        private float GetAngle()
            => (float)(Math.Atan2(_to.Position.Y - _from.Position.Y, _to.Position.X - _from.Position.X) * (180 / Math.PI));

        private void DrawTextWithFont(Graphics graphics, PointF textPosition, float angle)
        {
            using (Font font = new Font("Arial", 10))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                graphics.TranslateTransform(textPosition.X, textPosition.Y);
                graphics.RotateTransform(-angle);
                graphics.DrawString(_weight.ToString(), font, Brushes.Black, 0, 0, sf);
                graphics.ResetTransform();
            }
        }

        public float GetDistanceToPoint(PointF point)
        {
            PointF vectorAP = new PointF(point.X - _from.Position.X, point.Y - _from.Position.Y);
            PointF vectorAB = new PointF(_to.Position.X - _from.Position.X, _to.Position.Y - _from.Position.Y);
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
    }
}
