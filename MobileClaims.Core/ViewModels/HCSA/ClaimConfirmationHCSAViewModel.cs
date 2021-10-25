using MobileClaims.Core.Entities;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using MobileClaims.Core.Messages;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimConfirmationHCSAViewModel : HCSAViewModelBase
    {
        private IMvxMessenger _messenger;
        private IHCSAClaimService _hcsaservice;
        private IClaimService _claimservice;
        private IParticipantService _participantservice;
        private ObservableCollection<HCSAReferralType> _medicalprofessionaltypes;
        IUserDialogs dlg;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimConfirmationHCSAViewModel(IMvxMessenger messenger, 
                                              IHCSAClaimService hcsaservice, 
                                              IClaimService claimservice,
                                              IParticipantService participantservice)
        {
            _claimservice = claimservice;
            _messenger = messenger;
            _hcsaservice = hcsaservice;
            Claim = _hcsaservice.Claim;
            _participantservice = participantservice;
            dlg = Mvx.IoCProvider.Resolve<IUserDialogs>();

            _shouldcloseself = _messenger.Subscribe<ClearClaimConfirmationHCSAViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimConfirmationHCSAViewRequested>(_shouldcloseself);
                Close(this);
            });

            Participant = _participantservice.PlanMember.PlanMemberID == Claim.ParticipantNumber ? 
                new Participant()
                {
                    PlanMemberID = _participantservice.PlanMember.PlanMemberID,
                    FirstName = _participantservice.PlanMember.FirstName,
                    LastName = _participantservice.PlanMember.LastName,
                    TravelCovered = _participantservice.PlanMember.TravelCovered
                }
                : _participantservice.PlanMember.Dependents.Where(dependent => dependent.PlanMemberID == Claim.ParticipantNumber).FirstOrDefault();// _participantservice.SelectedParticipant;
            _medicalprofessionaltypes = JsonConvert.DeserializeObject<ObservableCollection<HCSAReferralType>>(Resource.HCSAReferralTypes);
            ReferralType = (from HCSAReferralType rt in _medicalprofessionaltypes where rt.Code == _hcsaservice.Claim.MedicalProfessionalID select rt).FirstOrDefault();
            
        }

        //Total Claim amount
        //Visibility for Type
        //
        public string ExpenseType
        {
            get
            {
                if(Claim.ExpenseType != null && Claim.ExpenseType.ID != 0)
                {
                    return Claim.ExpenseType.Name;
                }
                else
                {
                    return Claim.ClaimType.Name;
                }
            }
        }

        public double OtherPaidTotalAmount
        {
            get
            {
                double ret = Claim.Details.Sum(cd => cd.OtherPaidAmount);
                return ret;
            }
        }

        public double TotalClaimAmount
        {
            get
            {
                double ret = Claim.Details.Sum(cd => cd.ClaimAmount);
                return ret;
            }
        }

        public bool IsReferrerVisible
        {
            get
            {
                return !string.IsNullOrEmpty(Claim.MedicalProfessionalID);
            }
        }

        public string ClaimBoilerPlateLabel
        {
            get
            {
                return BrandResource.submittedToGSC;
            }
        }

        private Entities.HCSA.Claim _claim;
        public Entities.HCSA.Claim Claim
        {
            get
            {
                return _claim;
            }
            set
            {
                _claim = value;
                RaisePropertyChanged(() => Claim);
            }
        }


        private HCSAReferralType _referraltype;
        public HCSAReferralType ReferralType
        {
            get
            {
                return _referraltype;
            }
            set
            {
                _referraltype = value;
                RaisePropertyChanged(() => ReferralType);
            }
        }


        private Participant _participant;
        public Participant Participant
        {
            get
            {
                return _participant;
            }
            set
            {
                _participant = value;
                RaisePropertyChanged(() => Participant);
            }
        }

        public string ClaimedAmountLabel
        {
            get
            {
                return Resource.ClaimedAmount;
            }
        }

        public string OtherPaidAmountLabel
        {
            get
            {
                return Resource.OtherPaidAmount;
            }
        }

        public string TypeExpenseLabel
        {
            get
            {
                return Resource.claimConfirm_type_expense;
            }
        }

        public string TotalLabel
         {
             get
             {
                 return Resource.claimConfirm_total;
             }
         }

        public string TitleLabel
         {
             get
             {
                 return Resource.claimConfirm_title;
             }
         }

        public string RefferedLabel
         {
             get
             {
                 return Resource.claimConfirm_reffered;
             }
         }

        public string PlanInfoLabel
         {
             get
             {
                 return Resource.claimConfirm_plan_info;
             }
         }

        public string ParticipantLabel
        {
            get
            {
                return Resource.claimConfirm_participant;
            }
        }

        public string IDNumberLabel
        {
            get
            {
                return BrandResource.claimConfirm_id_number;
            }
        }

        public string GovernmentPlanLabel
        {
            get
            {
                return Resource.OtherPaidAmount;
            }
        }

        public string DetailsLabel
        {
            get
            {
                return Resource.claimConfirm_detais;
            }
        }

        public string DescriptionLabel
        {
            get
            {
                return BrandResource.claimConfirm_description;
            }
        }

        public string SubmitButtonLabel
        {
            get
            {
                return Resource.SubmitClaim;
            }
        }


        public string DateOfExpenseLabel
        {
            get
            {
                return Resource.DateOfExpense;
            }
        }

        public string ErrorMessageLabel
        {
            get
            {
                return Resource.error_message ;
            }
        }

        MvxCommand _SubmitClaimCommand;
        public System.Windows.Input.ICommand SubmitClaimCommand
        {
            get
            {
                _SubmitClaimCommand = _SubmitClaimCommand ?? new MvxCommand(() =>
                                                              {
                                                                  Mvx.IoCProvider.Resolve<IUserDialogs>().ShowLoading();
                                                                  
                                                                  _hcsaservice.SubmitClaim(Claim.ParticipantNumber,
                                                                                           Claim.ClaimTypeID,
                                                                                           Claim.ExpenseTypeID,
                                                                                           Claim.MedicalProfessionalID,
                                                                                           Claim.Details,
                                                                                           () =>
                                                                                           { 
                                                                                               Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                                               ShowViewModel<ClaimResultsViewModel>();
                                                                                           },
                                                                                           () =>
                                                                                           {
                                                                                               Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                                                                                               //service handles sends messages for all errors.  ViewmodelBase knows what to do
                                                                                           });
                                                              });
                return _SubmitClaimCommand;
            }
        }
    }
}
