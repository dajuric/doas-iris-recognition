using System;
using System.Collections.Generic;
using System.Drawing;

namespace OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors.SumOfEnergyExtractors
{
    internal struct SimpleFeature : FeatureExtractor.FeatureVector.IFeature
    {
        public static readonly int NUMBER_OF_ELEMENTS = 1;

        public double Value;

        public SimpleFeature(double value)
        {
            this.Value = value;
        }

        public double[] ToDoubleArray()
        {
            double[] array = new double[NUMBER_OF_ELEMENTS];
            array[0] = Value / 255;
            return array;
        }
    }
}
