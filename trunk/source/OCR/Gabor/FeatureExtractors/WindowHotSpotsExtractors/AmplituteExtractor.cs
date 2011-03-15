using System;
using OCR.Common;
using System.Drawing;
using AForge.Math;
using System.Collections.Generic;

namespace OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors
{
    public class AmplituteExtractor:FeatureExtractor
    {
        private EnergySpots eSpotsExtractor;

        public AmplituteExtractor(FilterBank filterBank, GeneralImage image)
            :base(filterBank, image)
        {
            eSpotsExtractor = new EnergySpots(filterBank, image);        
        }

        /// <summary>
        /// Izračunava amplitudu svih visoko-energetskih točaka. Struktura vektora: (i-ti)VektorSlike + (j-ti)VektorSlike + ... ; (i-ti)VektorSlike ={x, y, amplituda; ...}
        /// </summary>
        /// <returns>Vektor značajki</returns>
        public override FeatureVector GetFeatures()
        {
            return eSpotsExtractor.GetHotSpots(); //svejedno za vrijednosti ima amplitude (ono što nama treba) pa ne trebamo ponovno računati kao kod faze
        }

        public static new int GetFeatureVectorLength(FilterBank filterBank, Size imageSize)
        { return EnergySpots.GetTotalNumberOfHotSpots(filterBank, imageSize); }

    }
}
