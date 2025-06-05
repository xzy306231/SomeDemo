using CommunityToolkit.Mvvm.DependencyInjection;
using LearnApp.ViewModel;
using LearnApp.Win.Behaviors;
using LearnApp.Win.View;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LearnApp.Win
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }
        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Viewmodels
            services.AddTransient<ScheduleViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<UserViewModel>();
            services.AddTransient<LiquidTrendViewModel>();
            services.AddTransient<ScheduleTankListViewModel>();
            services.AddTransient<UserGanttViewModel>();
            services.AddTransient<UserDashboardViewModel>();
            services.AddTransient<UserOilViewModel>();
            services.AddTransient<UserConfigViewModel>();
            services.AddTransient<SelectOilVM>();
            services.AddTransient<UserChatViewModel>();

            return services.BuildServiceProvider();
        }

        public new static App Current => (App)Application.Current;

        public IServiceProvider Services
        {
            get;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            LiveCharts.Configure(x =>
            {
                x.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉'));

            });
            var win = new MainWindow();
            win.Show();
            base.OnStartup(e);
        }

        /// <summary>
        /// 捕获Task中的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 是在异常由应用程序引发但未进行处理时发生，但无法捕获多线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 专门捕获所有线程中的异常(不包括Task)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
