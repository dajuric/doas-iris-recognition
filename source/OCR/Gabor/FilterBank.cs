using System;
using System.Collections.Generic;
using OCR.Common;
using AForge.Math;

namespace OCR.GaborFilter
{
    //parametri preuzeti sa: http://www.ansatt.hig.no/erikh/papers/scia99/node6.html  
    public class FilterBank
    {
        public enum Work
        { 
            ImageConvolution,
            ImageConversion //misli se na pretvraranje u Complex[,] -> ByteImage
        }
        
        public delegate void delPercentCompleted(Work w, float percent);
        public event delPercentCompleted OnPrecentCompleted;
        
        private Gabor[,] filters;
        
        private double[] scaleFactors;
        private double[] orientations;

        /// <summary>
        /// Kreira slog Gaborovih filtera
        /// </summary>
        /// <param name="maxScale">Maksimalna veličina Gaborovih funkcija </param>
        /// <param name="numOfScales">Broj filtera različith veličina</param>
        /// <param name="numOfOrientations">Broj filtera različitih orijentacija</param>
        /// <param name="side">Veličina 2*side = veličina slike</param>
        /// <param name="f">Frekvenicja Gaborovih funkcija</param>
        public FilterBank(int maxScale, int numOfScales, int numOfOrientations, int side, double f)
        {
            scaleFactors = GenerateScales(maxScale, numOfScales);
            orientations = GenerateOrientations(numOfOrientations);

            filters = new Gabor[numOfScales, numOfOrientations];

            for (int scale = 0; scale < scaleFactors.Length; scale++)
            {
                for (int orientation = 0; orientation < orientations.Length; orientation++)
                {
                    filters[scale, orientation] = new Gabor(side, scaleFactors[scale], orientations[orientation], f);
                }
            }

        }

        public Gabor[,] Filters
        {
            get { return filters; }
        }

        public int NumberOfFilters
        {
            get { return this.filters.GetLength(0) * this.filters.GetLength(1); }
        }

        public int NumberOfScales
        {
            get { return this.filters.GetLength(0); }
        }

        public int NumberOfOrientations
        {
            get { return this.filters.GetLength(1); }
        }

        public List<Complex[,]> Kernels
        {
            get
            {
                List<Complex[,]> kernels = new List<Complex[,]>();
                
                foreach (Gabor g in this.Filters)
                {
                    kernels.Add(g.KernelValues);
                }

                return kernels;
            }
        }

        public List<Complex[,]> Convolve(GeneralImage image)
        {
            List<Complex[,]> convImages = new List<Complex[,]>();

            int i=0;
            foreach (Gabor g in this.Filters)
            {
                convImages.Add(g.Convolve(image));

                i++;
                if(OnPrecentCompleted!=null)
                    OnPrecentCompleted(Work.ImageConvolution, (float)i / this.filters.Length);
            }

            return convImages;
        }

        public static List<GeneralImage> ToGeneralImages(List<Complex[,]> complexImages, Gabor.ImagePart imagePart)
        {
            List<GeneralImage> images = new List<GeneralImage>();

            //int i = 0;
            foreach (Complex[,] convolvedImage in complexImages)
            {
                images.Add(Gabor.ToGeneralImage(convolvedImage, imagePart));

                //i++;
                //if (OnPrecentCompleted != null)
                //    OnPrecentCompleted(Work.ImageConversion, (float)i / complexImages.Count);
            }

            return images;
        }

        private double[] GenerateScales(int maxScale, int numOfScales)
        {
            double[] scales = new double[numOfScales];

            for (int i = 0; i < numOfScales-1; i++)
            {
                scales[i+1] = maxScale * Math.Pow(2, -(i+ 1.5)/2f);
            }

            scales[0] = maxScale;

            return scales;
        }

        private double[] GenerateOrientations(int numOfOrientations)
        {
            double[] orientations = new double[numOfOrientations];

            for (int i = 0; i < numOfOrientations; i++)
            {
                orientations[i] = i * Math.PI / 8;
            }

            return orientations;
        }

    }
}
