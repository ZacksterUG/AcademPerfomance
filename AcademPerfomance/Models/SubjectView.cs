using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public class SubjectView
    {
        public int dep_subjects_id { get; set; }
        public int subject_id { get; set; }
        public string subject_name { get; set; } = null!;
        public int department_id { get; set; }
        public string department_name { get; set; } = null!;
        [NotMapped]
        public string full_name { 
            get => $"{subject_name} ({department_name})";
        }
        public static SubjectView EmptySubject()
        {
            return new SubjectView
            {
                subject_id = -1,
                subject_name = "Не выбрано"
            };
        }
    }
}
