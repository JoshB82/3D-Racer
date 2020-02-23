using System;

namespace _3D_Racer
{
    public partial class Matrix
    {
        /// <summary>
        /// Element-wise addition of matrices
        /// </summary>
        /// <param name="m1">First addend</param>
        /// <param name="m2">Second addend</param>
        /// <returns></returns>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if ((m1.Rows != m2.Rows) || (m1.Cols != m2.Cols))
                throw new Exception("The matrix dimensions do not match.");

            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    m1.Data[i, j] += m2.Data[i, j];
            return m1;
        }

        /// <summary>
        /// Element-wise subtraction of matrices
        /// </summary>
        /// <param name="m1">Minuend</param>
        /// <param name="m2">Subtrahend</param>
        /// <returns></returns>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if ((m1.Rows != m2.Rows) || (m1.Cols != m2.Cols))
                throw new Exception("The matrix dimensions do not match.");

            for (int i = 0; i < m1.Rows; i++)
                for (int j = 0; j < m1.Cols; j++)
                    m1.Data[i, j] -= m2.Data[i, j];
            return m1;
        }

        /// <summary>
        /// Multiplication of two matrices.
        /// </summary>
        /// <param name="m1">Multiplicand</param>
        /// <param name="m2">Multiplier</param>
        /// <returns>Product of two matrices.</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.Data.GetLength(1) != m2.Data.GetLength(0))
                throw new Exception("The number of columns in the first matrix must be equal to the number of rows in the second matrix.");

            int num_rows = m1.Data.GetLength(0);
            int num_cols = m2.Data.GetLength(1);
            double[,] product = new double[num_rows, num_cols];

            double[] row = new double[m1.Data.GetLength(1)];
            double[] col = new double[m2.Data.GetLength(0)];

            for (int i = 0; i < num_rows; i++)
                for (int j = 0; j < num_cols; j++)
                {
                    for (int k = 0; k < row.Length; k++) row[k] = m1.Data[i, k];
                    for (int l = 0; l < col.Length; l++) col[l] = m2.Data[l, j];
                    product[i, j] = new Vector(row) * new Vector(col);
                }

            return new Matrix(product);
        }

        /// <summary>
        /// Multiplication of a scalar and a matrix.
        /// </summary>
        /// <param name="s">Scalar</param>
        /// <param name="m">Matrix</param>
        /// <returns>Product of a scalar and matrix.</returns>
        public static Matrix operator *(double s, Matrix m)
        {
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    m.Data[i, j] *= s;
            return m;
        }
    }
}