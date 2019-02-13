using ConvertCSV.Models;
using ConvertCSV.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConvertCSV.Helpers
{
    class XmlWriter
    {
        public static async void WriteXML(DBRepository DBRepository, string fileName, List<Person> persons) 
        {
            int n = persons.Count();

            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
            writer.WriteStartElement("TestProgram");
            for(int i = 0; i < n; i++)
            {
                City city = await DBRepository.GetCity(persons[i].CityId);
                writer.WriteStartElement("Record");
                writer.WriteAttributeString("id", (i + 1).ToString());
                writer.WriteStartElement("Date");
                writer.WriteString(persons[i].Date.ToShortDateString());
                writer.WriteEndElement();
                writer.WriteStartElement("FirstName");
                writer.WriteString(persons[i].Name);
                writer.WriteEndElement();
                writer.WriteStartElement("LastName");
                writer.WriteString(persons[i].Surname);
                writer.WriteEndElement();
                writer.WriteStartElement("SurName");
                writer.WriteString(persons[i].Patronymic);
                writer.WriteEndElement();
                writer.WriteStartElement("City");
                writer.WriteString(city.Name);
                writer.WriteEndElement();
                writer.WriteStartElement("Country");
                writer.WriteString((await DBRepository.GetCountry(city.CountryId)).Name);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
        }
    }
}
