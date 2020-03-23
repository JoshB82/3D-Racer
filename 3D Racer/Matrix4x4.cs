using System;

namespace _3D_Racer
{
    public class Matrix4x4
    {
        public float[][] Data { get; set; }

        /// <summary>
        /// Creates a zeroed 4x4 matrix.
        /// </summary>
        public Matrix4x4()
        {
            Data = new float[4][];
            for (int i = 0; i < 4; i++) Data[i] = new float[4];
        }

        /// <summary>
        /// Creates an 4x4 identity matrix.
        /// </summary>
        public static Matrix4x4 IdentityMatrix()
        {
            float[][] data = new float[4][];
            for (int i = 0; i < 4; i++)
            {
                data[i] = new float[4];
                data[i][i] = 1;
            }
            return new Matrix4x4(data);
        }

        public Matrix4x4(float i1, float i2, float i3, float i4, float i5, float i6, float i7, float i8, float i9, float i10, float i11, float i12, float i13, float i14, float i15, float i16)
        {
            Data = new float[4][];
            Data[0] = new float[4] { i1, i2, i3, i4 };
            Data[1] = new float[4] { i5, i6, i7, i8 };
            Data[2] = new float[4] { i9, i10, i11, i12 };
            Data[3] = new float[4] { i13, i14, i15, i16 };
        }

        public Matrix4x4(float[][] data)
        {
            if (data.Length != 4 || data[0].Length != 4 || data[1].Length != 4 || data[2].Length != 4 || data[3].Length != 4) throw new Exception("Array must be of size 4x4.");
            Data = data;
        }

        #region Operator overloading
        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2)
        {
            float[][] result = new float[4][];
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) result[i][j] = m1.Data[i][j] + m2.Data[i][j];
            return new Matrix4x4(result);
        }

        public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2)
        {
            float[][] result = new float[4][];
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) result[i][j] = m1.Data[i][j] - m2.Data[i][j];
            return new Matrix4x4(result);
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            float[][] result = new float[4][];
            float[] row = new float[4];
            float[] col = new float[4];

            for (int i = 0; i < 4; i++)
            {
                result[i] = new float[4];
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++) row[k] = m1.Data[i][k];
                    for (int l = 0; l < 4; l++) col[l] = m2.Data[l][j];
                    result[i][j] = new Vector4D(row) * new Vector4D(col);
                }
            }
            return new Matrix4x4(result);
        }

        public static Vector4D operator *(Matrix4x4 m, Vector4D v)
        {
            float[] result = new float[4];
            float[] row = new float[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) row[j] = m.Data[i][j];
                result[i] = new Vector4D(row) * v;
            }
            return new Vector4D(result);
        }

        public static Vertex operator *(Matrix4x4 m, Vertex v)
        {
            float[] result = new float[4];
            float[] row = new float[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) row[j] = m.Data[i][j];
                result[i] = new Vector4D(row) * v;
            }
            return new Vertex(result[0], result[1], result[2], result[3], v.Colour, v.Visible, v.Diameter);
        }

        public static Matrix4x4 operator *(float scalar, Matrix4x4 m)
        {
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) m.Data[i][j] *= scalar;
            return m;
        }

        public static Matrix4x4 operator /(float scalar, Matrix4x4 m)
        {
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) m.Data[i][j] /= scalar;
            return m;
        }
        #endregion
    }
}