using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels.HCSA
{
    public class ClaimExpenseTypesViewModel : HCSAViewModelBase
    {
        private IMvxMessenger _messenger;
        private IHCSAClaimService _claimservice;
        private IParticipantService _participantservice;
        private IRehydrationService _rehydrationservice;

        public ClaimExpenseTypesViewModel(IMvxMessenger messenger, IHCSAClaimService claimservice,IParticipantService participantservice, IRehydrationService rehydrationservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _rehydrationservice = rehydrationservice;
            _participantservice = participantservice;

            if (_claimservice.SelectedClaimType != null)
            {
                SelectedClaimType = _claimservice.SelectedClaimType;
            }
            if (_claimservice.ExpenseTypes != null && _claimservice.ExpenseTypes.Count() > 0)
            {
                ExpenseTypes = new ObservableCollection<ExpenseType>(_claimservice.ExpenseTypes);
            }
            else
            {
                _claimservice.GetClaimExpenseTypes(_loginservice.CurrentPlanMemberID, _claimservice.SelectedClaimType.ID,
                    () =>
                    {
                        ExpenseTypes =  new ObservableCollection<ExpenseType>(_claimservice.ExpenseTypes.ToArray());
                    },
                    (message, code) =>
                    {
                        ErrorMessage = message;
                        ErrorCode = code;
                    });
            }   
        }

        private ObservableCollection<ClaimType> _claimtypes;
        public ObservableCollection<ClaimType>ClaimTypes
        {
            get
            {
                return _claimtypes;
            }
            set
            {
                if(_claimtypes !=value)
                {
                    _claimtypes = value;
                    RaisePropertyChanged(() => ClaimTypes);
                }
            }
        }

        private ClaimType _selectedclaimtype;
        public ClaimType SelectedClaimType
        {
            get { return _selectedclaimtype; }
            set
            {
                if (_selectedclaimtype != value)
                {
                    _selectedclaimtype = value;
                    RaisePropertyChanged(() => SelectedClaimType);
                }
            }
        }

        private ObservableCollection<ExpenseType> _expensetypes;
        public ObservableCollection<ExpenseType> ExpenseTypes
        {
            get
            {
                return _expensetypes;   
            }
            set
            {
                if(_expensetypes != value)
                {
                    _expensetypes = value;
                    if (_expensetypes.Count > 0)
                    {
                        _expensetypes.Insert(0, null);
                        SelectedExpenseType = ExpenseTypes.First();
                    }
                    RaisePropertyChanged(() => ExpenseTypes);
                }
            }
        }

        private ExpenseType _selectedexpensetype;
        public ExpenseType SelectedExpenseType
        {
            get
            {
                return _selectedexpensetype;
            }
            set
            {
                _selectedexpensetype = value;
                RaisePropertyChanged(() => SelectedExpenseType);
            }
        }

        private string __errormessage;
        public string ErrorMessage
        {
            get
            {
                return __errormessage;
            }
            set
            {
                __errormessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            }
        }

        private int _errorcode;
        public int ErrorCode
        {
            get
            {
                return _errorcode;
            }
            set
            {
                _errorcode = value;
                RaisePropertyChanged(() => ErrorCode);
            }
        }
    }
}
