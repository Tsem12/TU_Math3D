using System;

namespace Maths_Matrices.Tests
{
    public class MatrixElementaryOperations
    {

        #region MatrixInt

        public static void SwapLines(MatrixInt matrix, int line1, int line2)
        {
            int[] temp = new int[matrix.Matrix.GetLength(1)];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = matrix[line1, i];
                matrix[line1, i] = matrix[line2, i];
            }

            for (int i = 0; i < temp.Length; i++)
            {
                matrix[line2, i] = temp[i];
            }
        }

        public static void SwapColumns(MatrixInt matrix, int col1, int col2)
        {
            int[] temp = new int[matrix.Matrix.GetLength(0)];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = matrix[i, col1];
                matrix[i, col1] = matrix[i, col2];
            }

            for (int i = 0; i < temp.Length; i++)
            {
                matrix[i, col2] = temp[i];
            }
        }

        public static void MultiplyLine(MatrixInt matrix, int line, int value)
        {
            if (value == 0)
                throw new MatrixScalarZeroException();
            
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                matrix[line, i] *= value;
            }
        }
        
        public static void MultiplyColumn(MatrixInt matrix, int column, int value)
        {
            if (value == 0)
                throw new MatrixScalarZeroException();
            
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                matrix[i, column] *= value;
            }
        }

        public static void AddLineToAnother(MatrixInt matrix, int line1, int line2, int value)
        {
            for (int i = 0; i < matrix.Matrix.GetLength(1); i++)
            {
                matrix[line2, i] += matrix[line1, i] * value;
            }
        }

        public static void AddColumnToAnother(MatrixInt matrix, int line1, int line2, int value)
        {
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                matrix[i, line2] += matrix[i, line1] * value;
            }
        }

        #endregion

        #region MatrixFloat

        public static void SwapLines(MatrixFloat matrix, int line1, int line2)
        {
            float[] temp = new float[matrix.Matrix.GetLength(1)];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = matrix[line1, i];
                matrix[line1, i] = matrix[line2, i];
            }

            for (int i = 0; i < temp.Length; i++)
            {
                matrix[line2, i] = temp[i];
            }
        }

        public static void SwapColumns(MatrixFloat matrix, int col1, int col2)
        {
            float[] temp = new float[matrix.Matrix.GetLength(0)];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = matrix[i, col1];
                matrix[i, col1] = matrix[i, col2];
            }

            for (int i = 0; i < temp.Length; i++)
            {
                matrix[i, col2] = temp[i];
            }
        }

        public static void MultiplyLine(MatrixFloat matrix, int line, float value)
        {
            if (value == 0)
                throw new MatrixScalarZeroException();
            
            for (int i = 0; i < matrix.NbColumns; i++)
            {
                matrix[line, i] *= value;
            }
        }
        
        public static void MultiplyColumn(MatrixFloat matrix, int column, float value)
        {
            if (value == 0)
                throw new MatrixScalarZeroException();
            
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                matrix[i, column] *= value;
            }
        }

        public static void AddLineToAnother(MatrixFloat matrix, int line1, int line2, float value)
        {
            for (int i = 0; i < matrix.Matrix.GetLength(1); i++)
            {
                matrix[line2, i] += matrix[line1, i] * value;
            }
        }

        public static void AddColumnToAnother(MatrixFloat matrix, int line1, int line2, float value)
        {
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                matrix[i, line2] += matrix[i, line1] * value;
            }
        }

        #endregion
    }
}