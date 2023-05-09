using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Input;
using System.Text.Json.Serialization;
using AcademPerfomance.Models;
using System.Threading.Tasks;

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
            try
            {
                var data = File.ReadAllText("config.json");
                var json = JsonDocument.Parse(data).RootElement;
                SaveLoginChecked.IsChecked = json.GetProperty("saveLogin").GetBoolean();

                if (SaveLoginChecked.IsChecked == true)
                {
                    LoginText.Text = json.GetProperty("login").GetString();
                    PasswordText.Focus();
                }
                else LoginText.Focus();
            }
            catch
            {
                JsonObject json = new()
                {
                    { "login", "" },
                    {"saveLogin", false }
                };
                File.WriteAllText("config.json", json.ToString());
            }
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

            var json = new JsonObject()
            {
                { "login", SaveLoginChecked.IsChecked == true? LoginText.Text: "" },
                {"saveLogin", SaveLoginChecked.IsChecked }
            };
            File.WriteAllText("config.json", json.ToString());

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

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                LoginButton_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
