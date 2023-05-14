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
        public int control_type_id { get; set; }
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
        public int curriculum_id { get; set; }
        public int semester { get; set; }
        public string subject_name { get; set; } = null!;
        public string department_name { get; set; } = null!;
        public int year { get; set; }
        public int? control_id { get; set; }
        public string control_name { get; set; } = null!;
        public int? control_type_id { get; set; }
        public int? course_event_id { get; set; }
        public string course_event_name { get; set; } = null!;
        public int? course_event_type_id { get; set; }
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
                            control_type_id = control_type_id ?? -1
                        };
                        l.Add(c);
                    }
                    if(course_event_id != null)
                    {
                        ControlEvent c = new ControlEvent
                        {
                            control_id = (int)course_event_id,
                            control_name = course_event_name,
                            control_type_id = course_event_type_id ?? -1
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
                curriculum_id = -1,
                subject_name = "Не выбрано",
            };
        }

    }
}
