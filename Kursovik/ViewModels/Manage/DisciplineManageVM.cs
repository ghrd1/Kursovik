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
    internal class DisciplineManageVM : ViewModelBase
    {
        private DisciplinesVM _disciplinesVM;

        public RelayCommand ConfirmAddDisciplineCommand { get; }

        public DisciplineManageVM() { }
        public DisciplineManageVM(DisciplinesVM disciplinesVM)
        {
            ConfirmAddDisciplineCommand = new RelayCommand(ConfirmAddDiscipline);
            _disciplinesVM = disciplinesVM;
            LoadPositions();
        }

        #region Commands
        private void ConfirmAddDiscipline(object parameter)
        {
            if (String.IsNullOrWhiteSpace(DisciplineName))
            {
                MessageBox.Show("Введіть назву предмета", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (IsDisciplineExists(DisciplineName))
            {
                MessageBox.Show("Предмет з такою назвою вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(SelectedPosition == null)
            {
                MessageBox.Show("Виберіть основну посаду", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newDiscipline = new Discipline
            {
                Name = DisciplineName,
                Room = "0",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.AddDays(1).Date,
                Time = 0,
                Count = 0,
                Busy = 0,
                PositionId = SelectedPosition.Id,
            };
            using (var dbContext = new DataContext())
            {
                dbContext.Disciplines.Add(newDiscipline);
                dbContext.SaveChanges();
            }
            MessageBox.Show("Предмет успішно створено", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            _disciplinesVM.LoadDisciplines();
            (parameter as System.Windows.Window)?.Close();
        }
        #endregion

        #region Properties
        private string _disciplineName;
        public string DisciplineName
        {
            get { return _disciplineName; }
            set { SetProperty(ref _disciplineName, value); }
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
        #endregion

        #region Functions
        private bool IsDisciplineExists(string disciplineName)
        {
            using (var dbContext = new DataContext())
            {
                return dbContext.Disciplines.Any(e => e.Name == disciplineName);
            }
        }
        private void LoadPositions()
        {
            using(var dbContext = new DataContext())
            {
                var positions = dbContext.Positions.ToList();
                Positions = new ObservableCollection<Position>(positions);
            }
        }
        #endregion
    }
}
