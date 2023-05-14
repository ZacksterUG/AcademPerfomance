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

namespace AcademPerfomance.Views.Pages.OrgAction
{
    /// <summary>
    /// Логика взаимодействия для DirectionManip.xaml
    /// </summary>
    public partial class DirectionManip : UserControl
    {
        private ActionType action;
        public void ResetDirection()
        {
            NameTB.Text = "";
            CodeTB.Text = "";
            SemesterSl.Value = 2;
        }
        public DirectionManip(ActionType action)
        {
            InitializeComponent();
            this.action = action;
            DirectionDataGrid.Visibility = Visibility.Collapsed;
            if (action == ActionType.Add)
            {
                Filter.DirectionCB.Visibility = Visibility.Collapsed;
                Filter.l3.Visibility = Visibility.Collapsed;
                DeleteBtn.Visibility = Visibility.Hidden;
                SaveEditBtn.Content = "Добавить";

                Filter.DepartmentCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;
                    if (Filter.SelectedDepartment.department_id == -1)
                    {
                        DirectionDataGrid.Visibility = Visibility.Collapsed;
                        return;
                    }

                    DirectionDataGrid.Visibility = Visibility.Visible;
                };
            }
            else
            {
                Filter.DirectionCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;
                    if (Filter.SelectedDirection.direction_id == -1)
                    {
                        DirectionDataGrid.Visibility = Visibility.Collapsed;
                        return;
                    }

                    DirectionDataGrid.Visibility = Visibility.Visible;

                    NameTB.Text = Filter.SelectedDirection.direction_name;
                    CodeTB.Text = Filter.SelectedDirection.code;
                    SemesterSl.Value = Filter.SelectedDirection.semester_count;
                };
            }
        }

        private void SaveEditBtn_Click(object sender, RoutedEventArgs e)
        {
            using ApplicationContext context = new();

            try
            {
                if (action == ActionType.Add)
                {
                    context.CreateDirection(
                        NameTB.Text,
                        Filter.SelectedDepartment.department_id,
                        (byte)SemesterSl.Value,
                        CodeTB.Text
                    );
                    Filter.ResetComboBoxes(1);
                    Filter.DepartmentCB.SelectedIndex = 0;
                    ResetDirection();
                }
                else
                {
                    context.EditDirection(
                        Filter.SelectedDirection.direction_id,
                        NameTB.Text,
                        Filter.SelectedDepartment.department_id,
                        (byte)SemesterSl.Value,
                        CodeTB.Text
                    );
                    Filter.ResetComboBoxes(1);
                    Filter.DepartmentCB.SelectedIndex = 0;
                }
                MainWindow.mainWindow.ShowMessage("Изменения применены успешно!");
            }
            catch (Exception ex) 
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            using ApplicationContext context = new();
            try 
            {
                context.DeleteDirection(Filter.SelectedDirection.direction_id);
                Filter.ResetComboBoxes(1);
                Filter.DepartmentCB.SelectedIndex = 0;
                MainWindow.mainWindow.ShowMessage("Удаление прошло успешно!");
                ResetDirection();
            }
            catch (Exception ex) 
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }
    }
}
