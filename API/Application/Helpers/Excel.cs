using ClosedXML.Excel;
using System.Collections.Generic;

namespace API.Application.Helpers
{
    public class Excel
    {
        IXLWorkbook workbook;
        List<IXLWorksheet> worksheets = new List<IXLWorksheet>();
        List<string> worksheetNames = new List<string>();
        public Excel()
        {

        }
        public void CreateNewFile()
        {
            workbook = new XLWorkbook();
        }
        public void CreateWorkSheet(string worksheetName)
        {            
            worksheets.Add(workbook.AddWorksheet(worksheetName));

            worksheetNames.Add(worksheetName);
        }
        public void WriteToCell(int row, int column, string data, string worksheetName)
        {
            row++;
            column++;
            int index = worksheetNames.IndexOf(worksheetName);

            worksheets[index].Cell(row, column).Value = data;
        }
        public void SaveAs(string path) => workbook.SaveAs(path);
    }
}