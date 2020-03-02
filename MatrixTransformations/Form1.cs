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
            this.Mat_Trl = Matrix.TranslateMatrix(translation);

            // Define axes
            x_axis = new AxisX(200);
            y_axis = new AxisY(200);

            // Create object
            square_1 = new Square(Color.Purple,100);
            square_2 = new Square(Color.Cyan, 100);
            square_3 = new Square(Color.Orange, 100);
            square_4 = new Square(Color.DarkBlue, 100);

            // TESTS (I know...)
            Vector v = new Vector(10, 20, 1);
            Matrix m1 = Matrix.TranslateMatrix(new Vector(5, -30, 1));

            Console.WriteLine(new Matrix(v));
            Console.WriteLine(m1);
            Console.WriteLine(m1 * new Matrix(v));

            Console.WriteLine(m1 * v);
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
                vectorBuffer.Add(v * Matrix.Identity());
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


            // Draw square_3 (orange)
            vectorBuffer.Clear();
            foreach (Vector v in square_3.vb)
            {
                vectorBuffer.Add(v * Mat_Rot);
            }

            ViewPortTransformation(vectorBuffer);
            square_3.Draw(e.Graphics, vectorBuffer);

            // Draw square_4 (dark blue)
            vectorBuffer.Clear();
            foreach (Vector v in square_4.vb)
            {
                vectorBuffer.Add(v * Mat_Trl);
            }

            ViewPortTransformation(vectorBuffer);
            square_4.Draw(e.Graphics, vectorBuffer);

            // Draw axes
            vectorBuffer.Clear();
            foreach (Vector v in x_axis.vb)
            {
                vectorBuffer.Add(v * Matrix.Identity());
            }
            ViewPortTransformation(vectorBuffer);
            x_axis.Draw(e.Graphics, vectorBuffer);

            vectorBuffer.Clear();
            foreach (Vector v in y_axis.vb)
            {
                vectorBuffer.Add(v * Matrix.Identity());
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
