using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixTransformations
{
    class Axis : DrawableObject
    {
        private Color color;
        private string text;

        public override List<Vector> vb { get; set; }

        public Axis(Color c, string text, Vector to )
        {
            this.color = c;
            this.text = text;

            vb = new List<Vector>();

            vb.Add(new Vector(0, 0, 0));
            vb.Add(to);
        }

        public override void Draw(Graphics g, List<Vector> vb)
        {
            Pen p = new Pen(color, 2f);
            g.DrawLine(p, vb[0].x, vb[0].y, vb[1].x, vb[1].y);

            Font font = new Font("Arial", 10);
            Brush brush = new SolidBrush(color);

            PointF point = new PointF(vb[1].x, vb[1].y);
            g.DrawString(this.text, font, brush, point);

            brush.Dispose();
        }
    }
}
