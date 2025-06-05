using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnApp.Control;
using LearnApp.Shared.Base;
using LearnApp.Shared.BaseBusinessDto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace LearnApp.ViewModel
{
    public class MainViewModel : BaseBindable
    {
        private string nowTime;
        public string NowTime
        {
            get { return nowTime; }
            set { SetProperty(ref nowTime, value); }
        }

        private DispatcherTimer timer;

        // 菜单 集合
        public List<MenuItemModelDto> TreeList { get; set; }
        // 页面 集合
        public ObservableCollection<PageItemModelDto> Pages { get; set; } = new ObservableCollection<PageItemModelDto>();
        public ICommand AlarmDetailCommand { get; set; }
        public MainViewModel()
        {
            #region 菜单初始化
            TreeList = new List<MenuItemModelDto>();
            {
                MenuItemModelDto tim0a = new MenuItemModelDto
                {
                    Header = "组态界面",
                    IconCode = "\ue611",
                    TargetView = "UserConfig",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                };
                TreeList.Add(tim0a);

                MenuItemModelDto tt = new MenuItemModelDto
                {
                    Header = "聊天界面",
                    IconCode = "\ue611",
                    TargetView = "UserChart",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                };
                TreeList.Add(tt);

                MenuItemModelDto tim0 = new MenuItemModelDto
                {
                    Header = "选油组件",
                    IconCode = "\ue611",
                    TargetView = "UserOil",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                };
                TreeList.Add(tim0);

                MenuItemModelDto tim1 = new MenuItemModelDto
                {
                    Header = "常减压加工计划",
                    IconCode = "\ue611",
                    TargetView = "UserCanvasGantt",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                };
                TreeList.Add(tim1);


                MenuItemModelDto tim3 = new MenuItemModelDto
                {
                    Header = "储罐液位图",
                    IconCode = "\ue656",
                    TargetView = "UserLiquidTrend",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                };
                TreeList.Add(tim3);

                MenuItemModelDto tim = new MenuItemModelDto
                {
                    Header = "原油加工排产",
                    IconCode = "\ue603"
                };
                TreeList.Add(tim);

                tim.Children.Add(new MenuItemModelDto
                {
                    Header = "智能排产",
                    TargetView = "UserScheduleRes",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                });
                tim.Children.Add(new MenuItemModelDto
                {
                    Header = "智能排产记录",
                    TargetView = "UserSchedule",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                });
                tim.Children.Add(new MenuItemModelDto
                {
                    Header = "手动排产",
                    TargetView = "UserControl1",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                });

                tim.Children.Add(new MenuItemModelDto
                {
                    Header = "手动排产记录",
                    TargetView = "PBomPage",
                    //OpenViewCommand = new Command<MenuItemModelDto>(OpenView)
                });


                MenuItemModelDto tim2 = new MenuItemModelDto { Header = "仓储管输调度", IconCode = "\ue601" };
                TreeList.Add(tim2);

                tim2.Children.Add(new MenuItemModelDto
                {
                    Header = "仓储调度排产",
                    TargetView = "UserScheduleTank",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                });
                tim2.Children.Add(new MenuItemModelDto
                {
                    Header = "仓储调度记录",
                    TargetView = "UserScheduleTankList",
                    OpenViewCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView)
                });
                tim2.Children.Add(new MenuItemModelDto
                {
                    Header = "手动仓储排产",
                    TargetView = "DevicePage",
                    // OpenViewCommand = new Command<MenuItemModelDto>(OpenView)
                });

                tim2.Children.Add(new MenuItemModelDto
                {
                    Header = "手动仓储排产列表",
                    TargetView = "PBomPage",
                    //OpenViewCommand = new Command<MenuItemModelDto>(OpenView)
                });
            }
            #endregion

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            AlarmDetailCommand = new AsyncRelayCommand<MenuItemModelDto>(OpenView);

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public async Task OpenView(MenuItemModelDto menu)
        {
            IsLoading = true;

            var page = Pages.ToList().FirstOrDefault(p => p.Header == menu.Header);
            if (page == null || menu.IsOpenNewView)
            {
                var type = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "LearnApp.Win.dll").GetType($"LearnApp.Win.View.{menu.TargetView}");
                object p = Activator.CreateInstance(type, menu?.Args);

                Pages.Add(new PageItemModelDto
                {
                    Header = menu.Header,
                    PageView = p,
                    IsSelected = true,
                    CloseTabCommand = new RelayCommand<PageItemModelDto>(ClosePage)
                });
                // BindingMethod(type, p as UserControl);
            }
            else
                page.IsSelected = true;

            IsLoading = false;
        }
        private void BindingMethod(Type type, UserControl win)
        {
            if (type.BaseType.Name == "BaseUserControl")
            {
                var binding = new Binding();
                binding.Path = new PropertyPath("DataContext.AlarmDetailCommand");
                binding.RelativeSource = new RelativeSource { AncestorType = typeof(Window) };

                win.SetBinding(BaseComponent.AlarmDetailCommandProperty, binding);

                //var child = win.Content as StackPanel;
                //foreach (UIElement item in child.Children)
                //{
                //    if (item is Button)
                //    {
                //        (item as Button).Command = AlarmDetailCommand;
                //    }
                //}
            }
        }
        private void ClosePage(PageItemModelDto menu)
        {
            Pages.Remove(menu);
        }
    }
}
