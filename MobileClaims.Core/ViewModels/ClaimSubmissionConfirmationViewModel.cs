using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Extensions;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.HCSA;
using MobileClaims.Core.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimSubmissionConfirmationViewModel : ViewModelBase<ClaimSubmissionConfirmationViewModelParameters>, IFileNamesContainer, IClaimSubmitProperties
    {
        public event EventHandler ListsCreated;
        public event EventHandler OnInvalidClaim;
        public event EventHandler OnInvalidGSCNumber;
        public event EventHandler OnNoMatchedDependent;
        public event EventHandler OnInvalidSecondaryPlanNumber;
        public event EventHandler OnInvalidOnlineClaim;
        public event EventHandler OnMultipleMatch;
        public event EventHandler OnSecondaryAccountDisabled;
        public event EventHandler OnSecondaryAccountNotRegistered;
        public event EventHandler OnSecondaryAccountHasntAcceptedAgreement;

        private readonly IUserDialogs _userDialogs;
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly IHCSAClaimService _hcsaservice;
        private readonly IParticipantService _participantservice;
        private MvxSubscriptionToken _claimsuccessful;
        private MvxSubscriptionToken _claimfailed;
        private MvxSubscriptionToken _sessionExpired;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private ObservableCollection<ClaimQuestionAnswerPair> _claimDetailInput;
        private ObservableCollection<ClaimQuestionAnswerList> _claimTreatmentInput;

        private string _comments;
        private bool _isHCSAClaim;
        private bool _busy;
        private bool _isParticipantsListVisible;
        private bool _isCommentVisible;
        private bool _isTreatmentDetailsVisible;
        private bool _isAttachmentsVisible;
        private bool _isHealthProviderVisible;
        private ParticipantGSC _selectedParticipant;
        private IClaimSubmitProperties _properties;
        private NonRealTimeClaimType _claimType;
        private List<ParticipantGSC> _participants;

        public string Title { get; private set; }
        public string GscIdNumber => string.Format(Resource.GscIdNumber, BrandResource.BrandAcronym);
        public string ParticipantName => Resource.claimConfirm_participant;
        public string HealthProvider => Resource.BottomNavBarHealthProviderLabel;
        public string PlanInformation => Resource.PlanInformation;
        public string ClaimDetails => Resource.ClaimDetails.ToUpperInvariant();
        public string TreatmentDetails => Resource.TreatmentDetails;
        public string AdditionalInformation => Resource.AdditionalInformation.ToUpperInvariant();
        public string UploadDocumentType { get; private set; }
        public string HCSAQuestion => Resource.HCSAQuestion;
        public string FooterNotes => string.Format(Resource.ClaimSubmissionFootNotes, BrandResource.BrandFullDisplayName);
        public string DocumentsToUpload => Resource.DocumentsToUpload;
        public string SubmitButtonText => _claimservice.SelectedClaimSubmissionType.ID.Equals("drug", StringComparison.OrdinalIgnoreCase) ?
            Resource.Submit : Resource.SubmitClaim;
        public string ClaimDetailsReasonOfAccident => Resource.ClaimDetailsReasonOfAccident;
        public string ClaimDetailsOtherTypeOfAccident => Resource.ClaimDetailsOtherTypeOfAccident;

        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        public bool IsCommentVisible
        {
            get => _isCommentVisible;
            set => SetProperty(ref _isCommentVisible, value);
        }

        public bool IsTreatmentDetailsVisible
        {
            get => _isTreatmentDetailsVisible;
            set => SetProperty(ref _isTreatmentDetailsVisible, value);
        }

        public bool IsAttachmentsVisible
        {
            get => _isAttachmentsVisible;
            set => SetProperty(ref _isAttachmentsVisible, value);
        }

        public bool IsHealthProviderVisible
        {
            get => _isHealthProviderVisible;
            set => SetProperty(ref _isHealthProviderVisible, value);
        }

        public bool IsHCSAClaim
        {
            get => _isHCSAClaim;
            set => SetProperty(ref _isHCSAClaim, value);
        }

        public bool Busy
        {
            get => _busy;
            set
            {
                if (_busy == value) return;

                _busy = value;
                RaisePropertyChanged(() => Busy);

                _messenger.Publish(new BusyIndicator(this)
                {
                    Busy = _busy
                });
                InvokeOnMainThread(SubmitClaimCommand.RaiseCanExecuteChanged);
            }
        }

        public bool IsParticipantsListVisible
        {
            get => _isParticipantsListVisible;
            set => SetProperty(ref _isParticipantsListVisible, value);
        }

        public Claim Claim => _claimservice.Claim;

        public Entities.HCSA.Claim HCSAClaim => _hcsaservice.Claim;

        public ParticipantGSC SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                if (value == _selectedParticipant) return;

                _selectedParticipant = value;
                if (_selectedParticipant != null)
                {
                    string participantNumber = _selectedParticipant.ParticipantNumber;
                    _claimservice.Claim.OtherGSCParticipantNumber = participantNumber;
                }
                else
                {
                    _claimservice.Claim.OtherGSCParticipantNumber = string.Empty;
                }

                RaisePropertyChanged(() => SelectedParticipant);
            }
        }

        public List<ParticipantGSC> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        public ObservableCollection<ClaimQuestionAnswerPair> ClaimDetailInput
        {
            get => _claimDetailInput;
            set => SetProperty(ref _claimDetailInput, value);
        }

        public ObservableCollection<ClaimQuestionAnswerList> ClaimTreatmentInput
        {
            get => _claimTreatmentInput;
            set => SetProperty(ref _claimTreatmentInput, value);
        }

        public ObservableCollection<DocumentInfo> Attachments { get; set; } = new ObservableCollection<DocumentInfo>();

        public IMvxCommand SubmitClaimCommand { get; }

        public ClaimSubmissionConfirmationViewModel(IMvxMessenger messenger,
                                                    IClaimService claimservice,
                                                    IHCSAClaimService hcsaservice,
                                                    IParticipantService participantservice,
                                                    IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;
            _messenger = messenger;
            _claimservice = claimservice;
            _hcsaservice = hcsaservice;
            _participantservice = participantservice;

            _shouldcloseself = _messenger.Subscribe<ClearClaimSubmissionConfirmationViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimSubmissionConfirmationViewRequested>(_shouldcloseself);
                Close(this);
            });
            IsHCSAClaim = _claimservice.IsHCSAClaim;
            SubmitClaimCommand = new MvxCommand(ExecuteSubmitClaimCommand);

            ClaimDetailInput = new ObservableCollection<ClaimQuestionAnswerPair>();
            ClaimTreatmentInput = new ObservableCollection<ClaimQuestionAnswerList>();
        }

        public override void Prepare(ClaimSubmissionConfirmationViewModelParameters parameter)
        {
            if ((parameter.Uploadable != null) && (parameter.Attachments != null))
            {
                Attachments = parameter.Attachments;
                Comments = parameter.Comment;

                Title = parameter.Uploadable.Title;
                IsCommentVisible = parameter.Uploadable.IsCommentVisible && !string.IsNullOrWhiteSpace(Comments);
                UploadDocumentType = parameter.Uploadable.UploadDocumentType;
                IsAttachmentsVisible = Attachments.Any();

                if (_claimservice != null && string.IsNullOrWhiteSpace(_claimservice.Claim?.Provider?.DoctorName))
                {
                    IsHealthProviderVisible = true;
                }
            }
            _claimType = parameter.ClaimType;
        }

        public override void Prepare()
        {
            // Fix for Bug- 14511.                
            _claimType = _claimservice.SelectedClaimSubmissionType.ID.Equals("drug", StringComparison.OrdinalIgnoreCase) ?
                NonRealTimeClaimType.Drug : NonRealTimeClaimType.NotDefined;

            _properties = NonRealTimeUploadFactory.Create(_claimType, nameof(ClaimSubmissionConfirmationViewModel))
                    as IClaimSubmitProperties;

            Title = _properties.Title;
            IsCommentVisible = _properties.IsCommentVisible && !string.IsNullOrWhiteSpace(Comments);
            UploadDocumentType = _properties.UploadDocumentType;

            // There shouldn't be any attachment when execution reach to this else conditions
            // Becaues resume functionality only come to confirmation screen for non-drug claims.
            IsAttachmentsVisible = Attachments.Any();

            if (_claimservice != null && string.IsNullOrWhiteSpace(_claimservice.Claim?.Provider?.DoctorName))
            {
                IsHealthProviderVisible = true;
            }
        }

        public async override Task Initialize()
        {
            await base.Initialize();

            // This delay is to give the iOS view a chance to subscribe to ListCreated event.
            await Task.Delay(500);
            CreateClaimDetailsList();
            CreateTreatmentDetailsList();
            RaisePropertyChanged(nameof(IsCommentVisible));
            ListsCreated?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void RaiseInvalidClaim(EventArgs e)
        {
            OnInvalidClaim?.Invoke(this, e);
        }

        protected virtual void RaiseInvalidGSCNumber(EventArgs e)
        {
            OnInvalidGSCNumber?.Invoke(this, e);
        }

        protected virtual void RaiseNoMatchedDependent(EventArgs e)
        {
            OnNoMatchedDependent?.Invoke(this, e);
        }

        protected virtual void RaiseInvalidSecondaryPlanNumber(EventArgs e)
        {
            OnInvalidSecondaryPlanNumber?.Invoke(this, e);
        }

        protected virtual void RaiseInvalidOnlineClaim(EventArgs e)
        {
            OnInvalidOnlineClaim?.Invoke(this, e);
        }

        protected virtual void RaiseMultipleMatch(EventArgs e)
        {
            OnMultipleMatch?.Invoke(this, e);
        }

        protected virtual void RaiseSecondaryAccountDisabled(EventArgs e)
        {
            OnSecondaryAccountDisabled?.Invoke(this, e);
        }

        protected virtual void RaiseSecondaryAccountNotRegistered(EventArgs e)
        {
            OnSecondaryAccountNotRegistered?.Invoke(this, e);
        }

        protected virtual void RaiseSecondaryAccountHasntAcceptedAgreement(EventArgs e)
        {
            OnSecondaryAccountHasntAcceptedAgreement?.Invoke(this, e);
        }

        public string claimDetailsOtherBenefitsQuestion { get; private set; } = Resource.ClaimDetailsOtherBenefitsTitle;

        // Create flat list of claim details question-answers
        private void CreateClaimDetailsList()
        {
#if CCQ || FPPM
            var isMinorFromQuebecProvice = _participantservice.SelectedParticipant.IsResidentOfQuebecProvince()
                && _participantservice.SelectedParticipant.IsOrUnderAgeOf18();
            if (isMinorFromQuebecProvice && _claimservice.SelectedClaimSubmissionType.ID.IsVisionEnhancement())
            {
                claimDetailsOtherBenefitsQuestion = BrandResource.ClaimDetailsOtherBenefitsTitle;
            }
            else
            {
                claimDetailsOtherBenefitsQuestion = Resource.ClaimDetailsOtherBenefitsTitle;
            }
#else
            claimDetailsOtherBenefitsQuestion = Resource.ClaimDetailsOtherBenefitsTitle;
#endif
            var claimDetailsOtherBenefitsTitle = new ClaimQuestionAnswerPair()
            {
                Question = claimDetailsOtherBenefitsQuestion,
                Answer = Claim.CoverageUnderAnotherBenefitsPlan ? Resource.Yes : Resource.No
            };
            ClaimDetailInput.Add(claimDetailsOtherBenefitsTitle);

            if (Claim.IsIsOtherCoverageWithGSCVisible)
            {
                var claimDetailsOtherBenefitsWithGsc = new ClaimQuestionAnswerPair()
                {
                    Question = string.Format(Resource.ClaimDetailsOtherBenefitsWithGsc, BrandResource.BrandAcronym),
                    Answer = Claim.IsOtherCoverageWithGSC ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsOtherBenefitsWithGsc);
            }

            if (Claim.IsHasClaimBeenSubmittedToOtherBenefitPlanVisible)
            {
                var claimDetailsOtherBenefitsSubmitted = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsOtherBenefitsSubmitted,
                    Answer = Claim.HasClaimBeenSubmittedToOtherBenefitPlan ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsOtherBenefitsSubmitted);
            }

            if (Claim.IsPayAnyUnpaidBalanceThroughOtherGSCPlanVisible)
            {
                var claimDetailsOtherBenefitsPayBalanceThroughOtherGSC = new ClaimQuestionAnswerPair()
                {
                    Question = string.Format(Resource.ClaimDetailsOtherBenefitsPayBalanceThroughOtherGSC, BrandResource.BrandAcronym),
                    Answer = Claim.PayAnyUnpaidBalanceThroughOtherGSCPlan ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsOtherBenefitsPayBalanceThroughOtherGSC);
            }

            if (Claim.IsOtherGSCNumberVisible)
            {
                var claimDetailsOtherBenefitsGscNumber = new ClaimQuestionAnswerPair()
                {
                    Question = string.Format(Resource.ClaimDetailsOtherBenefitsGscNumber, BrandResource.BrandAcronym),
                    Answer = Claim.OtherGSCNumber
                };
                ClaimDetailInput.Add(claimDetailsOtherBenefitsGscNumber);
            }

            if (Claim.IsPayUnderHCSAVisible)
            {
                var claimDetailsOtherBenefitsHSCA = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsOtherBenefitsHSCA,
                    Answer = Claim.PayUnderHCSA ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsOtherBenefitsHSCA);
            }

            var wasClaimDueToAccident = new ClaimQuestionAnswerPair()
            {
                Question = Resource.ClaimDetailsReasonOfAccident,
                Answer = Claim.IsClaimDueToAccident ? Resource.Yes : Resource.No
            };
            ClaimDetailInput.Add(wasClaimDueToAccident);

            if (Claim.IsTreatmentDueToAMotorVehicleAccident)
            {
                var claimDetailsMotorVehicleTitle = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsMotorVehicleTitle,
                    Answer = Claim.IsTreatmentDueToAMotorVehicleAccident ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsMotorVehicleTitle);

                if (Claim.IsDateOfMotorVehicleAccidentVisible)
                {
                    var claimDetailsMotorVehicleDate = new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.ClaimDetailsMotorVehicleDate,
                        Answer = Claim.DateOfMotorVehicleAccident.ToString("d")
                    };
                    ClaimDetailInput.Add(claimDetailsMotorVehicleDate);
                }
            }
            else if (Claim.IsTreatmentDueToAWorkRelatedInjury)
            {
                var claimDetailsWorkInjuryTitle = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsWorkInjuryTitle,
                    Answer = Claim.IsTreatmentDueToAWorkRelatedInjury ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsWorkInjuryTitle);

                if (Claim.IsDateOfWorkRelatedInjuryVisible)
                {
                    var claimDetailsWorkInjuryDate = new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.ClaimDetailsWorkInjuryDate,
                        Answer = Claim.DateOfWorkRelatedInjury.ToString("d")
                    };
                    ClaimDetailInput.Add(claimDetailsWorkInjuryDate);
                }

                if (Claim.IsWorkRelatedInjuryCaseNumberVisible)
                {
                    var claimDetailsWorkInjuryCaseNumber = new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.ClaimDetailsWorkInjuryCaseNumber,
                        Answer = Claim.WorkRelatedInjuryCaseNumber.ToString()
                    };
                    ClaimDetailInput.Add(claimDetailsWorkInjuryCaseNumber);
                }
            }
            else if (Claim.IsOtherTypeOfAccident)
            {
                if (Claim.IsOtherTypeOfAccidentVisible)
                {
                    var claimDetailsIsOtherTypeOfAccident = new ClaimQuestionAnswerPair()
                    {
                        Question = Resource.ClaimDetailsOtherTypeOfAccident,
                        Answer = Claim.IsOtherTypeOfAccidentVisible ? Resource.Yes : Resource.No
                    };
                    ClaimDetailInput.Add(claimDetailsIsOtherTypeOfAccident);
                }
              
            }

            if (Claim.IsHasReferralBeenPreviouslySubmittedVisible)
            {
                var ClaimDetailsMedicalItemTitle = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsMedicalItemTitle,
                    Answer = Claim.HasReferralBeenPreviouslySubmitted ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(ClaimDetailsMedicalItemTitle);
            }

            if (Claim.IsDateOfReferralVisible)
            {
                var ClaimDetailsMedicalItemReferralDate = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsMedicalItemReferralDate,
                    Answer = Claim.DateOfReferral.ToString("d")
                };
                ClaimDetailInput.Add(ClaimDetailsMedicalItemReferralDate);
            }

            if (Claim.IsTypeOfMedicalProfessionalVisible)
            {
                var claimDetailsMedicalItemProfessional = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsMedicalItemProfessional,
                    Answer = Claim.TypeOfMedicalProfessional.Name
                };
                ClaimDetailInput.Add(claimDetailsMedicalItemProfessional);
            }

            if (Claim.IsIsMedicalItemForSportsOnlyVisible)
            {
                var claimDetailsMedicalItemSport = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsMedicalItemSport,
                    Answer = Claim.IsMedicalItemForSportsOnly ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsMedicalItemSport);
            }

            if (Claim.IsIsGSTHSTIncludedVisible)
            {
                var claimDetailsGstIncludedQ = new ClaimQuestionAnswerPair()
                {
                    Question = Resource.ClaimDetailsGstIncludedQ,
                    Answer = Claim.IsGSTHSTIncluded ? Resource.Yes : Resource.No
                };
                ClaimDetailInput.Add(claimDetailsGstIncludedQ);
            }
        }

        // Create sectioned list of treatment details question-answers 
        private void CreateTreatmentDetailsList()
        {
            IsTreatmentDetailsVisible = Claim.TreatmentDetails != null && Claim.TreatmentDetails.Any();
            foreach (var treatment in Claim.TreatmentDetails.Where(x => x != null))
            {
                var qnaList = new ClaimQuestionAnswerList();
                if (!string.Equals(treatment.ClaimSubmissionType.ID, "DENTAL", StringComparison.OrdinalIgnoreCase))
                {
                    if (treatment.IsTypeOfTreatmentVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsTreatmentTitle,
                            Answer = treatment.TypeOfTreatmentListViewItem?.Name ?? string.Empty
                        }); ;
                    }

                    if (treatment.IsItemDescriptionVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsItemDescription,
                            Answer = treatment.ItemDescription?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsTreatmentDurationVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLengthOfTreatment,
                            Answer = treatment.TreatmentDurationListViewItem?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsTreatementDateVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsDateOfTreatment,
                            Answer = treatment.TreatmentDate.ToString("d")
                        });
                    }

                    if (treatment.IsDateOfMonthlyTreatmentVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsDateOfMonthlyTreatment,
                            Answer = treatment.DateOfMonthlyTreatment.ToString("d")
                        });
                    }

                    if (treatment.IsDateOfPurchaseVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsDateOfPurchaseService,
                            Answer = treatment.DateOfPurchase.ToString("d")
                        });
                    }

                    if (treatment.IsPickupDateVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsPickUpDate,
                            Answer = treatment.PickupDate.ToString("d")
                        });
                    }

                    if (treatment.IsQuantityVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsQuantity,
                            Answer = treatment.Quantity.ToString()
                        });
                    }

                    if (treatment.IsTreatmentAmountVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = treatment.ClaimSubmissionType.ID.Equals("MI", StringComparison.OrdinalIgnoreCase) ?
                                Resource.TreatmentDetailsTotalAmountMedicalItems : Resource.TreatmentDetailsTotalAmountOfVisit,
                            Answer = treatment.TreatmentAmountListViewItem.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsTotalAmountChargedForMIVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsTotalAmountMedicalItems,
                            Answer = treatment.TreatmentAmount.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsOrthodonticMonthlyFeeVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsOrthoMonthlyFee,
                            Answer = treatment.OrthodonticMonthlyFee.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsAlternateCarrierPaymentVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsAmountPaidAlt,
                            Answer = treatment.AlternateCarrierPayment.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsAmountPaidByPPorGPVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsAmountPaidByPpOrGp,
                            Answer = treatment.AmountPaidByPPorGP.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsGSTHSTIncludedInTotalVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsGstIncluded,
                            Answer = treatment.GSTHSTIncludedInTotal ? Resource.Yes : Resource.No
                        });
                    }

                    if (treatment.IsPSTIncludedInTotalVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsPstIncluded,
                            Answer = treatment.PSTIncludedInTotal ? Resource.Yes : Resource.No
                        });
                    }

                    if (treatment.IsHasReferralBeenPreviouslySubmittedVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsMedicalItem,
                            Answer = treatment.HasReferralBeenPreviouslySubmitted ? Resource.Yes : Resource.No
                        });
                    }

                    if (treatment.IsDateOfReferralVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsDateOfReferral,
                            Answer = treatment.DateOfReferral.ToString("d")
                        });
                    }

                    if (treatment.IsTypeOfMedicalProfessionalVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.ClaimDetailsTypeOfProfessional,
                            Answer = treatment.TypeOfMedicalProfessional.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsTypeOfEyewearVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsTypeOfEyewear,
                            Answer = treatment.TypeOfEyewear?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsTypeOfLensVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsTypeOfLens,
                            Answer = treatment.TypeOfLens?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsTypeOfLensVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsFrameAmount,
                            Answer = treatment.FrameAmount.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsFeeAmountVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsFeeAmount,
                            Answer = treatment.FeeAmount.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsEyeglassLensesAmountVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLensAmount,
                            Answer = treatment.EyeglassLensesAmount.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsTotalAmountChargedVisible)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsTotalAmountCharged,
                            Answer = treatment.TotalAmountCharged.AddDolarSignBasedOnCulture()
                        });
                    }

                    if (treatment.IsRightSphereEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsRightSphere,
                            Answer = treatment.RightSphere?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsLeftSphereEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLeftSphere,
                            Answer = treatment.LeftSphere?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsRightCylinderEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsRightCylinder,
                            Answer = treatment.RightCylinder?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsLeftCylinderEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLeftCylinder,
                            Answer = treatment.LeftCylinder?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsRightAxisEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsRightAxis,
                            Answer = treatment.RightAxis?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsLeftAxisEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLeftAxis,
                            Answer = treatment.LeftAxis?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsRightPrismEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsRightPrism,
                            Answer = treatment.RightPrism?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsLeftPrismEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLeftPrism,
                            Answer = treatment.LeftPrism?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsRightBifocalEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsRightBifocal,
                            Answer = treatment.RightBifocal?.Name ?? string.Empty
                        });
                    }

                    if (treatment.IsLeftBifocalEnabled)
                    {
                        qnaList.Add(new ClaimQuestionAnswerPair()
                        {
                            Question = Resource.TreatmentDetailsLeftBifocal,
                            Answer = treatment.LeftBifocal?.Name ?? string.Empty
                        });
                    }
                }
                else if (string.Equals(treatment.ClaimSubmissionType.ID, "DENTAL", StringComparison.OrdinalIgnoreCase))
                {
                    CreateTreatmentDetailListForDental(treatment, qnaList);
                }
                ClaimTreatmentInput.Add(qnaList);
            }
        }

        private static void CreateTreatmentDetailListForDental(TreatmentDetail treatment, ClaimQuestionAnswerList qnaList)
        {
            if (treatment.IsTreatementDateVisible)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsDateOfTreatment,
                    Answer = treatment.TreatmentDate.ToString("d")
                });
            }

            if (treatment.IsTypeOfTreatmentVisible)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsTreatmentTitle,
                    Answer = treatment.TypeOfTreatmentListViewItem?.Name ?? string.Empty
                }); ;
            }

            if (treatment.IsToothCodeRequired)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsToothCode,
                    Answer = treatment.ToothCode.ToString()
                });
            }

            if (treatment.IsToothSurfaceRequired)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsToothSurface,
                    Answer = treatment.ToothSurface
                });
            }

            qnaList.Add(new ClaimQuestionAnswerPair()
            {
                Question = Resource.TreatmentDetailsDentistFee,
                Answer = treatment.DentistFees?.AddDolarSignBasedOnCulture()
            });

            if (treatment.IsLaboratoryChargesRequired)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsLabCharge,
                    Answer = treatment.LaboratoryCharges?.AddDolarSignBasedOnCulture()
                });
            }

            if (treatment.IsTreatmentAmountVisible)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsTotalAmountCharged,
                    Answer = treatment.TreatmentAmountListViewItem.AddDolarSignBasedOnCulture()
                });
            }

            if (treatment.IsAlternateCarrierPaymentVisible)
            {
                qnaList.Add(new ClaimQuestionAnswerPair()
                {
                    Question = Resource.TreatmentDetailsAmountPaidAlt,
                    Answer = treatment.AlternateCarrierPayment.AddDolarSignBasedOnCulture()
                });
            }
        }

        private async void ExecuteSubmitClaimCommand()
        {
            if (_claimservice.IsHCSAClaim)
            {
                if (string.IsNullOrEmpty(HCSAClaim.ParticipantNumber)
                    || HCSAClaim.ParticipantNumber != _participantservice.SelectedParticipant.PlanMemberID)
                {
                    HCSAClaim.ParticipantNumber = _participantservice.SelectedParticipant.PlanMemberID;
                }

                await _hcsaservice.SubmitClaim(HCSAClaim.ParticipantNumber,
                    HCSAClaim.ClaimTypeID,
                    HCSAClaim.ExpenseTypeID,
                    HCSAClaim.MedicalProfessionalID,
                    HCSAClaim.Details,
                    () => { ShowViewModel<ClaimResultsViewModel>(); },
                    () =>
                    {
                        //service handles sends messages for all errors.  ViewmodelBase knows what to do
                    });
            }
            else
            {
                try
                {
                    _userDialogs.ShowLoading(Resource.Loading);
                    if (string.Equals(UploadDocumentType, UploadDocumentProcessType.NewClaimSubmission, StringComparison.OrdinalIgnoreCase))
                    {
                        await _claimservice.SubmitClaimAsync(Comments, UploadDocumentType, Attachments);

                        var uploadable = (IClaimCompletedProperties)NonRealTimeUploadFactory.Create(_claimType, nameof(ConfirmationOfPaymentCompletedViewModel));
                        await ShowViewModel<ConfirmationOfPaymentCompletedViewModel, ConfirmationOfPaymentCompletedViewModelParameters>(new ConfirmationOfPaymentCompletedViewModelParameters(uploadable));
                    }
                    else
                    {
                        _claimsuccessful = _messenger.Subscribe<ClaimSubmissionComplete>(message =>
                        {
                            Busy = false;
                            _userDialogs.HideLoading();

                            _messenger.Unsubscribe<ClaimSubmissionComplete>(_claimsuccessful);
                            _messenger.Unsubscribe<ClaimSubmissionError>(_claimfailed);
                            PublishMessages();
                            ShowViewModel<ClaimSubmissionResultViewModel>();
                        });

                        _sessionExpired = _messenger.Subscribe<UserSessionExpired>(message =>
                        {
                            Busy = false;
                            _userDialogs.HideLoading();
                            _userDialogs.Alert(Resource.GenericErrorDialogMessage);

                            _messenger.Unsubscribe<ClaimSubmissionComplete>(_claimsuccessful);
                            _messenger.Unsubscribe<ClaimSubmissionError>(_claimfailed);
                        });

                        _claimfailed = _messenger.Subscribe<ClaimSubmissionError>(HandleClaimSubmissionError);

                        Busy = true;
                        await _claimservice.SubmitClaim();
                    }
                }
                catch (Exception ex)
                {
                    _userDialogs.HideLoading();
                    _userDialogs.Alert(Resource.GenericErrorDialogMessage);
                }
                finally
                {
                    _userDialogs.HideLoading();
                }
            }
        }

        private void HandleClaimSubmissionError(ClaimSubmissionError message)
        {
            Busy = false;
            _messenger.Unsubscribe<ClaimSubmissionComplete>(_claimsuccessful);
            _messenger.Unsubscribe<ClaimSubmissionError>(_claimfailed);
            switch (message.Message)
            {
                case "0":
                    //should be success so we shouldn't even get here, but just in case we'll raise generic invalid event
                    RaiseInvalidClaim(new EventArgs());
                    break;
                case "1":
                    //The other GSC number does not have a matched dependent
                    RaiseNoMatchedDependent(new EventArgs());
                    break;
                case "2":
                    {
                        //More than one dependent covered by the secondary GSC plan was found. Please select the correct one from the drop down list
                        IsParticipantsListVisible = true;
                        Participants = _claimservice.ClaimResults.Participants;
                        RaiseMultipleMatch(new EventArgs());
                        break;
                    }
                case "3":
                    //The secondary plan number entered cannot be used for processing your secondary claim
                    RaiseInvalidSecondaryPlanNumber(new EventArgs());
                    break;
                case "4":
                    //Your secondary coverage plan does not allow for online claim submission. For coverage under your secondary plan, please submit the claim manually.
                    RaiseInvalidOnlineClaim(new EventArgs());
                    break;
                case "5":
                    //Your other GSC number is invalid
                    RaiseInvalidGSCNumber(new EventArgs());
                    break;
                case "6":
                    // Spouse's account diabled
                    RaiseSecondaryAccountDisabled(new EventArgs());
                    break;
                case "7":
                    // Spouse's account not registered for online services
                    RaiseSecondaryAccountNotRegistered(new EventArgs());
                    break;
                case "8":
                    // Spouse hasn't accepted Claim Submission Agreement
                    RaiseSecondaryAccountHasntAcceptedAgreement(new EventArgs());
                    break;
                default:
                    //if not defined we also shouldn't get here, but just in case we'll raise generic invalid event
                    RaiseInvalidClaim(new EventArgs());
                    break;
            }
        }

        private void PublishMessages()
        {
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }
    }
}