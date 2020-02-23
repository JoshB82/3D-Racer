/// <summary>
/// Provides Matrix data types and functionality.
/// </summary>

// Constructors for Matrix class

namespace _3D_Racer2
{
    public sealed partial class Matrix
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public double[,] Data { get; set; }

        /// <summary>
        /// Creates a zeroed matrix of specified size.
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Data = new double[rows, cols];
        }

        /// <summary>
        /// Creates an identity matrix of specified size.
        /// </summary>
        /// <param name="rowsAndCols">Number of rows/columns</param>
        public Matrix(int rows_and_cols)
        {
            Rows = Cols = rows_and_cols;
            Data = new double[rows_and_cols, rows_and_cols];
            for (int i = 0; i < Rows; i++) Data[i, i] = 1;
        }

        /// <summary>
        /// Creates a matrix from a data array.
        /// </summary>
        /// <param name="data">Array containing matrix data</param>
        public Matrix(double[,] data)
        {
            Rows = data.GetLength(0);
            Cols = data.GetLength(1);
            Data = data;
        }

        /// <summary>
        /// Creates a 2x2 matrix from four values.
        /// </summary>
        /// <param name="i1">Top left value</param>
        /// <param name="i2">Top right value</param>
        /// <param name="i3">Bottom left value</param>
        /// <param name="i4">Bottom right value</param>
        public Matrix(double i1, double i2, double i3, double i4)
        {
            Rows = Cols = 2;
            Data = new double[2, 2] { { i1, i2 }, { i3, i4 } };
        }

        /// <summary>
        /// Creates a 3x3 matrix from nine values.
        /// </summary>
        /// <param name="i1">Top left value</param>
        /// <param name="i2">Top centre value</param>
        /// <param name="i3">Top right value</param>
        /// <param name="i4">Middle left value</param>
        /// <param name="i5">Middle centre value</param>
        /// <param name="i6">Middle right value</param>
        /// <param name="i7">Bottom left value</param>
        /// <param name="i8">Bottom centre value</param>
        /// <param name="i9">Bottom right value</param>
        public Matrix(double i1, double i2, double i3, double i4, double i5, double i6, double i7, double i8, double i9)
        {
            Rows = Cols = 3;
            Data = new double[3, 3] { { i1, i2, i3 }, { i4, i5, i6 }, { i7, i8, i9 } };
        }

        /// <summary>
        /// Returns matrix as an array.
        /// </summary>
        /// <returns></returns>
        public double[,] MatrixToArray()
        {
            return Data;
        }

        /// <summary>
        /// Changes a single value of an entry in a matrix.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="row">Row number (from 1 to number of rows)</param>
        /// <param name="col">Column number (from 1 to number of columns)</param>
        /// <param name="newValue">The new value to be implemented into the matrix</param>
        public void ChangeSingleValue(int row, int col, double new_value)
        {
            Data[row - 1, col - 1] = new_value;
        }

        /// <summary>
        /// Returns a single value from a matrix.
        /// </summary>
        /// <param name="row">Row number (from 1 to number of rows)</param>
        /// <param name="col">Column number (from 1 to number of columns)</param>
        /// <returns></returns>
        public double GetSingleValue(int row, int col)
        {
            return Data[row - 1, col - 1];
        }
    }
}