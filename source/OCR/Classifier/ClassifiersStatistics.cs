using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCR.Classifier
{
    public class ClassifiersStatistics
    {
        private int[,] confusionMatrix = null; //stvarne klase su stupci, a predviđene retci
        
        internal ClassifiersStatistics(int numberOfClasses)
        {
            confusionMatrix = new int[numberOfClasses, numberOfClasses];
            this.NumberOfClasses = numberOfClasses;
        }

        internal void AddResult(int correctClass, int calculatedClass)
        {
            confusionMatrix[calculatedClass, correctClass] += 1;
        }

        internal void CalculateStatistics()
        {
            List<double> F1Micros = new List<double>();

            for (int classLabel = 0; classLabel < this.NumberOfClasses; classLabel++)
            {
                int truePositivePerClass = CalculateTruePositive(classLabel);
                int falsePositivePerClass = CalculateFalsePositive(classLabel);
                int falseNegativePerClass = CalculateFalseNegative(classLabel);

                double precisionPerClass = CalculatePrecision(truePositivePerClass, falsePositivePerClass);
                double recallPerClass = CalculateRecall(truePositivePerClass, falseNegativePerClass);
                F1Micros.Add(CalculateF1Micro(precisionPerClass, recallPerClass));

                this.TruePositive += truePositivePerClass;
                this.FalsePositive += falsePositivePerClass;
                this.FalseNegative += falseNegativePerClass;
            }

            this.Precision = CalculatePrecision(this.TruePositive, this.FalsePositive);
            this.Recall = CalculateRecall(this.TruePositive, this.FalseNegative);

            this.F1Macro = CalculateF1Macro(F1Micros);
            this.F1Micro = CalculateF1Micro(this.Precision, this.Recall);
            this.Accuracy = CalculateAccuracy();
        }

        private int CalculateFalsePositive(int classLabel)
        {
            int sum = 0;

            for (int col = 0; col < this.NumberOfClasses; col++)
            {
                sum += confusionMatrix[classLabel, col];
            }

            return (sum - confusionMatrix[classLabel, classLabel]);
        }

        private int CalculateFalseNegative(int classLabel)
        {
            int sum = 0;

            for (int row = 0; row < this.NumberOfClasses; row++)
            {
                sum += confusionMatrix[row, classLabel];
            }

            return (sum - confusionMatrix[classLabel, classLabel]);
        }

        private int CalculateTruePositive(int classLabel)
        {
            return confusionMatrix[classLabel, classLabel];
        }

        private double CalculatePrecision(int truePositive, int falsePositive)
        {
            return (double)truePositive / (truePositive + falsePositive);
        }

        private double CalculateRecall(int truePositive, int falseNegative)
        {
            return (double)truePositive / (truePositive + falseNegative);
        }

        private double CalculateF1Micro(double precison, double recall)
        {
            return 2 * precison * recall / (precison + recall);
        }

        private double CalculateF1Macro(List<double> F1Micros)
        {
            double sum = 0;

            foreach (double F1Micro in F1Micros)
            {
                sum += F1Micro;
            }

            return sum / F1Micros.Count;
        }

        private double CalculateAccuracy()
        {
            int sum = 0;
            foreach (int elem in confusionMatrix)
            {
                sum += elem;
            }

            int truePositive = 0;
            for (int diagonalIndex = 0; diagonalIndex < this.NumberOfClasses; diagonalIndex++)
            {
                truePositive += confusionMatrix[diagonalIndex, diagonalIndex];
            }

            return (double)truePositive / sum;
        }

        public int[,] ConfusionMatrix
        {
            get { return this.confusionMatrix; }
        }

        public int NumberOfClasses
        {
            get;
            private set;
        }

        public int TruePositive
        {
            get;
            private set;
        }

        public int FalsePositive
        {
            get;
            private set;
        }

        public int FalseNegative
        {
            get;
            private set;
        }

        public double Precision
        {
            get;
            private set;
        }

        public double Recall
        {
            get;
            private set;
        }

        public double F1Micro
        {
            get;
            private set;
        }

        public double F1Macro
        {
            get;
            private set;
        }

        public double Accuracy
        {
            get;
            private set;
        }

        public override string ToString()
        {
            int maxNum = 0;
            foreach (int elem in confusionMatrix)
            {
                if (elem > maxNum) maxNum = elem;
            }

            int longestNumLength = maxNum.ToString().Length;

            string result = "";

            for (int row = 0; row < this.NumberOfClasses; row++)
            {
                for (int col = 0; col < this.NumberOfClasses; col++)
                {
                    result += String.Format("{0, " + longestNumLength + "}", confusionMatrix[row, col]) + "  ";
                }

                result = result.Substring(0, result.Length - 2) + "\n";
            }

            result = result.Substring(0, result.Length - 1);

            return result;
        }

    }

}
