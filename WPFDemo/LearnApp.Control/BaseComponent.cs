using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LearnApp.Control
{
    public class BaseComponent : UserControl
    {
        public ICommand AlarmDetailCommand
        {
            get { return (ICommand)GetValue(AlarmDetailCommandProperty); }
            set { SetValue(AlarmDetailCommandProperty, value); }
        }

        public static readonly DependencyProperty AlarmDetailCommandProperty =
            DependencyProperty.Register("AlarmDetailCommand", typeof(ICommand), typeof(BaseComponent), new PropertyMetadata(null));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(BaseComponent), new PropertyMetadata(false));

        public Brush FillBrush
        {
            get { return (Brush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
        }

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register("FillBrush", typeof(Brush), typeof(BaseComponent), new PropertyMetadata(null));

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteCommand?.Execute(DeleteParameter);
        }
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(BaseComponent),
                new PropertyMetadata(null, new PropertyChangedCallback((d, e) =>
                {
                })));


        public object DeleteParameter
        {
            get { return (object)GetValue(DeleteParameterProperty); }
            set { SetValue(DeleteParameterProperty, value); }
        }
        public static readonly DependencyProperty DeleteParameterProperty =
            DependencyProperty.Register("DeleteParameter", typeof(object), typeof(BaseComponent), new PropertyMetadata(null));

    }
}
