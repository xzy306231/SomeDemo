using LearnApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LearnApp.Win.View
{
    /// <summary>
    /// UserGantt.xaml 的交互逻辑
    /// </summary>
    public partial class UserGantt : UserControl
    {
        public UserGantt(string planCode)
        {
            InitializeComponent();
            var dataContext = App.Current.Services.GetService<UserGanttViewModel>();
            dataList.ItemsSource = dataContext.GetGantt(planCode).Gantt;
        }

        Brush brush = null;
        Storyboard storyboard = null;
        private void itemBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            var tag = (sender as Border).Tag.ToString();
            var list = GetChildObjects<Border>(dataList, null);
            list.ForEach(a =>
            {
                if (a.Tag?.ToString() == tag)
                {
                    brush = a.Background;
                    ColorAnimation colorAnimation = new ColorAnimation();
                    colorAnimation.From = (a.Background as SolidColorBrush).Color;
                    colorAnimation.To = Color.FromArgb(255, 114, 244, 150);
                    colorAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    colorAnimation.AutoReverse = true;
                    colorAnimation.RepeatBehavior = new RepeatBehavior(int.MaxValue);

                    storyboard = new Storyboard();
                    storyboard.Children.Add(colorAnimation);
                    Storyboard.SetTarget(colorAnimation, a);
                    Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Background.Color"));
                    storyboard.Begin();
                }
            });
        }

        private void itemBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            var tag = (sender as Border).Tag.ToString();
            var list = GetChildObjects<Border>(dataList, null);
            list.ForEach(a =>
            {
                if (a.Tag?.ToString() == tag)
                {
                    a.Background = brush;
                    storyboard.Stop();
                }
            });
        }
        private List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));//指定集合的元素添加到List队尾  
            }
            return childList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() == true)
            {
                print.PrintVisual(view, "打印描述");
            }
        }
    }
}
