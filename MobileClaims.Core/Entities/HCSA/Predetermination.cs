using System.ComponentModel;

namespace MobileClaims.Core.Entities.HCSA
{
    public class Predetermination : SelectableItem
    {
        public Predetermination() : base() { }
        private long _claimtypeid;
        public long ClaimTypeId
        {
            get => _claimtypeid;
            set
            {
                if (_claimtypeid != value)
                {
                    _claimtypeid = value; 
                    RaisePropertyChanged(new PropertyChangedEventArgs("ClaimTypeId"));
                }
            }
        }

        private long _expensetypeid;

        public long ExpenseTypeID
        {
            get => _expensetypeid;
            set
            {
                if (_expensetypeid !=value)
                {
                    _expensetypeid = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ExpenseTypeID"));
                }
            }
        }

        private bool _manualsubmission;

        public bool ManualSubmission
        {
            get => _manualsubmission;
            set
            {
                if (_manualsubmission !=value)
                {
                    _manualsubmission = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ManualSubmission"));
                }
            }
        }

        private bool _referralrequired;
        public bool ReferralRequired
        {
            get => _referralrequired;
            set
            {
                if (_referralrequired != value)   
                {
                    _referralrequired = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ReferralRequired"));
                }
            }
        }
    }
}
