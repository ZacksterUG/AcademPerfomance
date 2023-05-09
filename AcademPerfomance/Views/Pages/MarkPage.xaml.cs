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
        private List<UserFio> studentList = new();
        private List<EventTypeMarksView> validMarkList = new();
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
            StudentCB.ItemsSource = studentList;
            MarkCB.ItemsSource = validMarkList;
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
                studentList.Clear();
                StudentCB.Items.Refresh();
                StudentCB.IsEnabled = false;
                StudentCB.SelectedIndex = 0;
            }
            if (index <= 6)
            {
                DateCB.IsEnabled = false;
                DateCB.SelectedDate = null;
            }
            if (index <= 7)
            {
                validMarkList.Clear();
                MarkCB.Items.Refresh();
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
            var groups = context.GroupViews
                .Where(x => x.department_id == department.department_id
                                                    && x.is_group_graduated == false)
                .ToList();
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

        private void ControlEventCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var controlEvent = e.AddedItems[0] as ControlEvent;
            ResetFromIndex(5);

            if (controlEvent.control_id == -1) return;

            using ApplicationContext context = new();
            var students = context.GetGroupStudentList(User.CurrentUser.unique_id, (GroupCB.SelectedItem as GroupView).group_id);
            studentList.AddRange(students);
            StudentCB.IsEnabled = true;
            StudentCB.Items.Refresh();
            DateCB.IsEnabled = true;
            DateCB.SelectedDate = DateTime.Now;

            var valid_marks = context.EventTypeMarks.Where(et => et.event_type_id == (ControlEventCB.SelectedItem as ControlEvent).control_type_id);
            validMarkList.AddRange(valid_marks);
            MarkCB.IsEnabled = true;
            MarkCB.Items.Refresh();

            if(studentList.Count > 0)
            {
                var firstStudent = studentList[0];
                var control_event_id = (ControlEventCB.SelectedItem as ControlEvent).control_id;
                SetCurrentMarkIfExists(firstStudent, control_event_id);
            }
        }
        private void SetCurrentMarkIfExists(UserFio User, int control_event_id)
        {
            using ApplicationContext context = new();
            var mark = context.GetStudentControlMark(control_event_id, User.user_unique_id);
            if(mark.mark_value != "-")
            {
                DateCB.SelectedDate = DateTime.ParseExact(mark.date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                if (validMarkList.Count > 0 && validMarkList[0].mark_value == "Нет оценки") validMarkList.RemoveAt(0);
                MarkCB.Items.Refresh();
                MarkCB.SelectedItem = validMarkList.Find(x => x.mark_value == mark.mark_value);
            }
            else
            {
                if (validMarkList.Count == 0 || validMarkList[0].mark_value != "Нет оценки") validMarkList.Insert(0, EventTypeMarksView.EmptyEventTypeMark());
                MarkCB.Items.Refresh();
                DateCB.SelectedDate = DateTime.Now;
                MarkCB.SelectedIndex = 0;
            }
        }

        private void StudentCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var student = e.AddedItems[0] as UserFio;
            SetCurrentMarkIfExists(student, (ControlEventCB.SelectedItem as ControlEvent).control_id);
        }

        private void MarkCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var mark = e.AddedItems[0] as EventTypeMarksView;

            if (mark.mark_value == "Нет оценки") SaveBtn.IsEnabled = false;
            else SaveBtn.IsEnabled = true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            using ApplicationContext context = new ApplicationContext();
            int control_event_id = (ControlEventCB.SelectedItem as ControlEvent).control_id;
            int student_id = (StudentCB.SelectedItem as UserFio).user_id;
            DateTime date = DateCB.SelectedDate ?? DateTime.Now;
            int mark_type_id = (MarkCB.SelectedItem as EventTypeMarksView).mark_type_id;

            context.SetStudentMark(control_event_id, student_id, date, mark_type_id);
        }
    }
}
