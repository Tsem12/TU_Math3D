using System;
using System.Numerics;


namespace Maths_Matrices.Tests
{
    public class MatrixInt
    {
        private int[,] _matrix;
        private int _nbLines;
        private int _nbColumns;

        #region Constructors

        public MatrixInt(int lines, int columns)
        {
            _nbLines = lines;
            _nbColumns = columns;
            _matrix = new int[lines,columns];
        }
        public MatrixInt(int[,] array2D)
        {
            _matrix = array2D;
            _nbLines = array2D.GetLength(0);
            _nbColumns = array2D.GetLength(1);
        }
        public MatrixInt(MatrixInt matrix)
        {
            _nbLines = matrix.NbLines;
            _nbColumns = matrix.NbColumns;
            _matrix = new int[_nbLines, _nbColumns];
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.Matrix.GetLength(1); j++)
                {
                    _matrix[i,j] = matrix[i,j];
                }
            }
        }

        #endregion

        #region Properties

        public int NbColumns => _nbColumns;
        public int NbLines => _nbLines;
        public int[,] Matrix => _matrix;
        public int[,] ToArray2D() => Matrix;
        public int this[int i, int j]
        {
            get => Matrix[i, j];
            set => Matrix[i, j] = value;
        }

        #endregion

        #region Identity

            public static MatrixInt Identity(int size)
            {
                int[,] result = new int[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result[i, j] = i == j ? 1 : 0;
                    }
                }
                return new MatrixInt(result);
            }

            public bool IsIdentity()
            {
                if (_nbColumns != _nbLines)
                    return false;
                
                for (int i = 0; i < _nbLines; i++)
                {
                    for (int j = 0; j < _nbLines; j++)
                    {
                        int identityValue = i == j ? 1 : 0;
                        if (_matrix[i, j] != identityValue)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

        #endregion

        #region ScalaireMultiply

        public void Multiply(int value)
        {
            for (int i = 0; i < _nbLines; i++)
            {
                for (int j = 0; j < _nbColumns; j++)
                {
                    _matrix[i, j] = _matrix[i, j] * value;
                }
            }
        }

        public static MatrixInt Multiply(MatrixInt matrix, int value)
        {
            MatrixInt newMatrix = new MatrixInt(matrix);
            newMatrix.Multiply(value);
            return newMatrix;
        }

        public static MatrixInt operator -(MatrixInt a)
        {
            for (int i = 0; i < a.NbLines; i++)
            {
                for (int j = 0; j < a.NbColumns; j++)
                {
                    a[i, j] = -a[i, j];
                }
            }
            return a;
        }
        
        public static MatrixInt operator *(MatrixInt a, int value)
        {
            return MatrixInt.Multiply(a, value);
        }

        public static MatrixInt operator *(int value, MatrixInt a) => a * value;

        #endregion

        #region Addition

        public void Add(MatrixInt value)
        {
            if (_nbColumns != value.NbColumns || _nbLines != value.NbLines)
                throw new MatrixSumException();
            
            for (int i = 0; i < _nbLines; i++)
            {
                for (int j = 0; j < _nbColumns; j++)
                {
                    _matrix[i,j] = _matrix[i,j] + value[i, j];
                }
            }
        }

        public static MatrixInt Add(MatrixInt a, MatrixInt b)
        {
            MatrixInt result = new MatrixInt(a);
            result.Add(b);
            return result;
        }

        public static MatrixInt operator +(MatrixInt a, MatrixInt b) => MatrixInt.Add(a, b);
        public static MatrixInt operator -(MatrixInt a, MatrixInt b) => MatrixInt.Add(a, -b);

        #endregion

        #region Multiply

        public MatrixInt Multiply(MatrixInt value)
        {
            if (_nbColumns != value.NbLines)
                throw new MatrixMultiplyException();

            int[,] result = new int[_nbLines, value.NbColumns];

            for (int i = 0; i < _nbLines; i++)
            {
                for (int j = 0; j < value.NbColumns; j++)
                {
                    int cellValue = 0;
                    for (int k = 0; k < _nbColumns; k++)
                    {
                        cellValue += this[i, k] * value[k, j];  
                    }
                    result[i, j] = cellValue;
                }
            }
            return new MatrixInt(result);
        }
        public static MatrixInt Multiply(MatrixInt a, MatrixInt b) => a.Multiply(b);

        public static MatrixInt operator *(MatrixInt a, MatrixInt b) => MatrixInt.Multiply(a, b);
        
        #endregion

        #region Transpose

        public MatrixInt Transpose()
        {
            int[,] result = new int[NbColumns, NbLines];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = this[j, i];
                }
            }
            return new MatrixInt(result);
        }
        public static MatrixInt Transpose(MatrixInt matrix) => matrix.Transpose();

        #endregion

        #region AugmentedMatrix

        public static MatrixInt GenerateAugmentedMatrix(MatrixInt matrix1, MatrixInt matrix2)
        {
            if (matrix1.NbLines != matrix2.NbLines)
                throw new MatrixAugmentException();
            
            int[,] result = new int[matrix1.NbLines, matrix1.NbColumns + matrix2.NbColumns];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = j >= matrix1.NbColumns ? matrix2[i, matrix1.NbColumns - j] : matrix1[i, j];
                }
            }

            return new MatrixInt(result);
        }

        public (MatrixInt matrix1, MatrixInt matrix2) Split(int separatorIndex)
        {
            int[,] result1 = new int[NbLines, NbColumns - (separatorIndex - 1)];
            int[,] result2 = new int[NbLines, separatorIndex - 1];

            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    if (j <= separatorIndex)
                    {
                        result1[i, j] = this[i, j];
                    }
                    else
                    {
                        result2[i, result1.GetLength(1) - j] = this[i, j];
                    }
                }
            }

            return (new MatrixInt(result1), new MatrixInt(result2));
        }
        
        #endregion
    }
}
