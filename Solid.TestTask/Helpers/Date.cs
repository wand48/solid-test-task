using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Solid.TestTask.Helpers
{
    public static class Date
    {
        public static DateOnly GetDate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return DateOnly.FromDateTime(DateTime.Now);
            }

            if (!DateOnly.TryParseExact(
                input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly result))
            {
                throw new ValidationException("Неверный формат даты. Необходимо указать дату в формате ДД.ММ.ГГГГ.");
            }

            return result;
        }
    }
}
