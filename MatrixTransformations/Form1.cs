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
        Square square_1, square_2, square_3;

        // Transform matrices 
        Matrix Mat_Scale = Matrix.ScaleMatrix((float)1.5);
        int rotation = 20; 
        Matrix Mat_Rot = Matrix.RotateMatrix(20);

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        public Form1()
        {
            InitializeComponent();

            this.Width = WIDTH;
            this.Height = HEIGHT;
            this.DoubleBuffered = true;

            Vector v1 = new Vector();
            Console.WriteLine(v1);
            Vector v2 = new Vector(1, 2);
            Console.WriteLine(v2);
            Vector v3 = new Vector(2, 6);
            Console.WriteLine(v3);
            Vector v4 = v2 + v3;
            Console.WriteLine(v4); // 3, 8

            Matrix m1 = new Matrix();
            Console.WriteLine(m1); // 1, 0, 0, 1
            Matrix m2 = new Matrix(
                 2,  4,
                -1,  3);
            Console.WriteLine(m2);
            Console.WriteLine(m1 + m2); // 3, 4, -1, 4
            Console.WriteLine(m1 - m2); // -1, -4, 1, -2
            Console.WriteLine(m2 * m2); // 0, 20, -5, 5

            Console.WriteLine(m2 * v3); // 28, 16

            // Define axes
            x_axis = new AxisX(200);
            y_axis = new AxisY(200);

            // Create object
            square_1 = new Square(Color.Purple,100);
            square_2 = new Square(Color.Cyan, 100);
            square_3 = new Square(Color.Orange, 100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Buffer for transformations
            List<Vector> vectorBuffer = new List<Vector>();

            // Draw square_1 (purple)
            vectorBuffer.Clear();
            foreach (Vector v in square_1.vb)
            {
                vectorBuffer.Add(v.Copy());
            }
            ViewPortTransformation(vectorBuffer);
            square_1.Draw(e.Graphics, vectorBuffer);

            // Draw square_2 (cyan)
            vectorBuffer.Clear();
            foreach(Vector v in square_2.vb)
            {
                vectorBuffer.Add(v * Mat_Scale);
            }

            ViewPortTransformation(vectorBuffer);
            square_2.Draw(e.Graphics, vectorBuffer);


            // Transformations for square_3 (orange)
            vectorBuffer.Clear();
            foreach (Vector v in square_3.vb)
            {
                vectorBuffer.Add(v * Mat_Rot);
            }

            ViewPortTransformation(vectorBuffer);
            square_3.Draw(e.Graphics, vectorBuffer);

            // Draw axes
            vectorBuffer.Clear();
            foreach (Vector v in x_axis.vb)
            {
                vectorBuffer.Add(v.Copy());
            }
            ViewPortTransformation(vectorBuffer);
            x_axis.Draw(e.Graphics, vectorBuffer);

            vectorBuffer.Clear();
            foreach (Vector v in y_axis.vb)
            {
                vectorBuffer.Add(v.Copy());
            }
            ViewPortTransformation(vectorBuffer);
            y_axis.Draw(e.Graphics, vectorBuffer);

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
                this.Mat_Rot = Matrix.RotateMatrix(this.rotation);
                Console.WriteLine("Current rotation: " + this.rotation + " degrees.");
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
        }

        private void ViewPortTransformation(List<Vector> vectors)
        {
            foreach (Vector v in vectors)
            {
                // Flip Y-axis
                v.y *= -1;

                // Move center
                v.x += (WIDTH / 2);
                v.y += (HEIGHT / 2);
            }
        }
    }
}
