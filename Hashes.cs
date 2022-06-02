using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Bezier
{
    [Serializable]
    public class HashItem
    {
        public int c { get; set; }
        public int n { get; set; }
        public double r { get; set; }
        public Digit d { get; set; }
    }

    public class HashItemComparer : IEqualityComparer<HashItem>
    {
        public bool Equals(HashItem x, HashItem y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            if (x.c == y.c && x.n == y.n && x.r == y.r && x.d.GetHashCode() == y.d.GetHashCode())
            {
                return true;
            }
            return
                false;
        }
        public int GetHashCode(HashItem number)
        {
            int hashX = number.c.GetHashCode();
            int hashY = number.n.GetHashCode();
            int hashZ = number.r.GetHashCode();
            int hashW = number.d.GetHashCode();

            return hashX ^ hashY ^ hashZ ^ hashW;
        }
    }

    [Serializable]
    public class DigitHash
    {
        public HashSet<HashItem> h = new HashSet<HashItem>();

        public bool SaveAsXML(String fileName)
        {
            using (FileStream writer = File.Open(fileName, FileMode.Create))
            {
                XmlSerializer sr = new XmlSerializer(this.GetType());
                try
                {
                    sr.Serialize(writer, this);
                }
                catch { return false; }
            }
            return true;
        }

        public static DigitHash LoadFromXML(String fileName)
        {
            return LoadFromXML(fileName, typeof(Digit));
        }

        protected static DigitHash LoadFromXML(String fileName, Type t)
        {
            using (FileStream reader = File.Open(fileName, FileMode.Open))
            {
                XmlSerializer sr = new XmlSerializer(t);
                try
                {
                    return (DigitHash)sr.Deserialize(reader);
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
