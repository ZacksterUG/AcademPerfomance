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

namespace AcademPerfomance.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MarkPage.xaml
    /// </summary>
    public partial class MarkPage : Page
    {
        private List<Institute> instituteList = new List<Institute>()
        {
            Institute.EmptyInstitute()
        };
        private List<Department> departmentList = new List<Department>()
        {
            Department.EmptyDepartment()
        };
        private List<GroupView> groupViews = new List<GroupView>()
        {
            GroupView.EmptyGroup()
        };
        private List<string> semesterList = new()
        {
            "Не выбрано"
        };
        private List<CurriculumElement> curriculumElementsList = new()
        {
            CurriculumElement.EmptyCurriculumElement(),
        };
        private List<ControlEvent> controlEventList = new()
        {
            ControlEvent.EmptyControlEvent()
        };
        public MarkPage()
        {
            InitializeComponent();
            using ApplicationContext context = new ();
            instituteList.AddRange(context.Institute.ToList());
            InstitutesCB.ItemsSource = instituteList;
            DepartmentsCB.ItemsSource = departmentList;
            GroupCB.ItemsSource = groupViews;
            SemesterCB.ItemsSource = semesterList;
            CurriculumElementCB.ItemsSource = curriculumElementsList;
            ControlEventCB.ItemsSource = controlEventList;
        }
        private void ResetFromIndex(int index)
        {
            if (index == 0)
            {
                departmentList.Clear();
                departmentList.Add(Department.EmptyDepartment());
                DepartmentsCB.Items.Refresh();
                DepartmentsCB.IsEnabled = false;
                DepartmentsCB.SelectedIndex = 0;
            }
            if(index <= 1)
            {
                groupViews.Clear();
                groupViews.Add(GroupView.EmptyGroup());
                GroupCB.Items.Refresh();
                GroupCB.IsEnabled = false;
                GroupCB.SelectedIndex = 0;
            }
            if (index <= 2)
            {
                semesterList.Clear();
                semesterList.Add("Не выбрано");
                SemesterCB.Items.Refresh();
                SemesterCB.IsEnabled = false;
                SemesterCB.SelectedIndex = 0;
            }
            if (index <= 3)
            {
                curriculumElementsList.Clear();
                curriculumElementsList.Add(CurriculumElement.EmptyCurriculumElement());
                CurriculumElementCB.Items.Refresh();
                CurriculumElementCB.IsEnabled = false;
                CurriculumElementCB.SelectedIndex = 0;
            }
            if (index <= 4)
            {
                controlEventList.Clear();
                controlEventList.Add(ControlEvent.EmptyControlEvent());
                ControlEventCB.Items.Refresh();
                ControlEventCB.IsEnabled = false;
                ControlEventCB.SelectedIndex = 0;
            }
            if (index <= 5)
            {
                StudentCB.IsEnabled = false;
                StudentCB.SelectedIndex = 0;
            }
            if (index <= 6)
            {
                DateCB.IsEnabled = false;
                DateCB.SelectedIndex = 0;
            }
            if (index <= 7)
            {
                MarkCB.IsEnabled = false;
                MarkCB.SelectedIndex = 0;
            }
        }
        private void InstitutesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var institute = e.AddedItems[0] as Institute;
            ResetFromIndex(0);

            if (institute.institute_id == -1) return;

            using ApplicationContext context = new();
            var deps = context.Department.Where(x => x.institute_id == institute.institute_id).ToList();
            departmentList.AddRange(deps);
            DepartmentsCB.IsEnabled = true;
            DepartmentsCB.Items.Refresh();
        }

        private void DepartmentsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var department = e.AddedItems[0] as Department;
            ResetFromIndex(1);

            if (department.department_id == -1) return;

            using ApplicationContext context = new();
            var groups = context.GroupViews.Where(x => x.department_id == department.department_id).ToList();
            groupViews.AddRange(groups);
            GroupCB.IsEnabled = true;
            GroupCB.Items.Refresh();
        }

        private void GroupCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var group = e.AddedItems[0] as GroupView;
            ResetFromIndex(2);

            if (group.group_id == -1) return;

            var semester = group.semester_count;

            for(int i = 1; i <= semester; i++)
            {
                semesterList.Add($"{i}");
            }

            SemesterCB.IsEnabled = true;
            SemesterCB.Items.Refresh();
        }

        private void SemesterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var semester = e.AddedItems[0] as string;
            ResetFromIndex(3);

            if (semester == "Не выбрано") return;

            using ApplicationContext context = new();
            var curriculumElements = context.GetGroupsCurriculum(User.CurrentUser.unique_id, (GroupCB.SelectedItem as GroupView).group_id)
                .Where(x => x.semester == int.Parse(semester))
                .ToList();
            curriculumElementsList.AddRange(curriculumElements);
            CurriculumElementCB.IsEnabled = true;
            CurriculumElementCB.Items.Refresh();
        }

        private void CurriculumElementCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var curriculumElement = e.AddedItems[0] as CurriculumElement;
            ResetFromIndex(4);

            if (curriculumElement.subject_name == "Не выбрано") return;

            using ApplicationContext context = new();
            var controlEvents = curriculumElement.ControlEvents;
            controlEventList.AddRange(controlEvents);
            ControlEventCB.IsEnabled = true;
            ControlEventCB.Items.Refresh();
        }
    }
}
