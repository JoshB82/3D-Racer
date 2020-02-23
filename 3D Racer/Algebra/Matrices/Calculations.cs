using System;

// Calculations involving matrices

namespace _3D_Racer2
{
    public sealed partial class Matrix
    {
        /// <summary>
        /// Finds the determinant of a square matrix.
        /// </summary>
        /// <returns>Determinant of matrix.</returns>
        public float CalculateDeterminant()
        {
            // Check for square matrix
            if (this.rows != this.cols) throw new Exception("The number of rows and columns do not match.");

            switch (this.rows)
            {
                case 2:
                    // 2x2
                    return this.data[0, 0] * this.data[1, 1] - this.data[0, 1] * this.data[1, 0];
                default:
                    // nxn  
                    float total = 0;
                    for (int i = 0; i < this.cols; i++)
                    {
                        float[,] subArray_1 = new float[this.rows - 1, i];
                        float[,] subArray_2 = new float[this.rows - 1, this.cols - i - 1];
                        for (int i1 = 0; i1 < this.rows - 1; i1++)
                        {
                            for (int j1 = 0; j1 < i; j1++) subArray_1[i1, j1] = this.data[i1 + 1, j1];
                            for (int j2 = i + 1; j2 < this.cols; j2++) subArray_2[i1, j2 - i - 1] = this.data[i1 + 1, j2];
                        }
                        Matrix subMatrix_1 = new Matrix(subArray_1);
                        Matrix subMatrix_2 = new Matrix(subArray_2);
                        subMatrix_1.ConcatMatrixAtRight(subMatrix_2);
                        float subDeterminant = subMatrix_1.CalculateDeterminant();
                        int polarity = i % 2 == 0 ? 1 : -1;
                        total += polarity * this.data[0, i] * subDeterminant;
                    }
                    return total;
            }
        }

        /// <summary>
        /// Adds two matrices together.
        /// </summary>
        /// <param name="toAdd">The matrix to add</param>
        /// <returns>An added matrix</returns>
        public Matrix AddMatrix(Matrix toAdd)
        {
            if ((this.rows != toAdd.rows) || (this.cols != toAdd.cols))
                throw new Exception("The matrix dimensions do not match.");

            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] += toAdd.data[i,j];
            return this;
        }

        /// <summary>
        /// Subtracts one matrix from another.
        /// </summary>
        /// <param name="toSubtract">The matrix to subtract</param>
        /// <returns>A subtracted matrix</returns>
        public Matrix SubtractMatrix(Matrix toSubtract)
        {
            if ((this.rows != toSubtract.rows) || (this.cols != toSubtract.cols))
                throw new Exception("The matrix dimensions do not match.");

            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] -= toSubtract.data[i, j];
            return this;
        }

        /// <summary>
        /// Finds the product of a scalar and a matrix.
        /// </summary>
        /// <param name="scalar">Scalar used in product.</param>
        public void MultiplyScalar(float scalar)
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] *= scalar;
        }

        /// <summary>
        /// Finds the product of two matrices.
        /// </summary>
        /// <param name="toMultiply">The right matrix in the multiplication.</param>
        /// <returns>The product of two matrices.</returns>
        public Matrix MultiplyMatrix(Matrix toMultiply)
        {
            if (this.data.GetLength(1) != toMultiply.data.GetLength(0))
                throw new Exception("The number of columns in the first matrix must be equal to the number of rows in the second matrix.");

            int numRows = this.data.GetLength(0);
            int numCols = toMultiply.data.GetLength(1);
            float[,] product = new float[numRows, numCols];

            float[] row = new float[this.data.GetLength(1)];
            float[] col = new float[toMultiply.data.GetLength(0)];

            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < numCols; j++)
                {
                    for (int k = 0; k < row.Length; k++) row[k] = this.data[i,k];
                    for (int l = 0; l < col.Length; l++) col[l] = toMultiply.data[l,j];
                    Vector rowVector = new Vector(row,"row");
                    Vector colVector = new Vector(col,"column");
                    float dotProduct = rowVector.DotProduct(colVector);
                    product[i, j] = dotProduct;
                }

            return new Matrix(product);
        }

        /// <summary>
        /// Finds the inverse of a matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            if (this.rows != this.cols) throw new Exception("The number of rows and columns must be the same.");

            // To do later

            return null;
        }
    }
}