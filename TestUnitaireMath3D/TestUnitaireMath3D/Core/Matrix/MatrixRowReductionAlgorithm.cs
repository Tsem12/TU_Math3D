namespace Maths_Matrices.Tests
{
    public class MatrixRowReductionAlgorithm
    {
        public static (MatrixFloat matrix1, MatrixFloat matrix2) Apply(MatrixFloat matrix1, MatrixFloat matrix2)
        {
            MatrixFloat result = MatrixFloat.GenerateAugmentedMatrix(matrix1, matrix2);
            bool canThrowExeption = matrix2.NbColumns == matrix1.NbColumns;

            for (int i = 0; i < result.NbLines; i++)
            {
                (int index, float value) kIndexFound = (i, result[i, i]);
                int nullValueCount = 0;
                for (int k = i; k < result.NbLines; k++)
                {
                    if (result[k, i] == 0)
                    {
                        nullValueCount++;
                        continue;
                    }
                    if (kIndexFound.value <= result[k, i])
                    {
                        kIndexFound = (k, result[k, i]);
                    }
                }

                if (nullValueCount < result.NbLines - i)
                {
                    if (kIndexFound.index != i)
                    {
                        MatrixElementaryOperations.SwapLines(result, i, kIndexFound.index);
                        kIndexFound.index = i;
                    }
                    MatrixElementaryOperations.MultiplyLine(result ,i ,1/result[i, i]);
                    for (int r = 0; r < result.NbLines; r++)
                    {
                        if (r != kIndexFound.index)
                        {
                            MatrixElementaryOperations.AddLineToAnother(result, i, r, -result[r,i]);
                        }
                    }
                }
                else if (canThrowExeption)
                    throw new MatrixInvertException();
            }

            return result.Split(matrix1.NbColumns-1);
        }
    }
}