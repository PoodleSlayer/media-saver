using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PhotoHelper.Utility.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool valueAsBool = (bool)value;
			if (valueAsBool)
			{
				return Color.LightGreen;
			}
			else
			{
				return Color.White;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// this converter only binds one-way so ConvertBack doesn't make sense to use
			return null;
		}
    }
}
