using System;

namespace _3D_Racer
{
    /// <summary>
    /// Handles constructors and operations involving two-dimensional vectors
    /// </summary>
    public class Vector2D
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector3D v)
        {
            X = v.X;
            Y = v.Y;
        }

        #region Common Vectors
        public static readonly Vector2D Zero = new Vector2D(0, 0);
        public static readonly Vector2D Unit_X = new Vector2D(1, 0);
        public static readonly Vector2D Unit_Y = new Vector2D(0, 1);
        public static readonly Vector2D Unit_Negative_X = new Vector2D(-1, 0);
        public static readonly Vector2D Unit_Negative_Y = new Vector2D(0, -1);
        #endregion

        #region Vector Operations (Operator Overloading)
        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        public static float operator *(Vector2D v1, Vector2D v2) => v1.X * v2.X + v1.Y * v2.Y;
        public static Vector2D operator *(Vector2D v, float scalar) => new Vector2D(v.X * scalar, v.Y * scalar);
        public static Vector2D operator /(Vector2D v, float scalar) => new Vector2D(v.X / scalar, v.Y / scalar);
        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.X, -v.Y);
        #endregion

        #region Vector Operations (Miscellaneous)
        public float Angle(Vector2D v) => (float)Math.Acos((this * v) / (this.Magnitude() * v.Magnitude()));

        public float Magnitude() => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

        public Vector2D Normalise() => this / Magnitude();
        #endregion
    }
}