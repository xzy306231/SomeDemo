using CommunityToolkit.Mvvm.Input;
using LearnApp.Shared.Base;
using LearnApp.Shared.BaseBusinessDto;
using LearnApp.Shared.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearnApp.ViewModel
{
    public class UserOilViewModel : BaseBindable
    {
        public ICommand SearchCommand { get; }
        public ICommand CheckCommand { get; }
        public ICommand UnCheckCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RemoveSelectOilCommand { get; }
        public UserOilViewModel()
        {
            oils = new ObservableCollection<OilDto>();
            selectOils = new ObservableCollection<OilDto>();
            GetOil().ForEach(x =>
            {
                oils.Add(x);
            });

            SearchCommand = new RelayCommand<KeyEventArgs>(Search);
            CheckCommand = new RelayCommand<OilDto>(Check);
            UnCheckCommand = new RelayCommand<OilDto>(UnCheck);
            ClearCommand = new RelayCommand(Clear);
            RemoveSelectOilCommand = new RelayCommand<OilDto>(RemoveSelectOil);
        }
        private void RemoveSelectOil(OilDto oil)
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
        private void UnCheck(OilDto oil)
        {
            if (SelectOils.Any(x => x.OilCode == oil.OilCode))
            {
                SelectOils.Remove(oil);
            }
        }
        private void Check(OilDto oil)
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
        public List<OilDto> GetOil()
        {
            string url = $"{BaseConfig.ScheduleUri}/api/CrudeBlend/externalResult/crudeNatureInfo?isIgnore=false";
            var oil = url.GetFdResult<OilsDto>();
            return oil.Oils;
        }

        private ObservableCollection<OilDto> oils;
        public ObservableCollection<OilDto> Oils
        {
            get { return oils; }
            set { SetProperty(ref oils, value); }
        }

        private ObservableCollection<OilDto> selectOils;
        public ObservableCollection<OilDto> SelectOils
        {
            get { return selectOils; }
            set { SetProperty(ref selectOils, value); }
        }
    }
}
