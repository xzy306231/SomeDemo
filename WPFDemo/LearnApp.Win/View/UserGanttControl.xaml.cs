using LearnApp.Control;
using LearnApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
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
using TaskStatus = LearnApp.Control.TaskStatus;

namespace LearnApp.Win.View
{
    /// <summary>
    /// UserGanttControl.xaml 的交互逻辑
    /// </summary>
    public partial class UserGanttControl : UserControl
    {
        public UserGanttControl()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<UserGanttControlViewModel>();
        }
 
      

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void ExportImage_Click(object sender, RoutedEventArgs e)
        {
            // 导出为图片的功能
            MessageBox.Show("导出功能待实现", "提示", MessageBoxButton.OK);
        }
    }
}
