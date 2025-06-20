using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Solid.TestTask.Helpers
{
    public static class ExcelHelpers
    {
        public static ICellStyle CreateBorderedCellStyleForNumber(XSSFWorkbook workbook)
        {
            var numberStyle = CreateBorderedCellStyle(workbook);
            var dataFormat = workbook.CreateDataFormat();
            numberStyle.DataFormat = dataFormat.GetFormat("0.0000");

            return numberStyle;
        }

        public static ICellStyle CreateBorderedCellStyle(XSSFWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.WrapText = false;

            return style;
        }
    }
}
