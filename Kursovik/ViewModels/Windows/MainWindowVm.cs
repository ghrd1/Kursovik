using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Pages;
using Kursovik.Views.Pages;
using Kursovik.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kursovik.ViewModels.Windows
{
    internal class MainWindowVm : ViewModelBase
    {
        public RelayCommand OpenProfileCommand { get; }
        public RelayCommand OpenLessonsCommand { get; }
        public RelayCommand OpenTeachersCommand { get; }
        public RelayCommand OpenPositionsCommand { get; }

        public RelayCommand ExitCommand { get; }

        public MainWindowVm(Teacher user)
        {
            CurrentTeacher = user;
            OpenProfileCommand = new RelayCommand(OpenProfile);
            OpenLessonsCommand = new RelayCommand(OpenLessons);
            OpenTeachersCommand = new RelayCommand(OpenTeachers);
            OpenPositionsCommand = new RelayCommand(OpenPositions);
            ExitCommand = new RelayCommand(Exit);
            LoadProfile();
        }
        public MainWindowVm() { }

        #region Commands
        private void OpenProfile(object parameter)
        {
            LoadProfile();
        }
        private void OpenLessons(object parameter)
        {
            var disciplinesVM = new DisciplinesVM(this);
            var disciplinesV = new Disciplines
            {
                DataContext = disciplinesVM
            };
            FrameContent = disciplinesV;
        }
        private void OpenTeachers(object parameter)
        {
            var teachersVM = new TeachersVM();
            var teachersV = new Teachers
            {
                DataContext = teachersVM
            };
            FrameContent = teachersV;
        }
        private void OpenPositions(object parameter)
        {
            var positionsVM = new PositionsVM();
            var positionsV = new Positions
            {
                DataContext = positionsVM
            };
            FrameContent = positionsV;
        }
        private void Exit(object parameter)
        {
            var autorizeVM = new AutorizeVM();
            var autorizeV = new Autorize
            {
                DataContext = autorizeVM,
            };
            autorizeV.Show();
            (parameter as System.Windows.Window)?.Close();
        }
        #endregion

        #region Properties
        private Teacher _currentTeacher;
        public Teacher CurrentTeacher
        {
            get { return _currentTeacher; }
            set { SetProperty(ref _currentTeacher, value); }
        }
        private Page _frameContent;
        public Page FrameContent
        {
            get { return _frameContent; }
            set { SetProperty(ref _frameContent, value); }
        }
        #endregion

        #region Functions   
        private void LoadProfile()
        {
            var profileVM = new ProfileVM(CurrentTeacher);
            var profileV = new Profile
            {
                DataContext = profileVM,
            };
            FrameContent = profileV;
        }
        #endregion
    }
}
