using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public record ControlEvent
    {
        public int control_id { get; set; }
        public string control_name { get; set; } = null!;
        public static ControlEvent EmptyControlEvent()
        {
            return new ControlEvent
            {
                control_id = -1,
                control_name = "Не выбрано"
            };
        }
    }
    public record CurriculumElement
    {
        public int semester { get; set; }
        public string subject_name { get; set; } = null!;
        public string department_name { get; set; } = null!;
        public int year { get; set; }
        public int? control_id { get; set; }
        public string control_name { get; set; } = null!;
        public int? course_event_id { get; set; }
        public string course_event_name { get; set; } = null!;
        public List<ControlEvent> ControlEvents
        {
            get {
                    List<ControlEvent> l = new();
                    if(control_id != null)
                    {
                        ControlEvent c = new ControlEvent
                        {
                            control_id = (int)control_id,
                            control_name = control_name,
                        };
                        l.Add(c);
                    }
                    if(course_event_id != null)
                    {
                        ControlEvent c = new ControlEvent
                        {
                            control_id = (int)course_event_id,
                            control_name = course_event_name,
                        };
                        l.Add(c);
                    }
                    return l;
                }
        }
        public static CurriculumElement EmptyCurriculumElement()
        {
            return new CurriculumElement
            {
                subject_name = "Не выбрано",
            };
        }

    }
}
