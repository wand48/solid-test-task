using System.Globalization;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace Solid.TestTask.Models
{
    [XmlRoot("ValCurs")]
    public class ValCurs
    {
        [XmlElement(nameof(Valute))]
        public List<Valute> Valute { get; set; }
    }

    public class Valute : IXmlSerializable
    {
        [XmlAttribute("ID")]
        public string Id { get; set; } = null!;

        [XmlElement(nameof(NumCode))]
        public string NumCode { get; set; } = null!;

        [XmlElement(nameof(CharCode))]
        public string CharCode { get; set; } = null!;

        [XmlElement(nameof(Nominal))]
        public int Nominal { get; set; }

        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Value))]
        public decimal Value { get; set; }

        [XmlElement(nameof(VunitRate))]
        public decimal VunitRate { get; set; }

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            Id = reader.GetAttribute("ID");

            reader.ReadStartElement("Valute");

            NumCode = reader.ReadElementString("NumCode");
            CharCode = reader.ReadElementString("CharCode");
            Nominal = int.Parse(reader.ReadElementString("Nominal"));
            Name = reader.ReadElementString("Name");
            Value = ParseDecimalInvariant(reader.ReadElementString("Value"));
            VunitRate = ParseDecimalInvariant(reader.ReadElementString("VunitRate"));

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ID", Id);
            writer.WriteElementString("NumCode", NumCode);
            writer.WriteElementString("CharCode", CharCode);
            writer.WriteElementString("Nominal", Nominal.ToString());
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("Value", Value.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("VunitRate", VunitRate.ToString(CultureInfo.InvariantCulture));
        }

        private static decimal ParseDecimalInvariant(string input)
        {
            string cleaned = input
                .Replace(" ", "")
                .Replace(",", ".");

            return decimal.Parse(
                cleaned,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                CultureInfo.InvariantCulture
            );
        }
    }
}
