using System;
using System.Globalization;

#if WINDOWS_STORE

using Windows.UI.Xaml;

#elif WINDOWS_PHONE

using System.Windows;

#endif

namespace SampleApp.Converters
{
    public class IndexToVisibilityConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
			var boolValue = false;

	        if (parameter != null)
	        {
				boolValue = value.ToString() == parameter.ToString();
	        }
	        else
	        {
		        boolValue = System.Convert.ToInt32(value) == 0;
	        }

	        return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return value.Equals(Visibility.Visible);
        }
    }
}