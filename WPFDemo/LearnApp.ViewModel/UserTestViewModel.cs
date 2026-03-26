using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnApp.ViewModel
{
    public partial  class UserTestViewModel : ObservableObject
    {
        // [ObservableProperty] 自动生成一个名为 UserName 的 public 属性，并实现 INotifyPropertyChanged
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string? _userName;

        // 同样，自动生成 Password 属性
        //[ObservableProperty]
        //private string? _password;

        // 源生成器会根据 [RelayCommand] 创建一个 LoginCommand 属性（ICommand）
        // CanExecute 指定一个方法，用于控制按钮的启用/禁用状态
        [RelayCommand(CanExecute = nameof(CanLogin))]
        private void Login()
        {
            // 模拟登录逻辑
            // 这里可以调用服务进行验证
            System.Diagnostics.Debug.WriteLine($"登录: {UserName}");
        }

        // CanExecute 的判断逻辑
        // 当 UserName 和 Password 都不为空时，命令才可用
        private bool CanLogin()
        {
            // return true;
            return !string.IsNullOrWhiteSpace(UserName);
        }

        //private string? _userName;
        //// 手动声明命令属性
        //public ICommand LoginCommand { get; }
        //public UserTestViewModel()
        //{
        //    // 手动创建 RelayCommand，传入执行方法和 CanExecute 方法
        //    LoginCommand = new RelayCommand(ExecuteLogin, CanLogin);
        //}
        //// 用户名属性，通知 UI 更新
        //public string? UserName
        //{
        //    get => _userName;
        //    set
        //    {
        //        if (SetProperty(ref _userName, value))
        //        {
        //            // 当 UserName 变化时，通知命令重新评估 CanExecute 状态
        //            ((RelayCommand)LoginCommand).NotifyCanExecuteChanged();
        //        }
        //    }
        //}
        //// 命令执行方法
        //private void ExecuteLogin()
        //{
        //    // 模拟登录逻辑
        //    System.Diagnostics.Debug.WriteLine($"登录尝试: {UserName}");
        //    // 这里可以调用服务进行验证
        //}

        //// 命令是否可执行
        //private bool CanLogin()
        //{
        //    return !string.IsNullOrWhiteSpace(UserName);
        //}
    }
}
