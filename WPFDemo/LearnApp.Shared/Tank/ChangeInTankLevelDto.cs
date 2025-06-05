using LearnApp.Shared.Schedule;
using LearnApp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.Tank
{
    public class ChangeInTankLevelDto
    {
        public string CanCode { get; set; }
        public double MaxAllowLiquid { get; set; }
        public double MinAllowLiquid { get; set; }
        public List<ChangeInTankLevelDetel> TankData { get; set; }
    }
    public class ChangeInTankLevelDetel
    {
        public long Timestamp { get; set; }
        public double LiquidLevel { get; set; }
        public decimal? OilWeight { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public List<FdPlanlibraryOilcombination> FdPlanlibraryOilcombination { get; set; }

        public DateTime DateTime
        {
            get
            {
                return Timestamp.GetDateTimeMilliseconds();
            }
        }

    }
}
