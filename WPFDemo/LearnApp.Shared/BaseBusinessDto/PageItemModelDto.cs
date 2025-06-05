using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnApp.Shared.BaseBusinessDto
{
    public class PageItemModelDto : ObservableObject
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }
        public string Header { get; set; }
        public object PageView { get; set; }

        public ICommand CloseTabCommand { get; set; }
    }
}
