using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LibraryManagementSystem
{
    public class AlternatingRowConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                switch (index % 3)
                {
                    case 0: return Brushes.Gray;   // Серый
                    case 1: return Brushes.White;  // Белый
                    case 2: return Brushes.Black;  // Чёрный
                }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
