using ConvertCSV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertCSV.Helpers
{
    class ExcelWriter
    {
        public static void WriteExcel(string fileName)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workBook;
            Microsoft.Office.Interop.Excel.Worksheet workSheet;
            excelApp.SheetsInNewWorkbook = 3;

            List<Person> persons = DBRepository.GetPerson().ToList();

            
            //persons.Where(c => DBRepository.GetCountry(c.CityId).Name == )

            workBook = excelApp.Workbooks.Add();
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            workSheet.Name = "Данные с CSV";

            workSheet.Cells[1, 1] = "Дата";
            workSheet.Cells[1, 2] = "Имя";
            workSheet.Cells[1, 3] = "Фамилия";
            workSheet.Cells[1, 4] = "Отчество";
            workSheet.Cells[1, 5] = "Город";
            workSheet.Cells[1, 6] = "Страна";
            int i = 2;
            foreach(Person person in persons)
            {
                workSheet.Cells[i, 1] = person.Date.ToString();
                workSheet.Cells[i, 2] = person.Name;
                workSheet.Cells[i, 3] = person.Surname;
                workSheet.Cells[i, 4] = person.Patronymic;
                workSheet.Cells[i, 5] = DBRepository.GetCity(person.CityId).Name;
                workSheet.Cells[i++, 6] = DBRepository.GetCountry(DBRepository.GetCity(person.CityId).CountryId).Name;
            }


            workBook.SaveAs(fileName);
            excelApp.Quit();



            
        }
    }
}
