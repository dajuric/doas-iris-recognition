using System;
using OCR.Common;
using System.Collections.Generic;
using System.Drawing;

namespace OCR.ImageTools
{
    class ImagePreparer
    {
        public const int IMAGE_SIZE = 22 + 2 * IMAGE_RESERVED_BORDER;
        private const int IMAGE_RESERVED_BORDER = 3;

        private const int LOWER_COLOR_TRESHOLD = 20;
       

        private GeneralImage TransformImage(GeneralImage sourceGi, Size newGiSize, Rectangle sourceRect, Rectangle destRect)
        {
            Bitmap sourceBmp = sourceGi.ToBitmap(false);
            Bitmap newBmp = new Bitmap(newGiSize.Width, newGiSize.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb); //ne 32bpp jer u A polje piše 255
                
            Graphics g = Graphics.FromImage(newBmp);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //g.FillRectangle(new SolidBrush(Color.Black), 0, 0, newBmp.Width, newBmp.Height); //svejedno će biti crno (nule)
            g.DrawImage(sourceBmp, destRect, sourceRect, GraphicsUnit.Pixel);
            g.Dispose();

            return GeneralImage.FromBitmap(newBmp);
        }

        public Rectangle GetBoundingBox(GeneralImage grayscaleImage, byte lowerColorTreshold)
        {
            int minX = grayscaleImage.Width - 1;
            int maxX = 0;

            int minY = grayscaleImage.Height - 1;
            int maxY = 0;
                  
            for (int j = 0; j < grayscaleImage.Height; j++)        
            {
                for (int i = 0; i < grayscaleImage.Width; i++)
                {
                    if (grayscaleImage[j, i] > lowerColorTreshold) 
                    {
                        if (i < minX) minX = i;
                        else if (i > maxX) maxX = i;

                        if (j < minY) minY = j;
                        else if (j > maxY) maxY = j;
                    }
                }
            }
          
            return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1); //+1 zbog 0
        }

        public Point FindCenterOfMass(GeneralImage grayscaleImage, byte lowerColorTreshold)
        {
            double xMass = 0;
            double yMass = 0;

            int sumOfPixels = 0;

            for (int j = 0; j < grayscaleImage.Height; j++)
            {
                for (int i = 0; i < grayscaleImage.Width; i++)
                {
                    int pxColor = grayscaleImage[j, i]; //boja za crtanje je bijela => 255

                    if (pxColor > lowerColorTreshold)
                    {
                        sumOfPixels += pxColor;

                        xMass += pxColor * i;
                        yMass += pxColor * j;
                    }
                }
            }

            if (sumOfPixels == 0) //ako nije ništa iscrtano
                return Point.Empty;
            else
                return new Point((int)Math.Round(xMass / sumOfPixels), (int)Math.Round(yMass / sumOfPixels));
        }

        public GeneralImage PrepareImage(GeneralImage gi)
        {
            //promijeni veličinu slike
            Rectangle boundingBox = GetBoundingBox(gi, LOWER_COLOR_TRESHOLD);
           
            int imgClientSize = IMAGE_SIZE - 2 * IMAGE_RESERVED_BORDER;

            double resizeFactor;

            if (boundingBox.Width > boundingBox.Height) //uvijek povećaj/smanji za veću veličinu
               resizeFactor = (double)imgClientSize / boundingBox.Width;
            else
               resizeFactor = (double)imgClientSize / boundingBox.Height;

            int newWidth = (int)Math.Round(boundingBox.Width * resizeFactor);
            int newHeight = (int)Math.Round(boundingBox.Height * resizeFactor);
          
            
            GeneralImage newGi = TransformImage(gi, new Size(imgClientSize, imgClientSize), 
                                          boundingBox, new Rectangle(0, 0, newWidth, newHeight));

            //centriraj sliku
            Point centerOfMass = FindCenterOfMass(newGi, LOWER_COLOR_TRESHOLD);
            Point newStartPoint = new Point(imgClientSize / 2 - centerOfMass.X, imgClientSize / 2 - centerOfMass.Y);

            //if (newStartPoint.X > imgClientSize - boundingBox.Width) newStartPoint.X = imgClientSize - boundingBox.Width;
            //if (newStartPoint.Y > imgClientSize - boundingBox.Height) newStartPoint.Y = imgClientSize - boundingBox.Height;


            //if (newStartPoint.X < 0 || newStartPoint.Y < 0)
            //    newStartPoint = Point.Empty;
          
            newGi = TransformImage(newGi, newGi.Size, new Rectangle(Point.Empty, newGi.Size), new Rectangle(newStartPoint, newGi.Size));

            //povečaj sliku za rub i centriraj je
            newGi = TransformImage(newGi, new Size(IMAGE_SIZE, IMAGE_SIZE),
                                         new Rectangle(0, 0, newGi.Width, newGi.Height), 
                                         new Rectangle(IMAGE_RESERVED_BORDER, IMAGE_RESERVED_BORDER, newGi.Width, newGi.Height));

            return newGi;
        }

 
        public GeneralImage PrepareImage(Bitmap bmp)
        {
            GeneralImage gi;
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
                GrayscaleConverter gConv = new GrayscaleConverter(GrayscaleConverter.GrayScaleExtractionMethod.YFromYCbCr);
                gi = gConv.Convert(bmp);
            }
            else
                gi = GeneralImage.FromBitmap(bmp);

            return PrepareImage(gi);
        }

    }
}
