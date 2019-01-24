using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ConvertCSV.Services
{
    class DBRepository
    {
        public static StarterTaskDbContext db = new StarterTaskDbContext();

        public static StarterTaskDbContext GetDataBase()
        {
            return db;
        }

        public static List<Person> GetPerson()
        {
            return db.Person.ToList();
        }

        public static List<City> GetCity()
        {
            return db.City.ToList();
        }

        public static List<Country> GetCountry()
        {
            return db.Country.ToList();
        }

        public static int GetCountryId(string name)
        {
            return db.Country.ToList().Where(c => string.Equals(c.Name, name)).First().Id;
        }

        public static int GetCityId(string name)
        {
            return db.City.ToList().Where(c => string.Equals(c.Name, name)).First().Id;
        }

        public static City GetCity(int id)
        {
            return db.City.ToList().Where(c => c.Id == id).First();
        }

        public static City GetCity(string name)
        {
            return db.City.ToList().Where(c => string.Equals(c.Name, name)).First();
        }

        public static List<City> GetCitiesFromCountry(string countryName)
        {            
            return GetCity().Where(c => string.Equals(c.Country.Name, countryName)).ToList();
        }

        public static Country GetCountry(int id)
        {
            return db.Country.ToList().Where(c => c.Id == id).First();
        }

        public static Country GetCountry(string cityName)
        {
            return GetCity(cityName).Country;
        }

        public static void LoadCSV(string Path, ProgressBar progressBar)
        {
            string[] temp = File.ReadAllLines(Path, System.Text.Encoding.Default);
            Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                progressBar.Maximum = temp.Count(); }));
            foreach (string s in temp)
            {
                Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    progressBar.Value++;
                }));
                string[] words = s.Split(';');
                if (!GetCountry().Any(c => string.Equals(c.Name, words[5])))
                {
                    Country country = new Country() { Name = words[5] };
                    db.Country.Add(country);
                    db.SaveChanges();
                    //MessageBox.Show($"Добавляю эту страну: {words[5]}");
                }
                if (!GetCity().Any(c => string.Equals(c.Name, words[4])))
                {
                    City city = new City() { Name = words[4], CountryId = GetCountryId(words[5]) };
                    db.City.Add(city);
                    db.SaveChanges();
                    //MessageBox.Show($"Добавляю эту страну: {words[4]}");
                }
                Person person = new Person() { Date = DateTime.Parse(words[0]), Name = words[1], Surname = words[2], Patronymic = words[3], CityId = GetCityId(words[4]) };
                db.Person.Add(person);
                db.SaveChanges();
            }
        }
    }
}
