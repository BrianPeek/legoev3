using System;
using System.Globalization;

using Windows.UI.Xaml;

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