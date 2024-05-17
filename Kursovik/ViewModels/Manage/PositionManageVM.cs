using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Manage
{
    internal class PositionManageVM : ViewModelBase
    {
        private PositionsVM _positionsVM;

        public RelayCommand ConfirmPositionCommand { get; }

        public PositionManageVM() { }
        public PositionManageVM(PositionsVM positionsVM)
        {
            _positionsVM = positionsVM;
            ConfirmPositionCommand = new RelayCommand(ConfirmAddPosition);
            Text = "Введіть назву посади:";
        }
        public PositionManageVM(PositionsVM positionsVM, Position position)
        {
            _positionsVM = positionsVM;
            ConfirmPositionCommand = new RelayCommand(ConfirmEditPosition);
            Text = "Введіть нову назву посади:";
            CurrentPosition = position;
            PositionName = position.Name;
        }

        #region Commands
        private void ConfirmAddPosition(object parameter)
        {
            if (String.IsNullOrWhiteSpace(PositionName))
            {
                MessageBox.Show("Введіть назву посади", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsPositionExists(PositionName))
            {
                MessageBox.Show("Посада з такою назвою вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newPosition = new Position
            {
                Name = PositionName,
            };

            // Сохраняем пользователя в базе данных
            using (var dbContext = new DataContext())
            {
                dbContext.Positions.Add(newPosition);
                dbContext.SaveChanges();
            }
            MessageBox.Show("Посада успішно створена", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

            _positionsVM.LoadPositions();

            (parameter as System.Windows.Window)?.Close();
        }
        private void ConfirmEditPosition(object parameter)
        {
            if (String.IsNullOrWhiteSpace(PositionName))
            {
                MessageBox.Show("Введіть назву посади", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (IsPositionExists(PositionName))
            {
                MessageBox.Show("Посада з такою назвою вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentPosition.Name = PositionName;
            using (var dbContext = new DataContext())
            {
                dbContext.Positions.Update(CurrentPosition);
                dbContext.SaveChanges();
            }
            MessageBox.Show("Назва посади успішно змінена", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

            _positionsVM.LoadPositions();

            (parameter as System.Windows.Window)?.Close();
        }
        #endregion

        #region Properties
        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        private string _positionName;
        public string PositionName
        {
            get { return _positionName; }
            set { SetProperty(ref _positionName, value); }
        }
        private Position _currentPosition;
        public Position CurrentPosition
        {
            get { return _currentPosition; }
            set { SetProperty(ref _currentPosition, value); }
        }
        #endregion

        #region Functions
        private bool IsPositionExists(string PosName)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Positions.Any(pn => pn.Name == PosName);
            }
        }
        #endregion
    }
}
