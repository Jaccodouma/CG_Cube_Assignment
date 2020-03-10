using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {

        //timer 
        private static System.Timers.Timer timer;
        // Axes
        AxisX x_axis;
        AxisY y_axis;
        AxisZ z_axis;

        // Objects
        Square square, square1, square2;
        Cube cube;

        float r = 10;
        float theta = -100;
        float phi = -10;
        float d = 800;
        float rX = 0;
        float rY = 0;
        float rZ = 0;
        float aX = 0;
        float aY = 0;
        float aZ = 0;
        float s = 0;
        Boolean subPhaseAnimation = true;
        Boolean transX = false;
        Boolean transY = false;
        Boolean transZ = false;
        int phase = 0;
        int animationSpeed = 50;



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
            Vector v2 = new Vector(1, 2, 0);
            Console.WriteLine(v2);
            Vector v3 = new Vector(2, 6, 0);
            Console.WriteLine(v3);
            Vector v4 = v2 + v3;
            Console.WriteLine(v4); // 3, 8

            Matrix m1 = new Matrix();
            Console.WriteLine(m1); // 1, 0, 0, 1
            Matrix m2 = new Matrix(
                2, 4, 0, 0,
                -1, 3, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
            Console.WriteLine(m2);
            Console.WriteLine(m1 + m2); // 3, 4, -1, 4
            Console.WriteLine(m1 - m2); // -1, -4, 1, -2
            Console.WriteLine(m2 * m2); // 0, 20, -5, 5

            Console.WriteLine(m2 * v3); // 28, 16

            // Define axes
            x_axis = new AxisX(3);
            y_axis = new AxisY(3);
            z_axis = new AxisZ(3);

            // Create object
            square = new Square(Color.Purple, 100);
            square1 = new Square(Color.Cyan, 100);
            square2 = new Square(Color.Orange, 100);

            cube = new Cube(Color.Orange);
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            //Defining transformation matrixes and new vector buffer to peform mutations on
            Matrix S = Matrix.ScaleMatrix(this.s);
            //Matrix S = Matrix.ScaleMatrix((float)100);
            Matrix Rx = Matrix.RotateMatrixX(this.rX);
            Matrix Ry = Matrix.RotateMatrixY(this.rY);
            Matrix Rz = Matrix.RotateMatrixZ(this.rZ);
            Matrix T = Matrix.TranslateMatrix(new Vector(this.aX, this.aY, this.aZ));
            Matrix Q = T * Rx * Ry * Rz * S;
            List<Vector> vectorBuffer = new List<Vector>();

            //mutate and draw axis
            x_axis.Draw(e.Graphics, viewingPipeline(x_axis.vb));
            y_axis.Draw(e.Graphics, viewingPipeline(y_axis.vb));
            z_axis.Draw(e.Graphics, viewingPipeline(z_axis.vb));

            vectorBuffer.Clear();
            foreach (Vector v in cube.vertexbuffer)
            {
                vectorBuffer.Add(Q * v);
            }

            //adjusting camera angle
            //adjusting WF position
            cube.Draw(e.Graphics, viewingPipeline(vectorBuffer));

            //Draw all parameters
            DrawInfo(e.Graphics);
            base.OnPaint(e);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape) { Application.Exit(); }
            if (e.KeyCode == Keys.D) { if (e.Shift) { this.d += 1; } else { this.d -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.R) { if (e.Shift) { this.r += 1; } else { this.r -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.T) { if (e.Shift) { this.theta += 1; } else { this.theta -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.P) { if (e.Shift) { this.phi += 1; } else { this.phi -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.X) { if (e.Shift) { this.rX += 1; } else { this.rX -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.Y) { if (e.Shift) { this.rY += 1; } else { this.rY -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.Z) { if (e.Shift) { this.rZ += 1; } else { this.rZ -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.S) { if (e.Shift) { this.s += (float)0.1; } else { this.s -= (float)0.1; } Invalidate(); }
            if (e.KeyCode == Keys.C) { if (e.Shift) { Reset(); Invalidate(); } }
            if (e.KeyCode == Keys.A) { if (e.Shift) { StartAnimation(); } else { StopAnimation(); } }
            if (e.KeyCode == Keys.NumPad1) { if (this.transX == true) { this.transX = false; } else { this.transX = true; }; }
            if (e.KeyCode == Keys.NumPad2) { if (this.transY == true) { this.transY = false; } else { this.transY = true; }; }
            if (e.KeyCode == Keys.NumPad3) { if (this.transZ == true) { this.transZ = false; } else { this.transZ = true; }; }

            if (e.KeyCode == Keys.Left) { this.aX -= 1; Invalidate(); }
            if (e.KeyCode == Keys.Right) { this.aX += 1; Invalidate(); }
            if (e.KeyCode == Keys.Down) { this.aY -= 1; Invalidate(); }
            if (e.KeyCode == Keys.Up) { this.aY += 1; Invalidate(); }
            if (e.KeyCode == Keys.PageDown) { this.aZ -= 1; Invalidate(); }
            if (e.KeyCode == Keys.PageUp) { this.aZ += 1; Invalidate(); }
            if (e.KeyCode == Keys.Add) { this.animationSpeed -= 1; StopAnimation(); StartAnimation(); Invalidate(); }
            if (e.KeyCode == Keys.Subtract) { this.animationSpeed += 1; StopAnimation(); StartAnimation(); Invalidate(); }

        }
        private void DrawInfo(Graphics g)
        {
            int x = 10;
            int y = 10;
            int up = 18;
            Font font = new Font("Arial", 12);
            PointF p = new PointF(x, y);
            g.DrawString("Scale:\t" + Math.Round(this.s, 2) + "\ts/S", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("TransX:\t" + Math.Round(this.aX, 2) + "\tLeft/Right", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("TransY:\t" + Math.Round(this.aY, 2) + "\tUp/Down", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("TransZ:\t" + Math.Round(this.aZ, 2) + "\tPageUp/PageDown", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("RotX:\t" + Math.Round(this.rX, 2) + "\tx/X", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("RotY:\t" + Math.Round(this.rY, 2) + "\ty/Y", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("RotZ:\t" + Math.Round(this.rZ, 2) + "\tz/Z", font, Brushes.Black, p); y += up * 2; p.Y = y;
            g.DrawString("r:\t" + Math.Round(this.r, 1) + "\tr/R", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("d:\t" + Math.Round(this.d, 1) + "\td/D", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("Phi:\t" + Math.Round(this.phi, 2) + "\tp/P", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("Theta:\t" + Math.Round(this.theta, 2) + "\tt/T", font, Brushes.Black, p); y += up * 2; p.Y = y;
            g.DrawString("Phase:\t" + this.phase, font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("Sub:\t" + this.subPhaseAnimation, font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("Speed:\t" + this.animationSpeed + "\t+/-", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("TransX:\t" + this.transX + "\tNumpad1", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("TransY:\t" + this.transY + "\tNumpad2", font, Brushes.Black, p); y += up; p.Y = y;
            g.DrawString("TransZ:\t" + this.transZ + "\tNumpad3", font, Brushes.Black, p); y += up; p.Y = y;

            Invalidate();
        }

        private void Animate(Object source, ElapsedEventArgs e)
        {
            switch (this.phase)
            {
                case 1:
                    if (this.subPhaseAnimation)
                    {
                        if (this.s < 1.5)
                        {
                            this.s += 0.01f;
                        }
                        else
                        {
                            this.subPhaseAnimation = false;
                        }
                    }
                    else
                    {
                        if (this.s >= 1)
                        {
                            this.s -= 0.01f;
                        }
                        else
                        {
                            this.subPhaseAnimation = true;
                            this.phase = 2;
                        }
                    }
                    if (this.transX)
                    {
                        this.aX += 0.01f;
                    }
                    if (this.transY)
                    {
                        this.aY += 0.01f;
                    }
                    if (this.transZ)
                    {
                        this.aZ += 0.01f;
                    }
                    this.theta--;
                    cube = new Cube(Color.Red);
                    break;
                case 2:
                    if (this.subPhaseAnimation)
                    {
                        if (this.rX < 45)
                        {
                            this.rX++;
                        }
                        else
                        {
                            this.subPhaseAnimation = false;
                        }
                    }
                    else
                    {
                        if (this.rX > 0)
                        {
                            this.rX--;
                        }
                        else
                        {
                            this.subPhaseAnimation = true;
                            this.phase = 3;
                        }
                    }
                    if (this.transX)
                    {
                        this.aX -= 0.01f;
                    }
                    if (this.transY)
                    {
                        this.aY -= 0.01f;
                    }
                    if (this.transZ)
                    {
                        this.aZ -= 0.01f;
                    }
                    this.theta--;
                    cube = new Cube(Color.Blue);
                    break;
                case 3:
                    if (this.subPhaseAnimation)
                    {
                        if (this.rY < 45)
                        {
                            this.rY++;
                        }
                        else
                        {
                            this.subPhaseAnimation = false;
                        }
                    }
                    else
                    {
                        if (this.rY > 0)
                        {
                            this.rY--;
                        }
                        else
                        {
                            this.subPhaseAnimation = true;
                            this.phase = 4;
                        }
                    }
                    cube = new Cube(Color.Purple);
                    if (this.transX)
                    {
                        this.aX += 0.01f;
                    }
                    if (this.transY)
                    {
                        this.aY += 0.01f;
                    }
                    if (this.transZ)
                    {
                        this.aZ += 0.01f;
                    }
                    this.phi++;
                    break;
                case 4:
                    if (this.theta > -100 || this.phi != -10 || this.aX != 0 || this.aY != 0 || this.aZ != 0)
                    {
                        if (this.theta < -100) this.theta++;
                        if (this.phi > -10) this.phi--;

                        if (this.aX > 0) this.aX -= 0.01f;
                        if (this.aX < 0) this.aX += 0.01f;

                        if (this.aY > 0) this.aY -= 0.01f;
                        if (this.aY < 0) this.aY += 0.01f;

                        if (this.aZ > 0) this.aZ -= 0.01f;
                        if (this.aZ < 0) this.aZ += 0.01f;
                    }
                    else
                    {
                        Reset();
                        phase = 1;
                    }
                    break;
                default:
                    this.phase = 0;
                    StopAnimation();
                    break;
            }
            Invalidate();
        }
        private List<Vector> ViewPortTransformation(List<Vector> list)
        {
            List<Vector> newList = new List<Vector>();
            Vector vNew = new Vector();
            foreach (Vector v in list)
            {
                vNew = v;
                //flip axis
                vNew.y = v.y * -1;

                //Recalculate to center
                //By using this.Width/Height the viewPort will be responsive to resizing the form
                vNew.x = v.x + (this.Width / 2);
                vNew.y = v.y + (this.Height / 2);
                newList.Add(vNew);
            }

            return newList;
        }

        private List<Vector> viewingPipeline(List<Vector> list)
        {
            List<Vector> newList = new List<Vector>();
            Vector vector = new Vector();

            foreach (Vector v in list)
            {
                Matrix view = Matrix.ViewTransformation(r, theta, phi);
                vector = view * v;


                Matrix proj = Matrix.ProjectionTransformation(d, vector.z);
                vector = proj * vector;
                newList.Add(vector);
            }
            return ViewPortTransformation(newList);
        }
        private void Reset()
        {
            this.r = 10;
            this.theta = -100;
            this.phi = -10;
            this.d = 800;
            this.rX = 0;
            this.rY = 0;
            this.rZ = 0;
            this.aX = 0;
            this.aY = 0;
            this.aZ = 0;
            this.s = 1;
            this.phase = 0;
            this.subPhaseAnimation = true;
            this.animationSpeed = 50;
        }

        public void StartAnimation()
        {
            if (timer == null || !timer.Enabled)
            {
                this.phase = 1;
                timer = new System.Timers.Timer(this.animationSpeed);
                timer.Elapsed += Animate;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }

        public void StopAnimation()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
