using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademPerfomance.Models
{
    public class Department
    {
        [Key]
        public int department_id { get; set; }
        public string name { get; set; } = null!;
        public int institute_id { get; set; }
        [ForeignKey("institute_id")]
        public Institute? institute { get; set; }
        public List<Direction> directions { get; set; } = new List<Direction>();
        public static Department EmptyDepartment()
        {
            return new Department
            {
                department_id = -1,
                name = "Не выбрано",
            };
        }
    }
}
