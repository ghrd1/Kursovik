using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Manage;
using Kursovik.ViewModels.Windows;
using Kursovik.Views.Manage;
using Kursovik.Views.Pages;
using Kursovik.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Pages
{
    internal class DisciplinesVM : ViewModelBase
    {
        private MainWindowVm _mainWindowVm;
        private TeacherWindowVm _teacherWindowVM;

        public RelayCommand AddNewDisciplineCommand { get; }
        public RelayCommand DeleteDisciplineCommand { get; }
        public RelayCommand ShowParameterDisciplineCommand { get; }

        public DisciplinesVM() { }
        public DisciplinesVM(TeacherWindowVm teacherWindowVM, Teacher teacher)
        {
            _teacherWindowVM = teacherWindowVM;
            CurrentTeacher = teacher;
            ShowParameterDisciplineCommand = new RelayCommand(ShowParameterDiscipline);
            LoadDisciplines();
            Size = "0*";
        }
        public DisciplinesVM(MainWindowVm mainWindowVm)
        {
            _mainWindowVm = mainWindowVm;
            AddNewDisciplineCommand = new RelayCommand(AddNewDiscipline);
            DeleteDisciplineCommand = new RelayCommand(DeleteDiscipline);
            ShowParameterDisciplineCommand = new RelayCommand(ShowParameterDiscipline);
            LoadDisciplines();
            Size = "1*";
        }

        #region Commands
        private void AddNewDiscipline(object parameter)
        {
            var disciplineManageVM = new DisciplineManageVM(this);
            var disciplineManageV = new DisciplineManage
            {
                DataContext = disciplineManageVM
            };
            disciplineManageV.ShowDialog();
        }
        private void DeleteDiscipline(object parameter)
        {
            if (parameter is Discipline discipline)
            {
                var result = MessageBox.Show("Ви впевнені, що бажаєте видалити це відрядження?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var dbContext = new DataContext())
                    {
                        dbContext.Disciplines.Remove(discipline);
                        dbContext.SaveChanges();
                    }
                    LoadDisciplines();
                }
            }
        }
        private void ShowParameterDiscipline(object parameter)
        {
            if (parameter is Discipline discipline)
            {
                if (_currentTeacher == null)
                {
                    var disciplineParameterVM = new DisciplineParameterVM(discipline);
                    var disciplineParameterV = new DisciplineParameter
                    {
                        DataContext = disciplineParameterVM
                    };
                    _mainWindowVm.FrameContent = disciplineParameterV;
                }
                else
                {
                    var disciplineParameterVM = new DisciplineParameterVM(discipline, CurrentTeacher);
                    var disciplineParameterV = new DisciplineParameter
                    {
                        DataContext = disciplineParameterVM
                    };
                    _teacherWindowVM.FrameContent = disciplineParameterV;
                }
            }
        }
        #endregion

        #region Properties
        private ObservableCollection<Discipline> _disciplines;
        public ObservableCollection<Discipline> Disciplines
        {
            get { return _disciplines; }
            set { SetProperty(ref _disciplines, value); }
        }
        private Teacher _currentTeacher;
        public Teacher CurrentTeacher
        {
            get { return _currentTeacher; }
            set { SetProperty(ref _currentTeacher, value); }
        }
        private string _size;
        public string Size
        {
            get { return _size; }
            set { SetProperty(ref _size, value); }
        }
        #endregion

        #region Functions
        public void LoadDisciplines()
        {
            using (var dbContext = new DataContext())
            {
                if (CurrentTeacher == null)
                {
                    var disciplines = dbContext.Disciplines
                        .ToList();
                    Disciplines = new ObservableCollection<Discipline>(disciplines);
                }
                else
                {
                    var disciplinesId = dbContext.TeacherDisciplines
                        .Where(td => td.TeacherId == CurrentTeacher.Id)
                        .Select(td => td.DisciplineId)
                        .ToList();
                    var disciplines = dbContext.Disciplines
                        .Where(e => disciplinesId.Contains(e.Id))
                        .ToList();
                    Disciplines = new ObservableCollection<Discipline>(disciplines);
                }
            }
        }
        #endregion
    }
}
