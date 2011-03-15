using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using System.Collections.Generic;

namespace OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors.SumOfEnergyExtractors
{
    class SimpleAmplitude:FeatureExtractors.FeatureExtractor
    {
        private List<GeneralImage> amplitudeImages;

        public SimpleAmplitude(FilterBank filterBank, GeneralImage image)
            :base(filterBank, image)
        {
            amplitudeImages = new List<GeneralImage>();

            List<Complex[,]> cmplxImages = filterBank.Convolve(image);
            amplitudeImages = FilterBank.ToGeneralImages(cmplxImages, Gabor.ImagePart.Amplitude);             
        }

        public override FeatureVector GetFeatures()
        {
            FeatureExtractor.FeatureVector featureVector = new FeatureExtractor.FeatureVector(SimpleFeature.NUMBER_OF_ELEMENTS);

            foreach (GeneralImage img in amplitudeImages)
            {
                SimpleFeature sumOfEnergy = this.GetSNormalizedSumOfPixels();
                featureVector.Add(sumOfEnergy);
            }

            return featureVector;
        }

        private SimpleFeature GetSNormalizedSumOfPixels()
        {
            int[,] data = this.image.Data;
            int sumOfPixels = 0;
            
            for (int row = 0; row < this.image.Height; row++)
            {
                for (int col = 0; col < this.image.Width; col++)
                {
                    sumOfPixels += data[row, col];
                }
            }

            int imageSize = image.Width * image.Height;
            return new SimpleFeature(sumOfPixels / (double)imageSize);
        }

        public static new int GetFeatureVectorLength(FilterBank filterBank, Size imageSize)
        {
            return filterBank.NumberOfFilters; 
        }

    }
}
