using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using System.Collections.Generic;

namespace OCR.GaborFilter.FeatureExtractors.RawExtractors
{
    class RawImaginaryPart:FeatureExtractors.FeatureExtractor
    {
        private List<GeneralImage> phaseImages;

        public RawImaginaryPart(FilterBank filterBank, GeneralImage image)
            :base(filterBank, image)
        {
            phaseImages = new List<GeneralImage>();

            List<Complex[,]> cmplxImages = filterBank.Convolve(image);
            phaseImages = FilterBank.ToGeneralImages(cmplxImages, Gabor.ImagePart.ImagPart);             
        }

        public override FeatureVector GetFeatures()
        {
            FeatureExtractor.FeatureVector featureVector = new FeatureExtractor.FeatureVector(RawFeature.GetNumberOfElements(image.Size));

            foreach (GeneralImage img in phaseImages)
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
