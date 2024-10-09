using System;

namespace Maths_Matrices.Tests
{
    public class MatrixFloat
    {
        private float[,] _matrix;
        private int _nbLines;
        private int _nbColumns;

        #region Constructors

        public MatrixFloat(int lines, int columns)
        {
            _nbLines = lines;
            _nbColumns = columns;
            _matrix = new float[lines,columns];
        }
        public MatrixFloat(float[,] array2D)
        {
            _matrix = array2D;
            _nbLines = array2D.GetLength(0);
            _nbColumns = array2D.GetLength(1);
        }
        public MatrixFloat(MatrixFloat matrix)
        {
            _nbLines = matrix.NbLines;
            _nbColumns = matrix.NbColumns;
            _matrix = new float[_nbLines, _nbColumns];
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
        public float[,] Matrix => _matrix;
        public float[,] ToArray2D() => Matrix;
        
        public Vector4 ToVector4()
        {
            if (NbColumns != 1)
                return null;

            return new Vector4(this[0, 0], this[1, 0], this[2, 0], this[3, 0]);
        }
        
        public Vector3 ToVector3()
        {
            if (NbColumns != 1)
                return null;

            return new Vector3(this[0, 0], this[1, 0], this[2, 0]);
        }
        public float this[int i, int j]
        {
            get => Matrix[i, j];
            set => Matrix[i, j] = value;
        }

        #endregion

        #region Identity

            public static MatrixFloat Identity(int size)
            {
                float[,] result = new float[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        result[i, j] = i == j ? 1 : 0;
                    }
                }
                return new MatrixFloat(result);
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

        public void Multiply(float value)
        {
            for (int i = 0; i < _nbLines; i++)
            {
                for (int j = 0; j < _nbColumns; j++)
                {
                    _matrix[i, j] = _matrix[i, j] * value;
                }
            }
        }

        public static MatrixFloat Multiply(MatrixFloat matrix, float value)
        {
            MatrixFloat newMatrix = new MatrixFloat(matrix);
            newMatrix.Multiply(value);
            return newMatrix;
        }

        public static MatrixFloat operator -(MatrixFloat a)
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
        
        public static MatrixFloat operator *(MatrixFloat a, int value)
        {
            return MatrixFloat.Multiply(a, value);
        }

        public static MatrixFloat operator *(int value, MatrixFloat a) => a * value;

        #endregion

        #region Addition

        public void Add(MatrixFloat value)
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

        public static MatrixFloat Add(MatrixFloat a, MatrixFloat b)
        {
            MatrixFloat result = new MatrixFloat(a);
            result.Add(b);
            return result;
        }

        public static MatrixFloat operator +(MatrixFloat a, MatrixFloat b) => MatrixFloat.Add(a, b);
        public static MatrixFloat operator -(MatrixFloat a, MatrixFloat b) => MatrixFloat.Add(a, -b);

        #endregion

        #region Multiply

        public MatrixFloat Multiply(MatrixFloat value)
        {
            if (_nbColumns != value.NbLines)
                throw new MatrixMultiplyException();

            float[,] result = new float[_nbLines, value.NbColumns];

            for (int i = 0; i < _nbLines; i++)
            {
                for (int j = 0; j < value.NbColumns; j++)
                {
                    float cellValue = 0;
                    for (int k = 0; k < _nbColumns; k++)
                    {
                        cellValue += this[i, k] * value[k, j];  
                    }
                    result[i, j] = cellValue;
                }
            }
            return new MatrixFloat(result);
        }
        public static MatrixFloat Multiply(MatrixFloat a, MatrixFloat b) => a.Multiply(b);

        public static MatrixFloat operator *(MatrixFloat a, MatrixFloat b) => MatrixFloat.Multiply(a, b);
        
        #endregion

        #region Transpose

        public MatrixFloat Transpose()
        {
            float[,] result = new float[NbColumns, NbLines];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = this[j, i];
                }
            }
            return new MatrixFloat(result);
        }
        public static MatrixFloat Transpose(MatrixFloat matrix) => matrix.Transpose();

        #endregion
        
        #region AugmentedMatrix

        public static MatrixFloat GenerateAugmentedMatrix(MatrixFloat matrix1, MatrixFloat matrix2)
        {
            if (matrix1.NbLines != matrix2.NbLines)
                throw new MatrixAugmentException();
            
            MatrixFloat result = new MatrixFloat(matrix1.NbLines, matrix1.NbColumns + matrix2.NbColumns);
            for (int i = 0; i < result.NbLines; i++)
            {
                for (int j = 0; j < result.NbColumns; j++)
                {
                    result[i, j] = j >= matrix1.NbColumns ? matrix2[i, j - matrix1.NbColumns] : matrix1[i, j];
                }
            }

            return new MatrixFloat(result);
        }

        public (MatrixFloat matrix1, MatrixFloat matrix2) Split(int separatorIndex)
        {
            MatrixFloat result1 = new MatrixFloat(NbLines, separatorIndex + 1);
            MatrixFloat result2 = new MatrixFloat(NbLines, NbColumns - separatorIndex - 1);

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
                        result2[i, j - result1.NbColumns] = this[i, j];
                    }
                }
            }

            return (result1,result2);
        }
        
        #endregion

        #region RowReduction

        public MatrixFloat InvertByRowReduction()
        {
            MatrixFloat result = MatrixRowReductionAlgorithm.Apply(this, Identity(NbLines)).matrix2;
            for (int i = 0; i < result.NbColumns; i++)
            {
                int nullCount = 0;
                for (int j = 0; j < result.NbLines; j++)
                {
                    if (result[i, j] == 0)
                    {
                        nullCount++;
                    }
                }

                if (nullCount >= result.NbLines)
                    throw new MatrixInvertException();
            }
            return result;
        }

        public static MatrixFloat InvertByRowReduction(MatrixFloat matrix) => matrix.InvertByRowReduction();

        #endregion

        #region SubMatrice

        public MatrixFloat SubMatrix(int line, int col)
        {
            MatrixFloat result = new MatrixFloat(NbLines - 1, NbColumns - 1);

            int lineReduction = 0;
            int colReduction = 0;
            for (int i = 0; i < NbLines; i++)
            {
                if (line == i)
                {
                    lineReduction = 1;
                    continue;
                }

                colReduction = 0;
                for (int j = 0; j < NbColumns; j++)
                {
                    if (col == j)
                    {
                        colReduction = 1;
                        continue;
                    }

                    result[i - lineReduction, j - colReduction] = this[i, j];
                }
            }

            return result;
        }

        public static MatrixFloat SubMatrix(MatrixFloat matrix, int line, int col) => matrix.SubMatrix(line, col);

        #endregion

        #region Determinant
        
        public static float Determinant(MatrixFloat matrixFloat)
        {
            if (matrixFloat.NbLines == 1 && matrixFloat.NbColumns == 1)
                return matrixFloat[0, 0];
            
            if (matrixFloat.NbLines == 2 && matrixFloat.NbColumns == 2)
            {
                return matrixFloat[0, 0] * matrixFloat[1, 1] - matrixFloat[0, 1] * matrixFloat[1, 0];
            }

            float summedMatrix = 0f;
            for (int col = 0; col < matrixFloat.NbColumns; col++)
            {
                summedMatrix += (col % 2 == 0 ? 1 : -1) * (matrixFloat[0, col] * Determinant(matrixFloat.SubMatrix(0, col)));
            }
            return summedMatrix;
            
        }
        
        #endregion

        #region Adjugate

        public MatrixFloat Adjugate()
        {
            float determinant = Determinant(this); 
            if (determinant == 0)
                throw new MatrixInvertException();

            float[,] adjugateMatrix = new float[NbLines, NbColumns];
            MatrixFloat result = new MatrixFloat(adjugateMatrix);
            MatrixFloat transposedMatrix = Transpose();

            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    result[i, j] = (float)Math.Pow(-1, i + j) * Determinant(SubMatrix(transposedMatrix,i, j));
                }
            }
            return result;
        }

        public static MatrixFloat Adjugate(MatrixFloat matrixFloat) => matrixFloat.Adjugate();

        #endregion

        #region InvertByDeterminant

            public MatrixFloat InvertByDeterminant()
            {
                float determinant = Determinant(this);
                MatrixFloat adjugateMatrix = Adjugate();
                return Multiply(adjugateMatrix, 1f / determinant);
            }

            public static MatrixFloat InvertByDeterminant(MatrixFloat matrixFloat) => matrixFloat.InvertByDeterminant();

            #endregion
            
    }
}