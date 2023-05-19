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
    /// Логика взаимодействия для CurriculumElementManip.xaml
    /// </summary>
    public partial class CurriculumElementManip : UserControl
    {
        private ActionType action;
        public List<SubjectView> TotalSubjects = new List<SubjectView>();
        public List<SubjectView> GroupSubjects = new List<SubjectView>();
        public List<string> Semesters = new List<string>();
        public List<EventTypeMarksView> ControlEvents = new List<EventTypeMarksView>() { EventTypeMarksView.EmptyEventTypeMark() };
        public List<EventTypeMarksView> CourseEvents = new List<EventTypeMarksView>() { EventTypeMarksView.EmptyEventTypeMark() };
        public CurriculumElementManip(ActionType action)
        {
            InitializeComponent();
            this.action = action;
            using ApplicationContext context = new ApplicationContext();

            if (action == ActionType.Add)
            {
                Filter.ResetVisibility(3);
                TotalSubjects.AddRange(context.SubjectViews.OrderBy(e => e.subject_name));
                CurriculumInfoAdd.Visibility = Visibility.Collapsed;
                CurriculumInfoEdit.Visibility = Visibility.Collapsed;

                Filter.GroupCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;

                    var group = Filter.SelectedGroup;
                    if(group.group_id == -1)
                    {
                        CurriculumInfoAdd.Visibility = Visibility.Collapsed;
                        return;
                    }
                    for (int i = 1; i <= Filter.SelectedGroup.semester_count; i++)
                    {
                        Semesters.Add(i.ToString());
                    }
                    SemesterCB.Items.Refresh();
                    CurriculumInfoAdd.Visibility = Visibility.Visible;
                };
            } 
            else
            {
                CurriculumInfoAdd.Visibility = Visibility.Collapsed;
                CurriculumInfoEdit.Visibility = Visibility.Collapsed;

                Filter.CurriculumElementCB.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
                {
                    if (e.AddedItems.Count == 0) return;

                    var curriculumElement = Filter.SelectedCurriculumElement;
                    if(curriculumElement.curriculum_id == -1)
                    {
                        CurriculumInfoEdit.Visibility = Visibility.Collapsed;
                        return;
                    }
                    for (int i = 1; i <= Filter.SelectedGroup.semester_count; i++)
                    {
                        Semesters.Add(i.ToString());
                    }
                    SemesterCB1.Items.Refresh();

                    SemesterCB1.SelectedItem = Semesters
                        .Where(s => s == Filter.SelectedSemester)
                        .First();

                    ControlEventCB1.SelectedItem = ControlEvents
                        .Where(c => 
                            c.event_type_id == Filter.SelectedCurriculumElement.control_type_id
                            || (Filter.SelectedCurriculumElement.control_type_id == null && c.event_type_id == -1 ))
                        .First();

                    CourseEventCB1.SelectedItem = CourseEvents
                        .Where(c =>
                            c.event_type_id == Filter.SelectedCurriculumElement.course_event_type_id
                            || (Filter.SelectedCurriculumElement.course_event_type_id == null && c.event_type_id == -1))
                        .First();

                    CurriculumInfoEdit.Visibility = Visibility.Visible;
                };
            }

            ControlEvents.AddRange(context
                .EventTypeMarks.Where(e => e.is_course_event == false)
                .ToList()
                .DistinctBy(e => e.event_type_id)
            );
            CourseEvents.AddRange(context
                .EventTypeMarks.Where(e => e.is_course_event == true)
                .ToList()
                .DistinctBy(e => e.event_type_id)
            );

            SubjectCB.ItemsSource = TotalSubjects;
            SubjectCB.ItemsSource = TotalSubjects;
            SemesterCB.ItemsSource = Semesters;
            SemesterCB1.ItemsSource = Semesters;
            ControlEventCB.ItemsSource = ControlEvents;
            ControlEventCB1.ItemsSource = ControlEvents;
            CourseEventCB.ItemsSource = CourseEvents;
            CourseEventCB1.ItemsSource = CourseEvents;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int dep_subjects_id = (SubjectCB.SelectedItem as SubjectView).dep_subjects_id;
                int group_id = Filter.SelectedGroup.group_id;
                int semester = int.Parse(SemesterCB.SelectedItem.ToString());
                int? control_event_id = (ControlEventCB.SelectedItem as EventTypeMarksView).event_type_id != -1?
                                        (ControlEventCB.SelectedItem as EventTypeMarksView).event_type_id : null;
                int? course_event_id = (CourseEventCB.SelectedItem as EventTypeMarksView).event_type_id != -1 ?
                                        (CourseEventCB.SelectedItem as EventTypeMarksView).event_type_id : null;

                using ApplicationContext context = new();
                context.AppendCurriculumSubject(group_id, dep_subjects_id, semester, control_event_id, course_event_id);

                MainWindow.mainWindow.ShowMessage("Дисциплина была успешно добавлена в курс");
            }
            catch (Exception ex)
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int curriculumElementId = Filter.SelectedCurriculumElement.curriculum_id;
                using ApplicationContext context = new();
                context.DeleteCurriculumElement(curriculumElementId);
                Filter.SemesterCB.SelectedIndex = 0;
                MainWindow.mainWindow.ShowMessage("Дисциплина была успешно удалена из курса");
            }
            catch (Exception ex)
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }

        private void SaveEditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int semester = int.Parse(SemesterCB1.SelectedItem as string);
                int curriculumId = Filter.SelectedCurriculumElement.curriculum_id;

                using ApplicationContext context = new();

                int control_type_event = (ControlEventCB1.SelectedItem as EventTypeMarksView).event_type_id;
                int course_type_event = (CourseEventCB1.SelectedItem as EventTypeMarksView).event_type_id;

                if(control_type_event != Filter.SelectedCurriculumElement.control_type_id
                    && !(control_type_event == -1 && Filter.SelectedCurriculumElement.control_type_id == null))
                {
                    if(control_type_event == -1) // Удаление контрольного мероприятия из курса
                    {
                        context.DeleteControlEvent((int)Filter.SelectedCurriculumElement.control_id);
                    }
                    else if(Filter.SelectedCurriculumElement.control_type_id == null)
                    {
                        context.AddControlEvent(curriculumId, control_type_event);
                    }
                    else
                    {
                        if(semester != Filter.SelectedCurriculumElement.semester)
                        {
                            Filter.SemesterCB.SelectedIndex = -1;
                        }
                        context.EditControlEvent((int)Filter.SelectedCurriculumElement.control_type_id, control_type_event);
                    }
                }

                if(course_type_event != Filter.SelectedCurriculumElement.course_event_type_id
                    && !(course_type_event == -1 && Filter.SelectedCurriculumElement.course_event_type_id == null))
                {
                    if (course_type_event == -1) // Удаление контрольного мероприятия из курса
                    {
                        context.DeleteControlEvent((int)Filter.SelectedCurriculumElement.course_event_id);
                    }
                    else if (Filter.SelectedCurriculumElement.course_event_type_id == null)
                    {
                        context.AddControlEvent(curriculumId, course_type_event);
                    }
                    else
                    {
                        if (semester != Filter.SelectedCurriculumElement.semester)
                        {
                            Filter.SemesterCB.SelectedIndex = -1;
                        }
                        context.EditControlEvent((int)Filter.SelectedCurriculumElement.course_event_type_id, course_type_event);
                    }
                }

                context.EditCurriculumElement(curriculumId, semester);
                MainWindow.mainWindow.ShowMessage("Изменения были успешно сохранены");
            }
            catch (Exception ex)
            {
                MainWindow.mainWindow.ShowMessage(ex.Message);
            }
        }
    }
}
