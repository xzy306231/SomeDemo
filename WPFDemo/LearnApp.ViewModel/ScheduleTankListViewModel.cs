using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LearnApp.Control;
using LearnApp.Shared.Base;
using LearnApp.Shared.BaseBusinessDto;
using LearnApp.Shared.Tank;
using LearnApp.Shared.Utils;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LearnApp.ViewModel
{
    public class ScheduleTankListViewModel : BaseBindable
    {
        public PaginationModel PaginationModel { get; set; } = new PaginationModel();
        public ICommand LoadGanttCommand { get; }
        public ICommand OpenViewCommand { get; }
        public ScheduleTankListViewModel()
        {
            _scheduleSource = new ObservableCollection<ScheduleDto>();

            LoadGanttCommand = new RelayCommand<string>(OpenGanttView);
            OpenViewCommand = new RelayCommand(OpenView);
            PaginationModel.NavCommand = new RelayCommand<object>(index =>
            {
                PaginationModel.PageIndex = int.Parse(index.ToString());
                GetData();
            });
            GetData();
        }

        private ObservableCollection<ScheduleDto> _scheduleSource;
        public ObservableCollection<ScheduleDto> ScheduleSource
        {
            get { return _scheduleSource; }
            set
            {
                if (_scheduleSource != value)
                {
                    SetProperty(ref _scheduleSource, value);
                }
            }
        }
        private void OpenView()
        {
            WeakReferenceMessenger.Default.Send(new MenuItemModelDto
            {
                TargetView = "UserScheduleTank",
                Header = "仓储调度排产",
                IsOpenNewView = true,
            }, "UserScheduleTank");
        }
        private void OpenGanttView(string planCode)
        {
            WeakReferenceMessenger.Default.Send(new MenuItemModelDto
            {
                Args = new object[] { planCode },
                TargetView = "UserGantt",
                Header = "仓储调度排产结果",
                IsOpenNewView = true,
            }, "UserGantt");
        }
        public override void GetData()
        {
            ScheduleSource.Clear();
            IsLoading = true;
            Task.Run(() =>
            {
                var url = $"{BaseConfig.TankUri}/api/CrudeBlend/schedule/scheduleList?limit={PaginationModel.PageSize}&page={PaginationModel.PageIndex}&createUser=xuzhiyang";
                var list = url.Get<FdJsonResult<ObservableCollection<ScheduleDto>>>();
                ScheduleSource = list.Data;
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    PaginationModel.FillPageNumbers(list.Count);
                });
                IsLoading = false;
            });
        }
        public override void DeleteData(object model)
        {
            base.DeleteData(model);
        }
    }
}
