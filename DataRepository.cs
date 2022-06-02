using System;
using System.IO;
using System.Xml.Serialization;

namespace Bezier
{
    public enum DataRepositoryType
    {
        XMLFile,
        XMLResource,
        Registry
    }
    public class DataRepository : IDisposable
    {
        private DataRepositoryType repositoryType = DataRepositoryType.XMLResource;
        private static string xmlRepositoryFileName = "digits.xml";

        public DataRepository(DataRepositoryType repositoryType)
        {
            this.repositoryType = repositoryType;

            switch (repositoryType)
            {
                case DataRepositoryType.Registry:
                    break;
                case DataRepositoryType.XMLFile:
                    Digits = LoadFromXML();
                    break;
                case DataRepositoryType.XMLResource:
                    string result = string.Empty;

                    using (Stream stream = this.GetType().Assembly.GetManifestResourceStream("Bezier." + xmlRepositoryFileName))
                    {
                        if (stream.Length == 0) throw new InvalidDataException("Внедренный ресурс не найден или отсутствуют данные");
                        Digits = LoadFromStream(stream);
                    }                    
                    break;
            }
        }
        public void Dispose()
        {
            //SaveAsXML();
        }

        public bool SaveRepository()
        {
            if (repositoryType == DataRepositoryType.XMLFile)
            {
                return SaveAsXML();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public Digit[] Digits { get; set; }
        private Digit[] LoadFromStream(Stream stream)
        {
            XmlSerializer sr = new XmlSerializer(typeof(Digit[]));
            try
            {
                var r = sr.Deserialize(stream);
                return (Digit[])r;
            }
            catch
            {
                return null;
            }
        }
        private Digit[] LoadFromXML()
        {
            if (!File.Exists(xmlRepositoryFileName)) return null;
            
            using (FileStream reader = File.Open(xmlRepositoryFileName, FileMode.Open))
            {
                return LoadFromStream(reader);
            }
        }
        private bool SaveAsXML()
        {
            using (FileStream writer = File.Open(xmlRepositoryFileName, FileMode.Create))
            {
                XmlSerializer sr = new XmlSerializer(typeof(Digit[]));
                try
                {
                    sr.Serialize(writer, Digits);
                }
                catch { return false; }
            }
            return true;
        }
    }
}
