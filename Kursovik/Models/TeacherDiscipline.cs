using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik.Models
{
    public partial class TeacherDiscipline
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int DisciplineId { get; set; }
        public virtual Teacher Teachers { get; set; }
        public virtual Discipline Discipline { get; set; }
    }
}
