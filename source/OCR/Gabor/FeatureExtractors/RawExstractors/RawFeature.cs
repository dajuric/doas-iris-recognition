using System;
using System.Collections.Generic;
using System.Drawing;
using OCR.Common;

namespace OCR.GaborFilter.FeatureExtractors.RawExtractors
{
    internal struct RawFeature : FeatureExtractor.FeatureVector.IFeature
    {
        //public static readonly int NUMBER_OF_ELEMENTS = ?; //ovisi o slici

        public GeneralImage Value;

        public RawFeature(GeneralImage value)
        {
            this.Value = value;
        }

        public double[] ToDoubleArray()
        {
            double[] array = new double[RawFeature.GetNumberOfElements(this.Value.Size)];

            int[,] data = this.Value.Data;

            int index=0;
            for (int row = 0; row < this.Value.Height; row++)
            {
                for (int col = 0; col < this.Value.Width; col++)
                {
                    array[index] = data[row, col] / (double)255;
                    index++;
                }
            }

            return array;
        }

        internal static int GetNumberOfElements(Size imageSize)
        {
            return imageSize.Width * imageSize.Height;
        }

    }
}
