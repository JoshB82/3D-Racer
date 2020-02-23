using System;

namespace _3D_Racer
{
    public partial class Vector
    {
        /// <summary>
        /// Element-wise addition of vectors
        /// </summary>
        /// <param name="m1">First addend</param>
        /// <param name="m2">Second addend</param>
        /// <returns>Sum of two vectors.</returns>
        public static Vector operator +(Vector m1, Vector m2)
        {
            if (m1.Size != m2.Size) throw new Exception("The matrix dimensions do not match.");
            for (int i = 0; i < m1.Size; i++) m1.Data[i] += m2.Data[i];
            return m1;
        }

        /// <summary>
        /// Element-wise subtraction of vectors
        /// </summary>
        /// <param name="m1">Minuend</param>
        /// <param name="m2">Subtrahend</param>
        /// <returns>Difference of two vectors.</returns>
        public static Vector operator -(Vector m1, Vector m2)
        {
            if (m1.Size != m2.Size) throw new Exception("The matrix dimensions do not match.");
            for (int i = 0; i < m1.Size; i++) m1.Data[i] -= m2.Data[i];
            return m1;
        }

        /// <summary>
        /// Dot product of two vectors.
        /// </summary>
        /// <param name="m1">First vector</param>
        /// <param name="m2">Second vector</param>
        /// <returns>Dot product of two vectors.</returns>
        public static double operator *(Vector m1, Vector m2)
        {
            if (m1.Size != m2.Size) throw new Exception("The number of elements in each vector must be the same.");

            double total = 0;

            for (int i = 0; i < m1.Size; i++)
                total += m1.Data[i] * m2.Data[i];

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
            double[] result = new double[v.Size];

            double[] row = new double[m.Cols];

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Cols; j++) row[j] = m.Data[i, j];
                result[i] = new Vector(row) * v;
            }

            return new Vector(result);
        }
    }
}