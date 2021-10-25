using System;
using System.Globalization;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities.ClaimsHistory;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class ClaimHistorySearchResultsToTotalValueConverter : MvxValueConverter<ObservableCollection<ClaimState>,double>
    {
        protected override double Convert(ObservableCollection<ClaimState> value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0.0;
            if (value != null)
            {
                if (value is ObservableCollection<ClaimState>)
                {
                    ObservableCollection<ClaimState> coll = value as ObservableCollection<ClaimState>;
                    foreach (ClaimState cs in coll)
                    {
                        result += cs.ClaimedAmount;
                    } 
                }
            }
            return (double)result;
        }

        public ObservableCollection<ClaimState> ConvertBack(double value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            return new ObservableCollection<ClaimState>();
        }
    }
}

