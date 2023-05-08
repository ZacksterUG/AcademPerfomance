using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public class GroupView
    {
        public int group_id { get; set; }
        public string group_number { get; set; } = null!;
        public int admission_year { get; set; }
        public Byte semester_count { get; set; }
        public int group_current_course { get; set; }
        public bool is_group_graduated { get; set; }
        public int institute_id { get; set; }
        public string institute_name { get; set; } = null!;
        public int department_id { get; set; }
        public string department_name { get; set; } = null!;
        public int direction_id { get; set; }
        public string direction_name { get; set; } = null!;
        public string direction_code { get; set; } = null!;
        public static GroupView EmptyGroup()
        {
            return new GroupView
            {
                group_id = -1,
                group_number = "Не выбрано",
            };
        }
    }
}
