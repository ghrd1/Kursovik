using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovik.Models
{
    public partial class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; } // ФИО
        public string Phone { get; set; } // Телефон
        public string Email { get; set; } // Почта
        public string Photo_path { get; set; } //путь фото
        public int? PositionId { get; set; } // Позиция сотрудника
        public int TypeId { get; set; } // Тип пользователя(админ/сотрудник)
        public string Login { get; set; } //Логин
        public string Password { get; set; } //Пароль
        public virtual Position Position { get; set; }
        public virtual TeacherType TeacherType { get; set; }
        public virtual ICollection<TeacherDiscipline> TeacherDisciplines { get; set; } = new List<TeacherDiscipline>();
    }
}
