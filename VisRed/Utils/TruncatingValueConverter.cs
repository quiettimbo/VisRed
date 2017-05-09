using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using StackExchange.Redis;

namespace VisRed.Utils
{
    public class TruncatingValueConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (value is HashEntry)
            {
                var hash = (HashEntry)value;
                s = $"{hash.Name}: {hash.Value}";
            }
            if (s != null && targetType == typeof(String) && s.Length > 30)
            {
                return s.Substring(0, 30) + " . . .";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
