using MobileClaims.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using MvvmCross;

namespace MobileClaims.Core.Entities.HCSA
{
    [JsonObject]
    public class ClaimDetail : SelectableItem
    {
        private Claim _parentclaim;
        [JsonIgnore]
        public Claim ParentClaim
        {
            get => _parentclaim;
            set
            {
                _parentclaim = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ParentClaim"));
            }
        }

        [JsonIgnore]
        public ExpenseType ExpenseType => ParentClaim.ExpenseType;

        [JsonIgnore]
        public string ExpenseTypeDescription
        {
            get
            {
                if (ParentClaim.ExpenseType != null)
                {
                    if (!string.IsNullOrEmpty(ParentClaim.ExpenseType.Name))
                    {
                        return ParentClaim.ExpenseType.Name;
                    }
                    else
                    {
                        return ParentClaim.ClaimType != null ? ParentClaim.ClaimType.Name : string.Empty;
                    }
                }
                else
                {
                    return ParentClaim.ClaimType != null ? ParentClaim.ClaimType.Name : string.Empty;
                }
            }
        }
        [JsonIgnore]
        public ClaimType ClaimType => ParentClaim.ClaimType;

        public ClaimDetail() : base() { }
        private DateTime? _expensedate;// = Mvx.IoCProvider.Resolve<IDeviceService>().CurrentDevice==GSCHelper.OS.Droid ? string.Empty : DateTime.Now.ToString("d");
        [JsonProperty(PropertyName ="expenseDate")]
        public DateTime? ExpenseDate
        {
            get => _expensedate;
            set
            {
                if (_expensedate != value)
                {
                    _expensedate = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ExpenseDate"));
                }
            }
        }

        private double _claimamount;
        [JsonProperty(PropertyName ="claimAmount")]
        public double ClaimAmount
        {
            get => _claimamount;
            set
            {
                if (_claimamount != value)
                {
                    _claimamount = value;
                    _claimamountstring = _claimamount.ToString();
                    RaisePropertyChanged(new PropertyChangedEventArgs("ClaimAmount"));
                    RaisePropertyChanged(new PropertyChangedEventArgs("ClaimAmountString"));
                }
            }
        }

        private string _claimamountstring;
        [JsonIgnore]
        public string ClaimAmountString
        {
            get => _claimamountstring;
            set
            {
                _claimamountstring = value;
                double test;
                if (double.TryParse(value, out test))
                {
                    ClaimAmount = test;
                }
                RaisePropertyChanged(new PropertyChangedEventArgs("ClaimAmountString"));
            }
        }

        private string _otherpaidamountstring;
        [JsonIgnore]
        public string OtherPaidAmountString
        {
            get => _otherpaidamountstring;
            set
            {
                _otherpaidamountstring = value;
                double test;
                if(double.TryParse(value, out test))
                {
                    OtherPaidAmount = test;
                }
                RaisePropertyChanged(new PropertyChangedEventArgs("OtherPaidAmountString"));
            }
        }

        private double _otherpaidamount;
        [JsonProperty(PropertyName ="otherPaidAmount")]
        public double OtherPaidAmount
        {
            get => _otherpaidamount;
            set
            {
                if (_otherpaidamount != value)
                {
                    _otherpaidamount = value;
                    _otherpaidamountstring = value.ToString();
                    RaisePropertyChanged(new PropertyChangedEventArgs("OtherPaidAmount")); //why are we using this kind of propertychanged notification?  It's very brittle
                    RaisePropertyChanged(new PropertyChangedEventArgs("OtherPaidAmountString"));
                }
            }
        }

        [JsonIgnore]
        public double PaidAmount { get; set; }

        [JsonIgnore]
        public string ClaimStatus { get; set; }

        [JsonIgnore]
        public long ClaimFormID { get; set; }

        [JsonIgnore]
        public List<ClaimEOBMessageGSC> EOBMessages { get; set; }

        [JsonIgnore]
        public string DateOfExpenseLabel => Resource.DateOfExpenseNoColon;

        [JsonIgnore]
        public string GovernmentPlanLabel => Resource.OtherPaidAmount;

        [JsonIgnore]
        public string TypeExpenseLabel => Resource.TypeOfExpenseNoColon;

        [JsonIgnore]
        public string ClaimedAmountLabel => Resource.ClaimedAmount;

        [JsonIgnore]
        public string PaidAmountLabel => Resource.PaidAmount;

        [JsonIgnore]
        public string ClaimStatusLabel => Resource.ClaimStatus;

        [JsonIgnore]
        public string OtherPaidLabel => Resource.OtherPaidAmount;

        [JsonIgnore]
        public string AwaitingPaymentLabel => Resource.AwaitingPayment;

        public string FormNumberLabel => Resource.FormNumber;

        public string EOBLabel => string.Empty; // Resource.ExplanationOfBenefits; }
    }

}
