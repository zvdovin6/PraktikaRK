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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Todo.Entities;
using Todo.Repository;


namespace Todo.View
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page, INotifyPropertyChanged
    {
        private string _username;
        private ObservableCollection<TaskModel> _allTasks;
        private ObservableCollection<TaskModel> _filteredTasks;
        private ObservableCollection<TaskModel> _allCompletedTasks;


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

            TaskManager.Instance.TasksUpdated += RefreshTasks;

            _allTasks = TaskManager.Instance.AllTasks;
            _allCompletedTasks = TaskManager.Instance.CompletedTasks;
            Tasks = new ObservableCollection<TaskModel>(_allTasks);
            taskListBox.ItemsSource = Tasks;

            DataContext = this;

            if (UserRepository.CurrentUser != null)
            {
                UserName = UserRepository.CurrentUser.Name;
            }
        }

        private void RefreshTasks()
        {
            // Очищаем текущий список задач
            Tasks.Clear();

            // В зависимости от текущего контекста обновляем список задач
            if (Tasks == _allCompletedTasks)
            {
                // Если отображаются выполненные задачи, обновляем их
                foreach (var task in TaskManager.Instance.CompletedTasks)
                {
                    Tasks.Add(task);
                }
            }
            else
            {
                // Иначе обновляем все задачи
                foreach (var task in TaskManager.Instance.AllTasks)
                {
                    Tasks.Add(task);
                }
            }

            // Обновляем отображение списка задач
            taskListBox.Items.Refresh();
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
                taskDateTextBlock.Text = selectedTask.DueDate.ToString("dd/MM/yyyy");
                taskDescriptionTextBlock.Text = selectedTask.Description;

                taskDetailsBorder.Visibility = Visibility.Visible;
                okButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;

                UpdateUIForTask(selectedTask);

            }
            else
            {
                taskDetailsBorder.Visibility = Visibility.Collapsed;
                okButton.Visibility = Visibility.Collapsed;
                deleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateUIForTask(TaskModel task)
        {
            if (task.IsCompleted)
            {
                // Если задача выполнена, скрываем элементы
                okButton.Visibility = Visibility.Collapsed;
                deleteButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Если задача активна, показываем элементы
                taskDetailsBorder.Visibility = Visibility.Visible;
                okButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
            }
        }

        private void CheckBoxTask_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskModel selectedTask)
            {
                Tasks.Remove(selectedTask);
                TaskManager.Instance.CompleteTask(selectedTask);
                taskListBox.Items.Refresh();

                UpdateUIForTask(selectedTask);
            }
        }

        private void CheckBoxTask_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TaskModel selectedTask)
            {
                TaskManager.Instance.UncompleteTask(selectedTask);

                UpdateUIForTask(selectedTask);
            }
        }


        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem is TaskModel selectedTask)
            {
                // Удаляем задачу из отображаемого списка
                Tasks.Remove(selectedTask);

                // Удаляем задачу из общего списка всех задач
                TaskManager.Instance.CompleteTask(selectedTask);

                // Обновляем отображение списка задач
                taskListBox.Items.Refresh();

                UpdateUIForTask(selectedTask);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem is TaskModel selectedTask)
            {
                _allTasks.Remove(selectedTask);
                Tasks.Remove(selectedTask);
                ClearTaskDetails();
                UpdateUIForTask(selectedTask);
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
            _filteredTasks.Clear();
            foreach (var task in _allCompletedTasks)
            {
                _filteredTasks.Add(task);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskCreation taskCreation = new TaskCreation();
            taskCreation.TaskCreated += TaskCreation_TaskCreated;
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            fadeOut.Completed += (s, e1) =>
            {
                // После завершения анимации исчезновения, переходим на страницу TaskCreation
                NavigationService.Navigate(taskCreation);

                // Анимация появления страницы TaskCreation
                DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                taskCreation.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);


        }

        private void TaskCreation_TaskCreated(object sender, TaskModel newTask)
        {

            TaskManager.Instance.SaveTask(newTask);  // Добавляем задачу в общий список
            Tasks.Add(newTask);  // Добавляем в отображаемый список
            taskListBox.Items.Refresh();  // Обновляем отображение
        }

        private void ShowAllTasks(object sender, MouseButtonEventArgs e)
        {
            Tasks.Clear();

            foreach (var task in _allTasks)
            {
                Tasks.Add(task);
            }
        }



    }
}