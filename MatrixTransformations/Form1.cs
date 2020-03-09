using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

namespace MatrixTransformations
{
    public partial class Form1 : Form
    {
        // Objects
        Axis axis_x, axis_y, axis_z;
        Grid grid; 
        Cube cube;

        // Transformations
        float dx, dy, dz, rx, ry, rz, scale, phase; 

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        // Timer for animations
        private static System.Timers.Timer timer;

        public Form1()
        {
            InitializeComponent();

            this.Width = WIDTH;
            this.Height = HEIGHT;

            this.DoubleBuffered = true;

            this.scale = 1; 

            // Define axes
            axis_x = new Axis(Color.Red, "x", new Vector(3, 0, 0));
            axis_y = new Axis(Color.Green, "y", new Vector(0, 3, 0));
            axis_z = new Axis(Color.Blue, "z", new Vector(0, 0, 3));

            grid = new Grid(3);

            // Create object
            cube = new Cube(Color.Orange);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            drawControls(e.Graphics);

            // Draw squares
            Matrix transformation = Matrix.TranslateMatrix(new Vector(dx, dy, dz)) * Matrix.RotateX(rx) * Matrix.RotateY(ry) * Matrix.RotateZ(rz) * Matrix.Scale(scale);
            cube.Draw(e.Graphics, ViewingPipeline(cube.vb, transformation));

            grid.Draw(e.Graphics, ViewingPipeline(grid.vb, Matrix.Identity()));

            // Draw axes
            axis_x.Draw(e.Graphics, ViewingPipeline(axis_x.vb, Matrix.Identity()));
            axis_y.Draw(e.Graphics, ViewingPipeline(axis_y.vb, Matrix.Identity()));
            axis_z.Draw(e.Graphics, ViewingPipeline(axis_z.vb, Matrix.Identity()));
        }

        private void drawControls(Graphics g)
        {
            Font font = new Font("Arial", 10);
            PointF p = new PointF(0,0);
            string str =
                "Scale: \t\t" + this.scale + "\t(S/s)\n" +
                "TranslateX: \t" + this.dx + "\t(Left/Right)\n" +
                "TranslateY: \t" + this.dy + "\t(Up/Down)\n" +
                "TranslateZ: \t" + this.dz + "\t(PgUp/PgDn)\n" +
                "RotateX: \t" + this.rx + "\t(X/x)\n" +
                "RotateY: \t" + this.ry + "\t(Y/y)\n" +
                "RotateZ: \t" + this.rz + "\t(Z/z)\n\n" +
                "r: \t" + this.r + "\t(R/r)\n" +
                "d: \t" + this.d + "\t(D/d)\n" +
                "phi: \t" + this.phi + "\t(P/p)\n" +
                "theta: \t" + this.theta + "\t(T/t)\n" + 
                "phase: \t" + this.phase;
            g.DrawString(str, font, Brushes.Black, p);
        }

        private List<Vector> ViewingPipeline(List<Vector> vectors, Matrix transformation)
        {
            List<Vector> vb = ModelTransformation(vectors, transformation);
            vb = ViewTransformation(vb);
            vb = ProjectionTransformation(vb);
            vb = ViewPortTransformation(vb);
            return vb;
        }

        // Camera
        float r = 10;
        float theta = -100; // θ
        float phi = -10; // φ
        float d = 800;

        private List<Vector> ProjectionTransformation(List<Vector> vectors)
        {
            List<Vector> vb = new List<Vector>();

            foreach (Vector v in vectors)
            {
                Vector n = v * new Matrix(
                    -(d/v.z), 0, 0, 0, 
                    0, (-d/v.z), 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0);
                vb.Add(n);
            }

            return vb;
        }

        private List<Vector> ViewTransformation(List<Vector> vectors)
        {
            List<Vector> vb = new List<Vector>();

            Matrix m = Matrix.InverseMatrix(r, theta, phi);

            foreach (Vector v in vectors)
            {
                // Translate to camera position
                Vector n = v * m;
                vb.Add(n);
            }

            return vb; 
        }

        private List<Vector> ModelTransformation(List<Vector> vectors, Matrix transformation)
        {
            List<Vector> vb = new List<Vector>();

            foreach (Vector v in vectors)
            {
                vb.Add(transformation * v);
            }

            return vb;
        }

        private List<Vector> ViewPortTransformation(List<Vector> vectors)
        {
            List<Vector> vb = new List<Vector>();

            foreach (Vector v in vectors)
            {
                vb.Add(v);
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
        private void startAnimation()
        {
            if (timer == null || !timer.Enabled)
            {
                this.phase = 1;

                // Start timer, do animationstep every 50 ms
                timer = new System.Timers.Timer(50);
                timer.Elapsed += AnimationStep;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }

        private void stopAnimation()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        bool subphase = true; 
        private void AnimationStep(Object source, ElapsedEventArgs e)
        {
            switch (phase)
            {
                case 1:
                    if (subphase)
                    {
                        scale += 0.01f;
                        if (scale >= 1.5) subphase = false; 
                    } else
                    {
                        scale -= 0.01f;
                        if (scale <= 1)
                        {
                            scale = 1;
                            subphase = true; 
                            phase++;
                        }
                    }
                    theta--;
                    break;
                case 2:
                    if (subphase)
                    {
                        rx += 1f;
                        if (rx >= 45) subphase = false;
                    }
                    else
                    {
                        rx -= 1f;
                        if (rx <= 0)
                        {
                            rx = 0;
                            subphase = true;
                            phase++;
                        }
                    }
                    theta--;
                    break;
                case 3:
                    if (subphase)
                    {
                        ry += 1f;
                        if (ry >= 45) subphase = false;
                    }
                    else
                    {
                        ry -= 1f;
                        if (ry <= 0)
                        {
                            ry = 0;
                            subphase = true;
                            phase++;
                        }
                    }
                    phi++;
                    break;
                case 4:
                    if (theta < -100) theta++;
                    if (phi > -10) phi--;
                    if (theta == -100 && phi == -10) phase = 1;
                    break;
                default:
                    phase = 0;
                    stopAnimation();
                    break;
            }
            Invalidate();
        }

        private void resetValues()
        {
            dx = 0;
            dy = 0;
            dz = 0;
            rx = 0;
            ry = 0;
            rz = 0;
            scale = 1;
            r = 10;
            theta = -100;
            phi = -10;
            d = 800;
            stopAnimation();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
            if (e.KeyCode == Keys.Up)
            {
                dy += 0.1f;
                Invalidate();
            }
            if (e.KeyCode == Keys.Down)
            {
                dy -= 0.1f;
                Invalidate();
            }
            if (e.KeyCode == Keys.Right)
            {
                dx += 0.1f;
                Invalidate();
            }
            if (e.KeyCode == Keys.Left)
            {
                dx -= 0.1f;
                Invalidate();
            }

            if (e.KeyCode == Keys.PageUp)
            {
                dz += 0.1f;
                Invalidate();
            }
            if (e.KeyCode == Keys.PageDown)
            {
                dz -= 0.1f;
                Invalidate();
            }

            if (e.KeyCode == Keys.X)
            {
                if (e.Shift)
                {
                    rx++;
                }
                else
                {
                    rx--;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.Y)
            {
                if (e.Shift)
                {
                    ry++;
                }
                else
                {
                    ry--;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.Z)
            {
                if (e.Shift)
                {
                    rz++;
                }
                else
                {
                    rz--;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.S)
            {
                if (e.Shift)
                {
                    scale += 0.1f;
                }
                else
                {
                    scale -= 0.1f;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.R)
            {
                if (e.Shift)
                {
                    r += 0.1f;
                }
                else
                {
                    r -= 0.1f;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.D)
            {
                if (e.Shift)
                {
                    d += 10;
                }
                else
                {
                    d -= 10;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.P)
            {
                if (e.Shift)
                {
                    phi++;
                }
                else
                {
                    phi--;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.T)
            {
                if (e.Shift)
                {
                    theta++;
                }
                else
                {
                    theta--;
                }
                Invalidate();
            }

            if (e.KeyCode == Keys.A)
            {
                startAnimation();
            }

            if (e.KeyCode == Keys.C)
            {
                resetValues();
                Invalidate();
            }
        }
    }
}
