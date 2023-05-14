using AcademPerfomance.Views.Pages.OrgAction;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AcademPerfomance.Views.Pages
{
    public class EmptyPage : Page
    {
        public EmptyPage()
        {
        }
    }
    public enum ActionType
    {
        None = 0,
        Add,
        Edit
    }
    public enum ObjectType
    {
        None = 0,
        Direction,
        Subject,
        Group,
        CurriculumElement,
        ControlEvent,
    }
    /// <summary>
    /// Логика взаимодействия для OrgActionPage.xaml
    /// </summary>
    public partial class OrgActionPage : Page
    {
        public OrgActionPage()
        {
            InitializeComponent();
        }
        private void ObjectCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ActionCB == null || ObjectCB == null) return;
            ResetManipulation((ActionType)ActionCB.SelectedIndex, (ObjectType)ObjectCB.SelectedIndex);
        }
        private void ResetManipulation(ActionType actionType, ObjectType objectType)
        {
            if (DependentPart == null) return;
            if (actionType == ActionType.None ||
                objectType == ObjectType.None) 
            {
                DependentPart.Content = null;
                return;
            }
            UserControl? control = null;
            if(objectType == ObjectType.Direction)
            {
                control = new DirectionManip(actionType);
            }
            DependentPart.Navigate(control);
        }

    }
}
