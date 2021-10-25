using System;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DirectDepositStep1Model : MvxNotifyPropertyChanged
    {
        private bool _isDirectDepositAuthorized;

        public string OptInForDirectDeposit { get; set; }

        public bool IsDirectDepositAuthorized
        {
            get => _isDirectDepositAuthorized;
            set
            {
                SetProperty(ref _isDirectDepositAuthorized, value);
                RaisePropertyChanged();
            }
        }
    }
}
