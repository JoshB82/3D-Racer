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

        #region Vector Operations (Operator Overloading)
        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        public static float operator *(Vector2D v1, Vector2D v2) => v1.X * v2.X + v1.Y + v2.Y;
        public static Vector2D operator *(Vector2D v, float scalar) => new Vector2D(v.X * scalar, v.Y * scalar);
        public static Vector2D operator /(Vector2D v, float scalar) => new Vector2D(v.X / scalar, v.Y / scalar);
        #endregion

        #region Vector Operations (Miscellaneous)
        public float Angle(Vector2D v) => (float)Math.Acos((this * v) / (this.Magnitude() * v.Magnitude()));

        public float Magnitude() => (float)Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

        public Vector2D Normalise() => this / Magnitude();
        #endregion
    }
}