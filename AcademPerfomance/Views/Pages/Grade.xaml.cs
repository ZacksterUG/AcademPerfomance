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

namespace AcademPerfomance.Views
{
    /// <summary>
    /// Логика взаимодействия для Grade.xaml
    /// </summary>
    public partial class GradePage: Page
    {
        ObservableCollection<StudentControlMark> studentControlMarks = new ();
        public GradePage()
        {
            InitializeComponent();
            using ApplicationContext db = new ApplicationContext();
            var curriculumList = db.GetUsersCurriculum(User.CurrentUser.unique_id).ToList().OrderBy(x => x.semester);
            CurriculumGrid.ItemsSource = curriculumList;
        }

        private void CurriculumGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var curriculumElement = e.AddedItems[0] as CurriculumElement;
            studentControlMarks.Clear();
            using ApplicationContext db = new ();
            var controlEvent = db.GetStudentControlMark(curriculumElement?.control_id);
            var courseEvent = db.GetStudentControlMark(curriculumElement?.course_event_id);

            if (controlEvent != null) studentControlMarks.Add(controlEvent);
            if (courseEvent != null) studentControlMarks.Add(courseEvent);

            MarkGrid.ItemsSource = studentControlMarks;

            if(studentControlMarks.Count == 0) MarkGrid.Visibility = Visibility.Collapsed;
            else MarkGrid.Visibility = Visibility.Visible;
        }
    }
}
