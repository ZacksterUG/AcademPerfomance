using AcademPerfomance.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AcademPerfomance.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserInfoPage.xaml
    /// </summary>
    public partial class UserInfoPage : Page
    {
        ObservableCollection<UserFio> GroupStudents { get; set; } = new();
        public UserInfoPage()
        {
            InitializeComponent();
            UserInfoText.Text = User.CurrentUser?.ToString();
            if (User.CurrentUser.role_name != "Студент") 
            {
                GroupInfo.Visibility = Visibility.Hidden;
            }
            GroupStudents.CollectionChanged += (object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
            {
                StudentGrid.ItemsSource = GroupStudents;
            };
        }

        private void ShowGroupStudentList(object sender, RoutedEventArgs e)
        {
            if (StudentGrid.Visibility == Visibility.Visible)
            {
                StudentGrid.Visibility = Visibility.Hidden;
                GroupStudents.Clear();
                return;
            }
            StudentGrid.Visibility = Visibility.Visible;
            using ApplicationContext db = new ApplicationContext();
            var g = db.GetGroupStudentList(User.CurrentUser.unique_id, User.CurrentUser.group_id);
            foreach (var s in g)
            {
                GroupStudents.Add(s);
            }
        }
    }
}
