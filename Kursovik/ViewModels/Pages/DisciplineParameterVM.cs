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
    internal class DisciplineParameterVM : ViewModelBase
    {
        public RelayCommand SavePropertiesDisciplineCommand { get; }
        public RelayCommand AddTeacherInDisciplineCommand { get; }
        public RelayCommand DeleteTeacherInDisciplineCommand { get; }

        public DisciplineParameterVM() { }
        public DisciplineParameterVM(Discipline discipline, Teacher teacher)
        {
            LoadPositions();
            SetPropertiesDiscipline(discipline);
            CurrentTeacher = teacher;
            CurrentDiscipline = discipline;
            LoadTeachers();
            Size = "0";
        }
        public DisciplineParameterVM(Discipline discipline)
        {
            SavePropertiesDisciplineCommand = new RelayCommand(SavePropertiesDiscipline);
            LoadPositions();
            SetPropertiesDiscipline(discipline);
            CurrentDiscipline = discipline;
            LoadTeachers();
            AddTeacherInDisciplineCommand = new RelayCommand(AddTeacherInDiscipline);
            DeleteTeacherInDisciplineCommand = new RelayCommand(DeleteTeacherInDiscipline);
            Size = "1*";
        }

        #region Commands
        private void SavePropertiesDiscipline(object parameter)
        {
            if (String.IsNullOrWhiteSpace(DisciplineName))
            {
                MessageBox.Show("Введіть назву відрядження", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DisciplineName != CurrentDiscipline.Name)
            {
                if (IsDisciplineNameExists(DisciplineName))
                {
                    MessageBox.Show("Відрядження з такою назвою вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(DisciplineRoom))
            {
                MessageBox.Show("Введіть класс", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(DisciplineTime != CurrentDiscipline.Time) { }
            if (DisciplineTime < 0)
            {
                MessageBox.Show("Години не можуть бути меншими за нуль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Discipline tmp = CurrentDiscipline;
            tmp.Name = DisciplineName;
            tmp.Room = DisciplineRoom;
            tmp.PositionId = SelectedPosition.Id;
            tmp.StartDate = DisciplineStartDate;
            tmp.EndDate = DisciplineEndDate;
            tmp.Time = DisciplineTime;

            using(var dbContext = new DataContext())
            {
                dbContext.Entry(tmp).Property(e => e.PositionId).IsModified = true;
                dbContext.Disciplines.Update(tmp);
                dbContext.SaveChanges();
            }
            CurrentDiscipline = tmp;
            ReBusy();
            MessageBox.Show("Данні предмета успішно оновлені", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void AddTeacherInDiscipline(object parameter)
        {
            var teacherDisciplineManageVM = new TeacherDisciplineManageVM(this, CurrentDiscipline);
            var teacherDisciplineManageV = new TeacherDisciplineManage
            {
                DataContext = teacherDisciplineManageVM
            };
            teacherDisciplineManageV.ShowDialog();
        }
        private void DeleteTeacherInDiscipline(object parameter)
        {
            if(parameter is Teacher teacher)
            {
                var result = MessageBox.Show("Ви впевнені, що бажаєте видалити обраного учасника?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using(var dbContext = new DataContext())
                    {
                        var teacherDiscipline = dbContext.TeacherDisciplines
                            .FirstOrDefault(td => td.TeacherId == teacher.Id && td.DisciplineId == CurrentDiscipline.Id);
                        if(teacherDiscipline != null)
                        {
                            dbContext.TeacherDisciplines.Remove(teacherDiscipline);
                            dbContext.SaveChanges();
                            LoadTeachers();
                            ReCount(-1);
                        }
                    }
                }
            }
        }
        #endregion

        #region Properties
        #region Discipline
        private Discipline _currentDiscipline;
        public Discipline CurrentDiscipline
        {
            get { return _currentDiscipline; }
            set { SetProperty(ref _currentDiscipline, value); }
        }
        private string _disciplineName;
        public string DisciplineName
        {
            get { return _disciplineName; }
            set { SetProperty(ref _disciplineName, value); }
        }
        private string _disciplineRoom;
        public string DisciplineRoom
        {
            get { return _disciplineRoom; }
            set { SetProperty(ref _disciplineRoom, value); }
        }
        private ObservableCollection<Position> _positions;
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set { SetProperty(ref _positions, value); }
        }
        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get { return _selectedPosition; }
            set { SetProperty(ref _selectedPosition, value); }
        }
        private string _positionName;
        public string PositionName
        {
            get { return _positionName; }
            set { SetProperty(ref _positionName, value); }
        }
        private DateTime _disciplineStartDate;
        public DateTime DisciplineStartDate
        {
            get { return _disciplineStartDate; }
            set { if (SetProperty(ref _disciplineStartDate, value)) ValidatesDate(); }
        }
        private DateTime _disciplineEndDate;
        public DateTime DisciplineEndDate
        {
            get { return _disciplineEndDate; }
            set { if (SetProperty(ref _disciplineEndDate, value)) ValidatesDate(); }
        }
        private int _disciplineTime;
        public int DisciplineTime
        {
            get { return _disciplineTime; }
            set { SetProperty(ref _disciplineTime, value); }
        }
        private int _disciplineCount;
        public int DisciplineCount
        {
            get { return _disciplineCount; }
            set { SetProperty(ref _disciplineCount, value); }
        }
        private float _disciplineBusy;
        public float DisciplineBusy
        {
            get { return _disciplineBusy; }
            set { SetProperty(ref _disciplineBusy, value); }
        }
        private string _disciplineComment;
        public string DisciplineComment
        {
            get { return _disciplineComment; }
            set { SetProperty(ref _disciplineComment, value); }
        }
        #endregion
        private ObservableCollection<Teacher> _teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set { SetProperty(ref _teachers, value); }
        }
        private string _size;
        public string Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }
        private Teacher _currentTeacher;
        public Teacher CurrentTeacher
        {
            get { return _currentTeacher; }
            set { SetProperty(ref _currentTeacher, value); }
        }
        #endregion

        #region Functions
        #region Discipline
        private void LoadPositions()
        {
            using(var dbContext = new DataContext())
            {
                var positions = dbContext.Positions.ToList();
                Positions = new ObservableCollection<Position>(positions);
            }
        }
        private void SetPropertiesDiscipline(Discipline discipline)
        {
            DisciplineName = discipline.Name;
            DisciplineRoom = discipline.Room;
            SelectedPosition = Positions.FirstOrDefault(e => e.Id == discipline.PositionId);
            PositionName = SelectedPosition.Name;
            DisciplineStartDate = discipline.StartDate;
            DisciplineEndDate = discipline.EndDate;
            DisciplineTime = discipline.Time;
            DisciplineCount = discipline.Count;
            DisciplineBusy = discipline.Busy;
            DisciplineComment = discipline.Comment;
        }
        private void ValidatesDate()
        {
            if (DisciplineEndDate < DisciplineStartDate)
            {
                DisciplineEndDate = DisciplineStartDate.AddDays(1);
            }
        }
        private bool IsDisciplineNameExists(string businesstripName)
        {
            using(var dbContext = new DataContext())
            {
                return dbContext.Disciplines.Any(e => e.Name == businesstripName);
            }
        }
        #endregion
        public void LoadTeachers()
        {
            using(var dbContext = new DataContext())
            {
                var teachersId = dbContext.TeacherDisciplines
                    .Where(td => td.DisciplineId == CurrentDiscipline.Id)
                    .Select(td => td.TeacherId)
                    .ToList();
                var teachers = dbContext.Teachers
                    .Include(e => e.Position)
                    .Where(e => teachersId.Contains(e.Id))
                    .ToList();
                Teachers = new ObservableCollection<Teacher>(teachers);
            }
        }
        public void ReCount(int k)
        {
            using(var dbContext = new DataContext())
            {
                CurrentDiscipline.Count += k;
                dbContext.Disciplines.Update(CurrentDiscipline);
                dbContext.SaveChanges();
            }
            ReBusy();
        }
        private void ReBusy()
        {
            using (var dbContext = new DataContext())
            {
                if (CurrentDiscipline.Count == 0)
                {
                    CurrentDiscipline.Busy = 0;
                }
                else
                {
                    CurrentDiscipline.Busy = CurrentDiscipline.Time / CurrentDiscipline.Count;
                }
                dbContext.Disciplines.Update(CurrentDiscipline);
                dbContext.SaveChanges();
            }
            SetPropertiesDiscipline(CurrentDiscipline);
        }
        #endregion
    }
}
