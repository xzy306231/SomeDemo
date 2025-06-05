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

namespace LearnApp.Win.View
{
    /// <summary>
    /// UserSchedule.xaml 的交互逻辑
    /// </summary>
    public partial class UserSchedule : UserControl
    {
        public UserSchedule()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<ScheduleViewModel>();
        }
        private void grid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
