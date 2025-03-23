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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Todo.Entities;

namespace Todo.View
{


    public partial class TaskCreation : Page, INotifyPropertyChanged
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
            DataContext = this;


        }

        private void LoadCategories()
        {
            var categories = new List<string> { "Дом", "Работа", "Учёба", "Отдых" };
            foreach (var category in categories)
            {
                Categories.Add(category);
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CategoryComboBox != null)
            {
                CategoryComboBox.IsDropDownOpen = !CategoryComboBox.IsDropDownOpen;
                e.Handled = true;
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
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            fadeOut.Completed += (s, e1) =>
            {
                // После завершения анимации, переходим на страницу Main
                Main mainPage = new Main();
                NavigationService.Navigate(mainPage);

                // Анимация появления страницы Main
                DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                mainPage.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);

        }



        private void Abolition_Button_Click(object sender, RoutedEventArgs e)
        {
            // Создаем анимацию для исчезновения текущей страницы
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));

            fadeOut.Completed += (s, e1) =>
            {
                // Условие для выбора страницы
                var mainPage = new Main();
                if (mainPage.Tasks.Count > 0)
                {
                    // Переход на страницу Main
                    NavigationService.Navigate(mainPage);

                    // Анимация появления страницы Main
                    DoubleAnimation fadeInMain = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                    mainPage.BeginAnimation(UIElement.OpacityProperty, fadeInMain);
                }
                else
                {
                    // Переход на страницу Main_empty
                    var mainEmptyPage = new Main_empty();
                    NavigationService.Navigate(mainEmptyPage);

                    // Анимация появления страницы Main_empty
                    DoubleAnimation fadeInEmpty = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                    mainEmptyPage.BeginAnimation(UIElement.OpacityProperty, fadeInEmpty);
                }
            };

            // Начинаем анимацию исчезновения текущей страницы
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<TaskModel> TaskCreated;
    }

}

