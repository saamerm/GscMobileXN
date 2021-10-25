using System;
using System.Globalization;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities.ClaimsHistory;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class ClaimHistorySearchResultSummaryTableHeightValueConverter
        : MvxValueConverter<ObservableCollection<ClaimHistorySearchResultSummary>, float>
    {
        protected override float Convert(ObservableCollection<ClaimHistorySearchResultSummary> value, Type targetType, object parameter, CultureInfo culture)
        {
            float result = 0f;
            if (value != null)
            {
                if (value is ObservableCollection<ClaimHistorySearchResultSummary>)
                {
                    ObservableCollection<ClaimHistorySearchResultSummary> coll = value as ObservableCollection<ClaimHistorySearchResultSummary>;
                    result = coll.Count * Constants.ClaimHistoryCountTableViewCellHeight;
                }
            }
            return (float)result;
        }

        public ObservableCollection<ClaimHistorySearchResultSummary> ConvertBack(float value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}