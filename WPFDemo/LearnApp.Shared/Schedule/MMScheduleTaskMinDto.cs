using LearnApp.Shared.BaseBusinessDto;
using LearnApp.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.Schedule
{
    public class MMScheduleTaskMinDto : BaseDto
    {
        public string PlanName { get; set; }
        public TaskState State { get; set; }
        public string TaskId { get; set; }
        public string Memo { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUserName { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyUserName { get; set; }
        public ArchiveState ArchiveState { get; set; }
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 排产结束时间
        /// </summary>
        public DateTime? SchedulingEndTime { get; set; }
        public bool IsPublish { get; set; }
        public DateTime? PublishTime { get; set; }
        public string PublishName { get; set; }
        public string Period
        {
            get
            {
                if (SchedulingEndTime.HasValue)
                {
                    var days = Math.Floor((SchedulingEndTime.Value - StartTime).TotalDays);
                    var hours = (SchedulingEndTime.Value - StartTime).TotalHours - days * 24;
                    return $"{days}天{hours}小时";
                }
                else
                    return null;
            }
        }
    }
}
