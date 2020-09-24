using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Text;
using Work.Classes;
using System.IO;
using ClosedXML.Excel;

namespace ClassLibraryExsel
{
    public class ClassExsel
    {
 
        private XLWorkbook workbook;
        private IXLWorksheet worksheet;
        Point p = new Point(2, 1);
        public ClassExsel(ICollection<Account> accounts)
        {
            CreateExsel();
            CreateTemplate();
            AddItemsInExsel(accounts);
        }

        private void CreateTemplate()
        {
            worksheet = workbook.Worksheets.Add("Звіт за " + DateTime.Now.Year.ToString());
            worksheet.Cell(p.Y, p.X).Value = "Сальдо на початок періоду";
            worksheet.Range(worksheet.Cell(p.Y, p.X++), worksheet.Cell(p.Y, p.X++)).Merge();
            worksheet.Cell(p.Y, p.X).Value = "Обороти за період";
            worksheet.Range(worksheet.Cell(p.Y, p.X++), worksheet.Cell(p.Y, p.X++)).Merge();
            worksheet.Cell(p.Y, p.X).Value = "Сальдо на кінець періоду";
            worksheet.Range(worksheet.Cell(p.Y, p.X++), worksheet.Cell(p.Y, p.X++)).Merge();
            p.X = 1;
            p.Y++;
            worksheet.Cell(p.Y, p.X++).Value = "Постачальники";
            for (int i = 0; i < 3; i++)
            {
                worksheet.Cell(p.Y, p.X++).Value = "Дебіт";
                worksheet.Cell(p.Y, p.X++).Value = "Кредит";
            }
            p.X = 1;
            p.Y++;
        }

        private void CreateExsel()
        {
            workbook = new XLWorkbook();
        }

        private void AddItemsInExsel(ICollection<Account> accounts)
        {
            foreach (var account in accounts)
            {
                worksheet.Cell(p.Y, p.X++).Value = account.name;
                worksheet.Cell(p.Y, p.X++).Value = account.debitStart;
                worksheet.Cell(p.Y, p.X++).Value = account.creditStart;
                worksheet.Cell(p.Y, p.X++).Value = account.debitPeroid;
                worksheet.Cell(p.Y, p.X++).Value = account.creditPeriod;
                worksheet.Cell(p.Y, p.X++).Value = account.debitEnd;
                worksheet.Cell(p.Y, p.X++).Value = account.creditEnd;
                p.X = 1;
                p.Y++;
            }
            worksheet.Columns("A", "H").Width=20;
            worksheet.Columns("A", "H").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Range("A1:G2").Style.Fill.BackgroundColor = XLColor.FromHtml("#A8E88D");
            Stream fs = new MemoryStream();
            worksheet.Range("A1:G2").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Range("A1:G2").Style.Border.BottomBorderColor = XLColor.FromHtml("#DBCB90");
            workbook.SaveAs(@"C:\Odz\Hello.xlsx");
        }

        
    }
}
