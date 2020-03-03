using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {
        // Axes
        AxisX x_axis;
        AxisY y_axis;

        // Objects
        Square square_1, square_2, square_3, square_4;

        // Transform matrices
        Matrix Mat_Scale, Mat_Rot, Mat_Trl;
        int rotation;
        Vector translation;

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        public Form1()
        {
            InitializeComponent();

            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.DoubleBuffered = true;

            // Initialize matrices
            this.rotation = 20;
            this.translation = new Vector(75, -25, 1);
            this.Mat_Scale = Matrix.Scale((float)1.5);
            this.Mat_Rot = Matrix.Rotate(rotation);

            // Define axes
            x_axis = new AxisX(200);
            y_axis = new AxisY(200);

            // Create object
            square_1 = new Square(Color.Purple,100);
            square_2 = new Square(Color.Cyan, 100);
            square_3 = new Square(Color.Orange, 100);
            square_4 = new Square(Color.DarkBlue, 100);

            // TESTS (I know...)
            Vector v = new Vector(100, 100, 1);
            Matrix m1 = Matrix.TranslateMatrix(new Vector(5, -30, 1));

            Console.WriteLine(m1);
            Console.WriteLine(v);

            Console.WriteLine(m1 * v);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw squares
            Matrix m = Matrix.TranslateMatrix(new Vector(-100, 50, 1)) * Matrix.Scale(0.79f) * Matrix.Rotate(45);
            square_1.Draw(e.Graphics, ViewPortTransformation(square_1.vb, m));
            square_2.Draw(e.Graphics, ViewPortTransformation(square_2.vb, Mat_Scale));
            square_3.Draw(e.Graphics, ViewPortTransformation(square_3.vb, Mat_Rot));
            square_4.Draw(e.Graphics, ViewPortTransformation(square_4.vb, Matrix.TranslateMatrix(translation)));

            // Draw axes
            x_axis.Draw(e.Graphics, ViewPortTransformation(x_axis.vb, Matrix.Identity()));
            y_axis.Draw(e.Graphics, ViewPortTransformation(y_axis.vb, Matrix.Identity()));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();

            if (e.KeyCode == Keys.S)
            {
                if (e.Shift)
                {
                    this.rotation -= 1;
                }
                else
                {
                    this.rotation += 1;
                }
                if (this.rotation < 0) this.rotation += 360;
                if (this.rotation >= 360) this.rotation -= 360; // %= still allows negatives? 
                this.Mat_Rot = Matrix.Rotate(this.rotation);
                Invalidate();
            }

            if (e.KeyCode == Keys.Subtract)
            {
                this.Mat_Scale -= Matrix.Identity() * 0.05f;
                Invalidate();
            } else if (e.KeyCode == Keys.Add)
            {
                this.Mat_Scale += Matrix.Identity() * 0.05f;
                Invalidate();
            }

            if (e.KeyCode == Keys.Up)
            {
                this.translation.y++;
                Invalidate();
            }
            if (e.KeyCode == Keys.Down)
            {
                this.translation.y--;
                Invalidate();
            }
            if (e.KeyCode == Keys.Left)
            {
                this.translation.x--;
                Invalidate();
            }
            if (e.KeyCode == Keys.Right)
            {
                this.translation.x++;
                Invalidate();
            }
        }

        private List<Vector> ViewPortTransformation(List<Vector> vectors, Matrix transformation)
        {
            List<Vector> vb = new List<Vector>();

            foreach (Vector v in vectors)
            {
                Vector n = transformation * v;
                vb.Add(n);
            }

            foreach (Vector v in vb)
            {
                // Flip Y-axis
                v.y *= -1;

                // Move center
                v.x += (WIDTH / 2);
                v.y += (HEIGHT / 2);
            }

            return vb;
        }
    }
}
