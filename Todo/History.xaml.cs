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
        public History(ObservableCollection<TaskModel> completedTasks)
        {
            InitializeComponent();
            historyListBox.ItemsSource = completedTasks;
        }

        public History()
        {
            InitializeComponent();
        }



        private void CategoryLabel_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label && label.Tag is string category)
            {

            }
        }
    }
}