using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertCSV
{
    class ModelForDataGrid
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

        public ModelForDataGrid(DateTime date, string name, string surname, string patronymic, string cityName, string countryName)
        {
            Date = date;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            CityName = cityName;
            CountryName = countryName;
        }
    }
}
