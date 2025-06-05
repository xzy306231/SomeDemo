using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.BaseBusinessDto
{
    public class OilInfo : ObservableObject
    {
        public string OilCode { get; set; }
        public string OilName { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetProperty(ref isChecked, value);
            }
        }

        private bool isSelect;
        public bool IsSelect
        {
            get { return isSelect; }
            set
            {
                SetProperty(ref isSelect, value);
            }
        }

        private decimal percent;
        public decimal Percent
        {
            get { return percent; }
            set
            {
                SetProperty(ref percent, value);
            }
        }
    }
}
