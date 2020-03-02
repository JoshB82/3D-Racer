using System;
using System.Diagnostics;

namespace _3D_Racer
{
    public partial class Matrix
    {
        /// <summary>
        /// Concatenates one matrix to another at the bottom of the first matrix.
        /// The number of columns in each matrix must be the same.
        /// </summary>
        /// <param name="to_join">Matrix to be joined.</param>
        public void ConcatMatrixAtBottom(Matrix to_join)
        {
            float[,] data_top = Data;
            float[,] data_bottom = to_join.MatrixToArray();

            int num_rows = data_top.GetLength(0) + data_bottom.GetLength(0);
            int num_cols = data_top.GetLength(1);

            if (num_cols != data_bottom.GetLength(1)) throw new Exception("The number of columns do not match.");

            float[,] new_array = new float[num_rows, num_cols];
            int i1;
            for (int j = 0; j < num_cols; j++)
            {
                for (i1 = 0; i1 < data_top.GetLength(1); i1++)
                    new_array[i1, j] = data_top[i1, j];
                for (int i2 = 0; i2 < data_bottom.GetLength(1); i2++)
                    new_array[i1 + i2, j] = data_bottom[i2, j];
            }
            Data = new_array;
            Rows = num_rows;
        }

        /// <summary>
        /// Concatenates one matrix to another at the right of the first vector/matrix.
        /// The number of rows in each matrix must be the same.
        /// </summary>
        /// <param name="to_join">Matrix to be joined.</param>
        public void ConcatMatrixAtRight(Matrix to_join)
        {
            float[,] data_left = Data;
            float[,] data_right = to_join.MatrixToArray();

            int num_rows = data_left.GetLength(0);
            int num_cols = data_left.GetLength(1) + data_right.GetLength(1);

            if (num_rows != data_right.GetLength(0)) throw new Exception("The number of rows do not match.");

            float[,] new_array = new float[num_rows, num_cols];
            int j1;
            for (int i = 0; i < num_rows; i++)
            {
                for (j1 = 0; j1 < data_left.GetLength(1); j1++)
                    new_array[i, j1] = data_left[i, j1];
                for (int j2 = 0; j2 < data_right.GetLength(1); j2++)
                    new_array[i, j1 + j2] = data_right[i, j2];
            }
            Data = new_array;
            Cols = num_cols;
        }

        /// <summary>
        /// Splits a matrix into two seperate matrices, with the split boundary being vertical.
        /// </summary>
        /// <param name="to_split">The matrix to be split.</param>
        /// <param name="num_cols_left">The number of columns the 'left' matrix should have.</param>
        /// <param name="split_matrix_left">The 'left' matrix after the split.</param>
        /// <param name="split_matrix_right">The 'right' matrix after the split.</param>
        public void SplitMatrixAtColumn(Matrix to_split, int num_cols_left, out Matrix split_matrix_left, out Matrix split_matrix_right)
        {
            float[,] data = to_split.MatrixToArray();
            int num_rows = data.GetLength(0);
            int num_cols = data.GetLength(1);
            int num_cols_right = num_cols - num_cols_left;
            float[,] data_left = new float[num_rows, num_cols_left];
            float[,] data_right = new float[num_rows, num_cols_right];
            int j1;
            for (int i = 0; i < num_rows; i++)
            {
                for (j1 = 0; j1 < num_cols_left; j1++)
                    data_left[i, j1] = data[i, j1];
                for (int j2 = 0; j2 < num_cols_right; j2++)
                    data_right[i, j2] = data[i, j1 + j2];
            }
            split_matrix_left = new Matrix(data_left);
            split_matrix_right = new Matrix(data_right);
        }

        /// <summary>
        /// Splits a matrix into two seperate matrices, with the split boundary being horizontal.
        /// </summary>
        /// <param name="to_split">The matrix to be split.</param>
        /// <param name="num_rows_top">The number of rows the 'top' matrix should have.</param>
        /// <param name="split_matrix_top">The 'top' matrix after the split.</param>
        /// <param name="split_matrix_bottom">The 'bottom' matrix after the split.</param>
        public void SplitMatrixAtRow(Matrix to_split, int num_rows_top, out Matrix split_matrix_top, out Matrix split_matrix_bottom)
        {
            float[,] data = to_split.MatrixToArray();
            int num_rows = data.GetLength(0);
            int num_cols = data.GetLength(1);
            int num_rows_bottom = num_rows - num_rows_top;
            float[,] data_top = new float[num_rows_top, num_cols];
            float[,] data_bottom = new float[num_rows_bottom, num_cols];
            int i1;
            for (int j = 0; j < num_rows; j++)
            {
                for (i1 = 0; i1 < num_rows_top; i1++)
                    data_top[i1, j] = data[i1, j];
                for (int i2 = 0; i2 < num_rows_bottom; i2++)
                    data_bottom[i2, j] = data[i1 + i2, j];
            }
            split_matrix_top = new Matrix(data_top);
            split_matrix_bottom = new Matrix(data_bottom);
        }

        /// <summary>
        /// Adds a specified number of zeroed row and column vectors to a vector/matrix.
        /// If the number of row and/or columns is negative, that many row and/or column vectors are deleted instead.
        /// </summary>
        /// <param name="num_rows">The number of rows to be added.</param>
        /// <param name="num_cols">The number of columns to be added.</param>
        public void Resize(int num_rows, int num_cols)
        {
            AddRows(num_rows);
            AddColumns(num_cols);
        }

        /// <summary>
        /// Adds a specified number of zeroed column vectors to a vector/matrix.
        /// If the number of columns is negative, that many column vectors are deleted instead.
        /// </summary>
        /// <param name="num_cols">The number of columns to be added.</param>
        public void AddColumns(int num_cols)
        {
            float[,] old_array = MatrixToArray();
            int rows = old_array.GetLength(0);
            int cols = old_array.GetLength(1);
            float[,] new_array = null;

            switch (Math.Sign(num_cols))
            {
                case -1:
                    // Columns are being deleted
                    new_array = new float[rows, cols + num_cols];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols + num_cols; j++)
                            new_array[i, j] = old_array[i, j];
                    break;
                case 0:
                    // No change required
                    return;
                case 1:
                    // Columns are being added
                    new_array = new float[rows, cols + num_cols];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            new_array[i, j] = old_array[i, j];
                    break;
            }
            Data = new_array;
            Cols = cols + num_cols;
        }

        /// <summary>
        /// Adds a specified number of zeroed row vectors to a vector/matrix.
        /// If the number of rows is negative, that many column vectors are deleted instead.
        /// </summary>
        /// <param name="num_rows">The number of rows to be added.</param>
        public void AddRows(int num_rows)
        {
            float[,] old_array = MatrixToArray();
            int rows = old_array.GetLength(0);
            int cols = old_array.GetLength(1);
            float[,] new_array = null;

            switch (Math.Sign(num_rows))
            {
                case -1:
                    // Rows are being deleted
                    new_array = new float[rows + num_rows, cols];
                    for (int i = 0; i < rows + num_rows; i++)
                        for (int j = 0; j < cols; j++)
                            new_array[i, j] = old_array[i, j];
                    break;
                case 0:
                    // No change required
                    return;
                case 1:
                    // Rows are being added
                    new_array = new float[rows + num_rows, cols];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            new_array[i, j] = old_array[i, j];
                    break;
            }
            Data = new_array;
            Rows = rows + num_rows;
        }

        /// <summary>
        /// Outputs a matrix to the debug console as a formatted string.
        /// </summary>
        public void OutputMatrixDebug()
        {
            float[,] data = MatrixToArray();
            int last_pos = data.GetLength(0) * data.GetLength(1);

            string output_text = "( ";
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    output_text += data[i, j].ToString() + " ";
                    int pos = (i * data.GetLength(1) + j + 1);
                    if (pos % data.GetLength(1) == 0 && pos != data.GetLength(0) * data.GetLength(1))
                        output_text += "\n";
                }
            output_text += ")";
            Debug.WriteLine(output_text);
        }

        /*
        public Vector ConvertToVector(string type)
        {
            if (!((Rows > 1 && this.cols == 1) || (this.rows == 1 && this.cols > 1)))
                throw new Exception("Matrix contains too many rows or columns.");

            float[] newVector;

            switch (type)
            {
                case "row":
                    newVector = new float[this.data.GetLength(1)];
                    for (int i = 0; i < this.data.GetLength(1); i++) newVector[i] = this.data[0, i];
                    break;
                case "column":
                    newVector = new float[this.data.GetLength(0)];
                    for (int i = 0; i < this.data.GetLength(0); i++) newVector[i] = this.data[i, 0];
                    break;
                default:
                    throw new Exception("Vector type is invalid.");
            }
            return new Vector(newVector, type);
        }
        */
    }
}