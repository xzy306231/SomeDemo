using CommunityToolkit.Mvvm.Input;
using LearnApp.Shared;
using LearnApp.Shared.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace LearnApp.ViewModel
{
    public class UserConfigViewModel : BaseBindable
    {
        public ObservableCollection<ComponentType> ComponentTypes { get; set; }
        public ObservableCollection<ComponetItemModel> ComponetItemModels { get; set; }
        public RelayCommand<DragEventArgs> ItemDropCommand { get; set; }
        public RelayCommand<ComponetItemModel> ComponentSelectedCommand { get; set; }

        public UserConfigViewModel()
        {
            var icon = "pack://application:,,,/LearnApp.Control;component/Images/";
            ComponentTypes = new ObservableCollection<ComponentType>();
            ComponetItemModels = new ObservableCollection<ComponetItemModel>();
            ComponentTypes.Add(new ComponentType
            {
                Header = "基础控件",
                Components = new List<ComponetModel> { new ComponetModel { Header = "Button", TargetType = "Button", Icon=$"{icon}Button.png" ,Width = 100, Height = 200 },
                                                       new ComponetModel { Header = "Radio", TargetType = "Button",Icon=$"{icon}Radio.png" ,Width = 100, Height = 200},
                                                       new ComponetModel { Header = "Toggle", TargetType = "Button" ,Icon=$"{icon}Toggle.png",Width = 100, Height = 200},
                                                       new ComponetModel { Header="Check", TargetType="Check",Icon=$"{icon}Check.png",Width = 100, Height = 200} }
            });
            ComponentTypes.Add(new ComponentType
            {
                Header = "码头",
                Components = new List<ComponetModel> { new ComponetModel { Header = "Ship", TargetType = "Ship",Icon = $"{icon}Ship.png",Width = 100, Height = 200 },
                                                       new ComponetModel { Header = "Port", TargetType = "Port",Icon = $"{icon}Port.png" ,Width = 100, Height = 200} }
            });

            ComponentTypes.Add(new ComponentType
            {
                Header = "管线",
                Components = new List<ComponetModel> { new ComponetModel { Header = "Pipe", TargetType = "Pipe",Icon= $"{icon}Pipe.png",Width = 100, Height = 200 },
                                                       new ComponetModel { Header = "弯头", TargetType = "Wantou", Icon = $"{icon}Wantou.png" ,Width = 100, Height = 200} }
            });
            ComponentTypes.Add(new ComponentType
            {
                Header = "储罐",
                Components = new List<ComponetModel> { new ComponetModel { Header = "Tank", TargetType = "Tank", Icon = $"{icon}Tank.png", Width = 80, Height = 100 } }
            });
            ComponentTypes.Add(new ComponentType
            {
                Header = "常减压",
                Components = new List<ComponetModel> { new ComponetModel { Header = "Process", TargetType = "Process", Icon = $"{icon}Process.png", Width = 100, Height = 200 } }
            });
            ItemDropCommand = new RelayCommand<DragEventArgs>(DoItemDropCommand);
            ComponentSelectedCommand = new RelayCommand<ComponetItemModel>(SelectItemCommand);
        }
        public void DragEnter(object sender, DragEventArgs e)
        {
            var element = sender as FrameworkElement;
            element.Effect = new DropShadowEffect() { Color = Colors.Red, ShadowDepth = 0 };
        }
        private void DoItemDropCommand(DragEventArgs e)
        {
            var color = new SolidColorBrush();
            color.Color = Colors.Gray;
            var point = e.GetPosition((IInputElement)e.Source);
            var data = (ComponetModel)e.Data.GetData(typeof(ComponetModel));
            ComponetItemModels.Add(new ComponetItemModel
            {
                DeviceType = data.TargetType,
                Header = data.Header,
                Height = data.Height,
                Width = data.Width,
                X = point.X - data.Width / 2,
                Y = point.Y - data.Height / 2,
                Z = 1,
                FillBrush = color,
                DeleteCommand = new RelayCommand<ComponetItemModel>(x =>
                {
                    ComponetItemModels.Remove(x);
                })
            });
        }
        private void SelectItemCommand(ComponetItemModel obj)
        {
            foreach (var item in ComponetItemModels)
                item.IsSelected = false;

            if (obj == null)
                return;

            if (obj != null)
                obj.IsSelected = true;

            ComponetItemModels.First(x => x.DeviceNum == obj.DeviceNum).IsSelected = true;
        }
    }
}
