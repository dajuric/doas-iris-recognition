using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OCR.Common
{
    public class GeneralImage: IEnumerable, ICloneable
    {        
        public int[,] Data; //polje koje sadržava sliku [indeksRetka, indeksStupca] (NE (x,y) !!!

        public GeneralImage(int width, int height)
        {
            this.Data = new int[height, width];
        }

        public GeneralImage(double[,] array)
        {
            if (array == null)
                throw new ArgumentNullException();

            this.Data = new int[array.GetLength(0), array.GetLength(1)];

            for (int i = 0; i < this.Data.GetLength(0); i++)
            {
                for (int j = 0; j < this.Data.GetLength(1); j++)
                {
                    this.Data[i, j] = (byte)array[i, j];
                }
            }
        }

        public GeneralImage(int[,] array)
        {
            if (array == null)
                throw new ArgumentNullException();

            this.Data = array;
        }

        public GeneralImage(byte[,] array)
        {
            if (array == null)
                throw new ArgumentNullException();

            this.Data = new int[array.GetLength(0), array.GetLength(1)];

            for (int i = 0; i < this.Data.GetLength(0); i++)
            {
                for (int j = 0; j < this.Data.GetLength(1); j++)
                {
                    this.Data[i, j] = array[i, j];
                }
            }
        }

        /// <summary>
        /// Copies amount of memory to another location
        /// </summary>
        /// <param name="sourcePtr">Source pointer</param>
        /// <param name="destPtr">Destination pointer</param>
        /// <param name="numOfBytes">Number of bytes to overwrite</param>
        /// <exception cref="IncorrectNumberOfBytes"></exception>
        private static unsafe void MemoryCopy(IntPtr sourcePtr, IntPtr destPtr, Int32 numOfBytes)
        {
            //if (numOfBytes <= 0)
            //    throw new IncorrectNumberOfBytes();

            System.Threading.Thread.BeginCriticalRegion();

            //numofBytes % sizeof(Int32); sizeof(Int32)==4
            int numOfAllignBytes = numOfBytes & 0x3;
            numOfBytes = numOfBytes - numOfAllignBytes;

            byte* srcPtrByte = (byte*)sourcePtr;
            byte* dstPtrByte = (byte*)destPtr;
            while (numOfAllignBytes != 0)
            {
                (*dstPtrByte) = (*srcPtrByte);
                srcPtrByte++;
                dstPtrByte++;
                numOfAllignBytes -= sizeof(byte);
            }
            
            Int32* srcPtrInt = (Int32*)srcPtrByte;
            Int32* dstPtrInt = (Int32*)dstPtrByte;
            while (numOfBytes != 0)
            {
                (*dstPtrInt) = (*srcPtrInt);
                srcPtrInt++;
                dstPtrInt++;
                numOfBytes -= sizeof(Int32);
            }

            System.Threading.Thread.EndCriticalRegion();
        }

        public unsafe static GeneralImage FromBitmap(Bitmap managedBitmap)
        {
            GeneralImage gi = null;
            
            switch (managedBitmap.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    gi = FromBmp1(managedBitmap);
                    break;
                case PixelFormat.Format8bppIndexed:
                    gi = FromBmp8(managedBitmap);
                    break;
                case PixelFormat.Format24bppRgb:
                    gi = FromBmp24(managedBitmap);
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    gi = FromBmp32(managedBitmap);
                    break;
                default:
                    throw new Exception("Unsupported pixel format: " + managedBitmap.PixelFormat.ToString());
            }

            return gi;
        }

        private unsafe static GeneralImage FromBmp1(Bitmap managedBitmap)
        {
            Bitmap bmpNew = new Bitmap(managedBitmap.Width, managedBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bmpNew);
            g.DrawImageUnscaled(managedBitmap, Point.Empty);
            g.Dispose();

            GeneralImage gi = FromBmp32(bmpNew);
            int[,] data = gi.Data;

            for (int row = 0; row < bmpNew.Height; row++)
            {
                for (int col = 0; col < bmpNew.Width; col++)
                {
                    if (data[row, col] == -1) //bijela
                        data[row, col] = 255;
                    else                      //crna
                        data[row, col] = 0;
                }
            }

            return gi;
        }

        private unsafe static GeneralImage FromBmp8(Bitmap managedBitmap)
        {
            int[,] bmpData = new int[managedBitmap.Height, managedBitmap.Width];

            BitmapData lockedBmp = managedBitmap.LockBits(new Rectangle(Point.Empty, managedBitmap.Size), ImageLockMode.ReadWrite, managedBitmap.PixelFormat);
            byte* sourcePtr = (byte*)lockedBmp.Scan0;

            GCHandle hDest = GCHandle.Alloc(bmpData, System.Runtime.InteropServices.GCHandleType.Pinned);
            int* destPtr = (int*)hDest.AddrOfPinnedObject();

            int destWidth = bmpData.GetLength(1);
            int destHeight = bmpData.GetLength(0);

            for (int row = 0; row < destHeight; row++)
            {
                sourcePtr = (byte*)lockedBmp.Scan0 + row * lockedBmp.Stride;              

                for (int col = 0; col < destWidth; col++)
                {
                    (*destPtr) = (*sourcePtr);

                    sourcePtr++; //za 1
                    destPtr++; //za 4                  
                }
            }

            managedBitmap.UnlockBits(lockedBmp);
            hDest.Free();

            return new GeneralImage(bmpData);
        }

        private unsafe static GeneralImage FromBmp24(Bitmap managedBitmap)
        {
            int[,] bmpData = new int[managedBitmap.Height, managedBitmap.Width];

            BitmapData lockedBmp = managedBitmap.LockBits(new Rectangle(Point.Empty, managedBitmap.Size), ImageLockMode.ReadWrite, managedBitmap.PixelFormat);
            byte* sourcePtr = (byte*)lockedBmp.Scan0;

            GCHandle hDest = GCHandle.Alloc(bmpData, System.Runtime.InteropServices.GCHandleType.Pinned);
            byte* destPtr = (byte*)hDest.AddrOfPinnedObject();

            int destWidth = bmpData.GetLength(1);
            int destHeight = bmpData.GetLength(0);

            for (int row = 0; row < destHeight; row++)
            {
                sourcePtr = (byte*)lockedBmp.Scan0 + row * lockedBmp.Stride;

                for (int col = 0; col < destWidth; col++)
                {
                    (*destPtr) = (*sourcePtr); //B
                    destPtr++; sourcePtr++; 

                    (*destPtr) = (*sourcePtr); //G
                    destPtr++; sourcePtr++;

                    (*destPtr) = (*sourcePtr); //R


                    destPtr++; destPtr++;
                    sourcePtr++;             //pomakni destPrt za 4, a sourcePtr za 3
                }
            }

            managedBitmap.UnlockBits(lockedBmp);
            hDest.Free();

            return new GeneralImage(bmpData);
        }

        private unsafe static GeneralImage FromBmp32(Bitmap managedBitmap)
        {
            int[,] bmpData = new int[managedBitmap.Height, managedBitmap.Width];

            BitmapData lockedBmp = managedBitmap.LockBits(new Rectangle(Point.Empty, managedBitmap.Size), ImageLockMode.ReadWrite, managedBitmap.PixelFormat);
            int* sourcePtr = (int*)lockedBmp.Scan0;

            GCHandle hDest = GCHandle.Alloc(bmpData, System.Runtime.InteropServices.GCHandleType.Pinned);
            int* destPtr = (int*)hDest.AddrOfPinnedObject();

            int destWidth = bmpData.GetLength(1);
            int destHeight = bmpData.GetLength(0);

            int sourceStrideAsNumOfPixels = lockedBmp.Stride / sizeof(int);
            for (int row = 0; row < destHeight; row++)
            {
                GeneralImage.MemoryCopy((IntPtr)sourcePtr, (IntPtr)destPtr, destWidth*sizeof(int));
                sourcePtr += sourceStrideAsNumOfPixels;
                destPtr += destWidth;
            }

            managedBitmap.UnlockBits(lockedBmp);

            return new GeneralImage(bmpData);
        }

        /// <summary>
        /// Creates a new 8bpp bitmap
        /// </summary>
        public virtual Bitmap ToBitmap()
        {
            return ToBitmap(true);
        }

        public virtual Bitmap ToBitmap(bool convertTo8bpp)
        {
            if (convertTo8bpp)
                return ToBpp8();
            else
                return ToBpp32();
        }

        private unsafe Bitmap ToBpp8()
        {
            GCHandle hSource = GCHandle.Alloc(this.Data, System.Runtime.InteropServices.GCHandleType.Pinned);
            int* sourcePtr = (int*)hSource.AddrOfPinnedObject();

            Bitmap managedBmp = new Bitmap(this.Width, this.Height, PixelFormat.Format8bppIndexed);
            BitmapData lockedBmp = managedBmp.LockBits(new Rectangle(Point.Empty, managedBmp.Size), ImageLockMode.ReadWrite, managedBmp.PixelFormat);
            byte* destPtr = (byte*)lockedBmp.Scan0;

            for (int row = 0; row < managedBmp.Height; row++)
            {
                destPtr = (byte*)lockedBmp.Scan0 + row * lockedBmp.Stride;

                for (int col = 0; col < managedBmp.Width; col++)
                {
                    (*destPtr) = (byte)(*sourcePtr);

                    sourcePtr++; //za 1
                    destPtr++; //za 4                  
                }
            }

            //promijeni paletu boja u tonove sive...
            ColorPalette pal = managedBmp.Palette;
            for (int i = 0; i < pal.Entries.Length; i++)
            {
                pal.Entries[i] = Color.FromArgb(i, i, i);
            }

            managedBmp.Palette = pal;
            managedBmp.UnlockBits(lockedBmp);
            hSource.Free();

            return managedBmp;
        }

        private unsafe Bitmap ToBpp32()
        {
            GCHandle hSource = GCHandle.Alloc(this.Data, System.Runtime.InteropServices.GCHandleType.Pinned);
            int* sourcePtr = (int*)hSource.AddrOfPinnedObject();

            Bitmap managedBmp = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppRgb);
            BitmapData lockedBmp = managedBmp.LockBits(new Rectangle(Point.Empty, managedBmp.Size), ImageLockMode.ReadWrite, managedBmp.PixelFormat);
            int* destPtr = (int*)lockedBmp.Scan0;

            int destStrideAsNumOfPixels = lockedBmp.Stride / sizeof(int);
            for (int row = 0; row < this.Height; row++)
            {
                GeneralImage.MemoryCopy((IntPtr)sourcePtr, (IntPtr)destPtr, this.Width * sizeof(int));
                sourcePtr += this.Width;
                destPtr += destStrideAsNumOfPixels;
            }

            ////promijeni paletu boja u tonove sive...
            //ColorPalette pal = managedBmp.Palette;
            //for (int i = 0; i < pal.Entries.Length; i++)
            //{
            //    pal.Entries[i] = Color.FromArgb(i, i, i);
            //}

            //managedBmp.Palette = pal;
            managedBmp.UnlockBits(lockedBmp);

            hSource.Free();

            return managedBmp;
        }

        public object Clone()
        {
            int[,] newArray = (int[,])this.Data.Clone();
            return new GeneralImage(newArray);
        }

        public int Width
        {
            get
            {
                return this.Data.GetUpperBound(1) + 1;
            }
        }

        public int Height
        {
            get
            {
                return this.Data.GetUpperBound(0) + 1;
            }
        }

        public Size Size
        {
            get { return new Size(this.Width, this.Height); }
        }

        /// <summary>
        /// Gets or sets a pixel value
        /// </summary>
        public int this[int row, int column]
        {
            get
            {
                return Data[row, column];
            }
            set
            {
                Data[row, column] = value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (int row = 0; row < this.Height; row++)
            { 
                for (int col = 0; col < this.Width; col++)
                {
                    yield return this.Data[row, col];
                }
            }
        }


        /// <summary>
        /// izracuna apsolutni histogram slike
        /// </summary>
        /// <returns>apsolutni histogram slike</returns>
        public int[] CalculateHistogram()
        {
            int[] histogram = new int[256];

            for (int row = 0; row < this.Height; row++)
            {
                for (int col = 0; col < this.Width; col++)
                {
                    int pixel = this.Data[row, col];
                    histogram[pixel]++;
                }
            }
            
            return histogram;
        }

        /// <summary>
        /// izracuna normalizirani histogram slike
        /// </summary>
        /// <returns>normalizirani histogram slike</returns>
        public double[] CalculateNormalizedHistogram()
        {
            int[] histogram = CalculateHistogram();
            double[] normHist = new double[256];

            for (int i = 0; i < histogram.Length; i++)
            {
                normHist[i] = histogram[i] / this.Data.Length;
            }
            
            return normHist;
        }

    }
}
