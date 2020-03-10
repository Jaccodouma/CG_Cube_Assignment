using System;
using System.Text;

namespace MatrixTransformations
{
    public class Vector
    {
        public float x, y, z, w;

        public Vector()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 1;
        }

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1;
        }

        public Vector(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector tempVector = new Vector();

            tempVector.x = v1.x + v2.x;
            tempVector.y = v1.y + v2.y;
            tempVector.z = v1.z + v2.z;
            tempVector.w = 1;
            return tempVector;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector tempVector = new Vector();

            tempVector.x = v1.x - v2.x;
            tempVector.y = v1.y - v2.y;
            tempVector.z = v1.z - v2.z;
            tempVector.w = 1;
            return tempVector;
        }

        public static Vector operator *(Vector v1, float constant)
        {
            Vector tempVector = new Vector();

            tempVector.x = v1.x * constant;
            tempVector.y = v1.y * constant;
            tempVector.z = v1.z * constant;
            tempVector.w = 1;
            return tempVector;
        }

        public override string ToString()
        {
            return x + "\n" +
                   y + "\n" +
                   z + "\n" +
                   w + "\n";

        }
    }
}
