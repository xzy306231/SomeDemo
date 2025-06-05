using LearnApp.Shared.Base;
using LearnApp.Shared.Tank;
using LearnApp.Shared.Utils;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LearnApp.Win.View
{
    /// <summary>
    /// UserCanvasGantt.xaml 的交互逻辑
    /// </summary>
    public partial class UserCanvasGantt : UserControl
    {
        public UserCanvasGantt()
        {
            InitializeComponent();
            dataList.ItemsSource = GetGantt("cf36d20f").Gantt.ToList();
        }

        public AllGanttDto GetGantt(string planCode)
        {
            var res = new AllGanttDto();

            var url = $"{BaseConfig.TankUri}/api/CrudeBlend/algorithmTask/loadGantt?task={planCode}";
            res = url.GetFdResult<AllGanttDto>();
            res.Gantt = res.Gantt.Where(x => x.dataInfo.Any()).ToList();

            var sTime = res.Gantt.SelectMany(x => x.dataInfo).Min(x => x.startTime);
            var eTime = res.Gantt.SelectMany(x => x.dataInfo).Max(x => x.endTime);
            res.Gantt.ForEach(a =>
            {
                a.dataInfo = a.dataInfo.OrderBy(x => x.startTime).ToList();

                var list = a.dataInfo;

                a.dataInfo.ForEach(b =>
                {
                    b.TimeLength = (b.endTime - b.startTime).TotalHours;

                    b.Width = (b.endTime - b.startTime).TotalSeconds / (eTime - sTime).TotalSeconds;

                    b.LeftWidth = (b.startTime - sTime).TotalSeconds / (eTime - sTime).TotalSeconds;

                    b.X = 1670.0 * b.LeftWidth;

                    foreach (var item in list.Where(x => x.startTime < b.endTime && x.Id != b.Id))
                    {
                        if (!item.IsHandle && item.actionType == "加工")
                        {
                            item.Y += 20;

                            item.IsHandle = true;
                        }
                        if ((item.Y + 20) > a.Height)
                            a.Height = item.Y + 30;
                    }
                    b.IsHandle = true;

                });
            });
            return res;
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
    }
}
