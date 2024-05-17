using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Manage
{
    internal class TeacherManageVM : ViewModelBase
    {
        private TeachersVM _teachersVM;

        public RelayCommand ConfirmEmployeeCommand { get; }

        public TeacherManageVM() { }
        public TeacherManageVM(TeachersVM teachersVM)
        {
            _teachersVM = teachersVM;
            ConfirmEmployeeCommand = new RelayCommand(ConfirmAddEmployee);
            LoadPositions();
        }
        public TeacherManageVM(TeachersVM teachersVM, Teacher teacher)
        {
            _teachersVM = teachersVM;
            ConfirmEmployeeCommand = new RelayCommand(ConfirmEditEmployee);
            LoadPositions();
            SetEmployeeProperties(teacher);
            CurrentTeacher = teacher;
        }

        #region Commands
        private void ConfirmAddEmployee(object parameter)
        {
            if (String.IsNullOrWhiteSpace(FIO))
            {
                MessageBox.Show("Введіть ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedPosition == null)
            {
                MessageBox.Show("Виберіть посаду", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Введіть Логін", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введіть пароль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (IsLoginExists(Login))
            {
                MessageBox.Show("Такий логін вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newTeacher = new Teacher
            {
                FullName = FIO,
                Phone = Phone,
                Email = Email,
                Photo_path = "pack://application:,,,/Images/null.png",
                PositionId = SelectedPosition.Id,
                TypeId = 2,
                Login = Login,
                Password = Password,
            };

            // Сохраняем пользователя в базе данных
            using (var dbContext = new DataContext())
            {
                dbContext.Teachers.Add(newTeacher);
                dbContext.SaveChanges();
            }
            MessageBox.Show("Сотрудник усіпішно зареєстрований", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

            _teachersVM.LoadTeachers();

            (parameter as System.Windows.Window)?.Close();
        }
        private void ConfirmEditEmployee(object parameter)
        {
            if (String.IsNullOrWhiteSpace(FIO))
            {
                MessageBox.Show("Введіть ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (SelectedPosition == null)
            {
                MessageBox.Show("Виберіть посаду", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Введіть Логін", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введіть пароль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Login != CurrentTeacher.Login)
            {
                if (IsLoginExists(Login))
                {
                    MessageBox.Show("Такий логін вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            // Обновляем свойства существующего сотрудника
            CurrentTeacher.FullName = FIO;
            CurrentTeacher.Phone = Phone;
            CurrentTeacher.Email = Email;
            CurrentTeacher.PositionId = SelectedPosition.Id;
            CurrentTeacher.Login = Login;
            CurrentTeacher.Password = Password;

            // Сохраняем изменения в базе данных
            using (var dbContext = new DataContext())
            {
                dbContext.Entry(CurrentTeacher).Property(e => e.PositionId).IsModified = true;
                dbContext.Teachers.Update(CurrentTeacher);
                dbContext.SaveChanges();
            }

            MessageBox.Show("Інформацію про співробітника оновлено", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

            _teachersVM.LoadTeachers();

            (parameter as System.Windows.Window)?.Close();
        }
        #endregion

        #region Properties
        private string _fio;
        public string FIO
        {
            get { return _fio; }
            set { SetProperty(ref _fio, value); }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get { return _selectedPosition; }
            set { SetProperty(ref _selectedPosition, value); }
        }
        private string _login;
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        private ObservableCollection<Position> _positions;
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set { SetProperty(ref _positions, value); }
        }
        private Teacher _currentTeacher;
        public Teacher CurrentTeacher
        {
            get { return _currentTeacher; }
            set { SetProperty(ref _currentTeacher, value); }
        }
        #endregion

        #region Functions
        private void LoadPositions()
        {
            using (var dbContext = new DataContext())
            {
                var positions = dbContext.Positions.ToList();
                Positions = new ObservableCollection<Position>(positions);
            }
        }
        private bool IsLoginExists(string login)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Teachers.Any(e => e.Login == login);
            }
        }
        private void SetEmployeeProperties(Teacher teacher)
        {
            FIO = teacher.FullName;
            Phone = teacher.Phone;
            Email = teacher.Email;
            SelectedPosition = Positions.FirstOrDefault(p => p.Id == teacher.PositionId);
            Login = teacher.Login;
            Password = teacher.Password;
        }
        #endregion
    }
}
