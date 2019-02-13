using ConvertCSV.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ConvertCSV.Services
{
    class DBRepository
    {
        public StarterTaskDbContext db = new StarterTaskDbContext();

        public async Task<List<Person>> GetPerson()
          => await db.Person.ToListAsync();

        public async Task<List<City>> GetCity()
          => await db.City.ToListAsync();

        public async Task<List<Country>> GetCountry()
          => await db.Country.ToListAsync();


        public async Task<int> GetCountryId(string name)
          => (await db.Country.Where(c => string.Equals(c.Name, name)).FirstAsync()).Id;


        public async Task<int> GetCityId(string name)
          => (await db.City.Where(c => string.Equals(c.Name, name)).FirstAsync()).Id;


        public async Task<City> GetCity(int id)
          => await db.City.Where(c => c.Id == id).FirstAsync();

        public async Task<City> GetCity(string name)
          => await db.City.Where(c => string.Equals(c.Name, name)).FirstAsync();


        public async Task<List<City>> GetCitiesFromCountry(string countryName)
          => (await GetCity()).Where(c => string.Equals(c.Country.Name, countryName)).ToList();


        public async Task<Country> GetCountry(int id)
          => await db.Country.Where(c => c.Id == id).FirstAsync();


        public async Task<Country> GetCountry(string cityName)
          => (await GetCity(cityName)).Country;
    }
}
