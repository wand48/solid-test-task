namespace Solid.TestTask.Models
{
    public class Currency : CurrencyBase
    {
        public int CurrencyId { get; set; }
    }

    public class CurrencyBase
    {
        public string Id { get; set; } = null!;

        public string NumCode { get; set; } = null!;

        public string CharCode { get; set; } = null!;

        public CurrencyBase() { }

        public CurrencyBase(
            string id, string numCode, string charCode)
        {
            Id = id;
            NumCode = numCode;
            CharCode = charCode;
        }
    }
}
