﻿using CommunityToolkit.Mvvm.DependencyInjection;
using LearnApp.Control;
using LearnApp.Shared.Base;
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
    /// UserScheduleTankList.xaml 的交互逻辑
    /// </summary>
    public partial class UserScheduleTankList : BaseUserControl
    {
        public UserScheduleTankList()
        {
            this.InitializeComponent();
            DataContext = App.Current.Services.GetService<ScheduleTankListViewModel>();
        }
    }
}
