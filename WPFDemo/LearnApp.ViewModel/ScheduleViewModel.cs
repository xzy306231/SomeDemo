using LearnApp.Shared.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using LearnApp.Control;
using System.Windows;
using LearnApp.Shared.Schedule;
using LearnApp.Shared.Utils;
using LearnApp.Shared.BaseBusinessDto;
using Pagination = LearnApp.Shared.BaseBusinessDto.Pagination;

namespace LearnApp.ViewModel
{
    public class ScheduleViewModel : BaseBindable
    {
        public PaginationModel PaginationModel { get; set; } = new PaginationModel();
        public ScheduleViewModel()
        {
            _scheduleSource = new ObservableCollection<MMScheduleTaskMinDto>();
            PaginationModel.NavCommand = new RelayCommand<object>(index =>
            {
                PaginationModel.PageIndex = int.Parse(index.ToString());
                GetData();
            });
            GetData();
        }

        private ObservableCollection<MMScheduleTaskMinDto> _scheduleSource;
        public ObservableCollection<MMScheduleTaskMinDto> ScheduleSource
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
        public override void GetData()
        {
            var fdSearch = new FdSearchDto { Pagination = new Pagination { PageIndex = PaginationModel.PageIndex, PageSize = PaginationModel.PageSize } };
            ScheduleSource.Clear();
            IsLoading = true;
            Task.Run(() =>
            {
                var url = $"{BaseConfig.ScheduleUri}/api/CrudeBlend/scheduleResult/scheduleList";
                var list = url.Post<FdJsonResult<ObservableCollection<MMScheduleTaskMinDto>>>(fdSearch, false);
                ScheduleSource = list.Data;
                Application.Current.Dispatcher?.Invoke(() =>
                {
                    PaginationModel.FillPageNumbers(list.Count);
                });
                IsLoading = false;
            });
        }
    }
}
