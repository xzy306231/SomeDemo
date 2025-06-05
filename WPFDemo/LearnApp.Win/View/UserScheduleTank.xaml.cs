using LearnApp.Win.View.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// UserScheduleTank.xaml 的交互逻辑
    /// </summary>
    public partial class UserScheduleTank : UserControl
    {
        List<Page> TypeList = new List<Page>();
        int index = 0;
        public UserScheduleTank()
        {
            InitializeComponent();
            mainFrame.Navigate(new TankBaseInfo());

            TypeList.Add(new TankBaseInfo());
            TypeList.Add(new TankInitStock());
            TypeList.Add(new TankShipPlan());
            TypeList.Add(new TankSchemeConfig());
            TypeList.Add(new TankScheduleConfig());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn.Content.ToString() == "上一步")
            {
                index--;
                if (index < 0)
                    index++;
                mainFrame.Navigate(TypeList[index]);
            }
            else
            {
                index++;
                if (index > TypeList.Count - 1)
                    index--;
                mainFrame.Navigate(TypeList[index]);
            }
        }
    }
}
