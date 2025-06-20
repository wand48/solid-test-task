using Solid.TestTask.Models;

namespace Solid.TestTask.Features.Currency
{
    public interface ICurrencyStore : IDisposable
    {
        public Models.Currency? GetCurrency(int currencyId);

        public Models.Currency? GetCurrencyByCharCode(string charCode);

        public int AddCurrency(CurrencyBase model);
    }
}
