using ConvertCSV.Helpers;
using ConvertCSV.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ConvertCSV.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
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
        


        public ICommand ShowCSVPath
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
                    LabelInfoVisibility = Visibility.Collapsed;
                    ProgressBarVisibility = Visibility.Visible;
                    ProgressBarValue = 0;
                    LoadCSVAsync();
                    Path = "";
                }, (obj) => !string.Equals(Path, null));
            }
        }

        private async void LoadCSVAsync() //Вызов асинхронного метода LoadCSV, для загрузки данных из csv файла
        {
            try
            {
                await Task.Run(() => CsvReader.LoadCSV(Path, SetProgressBarMaximum, AddProgressBarVvalue));
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
    }
}
