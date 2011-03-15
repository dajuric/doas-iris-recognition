using System;
using System.Collections.Generic;
using OCR.Common;
using OCR.GaborFilter;
using OCR.GaborFilter.FeatureExtractors;
using OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors;
using OCR.GaborFilter.FeatureExtractors.WindowHotSpotsExtractors.SumOfEnergyExtractors;
using OCR.GaborFilter.FeatureExtractors.RawExtractors;
using OCR.ImageTools;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Accord.Statistics.Analysis;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;

namespace OCR.Classifier
{
    public class RunClassifier
    {

        MulticlassSupportVectorMachine kSVM = null;

        
        public RunClassifier(string xmlConfigurationFilename)
        {
            this.XmlFilename = xmlConfigurationFilename;
        }

        public string XmlFilename
        {
            get;
            private set;
        }

        public void PerformTraining(int maxNumberOfFilesPerClass)
        {
            ImageLoader imgLoader = new ImageLoader(this.XmlFilename, ImageLoader.DataType.TrainExamples, maxNumberOfFilesPerClass);
          
            FilterBank fBank = new FilterBank(3, 2, 3, 20, 2 / 1f);

            double[][] featureVectorsMatrix = new double[imgLoader.TotalNumberOfFiles][];
            int[] classLabels = new int[featureVectorsMatrix.GetLength(0)]; 

            int rowIndex=0;
            foreach (ImageLoader.ImageAndMetadata imData in imgLoader.LoadData())
            {
                FeatureExtractor featExtractor = new SimpleAmplitude(fBank, imData.GeneralImage);

                //List<AForge.Math.Complex[,]> images = fBank.Convolve(imData.GeneralImage);
                //List<GeneralImage> gImages = FilterBank.ToGeneralImages(images, GaborFilter.Gabor.ImagePart.RealPart);

                //int i=0;
                //imData.GeneralImage.ToBitmap().Save(@"C:\gaborSlike\" + "izvornaSlika" + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //foreach (GeneralImage gI in gImages)
                //{
                //    gI.ToBitmap().Save(@"C:\gaborSlike\" + "image_" + i + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                //    i++;
                //}

                //break;

                featureVectorsMatrix[rowIndex] = featExtractor.GetFeatures().ToDoubleArray();
                //ampFeatExtractor.GetFeatures().CopyToDoubleArray(ref featureVectorsMatrix, rowIndex);
                classLabels[rowIndex] = Int32.Parse(imData.ClassName); //ime klase je "0", "1" ... "9"
                rowIndex++;

                Console.WriteLine("Loading train images..." + rowIndex);
            }
                    
            Console.WriteLine("Calculating LDA...");

            IKernel kernel = new Gaussian(6.260);

            kSVM = new MulticlassSupportVectorMachine(SimpleAmplitude.GetFeatureVectorLength(fBank, imgLoader.ImageSize), kernel, 10);

            MulticlassSupportVectorLearning kSVMLearning = new MulticlassSupportVectorLearning(kSVM, featureVectorsMatrix, classLabels);

            kSVMLearning.Algorithm = (svm, classInputs, classOutputs, i, j) =>
            {
                var smo = new SequentialMinimalOptimization(svm, classInputs, classOutputs);
                smo.Complexity = 2.0;
                smo.Epsilon = 0.001;
                smo.Tolerance = 0.02;
                return smo;
            };

            kSVMLearning.Run(false);

            //knn = new KNNClassifier<int>("", 1);
            //knn.Train(lda.Transform(featureVectorsMatrix), classLabels);
        }

        public void PerformTesting(int maxNumberOfFilesPerClass)
        {
            ClassifiersStatistics statistics = new ClassifiersStatistics(10);
            
            ImageLoader imgLoader = new ImageLoader(this.XmlFilename, ImageLoader.DataType.TestExamples, maxNumberOfFilesPerClass);

            FilterBank fBank = new FilterBank(3, 2, 3, 20, 2 / 1f);
       
            foreach (ImageLoader.ImageAndMetadata imData in imgLoader.LoadData())
            {
                FeatureExtractor featExtractor = new SimpleAmplitude(fBank, imData.GeneralImage);

                double[] featureVector = featExtractor.GetFeatures().ToDoubleArray();
                int classLabel = Int32.Parse(imData.ClassName); //ime klase je "0", "1" ... "9"

                //double[] transformedFeatureVector = lda.Transform(featureVector);
                int outputClassLabel = kSVM.Compute(featureVector);
                //int outputClassLabel = knn.Classify(transformedFeatureVector);

                statistics.AddResult(classLabel, outputClassLabel);

                if (outputClassLabel == classLabel)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("OK -> expected: " + classLabel + " image: " + imData.FileInfo.Name);                 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("notOK -> className: " + classLabel +  " output: " + outputClassLabel + " image: " + imData.FileInfo.Name);                  
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            statistics.CalculateStatistics();
            Console.WriteLine(statistics.ToString());
            Console.WriteLine("Accuracy: " + statistics.Accuracy);
            Console.WriteLine("F1Micro: " + statistics.F1Micro);
            Console.WriteLine("F1Macro: " + statistics.F1Macro);
        }

        public void Serialize(string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, kSVM);
            stream.Close();
        }

        public void DeSerialize(string filename)
        {
            //Stream stream = File.Open(filename, FileMode.Open);
            //BinaryFormatter bFormatter = new BinaryFormatter();
            //lda = (LinearDiscriminantAnalysis)bFormatter.Deserialize(stream);
            //stream.Close();
        }

    }
}
