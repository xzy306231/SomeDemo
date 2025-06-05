using LearnApp.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LearnApp.Shared.Tank
{
    public class ScheduleDto
    {
        public string Id { get; set; }
        public bool? IsTankSelf { get; set; }
        public bool IsCoverResult { get; set; } = false;
        public bool IsFromSchedule { get; set; } = false;
        public long ScheduleNo { get; set; }
        public string PlanName { get; set; }
        public string PlanCode { get; set; }

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public long Pid { get; set; } = 0;
        public int State { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? SolveTime { get; set; }
        public DateTime? ModifyTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string UserName { get; set; }
        public string RealName { get; set; }
        public string CreateUserName { get; set; }
        public string CreateUser { get { return CreateUserName; } }
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Memo { get; set; }
        public ScheduleDto Parent { get; set; }
        /// <summary>
        /// 0或null起作用，1不起作用
        /// </summary>
        public bool? IsEffect { get; set; }
        [JsonIgnore]
        public string ScheduleInputJson { get; set; }

        public bool? IsProcessLineHaveStore { get; set; } = true;

        [NotMapped]
        public List<ScheduleDto> ResultList { get; set; }

        public bool EnableOilAlias { get; set; } = true;
        public string OilAlias { get; set; }

        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public long ArchiveId { get; set; }
        public ArchiveState ArchiveState { get; set; }
        public string TimeLength
        {
            get
            {
                if (EndTime.HasValue)
                {
                    var days = Math.Floor((EndTime.Value - StartTime.Value).TotalDays);
                    var hours = Math.Round((EndTime.Value - StartTime.Value).TotalHours, 0) - days * 24;
                    return $"{days}天{hours}小时";
                }
                else
                    return null;
            }
            set { }
        }
    }
}
