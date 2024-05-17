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
    internal class TeacherWindowVm : ViewModelBase
    {
        public RelayCommand OpenProfileCommand { get; }
        public RelayCommand OpenLessonsCommand { get; }
        public RelayCommand ExitCommand { get; }

        public TeacherWindowVm(Teacher user)
        {
            CurrentTeacher = user;
            OpenProfileCommand = new RelayCommand(OpenProfile);
            OpenLessonsCommand = new RelayCommand(OpenLessons);
            ExitCommand = new RelayCommand(Exit);
            LoadProfile();
        }
        public TeacherWindowVm() { }

        #region Commands
        private void OpenProfile(object parameter)
        {
            LoadProfile();
        }
        private void OpenLessons(object parameter)
        {
            var disciplinesVM = new DisciplinesVM(this, CurrentTeacher);
            var disciplinesV = new Disciplines
            {
                DataContext = disciplinesVM
            };
            FrameContent = disciplinesV;
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
