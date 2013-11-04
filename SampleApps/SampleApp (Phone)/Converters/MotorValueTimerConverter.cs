using System;
using System.Globalization;

#if WINDOWS_STORE

using Windows.UI.Xaml;

#elif WINDOWS_PHONE

using System.Windows;

#endif

namespace SampleApp.Converters
{
	public class MotorValueTimerConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
			int roundValue = 2;

	        string returnValue = "";

	        if (parameter != null)
	        {
		        int.TryParse(parameter.ToString(), out roundValue);
	        }

	        if (value != null)
	        {
		        double parseValue;
		        double.TryParse(value.ToString(), out parseValue);

		        if (parseValue <= 0)
		        {
			        returnValue = "continuous";
		        }
		        else
		        {
			        returnValue = Math.Round(parseValue, roundValue).ToString(CultureInfo.InvariantCulture) + " sec";
		        }
	        }

	        return returnValue;
        }

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
        {
            return value.Equals(Visibility.Visible);
        }
    }
}