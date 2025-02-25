using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Todo
{


    public partial class TaskCreation : Window, INotifyPropertyChanged
    {
        private string _timeText;
        private TimeSpan? _dueTime;
        private DateTime _taskDate = DateTime.Today;
        private string _selectedCategory;

        public string TimeText
        {
            get => _timeText;
            set
            {
                if (_timeText != value)
                {
                    _timeText = value;
                    OnPropertyChanged(nameof(TimeText));
                    ValidateAndSetTime();
                }
            }
        }

        public TimeSpan? DueTime
        {
            get => _dueTime;
            set
            {
                if (_dueTime != value)
                {
                    _dueTime = value;
                    OnPropertyChanged(nameof(DueTime));
                }
            }
        }

        public DateTime TaskDate
        {
            get => _taskDate;
            set
            {
                if (_taskDate != value)
                {
                    _taskDate = value;
                    OnPropertyChanged(nameof(TaskDate));
                }
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        private ObservableCollection<string> _categories = new ObservableCollection<string>();

        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public TaskCreation()
        {
            InitializeComponent();
            LoadCategories();
            DataContext = this; // Устанавливаем контекст данных
        }

        private void LoadCategories()
        {
            var categories = new List<string> { "Дом", "Работа", "Учёба", "Отдых" };
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }

        private void ValidateAndSetTime()
        {
            if (TimeSpan.TryParseExact(TimeText, @"hh\:mm", CultureInfo.InvariantCulture, out var time))
            {
                DueTime = time;
            }
            else
            {
                DueTime = null;
            }
        }

        private void Сreation_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTask.Text) || string.IsNullOrWhiteSpace(SelectedCategory) || DueTime == null)
            {
                MessageBox.Show("Заполните все поля перед созданием задачи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newTask = new TaskModel
            {
                Title = NameTask.Text,
                DueDate = TaskDate.Add(DueTime.Value),
                IsCompleted = false,
                Description = DescriptionTask.Text,
                Category = SelectedCategory
            };

            TaskCreated?.Invoke(this, newTask);
            this.Close();
        }

        private void Abolition_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<TaskModel> TaskCreated;
    }

}

