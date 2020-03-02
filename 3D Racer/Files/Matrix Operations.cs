using System;

namespace _3D_Racer
{
    public partial class Matrix
    {
        /// <summary>
        /// Finds the determinant of a square matrix.
        /// </summary>
        /// <returns>Determinant of matrix.</returns>
        public float CalculateDeterminant()
        {
            // Check for square matrix
            if (Rows != Cols) throw new Exception("The number of rows and columns do not match.");

            switch (Rows)
            {
                case 2:
                    // 2x2
                    return Data[0, 0] * Data[1, 1] - Data[0, 1] * Data[1, 0];
                default:
                    // nxn  
                    float total = 0;
                    for (int i = 0; i < Cols; i++)
                    {
                        float[,] subArray_1 = new float[Rows - 1, i];
                        float[,] subArray_2 = new float[Rows - 1, Cols - i - 1];
                        for (int i1 = 0; i1 < Rows - 1; i1++)
                        {
                            for (int j1 = 0; j1 < i; j1++) subArray_1[i1, j1] = Data[i1 + 1, j1];
                            for (int j2 = i + 1; j2 < Cols; j2++) subArray_2[i1, j2 - i - 1] = Data[i1 + 1, j2];
                        }
                        Matrix subMatrix_1 = new Matrix(subArray_1);
                        Matrix subMatrix_2 = new Matrix(subArray_2);
                        subMatrix_1.ConcatMatrixAtRight(subMatrix_2);
                        float subDeterminant = subMatrix_1.CalculateDeterminant();
                        int polarity = i % 2 == 0 ? 1 : -1;
                        total += polarity * Data[0, i] * subDeterminant;
                    }
                    return total;
            }
        }
    }
}