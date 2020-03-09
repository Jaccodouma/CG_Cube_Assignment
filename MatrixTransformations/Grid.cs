using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixTransformations
{
    class Grid : DrawableObject
    {
        public override List<Vector> vb { get; set; }

        public Grid(int size)
        {
            vb = new List<Vector>();

            for (int i = -size; i <= size; i++)
            {
                vb.Add(new Vector(i, 0, -size));
                vb.Add(new Vector(i, 0, size));
            }

            for (int i = -size; i <= size; i++)
            {
                vb.Add(new Vector(-size, 0, i));
                vb.Add(new Vector(size, 0, i));
            }
        }

        public override void Draw(Graphics g, List<Vector> vb)
        {
            Pen p = new Pen(Color.FromArgb(120, Color.Black), 1f);
            Font font = new Font("Arial", 12, FontStyle.Bold);

            for (int i = 0; i < vb.Count(); i+=2)
            {
                g.DrawLine(p, vb[i].x, vb[i].y, vb[i+1].x, vb[i+1].y);
            }
        }
    }
}
