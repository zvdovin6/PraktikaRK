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
using System.Windows.Shapes;

namespace Todo.View
{
    /// <summary>
    /// Логика взаимодействия для Main_empty.xaml
    /// </summary>
    public partial class Main_empty : Page
    {
        public Main_empty()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click_SPZ(object sender, RoutedEventArgs e)
        {
            TaskCreation taskCreation = new TaskCreation();
            taskCreation.TaskCreated += TaskCreation_TaskCreated;
            NavigationService.Navigate(taskCreation);
        }

        private void TaskCreation_TaskCreated(object sender, TaskModel newTask)
        {
            TaskManager.Instance.SaveTask(newTask); // Сохраняем задачу в общий список

            // Создаем экземпляр Main и передаем туда задачу
            Main mainPage = new Main();
        }

    }
}
