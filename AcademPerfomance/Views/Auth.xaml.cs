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
using AcademPerfomance.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AcademPerfomance.Views
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e) 
        {
            var login = LoginText.Text;
            var password = PasswordText.Password;
            dialog.IsOpen = true;
            try
            {
                AuthButton.IsEnabled = false;
                ApplicationContext.Auth(login, password);
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();
            }
            catch (Exception ex) 
            {
                AuthButton.IsEnabled = true;
                TextMessage.Text = ex.Message;
                dialog.IsOpen = true;
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
