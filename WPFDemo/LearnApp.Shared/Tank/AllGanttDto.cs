using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.Tank
{
    public class AllGanttDto
    {
        public List<GanttDto> Gantt { get; set; }
        public List<DateTime> rollTimeList { get; set; }

        public bool IsIgnoreProcessTankArea { get; set; }

        public string ScheduleId { get; set; }
    }
    public class GanttDto
    {
        public int categoryType { get; set; }
        public string realCategoryName { get; set; }
        public string categoryName { get; set; }
        public string categoryCode { get; set; }
        public string area { get; set; }
        public string color { get; set; }
        public int sort { get; set; }
        public string memo { get; set; }
        public double Height { get; set; } = 25;
        public bool isVtank { get; set; } = false;
        public List<dataInfo> dataInfo { get; set; }
    }
    public class dataInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int offLoadType { get; set; }
        public string toCans { get; set; }
        public string toArea { get; set; }
        public string dataId { get; set; }
        public string batchId { get; set; } = Guid.NewGuid().ToString();
        public DateTime startTime { get; set; }
        public DateTime? planUnloadTime { get; set; }
        public DateTime endTime { get; set; }
        public string type { get; set; }
        public decimal quantity { get; set; }
        public decimal speed { get; set; }
        public string color { get; set; }
        public string toolTip { get; set; }
        public string oilNames { get; set; }
        public string oilMixPlan { get; set; }
        public bool isInOut { get; set; }
        public string actionType { get; set; }
        public string typeName { get; set; }
        public string RouteCode { get; set; }
        public string RouteName { get; set; }
        public string RouteColor { get; set; }
        public decimal ShipConflictHour { get; set; }
        public decimal NotUseQuatity { get; set; }
        public double Width { get; set; }
        public double LeftWidth { get; set; }
        public double TimeLength { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsHandle { get; set; } = false;
    }
}
