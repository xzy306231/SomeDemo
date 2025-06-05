using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LearnApp.Shared.Enum
{
    public enum TaskState
    {
        [Description("正在计算")]
        Scheduling = 0,
        [Description("排产成功")]
        Success = 1,
        [Description("排产失败")]
        Failed = 2,
        [Description("无排产结果")]
        NoResult = 3,
        [Description("草稿")]
        Draft = 4
    }
}
