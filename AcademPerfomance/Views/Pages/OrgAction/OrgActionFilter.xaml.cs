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
    /// Логика взаимодействия для OrgActionFilter.xaml
    /// </summary>
    public partial class OrgActionFilter : UserControl
    {
        public List<Institute> Institutes { get; set; } = new() { Institute.EmptyInstitute() };
        public List<Department> Departments { get; set; } = new() { Department.EmptyDepartment() };
        public List<Direction> Directions { get; set; } = new() { Direction.EmptyDirection() };
        public List<GroupView> Groups { get; set; } = new() { GroupView.EmptyGroup() };
        public List<string> Semesters { get; set; } = new() { "Не выбрано" };
        public List<CurriculumElement> CurriculumElements { get; set; } = new() { CurriculumElement.EmptyCurriculumElement() };
        public List<ControlEvent> ControlEvents { get; set; } = new() { ControlEvent.EmptyControlEvent() };
        public Institute SelectedInstitute
        {
            get => InstituteCB.SelectedItem as Institute;
        }
        public Department SelectedDepartment
        {
            get => DepartmentCB.SelectedItem as Department;
        }
        public Direction SelectedDirection
        {
            get => DirectionCB.SelectedItem as Direction;
        }
        public GroupView SelectedGroup
        {
            get => GroupCB.SelectedItem as GroupView;
        }
        public string SelectedSemester
        {
            get => SemesterCB.SelectedItem as string;
        }
        public CurriculumElement SelectedCurriculumElement 
        {
            get => CurriculumElementCB.SelectedItem as CurriculumElement;
        }
        public ControlEvent SelectedControlEvent
        {
            get => ControlEventCB.SelectedItem as ControlEvent;
        }
        public int VisibilityTo
        {
            get { return (int)GetValue(VisibilityToProperty); }
            set 
            {
                SetValue(VisibilityToProperty, value); 
            }
        }

        public static readonly DependencyProperty VisibilityToProperty =
            DependencyProperty.Register("VisibilityTo", typeof(int), typeof(OrgActionFilter), new PropertyMetadata(0));

        public OrgActionFilter()
        {
            InitializeComponent();

            InstituteCB.ItemsSource = Institutes;
            DepartmentCB.ItemsSource = Departments;
            DirectionCB.ItemsSource = Directions;
            GroupCB.ItemsSource = Groups;
            SemesterCB.ItemsSource = Semesters;
            CurriculumElementCB.ItemsSource = CurriculumElements;
            ControlEventCB.ItemsSource = ControlEvents;

            using ApplicationContext context = new();
            Institutes.AddRange(context.Institute.ToList());
        }
        public void ResetVisibility(int val)
        {
            if (val == 0)
            {
                DepartmentCB.Visibility = Visibility.Collapsed;
                l2.Visibility = Visibility.Collapsed;
            }
            if (val <= 1)
            {
                DirectionCB.Visibility = Visibility.Collapsed;
                l3.Visibility = Visibility.Collapsed;
            }
            if (val <= 2)
            {
                GroupCB.Visibility = Visibility.Collapsed;
                l4.Visibility = Visibility.Collapsed;
            }
            if (val <= 3)
            {
                SemesterCB.Visibility = Visibility.Collapsed;
                l5.Visibility = Visibility.Collapsed;
            }
            if (val <= 4)
            {
                CurriculumElementCB.Visibility = Visibility.Collapsed;
                l6.Visibility = Visibility.Collapsed;
            }
            if (val <= 5)
            {
                ControlEventCB.Visibility = Visibility.Collapsed;
                l7.Visibility = Visibility.Collapsed;
            }
        }
        public void ResetComboBoxes(int range)
        {
            if(range == 0)
            {
                Departments.RemoveRange(1, Departments.Count - 1);
                DepartmentCB.Items.Refresh();
                DepartmentCB.IsEnabled = false;
                DepartmentCB.SelectedIndex = 0;
            }
            if(range <= 1)
            {
                Directions.RemoveRange(1, Directions.Count - 1);
                DirectionCB.Items.Refresh();
                DirectionCB.IsEnabled = false;
                DirectionCB.SelectedIndex = 0;
            }
            if (range <= 2)
            {
                Groups.RemoveRange(1, Groups.Count - 1);
                GroupCB.Items.Refresh();
                GroupCB.IsEnabled = false;
                GroupCB.SelectedIndex = 0;
            }
            if (range <= 3)
            {
                Semesters.RemoveRange(1, Semesters.Count - 1);
                SemesterCB.Items.Refresh();
                SemesterCB.IsEnabled = false;
                SemesterCB.SelectedIndex = 0;
            }
            if (range <= 4)
            {
                CurriculumElements.RemoveRange(1, CurriculumElements.Count - 1);
                CurriculumElementCB.Items.Refresh();
                CurriculumElementCB.IsEnabled = false;
                CurriculumElementCB.SelectedIndex = 0;
            }
            if (range <= 5)
            {
                ControlEvents.RemoveRange(1, ControlEvents.Count - 1);
                ControlEventCB.Items.Refresh();
                ControlEventCB.IsEnabled = false;
                ControlEventCB.SelectedIndex = 0;
            }
        }
        private void InstituteCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            ResetComboBoxes(0);
            var institute = e.AddedItems[0] as Institute;
            if (institute.institute_id == -1) return;

            using ApplicationContext context = new ApplicationContext();
            Departments.AddRange(context.Department.Where(d => d.institute_id == institute.institute_id).ToList());
            DepartmentCB.Items.Refresh();
            DepartmentCB.IsEnabled = true;
        }

        private void DepartmentCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            ResetComboBoxes(1);
            var department = e.AddedItems[0] as Department;
            if (department.department_id == -1) return;

            using ApplicationContext context = new ApplicationContext();
            Directions.AddRange(context.Directions.Where(d => d.department_id == department.department_id).ToList());
            DirectionCB.Items.Refresh();
            DirectionCB.IsEnabled = true;
        }

        private void DirectionCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            ResetComboBoxes(2);
            var direction = e.AddedItems[0] as Direction;

            if (direction.direction_id == -1) return;
            using ApplicationContext context = new ApplicationContext();
            Groups.AddRange(context.GroupViews.Where(g => g.direction_id == direction.direction_id).ToList());
            GroupCB.Items.Refresh();
            GroupCB.IsEnabled = true;
        }

        private void GroupCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            ResetComboBoxes(3);
            var group = e.AddedItems[0] as GroupView;

            if (group.group_id == -1) return;
            using ApplicationContext context = new ApplicationContext();

            for(int i = 1; i <= group.semester_count; i++)
            {
                Semesters.Add(i.ToString());
            }

            SemesterCB.Items.Refresh();
            SemesterCB.IsEnabled = true;
        }

        private void SemesterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            ResetComboBoxes(4);
            var semester = e.AddedItems[0] as string;

            if (semester == "Не выбрано") return;
            int sem_number = int.Parse(semester);
            using ApplicationContext context = new ApplicationContext();
            CurriculumElements.AddRange(context.GetGroupsCurriculum(
                User.CurrentUser.unique_id, SelectedGroup.group_id)
                .Where(g => g.semester == sem_number)
            );

            CurriculumElementCB.Items.Refresh();
            CurriculumElementCB.IsEnabled = true;
        }

        private void CurriculumElementCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            ResetComboBoxes(5);
            var curriculumElement = e.AddedItems[0] as CurriculumElement;

            if (curriculumElement.curriculum_id == -1) return;

            ControlEvents.AddRange(curriculumElement.ControlEvents);
            ControlEventCB.Items.Refresh();
            ControlEventCB.IsEnabled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ResetVisibility(VisibilityTo);
        }
    }
}
