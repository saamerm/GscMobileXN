using System;
using System.Globalization;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class ClaimHistoryTypeHighlightValueConverter : MvxValueConverter<ClaimHistoryType, bool>
    {  
        protected override bool Convert(ClaimHistoryType value, Type targetType, object parameter, CultureInfo culture)
        { 
            bool result = false;
            ClaimHistoryType cht = (ClaimHistoryType)value;
            string para = parameter.ToString();
            switch (para)
            {
                case "1":
                    if (cht.ID.Equals(GSCHelper.DefaultClaimsHistoryTypeID))
                        result = true;
                    break;
                case "2":
                    if (cht.ID.Equals(GSCHelper.ClaimHistoryDentalPredeterminationID))
                        result = true;
                    break;
                case "3":
                    if (cht.ID.Equals(GSCHelper.ClaimHistoryMedicalItemID))
                        result = true;
                    break;
            }  
            return result;
        }

        public object ConvertBack(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            return  null;
        }
    }
}

