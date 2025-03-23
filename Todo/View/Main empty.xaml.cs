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
using System.Windows.Media.Animation;
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
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            fadeOut.Completed += (s, fadeOutCompletedEventArgs) =>
            {
                // После завершения анимации исчезновения, переходим на новую страницу
                NavigationService.Navigate(taskCreation);

                // Анимация появления новой страницы
                DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
                taskCreation.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            // Применяем анимацию исчезновения для текущей страницы
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void TaskCreation_TaskCreated(object sender, TaskModel newTask)
        {
            TaskManager.Instance.SaveTask(newTask); // Сохраняем задачу в общий список

            // Создаем экземпляр Main и передаем туда задачу
            Main mainPage = new Main();
        }

    }
}
