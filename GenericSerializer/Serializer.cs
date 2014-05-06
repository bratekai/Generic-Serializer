using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GenericSerializer
{
    public sealed class Serializer<T> where T : class
    {
        public T Deserialize(string stringXml, SerializationFormatter formatter = SerializationFormatter.Xml)
        {
            try
            {
                switch (formatter)
                {
                    case SerializationFormatter.Binary:
                        TextReader txtReader = new StringReader(stringXml);
                        using (var ms = new MemoryStream(Encoding.Default.GetBytes(stringXml)))
                        {
                            return (T)(new BinaryFormatter()).Deserialize(ms);
                        }
                    case SerializationFormatter.Xml:
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        return (T)serializer.Deserialize(new StringReader(stringXml));
                    default:
                        throw new NotSupportedException("Invalid formatter option");
                }
            }
            catch (SerializationException ex)
            {
                throw new NotSupportedException(ex.ToString());
            }
        }
        public T Deserialize(Stream stream, SerializationFormatter formatter = SerializationFormatter.Xml)
        {
            try
            {
                switch (formatter)
                {
                    case SerializationFormatter.Binary:
                        return (T)(new BinaryFormatter()).Deserialize(stream);
                    case SerializationFormatter.Xml:
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        return (T)serializer.Deserialize(stream);
                    default:
                        throw new NotSupportedException("Invalid formatter option");
                }
            }
            catch (SerializationException ex)
            {
                throw new NotSupportedException(ex.ToString());
            }
        }
        public void Serialize(Stream stream, T obj, SerializationFormatter formatter = SerializationFormatter.Xml)
        {
            try
            {
                switch (formatter)
                {
                    case SerializationFormatter.Binary:
                        (new BinaryFormatter()).Serialize(stream, obj);
                        break;
                    case SerializationFormatter.Xml:
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(stream, obj);
                        break;
                    default:
                        throw new NotSupportedException("Invalid formatter option");
                }
            }
            catch (SerializationException ex)
            {
                throw new NotSupportedException(ex.ToString());
            }
        }
        public void Serialize(string path, T obj, SerializationFormatter formatter = SerializationFormatter.Xml)
        {
            try
            {
                switch (formatter)
                {
                    case SerializationFormatter.Binary:
                        using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                        {
                            (new BinaryFormatter()).Serialize(fs, obj);
                        }
                        break;
                    case SerializationFormatter.Xml:
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        TextWriter txtWriter = new StreamWriter(path);
                        serializer.Serialize(txtWriter, obj);
                        break;
                    default:
                        throw new NotSupportedException("Invalid formatter option");
                }
            }
            catch (SerializationException ex)
            {
                throw new NotSupportedException(ex.ToString());
            }
        }
    }
}
