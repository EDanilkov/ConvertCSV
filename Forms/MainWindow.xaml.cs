using ConvertCSV.Helpers;
using ConvertCSV.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ConvertCSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DBRepository DBRepository = new DBRepository();

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ShowCreateExcel(object sender, RoutedEventArgs e)
        {
            ListViewSettings.UnselectAll();
            Settings.Visibility = Visibility.Collapsed;
            LoadCSV.Visibility = Visibility.Collapsed;
            DataGridView.Visibility = Visibility.Collapsed;
            CreateExcel.Visibility = Visibility.Visible;
            List<Person> persons = DBRepository.GetPerson().ToList();
            List<City> cities = DBRepository.GetCity().ToList();
            List<Country> countries = DBRepository.GetCountry().ToList();
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

            foreach(City city in cities)
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

            CheckBoxCountry.UnSelectAll();
            CheckBoxCity.UnSelectAll();
            CheckBoxName.UnSelectAll();
            CheckBoxSurname.UnSelectAll();
            CheckBoxPatronymic.UnSelectAll();
            CheckBoxCountry.ItemsSource = countryNames;
            CheckBoxCity.ItemsSource = cityNames;
            CheckBoxName.ItemsSource = personNames;
            CheckBoxSurname.ItemsSource = personSurnames;
            CheckBoxPatronymic.ItemsSource = personPatronymices;
            DateFromSearch.Text = null;
            DateToSearch.Text = null;
        }

        private void ShowLoadCSV(object sender, RoutedEventArgs e)
        {
            ListViewSettings.UnselectAll();
            Settings.Visibility = Visibility.Collapsed;
            LoadCSV.Visibility = Visibility.Visible;
            CreateExcel.Visibility = Visibility.Collapsed;
            DataGridView.Visibility = Visibility.Collapsed;
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            ListView.UnselectAll();
            Settings.Visibility = Visibility.Visible;
            LoadCSV.Visibility = Visibility.Collapsed;
            CreateExcel.Visibility = Visibility.Collapsed;
            DataGridView.Visibility = Visibility.Collapsed;
        }

        private void ButtonLoadCSV_Click(object sender, RoutedEventArgs e)
        {
            //DBRepository.LoadCSV(CSVPath.Text);
            ProgressBar.Visibility = Visibility.Visible;
            ProgressBar.Value = 0;
            LoadCSVAsync(CSVPath.Text, ProgressBar);
        }

        private async void LoadCSVAsync(string path, ProgressBar progressBar)
        {
            try
            {
                DBRepository DB = new DBRepository();
                await Task.Run(() => DB.LoadCSV(path, progressBar));
                Change_LabelInfo("Файлы успешно загружены !", "#FF76D353");
            }
            catch                                                              //ЭТО НУЖНО ????
            {
                Change_LabelInfo("Ошибка загрузки файлов !", "#FFDE5454");
            }
        }

        private void ShowDataGridView(object sender, RoutedEventArgs e)
        {
            ListViewSettings.UnselectAll();
            List<Person> persons = DBRepository.GetPerson();
            List<ModelForDataGrid> modelesForDataGrid = new List<ModelForDataGrid>();
            foreach (Person person in persons)
            {
                City city = DBRepository.GetCity(person.CityId);
                modelesForDataGrid.Add(new ModelForDataGrid(person.Date, person.Name, person.Surname, person.Patronymic, city.Name, DBRepository.GetCountry(city.CountryId).Name));
            }
            Settings.Visibility = Visibility.Collapsed;
            LoadCSV.Visibility = Visibility.Collapsed;
            CreateExcel.Visibility = Visibility.Collapsed;
            DataGrid.ItemsSource = modelesForDataGrid;
            DataGridView.Visibility = Visibility.Visible;
           
        }

        private void ShowCSVPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "(*.csv)|*.csv";

            if (openFile.ShowDialog() == true)
            {
                CSVPath.Text = openFile.FileName;
            }
        }
        private List<Person> SelectionPerson(List<string> names, List<string> surnames, List<string> patronymic, List<string> cityName, List<string> countryName, string dateFromSearch, string dateToSearch)
        {
            List<Person> persons = DBRepository.GetPerson().ToList();

            if (names.Count != 0) persons = persons.Where(c => names.Any(h => string.Equals(h, c.Name))).ToList();
            if (surnames.Count != 0) persons = persons.Where(c => surnames.Any(h => string.Equals(h, c.Surname))).ToList();
            if (patronymic.Count != 0) persons = persons.Where(c => patronymic.Any(h => string.Equals(h, c.Patronymic))).ToList();
            if (cityName.Count != 0) persons = persons.Where(c => cityName.Any(h => string.Equals(h, DBRepository.GetCity(c.CityId).Name))).ToList();
            if (countryName.Count != 0) persons = persons.Where(c => countryName.Any(h => string.Equals(h, DBRepository.GetCountry(DBRepository.GetCity(c.CityId).CountryId).Name))).ToList();
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

        private void ButtonSaveExcel_Click(object sender, RoutedEventArgs e)
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
                if(saveFile.FilterIndex == 1)
                {
                    ExcelWriter.WriteExcel(DBRepository, saveFile.FileName, SelectionPerson(personName, personSurname, personPatronymic, cityName, countryName, DateFromSearch.Text, DateToSearch.Text));
                }
                else
                {
                    XmlWriter.WriteXML(DBRepository, saveFile.FileName, SelectionPerson(personName, personSurname, personPatronymic, cityName, countryName, DateFromSearch.Text, DateToSearch.Text));
                }
            }
            Change_LabelInfo("Файл успешно сохранён !", "#FF76D353");
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



        private void CheckBoxCountry_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
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
                    foreach (City city in DBRepository.GetCitiesFromCountry(s))
                    {
                        cities.Add(city.Name);
                    }
                }
                CheckBoxCity.ItemsSource = cities;
            }
            else
            {
                CheckBoxCity.IsEnabled = false;
            }
        }

        private void ButtonShowDataGridPreview_Click(object sender, RoutedEventArgs e)
        {
            List<Person> persons = DBRepository.GetPerson().ToList();
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
            persons = SelectionPerson(personName, personSurname, personPatronymic, cityName, countryName, DateFromSearch.ToString(), DateToSearch.ToString());
            
            List<ModelForDataGrid> modelesForDataGrid = new List<ModelForDataGrid>();
            foreach (Person person in persons)
            {
                City city = DBRepository.GetCity(person.CityId);
                modelesForDataGrid.Add(new ModelForDataGrid(person.Date, person.Name, person.Surname, person.Patronymic, city.Name, DBRepository.GetCountry(city.CountryId).Name));
            }
            DataGridPreview.ItemsSource = modelesForDataGrid;
        }

        private void ClearDB(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (ListViewItem ls in ListView.Items)
                {
                    ls.IsSelected = false;
                }

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
                Change_LabelInfo("База данных успешно очищена !", "#FF76D353");
            }
            catch                                                              //ЭТО НУЖНО ????
            {
                Change_LabelInfo("Ошибка очистки базы данных !", "#FFDE5454");
            }
            
        }
    }
}
