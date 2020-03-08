using System;

namespace _3D_Racer
{
    /// <summary>
    /// Handles constructors and operations involving three-dimensional vectors
    /// </summary>
    public class Vector3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(Vector2D v, float z)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        public Vector3D(Vector4D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        #region Common Vectors
        public static readonly Vector3D Zero = new Vector3D(0, 0, 0);
        public static readonly Vector3D Unit_X = new Vector3D(1, 0, 0);
        public static readonly Vector3D Unit_Y = new Vector3D(0, 1, 0);
        public static readonly Vector3D Unit_Z = new Vector3D(0, 0, 1);
        public static readonly Vector3D Unit_Negative_X = new Vector3D(-1, 0, 0);
        public static readonly Vector3D Unit_Negative_Y = new Vector3D(0, -1, 0);
        public static readonly Vector3D Unit_Negative_Z = new Vector3D(0, 0, -1);
        #endregion

        #region Vector Operations (Operator Overloading)
        public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static float operator *(Vector3D v1, Vector3D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        public static Vector3D operator *(Vector3D v, float scalar) => new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static Vector3D operator /(Vector3D v, float scalar) => new Vector3D(v.X / scalar, v.Y / scalar, v.Z / scalar);
        public static Vector3D operator -(Vector3D v) => new Vector3D(-v.X, -v.Y, -v.Z);
        #endregion

        #region Vector Operations (Miscellaneous)
        public float Angle(Vector3D v) => (float)Math.Acos((this * v) / (this.Magnitude() * v.Magnitude()));

        public Vector3D Cross_Product(Vector3D v) => new Vector3D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);

        public float Magnitude() => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        public Vector3D Normalise() => this / Magnitude();
        #endregion
    }
}