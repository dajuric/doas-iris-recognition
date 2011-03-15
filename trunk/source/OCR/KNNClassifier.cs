using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OCR
{
    [Serializable]
    class KNNClassifier<T> : Classifier<T> where T: IComparable
    {
        //to je onaj K iz KNN
        private int k;

        private List<double[]> patternList;   //lokalna kopija svih uzoraka iz skupa ua učenje
        private List<T> classList;          //klasifikacija uzoraka iz skupa za učenje

        public KNNClassifier(string newTrainingFile, int newK): base(newTrainingFile)
        {
            k = newK;
            patternList= new List<double[]>();
            classList = new List<T>();

            //ovaj dio učitava podatke iz datoteke gdje su naučeni uzorci, ako ta datoteka postoji
           /* if(File.Exists(trainingFile)){
                string[] dataFromFile = File.ReadAllLines(trainingFile);
                int vectorSpaceDimension = int.Parse(dataFromFile[0]);     //na početku datoteke je upisana dimenzionalnost vektorskog prostora
                char[] splitter = {' ',':'};
                //Converter<string, T> converter = new Converter<string, T>();
                for (int i = 1; i < dataFromFile.Length; i++)            // format upisa u datoteku    x1 x2 .... xn : klasa
                {
                    string[] splittedLine = dataFromFile[i].Split(splitter,StringSplitOptions.RemoveEmptyEntries);
                    patternList.Add(new double[vectorSpaceDimension]);
                    
                    for (int j = 0; j < vectorSpaceDimension; j++)
                    {
                        patternList[patternList.Count-1][j] = double.Parse(splittedLine[j]);
                    }
                    classList.Add((T)Convert.ChangeType(splittedLine[splittedLine.Length - 1],typeof(T)));
                }
            }*/
        }

        public override void Train(double[,] trainData, T[] trainDataClass)
        {
           /* StreamWriter writer;
            if (File.Exists(trainingFile))   // ako datoteka postoji samo dodaj nove zapise u nju
            {
                writer = File.AppendText(@trainingFile);
            }
            else
            {                           // ako datoteka ne postoji stvori novu
                writer = File.CreateText(@trainingFile);
                writer.WriteLine(trainData.GetLength(1).ToString());  //prvi red datoteke sadrži dimenzionalnost prostora značajki
            }*/
            //novi uzorci se upisuju i u datoteku i u listu istovremeno, tako da se odmah mogu koristiti u klasifikaciji
            int numNewPatterns = trainData.GetLength(0);
            int vectorSpaceDimension = trainData.GetLength(1);
            for (int i = 0; i < numNewPatterns; i++)   //petlja koja ide po vektorima uzoraka
            {
                patternList.Add(new double[vectorSpaceDimension]);
                for (int j = 0; j < vectorSpaceDimension; j++)   //petlja koja ide po elementima vektora 
                {
//                    writer.Write(trainData[i, j].ToString() + " ");
                    patternList[patternList.Count - 1][j] = trainData[i, j];
                }
//                writer.Write(": ");
//                writer.WriteLine(trainDataClass[i].ToString());   //klasa u koju pripada uzorak
                classList.Add(trainDataClass[i]);
            }
//            writer.Close();
        }

        struct DistanceAndClass{               //mala struktura da se lakše barata sa udaljenošću uzoraka i njihovima klasama kad se to treba sortirati
            public double distance;
            public T clss;
            public DistanceAndClass(double d, T c){
                distance=d;
                clss=c;
            }
        }
        
        public override T Classify(double[] newPattern)
        {
            List<DistanceAndClass> distAndClass = new List<DistanceAndClass>();
            int vectorSize = patternList[0].Length;

            double distanceToNewPattern;
            for (int i = 0; i<patternList.Count; i++)    //izračunavanje udaljenosti svih uzoraka od uzorka kojem želimo odrediti klasu
            {
                distanceToNewPattern = 0;
                for(int j=0; j<vectorSize ; j++)
                {
                    distanceToNewPattern += (newPattern[j] - patternList[i][j]) * (newPattern[j] - patternList[i][j]);
                    
                }
                distAndClass.Add(new DistanceAndClass(distanceToNewPattern, classList[i]));
            }
            var firstKNeighIndexes = (from d in distAndClass                 //selektira klase k uzoraka koji su najbliži uzorku kojeg klasificiramo
                                         orderby d.distance ascending
                                         select d).Take(k);

            var mostFreqClassInNeigh = (from f in firstKNeighIndexes       //bira se klasa koja se najčešće pojavljuje u skupu od k susjeda
                                        group f by f.clss into g
                                        orderby g.Count() descending
                                        select g.Key).First();
            return mostFreqClassInNeigh;
        }
    }
}
