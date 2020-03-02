using System;

namespace _3D_Racer
{
    public partial class Vector
    {
        /// <summary>
        /// Element-wise addition of vectors
        /// </summary>
        /// <param name="v1">First addend</param>
        /// <param name="v2">Second addend</param>
        /// <returns>Sum of two vectors.</returns>
        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size) throw new Exception("The vector dimensions do not match.");
            for (int i = 0; i < v1.Size; i++) v1.Data[i] += v2.Data[i];
            return v1;
        }

        /// <summary>
        /// Element-wise subtraction of vectors
        /// </summary>
        /// <param name="v1">Minuend</param>
        /// <param name="v2">Subtrahend</param>
        /// <returns>Difference of two vectors.</returns>
        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size) throw new Exception("The vector dimensions do not match.");
            for (int i = 0; i < v1.Size; i++) v1.Data[i] -= v2.Data[i];
            return v1;
        }

        /// <summary>
        /// Dot product of two vectors.
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>Dot product of two vectors.</returns>
        public static float operator *(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size) throw new Exception("The number of elements in each vector must be the same.");

            float total = 0;
            for (int i = 0; i < v1.Size; i++) total += v1.Data[i] * v2.Data[i];
            return total;
        }

        /// <summary>
        /// Product of a matrix and a vector
        /// </summary>
        /// <param name="m">Matrix</param>
        /// <param name="v">Vector</param>
        /// <returns>Product of a matrix and a vector</returns>
        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.Cols != v.Size) throw new Exception("The number of columns in the matrix must equal the number of elements in the vector.");

            float[] result = new float[v.Size];
            float[] row = new float[m.Cols];

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Cols; j++) row[j] = m.Data[i, j];
                result[i] = new Vector(row) * v;
            }

            return new Vector(result);
        }
    }
}