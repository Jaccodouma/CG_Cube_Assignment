﻿using System;
using System.Text;

namespace MatrixTransformations
{
    public class Vector
    {
        public float x, y, z, w;

        public Vector()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }
        public Vector(float x, float y): this(x, y, 1, 1) { }
        public Vector(float x, float y, float z): this(x, y, z, 1) { }
        public Vector(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z; 
            this.w = w;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(
                v1.x + v2.x, 
                v1.y + v2.y, 
                v1.z + v2.z,
                v1.w + v2.w);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(
                v1.x - v2.x, 
                v1.y - v2.y, 
                v1.z - v2.z, 
                v1.w - v2.w);
        }

        public static Vector operator *(Vector v1, float multiplier)
        {
            return new Vector(
                v1.x * multiplier, 
                v1.y * multiplier, 
                v1.z * multiplier, 
                v1.w * multiplier);
        }

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}, Z: {2}, W: {3}", this.x, this.y, this.z, this.w);
        }
    }
}
