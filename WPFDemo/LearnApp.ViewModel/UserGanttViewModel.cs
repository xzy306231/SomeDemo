using LearnApp.Shared.Base;
using LearnApp.Shared.Tank;
using LearnApp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.ViewModel
{
    public class UserGanttViewModel : BaseBindable
    {
        public UserGanttViewModel()
        {

        }
        public  AllGanttDto GetGantt(string planCode)
        {
            IsLoading = true;

            var res = new AllGanttDto();
            Task.Delay(1000).Wait();

            var url = $"{BaseConfig.TankUri}/api/CrudeBlend/algorithmTask/loadGantt?task={planCode}";
            res = url.GetFdResult<AllGanttDto>();
            res.Gantt = res.Gantt.Where(x => x.dataInfo.Any()).ToList();

            var sTime = res.Gantt.SelectMany(x => x.dataInfo).Min(x => x.startTime);
            var eTime = res.Gantt.SelectMany(x => x.dataInfo).Max(x => x.endTime);
            res.Gantt.ForEach(a =>
            {
                a.dataInfo = a.dataInfo.OrderBy(x => x.startTime).ToList();
                DateTime? preEnd = null;
                a.dataInfo.ForEach(b =>
                {
                    b.TimeLength = (b.endTime - b.startTime).TotalHours;

                    b.Width = (b.endTime - b.startTime).TotalSeconds / (eTime - sTime).TotalSeconds;

                    if (preEnd != null)
                    {
                        b.LeftWidth = (b.startTime - preEnd.Value).TotalSeconds / (eTime - sTime).TotalSeconds;
                    }
                    else
                    {
                        if (b.startTime != sTime)
                            b.LeftWidth = (b.startTime - sTime).TotalSeconds / (eTime - sTime).TotalSeconds;
                    }
                    preEnd = b.endTime;
                });
            });
            IsLoading = false;
            return res;
        }
    }
}
