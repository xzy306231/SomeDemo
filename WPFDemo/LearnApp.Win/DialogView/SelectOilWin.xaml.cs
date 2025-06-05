using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnApp.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace LearnApp.Win.DialogView
{
    /// <summary>
    /// SelectOilWin.xaml 的交互逻辑
    /// </summary>
    public partial class SelectOilWin : BaseDialogWindow
    {

        public SelectOilWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
