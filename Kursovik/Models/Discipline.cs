using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik.Models
{
    public partial class Discipline
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Room { get; set; } // Класс
        public string Comment { get; set; } // Коментарий
        public DateTime StartDate { get; set; } // Дата начала дисциплины
        public DateTime EndDate { get; set; } // Дата окончания дисциплины
        public int Time { get; set; } // Часы
        public int Count { get; set; } // количество учителей
        public float Busy { get; set; } // занятость учителей
        public int? PositionId { get; set; }
        public virtual Position Position { get; set; }
        public virtual ICollection<TeacherDiscipline> TeacherDisciplines { get; set; } = new List<TeacherDiscipline>();
    }
}
