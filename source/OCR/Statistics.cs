using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord.Math;

namespace OCR
{
     public static partial class Statistics
    {
        /// <summary>Računamo mean(očekivanje) matrice.</summary>
        /// <param name="matrix">matrica čije očekivanje računamo</param>
        /// <returns>vraća redak matricu koji sadržava središnje vrijednosti pojednih stupaca</returns>
      
        public static double[] Mean(double[,] matrix)
        {
            return Mean(matrix, MatrixDimension.Column);
        }

        /// <summary>Računamo mean(očekivanje) matrice.</summary>
        /// <param name="matrix">matrica čije očekivanje računamo</param>
        /// <param name="dimension">
        /// //parametar koji odlučuje preko koje dimenzije se računa
        /// središnja vrijednosti, ako stavimo MatrixDimension.Column računamo vektor redak 
        /// središnjih vrijednosti stupca(znači središnja vrijednost stupca)
        /// ako stavimo MatrixDimension.Row računamo središnje vrijednosti redaka
        /// po defaultu je MatrixDimens.Column
        /// </param>
        /// <returns>Returns a vector containing the means of the given matrix.</returns>
      
        public static double[] Mean(double[,] matrix, MatrixDimension dimension)
        {
            if (dimension == MatrixDimension.Column)
            {
                double[] mean = new double[matrix.GetLength(1)];
                double rows = matrix.GetLength(0);

                // za svaki stupac
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    // za svaki redak
                    for (int i = 0; i < matrix.GetLength(0); i++)
                        mean[j] += matrix[i, j];

                    mean[j] = mean[j] / rows;
                }

                return mean;
            }
            else if (dimension == MatrixDimension.Row)
            {
                double[] mean = new double[matrix.GetLength(0)];
                double cols = matrix.GetLength(1);

                //za svaki redak
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    // za svaki stupac
                    for (int i = 0; i < matrix.GetLength(1); i++)
                        mean[j] += matrix[j, i];

                    mean[j] = mean[j] / cols;
                }

                return mean;
            }
            else
            {
                throw new ArgumentException("Invalid dimension.", "dimension");
            }
        }

        /// <summary>standrdna devijacija matrice</summary>
        /// <param name="matrix">matrica čija se devijacija računa</param>
        /// <returns>vraća se vektor standradnih devijacija .</returns>
        public static double[] StandardDeviation(double[,] matrix)
        {
            return StandardDeviation(matrix, Mean(matrix));
        }

        /// <summary>standrdna devijacija matrice</summary>
        /// <param name="matrix">matrica čija se devijacija računa</param>
        /// <param name="means">za svaki stupac već izračunati mean.</param>
        /// <returns>vektor standardnih devijacija</returns>
        public static double[] StandardDeviation(this double[,] matrix, double[] means)
        {
            return Matrix.Sqrt(Variance(matrix, means));
        }

        /// <summary>varijanca matrice</summary>
        /// <param name="matrix">matrica čiju varijancu računamo</param>
        /// <returns>vektor varijanci</returns>
        public static double[] Variance(this double[,] matrix)
        {
            return Variance(matrix, Mean(matrix));
        }

        /// <summary>varijanca matrice</summary>
        /// <param name="matrix">matrica čiju varijancu računamo</param>
        /// <param name="means">za svaki stupac već izračunati mean</param>
        /// <returns>vektor varijanci</returns>
        public static double[] Variance(this double[,] matrix, double[] means)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double N = rows;

            double[] variance = new double[cols];

            // for each column (for each variable)
            for (int j = 0; j < cols; j++)
            {
                double sum1 = 0.0;
                double sum2 = 0.0;
                double x = 0.0;

                // for each row (observation of the variable)
                for (int i = 0; i < rows; i++)
                {
                    x = matrix[i, j] - means[j];
                    sum1 += x;
                    sum2 += x * x;
                }

                // calculate the variance
                variance[j] = (sum2 - ((sum1 * sum1) / N)) / (N - 1);
            }

            return variance;
        }


        /// <summary>
        ///   računamo matricu raspršenosti 
        /// </summary>
        /// <remarks>
      
        /// </remarks>
        /// <param name="matrix">matrica koja sadrži ulzazne vrijednosti</param>
        /// <param name="means">mean vektori ulzanih podataka</param>
        /// <returns>kovarijacijska matrica</returns>
        public static double[,] Scatter(double[,] matrix, double[] means)
        {
            return Scatter(matrix, means, 1.0, 0);
        }

        /// <summary>
        ///   računamo matricu raspršenosti 
        /// </summary>
     
        /// <param name="matrix">matrica koja sadrži ulzazne vrijednosti</param>
        /// <param name="means">mean vektori ulzanih podataka</param>
        /// <param name="divisor">ako hocemo podijeliti svaki element s nekim brojem</param>
        /// <returns>kovarijacijska matrica</returns>
        public static double[,] Scatter(double[,] matrix, double[] means, double divisor)
        {
            return Scatter(matrix, means, divisor, MatrixDimension.Column);
        }

        /// <summary>
        ///   računamo matricu raspršenosti 
        /// </summary>
       
        /// <param name="matrix">matrica koja sadrži ulzazne vrijednosti</param>
        /// <param name="means">mean vektori ulzanih podataka</param>
        /// <param name="dimension">
        ///   Pass 0 to if mean vector is a row vector, 1 otherwise. Default value is 0.
        /// </param>
        /// <returns>kovarijacijska matrica</returns>
        public static double[,] Scatter(double[,] matrix, double[] means, MatrixDimension dimension)
        {
            return Scatter(matrix, means, 1.0, dimension);
        }

        /// <summary>
        ///   računamo matricu raspršenosti 
        /// </summary>
      
        /// <param name="matrix">matrica koja sadrži ulzazne vrijednosti.</param>
        /// <param name="means">mean vektori ulzanih podataka</param>
        /// <param name="divisor">ako hocemo podijeliti svaki element s nekim brojem</param>
        /// <param name="dimension">
        ///   kao i uvijek MatrixDimension, po kojoj računamo
        /// </param>
        /// <returns>kovarijacijska matrica</returns>
        public static double[,] Scatter(double[,] matrix, double[] means, double divisor, MatrixDimension dimension)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] cov;

            if (dimension == MatrixDimension.Column)
            {
                if (means.Length != cols) throw new ArgumentException(
                    "Length of the mean vector should equal the number of columns", "mean");

                cov = new double[cols, cols];
                for (int i = 0; i < cols; i++)
                {
                    //matrica je simetrična
                    for (int j = i; j < cols; j++)
                    {
                        double s = 0.0;
                        for (int k = 0; k < rows; k++)
                            s += (matrix[k, j] - means[j]) * (matrix[k, i] - means[i]);
                        s /= divisor;
                        cov[i, j] = s;
                        cov[j, i] = s;
                    }
                }
            }
            else if (dimension == MatrixDimension.Row)
            {
                if (means.Length != rows) throw new ArgumentException(
                    "Length of the mean vector should equal the number of rows", "mean");

                cov = new double[rows, rows];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = i; j < rows; j++)
                    {
                        double s = 0.0;
                        for (int k = 0; k < cols; k++)
                            s += (matrix[j, k] - means[j]) * (matrix[i, k] - means[i]);
                        s /= divisor;
                        cov[i, j] = s;
                        cov[j, i] = s;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Invalid dimension.", "dimension");
            }

            return cov;
        }

       

       
    }
}
