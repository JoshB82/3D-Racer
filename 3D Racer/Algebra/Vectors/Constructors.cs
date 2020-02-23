using System;

namespace _3D_Racer2
{
    public partial class Vector
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public float[] Data { get; set; }
        public string Type { get; set; }

        public Vector(float[] data, string type)
        {
            Data = data;
            Type = type;

            switch (type)
            {
                case "row":
                    Rows = 1;
                    Cols = data.Length;
                    break;
                case "column":
                    Rows = data.Length;
                    Cols = 1;
                    break;
                default:
                    throw new Exception("Invalid vector type.");
            }
        }

        public Vector(float x, float y, float z)
        {
            Data = new float[3] { x, y, z };
            Type = "row";
            Rows = 1;
            Cols = 3;
        }

        public static Vector operator+ (Vector v1, Vector v2)
        {
            for (int i = 0; i < v1.Data.Length; i++) v1.Data[i] += v2.Data[i];
            return v1;
        }

        public static Vector operator- (Vector v1, Vector v2)
        {
            for (int i = 0; i < v1.Data.Length; i++) v1.Data[i] -= v2.Data[i];
            return v1;
        }
    }
}