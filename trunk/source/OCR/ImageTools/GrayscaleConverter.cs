using System;
using System.Drawing;
using System.Runtime.InteropServices;
using OCR.Common;

namespace OCR.ImageTools
{
    public class GrayscaleConverter
    {
        public enum GrayScaleExtractionMethod
        {
            VFromHSV,
            YFromYCbCr,
            LFromLab
        }
        
        private enum ARGBIndex : int
        {
            A = 3,
            R = 2,
            G = 1,
            B = 0
        }

        private GrayScaleExtractionMethod method;

        public GrayscaleConverter(GrayScaleExtractionMethod method)
        {
            this.method = method;
        }

        private byte CalculateGrayValue(byte red, byte green, byte blue)
        {
            byte result = 0;
            switch (this.method)
            {
                case GrayScaleExtractionMethod.YFromYCbCr:
                    result = (byte)((double)red * 0.299 + (double)green * 0.587 + (double)blue * 0.114); // BT.601 standard: http://en.wikipedia.org/wiki/YCbCr#ITU-R_BT.601_conversion
                    break;
                case GrayScaleExtractionMethod.VFromHSV:
                    result = (red > green ? (red > blue ? red : blue) : (green > blue ? green : blue)); // 'hexcone' model: http://en.wikipedia.org/wiki/HSL_and_HSV#Lightness
                    break;
                case GrayScaleExtractionMethod.LFromLab:
                    double Y = 1 / 0.17697 * ((double)red * 0.17697 + (double)green * 0.81240 + (double)blue * 0.01063); // Y from XYZ: http://en.wikipedia.org/wiki/XYZ_color_space#Construction_of_the_CIE_XYZ_color_space_from_the_Wright.E2.80.93Guild_data
                    result = (byte)(116 * CieFunc(Y / 1440.9222) - 16); // L from Cie L*a*b*: http://en.wikipedia.org/wiki/Lab_color_space#CIE_XYZ_to_CIE_L.2Aa.2Ab.2A_.28CIELAB.29_and_CIELAB_to_CIE_XYZ_conversions
                    break;
            }
            return result;
        }

        /// <summary>
        /// Definicija CIE funkcije: http://en.wikipedia.org/wiki/Lab_color_space#CIE_XYZ_to_CIE_L.2Aa.2Ab.2A_.28CIELAB.29_and_CIELAB_to_CIE_XYZ_conversions
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private double CieFunc(double t)
        {
            if (t > Math.Pow(6.0 / 29.0, 3))
            {
                return Math.Pow(t, 1.0 / 3.0);
            }
            else
                return Math.Pow(29.0 / 6.0, 2) / 3.0 * t + 4.0 / 29.0;
        }

        public unsafe GeneralImage Convert(GeneralImage source)
        {
            GeneralImage dest = new GeneralImage(source.Width, source.Height);

            GCHandle hSource = GCHandle.Alloc(source.Data, System.Runtime.InteropServices.GCHandleType.Pinned);
            GCHandle hDest = GCHandle.Alloc(dest.Data, System.Runtime.InteropServices.GCHandleType.Pinned);

            byte* bmpAdressSource = (byte*)hSource.AddrOfPinnedObject();
            int* bmpAdressDest = (int*)hDest.AddrOfPinnedObject();

            int numOfPixels = source.Width * source.Height;

            int i = 0;
            while (i < numOfPixels /*- (Matrix32.BYTE_PER_PIXEL-1)*/) //petlja gleda još unaprijed za 3 zbog ARGB gdje je B=0
            {
                (*bmpAdressDest) = CalculateGrayValue(bmpAdressSource[(int)ARGBIndex.R], bmpAdressSource[(int)ARGBIndex.G], bmpAdressSource[(int)ARGBIndex.B]);

                //sljedeći piksel
                bmpAdressSource += sizeof(Int32);
                bmpAdressDest++;

                i++;
            }

            hSource.Free();
            hDest.Free();

            return dest;
        }

        public GeneralImage Convert(Bitmap source)
        {
            GeneralImage gI = GeneralImage.FromBitmap(source);
            return Convert(gI);
        }

    }
}

