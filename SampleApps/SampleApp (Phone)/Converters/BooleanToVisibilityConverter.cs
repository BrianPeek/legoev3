using System;
using System.Globalization;
using System.Windows;
#if WINDOWS_STORE

using Windows.UI.Xaml;

#elif WINDOWS_PHONE
#endif

namespace SampleApp.Converters
{
    public class BooleanToVisibilityConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
			var boolValue = System.Convert.ToBoolean(value);

			if (parameter != null)
				boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
			throw new NotImplementedException();
        }
    }
}