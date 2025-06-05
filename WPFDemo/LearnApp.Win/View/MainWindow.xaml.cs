using CommunityToolkit.Mvvm.Messaging;
using LearnApp.Shared;
using LearnApp.Shared.BaseBusinessDto;
using LearnApp.ViewModel;
using LearnApp.Win.DialogView;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LearnApp.Win.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<MainViewModel>();

            WindowManager.Register<SelectOilWin>("SelectOilWin", this);

            WeakReferenceMessenger.Default.Register<MenuItemModelDto, string>(this, "UserGantt", async (a, b) =>
            {
                var vm = this.DataContext as MainViewModel;
                await vm.OpenView(new MenuItemModelDto
                {
                    Header = b.Header,
                    TargetView = b.TargetView,
                    IsOpenNewView = b.IsOpenNewView,
                    Args = b.Args
                });
            });

            WeakReferenceMessenger.Default.Register<MenuItemModelDto, string>(this, "UserScheduleTank", async (a, b) =>
            {
                var vm = this.DataContext as MainViewModel;
                await vm.OpenView(new MenuItemModelDto
                {
                    Header = b.Header,
                    TargetView = b.TargetView,
                    IsOpenNewView = b.IsOpenNewView,
                    Args = b.Args
                });
            });
        }

        bool MenuClosed = false;
        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MenuClosed)
            {
                Storyboard openMenu = (Storyboard)border.FindResource("OpenMenu");
                openMenu.Begin();
                txt.Text = "\ue67e";
            }
            else
            {
                Storyboard closeMenu = (Storyboard)border.FindResource("CloseMenu");
                closeMenu.Begin();
                txt.Text = "\ue67d";
            }
            MenuClosed = !MenuClosed;
        }
    }
}
