using ConvertCSV.Helpers;
using ConvertCSV.Models;
using ConvertCSV.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ConvertCSV.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        DBRepository DBRepository = new DBRepository();

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                OnPropertyChanged();
            }
        }

        private string _CheckBoxNameSelectedValue;
        public string CheckBoxNameSelectedValue
        {
            get { return _CheckBoxNameSelectedValue; }
            set
            {
                _CheckBoxNameSelectedValue = value;
                OnPropertyChanged();
            }
        }

        private string _CheckBoxSurnameSelectedValue;
        public string CheckBoxSurnameSelectedValue
        {
            get { return _CheckBoxSurnameSelectedValue; }
            set
            {
                _CheckBoxSurnameSelectedValue = value;
                OnPropertyChanged();
            }
        }

        private string _CheckBoxPatronymicSelectedValue;
        public string CheckBoxPatronymicSelectedValue
        {
            get { return _CheckBoxPatronymicSelectedValue; }
            set
            {
                _CheckBoxPatronymicSelectedValue = value;
                OnPropertyChanged();
            }
        }

        private string _CheckBoxCountrySelectedValue;
        public string CheckBoxCountrySelectedValue
        {
            get { return _CheckBoxCountrySelectedValue; }
            set
            {
                _CheckBoxCountrySelectedValue = value;
                OnPropertyChanged();
            }
        }

        private string _CheckBoxCitySelectedValue;
        public string CheckBoxCitySelectedValue
        {
            get { return _CheckBoxCitySelectedValue; }
            set
            {
                _CheckBoxCitySelectedValue = value;
                OnPropertyChanged();
            }
        }

        private List<string> _CheckBoxNameItemsSource;
        public List<string> CheckBoxNameItemsSource
        {
            get { return _CheckBoxNameItemsSource; }
            set
            {
                _CheckBoxNameItemsSource = value;
                OnPropertyChanged();
            }
        }

        private List<string> _CheckBoxSurnameItemsSource;
        public List<string> CheckBoxSurnameItemsSource
        {
            get { return _CheckBoxSurnameItemsSource; }
            set
            {
                _CheckBoxSurnameItemsSource = value;
                OnPropertyChanged();
            }
        }

        private List<string> _CheckBoxPatronymicItemsSource;
        public List<string> CheckBoxPatronymicItemsSource
        {
            get { return _CheckBoxPatronymicItemsSource; }
            set
            {
                _CheckBoxPatronymicItemsSource = value;
                OnPropertyChanged();
            }
        }

        private List<string> _CheckBoxCityItemsSource;
        public List<string> CheckBoxCityItemsSource
        {
            get { return _CheckBoxCityItemsSource; }
            set
            {
                _CheckBoxCityItemsSource = value;
                OnPropertyChanged();
            }
        }

        private List<string> _CheckBoxCountryItemsSource;
        public List<string> CheckBoxCountryItemsSource
        {
            get { return _CheckBoxCountryItemsSource; }
            set
            {
                _CheckBoxCountryItemsSource = value;
                OnPropertyChanged();
            }
        }

        private DateTime _DateFromSearch = DateTime.Today;
        public DateTime DateFromSearch
        {
            get { return _DateFromSearch; }
            set
            {
                _DateFromSearch = value;
                OnPropertyChanged();
            }
        }

        private DateTime _DateToSearch = DateTime.Today;
        public DateTime DateToSearch
        {
            get { return _DateToSearch; }
            set
            {
                _DateToSearch = value;
                OnPropertyChanged();
            }
        }

        private int _ProgressBarValue;
        public int ProgressBarValue
        {
            get { return _ProgressBarValue; }
            set
            {
                _ProgressBarValue = value;
                OnPropertyChanged();
            }
        }

        private int _ProgressBarMaximum;
        public int ProgressBarMaximum
        {
            get { return _ProgressBarMaximum; }
            set
            {
                _ProgressBarMaximum = value;
                OnPropertyChanged();
            }
        }

        private bool _ButtonLoadCsvEnabled = false;
        public bool ButtonLoadCsvEnabled
        {
            get { return _ButtonLoadCsvEnabled; }
            set
            {
                _ButtonLoadCsvEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _ButtonSaveExcelEnabled = true;
        public bool ButtonSaveExcelEnabled
        {
            get { return _ButtonSaveExcelEnabled; }
            set
            {
                _ButtonSaveExcelEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool _ButtonClearDBEnabled = true;
        public bool ButtonClearDBEnabled
        {
            get { return _ButtonClearDBEnabled; }
            set
            {
                _ButtonClearDBEnabled = value;
                OnPropertyChanged();
            }
        }

        


        private int _TabControlSelectedIndex;
        public int TabControlSelectedIndex
        {
            get { return _TabControlSelectedIndex; }
            set
            {
                _TabControlSelectedIndex = value;
                OnPropertyChanged();
            }
        }
        
        private Visibility _ProgressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get { return _ProgressBarVisibility; }
            set
            {
                _ProgressBarVisibility = value;
                OnPropertyChanged();
            }
        }

        private List<RecordViewModel> _DataGridItemSourse;
        public List<RecordViewModel> DataGridItemSourse
        {
            get { return _DataGridItemSourse; }
            set
            {
                _DataGridItemSourse = value;
                OnPropertyChanged();
            }
        }

        private List<RecordViewModel> _DataGridPreviewItemSourse;
        public List<RecordViewModel> DataGridPreviewItemSourse
        {
            get { return _DataGridPreviewItemSourse; }
            set
            {
                _DataGridPreviewItemSourse = value;
                OnPropertyChanged();
            }
        }

        private Visibility _LockInterfaceVisibility = Visibility.Collapsed;
        public Visibility LockInterfaceVisibility
        {
            get { return _LockInterfaceVisibility; }
            set
            {
                _LockInterfaceVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _SpinnerVisibility = Visibility.Collapsed;
        public Visibility SpinnerVisibility
        {
            get { return _SpinnerVisibility; }
            set
            {
                _SpinnerVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _LabelInfoText;
        public string LabelInfoText
        {
            get { return _LabelInfoText; }
            set
            {
                _LabelInfoText = value;
                OnPropertyChanged();
            }
        }

        private Visibility _LabelInfoVisibility;
        public Visibility LabelInfoVisibility
        {
            get { return _LabelInfoVisibility; }
            set
            {
                _LabelInfoVisibility = value;
                OnPropertyChanged();
            }
        }

        private Brush _LabelInfoBackground;
        public Brush LabelInfoBackground
        {
            get { return _LabelInfoBackground; }
            set
            {
                _LabelInfoBackground = value;
                OnPropertyChanged();
            }
        }


        public ICommand ShowCSVPath //Обработчик нажжатия на кнопку Browse на вкладке Load CSV
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "(*.csv)|*.csv";

                    if (openFile.ShowDialog() == true)
                    {
                        Path = openFile.FileName;
                        ButtonLoadCsvEnabled = true;
                    }
                });
            }
        }  

        public ICommand ButtonShowDataGridPreview_Click //Обработчик нажжатия на кнопку View на вкладке Create File
        {
            get
            {
                return new DelegateCommand(async(obj) =>
                {
                    try
                    {
                        List<Person> persons = new List<Person>();
                        List<string> countryName = new List<string>();
                        List<string> personName = new List<string>();
                        List<string> personSurname = new List<string>();
                        List<string> personPatronymic = new List<string>();
                        List<string> cityName = new List<string>();

                        foreach (string c in CheckBoxNameSelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            personName.Add(c);
                        }
                        foreach (string c in CheckBoxSurnameSelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            personSurname.Add(c);
                        }
                        foreach (string c in CheckBoxPatronymicSelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            personPatronymic.Add(c);
                        }
                        foreach (string c in CheckBoxCitySelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            cityName.Add(c);
                        }
                        foreach (string c in CheckBoxCountrySelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            countryName.Add(c);
                        }

                        string dateFromSearch = DateFromSearch.ToString();
                        string dateToSearch = DateToSearch.ToString();
                        LockInterfaceVisibility = Visibility.Visible;
                        SpinnerVisibility = Visibility.Visible;
                        persons = await Task.Run(() => SelectionPerson(personName, personSurname, personPatronymic, cityName, countryName, dateFromSearch, dateToSearch));
                        List<RecordViewModel> modelesForDataGrid = new List<RecordViewModel>();
                        foreach (Person person in persons)
                        {
                            City city = await DBRepository.GetCity(person.CityId);
                            modelesForDataGrid.Add(new RecordViewModel(person.Date, person.Name, person.Surname, person.Patronymic, city.Name, (await DBRepository.GetCountry(city.CountryId)).Name));
                        }
                        DataGridPreviewItemSourse = modelesForDataGrid;
                        LockInterfaceVisibility = Visibility.Collapsed;
                        SpinnerVisibility = Visibility.Collapsed;
                    }
                    catch
                    {
                        Change_LabelInfo("Error loading table !", "#FFDE5454");
                    }
                });
            }
        }

        public ICommand ButtonSaveExcel_Click
        {
            get
            {
                return new DelegateCommand(async(obj) =>
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

                        foreach (string c in CheckBoxNameSelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            personName.Add(c);
                        }
                        foreach (string c in CheckBoxSurnameSelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            personSurname.Add(c);
                        }
                        foreach (string c in CheckBoxPatronymicSelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            personPatronymic.Add(c);
                        }
                        foreach (string c in CheckBoxCitySelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
                        {
                            cityName.Add(c);
                        }
                        foreach (string c in CheckBoxCountrySelectedValue.Split(',').Where(c => !string.Equals(c, "")).ToList())
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
                });
            }
        }

        public void SetProgressBarMaximum(int max)
        {
            ProgressBarMaximum = max;
        }

        public void AddProgressBarVvalue()
        {
            ProgressBarValue++;
        }

        public ICommand ClickAdd
        {
            get
            {
                return new DelegateCommand((obj) =>
                {
                    try
                    {
                        LabelInfoVisibility = Visibility.Collapsed;
                        ProgressBarVisibility = Visibility.Visible;
                        ProgressBarValue = 0;
                        LoadCSVAsync();
                        Path = "";
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }, (obj) => !string.Equals(Path, ""));
            }
        }

        private async void LoadCSVAsync() //Вызов асинхронного метода LoadCSV, для загрузки данных из csv файла
        {
            try
            {
                ButtonClearDBEnabled = false;
                await Task.Run(() => CsvReader.LoadCSV(Path, SetProgressBarMaximum, AddProgressBarVvalue));
                ButtonClearDBEnabled = true;
                Change_LabelInfo("Files successfully uploaded !", "#FF76D353");
            }
            catch
            {
                Change_LabelInfo("File upload error !", "#FFDE5454");
            }
        }

        private void Change_LabelInfo(string info, string colorHEX)
        {
            ProgressBarVisibility = Visibility.Collapsed;
            LabelInfoText = info;
            LabelInfoBackground = (Brush)new BrushConverter().ConvertFrom(colorHEX);
            LabelInfoVisibility = Visibility.Visible;
        }

        private async void LoadDataGrid()//Вызов асинхронного метода LoadDataGrid, для загрузки данных в таблицу данных
        {
            await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                LockInterfaceVisibility = Visibility.Visible;
                SpinnerVisibility = Visibility.Visible;
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
                DataGridItemSourse = modelesForDataGrid;
                SpinnerVisibility = Visibility.Collapsed;
                LockInterfaceVisibility = Visibility.Collapsed;
            }));
        }


        public ICommand TabControl_SelectionChanged
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    if (TabControlSelectedIndex == 1)
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
                    else if (TabControlSelectedIndex == 2)
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
                });
            }
        }

        private async void LoadDataAsync()  //Вызов асинхронного метода LoadData, для загрузки данных в фильтры
        {
            await Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                LockInterfaceVisibility = Visibility.Visible;
             /*   CheckBoxCountry.UnSelectAll();
                CheckBoxCity.UnSelectAll();
                CheckBoxName.UnSelectAll();
                CheckBoxSurname.UnSelectAll();
                CheckBoxPatronymic.UnSelectAll();*/
               // DateFromSearch.Text = null;
               // DateToSearch.Text = null;
                SpinnerVisibility = Visibility.Visible;
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
                CheckBoxCountryItemsSource = countryNames;
                CheckBoxCityItemsSource = cityNames;
                CheckBoxNameItemsSource = personNames;
                CheckBoxSurnameItemsSource = personSurnames;
                CheckBoxPatronymicItemsSource = personPatronymices;
                SpinnerVisibility = Visibility.Collapsed;
                LockInterfaceVisibility = Visibility.Collapsed;
            }));
        }

        public ICommand ButtonClearDB_Click
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    try
                    {
                        LockInterfaceVisibility = Visibility.Visible;
                        SpinnerVisibility = Visibility.Visible;
                        await Task.Run(() => ClearDB());
                        SpinnerVisibility = Visibility.Collapsed;
                        LockInterfaceVisibility = Visibility.Collapsed;
                        Change_LabelInfo("Database successfully cleaned !", "#FF76D353");
                    }
                    catch
                    {
                        Change_LabelInfo("Database cleanup error !", "#FFDE5454");
                    }
                });
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

        public ICommand CheckBoxCity_Opened
        {
            get
            {
                return new DelegateCommand(async (obj) =>
                {
                    List<string> countriesName = new List<string>();
                    List<string> cities = new List<string>();
                    foreach (string c in CheckBoxCountrySelectedValue.Split(',').ToList())
                    {
                        countriesName.Add(c);
                    }
                    if (countriesName.Count != 0)
                    {
                        foreach (string s in countriesName)
                        {
                            foreach (City city in (await DBRepository.GetCitiesFromCountry(s)))
                            {
                                cities.Add(city.Name);
                            }
                        }
                        CheckBoxCityItemsSource = cities;
                    }
                });
            }
        }
    }
}
