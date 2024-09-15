using ClassesWeb;
using OfficeOpenXml;

namespace Asp.Excel
{
    public class ExcelTenderService
    {
        private readonly string _folderPath;

        public ExcelTenderService(string folderPath)
        {
            _folderPath = folderPath;
        }

        public IEnumerable<string> GetExcelFiles()
        {
            return Directory.GetFiles(_folderPath, "*.xlsx").Select(Path.GetFileName);
        }

        public IEnumerable<Tender> GetTenders(string fileName)
        {
            var filePath = Path.Combine(_folderPath, fileName);
            var tenders = new List<Tender>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var columnMapping = GetColumnMapping(worksheet);

                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    var tender = new Tender
                    {
                        Title = worksheet.Cells[row, columnMapping["Название тендера"]].Text,
                        StartDate = DateTime.Parse(worksheet.Cells[row, columnMapping["Дата начала"]].Text),
                        EndDate = DateTime.Parse(worksheet.Cells[row, columnMapping["Дата окончания"]].Text),
                        Url = worksheet.Cells[row, columnMapping["URL тендерной площадки"]].Text
                    };
                    tenders.Add(tender);
                }
            }

            return tenders;
        }

        private Dictionary<string, int> GetColumnMapping(ExcelWorksheet worksheet)
        {
            var columnMapping = new Dictionary<string, int>();

            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
            {
                columnMapping[worksheet.Cells[1, col].Text] = col;
            }

            return columnMapping;
        }
    }

}
