using System;
using System.Text;

namespace MatrixTransformations
{
    public class Matrix
    {
        float[,] mat = new float[4, 4];

        public Matrix()
        {
            mat[0, 0] = 1; mat[0, 1] = 0; mat[0, 2] = 0; mat[0, 3] = 0;
            mat[1, 0] = 0; mat[1, 1] = 1; mat[1, 2] = 0; mat[1, 3] = 0;
            mat[2, 0] = 0; mat[2, 1] = 0; mat[2, 2] = 1; mat[2, 3] = 0;
            mat[3, 0] = 0; mat[3, 1] = 0; mat[3, 2] = 0; mat[3, 3] = 1;
        }
        public Matrix(
                    float m11, float m12, float m13, float m14,
                    float m21, float m22, float m23, float m24,
                    float m31, float m32, float m33, float m34,
                    float m41, float m42, float m43, float m44)
        {
            mat[0, 0] = m11; mat[0, 1] = m12; mat[0, 2] = m13; mat[0, 3] = m14;
            mat[1, 0] = m21; mat[1, 1] = m22; mat[1, 2] = m23; mat[1, 3] = m24;
            mat[2, 0] = m31; mat[2, 1] = m32; mat[2, 2] = m33; mat[2, 3] = m34;
            mat[3, 0] = m41; mat[3, 1] = m42; mat[3, 2] = m43; mat[3, 3] = m44;

        }

        public Matrix(Vector v)
        {
            mat[0, 0] = v.x; mat[0, 1] = 0; mat[0, 2] = 0; mat[0, 3] = 0;
            mat[1, 0] = 0; mat[1, 1] = v.y; mat[1, 2] = 0; mat[1, 3] = 0;
            mat[2, 0] = 0; mat[2, 1] = 0; mat[2, 2] = v.z; mat[2, 3] = 0;
            mat[3, 0] = 0; mat[3, 1] = 0; mat[3, 2] = 0; mat[3, 3] = 1;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix tempMatrix = new Matrix();
            for (int i = 0; i < m1.mat.GetLength(0); i++)
            {
                for (int x = 0; x < m1.mat.GetLength(1); x++)
                {
                    tempMatrix.mat[i, x] = m1.mat[i, x] + m2.mat[i, x];
                }
            }
            return tempMatrix;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix tempMatrix = new Matrix();
            for (int i = 0; i < m1.mat.GetLength(0); i++)
            {
                for (int x = 0; x < m1.mat.GetLength(1); x++)
                {
                    tempMatrix.mat[i, x] = m1.mat[i, x] - m2.mat[i, x];
                }
            }
            return tempMatrix;
        }
        public static Matrix operator *(Matrix m1, float f)
        {
            Matrix newMatrix = new Matrix();
            for (int i = 0; i < m1.mat.GetLength(0); i++)
            {
                for (int x = 0; x < m1.mat.GetLength(1); x++)
                {
                    newMatrix.mat[i, x] = m1.mat[i, x] * f;
                }
            };

            return newMatrix;
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
                    for (int z = 0; z < m1.mat.GetLength(0); z++)
                        n += m1.mat[x, z] * m2.mat[z, y];
                    m.mat[x, y] = n;
                }
            return m;
        }

        public static Vector operator *(Matrix m1, Vector v)
        {
            Vector newVector = new Vector();

            for (int x = 0; x < m1.mat.GetLength(0); x++)
            {
                float axisValue = 0;
                for (int y = 0; y < m1.mat.GetLength(1); y++)
                {
                    if (y == 0) axisValue += m1.mat[x, y] * v.x;
                    if (y == 1) axisValue += m1.mat[x, y] * v.y;
                    if (y == 2) axisValue += m1.mat[x, y] * v.z;
                    if (y == 3) axisValue += m1.mat[x, y] * v.w;
                }
                if (x == 0) newVector.x = axisValue;
                if (x == 1) newVector.y = axisValue;
                if (x == 2) newVector.z = axisValue;
                if (x == 3) newVector.w = axisValue;

            }
            return newVector;
        }

        public static Matrix Identity()
        {
            return new Matrix();
        }

        public Vector ToVector()
        {
            return new Vector(mat[0, 0], mat[1, 0], mat[2, 0]);
        }

        public override string ToString()
        {
            string returnS = "";
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int y = 0; y < mat.GetLength(1); y++)
                {
                    if (y == 0) returnS += mat[i, y] + "\t";
                    if (y == 1) returnS += mat[i, y] + "\t";
                    if (y == 2) returnS += mat[i, y] + "\t";
                    if (y == 3) returnS += mat[i, y] + "\n";
                }
            }
            return returnS;
        }

        public static Matrix ScaleMatrix(float s)
        {
            Matrix scaler = Identity() * s;
            scaler.mat[3, 3] = 1;
            return scaler;
        }

        public static Matrix RotateMatrixZ(float degrees)
        {
            float r = Matrix.toRadian(degrees);
            float cos = (float)Math.Cos(r);
            float sin = (float)Math.Sin(r);
            return new Matrix(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
                );
        }

        public static Matrix RotateMatrixX(float degrees)
        {
            float r = Matrix.toRadian(degrees);
            float cos = (float)Math.Cos(r);
            float sin = (float)Math.Sin(r);
            return new Matrix(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1
                );
        }
        public static Matrix RotateMatrixY(float degrees)
        {
            float r = Matrix.toRadian(degrees);
            float cos = (float)Math.Cos(r);
            float sin = (float)Math.Sin(r);
            return new Matrix(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1
                );
        }

        public static Matrix TranslateMatrix(Vector t)
        {
            return new Matrix(
                    1, 0, 0, t.x,
                    0, 1, 0, t.y,
                    0, 0, 1, t.z,
                    0, 0, 0, 1
                );
        }

        public static Matrix ViewTransformation(float r, float theta, float phi)
        {
            float t = Matrix.toRadian(theta);
            float p = Matrix.toRadian(phi);
            float sinT = (float)Math.Sin(t);
            float cosT = (float)Math.Cos(t);
            float sinP = (float)Math.Sin(p);
            float cosP = (float)Math.Cos(p);
            Matrix viewTransformation = new Matrix(
                    -sinT,          cosT,           0,      0,
                    -(cosT * cosP), -(cosP * sinT), sinP,   0,
                    (cosT * sinP),  (sinT * sinP),  cosP,   -r,
                    0,              0,              0,      1
                );
            return viewTransformation;
        }

        public static Matrix ProjectionTransformation(float d, float z)
        {
            Matrix proj = new Matrix(
                    -(d / z), 0, 0, 0,
                    0, -(d / z), 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0
                );

            return proj;
        }

        public static float toRadian(float degrees)
        {
            return degrees * (float)Math.PI / 180;
        }

    }
}
