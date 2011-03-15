using System;
using System.Collections.Generic;
using System.Xml;
using OCR.Common;
using System.IO;
using System.Drawing;

namespace OCR.ImageTools
{
    public class ImageLoader
    {
        private const string FILE_EXTENSION = ".bmp";
        
        List<string> classNames;
        List<string> directoryPaths;
        
        private static class TagName
        {
            public const string TRAIN_EXAMPLES = "trainExamples";
            public const string TEST_EXAMPLES = "testExamples";
            public const string CLASS = "class";
            public const string CLASS_ATTRIB_NAME = "name";
            public const string CLASS_ATTRIB_DIR = "directory";
        }
        
        public enum DataType
        {
            TrainExamples=0,
            TestExamples
        }
        
        private XmlDocument xmlDoc;
      
        public ImageLoader(string xmlConfigurationFilename, DataType dataType)
        {
            Initialize(xmlConfigurationFilename, dataType);

            this.MaxOfFilesPerClass = Int32.MaxValue;
            this.TotalNumberOfFiles = GetNumberOfFiles(Int32.MaxValue);
        }

        public ImageLoader(string xmlConfigurationFilename, DataType dataType, int maxFilesPerClass)
        {
            Initialize(xmlConfigurationFilename, dataType);

            if (maxFilesPerClass <= 0)
                throw new ArgumentException("Max number of files per class must be positive value.");
            else
            {
                this.MaxOfFilesPerClass = maxFilesPerClass;
                this.TotalNumberOfFiles = GetNumberOfFiles(maxFilesPerClass);
            }
        }

        private void Initialize(string xmlConfigurationFilename, DataType dataType)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlConfigurationFilename);

            this.XmlFilename = xmlConfigurationFilename;
            this.TypeOfData = dataType;

            ReadXml(out classNames, out directoryPaths);
        }

        public int TotalNumberOfFiles
        {
            get;
            private set;
        }

        public int MaxOfFilesPerClass
        {
            get;
            private set;
        }

        public string XmlFilename
        {
            get;
            private set;
        }

        public DataType TypeOfData
        {
            get;
            private set;
        }

        public List<string> ClassNames
        {
            get { return this.classNames; }
        }

        public Size ImageSize
        {
            get
            {
                string[] files = Directory.GetFiles(this.directoryPaths[0], "*" + FILE_EXTENSION, SearchOption.TopDirectoryOnly);
                Image im = Image.FromFile(files[0]);

                return im.Size;
            }
        }

        private void ReadXml(out List<string> classNames, out List<string> directoryPaths)
        {
            XmlNode dataType;

            if (TypeOfData == DataType.TrainExamples)
                dataType = xmlDoc.GetElementsByTagName(TagName.TRAIN_EXAMPLES)[0];
            else
                dataType = xmlDoc.GetElementsByTagName(TagName.TEST_EXAMPLES)[0];

            XmlNodeList classesData = xmlDoc.GetElementsByTagName(TagName.CLASS);

            classNames = new List<string>();
            directoryPaths = new List<string>();

            string rootPath = System.IO.Path.GetDirectoryName(this.XmlFilename);

            foreach (XmlNode classData in classesData)
            {
                if (classData.ParentNode == dataType) //ili samo test ili treniranje
                {
                    classNames.Add(classData.Attributes[TagName.CLASS_ATTRIB_NAME].Value);

                    string dirPath = System.IO.Path.Combine(rootPath, classData.Attributes[TagName.CLASS_ATTRIB_DIR].Value);
                    directoryPaths.Add(dirPath);
                }
            }
        }

        private int GetNumberOfFiles(int maxFilesPerClass)
        {
            int numOfFiles = 0;

            foreach (string path in this.directoryPaths)
            {
                int filesInDir = Directory.GetFiles(path, "*" + FILE_EXTENSION, SearchOption.TopDirectoryOnly).Length;

                int allowedNumOfFiles = (maxFilesPerClass < filesInDir) ? maxFilesPerClass : filesInDir;

                numOfFiles += allowedNumOfFiles;
            }

            return numOfFiles;
        }

        public struct ImageAndMetadata
        {
            public GeneralImage GeneralImage;
            public FileInfo FileInfo;
            public string ClassName;

            public ImageAndMetadata(GeneralImage generalImage, FileInfo fileInfo, string className)
            {
                this.GeneralImage = generalImage;
                this.FileInfo = fileInfo;
                this.ClassName = className;
            }
        }

        public IEnumerable<ImageAndMetadata> LoadData()
        {
            for (int i = 0; i < this.directoryPaths.Count; i++) //za svaki direktorij pojedinog razreda
            {
                string dirPath = directoryPaths[i];
                string className = classNames[i];

                int numOfLoadedFiles = 0;
                foreach (string fileName in Directory.GetFiles(dirPath, "*" + FILE_EXTENSION, SearchOption.TopDirectoryOnly))
                {
                    if (numOfLoadedFiles == this.MaxOfFilesPerClass) continue;
                    numOfLoadedFiles++;

                    ImageAndMetadata dataInfo = new ImageAndMetadata(GeneralImage.FromBitmap((Bitmap)Image.FromFile(fileName)), new FileInfo(fileName), className);
                    yield return dataInfo;
                }
            }

        }

    }
}
