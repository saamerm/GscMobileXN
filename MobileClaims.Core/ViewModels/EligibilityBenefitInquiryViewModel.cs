using FluentValidation;
using FluentValidation.Results;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using MobileClaims.Core.Validators;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class EligibilityBenefitInquiryViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IEligibilityService _eligibilityservice;
        private readonly IParticipantService _participantservice;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private MvxSubscriptionToken _submissioncomplete;

        public EligibilityBenefitInquiryViewModel(IMvxMessenger messenger, IEligibilityService eligibilityservice, IParticipantService participantservice)
        {
            _messenger = messenger;
            _eligibilityservice = eligibilityservice;
            _participantservice = participantservice;

            SelectedParticipants = _eligibilityservice.SelectedParticipantsForBenefitInquiry;

            _shouldcloseself = _messenger.Subscribe<ClearEligibilityBenefitInquiryRequest >((message) =>
            {
                _messenger.Unsubscribe<ClearEligibilityBenefitInquiryRequest>(_shouldcloseself);
                Close(this);
            });
          
        }

        public EligibilityCheckType EligibilityCheckType => _eligibilityservice.SelectedEligibilityCheckType;

        public PlanMember PlanMember => _participantservice.PlanMember;

        private List<ParticipantEligibilityResult> _selectedParticipants;
        public List<ParticipantEligibilityResult> SelectedParticipants
        {
            get => _selectedParticipants;
            set
            {
                _selectedParticipants = value;
                RaisePropertyChanged(() => SelectedParticipants);
            }
        }

        private string _inquiryText;
        public string InquiryText
        {
            get => _inquiryText;
            set
            {
                _inquiryText = value;
                RaisePropertyChanged(() => InquiryText);
            }
        }

        private bool _invalidInquiryText = false;
        public bool InvalidInquiryText
        {
            get => _invalidInquiryText;
            set
            {
                _invalidInquiryText = value;
                RaisePropertyChanged(() => InvalidInquiryText);
            }
        }

        private bool _inquirySubmitted = false;
        public bool InquirySubmitted
        {
            get => _inquirySubmitted;
            set
            {
                _inquirySubmitted = value;
                RaisePropertyChanged(() => InquirySubmitted);
            }
        }

        public ICommand SubmitBenefitInquiryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    if (IsValid())
                    {

                        if (this.PlanMember != null)
                        {
                            if (!string.IsNullOrEmpty(this.PlanMember.PlanMemberID))
                            {
                                _submissioncomplete = _messenger.Subscribe<EligibilityInquirySubmissionComplete>((message) =>
                                {
                                    _messenger.Unsubscribe<EligibilityInquirySubmissionComplete>(_submissioncomplete);
                                    InquirySubmitted = true;
                                });

                                long pmid;
                                long.TryParse(this.PlanMember.PlanMemberID.Split('-')[0], out pmid);
                                EligibilityInquiry ei = new EligibilityInquiry();
                                ei.PlanMemberID = pmid;
                                ei.Body = InquiryText;
                                ei.ParticipantEligibilityResults = SelectedParticipants;

                                _eligibilityservice.EligibilityInquiry(ei);
                            }
                        }
                    }
                });
            }
        }

        public ICommand CancelBenefitInquiryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Close(this);
                });
            }
        }

        public ICommand SubmitAnotherEligibilityCheckCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    this.ShowViewModel<EligibilityCheckTypesViewModel>();
                });
            }
        }

        private bool IsValid()
        {
            // Reset for validation      
            InvalidInquiryText = false;
            EligibilityBenefitInquiryValidator validator = new EligibilityBenefitInquiryValidator();
            ValidationResult result = null;
			//TODO: this view model is no longer used, but if it's brought back in, may need to look into this validator.
            result = validator.Validate<EligibilityBenefitInquiryViewModel>(this,"");
            if (!result.IsValid)
            {
                RaiseOnInvalidEligibilityCheck(new EventArgs());
                return false;
            }
            return true;
        }

        public event EventHandler OnInvalidEligibilityCheck;
        protected virtual void RaiseOnInvalidEligibilityCheck(EventArgs e)
        {
            if (this.OnInvalidEligibilityCheck != null)
            {
                OnInvalidEligibilityCheck(this, e);
            }
        }
    }
}
