using Dapper;
using Dapper.Transaction;
using Solid.TestTask.Exceptions;
using Solid.TestTask.Features.Currency;
using Solid.TestTask.Models;

namespace Solid.TestTask.Store
{
    class CurrencyStore : TestStore, ICurrencyStore
    {
        public Currency? GetCurrency(int currencyId)
        {
            string query = @"SELECT
                    CurrencyId,
                    Id,
                    NumCode,
                    CharCode
                FROM
                    Currency
                WHERE
                    CurrencyId = @currencyId";

            try
            {
                return Connection.QueryFirstOrDefault<Currency?>(query, new { currencyId });
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public Currency? GetCurrencyByCharCode(string charCode)
        {
            string query = @"SELECT
                    CurrencyId,
                    Id,
                    NumCode,
                    CharCode
                FROM
                    Currency
                WHERE
                    CharCode = @charCode";

            try
            {
                return Connection.QueryFirstOrDefault<Currency?>(query, new { charCode });
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public int AddCurrency(CurrencyBase model)
        {
            string query = @"INSERT Currency
                    (ID, NumCode, CharCode)
                VALUES
                    (@Id, @NumCode, @CharCode)

                SELECT SCOPE_IDENTITY()";

            try
            {
                using var transaction = Connection.BeginTransaction();

                var result = transaction.ExecuteScalar<int>(query, model);

                transaction.Commit();

                return result;
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
