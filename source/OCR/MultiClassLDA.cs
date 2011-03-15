using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics;
using System.Collections;
using System.Collections.ObjectModel;

namespace OCR
{
    [Serializable]
    class MultiClassLDA
    {
        //dimenzija ulaznih podataka
        private int dimension;
        //broj klasa
        private int classes;

        //sve srednje vrijednosti
        private double[] totalMeans;
        //sve standardne devijacije
        private double[] totalStdDevs;
        

        private int[] classCount;
        private double[][] classMeans;
        private double[][] classStdDevs;
        private double[][,] classScatter;
        
        private double[,] eigenVectors;
        private double[] eigenValues;
       
        
        private double[,] result;
        //ulazni podaci
        private double[,] source;
        //željeni izlaz
        private int[] outputs;
        //matrice raspršenosti
        double[,] Sw, Sb, St; // Scatter matrices

        private double[] discriminantProportions;
        private double[] discriminantCumulative;

        
        DiscriminantAnalysisClassCollection classCollection;

        #region Construction
         /// <summary>
        ///  konstruktor
        /// </summary>
        /// <param name="inputs">ulazna matrica s podacima
        /// varijable su stupci  a opažanje reci</param>
        /// <param name="output">za svaki redak koji bi trebao biti izlaz</param>
        public MultiClassLDA(double[,] inputs, int[] output)
        {
            // uzmemo broj klasa u ulaznim podacima
            int startingClass = output.Min();
            this.classes = output.Max() - startingClass + 1;

            //pohranimo originalne podatke
            this.source = inputs;
            this.outputs = output;
            this.dimension = inputs.GetLength(1);

            // pomoćne strkture
            this.classCount = new int[classes];
            this.classMeans = new double[classes][];
            this.classStdDevs = new double[classes][];
            this.classScatter = new double[classes][,];

            
            // nista pametno, samo jedna klasa koja čuva sve informacije o klasi
            DiscriminantAnalysisClass[] collection = new DiscriminantAnalysisClass[classes];
            for (int i = 0; i < classes; i++)
                collection[i] = new DiscriminantAnalysisClass(this, i, startingClass + i);
            //opet nista pametno, sve natrpamo u jednu klasu
            this.classCollection = new DiscriminantAnalysisClassCollection(collection);
             
        }
        #endregion

        #region Properties
        /// <summary>Origigi ulazni podaci</summary>
        public double[,] Source
        {
            get { return this.source; }
        }

        /// <summary>
        ///   rezulat primejne analize
        /// </summary>
        public double[,] Result
        {
            get { return this.result; }
            protected set { this.result = value; }
        }

        /// <summary>
        ///  izlazni element
        /// </summary>
        public int[] Classifications
        {
            get { return this.outputs; }
        }

        /// <summary>središnje vrijednosti</summary>
        public double[] Means
        {
            get { return totalMeans; }
            protected set { totalMeans = value; }
        }

        /// <summary>standardne devijacije</summary>
        public double[] StandardDeviations
        {
            get { return totalStdDevs; }
            protected set { totalStdDevs = value; }
        }

        /// <summary>SW</summary>
        public double[,] ScatterWithinClass
        {
            get { return Sw; }
            protected set { Sw = value; }
        }

        /// <summary>SB</summary>
        public double[,] ScatterBetweenClass
        {
            get { return Sb; }
            protected set { Sb = value; }
        }

        /// <summary>suma matrica</summary>
        public double[,] ScatterMatrix
        {
            get { return St; }
            protected set { St = value; }
        }

        /// <summary>
        /// svojstveni vektori
        /// </summary>
        public double[,] DiscriminantMatrix
        {
            get { return eigenVectors; }
            protected set { eigenVectors = value; }
        }

        /// <summary>
        /// svojstvene vrijednosti
        /// </summary>
        public double[] Eigenvalues
        {
            get { return eigenValues; }
            protected set { eigenValues = value; }
        }

       

      

        /// <summary>za svaku klasu daje neke informacije malo objektno</summary>
        public DiscriminantAnalysisClassCollection Classes
        {
            get { return classCollection; }
        }

        /// <summary>
        ///  za savaku klasu matrica rasprešenosti
        /// </summary>
        public double[][,] ClassScatter
        {
            get { return classScatter; }
        }

        /// <summary>
        ///  za svaku klasu vektor središnjih vrijednosti
        /// </summary>
        public double[][] ClassMeans
        {
            get { return classMeans; }
        }

        /// <summary>
        ///  za svaku klasu vraća vektor standardnih devijacija
        /// </summary>
        public double[][] ClassStandardDeviations
        {
            get { return classStdDevs; }
        }

        /// <summary>
        ///   broj klasa
        /// </summary>
        public int[] ClassCount
        {
            get { return classCount; }
        }
        #endregion


        public virtual void Compute()
        {
            
            //izračunaj srednju vrijednost svake klase
            Means =Statistics.Mean(source);
            //izračunaj devijaciju svake klase
            StandardDeviations =Statistics.StandardDeviation(source, totalMeans);
            //ukupna dimenzionalnost podataka
            double total = dimension;

            
            //konstrukcija matirca raspršenosti
            //W-within
            //B-Between
            this.Sw = new double[dimension, dimension];
            this.Sb = new double[dimension, dimension];

            
            //
            //za svaki c od ukupnog broja klasa
            //uzmi podatke za sadašnju klasu podataka(subset)
            //izračunaj središnju vrijednost za klasu(mean) 
            //izračunaj ScatterWithin(Swc) za sadašnju klasu
            //dodaj tako izračunate SWc na ukupni SW(pazimo da idemo po svim dimenzijama jer ipak su to matrice
            //za računanje ScatterBetween(Sb) potrebno je oduzeti ukupnu srednju vrijednost(totalMeans) od srednje 
            //vrijednsoti za sadašnju klasu(pohranjumo u varijablu d). Izračunamo trenutačni Sbc i na kraju po zbrajamo
            //sa sadašnjim Sb-om
            for (int c = 0; c < Classes.Count; c++)
            {
                // uzimamo podatke koji pripadaju toj klasi
                double[,] subset = Classes[c].Subset;
                int count = subset.GetLength(0);
               //ovo tu postoji jer je ovaj Accord malo cudan, njemu je dimenzija matrica 0 puta nesto ok
                if (count == 0)
                {
                    continue;
                }

                // ovdje računamo srednju vrijednost za trenutačnu klasu
                double[] mean =Statistics.Mean(subset);


                // izračunamo matricu rasprešenosti za sadašnju klasu
                double[,] Swc =Statistics.Scatter(subset, mean, (double)count);

                // Sw = Sw + Swc
                for (int i = 0; i < dimension; i++)
                    for (int j = 0; j < dimension; j++)
                        Sw[i, j] += Swc[i, j];


                // za dobivanje Sb
                double[] d = mean.Subtract(totalMeans);
                double[,] Sbc = Matrix.OuterProduct(d, d).Multiply(total);

                // Sb = Sb + Sbc
                for (int i = 0; i < dimension; i++)
                    for (int j = 0; j < dimension; j++)
                        Sb[i, j] += Sbc[i, j];


                // Jos malo nekih dodatnih informacija
                
                this.classScatter[c] = Swc;
                this.classCount[c] = count;
                this.classMeans[c] = mean;
                this.classStdDevs[c] =Statistics.StandardDeviation(subset, mean);
                 
            }


            // tražimo svojstevene vektore jer kriterijska funkcija W poprima max za vrijednosti
            //koje su jednake svojstvenim vektorima
            GeneralizedEigenvalueDecomposition gevd = new GeneralizedEigenvalueDecomposition(Sb, Sw);

            // uzmemo svojstvene vektore i svojstvene vrijednosti
            double[] evals = gevd.RealEigenvalues;
            double[,] eigs = gevd.Eigenvectors;

            // sortiramo vektore padajućim poretkom(zanimaju nas najvažniji vektori)
            eigs = Matrix.Sort(evals, eigs, new GeneralComparer(ComparerDirection.Descending, true));


            // pohranimo dobivene informacije
            this.Eigenvalues = evals;
            this.DiscriminantMatrix = eigs;

            // ulazne podatke projiciramo korištenjem dobivenih vektora u niži potprostor
            this.result = source.Multiply(eigenVectors);

           
        }



        /// <summary>projicira točku u novi prostor manje dimenzionalnosti</summary>
        /// <param name="data">matrica koja se projicira</param>
        /// <param name="discriminants">broj vektora koji se koristi u projekciji</param>
        public virtual double[,] Transform(double[,] data, int discriminants)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            double[,] r = new double[rows, discriminants];

            // jednostavno množimo matricu sa zadanim brojem prvih svojstvenih vektora,
            //zašto prvih, pa zato što smo ih prije poredali po veličini i oni imaju 
            //najveću diskriminacijsku veličinu
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < discriminants; j++)
                    for (int k = 0; k < cols; k++)
                        r[i, j] += data[i, k] * eigenVectors[k, j];

            return r;
        }

        /// <summary>projicira točku u novi prostor manje dimenzionalnosti</summary>
        /// <remarks> u ovom slučaju koristimo sve vektore</remarks>
        /// <param name="data">točka koju projiciramo</param>
        public double[] Transform(double[] data)
        {
            return Transform(data.ToMatrix(),eigenValues.Length).GetRow(0);
        }

        /// <summary>projicira točku u novi prostor manje dimenzionalnosti</summary>
        /// <param name="data">točka koju projiciramo</param>
        /// <param name="discriminants">broj vektora koji koristimo u projekciji.</param>
        public double[] Transform(double[] data, int discriminants)
        {
            return Transform(data.ToMatrix(), discriminants).GetRow(0);
        }
    }

    #region Support Classes
   
    [Serializable]
    public class DiscriminantAnalysisClass
    {
        //analiza iz koje proizlazi ovaj objekt
        private MultiClassLDA analysis;
        //klasa podataka koju wrapa
        private int classNumber;
        //index u listi elementata
        private int index;

        /// <summary>
        ///  napravimo novi objekt te klase
        /// </summary>
        internal DiscriminantAnalysisClass(MultiClassLDA analysis, int index, int classNumber)
        {
            this.analysis = analysis;
            this.index = index;
            this.classNumber = classNumber;
        }

        /// <summary>
        ///   index u izvornoj analizi
        /// </summary>
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        ///  broj koji govori koja je to klasa
        /// </summary>
        public int Number
        {
            get { return classNumber; }
        }

        /// <summary>
        ///   postotak elementa od svih uzoraka
        ///   zapravo govori koliko ta klasa prevladava
        ///   nad ostalim klasama
        /// </summary>
        public double Prevalence
        {
            get { return (double)Count / analysis.Source.GetLength(0); }
        }

        /// <summary>
        ///   za tu klasu koja je središnja vrijednost
        /// </summary>
        public double[] Mean
        {
            get { return analysis.ClassMeans[index]; }
        }

        /// <summary>
        ///  za tu klasu koja je standardna devijacija
        /// </summary>
        public double[] StandardDeviation
        {
            get { return analysis.ClassStandardDeviations[index]; }
        }

        /// <summary>
        ///   scaterr matrica za tu klasu
        /// </summary>
        public double[,] Scatter
        {
            get { return analysis.ClassScatter[index]; }
        }

        /// <summary>
        ///   indexi redaka u ulaznim podacima koji pripadaju toj klasi
        /// </summary>
        public int[] Indexes
        {
            get { return Matrix.Find(analysis.Classifications, y => y == classNumber); }
        }

        /// <summary>
        ///   podaci iz svih originalnih koji obuhvaćaju tu klasu
        /// </summary>
        public double[,] Subset
        {
            get
            {
                return analysis.Source.Submatrix(Indexes);
            }
        }

        /// <summary>
        ///   broj uzoraka u toj klasi
        /// </summary>
        public int Count
        {
            get { return analysis.ClassCount[index]; }
        }

      
    }

   
   

    /// <summary>
    ///  Kolekcija objekata,postoji samo da povećamo broj klasa :)
    ///   
  
    /// </summary>
    /// 
    [Serializable]
    public class DiscriminantAnalysisClassCollection : ReadOnlyCollection<DiscriminantAnalysisClass>
    {
        internal DiscriminantAnalysisClassCollection(DiscriminantAnalysisClass[] components)
            : base(components)
        {
        }
    }
    #endregion
}
