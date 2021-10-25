using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.Validators;
using MvvmCross;
using MvvmCross.Plugin.Messenger;


namespace MobileClaims.Core.ViewModels
{
    public class ClaimOtherBenefitsViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IParticipantService _participantservice;
        private readonly IRehydrationService _rehydrationservice;
        private Timer saveTimeout;

        private OtherBenefitsValidator _validator;

        public ClaimOtherBenefitsViewModel(IMvxMessenger messenger, IClaimService claimservice, IParticipantService participantservice)
        {
            saveTimeout = new Timer(s =>
            {
                _claimservice.PersistClaim();
            }, 1000);

            _messenger = messenger;
            _claimservice = claimservice;
            _participantservice = participantservice;
            _rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
            _validator = new OtherBenefitsValidator();

            if (_rehydrationservice.Rehydrating || this.CreatedFromRehydration)
            {
                _coverageUnderAnotherBenefitsPlan = _claimservice.Claim.CoverageUnderAnotherBenefitsPlan;
                _isOtherCoverageWithGSC = _claimservice.Claim.IsOtherCoverageWithGSC;
                _hasClaimBeenSubmittedToOtherBenefitPlan = _claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan;
                _payAnyUnpaidBalanceThroughOtherGSCPlan = _claimservice.Claim.PayAnyUnpaidBalanceThroughOtherGSCPlan;
                _otherGSCNumber = _claimservice.Claim.UnmodifiedOtherGSCNumber;
                _payUnderHCSA = _claimservice.Claim.PayUnderHCSA;
                SetIsQ4Enabled();
                RaiseAllPropertiesChanged();
            }

            CoverageUnderAnotherBenefitsPlan = _claimservice.Claim.CoverageUnderAnotherBenefitsPlan;
            IsOtherCoverageWithGSC = _claimservice.Claim.IsOtherCoverageWithGSC;
            HasClaimBeenSubmittedToOtherBenefitPlan = _claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan;
            PayAnyUnpaidBalanceThroughOtherGSCPlan = _claimservice.Claim.PayAnyUnpaidBalanceThroughOtherGSCPlan;
            OtherGSCNumber = _claimservice.Claim.UnmodifiedOtherGSCNumber;
            PayUnderHCSA = _claimservice.Claim.PayUnderHCSA;

            if (_participantservice.PlanMember != null
                && _participantservice.PlanMember.PlanConditions.SpendingAccountExists
                && !_participantservice.PlanMember.PlanConditions.IsAutoCoordinationOn
                && !_claimservice.Claim.Type.IsAutoCoordinationOn)
            {
                PayUnderHCSAVisible = true;
            }
            else
            {
                PayUnderHCSAVisible = false;
                PayUnderHCSA = false;
            }
            PersistClaim();
        }

        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimservice.SelectedClaimSubmissionType;
            }
        }

        private bool _coverageUnderAnotherBenefitsPlan = false;
        public bool CoverageUnderAnotherBenefitsPlan //Q1
        {
            get
            {
                return _coverageUnderAnotherBenefitsPlan;
            }
            set
            {
                _coverageUnderAnotherBenefitsPlan = value;
                _claimservice.Claim.CoverageUnderAnotherBenefitsPlan = _coverageUnderAnotherBenefitsPlan;
                Questions2Through5Visible = _coverageUnderAnotherBenefitsPlan;
                SetIsQ4Enabled();
                RaisePropertyChanged(() => CoverageUnderAnotherBenefitsPlan);
                PersistClaim();
            }
        }

        private bool _questions2Through5Visible = false;
        public bool Questions2Through5Visible
        {
            get
            {
                return _questions2Through5Visible;
            }
            set
            {
                _questions2Through5Visible = value;
                if (!_questions2Through5Visible)
                {
                    IsOtherCoverageWithGSC = false;
                    HasClaimBeenSubmittedToOtherBenefitPlan = false;
                    PayAnyUnpaidBalanceThroughOtherGSCPlan = false;
                    OtherGSCNumber = string.Empty;
                }
                RaisePropertyChanged(() => Questions2Through5Visible);
                PersistClaim();
            }
        }

        private bool _isOtherCoverageWithGSC = false;
        public bool IsOtherCoverageWithGSC //Q2
        {
            get
            {
                return _isOtherCoverageWithGSC;
            }
            set
            {
                _isOtherCoverageWithGSC = value;
                _claimservice.Claim.IsOtherCoverageWithGSC = _isOtherCoverageWithGSC;
                SetIsQ4Enabled();
                RaisePropertyChanged(() => IsOtherCoverageWithGSC);
                PersistClaim();
            }
        }

        private bool _hasClaimBeenSubmittedToOtherBenefitPlan = false;
        public bool HasClaimBeenSubmittedToOtherBenefitPlan //Q3
        {
            get
            {
                return _hasClaimBeenSubmittedToOtherBenefitPlan;
            }
            set
            {
                _hasClaimBeenSubmittedToOtherBenefitPlan = value;
                _claimservice.Claim.HasClaimBeenSubmittedToOtherBenefitPlan = _hasClaimBeenSubmittedToOtherBenefitPlan;
                SetIsQ4Enabled();
                RaisePropertyChanged(() => HasClaimBeenSubmittedToOtherBenefitPlan);
                PersistClaim();
            }
        }

        private bool _payAnyUnpaidBalanceThroughOtherGSCPlan = false;
        public bool PayAnyUnpaidBalanceThroughOtherGSCPlan //Q4
        {
            get
            {
                return _payAnyUnpaidBalanceThroughOtherGSCPlan;
            }
            set
            {
                _payAnyUnpaidBalanceThroughOtherGSCPlan = value;
                _claimservice.Claim.PayAnyUnpaidBalanceThroughOtherGSCPlan = _payAnyUnpaidBalanceThroughOtherGSCPlan;
                OtherGSCNumberEnabled = _payAnyUnpaidBalanceThroughOtherGSCPlan;
                RaisePropertyChanged(() => PayAnyUnpaidBalanceThroughOtherGSCPlan);
                PersistClaim();
            }
        }

        private string _otherGSCNumber;
        public string OtherGSCNumber //Q5
        {
            get
            {
                return _otherGSCNumber;
            }
            set
            {
                _otherGSCNumber = value;

                string pmID = string.Empty;
                string participantNumber = string.Empty;
                if (!string.IsNullOrEmpty(_otherGSCNumber))
                {
                    string[] split = _otherGSCNumber.Split('-');
                    if (split.Length == 2)
                    {
                        pmID = split[0];
                        participantNumber = split[1];
                    }
                    else
                    {
                        pmID = split[0];
                    }
                }
                _claimservice.Claim.UnmodifiedOtherGSCNumber = _otherGSCNumber;
                _claimservice.Claim.OtherGSCNumber = pmID;
                _claimservice.Claim.OtherGSCParticipantNumber = participantNumber;

                RaisePropertyChanged(() => OtherGSCNumber);
                PersistClaim();
            }
        }

        private bool _otherGSCNumberEnabled = false;
        public bool OtherGSCNumberEnabled
        {
            get
            {
                return _otherGSCNumberEnabled;
            }
            set
            {
                _otherGSCNumberEnabled = value;
                RaisePropertyChanged(() => OtherGSCNumberEnabled);
                PersistClaim();
            }
        }

        private bool _isQ4Enabled = false;
        public bool IsQ4Enabled
        {
            get
            {
                return _isQ4Enabled;
            }
            set
            {
                _isQ4Enabled = value;
                RaisePropertyChanged(() => IsQ4Enabled);
                PersistClaim();
            }
        }

        private bool _payUnderHCSA = false;
        public bool PayUnderHCSA
        {
            get
            {
                return _payUnderHCSA;
            }
            set
            {
                _payUnderHCSA = value;
                _claimservice.Claim.PayUnderHCSA = _payUnderHCSA;
                RaisePropertyChanged(() => PayUnderHCSA);
                PersistClaim();
            }
        }

        private bool _payUnderHCSAVisible = false;

        public bool PayUnderHCSAVisible
        {
            get
            {
                return _payUnderHCSAVisible;
            }
            set
            {
                _payUnderHCSAVisible = value;
                _claimservice.Claim.IsPayUnderHCSAVisible = _payUnderHCSAVisible;
                RaisePropertyChanged(() => PayUnderHCSAVisible);
                PersistClaim();
            }
        }

        public string HCSASubtitle
        {
            get { return _claimservice.HCSAName; }
        }

        public string AlternateHCSASubtitle
        {
            get { return _claimservice.HCSAName.ToUpperInvariant(); }
        }

        public string HCSAQuestion
        {
            get { return Resource.HCSAQuestion; }
        }

        public override Task RaisePropertyChanged(System.ComponentModel.PropertyChangedEventArgs changedArgs)
        {
            PersistClaim();
            return base.RaisePropertyChanged(changedArgs);
        }

        public override Task RaiseAllPropertiesChanged()
        {
            PersistClaim();
            return base.RaiseAllPropertiesChanged();
        }

        private void PersistClaim()
        {
            // If this is called multiple times in quick succession, only take the last one.
            saveTimeout.Reset();
        }

        public ValidationResult Validate()
        {
            ValidationResult results = new ValidationResult();
            if (OtherGSCNumberEnabled)
            {
                results = _validator.Validate<ClaimOtherBenefitsViewModel>(this, "OtherGSCNumber");
            }
            return results;
        }

        public void ClearData()
        {
            this.PayUnderHCSA = false;
            this.OtherGSCNumber = string.Empty;
            this.PayAnyUnpaidBalanceThroughOtherGSCPlan = false;
            this.HasClaimBeenSubmittedToOtherBenefitPlan = false;
            this.IsOtherCoverageWithGSC = false;
            this.CoverageUnderAnotherBenefitsPlan = false;
        }

        private void SetIsQ4Enabled()
        {
            if (CoverageUnderAnotherBenefitsPlan && IsOtherCoverageWithGSC && !HasClaimBeenSubmittedToOtherBenefitPlan)
            {
                IsQ4Enabled = true;
            }
            else
            {
                IsQ4Enabled = false;
                PayAnyUnpaidBalanceThroughOtherGSCPlan = false;
                if (!_rehydrationservice.Rehydrating)
                {
                    OtherGSCNumber = string.Empty;
                    OtherGSCNumberEnabled = false;
                }
            }
        }
    }

    public class PlanMemberIdValidationMessages
    {
        public const string EmptyString = "Empty";        
        public const string InvalidGSCPlanMemberId = "Invalid Number";
        public const string NonAlphaNumbericGSCPlanMemberId = "Not Alphanumeric";
        public const int PlanMemberIdMinLength = 1;
        public const int PlanMemberIdMaxLength = 14;
    }

    public class PlanMemberValidation
    {
        public const string Regex = @"^[a-zA-Z0-9]+(?:[-]*[a-zA-Z0-9]+)*$";
    }
}
