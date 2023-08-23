using ClosedXML.Excel;
using System.IO;
namespace TrainingBE.Service
{
    public class ExcelService : IExcelService
    {
        public byte[] ExportToExcel<T>(List<T> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Lặp qua các thuộc tính của DTO để tạo header
                var properties = typeof(T).GetProperties();
                var headerRow = worksheet.Row(1);
                for (int i = 0; i < properties.Length; i++)
                {
                    var headerCell = headerRow.Cell(i + 1);
                    headerCell.Value = properties[i].Name.ToUpper();
                    headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    headerCell.Style.Font.Italic = true;
                    headerCell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    headerCell.Style.Fill.BackgroundColor = XLColor.Beige;
                    headerCell.Style.Font.SetBold(true);
                }
                for (int row = 1; row <= data.Count; row++)
                {
                    var currentRow = worksheet.Row(row + 1);
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var cell = currentRow.Cell(col + 1);
                        cell.Value = XLCellValue.FromObject(properties[col].GetValue(data[row - 1]));
                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        if (row % 2 == 1)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                    }
                    
                }

                // Ghi workbook vào stream và trả về dữ liệu Excel
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
