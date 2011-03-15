using System.Collections.Generic;
using System.Drawing;
using AForge.Math;
using OCR.Common;

namespace OCR.GaborFilter.FeatureExtractors
{
    public abstract class FeatureExtractor
    {
        protected GeneralImage image;
        protected FilterBank filterBank;

        public class FeatureVector
        {
            internal interface IFeature
            {
                //public static readonly int NUMBER_OF_ELEMENTS = -1; //vrijednost je specifična za svaku klasu koja implementira ovu

                double[] ToDoubleArray();
            }

            private int numberOfFeatureElements=-1;

            internal List<IFeature> Features = new List<IFeature>();

            internal FeatureVector(int numberOfFeatureElements)
            {
                this.numberOfFeatureElements = numberOfFeatureElements;
            }

            internal void Add(IFeature f)
            {
                Features.Add(f);
            }

            public void Concat(FeatureVector fVector)
            {
                Features.AddRange(fVector.Features);
            }

            public double[] ToDoubleArray()
            {
               double[] featuresArray = new double[numberOfFeatureElements * Features.Count];

               int indexInArray = 0;
                foreach (IFeature f in this.Features)
                {
                    f.ToDoubleArray().CopyTo(featuresArray, indexInArray);
                    indexInArray+=numberOfFeatureElements;
                }

                return featuresArray;
            }

            public void CopyToDoubleArray(ref double[,] array, int rowIndex)
            {
                int arrayOffset = array.GetLength(1) * sizeof(double) * rowIndex;
                int numOfBytesToCopy = numberOfFeatureElements * sizeof(double);
                
                foreach (IFeature f in this.Features)
                {
                    double[] feature = f.ToDoubleArray();

                    System.Buffer.BlockCopy(feature, 0, array, arrayOffset, numOfBytesToCopy);

                    arrayOffset += numOfBytesToCopy;
                }
            }
        }

        public FeatureExtractor(FilterBank filterBank, GeneralImage image)
        {
            this.filterBank = filterBank;
            this.image = image;
        }

        public abstract FeatureVector GetFeatures();

        public static int GetFeatureVectorLength(FilterBank filterBank, Size imageSize)
        { return -1; } //svaka klasa koja implementira ovu vrača svoj podatak
    }
}
