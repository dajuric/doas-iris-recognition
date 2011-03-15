using System;
using System.Collections.Generic;
using System.Drawing;

namespace OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors
{
    internal struct HotSpotsFeature:FeatureExtractor.FeatureVector.IFeature
    {
        public static readonly int NUMBER_OF_ELEMENTS = 3;

        public Point Point;
        public int Value;

        public HotSpotsFeature(Point point, int value)
        {
            this.Point = point;
            this.Value = value;
        }

        public double[] ToDoubleArray()
        {
            double[] array = new double[NUMBER_OF_ELEMENTS];
            array[0] = Point.X /(double)255; array[1] = Point.Y /(double)255; array[2] = Value / (double)255;
            return array;
        }
    }
}
