﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Todo.Entities;
using Todo.Repository;
using Todo.View; 

namespace Todo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            EmailTextBox.Text = "Введите почту";
            EmailTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            PasswordTextBox.Text = "Введите пароль";
            PasswordTextBox.Foreground = new SolidColorBrush(Colors.Gray);


        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "Введите почту")
            {
                EmailTextBox.Text = "";
                EmailTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                EmailTextBox.Text = "Введите почту";
                EmailTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text == "Введите пароль")
            {
                PasswordTextBox.Text = "";
                PasswordTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                PasswordTextBox.Text = "Введите пароль";
                PasswordTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in MainGrid.Children)
            {
                element.Visibility = Visibility.Collapsed;
            }

            // Переходим на страницу Registration
            MainFrame.Navigate(new Registration());

            // Показываем Frame, чтобы страница Registration была видимой
            MainFrame.Visibility = Visibility.Visible;

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            var user = UserRepository.Login(email, password);

            if (user == null)
            {
                MessageBox.Show("Неверный email или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (UIElement element in ((Grid)this.Content).Children)
                {
                    element.Visibility = Visibility.Collapsed;
                }

                // Переходим на страницу Main_empty
                MainFrame.Navigate(new Main_empty());

                // Показываем Frame, чтобы страница Main_empty была видимой
                MainFrame.Visibility = Visibility.Visible;
            }
        }
        
    }
    
}