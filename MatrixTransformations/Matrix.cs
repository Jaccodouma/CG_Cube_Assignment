using System;
using System.Text;

namespace MatrixTransformations
{
    public class Matrix
    {
        float[,] mat = new float[3,3];

        public Matrix()
        { // CREATES IDENTITY MATRIX, UPDATE Identity() IF THIS CHANGES (:
            for (int x=0; x < mat.GetLength(0); x++)
                for (int y = 0; y < mat.GetLength(1); y++)
                    if (x == y) { mat[x, y] = 1; } else { mat[x, y] = 0; }
        }
        //public Matrix(float m11, float m12, float m13,
        //              float m21, float m22, float m23,
        //              float m31, float m32, float m33)
        //{
        //    mat[0, 0] = m11; mat[0, 1] = m12; mat[0, 2] = m13;
        //    mat[1, 0] = m21; mat[1, 1] = m22; mat[1, 2] = m23;
        //    mat[2, 0] = m31; mat[2, 1] = m32; mat[2, 2] = m33;
        //}
        public Matrix(float m11, float m12,
                      float m21, float m22)
        {
            mat[0, 0] = m11; mat[0, 1] = m12;
            mat[1, 0] = m21; mat[1, 1] = m22;
        }

        public Matrix(Vector v)
        {
            mat[0, 0] = v.x;
            mat[1, 0] = v.y;
            //mat[2, 0] = v.w; 
        }

        public Vector ToVector()
        {
            return new Vector(mat[0,0],mat[1,0]/*, mat[2,0]*/);
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix m = new Matrix();
            for (int x = 0; x < m1.mat.GetLength(0); x++)
                for (int y = 0; y < m1.mat.GetLength(1); y++)
                    m.mat[x, y] = m1.mat[x, y] + m2.mat[x, y];
            return m;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix m = new Matrix();
            for (int x = 0; x < m1.mat.GetLength(0); x++)
                for (int y = 0; y < m1.mat.GetLength(1); y++)
                    m.mat[x, y] = m1.mat[x, y] - m2.mat[x, y];
            return m;
        }
        public static Matrix operator *(Matrix m1, float f)
        {
            Matrix m = new Matrix();
            for (int x = 0; x < m1.mat.GetLength(0); x++)
                for (int y = 0; y < m1.mat.GetLength(1); y++)
                    m.mat[x, y] = m1.mat[x, y] * f;
            return m;
        }

        public static Matrix operator *(float f, Matrix m1)
        {
            return m1 * f;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix m = new Matrix();
            for (int x = 0; x < m1.mat.GetLength(0); x++)
                for (int y = 0; y < m1.mat.GetLength(1); y++)
                {
                    float n = 0;
                    for(int z=0; z<m1.mat.GetLength(0); z++)
                        n += m1.mat[x,z] * m2.mat[z,y];
                    m.mat[x, y] = n;
                }
            return m;
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            Matrix vM = new Matrix(v);
            return (m * vM).ToVector();
        }

        public static Vector operator *(Vector v, Matrix m)
        {
            return m * v;
        }

        public static Matrix Identity()
        {
            return new Matrix(); // Default constructor returns identity
        }

        public static Matrix ScaleMatrix(float s)
        {
            return Matrix.Identity() * s;
        }

        public static Matrix RotateMatrix(float degrees)
        {
            degrees = degrees * (float) Math.PI / 180;
            return new Matrix(
                (float)Math.Cos(degrees), (float)-Math.Sin(degrees),
                (float)Math.Sin(degrees), (float)Math.Cos(degrees)
            );
        }


        public override string ToString()
        {
            string s = "";
            for (int x=0; x < mat.GetLength(0); x++)
            {
                if (x==0) { s += "/\t"; } else if (x==mat.GetLength(0)-1) { s += "\\\t"; } else { s += "|\t"; }; // Nice formatting
                for (int y=0; y<mat.GetLength(1); y++)
                {
                    s += mat[x, y] + "\t";
                }
                if (x == 0) { s += "\\"; } else if (x == mat.GetLength(0) - 1) { s += "/"; } else { s += "|"; }; // Nice formatting

                s += "\n";
            }
            return s; 
        }
    }
}
