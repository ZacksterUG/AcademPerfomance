using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public record EventTypeMarksView
    {
        public int event_type_id { get; set; }
        public string name { get; set; } = null!;
        public bool is_course_event { get; set; }
        public int mark_type_id { get; set; }
        public string mark_value { get; set; } = null!;
        public static EventTypeMarksView EmptyEventTypeMark()
        {
            return new EventTypeMarksView
            {
                event_type_id = -1,
                mark_value = "Нет оценки",
            };
        }
    }
}
