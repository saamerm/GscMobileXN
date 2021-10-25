using System;
using System.Reflection;
using UIKit;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Target;

namespace MobileClaims.iOS
{
	public class CustomUIDatePickerDateTargetBinding : MvxBaseUIDatePickerTargetBinding
	{
		public CustomUIDatePickerDateTargetBinding(object target, PropertyInfo targetPropertyInfo)
			: base(target, targetPropertyInfo)
		{
		}

		protected override object GetValueFrom(UIDatePicker view)
		{
			return ((DateTime) view.Date).Date;
		}

		protected override object MakeSafeValue(object value)
		{
			if (value == null)
				value = DateTime.Now;
			var date = new DateTime(
				((DateTime)value).Year,
				((DateTime)value).Month,
				((DateTime)value).Day,
				0,
				0,
				0,
				DateTimeKind.Local);
			NSDate nsDate = (NSDate)date;
			return nsDate;
		}
	}
}

