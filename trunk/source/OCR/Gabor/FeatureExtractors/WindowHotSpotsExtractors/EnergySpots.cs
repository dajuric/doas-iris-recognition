using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using System.Collections.Generic;

namespace OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors
{
    class EnergySpots
    {
        private GeneralImage image;
        private FilterBank filterBank;
        internal List<Complex[,]> cmplxImages; //čuvaju se konvoluirane slike
        
        private const int WINDOW_SIZE = 4;

        private List<GeneralImage> amplitudeImages;

        public EnergySpots(FilterBank filterBank, GeneralImage image)
        {
            this.image = image;
            this.filterBank = filterBank;
            
            amplitudeImages = new List<GeneralImage>();
            
            this.cmplxImages = filterBank.Convolve(image);
            amplitudeImages = FilterBank.ToGeneralImages(cmplxImages, Gabor.ImagePart.Amplitude);          
        }

        /// <summary>
        /// Nalazi visokoenergetske točke na slikama
        /// </summary>
        /// <returns>Visoko-energetske točke pojedine slike</returns>
        public FeatureExtractor.FeatureVector GetHotSpots()
        {
            FeatureExtractor.FeatureVector featureVector = new FeatureExtractor.FeatureVector(HotSpotsFeature.NUMBER_OF_ELEMENTS);

            foreach (GeneralImage img in amplitudeImages)
            {
                FeatureExtractor.FeatureVector maximumsInImage = FindMaximumsInImage(img);
                featureVector.Concat(maximumsInImage);
            }

            return featureVector;
        }

        internal int TotalNumberOfHotSpots
        {
            get { return GetTotalNumberOfHotSpots(this.filterBank, image.Size); }
        }

        internal static int GetTotalNumberOfHotSpots(FilterBank filterBank, Size imageSize)
        {
            int numOfHorizontalWindows = (int)Math.Ceiling((double)imageSize.Width / WINDOW_SIZE);
            int numOfVerticalWindows = (int)Math.Ceiling((double)imageSize.Height / WINDOW_SIZE);

            return filterBank.NumberOfFilters * HotSpotsFeature.NUMBER_OF_ELEMENTS * numOfHorizontalWindows * numOfVerticalWindows;
        }

        private FeatureExtractor.FeatureVector FindMaximumsInImage(GeneralImage amplitudeImage)
        {
            FeatureExtractor.FeatureVector fVector = new FeatureExtractor.FeatureVector(HotSpotsFeature.NUMBER_OF_ELEMENTS);

            int max;
            Point maxLocation;

            for (int y = 0; y < amplitudeImage.Height; y += WINDOW_SIZE)
            {
                for (int x = 0; x < amplitudeImage.Width; x += WINDOW_SIZE)
                {
                    FindMaximumInWindow(new Point(x, y), amplitudeImage, out max, out maxLocation);

                    fVector.Add(new HotSpotsFeature(maxLocation, max));
                }
            }

            return fVector;
        }

        private void FindMaximumInWindow(Point windowLocation, GeneralImage amplitudeImage, out int max, out Point maxLocation)
        {
            max = 0;
            maxLocation = new Point();

            int maxWindowY = ((windowLocation.Y + WINDOW_SIZE) <= amplitudeImage.Height) ? (windowLocation.Y + WINDOW_SIZE) : amplitudeImage.Height;
            int maxWindowX = ((windowLocation.X + WINDOW_SIZE) <= amplitudeImage.Width) ? (windowLocation.X + WINDOW_SIZE) : amplitudeImage.Width;

            for (int y = windowLocation.Y; y < maxWindowY; y++) //strogo manje jer npr. ako je prozor veličine 1 treba samo ući jednom u petlju
            {
                for (int x = windowLocation.X; x < maxWindowX; x++)
                {
                    int pixel = amplitudeImage[y, x];
                    if (pixel > max)
                    {
                        max = pixel;
                        maxLocation = new Point(x, y);
                    }
                }
            }
        }
   
    }
}
