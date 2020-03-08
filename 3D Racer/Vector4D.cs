﻿using System;

namespace _3D_Racer
{
    /// <summary>
    /// Handles constructors and operations involving four-dimensional vectors with focus on their use in 3D graphics
    /// </summary>
    public class Vector4D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector4D(float x, float y, float z, float w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4D(float[] data)
        {
            X = data[0];
            Y = data[1];
            Z = data[2];
            W = data[3];
        }

        public Vector4D(Vector3D v, float w = 1)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        #region Common Vectors
        public static readonly Vector4D Zero = new Vector4D(0, 0, 0);
        public static readonly Vector4D Unit_X = new Vector4D(1, 0, 0);
        public static readonly Vector4D Unit_Y = new Vector4D(0, 1, 0);
        public static readonly Vector4D Unit_Z = new Vector4D(0, 0, 1);
        public static readonly Vector4D Unit_Negative_X = new Vector4D(-1, 0, 0);
        public static readonly Vector4D Unit_Negative_Y = new Vector4D(0, -1, 0);
        public static readonly Vector4D Unit_Negative_Z = new Vector4D(0, 0, -1);
        #endregion

        #region Vector Operations (Operator Overloading)
        public static Vector4D operator +(Vector4D v1, Vector4D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W);
        public static Vector4D operator +(Vector4D v1, Vector3D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W);
        public static Vector4D operator -(Vector4D v1, Vector4D v2) => new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);
        public static float operator *(Vector4D v1, Vector4D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        public static Vector4D operator *(Vector4D v, float scalar) => new Vector4D(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);
        public static Vector4D operator /(Vector4D v, float scalar) => new Vector4D(v.X / scalar, v.Y / scalar, v.Z / scalar, v.W / scalar);
        public static Vector4D operator -(Vector4D v) => new Vector4D(-v.X, -v.Y, -v.Z, -v.W);
        #endregion

        #region Vector Operations (Miscellaneous)
        public float Angle(Vector4D v) => (float)Math.Acos((this * v) / (this.Magnitude() * v.Magnitude()));

        public Vector4D Cross_Product(Vector4D v) => new Vector4D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X, this.W);

        public float Magnitude() => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2) + Math.Pow(W, 2));

        public Vector4D Normalise() => this / Magnitude();
        #endregion
    }
}