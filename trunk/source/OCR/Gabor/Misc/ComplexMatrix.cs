using System;
using System.Collections.Generic;

using AForge.Math;
using System.Drawing;

namespace OCR.GaborFilter.Misc
{
    static class ComplexMatrix
    {
        public static int GetNumOfRows(this Complex[,] m)
        {
            return m.GetLength(0);
        }

        public static int GetNumOfCols(this Complex[,] m)
        {
            return m.GetLength(1);
        }
        
        public static Complex[,] Multiply(Complex[,] a, Complex[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) //broj stupaca prve ?= broj redaka druge
                throw new Exception("Specific Matrixes dimensions are not equal");

            int resRows = a.GetLength(0);
            int resCols = b.GetLength(1);
            Complex[,] res = new Complex[resRows, resCols];

            for (int rowA = 0; rowA < a.GetLength(0); rowA++)
            {
                for (int colB = 0; colB < b.GetLength(1); colB++)
                {
                    for (int jA = 0; jA < a.GetLength(1); jA++)
                    {
                        for (int iB = 0; iB < b.GetLength(0); iB++)
                        {
                            res[rowA, colB] = a[rowA, jA] * b[iB, colB];
                        }
                    }
                }
            }

            return res;
        }

        public static Complex[,] MultiplyByScalar(Complex[,] a, double scalar)
        {
            int resRows = a.GetLength(0);
            int resCols = a.GetLength(1);
            Complex[,] res = new Complex[resRows, resCols];

            for (int row = 0; row < a.GetLength(0); row++)
            {
                for (int col = 0; col < a.GetLength(1); col++)
                {
                    res[row, col] = a[row, col] * scalar;
                }
            }

            return res;
        }

        public static Complex[,] MultiplyCellToCell(Complex[,] a, Complex[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new Exception("Matrix dimensions must be the same!");

            int resRows = a.GetLength(0);
            int resCols = a.GetLength(1);
            Complex[,] res = new Complex[resRows, resCols];

            for (int row = 0; row < a.GetLength(0); row++)
            {
                for (int col = 0; col < a.GetLength(1); col++)
                {
                    res[row, col] = a[row, col] * b[row, col];
                }
            }

            return res;
        }

        public static Complex[,] Multiply(Complex[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) //broj stupaca prve ?= broj redaka druge
                throw new Exception("Specific Matrixes dimensions are not equal");

            int resRows = a.GetLength(0);
            int resCols = b.GetLength(1);
            Complex[,] res = new Complex[resRows, resCols];

            for (int rowA = 0; rowA < a.GetLength(0); rowA++)
            {
                for (int colB = 0; colB < b.GetLength(1); colB++)
                {
                    for (int jA = 0; jA < a.GetLength(1); jA++)
                    {
                        for (int iB = 0; iB < b.GetLength(0); iB++)
                        {
                            res[rowA, colB] = a[rowA, jA] * b[iB, colB];
                        }
                    }
                }
            }

            return res;
        }

        public static Complex[,] Multiply(double[,] a, Complex[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) //broj stupaca prve ?= broj redaka druge
                throw new Exception("Specific Matrixes dimensions are not equal");

            int resRows = a.GetLength(0);
            int resCols = b.GetLength(1);
            Complex[,] res = new Complex[resRows, resCols];

            for (int rowA = 0; rowA < a.GetLength(0); rowA++)
            {
                for (int colB = 0; colB < b.GetLength(1); colB++)
                {
                    for (int jA = 0; jA < a.GetLength(1); jA++)
                    {
                        for (int iB = 0; iB < b.GetLength(0); iB++)
                        {
                            res[rowA, colB] = a[rowA, jA] * b[iB, colB];
                        }
                    }
                }
            }

            return res;
        }

        public enum Mirror
        {
            Horizontal,
            Vertical,
            None
        }

        public static void Copy(Complex[,] sourceMatrix, Complex[,] destMatrix, Rectangle sourceRect, Point destPosition, Mirror mirroring)
        {
            switch (mirroring)
            {
                case Mirror.Horizontal:
                    CopyAndMirrorHorizontal(sourceMatrix, destMatrix, sourceRect, destPosition);
                    break;
                case Mirror.Vertical:
                    CopyAndMirrorVertical(sourceMatrix, destMatrix, sourceRect, destPosition);
                    break;
                default:
                    Copy(sourceMatrix, destMatrix, sourceRect, destPosition);
                    break;
            }
        }

        public static void Copy(Complex[,] sourceMatrix, Complex[,] destMatrix, Rectangle sourceRect, Point destPosition)
        {
            for (int row = 0; row < sourceRect.Height; row++)
            {
                for (int col = 0; col < sourceRect.Width; col++)
                {
                    destMatrix[row + destPosition.Y, col + destPosition.X] = sourceMatrix[row + sourceRect.Y, col + sourceRect.X];
                }
            }
        }   

        private static void CopyAndMirrorHorizontal(Complex[,] sourceMatrix, Complex[,] destMatrix, Rectangle sourceRect, Point destPosition)
        {
            for (int row = 0; row < sourceRect.Height; row++)
            {
                for (int col = 0; col < sourceRect.Width; col++)
                {
                    destMatrix[row + destPosition.Y, col + destPosition.X] = sourceMatrix[row + sourceRect.Y ,sourceRect.Right - col];
                }
            }
        }

        private static void CopyAndMirrorVertical(Complex[,] sourceMatrix, Complex[,] destMatrix, Rectangle sourceRect, Point destPosition)
        {
            for (int row = 0; row < sourceRect.Height; row++)
            {
                for (int col = 0; col < sourceRect.Width; col++)
                {
                    destMatrix[row + destPosition.Y, col + destPosition.X] = sourceMatrix[sourceRect.Bottom - row, col + sourceRect.X];
                }
            }
        }
        
        public static void Copy(int[,] sourceMatrix, Complex[,] destMatrix, Rectangle sourceRect, Point destPosition)
        {
            for (int row = 0; row < sourceRect.Height; row++)
            {
                for (int col = 0; col < sourceRect.Width; col++)
                {
                    destMatrix[row + destPosition.Y, col + destPosition.X].Re = sourceMatrix[row + sourceRect.Y, col + sourceRect.X];
                }
            }
        }

        public static void Copy(Complex[,] sourceMatrix, byte[,] destMatrix, Rectangle sourceRect, Point destPosition)
        {
            for (int x = 0; x < sourceRect.Width; x++)
            {
                for (int y = 0; y < sourceRect.Height; y++)
                {
                    destMatrix[x + destPosition.X, y + destPosition.Y] = (byte)sourceMatrix[x + sourceRect.X, y + sourceRect.Y].Re;
                }
            }
        }

        public static Complex[,] Transponse(this Complex[,] matrix)
        {
            Complex[,] res=new Complex[matrix.GetNumOfCols(), matrix.GetNumOfRows()];
            
            for (int x = 0; x < matrix.GetNumOfCols(); x++)
            {
                for (int y = 0; y < matrix.GetNumOfRows(); y++)
                {
                    res[x, y] = matrix[y, x];
                }
            }

            return res;
        }

    }
}
