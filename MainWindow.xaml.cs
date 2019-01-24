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

            CheckBoxCountry.ItemsSource = countryNames;
            CheckBoxCity.ItemsSource = cityNames;
            CheckBoxName.ItemsSource = personNames;
            CheckBoxSurname.ItemsSource = personSurnames;
            CheckBoxPatronymic.ItemsSource = personPatronymices;
           
        }

        private void ShowLoadCSV(object sender, RoutedEventArgs e)
        {
            foreach (ListViewItem ls in ListViewSettings.Items) //УДАЛИТЬ (Убирает Select с Setting)
            {
                ls.IsSelected = false;
            }
            LoadCSV.Visibility = Visibility.Visible;
            CreateExcel.Visibility = Visibility.Collapsed;
            DataGridView.Visibility = Visibility.Collapsed;
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            foreach(ListViewItem ls in ListView.Items)
            {
                ls.IsSelected = false;
            }
            LoadCSV.Visibility = Visibility.Collapsed;
            CreateExcel.Visibility = Visibility.Collapsed;
            DataGridView.Visibility = Visibility.Collapsed;

            StarterTaskDbContext context = new StarterTaskDbContext();
            List<Person> order = context.Person.ToList();
            foreach(Person p in order)
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

        private void ButtonLoadCSV_Click(object sender, RoutedEventArgs e)
        {
            //DBRepository.LoadCSV(CSVPath.Text);
            LoadCSVAsync(CSVPath.Text, ProgressBar);
        }

        private async void LoadCSVAsync(string path, ProgressBar progressBar)
        {
            await Task.Run(() => DBRepository.LoadCSV(path, progressBar));
        }

        private void ShowDataGridView(object sender, RoutedEventArgs e)
        {
            LoadCSV.Visibility = Visibility.Collapsed;
            CreateExcel.Visibility = Visibility.Collapsed;
            DataGrid.ItemsSource = DBRepository.GetPerson();
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

        private void ButtonSaveExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "(*.xlsx)|*.xlsx|Все файлы (*.*)|*.* ";
            saveFile.OverwritePrompt = false;
            List<string> countryName = new List<string>();
            List<string> cityName = new List<string>();
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
                ExcelWriter.WriteExcel(saveFile.FileName);
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dtpToSearch.DisplayDateStart = dtpFromSearch.SelectedDate;
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

        /*private void ListViewCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ListViewCountry_SelectionChanged(object sender, SelectionChangedEventArgs e) // ДОДЕЛАТЬ!
        {

        }*/
    }
}
