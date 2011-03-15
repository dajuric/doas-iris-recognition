using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OCR.Common;
using OCR.ImageTools;

using OCR.GaborFilter;
using OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors;
using OCR.Classifier;

using Accord.Math;

namespace OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();  
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //double[,] a = new double[5000, 3528];

            //for (int i = 0; i < 1000; i++)
            //    for (int j = 0; j < 1000; j++)
            //        a[i, j] += i;
            
            //Bitmap img = (Bitmap)Image.FromFile(@"C:\a.bmp");
            Bitmap img = charDrawer1.SaveDrawing();

            ImagePreparer imPrep = new ImagePreparer();
            
            //GeneralImage gImage = imPrep.PrepareImage(img); 

            charDrawer2.Image = imPrep.PrepareImage(img).ToBitmap(true);
            //charDrawer2.Image = FilterBank.ToGeneralImages(fBank.Convolve(gImage), Gabor.ImagePart.Amplitude)[0].ToBitmap(true);

            charDrawer2.Image.Save(@"C:\b.bmp");

            RunClassifier runClassifier = new RunClassifier(@"C:\uzorci\configurationFile.xml");
            Console.WriteLine("\n\n ********* TRAINING ********");
            runClassifier.PerformTraining(200);
            Console.WriteLine("\n\n ********* TESTING ********");
            runClassifier.PerformTesting(100);

            runClassifier.Serialize(@"C:\classifier.clas");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            charDrawer1.Reset();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //FilterBank fb = new FilterBank(3, 3, 8, 10, 2 / 2.3f);

            //for (int scale = 0; scale < 3; scale++)
            //{
            //    for (int orientation = 0; orientation < 8; orientation++)
            //    {
            //        GeneralImage gi = Gabor.ToGeneralImage(fb.Filters[scale, orientation].KernelValues, Gabor.ImagePart.RealPart);
            //        gi.ToBitmap().Save("realPart_" + (scale + 1) + "_" + (orientation + 1) + ".bmp");
            //    }
            //}

            ClassifiersStatistics stat = new ClassifiersStatistics(3);
            stat.CalculateStatistics();
            Console.WriteLine(stat.ToString());
        }
    }
}
