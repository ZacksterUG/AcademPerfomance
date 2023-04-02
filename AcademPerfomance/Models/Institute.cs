using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcademPerfomance.Models
{
   public class Institute
   {
        [Column("institute_id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("abbreviation")]
        public string Abbreviation { get; set; } = null!;
   }
}
