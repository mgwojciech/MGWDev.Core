using MGWDev.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MGWDev.Core.Repositories
{
    class XMLFileRepository<T,U> : InMemoryRepository<T, U> where T : class
    {
        string Path { get; set; }
        public XMLFileRepository(string filePath, Func<U, Func<T, bool>> identityQuery) :
            base(ConvertFromString(FileHelper.ReadFile(filePath)), identityQuery)
        {
            Path = filePath;

        }
        public override void Commit()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xmlSerializer.Serialize(writer, Entities);
                    FileHelper.OverwriteFile(Path, sww.ToString());
                }
            }
        }

        private static List<T> ConvertFromString(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            object result;

            using (TextReader reader = new StringReader(xmlString))
            {
                result = serializer.Deserialize(reader);
            }

            return result as List<T>;
        }
    }
}
