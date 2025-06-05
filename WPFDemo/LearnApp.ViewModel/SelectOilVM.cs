using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LearnApp.Shared.BaseBusinessDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearnApp.ViewModel
{
    public class SelectOilVM : ObservableObject
    {
        public ICommand SearchCommand { get; }
        public ICommand CheckCommand { get; }
        public ICommand UnCheckCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RemoveSelectOilCommand { get; }
        public SelectOilVM()
        {
            SearchCommand = new RelayCommand<KeyEventArgs>(Search);
            CheckCommand = new RelayCommand<OilInfo>(Check);
            UnCheckCommand = new RelayCommand<OilInfo>(UnCheck);
            ClearCommand = new RelayCommand(Clear);
            RemoveSelectOilCommand = new RelayCommand<OilInfo>(RemoveSelectOil);
        }
        public ObservableCollection<OilInfo> Oils { get; set; } = new();

        public ObservableCollection<OilInfo> SelectOils { get; set; } = new();

        private void RemoveSelectOil(OilInfo oil)
        {
            SelectOils.Remove(oil);
            Oils.First(x => x.OilCode == oil.OilCode).IsSelect = false;
            Oils.First(x => x.OilCode == oil.OilCode).IsChecked = false;
            Oils.First(x => x.OilCode == oil.OilCode).Percent = 0;
        }
        private void Clear()
        {
            SelectOils.Clear();
            foreach (var oil in Oils)
            {
                oil.Percent = 0;
                oil.IsSelect = false;
                oil.IsChecked = false;
            }
        }
        private void UnCheck(OilInfo oil)
        {
            if (SelectOils.Any(x => x.OilCode == oil.OilCode))
            {
                SelectOils.Remove(oil);
            }
        }
        private void Check(OilInfo oil)
        {
            var sum = SelectOils.Sum(x => x.Percent);
            oil.Percent = 100 - sum;
            if (!SelectOils.Any(x => x.OilCode == oil.OilCode))
            {
                SelectOils.Add(oil);
            }
            Oils.First(x => x.OilCode == oil.OilCode).Percent = oil.Percent;
        }
        private void Search(KeyEventArgs e)
        {      
            var txtBox = e.Source as TextBox;
            if (txtBox.Text == null || txtBox.Text == "")
            {
                foreach (var item in Oils)
                {
                    item.IsSelect = false;
                }
                return;
            }
            foreach (var item in Oils)
            {
                if (item.OilName.Contains(txtBox.Text))
                    item.IsSelect = true;
                else
                    item.IsSelect = false;
            }
        }
    }
}
