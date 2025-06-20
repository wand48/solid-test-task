namespace Solid.TestTask.Models
{
    public class Rate : RateBase
    {
        public int RateId { get; set; }
    }

    public class RateBase
    {
        public int CurrencyId { get; set; }

        public DateOnly Date { get; set; }

        public int Nominal { get; set; }

        public decimal Value { get; set; }

        public RateBase() { }

        public RateBase(
            int currencyId, DateOnly date, int nominal, decimal value)
        {
            CurrencyId = currencyId;
            Date = date;
            Nominal = nominal;
            Value = value;
        }
    }
}
