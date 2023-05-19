using AcademPerfomance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace AcademPerfomance.Views.Pages.OrgAction
{
    /// <summary>
    /// Логика взаимодействия для GroupManip.xaml
    /// </summary>
    public partial class GroupManip : UserControl
    {
        private ActionType action;
        private readonly int minYear = 2000;
        private readonly int maxYear = 2100;
        private readonly int minNumber = 1;
        private readonly int maxNumber = 99;
        private int ValidateStringNumber(string str, int min, int max)
        {
            bool f = int.TryParse(str, out var result);
            if (!f) throw new Exception("Введеное значение не является числом!");
            if(result < min || result > max) throw new Exception("Введеное число выходит за пределы допустимых значений!");
            return int.Parse(str);
        }
        public GroupManip(ActionType action)
        {
            InitializeComponent();
            this.action = action;
            GroupInfo.Visibility = Visibility.Collapsed;
            if (action == ActionType.Add)
            {
                Filter.l4.Visibility = Visibility.Collapsed;
                Filter.GroupCB.Visibility = Visibility.Collapsed;
                DeleteBtn.Visibility = Visibility.Collapsed;
                Filter.DirectionCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) => 
                {
                    if (e.AddedItems.Count == 0) return;

                    var direction = Filter.SelectedDirection;

                    if(direction.direction_id == -1)
                    {
                        GroupInfo.Visibility = Visibility.Collapsed;
                        return;
                    }

                    GroupInfo.Visibility = Visibility.Visible;
                    NumberTB.Text = 1.ToString();
                    YearTB.Text = DateTime.Now.Year.ToString();

                };
            }
            else
            {
                AddBtn.Visibility = Visibility.Collapsed;
                l1.Visibility = Visibility.Collapsed;
                l2.Visibility = Visibility.Collapsed;
                NumberTB.Visibility = Visibility.Collapsed;
                YearTB.Visibility = Visibility.Collapsed;
                Filter.GroupCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;

                    var group = Filter.SelectedGroup;

                    if (group.group_id == -1) GroupInfo.Visibility = Visibility.Collapsed;
                    else GroupInfo.Visibility = Visibility.Visible;
                };
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using ApplicationContext context = new();
                int group_id = Filter.SelectedGroup.group_id;
                context.DeleteGroup(group_id);
                MainWindow.mainWindow.ShowMessage("Удаление прошло успешно");
                Filter.DirectionCB.SelectedIndex = 0;
                Filter.ResetComboBoxes(2);
            }
            catch(Exception ex)
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string yearStr = YearTB.Text;
                var year = ValidateStringNumber(yearStr, minYear, maxYear);
                string numberStr = NumberTB.Text;
                var number = ValidateStringNumber(numberStr, minNumber, maxNumber);
                using ApplicationContext context = new();
                int direction_id = Filter.SelectedDirection.direction_id;
                context.GenerateGroup(direction_id, (byte)number, year);
                Filter.DepartmentCB.SelectedIndex = 0;
                MainWindow.mainWindow.ShowMessage("Группа была успешно сформирована");
                Filter.ResetComboBoxes(1);
            }
            catch (Exception ex)
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }
    }
}
