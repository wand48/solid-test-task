using Solid.TestTask.Models;
using System.Xml.Serialization;
using System.Xml;

namespace Solid.TestTask.Extensions
{
    public static class CbrXml
    {
        public static bool TryDeserializeError(this XmlDocument xmlDoc, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var serializer = new XmlSerializer(typeof(ValCursError));
                using (var nodeReader = new XmlNodeReader(xmlDoc))
                {
                    var errorResult = (ValCursError)serializer.Deserialize(nodeReader);
                    if (errorResult != null && errorResult.HasError)
                    {
                        errorMessage = errorResult.ErrorMessage;
                        return true;
                    }
                }
            }
            catch
            {
                //fire and forget
            }

            return false;
        }
    }
}
