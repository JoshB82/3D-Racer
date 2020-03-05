namespace _3D_Racer
{
    public partial class ShapeComponents : Vector
    {
        public static ShapeComponents operator +(ShapeComponents v1, ShapeComponents v2)
        {
            v1.X += v2.X;
            v1.Y += v2.Y;
            v1.Z += v2.Z;
            return v1;
        }

        public static ShapeComponents operator -(ShapeComponents v1, ShapeComponents v2)
        {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            v1.Z -= v2.Z;
            return v1;
        }

        public static ShapeComponents operator +(ShapeComponents v1, Vector v2)
        {
            v1.X += v2.Data[0];
            v1.Y += v2.Data[1];
            v1.Z += v2.Data[2];
            return v1;
        }

        public static ShapeComponents operator -(ShapeComponents v1, Vector v2)
        {
            v1.X -= v2.Data[0];
            v1.Y -= v2.Data[1];
            v1.Z -= v2.Data[2];
            return v1;
        }

        public static ShapeComponents operator *(Matrix m, ShapeComponents v)
        {
            float[] result = new float[m.Rows];
            float[] row = new float[m.Cols];

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Cols; j++) row[j] = m.Data[i, j];
                result[i] = new Vector(row) * v;
            }

            return new ShapeComponents(result[0], result[1], result[2]);
        }
    }
}