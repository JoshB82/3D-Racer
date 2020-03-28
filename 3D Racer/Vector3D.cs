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

        public static Vector3D Line_Intersect_Plane(Vector3D line_start, Vector3D line_finish, Vector3D plane_point, Vector3D plane_normal)
        {
            Vector3D line = (line_finish - line_start).Normalise();
            float d = ((plane_point - line_start) * plane_normal) / (line * plane_normal);
            Vector3D to_return = line * d + line_start;
            // Round in direction of normal!?
            // Y-AXES WRONG (upside down)?
            return new Vector3D((int)Math.Round(to_return.X), (int)Math.Round(to_return.Y), to_return.Z);
        }

        public static Vector3D Normal_To_Plane(Vector3D p1, Vector3D p2, Vector3D p3) => (p2 - p1).Cross_Product(p3 - p1).Normalise();

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