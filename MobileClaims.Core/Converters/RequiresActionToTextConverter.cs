using MobileClaims.Core.Entities;
using System;
using System.Globalization;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class RequiresActionToTextConverter : MvxValueConverter<ClaimActionState, string>
    {
        protected override string Convert(ClaimActionState value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == ClaimActionState.None 
                ? Resource.ClaimHistoryDetailRequriesCopFalse
                : Resource.ClaimHistoryDetailRequriesCopTrue;
        }
    }
}
