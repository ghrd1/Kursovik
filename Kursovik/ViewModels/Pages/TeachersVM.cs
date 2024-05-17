using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Manage;
using Kursovik.Views.Manage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Pages
{
    internal class TeachersVM : ViewModelBase
    {
        public RelayCommand AddNewTeacherCommand { get; }
        public RelayCommand DeleteTeacherCommand { get; }
        public RelayCommand EditTeacherCommand { get; }

        public TeachersVM()
        {
            AddNewTeacherCommand = new RelayCommand(AddNewTeacher);
            DeleteTeacherCommand = new RelayCommand(DeleteTeacher);
            EditTeacherCommand = new RelayCommand(EditTeacher);
            LoadTeachers();
        }

        #region Commands
        private void AddNewTeacher(object parameter)
        {
            var teacherManageVM = new TeacherManageVM(this);
            var teacherManageV = new TeacherManage
            {
                DataContext = teacherManageVM
            };
            teacherManageV.ShowDialog();
        }
        private void EditTeacher(object parameter)
        {
            if (parameter is Teacher teacher)
            {
                var teacherManageVM = new TeacherManageVM(this, teacher);
                var teacherManageV = new TeacherManage
                {
                    DataContext = teacherManageVM
                };
                teacherManageV.ShowDialog();
            }
        }
        private void DeleteTeacher(object parameter)
        {
            if (parameter is Teacher teacher)
            {
                var result = MessageBox.Show("Ви впевнені, що бажаєте видалити цього співробітника?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ReDisciplines(teacher.Id);
                    using (var dbContext = new DataContext())
                    {
                        dbContext.Teachers.Remove(teacher);
                        dbContext.SaveChanges();
                    }
                    LoadTeachers();
                }
            }

        }
        #endregion

        #region Properties
        private ObservableCollection<Teacher> _teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set { SetProperty(ref _teachers, value); }
        }
        #endregion

        #region Functions
        public void LoadTeachers()
        {
            using (var dbContext = new DataContext())
            {
                var teachers = dbContext.Teachers
                    .Include(e => e.Position)
                    .Include(e => e.TeacherType)
                    .Where(e => e.Id != 1)
                    .ToList();
                Teachers = new ObservableCollection<Teacher>(teachers);
            }
        }
        private void ReDisciplines(int teacherId)
        {
            using(var dbContext = new DataContext())
            {
                var disciplinesId = dbContext.TeacherDisciplines
                    .Where(td => td.TeacherId == teacherId)
                    .Select(e => e.DisciplineId)
                    .ToList();
                var disciplines = dbContext.Disciplines
                    .Where(e => disciplinesId.Contains(e.Id))
                    .ToList();
                foreach(var discipline in disciplines)
                {
                    if(discipline is Discipline selecteddiscipline)
                    {
                        selecteddiscipline.Count--;
                        if(selecteddiscipline.Count == 0)
                        {
                            selecteddiscipline.Busy = 0;
                        }
                        else
                        {
                            selecteddiscipline.Busy = selecteddiscipline.Time / selecteddiscipline.Count;
                        }
                        dbContext.Disciplines.Update(selecteddiscipline);
                    }
                }
                dbContext.SaveChanges();
            }
        }
        #endregion
    }
}
