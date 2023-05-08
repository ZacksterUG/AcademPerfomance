using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcademPerfomance.Models
{
   public class Institute
   {
        [Key]
        public int institute_id { get; set; }
        public string name { get; set; } = null!;
        public string abbreviation { get; set; } = null!;
        public List<Department> departments { get; set; } = new();
        public static Institute EmptyInstitute()
        {
            return new Institute
            {
                institute_id = -1,
                name = "Не выбрано"
            };
        }
    }
}
