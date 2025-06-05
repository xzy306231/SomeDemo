using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.Schedule
{
    public class MMScheduleInfoDto
    {
        public List<AutoSchedulePlanInfo> SchemeList { get; set; }
    }
    public class ProcessScheme
    {
        public string ProcessName { get; set; }
        public string ProcessCode { get; set; }
        public List<AutoSchedulePlanInfo> SchemeList { get; set; }
    }
    public class AutoSchedulePlanInfo
    {
        public bool? UnParallelRegionMix { get; set; }
        public int PlanSysCode { get; set; }
        public string ProcessName { get; set; }
        public string ProcessCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PlanCode { get; set; }
        public decimal TotalWeight { get; set; }
        public decimal LimitMaxWeight { get; set; }
        public decimal TotalVolume { get; set; }
        public string Route { get; set; }
        public string RouteCode { get; set; }
        public decimal Speed { get; set; }

        public List<FdPlanlibraryOilcombination> FdPlanlibraryOilcombination { get; set; }
        public List<FdPlanlibraryOilcombination> AliaList { get; set; }
        public List<FdPlanlibraryOilcombination> RawAliaList { get; set; }
        public double Width { get; set; }

        public string Plans
        {
            get
            {
                return string.Join(":", FdPlanlibraryOilcombination.Select(x => x.oilName).ToList());
            }
        }
    }
    public class FdPlanlibraryOilcombination
    {
        public string oilName { get; set; }
        public string oilCode { get; set; }
        public decimal oilCounted { get; set; }
        public decimal Weight { get; set; }
        public string oilShortName { get; set; }
        public string OilRealName { get; set; }

    }
}
