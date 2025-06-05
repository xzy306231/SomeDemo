using LearnApp.Shared.Base;
using LearnApp.Shared.Tank;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Linq;
using LearnApp.Shared.Utils;

namespace LearnApp.ViewModel
{
    public class LiquidTrendViewModel : BaseBindable
    {
        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "液位(m)",
                    NamePaint = new SolidColorPaint(SKColors.Black),

                    LabelsPaint = new SolidColorPaint(SKColors.Green),
                    TextSize = 20,
                    MinStep=5
                }
            };

        public LiquidTrendViewModel()
        {
            var url = $"{BaseConfig.TankUri}/api/CrudeBlend/ganttDataDisplay/tankDetails";
            var para = new GanttGetDataInputDto
            {
                EndTime = "2023-11-01 07:00:00",
                PlanCode = "f1ef5c99",
                scheduleNo = "614025757546647553",
                StartTime = "2023-10-01 06:00:00",
                TankCode = "Tank44",
                TankName = "117"
            };

            var res = url.Post<ChangeInTankLevelDto>(para);
            Series = new[]
            {
                new LineSeries<double>
                {
                    Values =res.TankData.Select(x=>x.LiquidLevel).ToArray(),
                    GeometrySize=0,
                    Fill=null,
                    DataPadding=new LiveChartsCore.Drawing.LvcPoint(0,0), 
                },
                new LineSeries<double>
                {
                    Values =res.TankData.Select(x=> res.MaxAllowLiquid).ToArray(),
                    GeometrySize=0,
                    Fill=null,
                    DataPadding=new LiveChartsCore.Drawing.LvcPoint(0,0),  },
                new LineSeries<double>
                {
                    Values =res.TankData.Select(x=> res.MinAllowLiquid).ToArray(),
                    GeometrySize=0,
                    Fill=null,
                    DataPadding=new LiveChartsCore.Drawing.LvcPoint(0,0),  }
            };
            XAxes = new Axis[] {
                new Axis { Name = para.TankName,
                           Labels = res.TankData.Select(x => x.DateTime.ToString("yyyy-MM-dd HH:mm:ss")).ToList() }
            };

        }
    }
}
