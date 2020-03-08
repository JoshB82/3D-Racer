using System;

namespace _3D_Racer
{
    public class Quaternion
    {
        public float Q1 { get; set; }
        public float Q2 { get; set; }
        public float Q3 { get; set; }
        public float Q4 { get; set; }

        public Quaternion(float q1, float q2, float q3, float q4)
        {
            Q1 = q1;
            Q2 = q2;
            Q3 = q3;
            Q4 = q4;
        }

        public Quaternion(float[] data)
        {
            Q1 = data[0];
            Q2 = data[1];
            Q3 = data[2];
            Q4 = data[3];
        }

        public Quaternion(Vector4D v)
        {
            Q1 = v.X;
            Q2 = v.Y;
            Q3 = v.Z;
            Q4 = v.W;
        }

        public Quaternion(float scalar, Vector3D v)
        {
            Q1 = scalar;
            Q2 = v.X;
            Q3 = v.Y;
            Q4 = v.Z;
        }

        public static Quaternion operator +(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 + q2.Q1, q1.Q2 + q2.Q2, q1.Q3 + q2.Q3, q1.Q4 + q2.Q4);
        public static Quaternion operator -(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 - q2.Q1, q1.Q2 - q2.Q2, q1.Q3 - q2.Q3, q1.Q4 - q2.Q4);
        public static Quaternion operator *(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 * q2.Q1 - q1.Q2 * q2.Q2 - q1.Q3 * q2.Q3 - q1.Q4 * q2.Q4, q1.Q1 * q2.Q2 - q1.Q2 * q2.Q1 - q1.Q3 * q2.Q4 - q1.Q4 * q2.Q3, q1.Q1 * q2.Q3 - q1.Q2 * q2.Q4 - q1.Q3 * q2.Q1 - q1.Q4 * q2.Q2, q1.Q1 * q2.Q4 - q1.Q2 * q2.Q3 - q1.Q3 * q2.Q2 - q1.Q4 * q2.Q1);
        public static Quaternion operator /(Quaternion q, float scalar) => new Quaternion(q.Q1 / scalar, q.Q2 / scalar, q.Q3 / scalar, q.Q4 / scalar);

        public float Magnitude() => (float)Math.Sqrt(Math.Pow(Q1, 2) + Math.Pow(Q2, 2) + Math.Pow(Q3, 2) + Math.Pow(Q4, 2));

        public Quaternion Normalise() => this / Magnitude();
    }
}
