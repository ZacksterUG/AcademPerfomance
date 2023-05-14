using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public class Direction
    {
        [Key]
        public int direction_id { get; set; }
        public string code { get; set; } = null!;
        public byte semester_count { get; set; }
        public int department_id { get; set; }
        [ForeignKey("department_id")]
        public Department department { get; set; } = null!;
        public string direction_name { get; set; } = null!;
        public static Direction EmptyDirection()
        {
            return new Direction 
            { 
                direction_id = -1,
                direction_name = "Не выбрано"
            };
        }
    }
}
