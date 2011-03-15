using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCR
{
    [Serializable]
    abstract class Classifier<T> where T : IComparable           //oznaka klase je implementirana kao generic tako da se to može po želji staviti
    {  
        // varijabla koja sadrži parametre koje je klasifikator naučio
        protected string trainingFile; 

        //metoda za učenje klasifikatora, parametri: polje uzoraka, oznake pripadnosti uzoraka klasama (ja sam stavio int za sada, ali može biti i neki drugi tip podataka)
        public abstract void Train(double[,] trainData, T[] trainDataClass);

        //metoda za klasifikaciju uzoraka
        public abstract T Classify(double[] newPattern);

        //konstruktor je protected jer se zove iz konstruktora klasa koje ga nasljeđuju
        protected Classifier(string newTrainingFile)
        {
            trainingFile = newTrainingFile;
        }
    }
}
