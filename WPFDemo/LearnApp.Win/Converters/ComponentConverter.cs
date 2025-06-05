using LearnApp.Control;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LearnApp.Win.Converters
{
    public class ComponentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var assembly = Assembly.Load("LearnApp.Control");
            Type t = assembly.GetType(@"LearnApp.Control.Component." + value.ToString())!;
            var obj = Activator.CreateInstance(t)!;

            //ctrl控件的IsSelectedProperty绑定IsSelected属性
            var ctrl = (BaseComponent)obj;
            var binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("IsSelected");
            ctrl.SetBinding(BaseComponent.IsSelectedProperty, binding);

            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("DeleteCommand");
            ctrl.SetBinding(BaseComponent.DeleteCommandProperty, binding);

            binding = new Binding();
            ctrl.SetBinding(BaseComponent.DeleteParameterProperty, binding);

            binding = new Binding();
            binding.Path = new System.Windows.PropertyPath("FillBrush");
            ctrl.SetBinding(BaseComponent.FillBrushProperty, binding);

            return ctrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
