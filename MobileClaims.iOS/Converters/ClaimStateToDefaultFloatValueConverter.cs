using System;
using System.Globalization;
using MobileClaims.Core.Entities;
using MvvmCross.Converters;

namespace MobileClaims.iOS.Converters
{
    public class ClaimStateToDefaultFloatValueConverter : MvxValueConverter<ClaimActionState, nfloat>
    {
        private bool _inverted;

        public ClaimStateToDefaultFloatValueConverter()
        {
        }

        public ClaimStateToDefaultFloatValueConverter(bool inverted = false)
        {
            _inverted = inverted;
        }

        protected override nfloat Convert(ClaimActionState value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ClaimActionState.Audit:
                    return _inverted ? (nfloat)parameter : 0;
                case ClaimActionState.Cop:
                    return _inverted ? 0 : (nfloat)parameter;             
                default:
                    return 0;
            }
        }
    }
}
