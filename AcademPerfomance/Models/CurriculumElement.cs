using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
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

    }
}
