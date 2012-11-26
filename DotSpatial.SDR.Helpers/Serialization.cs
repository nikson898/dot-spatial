using System.IO;
using System.Xml.Serialization;

namespace DotSpatial.SDR.Helpers
{
    public class Serialization<T>
    {
        public static void SerializerSaveFile(string xmlPath, T settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(xmlPath);
            serializer.Serialize(writer, settings);
            writer.Close();
        }

        public static T SerializerLoadFile(string xmlPath)
        {
            T temp;
            XmlSerializer deSerializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(xmlPath);
            temp = (T)deSerializer.Deserialize(reader);
            reader.Close();
            return temp;
        }
    }
}
