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

        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TaskModel> tasks { get; private set; }

        public Main()
        {
            InitializeComponent();
            LoadTasks();
            taskListBox.ItemsSource = tasks;

            if (Todo.Repository.UserRepository.CurrentUser != null)
            {
                UserName = Todo.Repository.UserRepository.CurrentUser.Name;
            }
        }

        private void LoadTasks()
        {
            tasks = new ObservableCollection<TaskModel>
            {
                new TaskModel { Title = "Задача 1", DueDate = DateTime.Now.AddDays(1), IsCompleted = false, Description = "Описание задачи 1" },
                new TaskModel { Title = "Задача 2", DueDate = DateTime.Now.AddDays(2), IsCompleted = false, Description = "Описание задачи 2" }
            };
        }

        private void taskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (taskListBox.SelectedItem is TaskModel selectedTask)
            {
                taskTitleTextBlock.Text = selectedTask.Title;
                taskDueDateTextBlock.Text = selectedTask.DueDate.ToString("dd.MM.yyyy");
                taskDescriptionTextBlock.Text = selectedTask.Description;
                okButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
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
                tasks.Remove(selectedTask);
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
    }
}



