using LearnApp.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace LearnApp.Win.Converters
{
    public class TaskStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((TaskState)value == TaskState.Scheduling)
                return "正在计算";
            if ((TaskState)value == TaskState.Success)
                return "排产成功";
            if ((TaskState)value == TaskState.Failed)
                return "排产失败";
            if ((TaskState)value == TaskState.NoResult)
                return "无排产结果";
            if ((TaskState)value == TaskState.Draft)
                return "草稿";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
