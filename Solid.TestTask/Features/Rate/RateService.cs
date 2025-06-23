using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Solid.TestTask.Helpers;
using Solid.TestTask.Models;
using Solid.TestTask.Store;

namespace Solid.TestTask.Features.Rate
{
    public class RateService : IRateService
    {
        public void ImportRate(RateBase model)
        {
            using IRateStore rateStore = new RateStore();

            var rate = rateStore.GetRate(model.CurrencyId, model.Date);

            if (rate is null)
            {
                rateStore.AddRate(model);
            }
            else
            {
                rate.Nominal = model.Nominal;
                rate.Value = model.Value;

                rateStore.UpdateRate(rate);
            }
        }

        public void ExportCrossRates(DateOnly date)
        {
            using IRateStore rateStore = new RateStore();

            try
            {
                var currenciesRates = rateStore.GetCurrenciesRates(date);

                if (!currenciesRates.Any())
                {
                    Console.WriteLine($"\nНа текущую дату отсутствуют курсы валют.");
                    return;
                }

                ExportExcel(currenciesRates, date);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при экспорте кросс-валют: {ex.Message}");
            }
        }

        private static void ExportExcel(IEnumerable<CurrencyRates> currenciesRates, DateOnly date)
        {
            var gcr = currenciesRates
                .GroupBy(c => c.FromCurrency);

            var workbook = new XSSFWorkbook();

            var font = workbook.CreateFont();
            font.IsBold = true;

            var headerStyle = Excel.CreateBorderedCellStyle(workbook);
            headerStyle.SetFont(font);

            var numberStyle = Excel.CreateBorderedCellStyleForNumber(workbook);
            var dataFormat = workbook.CreateDataFormat();
            numberStyle.DataFormat = dataFormat.GetFormat("0.0000");

            var textStyle = Excel.CreateBorderedCellStyle(workbook);

            foreach (var fromCurrency in gcr)
            {
                var sheet = workbook.CreateSheet(fromCurrency.Key);

                var rowHeader = sheet.CreateRow(0);
                var headerCellName = Excel.CreateCell(rowHeader, 0, headerStyle);
                headerCellName.SetCellValue("Наименование");

                var headerCellCrossRate = Excel.CreateCell(rowHeader, 1, headerStyle);
                headerCellCrossRate.SetCellValue("Кросс-курс");

                int rowIndex = 1;
                foreach (var toCurrency in fromCurrency)
                {
                    var row = sheet.CreateRow(rowIndex);
                    var nameCell = Excel.CreateCell(row, 0, textStyle);
                    nameCell.SetCellValue($"{fromCurrency.Key}/{toCurrency.ToCurrency}");

                    var valueCell = Excel.CreateCell(row, 1, numberStyle);
                    valueCell.SetCellValue((double)toCurrency.CrossRate);

                    rowIndex++;
                }

                sheet.AutoSizeColumns([0, 1]);
            }

            using var fileStream = new FileStream($"{date:yyyyMMdd}.xlsx", FileMode.Create);
            workbook.Write(fileStream);
        }
    }
}
