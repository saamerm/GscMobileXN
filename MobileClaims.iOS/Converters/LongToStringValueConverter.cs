﻿using System;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.UI;
using MobileClaims.iOS.Views;

namespace MobileClaims.Core.Converters
{
	public class LongToStringValueConverter : MvxValueConverter
	{
		public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (((long)value).ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
			return value;
		}
	}
}