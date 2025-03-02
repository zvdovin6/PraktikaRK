using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Todo
{
    /// <summary>
    /// Логика взаимодействия для History.xaml
    /// </summary>
    public partial class History : Window
    {
        private ObservableCollection<TaskModel> _allCompletedTasks; // Все завершенные задачи
        private ObservableCollection<TaskModel> _filteredTasks; // Отфильтрованные задачи

        public History(ObservableCollection<TaskModel> completedTasks)
        {
            InitializeComponent();
            _allCompletedTasks = completedTasks ?? new ObservableCollection<TaskModel>();
            _filteredTasks = new ObservableCollection<TaskModel>(_allCompletedTasks);
            historyListBox.ItemsSource = _filteredTasks;
        }

        // Фильтрация по категории
        private void CategoryLabel_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label && label.Tag is string category)
            {
                _filteredTasks.Clear();
                foreach (var task in _allCompletedTasks.Where(t => t.Category == category))
                {
                    _filteredTasks.Add(task);
                }
            }
        }

        // Показать все завершенные задачи
        private void historyShowAllTasks(object sender, MouseButtonEventArgs e)
        {
            _filteredTasks.Clear();
            foreach (var task in _allCompletedTasks)
            {
                _filteredTasks.Add(task);
            }
        }

        private void historyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (historyListBox.SelectedItem is TaskModel selectedTask)
            {
                taskTitleTextBlock.Text = selectedTask.Title;
                taskDueDateTextBlock.Text = selectedTask.DueDate.ToString("HH:mm");
                taskDateTextBlock.Text = selectedTask.DueDate.ToString("dd/MM/yyyy");
                taskDescriptionTextBlock.Text = selectedTask.Description;

                taskDetailsBorder.Visibility = Visibility.Visible;
            }
            else
            {
                taskDetailsBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Close();
        }
    }
}