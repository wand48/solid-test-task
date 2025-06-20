using Solid.TestTask.Models;

namespace Solid.TestTask.Features.Currency
{
    public interface ICurrencyService
    {
        public Models.Currency ImportCurrency(Valute valute);
    }
}
