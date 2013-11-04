using System;
using System.Globalization;

using System.Windows.Data;

namespace SampleApp.Converters
{
	public abstract class ValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture, null);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ConvertBack(value, targetType, parameter, culture, null);
		}

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return Convert(value, targetType, parameter, null, language);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return ConvertBack(value, targetType, parameter, null, language);
		}

		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			throw new NotImplementedException();
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture, string language)
		{
			throw new NotImplementedException();
		}
	}
}