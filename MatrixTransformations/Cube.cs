using System.Collections.Generic;
using System.Drawing;

namespace MatrixTransformations
{
    public class Cube : DrawableObject
    {

        //          7----------4
        //         /|         /|
        //        / |        / |                y
        //       /  6-------/--5                |
        //      3----------0  /                 ----x
        //      | /        | /                 /
        //      |/         |/                  z
        //      2----------1

        private const int size = 1;
        public override List<Vector> vb { get; set; }

        private int _hue;
        public int hue
        {
            get
            {
                return this._hue;
            }
            set
            {
                while (value < 360) value += 360;
                value %= 360;
                this._hue = value; 
            }
        }

        public Cube(int hue) {
            this.hue = hue;
            vb = new List<Vector>
            {
                new Vector( 1.0f,  1.0f, 1.0f),     //0
                new Vector( 1.0f, -1.0f, 1.0f),     //1
                new Vector(-1.0f, -1.0f, 1.0f),     //2
                new Vector(-1.0f,  1.0f, 1.0f),     //3

                new Vector( 1.0f,  1.0f, -1.0f),    //4
                new Vector( 1.0f, -1.0f, -1.0f),    //5
                new Vector(-1.0f, -1.0f, -1.0f),    //6
                new Vector(-1.0f,  1.0f, -1.0f),    //7

                new Vector( 1.2f,  1.2f, 1.2f),     //0
                new Vector( 1.2f, -1.2f, 1.2f),     //1
                new Vector(-1.2f, -1.2f, 1.2f),     //2
                new Vector(-1.2f,  1.2f, 1.2f),     //3

                new Vector( 1.2f,  1.2f, -1.2f),    //4
                new Vector( 1.2f, -1.2f, -1.2f),    //5
                new Vector(-1.2f, -1.2f, -1.2f),    //6
                new Vector(-1.2f,  1.2f, -1.2f)     //7
            };
        }

        public override void Draw(Graphics g, List<Vector> vb)
        {
            Pen pen = new Pen(colorFromHue(hue), 3f);
            g.DrawLine(pen, vb[0].x, vb[0].y, vb[1].x, vb[1].y);    //0 -> 1
            g.DrawLine(pen, vb[1].x, vb[1].y, vb[2].x, vb[2].y);    //1 -> 2
            g.DrawLine(pen, vb[2].x, vb[2].y, vb[3].x, vb[3].y);    //2 -> 3
            g.DrawLine(pen, vb[3].x, vb[3].y, vb[0].x, vb[0].y);    //3 -> 0

            g.DrawLine(pen, vb[4].x, vb[4].y, vb[5].x, vb[5].y);    //4 -> 5
            g.DrawLine(pen, vb[5].x, vb[5].y, vb[6].x, vb[6].y);    //5 -> 6
            g.DrawLine(pen, vb[6].x, vb[6].y, vb[7].x, vb[7].y);    //6 -> 7
            g.DrawLine(pen, vb[7].x, vb[7].y, vb[4].x, vb[4].y);    //7 -> 4

            //pen.DashStyle = DashStyle.DashDot;
            g.DrawLine(pen, vb[0].x, vb[0].y, vb[4].x, vb[4].y);    //0 -> 4
            g.DrawLine(pen, vb[1].x, vb[1].y, vb[5].x, vb[5].y);    //1 -> 5
            g.DrawLine(pen, vb[2].x, vb[2].y, vb[6].x, vb[6].y);    //2 -> 6
            g.DrawLine(pen, vb[3].x, vb[3].y, vb[7].x, vb[7].y);    //3 -> 7

            Font font = new Font("Arial", 12, FontStyle.Bold);
            for (int i = 0; i < 8; i++)
            {
                PointF p = new PointF(vb[i + 8].x, vb[i + 8].y);
                g.DrawString(i.ToString(), font, Brushes.Black, p);
            }
        }
        public Color colorFromHue(int hue)
        {
            hue = hue % 360;
            while (hue < 0) hue += 360;
            int r = 0, g = 0, b = 0;

            if (hue < 60)
            {
                r = 255;
                g = (hue % 60) * (255 / 60);
                b = 0;
            }
            else if (hue < 120)
            {
                r = 255 - (hue % 60) * (255 / 60);
                g = 255;
                b = 0;
            }
            else if (hue < 180)
            {
                r = 0;
                g = 255;
                b = (hue % 60) * (255 / 60);
            }
            else if (hue < 240)
            {
                r = 0;
                g = 255 - (hue % 60) * (255 / 60);
                b = 255;
            }
            else if (hue < 300)
            {
                r = (hue % 60) * (255 / 60);
                g = 0;
                b = 255;
            }
            else if (hue < 360)
            {
                r = 255;
                g = 0;
                b = 255 - (hue % 60) * (255 / 60);
            };
            return Color.FromArgb(r, g, b);
        }
    }   
}
