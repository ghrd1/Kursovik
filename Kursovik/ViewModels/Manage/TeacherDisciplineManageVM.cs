using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Manage
{
    class TeacherDisciplineManageVM : ViewModelBase
    {
        private DisciplineParameterVM _disciplineParameterVM;

        public RelayCommand ConfirmAddSelectedTeachersCommand { get; }

        public TeacherDisciplineManageVM() { }
        public TeacherDisciplineManageVM(DisciplineParameterVM disciplineParameterVM, Discipline discipline)
        {
            _disciplineParameterVM = disciplineParameterVM;
            CurrentDiscipline = discipline;
            LoadTeachers();
            ConfirmAddSelectedTeachersCommand = new RelayCommand(AddSelectedTeachers);
        }

        #region Commands
        private void AddSelectedTeachers(object parameter)
        {
            if (parameter is IList<object> selectedTeachers && selectedTeachers.Any())
            {
                int k = 0;
                using (var dbContext = new DataContext())
                {
                    foreach (var selectedTeacher in selectedTeachers)
                    {
                        if (selectedTeacher is Teacher teacher)
                        {
                            // Создаем новую запись EmployeeBusinessTrip для связи выбранного сотрудника с текущей командировкой
                            var teacherDiscipline = new TeacherDiscipline
                            {
                                DisciplineId = CurrentDiscipline.Id,
                                TeacherId = teacher.Id,
                            };

                            // Добавляем запись в контекст базы данных
                            dbContext.TeacherDisciplines.Add(teacherDiscipline);
                            k++;
                        }
                    }

                    // Сохраняем изменения
                    dbContext.SaveChanges();
                }

                // После добавления перезагружаем список сотрудников
                _disciplineParameterVM.LoadTeachers();
                _disciplineParameterVM.ReCount(k);
                //получение активного окна в приложении(альтернативный вариант)
                var activeWindow = Application.Current.Windows.OfType<System.Windows.Window>().SingleOrDefault(x => x.IsActive);

                if (activeWindow != null)
                {
                    activeWindow.Close();
                }
            }
            else
            {
                // Если ни один сотрудник не был выбран, выводим сообщение об ошибке
                MessageBox.Show("Виберіть хоча б одного вчителя", "помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        #endregion

        #region Properties
        private Discipline _currentDiscipline;
        public Discipline CurrentDiscipline
        {
            get { return _currentDiscipline; }
            set { SetProperty(ref _currentDiscipline, value); }
        }
        private ObservableCollection<Teacher> _teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set { SetProperty(ref _teachers, value); }
        }
        #endregion

        #region Functions
        private void LoadTeachers()
        {
            using(var dbContext = new DataContext())
            {
                var existingTeachersId = dbContext.TeacherDisciplines
                    .Where(td => td.DisciplineId == CurrentDiscipline.Id)
                    .Select(td => td.TeacherId)
                    .ToList();
                var availableTeachers = dbContext.Teachers
                    .Include(e => e.Position)
                    .Where(e => !existingTeachersId.Contains(e.Id) && e.Id != 1 && e.PositionId == CurrentDiscipline.PositionId)
                    .ToList();
                Teachers = new ObservableCollection<Teacher>(availableTeachers);
            }
        }
        #endregion
    }
}
