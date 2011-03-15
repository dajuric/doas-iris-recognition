using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using System.Collections.Generic;

namespace OCR.GaborFilter.FeatureExtractors.RawExtractors
{
    class RawAmplitude:FeatureExtractors.FeatureExtractor
    {
        private List<GeneralImage> amplitudeImages;

        public RawAmplitude(FilterBank filterBank, GeneralImage image)
            :base(filterBank, image)
        {
            amplitudeImages = new List<GeneralImage>();

            List<Complex[,]> cmplxImages = filterBank.Convolve(image);
            amplitudeImages = FilterBank.ToGeneralImages(cmplxImages, Gabor.ImagePart.Amplitude);             
        }

        public override FeatureVector GetFeatures()
        {
            FeatureExtractor.FeatureVector featureVector = new FeatureExtractor.FeatureVector(RawFeature.GetNumberOfElements(image.Size));

            foreach (GeneralImage img in amplitudeImages)
            {
                RawFeature wholeImage = new RawFeature(img);
                featureVector.Add(wholeImage);
            }

            return featureVector;
        }

        public static new int GetFeatureVectorLength(FilterBank filterBank, Size imageSize)
        {
            return filterBank.NumberOfFilters * RawFeature.GetNumberOfElements(imageSize);
        }

    }
}
