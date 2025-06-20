using Solid.TestTask.Models;

namespace Solid.TestTask.Features.Rate
{
    public interface IRateStore : IDisposable
    {
        public Models.Rate? GetRate(int currencyId, DateOnly date);

        public IEnumerable<CurrencyRates> GetCurrenciesRates(DateOnly date);

        public void AddRate(RateBase model);

        public void UpdateRate(Models.Rate rate);
    }
}
