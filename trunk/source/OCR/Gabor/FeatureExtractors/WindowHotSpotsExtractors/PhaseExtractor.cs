using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using System.Collections.Generic;

namespace OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors
{
    class PhaseExtractor:FeatureExtractor
    {
        private List<GeneralImage> phaseImages;
        private EnergySpots eSpotsExtractor;

        public PhaseExtractor(FilterBank filterBank, GeneralImage image)
            :base(filterBank, image)
        {
            phaseImages = new List<GeneralImage>();
            eSpotsExtractor = new EnergySpots(filterBank, image);

            List<Complex[,]> cmplxImages = eSpotsExtractor.cmplxImages;
            phaseImages = FilterBank.ToGeneralImages(cmplxImages, Gabor.ImagePart.Phase);          
        }

        /// <summary>
        /// Izračunava amplitudu svih visoko-energetskih točaka. Struktura vektora: (i-ti)VektorSlike + (j-ti)VektorSlike + ... ; (i-ti)VektorSlike ={x, y, faza; ...}
        /// </summary>
        /// <returns>Vektor značajki</returns>
        public override FeatureVector GetFeatures()
        {
            FeatureVector hotSpots = eSpotsExtractor.GetHotSpots();

            int vectorLengthPerImage = eSpotsExtractor.TotalNumberOfHotSpots / (filterBank.NumberOfFilters * HotSpotsFeature.NUMBER_OF_ELEMENTS);
            int phaseImageIndex = 0;

            FeatureVector featureVector = new FeatureVector(HotSpotsFeature.NUMBER_OF_ELEMENTS);

            for (int startPos = 0; startPos < (eSpotsExtractor.TotalNumberOfHotSpots / HotSpotsFeature.NUMBER_OF_ELEMENTS); startPos += vectorLengthPerImage)
            {
                //pretpostavka je da su vektori isto složeni svaki put
                List<FeatureVector.IFeature> lstHotSpotsPerImage = hotSpots.Features.GetRange(startPos, vectorLengthPerImage);

                GeneralImage phaseImage = phaseImages[phaseImageIndex];
                phaseImageIndex++;

                foreach (HotSpotsFeature f in lstHotSpotsPerImage)
                {
                    featureVector.Add(new HotSpotsFeature(f.Point, phaseImage[f.Point.Y, f.Point.X])); //za svaku sliku (hotSpot) umjesto amplitude zapiši fazu
                }
            }

            return featureVector;
        }

        public static new int GetFeatureVectorLength(FilterBank filterBank, Size imageSize)
        { return EnergySpots.GetTotalNumberOfHotSpots(filterBank, imageSize); }
  
    }
}
