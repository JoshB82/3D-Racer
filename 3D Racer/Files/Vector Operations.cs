using System;

namespace _3D_Racer
{
    public partial class Vector
    {
        public float Magnitude()
        {
            float total = 0;
            for (int i = 0; i < Size; i++) total += (float)Math.Pow(Data[i], 2);
            return (float)Math.Sqrt(total);
        }

        public static float Angle_Between_Vectors(Vector v1, Vector v2) => (float)Math.Acos((v1 * v2) / (v1.Magnitude() * v2.Magnitude()));

        public void Normalise()
        {
            float magnitude = Magnitude();
            for (int i = 0; i < Size; i++) Data[i] /= magnitude;
        }

        public static Vector Cross_Product(Vector v1, Vector v2)
        {
            return new Vector(v1.Data[1] * v2.Data[2] - v1.Data[2] * v2.Data[1],
                              v1.Data[2] * v2.Data[0] - v1.Data[0] * v2.Data[2],
                              v1.Data[0] * v2.Data[1] - v1.Data[1] * v2.Data[0]);
        }
    }
}
