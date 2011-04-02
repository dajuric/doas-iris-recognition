using System;
using System.Drawing;
using AForge.Math;
using MLApp;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Matlab...");
            MLAppClass matlab = new MLAppClass();
          

            //*************test bool**************************
            Console.Write("Testing bool variable operations --> ");

            bool aBool = true;

            bool cBool;

            matlab.PutFullMatrix("mat1b", aBool);

            matlab.ExecuteWithErrorCheck("resb= not(mat1b);");
            cBool = matlab.GetFullMatrix<bool>("resb");

            Console.WriteLine("OK");
            //*************test int**************************
            Console.Write("Testing int variable operations --> ");

            int aInt = 2;

            matlab.PutFullMatrix("mat1i", aInt);

            matlab.ExecuteWithErrorCheck("resi=mat1i*4;");
            int cInt = matlab.GetFullMatrix<int>("resi");

            Console.WriteLine("OK");
            //*************test double**************************
            Console.Write("Testing double variable operations --> ");

            double aDouble = 2;

            double cDouble;

            matlab.PutFullMatrix("mat1d", aDouble);

            matlab.ExecuteWithErrorCheck("resd=mat1d*4;");
            cDouble = matlab.GetFullMatrix<double>("resd");

            Console.WriteLine("OK");
            Console.WriteLine();


            //*************test double[]**************************
            Console.Write("Testing double vector operations --> ");

            double[] aDoubleArr = new double[2];

            aDoubleArr[0] = 2;
            aDoubleArr[1] = 4;

            double[] bDoubleArr = aDoubleArr;

            matlab.PutFullMatrix("mat1d", aDoubleArr);
            matlab.PutFullMatrix("mat2d", bDoubleArr);

            matlab.ExecuteWithErrorCheck("resd=mat1d+mat2d;");
            double[,] cDoubleArr = matlab.GetFullMatrix<double[,]>("resd");

            Console.WriteLine("OK");
            Console.WriteLine();


            //*************test double[,]**************************
            Console.Write("Testing double matrix operations --> ");

            double[,] aDoubleMat = new double[2, 2];

            aDoubleMat[0, 0] = 2;
            aDoubleMat[1, 1] = 4;

            double[,] bDoubleMat = aDoubleMat;

            matlab.PutFullMatrix("mat1d", aDoubleMat);
            matlab.PutFullMatrix("mat2d", bDoubleMat);

            matlab.ExecuteWithErrorCheck("resd=mat1d*mat2d;");
            double[,] cDoubleMat = matlab.GetFullMatrix("resd");

            Console.WriteLine("OK");
            //*************test int[,]**************************
            Console.Write("Testing Int32 matrix operations --> ");

            int[,] aIntMatrix = new int[2, 2];

            aIntMatrix[0, 0] = 2;
            aIntMatrix[1, 1] = 4;
        
            matlab.PutFullMatrix("mat1i", aIntMatrix);

            matlab.ExecuteWithErrorCheck("resi=mat1i*4;");
            int[,] cIntMatrix = matlab.GetFullMatrix<int[,]>("resi");

            Console.WriteLine("OK");
            //**********test complex[,]***********************
            Console.Write("Testing Complex matrix operations --> ");

             Complex[,] aCmplx = new Complex[2,2];
             aCmplx[0,0].Re = 2; aCmplx[0,0].Im = 4;
             aCmplx[1,0].Re = 1; aCmplx[1,0].Im = -2; 

             matlab.PutFullMatrix("mat1c", aCmplx);
             matlab.ExecuteWithErrorCheck("resc=mat1c*45");

             Complex[,] cCmplx = matlab.GetFullMatrix<Complex[,]>("resc");

             Console.WriteLine("OK");
             Console.WriteLine();
             //********** General Image and Script Exec ******
             Console.Write("Testing image transfer and function execution --> ");

             matlab.ChangeDirectory(matlab.GetMyScriptFolder());

             GeneralImage gi = GeneralImage.FromBitmap((Bitmap)Image.FromFile(matlab.GetMyScriptFolder() + @"\test.bmp"));

             matlab.PutFullMatrix("imageTest", gi.Data);
             matlab.ExecuteWithErrorCheck("imageTest=uint8(imageTest)");
             matlab.ExecuteWithErrorCheck("Main(imageTest)");

            Console.WriteLine("OK. DONE");
            Console.ReadKey();
        }
    }
}
