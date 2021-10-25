using System;
using System.Globalization;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities.ClaimsHistory;
using MvvmCross.Converters;

namespace MobileClaims.Core.Converters
{
    public class ClaimHistorySearchResultsToCountValueConverter : MvxValueConverter<ObservableCollection<ClaimState>, int>
    {
        protected override int Convert(ObservableCollection<ClaimState> value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = 0;
            if (value != null)
            {
                if (value is ObservableCollection<ClaimState>)
                {
                    ObservableCollection<ClaimState> coll = value as ObservableCollection<ClaimState>;
                    result = coll.Count;  
                }
            }
            return result;
        }

        public ObservableCollection<ClaimState> ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            return new ObservableCollection<ClaimState>();
        }
    }
}

