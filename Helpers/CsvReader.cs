using ConvertCSV.Models;
using ConvertCSV.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ConvertCSV.Helpers
{
    delegate void SetProgressBarMax(int max);
    delegate void AddProgressBar();
    
    class CsvReader
    {
        public static void LoadCSV(string Path, SetProgressBarMax SetProgressBarMax, AddProgressBar AddProgressBar)//передавать делегат вместо progressbar
        {
            DBRepository dBRepository = new DBRepository();
            string[] temp = File.ReadAllLines(Path, Encoding.Default);
            Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                SetProgressBarMax(temp.Count());
            }));
            List<Country> countries =  dBRepository.GetCountry().Result;
            List<City> cities =  dBRepository.GetCity().Result;
            foreach (string s in temp)
            {
                 Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    AddProgressBar();
                }));
                string[] words = s.Split(';');
                if (!( dBRepository.GetCountry().Result.Any(c => string.Equals(c.Name, words[5]))))
                {
                    Country country = new Country() { Name = words[5] };
                    dBRepository.db.Country.Add(country);
                    dBRepository.db.SaveChanges();
                    //MessageBox.Show($"Добавляю эту страну: {words[5]}");
                }
                if (!( dBRepository.GetCity().Result.Any(c => string.Equals(c.Name, words[4]))))
                {
                    City city = new City() { Name = words[4], CountryId =  dBRepository.GetCountryId(words[5]).Result };
                    dBRepository.db.City.Add(city);
                    dBRepository.db.SaveChanges();
                    //MessageBox.Show($"Добавляю эту страну: {words[4]}");
                }
                Person person = new Person() { Date = DateTime.Parse(words[0]), Name = words[1], Surname = words[2], Patronymic = words[3], CityId =  dBRepository.GetCityId(words[4]).Result };
                dBRepository.db.Person.Add(person);
                dBRepository.db.SaveChanges();
            }
        }
    }
}
