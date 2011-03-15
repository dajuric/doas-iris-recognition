using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using OCR.GaborFilter.Misc;

namespace OCR.GaborFilter
{
    public class Gabor
    {
        public enum ImagePart
        {
            Amplitude,
            Phase,
            RealPart,
            ImagPart
        }
       
        private Complex[,] kernelValues;

        /// <summary>
        /// Kreira novi Gaborov filter
        /// </summary>
        /// <param name="side">Veličina pola strane Gaborovog filtra u nekoj dimenziji</param>
        /// <param name="scale">Faktor za skaliranje filtra. Veća vrijednost, veći filtar</param>
        /// <param name="theta">Kut zakreta gaborovog filtra</param>
        /// <param name="f">Frekvencija titranja sinusnog signala (u prostornoj domeni)</param>
        public Gabor(int side, double scale, double theta, double f)
        {
            Initialize(side, scale, theta, f, true);
        }        

        private void Initialize(int side, double scale, double theta, double f, bool removeDC)
        {
            kernelValues = new Complex[side * 2 + 1, side * 2 + 1];

            for (int x = -side; x <= side; x++)
            {
                for (int y = -side; y <= side; y++)
                {                    
                    double gauss = Math.Exp(-(x * x + y * y) / (2 * scale * scale));

                    double sinArgument = Math.PI / (f * scale) * (x * Math.Sin(theta) - y * Math.Cos(theta));
                    double sinReal = Math.Cos(sinArgument);
                    double sinImag = Math.Sin(sinArgument);
               
                    kernelValues[y + side, x + side].Re = gauss * sinReal; //y pa x jer slika (ByteImage) ima koordinate (row, col),a ne (x,y)
                    kernelValues[y + side, x + side].Im = gauss * sinImag;
                }
            }

            //oduzmi svakoj vrijednosti srednju vrijednost (Imaginarna vrijednost NEMA DC komponente)
            if (removeDC)
            {
                int numOfElements = (2 * side + 1) * (2 * side + 1);

                double sumOfElements = 0;

                foreach (Complex element in this.kernelValues)
                {
                    sumOfElements += element.Re;
                }

                double correctFactor = sumOfElements / numOfElements;

                for (int row = 0; row < 2 * side + 1; row++)
                {
                    for (int col = 0; col < 2 * side + 1; col++)
                    {
                        kernelValues[row, col] -= correctFactor;
                    }
                }
            }

        }

        public Complex[,] KernelValues
        {
            get 
            {
                return this.kernelValues;
            }
        }

        private static double[,] ScaleValues(double[,] values, double minValue, double maxValue)
        {
            double oldMin = Double.MaxValue;
            double oldMax = Double.MinValue;

            for (int row = 0; row <= values.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= values.GetUpperBound(1); col++)
                {
                    double value=values[row, col];
                    
                    if (value < oldMin)
                        oldMin = value;
                    else if (value > oldMax)
                        oldMax = value;
                }
            }

            double[,] scaledData = new double[values.GetLength(0), values.GetLength(1)];

            double constant = (maxValue - minValue) / (oldMax - oldMin);
            for (int row = 0; row <= values.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= values.GetUpperBound(1); col++)
                {
                    scaledData[row, col] = constant * (values[row, col] - oldMin);
                }
            }

            return scaledData;
        }

        private static double[,] GetAmplitudes(Complex[,] values)
        {
            double[,] amplitudes = new double[values.GetNumOfRows(), values.GetNumOfCols()];

            for (int row = 0; row <= values.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= values.GetUpperBound(1); col++)
                {
                    amplitudes[row, col] = values[row, col].Magnitude;
                }
            }

            return amplitudes;
        }

        private static double[,] GetPhases(Complex[,] values)
        {
            double[,] phases = new double[values.GetNumOfRows(), values.GetNumOfCols()];

            for (int row = 0; row <= values.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= values.GetUpperBound(1); col++)
                {
                    phases[row, col] = values[row, col].Phase;
                }
            }

            return phases;
        }

        private static double[,] GetRealParts(Complex[,] values)
        {
            double[,] realParts = new double[values.GetNumOfRows(), values.GetNumOfCols()];

            for (int row = 0; row <= values.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= values.GetUpperBound(1); col++)
                {
                    realParts[row, col] = values[row, col].Re;
                }
            }

            return realParts;
        }

        private static double[,] GetImagParts(Complex[,] values)
        {
            double[,] imagParts = new double[values.GetNumOfRows(), values.GetNumOfCols()];

            for (int row = 0; row <= values.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= values.GetUpperBound(1); col++)
                {
                    imagParts[row, col] = values[row, col].Im;
                }
            }

            return imagParts;
        }

        /// <summary>
        /// Izlučuje pojedine dijelove podataka i konvertira ih u sliku
        /// </summary>
        /// <param name="values">Matrica sa podacima</param>
        /// <param name="imagePart">Odabiranje željenog dijela podataka</param>
        /// <returns></returns>
        public static GeneralImage ToGeneralImage(Complex[,] values, ImagePart imagePart)
        {
            double[,] data = null;

            switch (imagePart)
            {
                case ImagePart.Amplitude:
                    data = Gabor.GetAmplitudes(values);               
                    break;
                case ImagePart.Phase:
                    data = Gabor.GetPhases(values);
                    break;
                case ImagePart.RealPart:
                    data = Gabor.GetRealParts(values);
                    break;
                case ImagePart.ImagPart:
                    data = Gabor.GetImagParts(values);
                    break;
            }

            double[,] scaledData = ScaleValues(data, 0, 255);
            return new GeneralImage(scaledData);
        }

        /// <summary>
        /// Vrši konvoluciju slike i filtra
        /// </summary>
        /// <param name="sourceImage">Slika nad kojoj se vrši konvolucija</param>
        /// <returns></returns>
        public Complex[,] Convolve(GeneralImage sourceImage)
        {
            int FFTNumOfCols = (int)Math.Pow(2.0, Math.Ceiling(Math.Log(kernelValues.GetNumOfCols() + sourceImage.Width, 2.0)));
            int FFTNumOfRows = (int)Math.Pow(2.0, Math.Ceiling(Math.Log(kernelValues.GetNumOfRows() + sourceImage.Height, 2.0)));

            Complex[,] FFTKernel = new Complex[FFTNumOfRows, FFTNumOfCols];
            Complex[,] FFTImage = new Complex[FFTNumOfRows, FFTNumOfCols];

            ComplexMatrix.Copy(this.kernelValues, FFTKernel, new Rectangle(0, 0, kernelValues.GetNumOfCols(), kernelValues.GetNumOfRows()), Point.Empty);

            ComplexMatrix.Copy(sourceImage.Data, FFTImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), Point.Empty);
            //FFTImage = FFTImage.Transponse();

            int fillX = Math.Min(sourceImage.Width, kernelValues.GetNumOfCols() / 2);
            int fillY = Math.Min(sourceImage.Height, kernelValues.GetNumOfRows() / 2);

            //zrcaljenje........

            ////desni dio slike
            //ComplexMatrix.Copy(FFTImage, FFTImage, new Rectangle(sourceImage.Width - fillX - 1, 0, fillX, sourceImage.Height),
            //                 new Point(sourceImage.Width, 0), ComplexMatrix.Mirror.Horizontal);

            ////donji dio slike
            //ComplexMatrix.Copy(FFTImage, FFTImage, new Rectangle(0, sourceImage.Height - fillY - 1, sourceImage.Width, fillY),
            //                 new Point(0, sourceImage.Height), ComplexMatrix.Mirror.Vertical);

            ////donji-desni dio slike
            //ComplexMatrix.Copy(FFTImage, FFTImage, new Rectangle(sourceImage.Width, sourceImage.Height - fillY - 1, fillX, fillY),
            //                 new Point(sourceImage.Width, sourceImage.Height), ComplexMatrix.Mirror.Vertical);

            ////lijevi produženi dio slike
            //ComplexMatrix.Copy(FFTImage, FFTImage, new Rectangle(0, 0, fillX, sourceImage.Height + fillY),
            //                 new Point(FFTImage.GetNumOfCols() - fillX, 0), ComplexMatrix.Mirror.Horizontal);

            ////gornji produženi dio sllike
            //ComplexMatrix.Copy(FFTImage, FFTImage, new Rectangle(0, 0, sourceImage.Width + fillX, fillY),
            //                 new Point(0, FFTImage.GetNumOfRows() - fillY), ComplexMatrix.Mirror.Vertical);

            ////gornji lijevi dio slike
            //ComplexMatrix.Copy(FFTImage, FFTImage, new Rectangle(0, 0, fillX, fillY),
            //                 new Point(FFTImage.GetNumOfCols() - fillX, FFTImage.GetNumOfRows() - fillY - 1), ComplexMatrix.Mirror.Vertical);

            //return new ByteImage(FFTImage);

            Fourier.FFT2(FFTKernel, Fourier.Direction.Forward); //transformacija jezgre
            Fourier.FFT2(FFTImage, Fourier.Direction.Forward);  //transformacija slike  

            Complex[,] IFFToutput = ComplexMatrix.MultiplyCellToCell(FFTKernel, FFTImage);
            Fourier.FFT2(IFFToutput, Fourier.Direction.Backward); //inverzna transformacija rezultata     
            Complex[,] output= new Complex[sourceImage.Height, sourceImage.Width];
            ComplexMatrix.Copy(IFFToutput, output, new Rectangle(fillX, fillY, sourceImage.Width, sourceImage.Height), Point.Empty);

            return output;
        }

    }
}
