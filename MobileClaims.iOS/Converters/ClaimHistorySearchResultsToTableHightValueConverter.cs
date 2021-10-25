using System;
using System.Globalization;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities.ClaimsHistory;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class ClaimHistorySearchResultsToTableHightValueConverter : MvxValueConverter<ObservableCollection<ClaimState>, float>
    {
        protected override float Convert(ObservableCollection<ClaimState> value, Type targetType, object parameter, CultureInfo culture)
        {
            float result = 0f;
            if (value != null)
            {
                if (value is ObservableCollection<ClaimState>)
                {
                    ObservableCollection<ClaimState> coll = value as ObservableCollection<ClaimState>;
                    result = coll.Count * 90f;
                }
            }
            return (float)result;
        }

        public ObservableCollection<ClaimState> ConvertBack(float value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            return new ObservableCollection<ClaimState>();
        }
    }
     
}

