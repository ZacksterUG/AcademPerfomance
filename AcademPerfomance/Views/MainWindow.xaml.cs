using AcademPerfomance.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using AcademPerfomance.Views;
using AcademPerfomance.Views.Pages;

namespace AcademPerfomance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public delegate void NavigatorChangeHandler(int number, string name);
        public event NavigatorChangeHandler? OnNavigatorChange; 
        public MainWindow()
        {
            InitializeComponent();
            if(User.CurrentUser?.role_name == Roles.student)
            {
                MarkTab.Visibility = Visibility.Collapsed;
            }
            if(User.CurrentUser?.role_name != Roles.student)
            {
                CurriculumTab.Visibility = Visibility.Collapsed;
            }
            OnNavigatorChange += (int number, string name) =>
            {
                Page? page = null;
                switch(number)
                {
                    case 1:
                        page = new UserInfoPage();
                        break;
                    case 2:
                        page = new GradePage();
                        break;
                    case 3:
                        page = new MarkPage();
                        break;
                }
                mainPage.Navigate(page);
            };
            SetTab(AboutTab, null);
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private List<Button> TabButtonsList
        {
            get => new()
            {
                AboutTab,
                CurriculumTab,
                MarkTab,
            };
                
        }
        private void SetTab(object sender, RoutedEventArgs? e)
        {
            var ButtonEmitter = (Button)sender;
            OnNavigatorChange?.Invoke(int.Parse((string)ButtonEmitter.Tag), ButtonEmitter.Content.ToString());
            foreach (var tab in TabButtonsList)
            {
                if (tab == ButtonEmitter) tab.Style = (Style)FindResource("MaterialDesignFlatMidBgButton");
                else tab.Style = (Style)FindResource("MaterialDesignFlatLightButton");
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

        private void OnActivate(object sender, EventArgs e)
        {
        }
    }
}
