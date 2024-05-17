using Kursovik.Interface.Commands;
using Kursovik.Models;
using Kursovik.Models.Data;
using Kursovik.ViewModels.Base;
using Kursovik.ViewModels.Manage;
using Kursovik.Views.Manage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kursovik.ViewModels.Pages
{
    internal class PositionsVM : ViewModelBase
    {
        public RelayCommand AddNewPositionCommand { get; }
        public RelayCommand DeletePositionCommand { get; }
        public RelayCommand EditPositionCommand { get; }

        public PositionsVM()
        {
            AddNewPositionCommand = new RelayCommand(AddNewPosition);
            DeletePositionCommand = new RelayCommand(DeletePosition);
            EditPositionCommand = new RelayCommand(EditPosition);
            LoadPositions();
        }

        #region Commands
        private void AddNewPosition(object parameter)
        {
            var positionManageVM = new PositionManageVM(this);
            var positionManageV = new PositionManage
            {
                DataContext = positionManageVM
            };
            positionManageV.ShowDialog();
        }
        private void DeletePosition(object parameter)
        {
            if (parameter is Position position)
            {
                var result = MessageBox.Show("Ви впевнені, що бажаєте видалити цю посаду?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var dbContext = new DataContext())
                    {
                        dbContext.Positions.Remove(position);
                        dbContext.SaveChanges();
                    }
                    LoadPositions();
                }
            }
        }
        private void EditPosition(object parameter)
        {
            if (parameter is Position position)
            {
                var positionManageVM = new PositionManageVM(this, position);
                var positionManageV = new PositionManage
                {
                    DataContext = positionManageVM
                };
                positionManageV.ShowDialog();
            }
        }
        #endregion

        #region Properties
        private ObservableCollection<Position> _positions;
        public ObservableCollection<Position> Positions
        {
            get { return _positions; }
            set { SetProperty(ref _positions, value); }
        }
        #endregion

        #region Functions
        public void LoadPositions()
        {
            using (var dbContext = new DataContext())
            {
                var positions = dbContext.Positions.ToList();
                Positions = new ObservableCollection<Position>(positions);
            }
        }
        #endregion
    }
}
