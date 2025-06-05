using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearnApp.Control
{
    /// <summary>
    /// Waiting.xaml 的交互逻辑
    /// </summary>
    public partial class Waiting : UserControl
    {
        public Waiting()
        {
            InitializeComponent();
        }



        public bool IsStart
        {
            get { return (bool)GetValue(IsStartProperty); }
            set
            {
               
                SetValue(IsStartProperty, value);
            }
        }

        public static readonly DependencyProperty IsStartProperty =
            DependencyProperty.Register("IsStart", typeof(bool), typeof(Waiting), new PropertyMetadata(false));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);

            }
        }

        public static readonly DependencyProperty TextProperty =
             DependencyProperty.Register("Text", typeof(string), typeof(Waiting), new PropertyMetadata("计算中..."));
    }
}
