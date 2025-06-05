using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LearnApp.Win.Converters
{
    public class GanttFontConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null)
            {
                var color = Color.FromArgb(255, 51, 51, 51);
                return new SolidColorBrush(color);
            }

            if (values[0].ToString().StartsWith("process"))
            {
                var color = Color.FromArgb(255, 255, 255, 255);
                return new SolidColorBrush(color);
            }
            else
            {
                var color = Color.FromArgb(255, 51, 51, 51);
                return new SolidColorBrush(color);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
