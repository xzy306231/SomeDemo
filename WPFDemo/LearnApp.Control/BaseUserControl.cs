using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearnApp.Control
{
    public class BaseUserControl : UserControl
    {
        public ICommand OpenView
        {
            get { return (ICommand)GetValue(OpenViewProperty); }
            set { SetValue(OpenViewProperty, value); }
        }

        public static readonly DependencyProperty OpenViewProperty =
             DependencyProperty.Register("OpenView", typeof(ICommand), typeof(BaseUserControl), new PropertyMetadata(null));


    }
}
