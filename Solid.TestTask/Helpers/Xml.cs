using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Solid.TestTask.Helpers
{
    public static class Xml
    {
        public static T DeserializeXml<T>(string xmlContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xmlContent))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static void ValidateXmlDocument(XmlDocument xmlDoc)
        {
            try
            {
                var validationErrors = new List<string>();
                xmlDoc.Validate((sender, e) =>
                {
                    validationErrors.Add(e.Message);
                });

                if (validationErrors.Any())
                {
                    throw new XmlSchemaValidationException($"XML не прошел валидацию: {string.Join("; ", validationErrors)}");
                }
            }
            catch (XmlSchemaValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new XmlSchemaValidationException($"Ошибка при валидации XML: {ex.Message}", ex);
            }
        }
    }
}
