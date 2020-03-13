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
        float dx, dy, dz, rx, ry, rz, scale;

        // Camera
        float r = 10;
        float theta = -100; // θ
        float phi = -10; // φ
        float d = 800;

        // Hue for cube colour
        private int _hue = 30;
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

        // Window dimensions
        const int WIDTH = 800;
        const int HEIGHT = 600;

        // Timer for animations
        private static System.Timers.Timer timer;

        // Animations
        Boolean subPhaseAnimation = true;
        Boolean transX = false;
        Boolean transY = false;
        Boolean transZ = false;
        int phase = 0;
        int animationSpeed = 20;

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

            grid = new Grid(8);

            // Create object
            cube = new Cube(hue);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawControls(e.Graphics);

            // Draw squares
            Matrix transformation = Matrix.TranslateMatrix(new Vector(dx, dy, dz)) * Matrix.RotateX(rx) * Matrix.RotateY(ry) * Matrix.RotateZ(rz) * Matrix.Scale(scale);
            cube.Draw(e.Graphics, ViewingPipeline(cube.vb, transformation));

            grid.Draw(e.Graphics, ViewingPipeline(grid.vb, Matrix.Identity()));

            // Draw axes
            axis_x.Draw(e.Graphics, ViewingPipeline(axis_x.vb, Matrix.Identity()));
            axis_y.Draw(e.Graphics, ViewingPipeline(axis_y.vb, Matrix.Identity()));
            axis_z.Draw(e.Graphics, ViewingPipeline(axis_z.vb, Matrix.Identity()));
        }

        private void DrawControls(Graphics g)
        {
            Font font = new Font("Arial", 10);
            PointF p = new PointF(0,0);
            Color c = cube.colorFromHue(cube.hue);
            string str =
                "Press A to play/pause animation\n\n" +

                "Scale: \t\t" +     Math.Round(this.scale,2)    + "\t(S/s)\n" +
                "TranslateX: \t" +  Math.Round(this.dx, 2) + "\t(Left/Right)\n" +
                "TranslateY: \t" +  Math.Round(this.dy, 2) + "\t(Up/Down)\n" +
                "TranslateZ: \t" +  Math.Round(this.dz, 2) + "\t(PgUp/PgDn)\n" +
                "RotateX: \t" +     Math.Round(this.rx, 2) + "\t(X/x)\n" +
                "RotateY: \t" +     Math.Round(this.ry, 2) + "\t(Y/y)\n" +
                "RotateZ: \t" +     Math.Round(this.rz, 2) + "\t(Z/z)\n" +

                "\nr: \t" +         Math.Round(this.r, 2) + "\t(R/r)\n" +
                "d: \t" +           Math.Round(this.d, 2) + "\t(D/d)\n" +
                "phi: \t" +         Math.Round(this.phi, 2) + "\t(P/p)\n" +
                "theta: \t" +       Math.Round(this.theta, 2) + "\t(T/t)\n" +
                "phase: \t" +       this.phase + "\n" +
                "subphase:\t" +     this.subPhaseAnimation + "\n" +
                "speed:\t" +        this.animationSpeed + "\t(+/-)\n" +
                "TransX:\t" +       this.transX + "\tNum1\n" +
                "TransY:\t" +       this.transY + "\tNum2\n" +
                "TransZ:\t" +       this.transZ + "\tNum3\n" +

                "\nhue: \t" + this.hue + "\t(H/h)\n" +
                "R:" + c.R + "\t" + "G:" + c.G + "\t" + "B:" + c.B;
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
        private void StartAnimation()
        {
            if (timer == null || !timer.Enabled)
            {
                this.phase = 1;

                // Start timer, do animationstep every 50 ms
                timer = new System.Timers.Timer(this.animationSpeed);
                timer.Elapsed += AnimationStep;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }

        private void StopAnimation()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        private void AnimationStep(Object source, ElapsedEventArgs e)
        {
            cube.hue = hue++;
            hue %= 360;

            switch (this.phase)
            {
                case 1:
                    if (this.subPhaseAnimation)
                    {
                        if (this.scale < 1.5)
                        {
                            this.scale += 0.01f;
                        }
                        else
                        {
                            this.subPhaseAnimation = false;
                        }
                    }
                    else
                    {
                        if (this.scale > 1)
                        {
                            this.scale -= 0.01f;
                        }
                        else
                        {
                            this.subPhaseAnimation = true;
                            this.phase = 2;
                        }
                    }
                    if (this.transX)
                    {
                        this.dx += 0.01f;
                    }
                    if (this.transY)
                    {
                        this.dy += 0.01f;
                    }
                    if (this.transZ)
                    {
                        this.dz += 0.01f;
                    }
                    this.theta--;
                    break;
                case 2:
                    if (this.subPhaseAnimation)
                    {
                        if (this.rx < 45)
                        {
                            this.rx++;
                        }
                        else
                        {
                            this.subPhaseAnimation = false;
                        }
                    }
                    else
                    {
                        if (this.rx > 0)
                        {
                            this.rx--;
                        }
                        else
                        {
                            this.subPhaseAnimation = true;
                            this.phase = 3;
                        }
                    }
                    if (this.transX)
                    {
                        this.dx -= 0.01f;
                    }
                    if (this.transY)
                    {
                        this.dy -= 0.01f;
                    }
                    if (this.transZ)
                    {
                        this.dz -= 0.01f;
                    }
                    this.theta--;
                    break;
                case 3:
                    if (this.subPhaseAnimation)
                    {
                        if (this.ry < 45)
                        {
                            this.ry++;
                        }
                        else
                        {
                            this.subPhaseAnimation = false;
                        }
                    }
                    else
                    {
                        if (this.ry > 0)
                        {
                            this.ry--;
                        }
                        else
                        {
                            this.subPhaseAnimation = true;
                            this.phase = 4;
                        }
                    }
                    if (this.transX)
                    {
                        this.dx += 0.01f;
                    }
                    if (this.transY)
                    {
                        this.dy += 0.01f;
                    }
                    if (this.transZ)
                    {
                        this.dz += 0.01f;
                    }
                    this.phi++;
                    break;
                case 4:
                    if (this.theta >= -100 || this.phi != -10 || this.dx != 0 || this.dy != 0 || this.dz != 0)
                    {
                        if (this.theta < -100) this.theta++;
                        if (this.phi > -10) this.phi--;

                        if (this.dx > 0) this.dx -= 0.01f;
                        if (this.dx < 0) this.dx += 0.01f;

                        if (this.dy > 0) this.dy -= 0.01f;
                        if (this.dy < 0) this.dy += 0.01f;

                        if (this.dz > 0) this.dz -= 0.01f;
                        if (this.dz < 0) this.dz += 0.01f;

                        // Rounding 
                        this.dx = (float) Math.Round(this.dx, 2);
                        this.dy = (float) Math.Round(this.dy, 2);
                        this.dz = (float) Math.Round(this.dz, 2);
                    }
                    else
                    {
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
        private void ResetValues()
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
            hue = 30;
            cube.hue = hue;
            phase = 0;
            subPhaseAnimation = true;
            animationSpeed = 20;
            StopAnimation();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { Application.Exit(); }
            if (e.KeyCode == Keys.D) { if (e.Shift) { this.d += 1; } else { this.d -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.R) { if (e.Shift) { this.r += 1; } else { this.r -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.T) { if (e.Shift) { this.theta += 1; } else { this.theta -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.P) { if (e.Shift) { this.phi += 1; } else { this.phi -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.X) { if (e.Shift) { this.rx += 1; } else { this.rx -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.Y) { if (e.Shift) { this.ry += 1; } else { this.ry -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.Z) { if (e.Shift) { this.rz += 1; } else { this.rz -= 1; } Invalidate(); }
            if (e.KeyCode == Keys.S) { if (e.Shift) { this.scale += (float)0.1; } else { this.scale -= (float)0.1; } Invalidate(); }
            if (e.KeyCode == Keys.C) { ResetValues(); Invalidate(); }
            if (e.KeyCode == Keys.A) { if (timer == null || !timer.Enabled) { StartAnimation(); } else { StopAnimation(); } }
            if (e.KeyCode == Keys.H) { if (e.Shift) { hue--; } else { hue++; } hue %= 360; cube.hue = hue; Invalidate(); }

            if (e.KeyCode == Keys.NumPad1) { this.transX = !this.transX; Invalidate(); }
            if (e.KeyCode == Keys.NumPad2) { this.transY = !this.transY; Invalidate(); }
            if (e.KeyCode == Keys.NumPad3) { this.transZ = !this.transZ; Invalidate(); }

            if (e.KeyCode == Keys.Left) { this.dx -= 1; Invalidate(); }
            if (e.KeyCode == Keys.Right) { this.dx += 1; Invalidate(); }
            if (e.KeyCode == Keys.Down) { this.dy -= 1; Invalidate(); }
            if (e.KeyCode == Keys.Up) { this.dy += 1; Invalidate(); }
            if (e.KeyCode == Keys.PageDown) { this.dz -= 1; Invalidate(); }
            if (e.KeyCode == Keys.PageUp) { this.dz += 1; Invalidate(); }
            if (e.KeyCode == Keys.Add) { 
                if (this.animationSpeed > 1) 
                    this.animationSpeed -= 1;
                timer.Interval = this.animationSpeed;
            }
            if (e.KeyCode == Keys.Subtract) { 
                this.animationSpeed += 1;
                timer.Interval = this.animationSpeed;
            }

        }
    }
}