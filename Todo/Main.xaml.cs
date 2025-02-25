using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Todo.Entities;
using Todo.Repository;

namespace Todo
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window, INotifyPropertyChanged
    {
        private string _username;
        private ObservableCollection<TaskModel> _allTasks; // Все задачи
        private ObservableCollection<TaskModel> _filteredTasks; // Отфильтрованные задачи

        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TaskModel> Tasks
        {
            get => _filteredTasks;
            set
            {
                _filteredTasks = value;
                OnPropertyChanged();
            }
        }

        public Main()
        {
            InitializeComponent();
            LoadTasks();
            Tasks = new ObservableCollection<TaskModel>(_allTasks);
            taskListBox.ItemsSource = Tasks;

            DataContext = this;

            if (UserRepository.CurrentUser != null)
            {
                UserName = UserRepository.CurrentUser.Name;
            }
        }

        private void LoadTasks()
        {
            _allTasks = new ObservableCollection<TaskModel>
        {
            new TaskModel { Title = "Дом", DueDate = DateTime.Now.AddDays(1), IsCompleted = false, Description = "Убраться дома", Category = "Дом" },
            new TaskModel { Title = "Работа", DueDate = DateTime.Now.AddDays(2), IsCompleted = false, Description = "Написать отчёт по проекту", Category = "Работа" },
            new TaskModel { Title = "Учеба", DueDate = DateTime.Now.AddDays(3), IsCompleted = false, Description = "Повторить материал", Category = "Учёба" },
            new TaskModel { Title = "Отдых", DueDate = DateTime.Now.AddDays(4), IsCompleted = false, Description = "Прогуляться вечером на улице", Category = "Отдых" }
        };

            Tasks = new ObservableCollection<TaskModel>(_allTasks);
        }

        private void FilterTasksByCategory(string category)
        {
            Tasks.Clear();

            foreach (var task in _allTasks.Where(t => t.Category == category))
            {
                Tasks.Add(task);
            }
        }


        private void CategoryLabel_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label && label.Tag is string category)
            {
                FilterTasksByCategory(category); // Фильтруем задачи
            }
        }

        private void taskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (taskListBox.SelectedItem is TaskModel selectedTask)
            {
                taskTitleTextBlock.Text = selectedTask.Title;
                taskDueDateTextBlock.Text = selectedTask.DueDate.ToString("HH:mm");
                taskDescriptionTextBlock.Text = selectedTask.Description;

                taskDetailsBorder.Visibility = Visibility.Visible;
                okButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
            }
            else
            {
                taskDetailsBorder.Visibility = Visibility.Collapsed;
                okButton.Visibility = Visibility.Collapsed;
                deleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem is TaskModel selectedTask)
            {
                selectedTask.IsCompleted = true;
                taskListBox.Items.Refresh();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem is TaskModel selectedTask)
            {
                _allTasks.Remove(selectedTask);
                Tasks.Remove(selectedTask);
                ClearTaskDetails();
                taskListBox.Items.Refresh();
            }
        }

        private void ClearTaskDetails()
        {
            taskTitleTextBlock.Text = string.Empty;
            taskDueDateTextBlock.Text = string.Empty;
            taskDescriptionTextBlock.Text = string.Empty;
            okButton.Visibility = Visibility.Collapsed;
            deleteButton.Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenHistoryWindow(object sender, MouseButtonEventArgs e)
        {
            var completedTasks = new ObservableCollection<TaskModel>(_allTasks.Where(t => t.IsCompleted));
            History historyWindow = new History(completedTasks);
            historyWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskCreation taskCreation = new TaskCreation();
            taskCreation.TaskCreated += TaskCreation_TaskCreated;
            taskCreation.Show();


        }

        private void TaskCreation_TaskCreated(object sender, TaskModel newTask)
        {
            _allTasks.Add(newTask); // Добавляем в основной список
            Tasks.Add(newTask);     // Добавляем в отображаемый список
            taskListBox.Items.Refresh(); // Обновляем отображение
        }




    }
}