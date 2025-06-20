using Solid.TestTask.Exceptions;
using Solid.TestTask.Models;
using Solid.TestTask.Store;

namespace Solid.TestTask.Features.Currency
{
    public class CurrencyService : ICurrencyService
    {
        public Models.Currency ImportCurrency(Valute valute)
        {
            using ICurrencyStore currencyStore = new CurrencyStore();

            CurrencyBase currencyBase = new(valute.Id, valute.NumCode, valute.CharCode);

            var currency = currencyStore.GetCurrencyByCharCode(valute.CharCode);

            if (currency is null)
            {
                int currencyId = currencyStore.AddCurrency(currencyBase);

                currency = currencyStore.GetCurrency(currencyId);

                if (currency is null)
                    throw new NotFoundException($"Валюта с {currencyId} не найдена.");
            }

            return currency;
        }
    }
}
