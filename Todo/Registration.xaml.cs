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
using Todo.Entities;
using Todo.Repository;


namespace Todo
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();

            EmailTextBox1.Text = "Введите почту";
            EmailTextBox1.Foreground = new SolidColorBrush(Colors.Gray);
            PasswordTextBox1.Text = "Введите пароль";
            PasswordTextBox1.Foreground = new SolidColorBrush(Colors.Gray);
            NameTextBox.Text = "Введите имя";
            NameTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            ConfirmPasswordTextBox.Text = "Повторите пароль";
            ConfirmPasswordTextBox.Foreground = new SolidColorBrush(Colors.Gray);
        }
        private void EmailTextBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox1.Text == "Введите почту")
            {
                EmailTextBox1.Text = "";
                EmailTextBox1.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void EmailTextBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox1.Text))
            {
                EmailTextBox1.Text = "Введите почту";
                EmailTextBox1.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void PasswordTextBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox1.Text == "Введите пароль")
            {
                PasswordTextBox1.Text = "";
                PasswordTextBox1.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void PasswordTextBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox1.Text))
            {
                PasswordTextBox1.Text = "Введите пароль";
                PasswordTextBox1.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void ConfirmPasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordTextBox.Text == "Повторите пароль")
            {
                ConfirmPasswordTextBox.Text = "";
                ConfirmPasswordTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void ConfirmPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ConfirmPasswordTextBox.Text))
            {
                ConfirmPasswordTextBox.Text = "Повторите пароль";
                ConfirmPasswordTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "Введите имя")
            {
                NameTextBox.Text = "";
                NameTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                NameTextBox.Text = "Введите имя";
                NameTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox1.Text;
            string password = PasswordTextBox1.Text;
            string confirmPassword = ConfirmPasswordTextBox.Text;
            string name = NameTextBox.Text;

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(email) || email == "Введите почту" ||
                string.IsNullOrWhiteSpace(password) || password == "Введите пароль" ||
                string.IsNullOrWhiteSpace(name) || name == "Введите имя" ||
                string.IsNullOrWhiteSpace(confirmPassword) || confirmPassword == "Повторите пароль")
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на валидность почты
            if (!email.IsValidEmail())
            {
                MessageBox.Show("Неверный формат электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на валидность пароля
            if (!password.IsValidPassword())
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на валидность имени
            if (!name.IsValidName())
            {
                MessageBox.Show("Имя должно содержать не менее 3 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на совпадение паролей
            if (!password.ArePasswordsMatching(confirmPassword))
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = UserRepository.Register(new UserModel
            {
                Name = name,
                Email = email,
                Password = password
            });

            // Проверка на уникальность email
            if (result.Contains("существует"))
            {
                MessageBox.Show(result, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // После успешной регистрации возвращаем данные в MainWindow
            var mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

    }


}


