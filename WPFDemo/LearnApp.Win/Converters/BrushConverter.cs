using LearnApp.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace LearnApp.Win.Converters
{
    public class BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if ((TaskState)value == TaskState.Scheduling)
            {
                var color = Color.FromArgb(255, 21, 100, 190);
                return new SolidColorBrush(color);
            }
            if ((TaskState)value == TaskState.Success)
            {
                var color = Color.FromArgb(255, 81, 142, 51);
                return new SolidColorBrush(color);
            }
            if ((TaskState)value == TaskState.Failed)
            {
                var color = Color.FromArgb(255, 163, 51, 51);
                return new SolidColorBrush(color);
            }
            if ((TaskState)value == TaskState.NoResult)
            {
                var color = Color.FromArgb(255, 138, 138, 138);
                return new SolidColorBrush(color);
            }
            if ((TaskState)value == TaskState.Draft)
            {
                var color = Color.FromArgb(255, 138, 138, 138);
                return new SolidColorBrush(color);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
