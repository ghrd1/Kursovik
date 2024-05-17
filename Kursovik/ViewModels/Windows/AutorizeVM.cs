using Kursovik.Interface.Commands;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Windows
{
    internal class AutorizeVM : ViewModelBase
    {
        public RelayCommand ConfirmLoginCommand { get; }

        public AutorizeVM()
        {
            ConfirmLoginCommand = new RelayCommand(ConfirmLogin);
        }

        #region Commands
        private void ConfirmLogin(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Заповніть всі поля", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (var dbContext = new DataContext())
            {
                var user = dbContext.Teachers.FirstOrDefault(u => u.Login == Login && u.Password == Password);

                if (user != null)
                {
                    if (user.TypeId == 1) //администратор
                    {
                        var mainwindowVM = new MainWindowVm(user);
                        var mainwindowV = new MainWindow
                        {
                            DataContext = mainwindowVM
                        };
                        mainwindowV.Show();
                    }
                    else if (user.TypeId == 2) //сотрудник
                    {
                        var teacherWindowVM = new TeacherWindowVm(user);
                        var teacherWindowV = new TeacherWindow
                        {
                            DataContext = teacherWindowVM
                        };
                        teacherWindowV.Show();
                    }
                    (parameter as System.Windows.Window)?.Close();
                }
                else
                {
                    MessageBox.Show("Невірний логін чи пароль", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
        #endregion

        #region Properties
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
    }
}
