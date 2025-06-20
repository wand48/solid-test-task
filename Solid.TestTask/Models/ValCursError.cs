using System.Xml.Serialization;

namespace Solid.TestTask.Models
{
    [XmlRoot("ValCurs")]
    public class ValCursError
    {
        [XmlText]
        public string ErrorMessage { get; set; } = string.Empty;

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
    }
}
