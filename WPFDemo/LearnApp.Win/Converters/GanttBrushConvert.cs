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
    public class GanttBrushConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null && values[1] == null)
            {
                var color = Color.FromArgb(255,223, 224, 226);
                return new SolidColorBrush(color);
            }

            if (values[0].ToString().StartsWith("unload"))
            {
                var color = Color.FromArgb(255, 141, 169, 253);
                return new SolidColorBrush(color);
            }
            if (values[0].ToString().StartsWith("transfer-1"))
            {
                var color = Color.FromArgb(255, 252, 235, 180);
                return new SolidColorBrush(color);
            }
            if (values[0].ToString().StartsWith("transfer-2"))
            {
                var color = Color.FromArgb(255, 222, 178, 146);
                return new SolidColorBrush(color);
            }
            if (values[0].ToString().StartsWith("transfer-3"))
            {
                var color = Color.FromArgb(255, 213, 192, 92);
                return new SolidColorBrush(color);
            }
            if (values[0].ToString().StartsWith("process"))
            {
                if (values[1].ToString() == "Route1")
                {
                    var color = Color.FromArgb(255, 38, 108, 76);
                    return new SolidColorBrush(color);
                }
                if (values[1].ToString() == "Route2")
                {
                    var color = Color.FromArgb(255, 18, 147, 164);
                    return new SolidColorBrush(color);
                }
                if (values[1].ToString() == "Route3")
                {
                    var color = Color.FromArgb(255, 79, 75, 159);
                    return new SolidColorBrush(color);
                }
                if (values[1].ToString() == "Route4")
                {
                    var color = Color.FromArgb(255, 35, 96, 159);
                    return new SolidColorBrush(color);
                }
                if (values[1].ToString() == "Route5")
                {
                    var color = Color.FromArgb(255, 38, 108, 76);
                    return new SolidColorBrush(color);
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
