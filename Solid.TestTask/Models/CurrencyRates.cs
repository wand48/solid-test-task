namespace Solid.TestTask.Models
{
    public class CurrencyRates
    {
        public string FromCurrency { get; set; } = null!;

        public string ToCurrency { get; set; } = null!;

        public decimal CrossRate { get; set; }
    }
}
