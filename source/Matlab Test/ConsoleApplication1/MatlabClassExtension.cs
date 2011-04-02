using System;
using System.IO;
using AForge.Math;
using MLApp;

namespace ConsoleApplication1
{
    public static class MatlabClassExtension
    {
        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, bool value)
        {
            matlab.PutWorkspaceData(matrixName, "base", value);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, int value)
        {
            matlab.PutWorkspaceData(matrixName, "base", value);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, double value)
        {
            matlab.PutWorkspaceData(matrixName, "base", value);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, Complex value)
        {
            double[] matrixRe = new double[1];
            double[] matrixIm = new double[1];

            matrixRe[0] = value.Re;
            matrixIm[0] = value.Im;

            matlab.PutFullMatrix(matrixName, "base", matrixRe, matrixIm);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, int[] matrix)
        {
            matlab.PutWorkspaceData(matrixName, "base", matrix);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, double[] matrix)
        {
            matlab.PutWorkspaceData(matrixName, "base", matrix);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, Complex[] matrix)
        {
            double[] matrixRe, matrixIm;
            matrix.ConvertToDouble(out matrixRe, out matrixIm);

            matlab.PutFullMatrix(matrixName, "base", matrixRe, matrixIm);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, int[,] matrix)
        {
            matlab.PutWorkspaceData(matrixName, "base", matrix); 
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, double[,] matrix)
        {
            matlab.PutWorkspaceData(matrixName, "base", matrix);
        }

        public static void PutFullMatrix(this MLAppClass matlab, string matrixName, Complex[,] matrix)
        {
            double[,] matrixRe, matrixIm;
            matrix.ConvertToDouble(out matrixRe, out matrixIm);

            matlab.PutFullMatrix(matrixName, "base", matrixRe, matrixIm);
        }

        public static double[,] GetFullMatrix(this MLAppClass matlab, string matrixName)
        {
            return matlab.GetFullMatrix<double[,]>(matrixName);
        }

        public static T GetFullMatrix<T>(this MLAppClass matlab, string matrixName)
        {
            matlab.Execute("class (" + matrixName + ");");
            string varClass = (string)matlab.GetVariable("ans", "base");

            string requestedVariableType = typeof(T).Name;
            //requestedVariableType = requestedVariableType.Replace("[]", ""); //ne može se cast u []
            requestedVariableType = requestedVariableType.Replace("[,]", "");

            object output = null;
            switch (requestedVariableType)
            {
                case "Int32":
                    //je li potrebno pretvaranje matlab tipa varijable
                    if (varClass != "int32")
                    {
                        matlab.Execute("int32(" + matrixName + ");");
                        matrixName = "ans";
                    }
                    matlab.GetWorkspaceData(matrixName, "base", out output);
                    break;

                case "Double":
                    if (varClass != "double")
                    {
                        matlab.Execute("double(" + matrixName + ");");
                        matrixName = "ans";
                    }
                    matlab.GetWorkspaceData(matrixName, "base", out output);
                    break;

                case "Boolean":
                    if (varClass != "logical")
                    {
                        matlab.Execute("logical(" + matrixName + ");");
                        matrixName = "ans";
                    }
                    matlab.GetWorkspaceData(matrixName, "base", out output);
                    break;

                case "Complex":
                    matlab.Execute("size(" + matrixName + ", 1);");

                    int numOfRows = (int)((double)matlab.GetVariable("ans", "base"));
                    matlab.Execute("size(" + matrixName + ", 2);");
                    int numOfCols = (int)((double)matlab.GetVariable("ans", "base"));

                    if (varClass != "double")
                    {
                        matlab.Execute("double(" + matrixName + ");");
                        matrixName = "ans";
                    }

                    Array matrixRe = new double[numOfRows, numOfCols];
                    Array matrixIm = new double[numOfRows, numOfCols];

                    matlab.GetFullMatrix(matrixName, "base", ref matrixRe, ref matrixIm);

                    Complex[,] res = MatlabClassExtension.ConvertToComplex((double[,])matrixRe, (double[,])matrixIm);
                    output = res;
                              
                    break;

                default:
                    throw new NotSupportedException(); 
            }
    
            return (T)output;
        }

        public static string GetMyScriptFolder(this MLAppClass matlab)
        {
            string currentDir = Directory.GetCurrentDirectory();  // App\bin\Relase
            DirectoryInfo parentDir = Directory.GetParent(currentDir); //App\bin
            parentDir = Directory.GetParent(parentDir.FullName);  //App

            return Path.Combine(parentDir.FullName, "MatlabScripts"); //App\MatlabScripts
        }

        public static void ChangeDirectory(this MLAppClass matlab, string path)
        {
            matlab.Execute("cd '" + path + "'");
        }

        public static double[,] ConvertToDouble(this int[,] matrix)
        {
            int numOfRows = matrix.GetLength(0);
            int numOfCols = matrix.GetLength(1);

            double[,] matrixDouble = new double[numOfRows, numOfCols];

            for (int i = 0; i < numOfRows; i++)
            {
                for (int j = 0; j < numOfCols; j++)
                {
                    matrixDouble[i, j] = matrix[i, j];
                }
            }

            return matrixDouble;
        }


        public static void ConvertToDouble(this Complex[] matrix, out double[] matrixRe, out double[] matrixIm)
        {
            int numOfElems = matrix.GetLength(0);

            matrixRe = new double[numOfElems];
            matrixIm = new double[numOfElems];

            for (int i = 0; i < numOfElems; i++)
            {
                    matrixRe[i] = matrix[i].Re;
                    matrixIm[i] = matrix[i].Im;
            }
        }

        public static void ConvertToDouble(this Complex[,] matrix, out double[,] matrixRe, out double[,] matrixIm)
        {
            int numOfRows = matrix.GetLength(0);
            int numOfCols = matrix.GetLength(1);

            matrixRe = new double[numOfRows, numOfCols];
            matrixIm = new double[numOfRows, numOfCols];

            for (int i = 0; i < numOfRows; i++)
            {
                for (int j = 0; j < numOfCols; j++)
                {
                    matrixRe[i, j] = matrix[i, j].Re;
                    matrixIm[i, j] = matrix[i, j].Im;
                }
            }
        }

        public static Complex[,] ConvertToComplex(double[,] matrixRe, double[,] matrixIm)
        {
            int numOfRows = matrixRe.GetLength(0);
            int numOfCols = matrixRe.GetLength(1);

            Complex[,] matrix = new Complex[numOfRows, numOfCols];

            for (int i = 0; i < numOfRows; i++)
            {
                for (int j = 0; j < numOfCols; j++)
                {
                    matrix[i, j] = new Complex(matrixRe[i, j], matrixIm[i, j]);
                }
            }

            return matrix;
        }

        public static int[,] ConvertToInt32(this double[,] matrix)
        {
            int numOfRows = matrix.GetLength(0);
            int numOfCols = matrix.GetLength(1);

            int[,] matrixInt = new int[numOfRows, numOfCols];

            for (int i = 0; i < numOfRows; i++)
            {
                for (int j = 0; j < numOfCols; j++)
                {
                    matrixInt[i, j] = (int)matrix[i, j];
                }
            }

            return matrixInt;
        }
    }
}
