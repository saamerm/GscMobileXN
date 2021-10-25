using System;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels.DirectDeposit
{
    public class DirectDepositStep3Model : MvxNotifyPropertyChanged
    {
        private bool _isOptedForNotification;

        public string OptInForEmailNotification { get; set; }

        public string OptOutOfEmailNotification { get; set; }

        public bool IsOptedForNotification
        {
            get => _isOptedForNotification;
            set => SetProperty(ref _isOptedForNotification, value);
        }
    }
}
