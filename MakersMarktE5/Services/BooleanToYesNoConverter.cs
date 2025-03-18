using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Services
{
	public class BooleanToYesNoConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (bool)value ? "Yes" : "No";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return (string)value == "Yes";
		}
	}
}
