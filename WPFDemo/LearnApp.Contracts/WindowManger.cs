using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnApp.Contracts
{
    /// <summary>
    /// 窗口管理器
    /// </summary>
    public class WindowManager
    {
        static Dictionary<string, WindowStruct> _regWindowContainer = new Dictionary<string, WindowStruct>();

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        public static void Register<T>(string name, Window owner = null)
        {
            if (!_regWindowContainer.ContainsKey(name))
            {
                _regWindowContainer.Add(name, new WindowStruct { WindowType = typeof(T), Owner = owner });
            }
        }
        public static void Unregister<T>(string name)
        {
            if (_regWindowContainer.ContainsKey(name))
            {
                _regWindowContainer.Remove(name);
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public static bool ShowDialog<T>(string name, T dataContext, WindowStartupLocation manual = WindowStartupLocation.CenterOwner, double left = 0, double top = 0)
        {
            if (_regWindowContainer.ContainsKey(name))
            {
                Type type = _regWindowContainer[name].WindowType;
                //反射创建窗体对象
                var window = (Window)Activator.CreateInstance(type);
                window.Owner = _regWindowContainer[name].Owner;

                _regWindowContainer[name].Owner.Effect = new BlurEffect() { Radius = 5 };
                //Grid layer = new Grid() { Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0)) };
                //UIElement original = _regWindowContainer[name].Owner.Content as UIElement;//MainWindows父窗体
                //_regWindowContainer[name].Owner.Content = null;
                //Grid container = new Grid();
                //container.Children.Add(original);//放入原来的内容
                //container.Children.Add(layer);//在上面放一层蒙板
                // //将装有原来内容和蒙板的容器赋给父级窗体
                //_regWindowContainer[name].Owner.Content = container;


                window.DataContext = dataContext;
                window.WindowStartupLocation = manual;
                //  window.Left = _regWindowContainer[name].Owner.Left + left;
                //   window.Top = _regWindowContainer[name].Owner.Top + top;

                window.Left = left;
                window.Top = top;

                var diaRes = window.ShowDialog().Value;
                _regWindowContainer[name].Owner.Effect = null;

                ////容器Grid
                //Grid grid = _regWindowContainer[name].Owner.Content as Grid;
                ////父级窗体原来的内容
                //UIElement original1 = VisualTreeHelper.GetChild(grid, 0) as UIElement;
                ////将父级窗体原来的内容在容器Grid中移除
                //grid.Children.Remove(original1);
                ////赋给父级窗体
                //_regWindowContainer[name].Owner.Content = original1;

                return diaRes;
            }
            return false;
        }
    }

    public class WindowStruct
    {
        public Type WindowType { get; set; }
        public System.Windows.Window Owner { get; set; }
    }
}
