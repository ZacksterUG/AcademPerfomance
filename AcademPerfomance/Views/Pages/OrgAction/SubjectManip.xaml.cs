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
    /// Логика взаимодействия для SubjectManip.xaml
    /// </summary>

    public partial class SubjectManip : UserControl
    {
        private ActionType action;
        public List<SubjectView> Subjects = new List<SubjectView>()
        {
            SubjectView.EmptySubject()
        };
        public SubjectManip(ActionType action)
        {
            InitializeComponent();
            DisciplineCB.ItemsSource = Subjects;
            DisciplineCB.SelectedIndex = 0;
            this.action = action;
            if (action == ActionType.Add)
            {
                l1.Visibility = Visibility.Collapsed;
                DisciplineCB.Visibility = Visibility.Collapsed;
                SaveEditBtn.Content = "Добавить";
                Filter.DepartmentCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;

                    var department = e.AddedItems[0] as Department;
                    DisciplineTB.Text = "";
                    if (department.department_id == -1)
                    {
                        SubjectManipGrid.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        SubjectManipGrid.Visibility = Visibility.Visible;
                    }
                };
            }
            else
            {
                Filter.DepartmentCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;

                    var department = e.AddedItems[0] as Department;
                    DisciplineCB.SelectedIndex = 0;
                    Subjects.RemoveRange(1, Subjects.Count - 1);
                    DisciplineCB.Items.Refresh();
                    if (department.department_id == -1)
                    {
                        DisciplineCB.IsEnabled = false;
                        return;
                    }
                    DisciplineCB.IsEnabled = true;
                    using ApplicationContext context = new();

                    Subjects.AddRange(context.SubjectViews
                        .Where(s => s.department_id == Filter.SelectedDepartment.department_id)
                        .OrderBy(s => s.subject_name)
                    );

                    DisciplineCB.Items.Refresh();
                };
            }
            SubjectManipGrid.Visibility = Visibility.Collapsed;
        }

        private void SaveEditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string subject_name = DisciplineTB.Text;
                int department_id = Filter.SelectedDepartment.department_id;

                using ApplicationContext context = new();
                if (action == ActionType.Add)
                {
                    context.AddSubject(department_id, subject_name);
                    MainWindow.mainWindow.ShowMessage("Дисциплина успешно добавлена!");
                }
                if (action == ActionType.Edit)
                {
                    int dep_subject_id = (DisciplineCB.SelectedItem as SubjectView).dep_subjects_id;
                    context.EditSubject(dep_subject_id, department_id, subject_name);
                    MainWindow.mainWindow.ShowMessage("Изменения сохранены успешно!");
                    Filter.ResetComboBoxes(1);
                    Filter.DepartmentCB.SelectedIndex = 0;
                }
                DisciplineTB.Text = "";
            }
            catch(Exception err)
            {
                MainWindow.mainWindow.ShowMessage(err.Message);
            }
        }

        private void DisciplineCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var subject = e.AddedItems[0] as SubjectView;

            if(subject.subject_id == -1)
            {
                SubjectManipGrid.Visibility = Visibility.Collapsed;
                DisciplineTB.Text = "";
            }
            else
            {
                SubjectManipGrid.Visibility = Visibility.Visible;
                DisciplineTB.Text = subject.subject_name;
            }
        }
    }
}
