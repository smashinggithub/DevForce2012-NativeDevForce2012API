using System;
using System.Globalization;
using System.Windows.Data;

namespace ClientUIDataApp5.Converters
{
    public class CustomGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int categoryID = (int)value;
            if (categoryID >= 1 && categoryID <= 3)
            {
                return "1 - 3";
            }
            else if (categoryID >= 4 && categoryID <= 6)
            {
                return "4 - 6";
            }
            else
            {
                return ">7";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
