using System;
using System.Diagnostics;

// Methods for changing structures of matrices: concatenation, split, output

namespace _3D_Racer2
{
    public sealed partial class Matrix
    {
        /// <summary>
        /// Concatenates one vector/matrix to another at the bottom of the first vector/matrix.
        /// The number of columns in each vector/matrix must be the same.
        /// </summary>
        /// <param name="toJoin">Matrix to be joined.</param>
        public void ConcatMatrixAtBottom(Matrix toJoin)
        {
            float[,] dataTop = this.data;
            float[,] dataBottom = toJoin.MatrixToArray();

            int numRows = dataTop.GetLength(0) + dataBottom.GetLength(0);
            int numCols = dataTop.GetLength(1);

            if (numCols != dataBottom.GetLength(1)) throw new Exception("The number of columns do not match.");

            float[,] newArray = new float[numRows, numCols];
            int i1;
            for (int j = 0; j < numCols; j++)
            {
                for (i1 = 0; i1 < dataTop.GetLength(1); i1++)
                    newArray[i1, j] = dataTop[i1, j];
                for (int i2 = 0; i2 < dataBottom.GetLength(1); i2++)
                    newArray[i1 + i2, j] = dataBottom[i2, j];
            }
            this.data = newArray;
            this.rows = numRows;
        }

        /// <summary>
        /// Concatenates one vector/matrix to another at the right of the first vector/matrix.
        /// The number of rows in each vector/matrix must be the same.
        /// </summary>
        /// <param name="toJoin">Matrix to be joined.</param>
        public void ConcatMatrixAtRight(Matrix toJoin)
        {
            float[,] dataLeft = this.data;
            float[,] dataRight = toJoin.MatrixToArray();

            int numRows = dataLeft.GetLength(0);
            int numCols = dataLeft.GetLength(1) + dataRight.GetLength(1);

            if (numRows != dataRight.GetLength(0)) throw new Exception("The number of rows do not match.");

            float[,] newArray = new float[numRows, numCols];
            int j1;
            for (int i = 0; i < numRows; i++)
            {
                for (j1 = 0; j1 < dataLeft.GetLength(1); j1++)
                    newArray[i, j1] = dataLeft[i, j1];
                for (int j2 = 0; j2 < dataRight.GetLength(1); j2++)
                    newArray[i, j1 + j2] = dataRight[i, j2];
            }
            this.data = newArray;
            this.cols = numCols;
        }

        /// <summary>
        /// Splits a matrix into two seperate matrices, with the split boundary being vertical.
        /// </summary>
        /// <param name="toSplit">The matrix to be split.</param>
        /// <param name="numColsLeft">The number of columns the 'left' matrix should have.</param>
        /// <param name="splitMatrixLeft">The 'left' matrix after the split.</param>
        /// <param name="splitMatrixRight">The 'right' matrix after the split.</param>
        public void SplitMatrixAtColumn(Matrix toSplit, int numColsLeft, out Matrix splitMatrixLeft, out Matrix splitMatrixRight)
        {
            float[,] data = toSplit.MatrixToArray();
            int numRows = data.GetLength(0);
            int numCols = data.GetLength(1);
            int numColsRight = numCols - numColsLeft;
            float[,] dataLeft = new float[numRows, numColsLeft];
            float[,] dataRight = new float[numRows, numColsRight];
            int j1;
            for (int i = 0; i < numRows; i++)
            {
                for (j1 = 0; j1 < numColsLeft; j1++)
                    dataLeft[i, j1] = data[i, j1];
                for (int j2 = 0; j2 < numColsRight; j2++)
                    dataRight[i, j2] = data[i, j1 + j2];
            }
            splitMatrixLeft = new Matrix(dataLeft);
            splitMatrixRight = new Matrix(dataRight);
        }

        /// <summary>
        /// Splits a matrix into two seperate matrices, with the split boundary being horizontal.
        /// </summary>
        /// <param name="toSplit">The matrix to be split.</param>
        /// <param name="numRowsTop">The number of rows the 'top' matrix should have.</param>
        /// <param name="splitMatrixTop">The 'top' matrix after the split.</param>
        /// <param name="splitMatrixBottom">The 'bottom' matrix after the split.</param>
        public void SplitMatrixAtRow(Matrix toSplit, int numRowsTop, out Matrix splitMatrixTop, out Matrix splitMatrixBottom)
        {
            float[,] data = toSplit.MatrixToArray();
            int numRows = data.GetLength(0);
            int numCols = data.GetLength(1);
            int numRowsBottom = numRows - numRowsTop;
            float[,] dataTop = new float[numRowsTop, numCols];
            float[,] dataBottom = new float[numRowsBottom, numCols];
            int i1;
            for (int j = 0; j < numRows; j++)
            {
                for (i1 = 0; i1 < numRowsTop; i1++)
                    dataTop[i1, j] = data[i1, j];
                for (int i2 = 0; i2 < numRowsBottom; i2++)
                    dataBottom[i2, j] = data[i1 + i2, j];
            }
            splitMatrixTop = new Matrix(dataTop);
            splitMatrixBottom = new Matrix(dataBottom);
        }

        /// <summary>
        /// Adds a specified number of zeroed row and column vectors to a vector/matrix.
        /// If the number of row and/or columns is negative, that many row and/or column vectors are deleted instead.
        /// </summary>
        /// <param name="numRows">The number of rows to be added.</param>
        /// <param name="numCols">The number of columns to be added.</param>
        public void Resize(int numRows, int numCols)
        {
            this.AddColumns(numCols);
            this.AddRows(numRows);
        }

        /// <summary>
        /// Adds a specified number of zeroed column vectors to a vector/matrix.
        /// If the number of columns is negative, that many column vectors are deleted instead.
        /// </summary>
        /// <param name="numCols">The number of columns to be added.</param>
        public void AddColumns(int numCols)
        {
            float[,] oldArray = this.MatrixToArray();
            int rows = oldArray.GetLength(0);
            int cols = oldArray.GetLength(1);
            float[,] newArray = null;

            switch (Math.Sign(numCols))
            {
                case -1:
                    // Columns are being deleted
                    newArray = new float[rows, cols + numCols];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols + numCols; j++)
                            newArray[i, j] = oldArray[i, j];
                    break;
                case 0:
                    // No change required
                    return;
                case 1:
                    // Columns are being added
                    newArray = new float[rows, cols + numCols];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            newArray[i, j] = oldArray[i, j];
                    break;
            }
            this.data = newArray;
            this.cols = cols + numCols;
        }

        /// <summary>
        /// Adds a specified number of zeroed row vectors to a vector/matrix.
        /// If the number of rows is negative, that many column vectors are deleted instead.
        /// </summary>
        /// <param name="numRows">The number of rows to be added.</param>
        public void AddRows(int numRows)
        {
            float[,] oldArray = this.MatrixToArray();
            int rows = oldArray.GetLength(0);
            int cols = oldArray.GetLength(1);
            float[,] newArray = null;

            switch (Math.Sign(numRows))
            {
                case -1:
                    // Rows are being deleted
                    newArray = new float[rows + numRows, cols];
                    for (int i = 0; i < rows + numRows; i++)
                        for (int j = 0; j < cols; j++)
                            newArray[i, j] = oldArray[i, j];
                    break;
                case 0:
                    // No change required
                    return;
                case 1:
                    // Rows are being added
                    newArray = new float[rows + numRows, cols];
                    for (int i = 0; i < rows; i++)
                        for (int j = 0; j < cols; j++)
                            newArray[i, j] = oldArray[i, j];
                    break;
            }
            this.data = newArray;
            this.rows = rows + numRows;
        }

        /// <summary>
        /// Outputs a matrix to the debug console as a formatted string.
        /// </summary>
        public void OutputMatrixDebug()
        {
            float[,] data = this.MatrixToArray();
            int lastPos = data.GetLength(0) * data.GetLength(1);

            string outputText = "( ";
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    outputText += data[i, j].ToString() + " ";
                    int pos = (i * data.GetLength(1) + j + 1);
                    if (pos % data.GetLength(1) == 0 && pos != data.GetLength(0) * data.GetLength(1))
                        outputText += "\n";
                }
            outputText += ")";
            Debug.WriteLine(outputText);
        }

        public Vector ConvertToVector(string type)
        {
            if (!((this.rows > 1 && this.cols == 1) || (this.rows == 1 && this.cols > 1)))
                throw new Exception("Matrix contains too many rows or columns.");

            float[] newVector;

            switch (type) {
                case "row":
                    newVector = new float[this.data.GetLength(1)];
                    for (int i = 0; i < this.data.GetLength(1); i++) newVector[i] = this.data[0,i];
                    break;
                case "column":
                    newVector = new float[this.data.GetLength(0)];
                    for (int i = 0; i < this.data.GetLength(0); i++) newVector[i] = this.data[i, 0];
                    break;
                default:
                    throw new Exception("Vector type is invalid.");
            }
            return new Vector(newVector,type);
        }
    }
}