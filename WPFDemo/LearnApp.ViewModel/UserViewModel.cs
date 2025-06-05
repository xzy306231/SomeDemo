using CommunityToolkit.Mvvm.Input;
using LearnApp.Control;
using LearnApp.Shared;
using LearnApp.Shared.Base;
using LearnApp.Shared.BaseBusinessDto;
using LearnApp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LearnApp.ViewModel
{
    public class UserViewModel : BaseBindable
    {

        private readonly SelectOilVM selectOil;
        public ObservableCollection<OilDto> List { get; set; } = new();
        public ObservableCollection<WebMsg> WebList { get; set; } = new();

        private List<OilDto> list;

        public ICommand ConnectCommand { get; }
        public UserViewModel(SelectOilVM selectOil)
        {
            this.selectOil = selectOil;
            list = InitData();
            ConnectCommand = new AsyncRelayCommand<string>(Connect);
        }
        private async Task Connect(string str)
        {
            var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("ws://192.168.6.216:8005/"), CancellationToken.None);
            var json = JsonExtend.J2String(new WebBody { ProblemId = str });
            var array = Encoding.UTF8.GetBytes(json);
            await ws.SendAsync(new ArraySegment<byte>(array), WebSocketMessageType.Text, true, CancellationToken.None);
            var receiveTask = Task.Run(async () =>
            {
                var buffer = new byte[1024];
                while (true)
                {
                    try
                    {
                        var res = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (res.MessageType == WebSocketMessageType.Close)
                        {
                            break;
                        }
                        var message = Encoding.UTF8.GetString(buffer, 0, res.Count);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            WebList.Insert(0, new WebMsg { Msg = message });
                        });
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            });
        }
        private List<OilDto> InitData()
        {
            var url = BaseConfig.ScheduleUri + "/api/CrudeBlend/externalResult/crudeNatureInfo?isIgnore=false";
            var oils = url.GetFdResult<OilsDto>();
            return oils.Oils;
        }
        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(GetParent((FrameworkElement)sender));

            selectOil.Oils.Clear();
            selectOil.SelectOils.Clear();
            foreach (var item in list)
            {
                selectOil.Oils.Add(new OilInfo { OilCode = item.OilCode, OilName = item.OilName });
            }

            if (WindowManager.ShowDialog("SelectOilWin", selectOil, WindowStartupLocation.Manual, point.X, point.Y))
            {
                if (selectOil.SelectOils != null && selectOil.SelectOils.Any())
                {
                    foreach (var item in selectOil.SelectOils)
                    {
                        List.Add(new OilDto { OilCode = item.OilCode, OilName = item.OilName, Percent = item.Percent });
                    }
                }
            }
        }
        private Window GetParent(FrameworkElement d)
        {
            dynamic obj = VisualTreeHelper.GetParent(d);
            if (obj != null && obj is Window)
                return obj;

            return GetParent(obj);
        }
    }
    public class WebMsg
    {
        public string Msg { get; set; }
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
    public class WebBody
    {
        public string ProblemId { get; set; }
    }
}
