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

            var headerStyle = ExcelHelpers.CreateBorderedCellStyle(workbook);
            headerStyle.SetFont(font);

            var numberStyle = ExcelHelpers.CreateBorderedCellStyleForNumber(workbook);
            var dataFormat = workbook.CreateDataFormat();
            numberStyle.DataFormat = dataFormat.GetFormat("0.0000");

            var textStyle = ExcelHelpers.CreateBorderedCellStyle(workbook);

            foreach (var fromCurrency in gcr)
            {
                var sheet = workbook.CreateSheet(fromCurrency.Key);

                var rowHeader = sheet.CreateRow(0);
                var headerCell1 = rowHeader.CreateCell(0);
                headerCell1.SetCellValue("Наименование");
                headerCell1.CellStyle = headerStyle;

                var headerCell2 = rowHeader.CreateCell(1);
                headerCell2.SetCellValue("Кросс-курс");
                headerCell2.CellStyle = headerStyle;

                int rowIndex = 1;
                foreach (var toCurrency in fromCurrency)
                {
                    var row = sheet.CreateRow(rowIndex);
                    var nameCell = row.CreateCell(0);
                    nameCell.SetCellValue($"{fromCurrency.Key}/{toCurrency.ToCurrency}");
                    nameCell.CellStyle = textStyle;

                    var valueCell = row.CreateCell(1);
                    valueCell.SetCellValue((double)toCurrency.CrossRate);
                    valueCell.CellStyle = numberStyle;

                    rowIndex++;
                }

                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
            }

            using var fileStream = new FileStream($"{date:yyyyMMdd}.xlsx", FileMode.Create);
            workbook.Write(fileStream);
        }
    }
}
