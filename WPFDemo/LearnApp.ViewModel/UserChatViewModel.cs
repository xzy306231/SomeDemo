using CommunityToolkit.Mvvm.Input;
using LearnApp.Control;
using LearnApp.Shared.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace LearnApp.ViewModel
{
    public class UserChatViewModel : BaseBindable
    {
        public ICommand SendCommand { get; set; }
        public ObservableCollection<ChatContent> List { get; set; } = new ObservableCollection<ChatContent>();
        public UserChatViewModel()
        {
            List.Add(new ChatContent { Content = "你好啊", DateTime = DateTime.Now, Name = "张三", Type = 1 });
            List.Add(new ChatContent { Content = "你好啊", DateTime = DateTime.Now, Name = "张三", Type = 2 });
            List.Add(new ChatContent { Content = "管输调度怎么样了", DateTime = DateTime.Now, Name = "张三", Type = 1 });
            List.Add(new ChatContent { Content = "做好了", DateTime = DateTime.Now, Name = "张三", Type = 2 });
            List.Add(new ChatContent { Content = "发布一下", DateTime = DateTime.Now, Name = "张三", Type = 1 });

            SendCommand = new RelayCommand(Send);
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }

        private void Send()
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            List.Add(new ChatContent { Status = 1, Content = Message, DateTime = DateTime.Now });
            Message = "";
        }
    }
}
