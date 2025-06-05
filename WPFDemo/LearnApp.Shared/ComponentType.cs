using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnApp.Shared
{
    public class ComponentType
    {
        public string Header { get; set; }
        public List<ComponetModel> Components { get; set; } = new List<ComponetModel>();
    }
    public class ComponetModel
    {
        public string Icon { get; set; }
        public string TargetType { get; set; }
        public string Header { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public RelayCommand<object> ComponentCommand { get; set; }
        public ComponetModel()
        {
            ComponentCommand = new RelayCommand<object>(ComponentDrag);
        }
        private void ComponentDrag(object obj)
        {
            DragDrop.DoDragDrop(obj as DependencyObject, this, DragDropEffects.Copy);
        }
    }
}
