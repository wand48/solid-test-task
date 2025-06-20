using Dapper;
using Dapper.Transaction;
using Solid.TestTask.Exceptions;
using Solid.TestTask.Features.Rate;
using Solid.TestTask.Models;

namespace Solid.TestTask.Store
{
    public class RateStore : TestStore, IRateStore
    {
        public Rate? GetRate(int currencyId, DateOnly date)
        {
            string query = @"SELECT
                    RateId,
                    CurrencyId,
                    Date,
                    Nominal,
                    Value
                FROM
                    Rate
                WHERE
                    CurrencyId = @currencyId
                    AND Date = @Date";

            try
            {
                var result = Connection.QueryFirstOrDefault(
                    query,
                    new { currencyId, Date = date.ToDateTime(TimeOnly.MinValue) });

                if (result == null)
                    return null;

                return new Rate
                {
                    RateId = result.RateId,
                    CurrencyId = result.CurrencyId,
                    Date = DateOnly.FromDateTime(result.Date),
                    Nominal = result.Nominal,
                    Value = result.Value
                };
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public IEnumerable<CurrencyRates> GetCurrenciesRates(DateOnly date)
        {
            string query = @"WITH CurrencyRates AS (
                    SELECT 
                        c.CurrencyID,
		                c.CharCode,
                        r.Nominal,
                        r.[Value]
                    FROM 
                        dbo.Rate r
		                INNER JOIN dbo.Currency c ON c.CurrencyID = r.CurrencyID
                    WHERE 
                        r.[Date] = @Date
                )
                SELECT 
                    crFrom.CharCode AS FromCurrency,
                    crTo.CharCode AS ToCurrency,
                    ROUND((crFrom.[Value] / crFrom.Nominal) / (crTo.[Value] / crTo.Nominal), 4) AS CrossRate
                FROM 
                    CurrencyRates crFrom
                CROSS JOIN 
                    CurrencyRates crTo
                WHERE 
                    crFrom.CurrencyID <> crTo.CurrencyID
                ORDER BY 
                    crFrom.CharCode, crTo.CharCode";

            try
            {
                return Connection.Query<CurrencyRates>(
                    query,
                    new { Date = date.ToDateTime(TimeOnly.MinValue) });
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public void AddRate(RateBase model)
        {
            string query = @"INSERT Rate
                    (CurrencyId, Date, Nominal, Value)
                VALUES
                    (@CurrencyId, @Date, @Nominal, @Value)";

            try
            {
                using var transaction = Connection.BeginTransaction();

                transaction.Execute(
                    query,
                    new { model.CurrencyId, Date = model.Date.ToDateTime(TimeOnly.MinValue), model.Nominal, model.Value });

                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public void UpdateRate(Rate rate)
        {
            string query = @"UPDATE Rate
                SET
                    Nominal = @Nominal,
                    Value = @Value
                WHERE
                    RateId = @RateId
                    AND Date = @Date";

            try
            {
                using var transaction = Connection.BeginTransaction();

                transaction.Execute(
                    query,
                    new { rate.RateId, Date = rate.Date.ToDateTime(TimeOnly.MinValue), rate.Nominal, rate.Value });

                transaction.Commit();
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Connection.Close();
            }
        }
    }
}
