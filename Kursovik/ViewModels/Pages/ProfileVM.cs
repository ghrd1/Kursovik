using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Pages
{
    internal class ProfileVM : ViewModelBase
    {
        public RelayCommand SaveCommand { get; }
        public ProfileVM(Teacher user)
        {
            CurrentTeacher = user;
            SaveCommand = new RelayCommand(Save);
            SetProperties();
        }
        public ProfileVM() { }

        #region Commands
        private void Save(object parameter)
        {
            if (String.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Введіть ПІБ", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Введіть логін", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (String.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введіть пароль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Login != CurrentTeacher.Login)
            {
                if (IsLoginExists(Login))
                {
                    MessageBox.Show("Такий лоін вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Teacher temp = CurrentTeacher;
            temp.FullName = FullName;
            temp.Phone = Phone;
            temp.Email = Email;
            temp.Login = Login;
            temp.Password = Password;
            using (var dbContext = new DataContext())
            {
                dbContext.Teachers.Update(temp);
                dbContext.SaveChanges();
            }
            CurrentTeacher = temp;
            MessageBox.Show("Ваші дані були успішно оновлені", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Properties
        private Teacher _currentTeacher;
        public Teacher CurrentTeacher
        {
            get { return _currentTeacher; }
            set { SetProperty(ref _currentTeacher, value); }
        }
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
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
        private string _photo;
        public string Photo
        {
            get { return _photo; }
            set { SetProperty(ref _photo, value); }
        }
        private string _posi;
        public string Posi
        {
            get { return _posi; }
            set { SetProperty(ref _posi, value); }
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
        #endregion

        #region Functions
        private void SetProperties()
        {
            FullName = CurrentTeacher.FullName;
            Phone = CurrentTeacher.Phone;
            Email = CurrentTeacher.Email;
            Photo = CurrentTeacher.Photo_path;
            Posi = GetPosition();
            Login = CurrentTeacher.Login;
            Password = CurrentTeacher.Password;
        }
        private string GetPosition()
        {
            using (var dbContext = new DataContext())
            {
                var POS = dbContext.Positions.FirstOrDefault(u => u.Id == CurrentTeacher.PositionId);
                if (POS == null)
                {
                    return "Директор";
                }
                return POS.Name;
            }
        }
        private bool IsLoginExists(string login)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Teachers.Any(e => e.Login == login);
            }
        }
        #endregion
    }
}
