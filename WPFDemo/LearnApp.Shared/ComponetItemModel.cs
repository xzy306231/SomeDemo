using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LearnApp.Shared
{
    public class ComponetItemModel : ObservableObject
    {
        public string Header { get; set; }
        public string DeviceType { get; set; }
        public string DeviceNum { get; set; } = Guid.NewGuid().ToString();
        public RelayCommand<ComponetItemModel> DeleteCommand { get; set; }

        private bool _isSelected;
        int z_temp = 0;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);

                if (value)
                {
                    z_temp = this.Z;
                    this.Z = 999;
                }
                else this.Z = z_temp;
            }
        }
        private double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }
        private double _height;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        private double y;
        public double Y
        {
            get { return y; }
            set { SetProperty(ref y, value); }
        }


        private double x;
        public double X
        {
            get { return x; }
            set { SetProperty(ref x, value); }
        }

        private int z;
        public int Z
        {
            get { return z; }
            set { SetProperty(ref z, value); }
        }

        private Brush fillBrush;
        public Brush FillBrush
        {
            get { return fillBrush; }
            set
            {
                SetProperty(ref fillBrush, value);
            }
        }

        Point startP = new Point(0, 0);
        bool isMoving = false;
        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startP = e.GetPosition((System.Windows.IInputElement)sender);
            isMoving = true;

            Mouse.Capture((IInputElement)sender);

            e.Handled = true;
        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            // 移动是在按下之后
            if (isMoving)
            {
                // 计算出新的位置
                // 相对的是Canvas画布
                // 可以通过视觉树查找  
                // 这个坐标应该是拖动对象的新位置 
                Point p = e.GetPosition(GetParent((FrameworkElement)sender));

                double _x = p.X - startP.X;
                double _y = p.Y - startP.Y;

                // 数据驱动   通过数据模型中的属性变化 ，驱使页面对象的呈现改变
                X = _x;
                Y = _y;
            }
        }
        public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isMoving = false;
            // 释放光标
            Mouse.Capture(null);
        }
        private Canvas GetParent(FrameworkElement d)
        {
            dynamic obj = VisualTreeHelper.GetParent(d);
            if (obj != null && obj is Canvas)
                return obj;

            return GetParent(obj);
        }
    }
}
