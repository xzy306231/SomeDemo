using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.Tank
{
    public class GanttGetDataInputDto
    {
        /// <summary>
        /// 排产单号
        /// </summary>
        public string scheduleNo { get; set; }
        /// <summary>
        /// 储罐编号（获取储罐液位变化曲线的接口）
        /// </summary>
        public string TankCode { get; set; }
        public string TankName { get; set; }
        /// <summary>
        /// 罐区编码 （库存信息-获取罐周转率的接口）
        /// </summary>
        public string AreaCode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string PlanCode { get; set; }
    }
}
