using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace DawCommunication
{
    public class Serializer
    {
        public string Serialize(Object obj)
        {
            var res = "";
            XmlSerializer x = new XmlSerializer(obj.GetType());
            StringWriter writer = new StringWriter();//.Create(stringBuilder);
            x.Serialize(writer, obj);
            res = writer.ToString();
            writer.Dispose();
            return res;
        }

        public bool Deserialize<T>(string xmlText, out T instance) where T: class
        {
            xmlText.Replace("\n", "\r\n");
            instance = null;
            var jobOk = true;
            XmlSerializer x = new XmlSerializer(typeof(T));
            XmlReader reader = XmlReader.Create(new StringReader(xmlText));
            instance = x.Deserialize(reader) as T;
            return jobOk;
        } 


    }

}
