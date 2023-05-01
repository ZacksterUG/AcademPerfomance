using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public record StudentControlMark
    {
        public int control_event_id { get; set; }
        public string event_type { get; set; } = null!;
        public string subject_name { get; set; } = null!;
        public string department_name { get; set; } = null!;
        public string mark_value { get; set;} = null!;
        public string teacher_fio { get; set; } = null!;
        public string date { get; set; } = null!;
    }
}
