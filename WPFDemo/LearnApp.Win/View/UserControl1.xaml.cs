using CommunityToolkit.Mvvm.Messaging;
using LearnApp.Control;
using LearnApp.ViewModel;
using LearnApp.Win.DialogView;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LearnApp.Win.View
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : BaseComponent
    {
        public UserControl1()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<UserViewModel>();

        }
    }
}
