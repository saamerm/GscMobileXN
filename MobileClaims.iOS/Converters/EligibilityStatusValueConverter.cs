using System;
using System.Globalization;
using MobileClaims.iOS;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class EligibilityStatusValueConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return LocalizableBrand.UnableToDetermineEligibility
                            .FormatWithBrandKeywords(LocalizableBrand.PhoneNumber);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}