using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnApp.Shared.BaseBusinessDto
{
    public class MenuItemModelDto
    {
        public string IconCode { get; set; }
        public string Header { get; set; }
        public string TargetView { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsOpenNewView { get; set; } = false;
        public object?[]? Args { get; set; }
        public ICommand OpenViewCommand { get; set; }
        public List<MenuItemModelDto> Children { get; set; } = new List<MenuItemModelDto>();
    }
}
