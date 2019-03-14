using ConvertCSV.Helpers;
using ConvertCSV.Models;
using ConvertCSV.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ConvertCSV
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DBRepository DBRepository = new DBRepository();

       /* private async void ShowCreateExcel(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => LoadDataAsync());
            }
            catch
            {
                Change_LabelInfo("Page load error!", "#FFDE5454");
            }
        }*/

        private async void LoadDataAsync()  //Вызов асинхронного метода LoadData, для загрузки данных в фильтры
        {
            await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                LockInterface.Visibility = Visibility.Visible;
                CheckBoxCountry.UnSelectAll();
                CheckBoxCity.UnSelectAll();
                CheckBoxName.UnSelectAll();
                CheckBoxSurname.UnSelectAll();
                CheckBoxPatronymic.UnSelectAll();
                DateFromSearch.Text = null;
                DateToSearch.Text = null;
                Spinner.Visibility = Visibility.Visible;
            }));
            List<Person> persons = (await DBRepository.GetPerson()).ToList();
            List<City> cities = (await DBRepository.GetCity()).ToList();
            List<Country> countries = (await DBRepository.GetCountry()).ToList();
            List<string> personNames = new List<string>();
            List<string> personSurnames = new List<string>();
            List<string> personPatronymices = new List<string>();
            List<string> cityNames = new List<string>();
            List<string> countryNames = new List<string>();

            foreach (Person person in persons)
            {
                personNames.Add(person.Name);
                personSurnames.Add(person.Surname);
                personPatronymices.Add(person.Patronymic);
            }

            foreach (City city in cities)
            {
                cityNames.Add(city.Name);
            }

            foreach (Country country in countries)
            {
                countryNames.Add(country.Name);
            }
            personNames = personNames.Distinct().ToList();
            personSurnames = personSurnames.Distinct().ToList();
            personPatronymices = personPatronymices.Distinct().ToList();
            await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                CheckBoxCountry.ItemsSource = countryNames;
                CheckBoxCity.ItemsSource = cityNames;
                CheckBoxName.ItemsSource = personNames;
                CheckBoxSurname.ItemsSource = personSurnames;
                CheckBoxPatronymic.ItemsSource = personPatronymices;
                Spinner.Visibility = Visibility.Collapsed;
                LockInterface.Visibility = Visibility.Collapsed;
            }));
        }

       /* private void ButtonLoadCSV_Click(object sender, RoutedEventArgs e) ТУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУУТ
        {
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.Value = 0;
            LoadCSVAsync(CSVPath.Text, ProgressBar);
            ButtonLoadCSV.IsEnabled = false;
            CSVPath.Text = "";
        }

        private async void LoadCSVAsync(string path, ProgressBar progressBar) //Вызов асинхронного метода LoadCSV, для загрузки данных из csv файла
        {
            try
            {
                await Task.Run(() => CsvReader.LoadCSV(path, progressBar));
                Change_LabelInfo("Files successfully uploaded !", "#FF76D353");
            }
            catch                                                              
            {
                Change_LabelInfo("File upload error !", "#FFDE5454");
            }
        }  */

        private async void ShowDataGridView1(/*object sender, RoutedEventArgs e*/)
        {
            try
            {
                await Task.Run(() => LoadDataGrid());
               
            }
            catch
            {
                Change_LabelInfo("Error loading table !", "#FFDE5454");
            }
        }
        
        private async void LoadDataGrid()//Вызов асинхронного метода LoadDataGrid, для загрузки данных в таблицу данных
        {
            await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                LockInterface.Visibility = Visibility.Visible;
                Spinner.Visibility = Visibility.Visible;
            }));
            List<Person> persons = (await DBRepository.GetPerson()).ToList();
            List<RecordViewModel> modelesForDataGrid = new List<RecordViewModel>();
            foreach (Person person in persons)
            {
                City city = await DBRepository.GetCity(person.CityId);
                modelesForDataGrid.Add(new RecordViewModel(person.Date, person.Name, person.Surname, person.Patronymic, city.Name, (await DBRepository.GetCountry(city.CountryId)).Name));
            }
            await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                DataGrid.ItemsSource = modelesForDataGrid;
                Spinner.Visibility = Visibility.Collapsed;
                LockInterface.Visibility = Visibility.Collapsed;
            }));
        }

        private void ShowCSVPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "(*.csv)|*.csv";

            if (openFile.ShowDialog() == true)
            {
                CSVPath.Text = openFile.FileName;
                ButtonLoadCSV.IsEnabled = true;
            }
        }
        private async Task<List<Person>> SelectionPerson(List<string> names, List<string> surnames, List<string> patronymic, List<string> cityName, List<string> countryName, string dateFromSearch, string dateToSearch)
        {
            List<Person> persons = await DBRepository.GetPerson();

            if (names.Count != 0) persons = persons.Where(c => names.Any(h => string.Equals(h, c.Name))).ToList();
            if (surnames.Count != 0) persons = persons.Where(c => surnames.Any(h => string.Equals(h, c.Surname))).ToList();
            if (patronymic.Count != 0) persons = persons.Where(c => patronymic.Any(h => string.Equals(h, c.Patronymic))).ToList();
            if (cityName.Count != 0)
            {
                persons = persons.Where(c => cityName.Any(h => string.Equals(h, DBRepository.GetCity(c.CityId).Result.Name))).ToList();
            }
            if (countryName.Count != 0) persons = persons.Where(c => countryName.Any(h => string.Equals(h, DBRepository.GetCountry(DBRepository.GetCity(c.CityId).Result.CountryId).Result.Name))).ToList();
            if (!string.IsNullOrWhiteSpace(dateFromSearch))
            {
                DateTime dateFrom = DateTime.Parse(dateFromSearch);
                persons = persons.Where(c => c.Date >= dateFrom).ToList();
            }
            if (!string.IsNullOrWhiteSpace(dateToSearch))
            {
                DateTime dateTo = DateTime.Parse(dateToSearch);
                persons = persons.Where(c => c.Date <= dateTo).ToList();
            }
            return persons;
        }

        private async void ButtonSaveExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "(*.xlsx)|*.xlsx|(*.xml)|*.xml";
                saveFile.OverwritePrompt = false;
                List<string> countryName = new List<string>();
                List<string> personName = new List<string>();
                List<string> personSurname = new List<string>();
                List<string> personPatronymic = new List<string>();
                List<string> cityName = new List<string>();

                foreach (string c in CheckBoxName.SelectedItems)
                {
                    personName.Add(c);
                }
                foreach (string c in CheckBoxSurname.SelectedItems)
                {
                    personSurname.Add(c);
                }
                foreach (string c in CheckBoxPatronymic.SelectedItems)
                {
                    personPatronymic.Add(c);
                }
                foreach (string c in CheckBoxCity.SelectedItems)
                {
                    cityName.Add(c);
                }
                foreach (string c in CheckBoxCountry.SelectedItems)
                {
                    countryName.Add(c);
                }
                if (saveFile.ShowDialog() == true)
                {
                    string dateFromSearch = DateFromSearch.ToString();
                    string dateToSearch = DateToSearch.ToString();
                    List<Person> persons = await Task.Run(() => SelectionPerson(personName, personSurname, personPatronymic, cityName, countryName, dateFromSearch, dateToSearch));
                    if (saveFile.FilterIndex == 1)
                    {
                        ExcelWriter.WriteExcel(DBRepository, saveFile.FileName, persons);
                    }
                    else
                    {
                        XmlWriter.WriteXML(DBRepository, saveFile.FileName, persons);
                    }
                }
                Change_LabelInfo("The file is saved !", "#FF76D353");
            }
            catch
            {
                Change_LabelInfo("Error writing to file !", "#FFDE5454");
            }
                
        }

        private void Change_LabelInfo(string info, string colorHEX)
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            LabelInfo.Text = info;
            LabelInfo.Background = (Brush)new BrushConverter().ConvertFrom(colorHEX);
            LabelInfo.Visibility = Visibility.Visible;
        }

        private void DatePicker_SelectedDateFromChanged(object sender, SelectionChangedEventArgs e)
        {
            DateToSearch.DisplayDateStart = DateFromSearch.SelectedDate;
        }

        private void DatePicker_SelectedDateToChanged(object sender, SelectionChangedEventArgs e)
        {
            DateFromSearch.DisplayDateEnd = DateToSearch.SelectedDate;
        }
        
        private async void CheckBoxCountry_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            List<string> countriesName = new List<string>();
            List<string> cities = new List<string>();

            foreach (string c in CheckBoxCountry.SelectedItems)
            {
                countriesName.Add(c);
            }
            if(countriesName.Count != 0)
            {
                CheckBoxCity.IsEnabled = true;
                foreach (string s in countriesName)
                {
                    foreach (City city in (await DBRepository.GetCitiesFromCountry(s)))
                    {
                        cities.Add(city.Name);
                    }
                }
                CheckBoxCity.ItemsSource = cities;
            }
            else
            {
                CheckBoxCity.IsEnabled = false;
                CheckBoxCity.UnSelectAll();
            }
        }

        private async void ButtonShowDataGridPreview_Click(object sender, RoutedEventArgs e) //Показывает таблицу перед сохранением в Excel или XML
        {
            try
            {
                List<Person> persons = new List<Person>();
                List<string> countryName = new List<string>();
                List<string> personName = new List<string>();
                List<string> personSurname = new List<string>();
                List<string> personPatronymic = new List<string>();
                List<string> cityName = new List<string>();

                foreach (string c in CheckBoxName.SelectedItems)
                {
                    personName.Add(c);
                }
                foreach (string c in CheckBoxSurname.SelectedItems)
                {
                    personSurname.Add(c);
                }
                foreach (string c in CheckBoxPatronymic.SelectedItems)
                {
                    personPatronymic.Add(c);
                }
                foreach (string c in CheckBoxCity.SelectedItems)
                {
                    cityName.Add(c);
                }
                foreach (string c in CheckBoxCountry.SelectedItems)
                {
                    countryName.Add(c);
                }

                string dateFromSearch = DateFromSearch.ToString();
                string dateToSearch = DateToSearch.ToString();
                LockInterface.Visibility = Visibility.Visible;
                Spinner.Visibility = Visibility.Visible;
                persons = await Task.Run(() => SelectionPerson(personName, personSurname, personPatronymic, cityName, countryName, dateFromSearch, dateToSearch));
                LockInterface.Visibility = Visibility.Collapsed;
                Spinner.Visibility = Visibility.Collapsed;
                List<RecordViewModel> modelesForDataGrid = new List<RecordViewModel>();
                foreach (Person person in persons)
                {
                    City city = await DBRepository.GetCity(person.CityId);
                    modelesForDataGrid.Add(new RecordViewModel(person.Date, person.Name, person.Surname, person.Patronymic, city.Name, (await DBRepository.GetCountry(city.CountryId)).Name));
                }
                DataGridPreview.ItemsSource = modelesForDataGrid;
            }
            catch
            {
                Change_LabelInfo("Error loading table !", "#FFDE5454");
            }
            
        }

        private async void ButtonClearDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LockInterface.Visibility = Visibility.Visible;
                Spinner.Visibility = Visibility.Visible;
                await Task.Run(() => ClearDB());
                Spinner.Visibility = Visibility.Collapsed;
                LockInterface.Visibility = Visibility.Collapsed;
                Change_LabelInfo("Database successfully cleaned !", "#FF76D353");
            }
            catch                                                             
            {
                Change_LabelInfo("Database cleanup error !", "#FFDE5454");
            }
            
        }

        private void ClearDB()  //Полностью очищает базу данных
        {
            StarterTaskDbContext context = new StarterTaskDbContext();
            List<Person> order = context.Person.ToList();
            foreach (Person p in order)
            {
                context.Person.Remove(p);
                context.SaveChanges();
            }

            List<Country> country = context.Country.ToList();
            foreach (Country p in country)
            {
                context.Country.Remove(p);
                context.SaveChanges();
            }

            List<City> city = context.City.ToList();
            foreach (City p in city)
            {
                context.City.Remove(p);
                context.SaveChanges();
            }
        }

        private void GridMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TabControl.MinWidth = this.Width;
            TabControl.MinHeight = this.Height;
        }

        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {

             if (TabControl.SelectedIndex == 1)
             {
                 try
                 {
                     await Task.Run(() => LoadDataAsync());
                 }
                 catch
                 {
                     Change_LabelInfo("Page load error!", "#FFDE5454");
                 }
             }
             else if(TabControl.SelectedIndex == 2)
             {
                 try
                 {
                     await Task.Run(() => LoadDataGrid());

                 }
                 catch
                 {
                     Change_LabelInfo("Error loading table !", "#FFDE5454");
                 }
             }
         }
    }
}
