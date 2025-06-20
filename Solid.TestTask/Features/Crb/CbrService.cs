using Solid.TestTask.Config;
using Solid.TestTask.Extensions;
using Solid.TestTask.Helpers;
using Solid.TestTask.Models;
using Solid.TestTask.Registration;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Solid.TestTask.Features.Crb
{
    public class CbrService : ICbrService
    {
        private readonly AppConfig _config;

        public CbrService()
        {
            _config = ConfigurationRegistration.Config;
        }

        public ValCurs GetQuotations(DateOnly date)
        {
            try
            {
                Uri xmlEndpoint = UriHelper.GetUri(
                    $"{_config.Cbr.XmlDaily}?date_req={date.Day}/{date.Month:D2}/{date.Year}",
                    "для получения котировок на заданный день");

                return GetXmlDaily(xmlEndpoint);
            }
            catch (UriFormatException)
            {
                throw;
            }
            catch (XmlSchemaValidationException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при получении котировок: {ex.Message}");
            }
        }

        private ValCurs GetXmlDaily(Uri xmlEndpoint)
        {
            byte[] xmlBytes = Helpers.HttpClient.DownloadBytes(xmlEndpoint);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Encoding.GetEncoding(1251).GetString(xmlBytes));

            if (xmlDoc.TryDeserializeError(out string errorMessage))
            {
                throw new InvalidOperationException($"Файл с котировками содержит ошибку: {errorMessage}");
            }

            byte[] xsdBytes = Helpers.HttpClient.DownloadBytes(_config.Cbr.ValCursXsd);
            using (var xsdReader = new StringReader(Encoding.GetEncoding(1251).GetString(xsdBytes)))
            {
                var schema = XmlSchema.Read(xsdReader, null);
                xmlDoc.Schemas.Add(schema);
            }

            Xml.ValidateXmlDocument(xmlDoc);

            var serializer = new XmlSerializer(typeof(ValCurs));
            using (var nodeReader = new XmlNodeReader(xmlDoc))
            {
                return (ValCurs)serializer.Deserialize(nodeReader);
            }
        }
    }
}