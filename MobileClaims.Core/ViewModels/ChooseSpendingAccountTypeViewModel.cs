using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ChooseSpendingAccountTypeViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly ISpendingAccountService _accountservice;
        private readonly IParticipantService _participantservice;
        private object _sync = new object();

        public ChooseSpendingAccountTypeViewModel(IMvxMessenger messenger, ISpendingAccountService accountservice, IParticipantService participantservice, IDeviceService deviceservice)
        {
            _accountservice = accountservice;
            _messenger = messenger;
            _participantservice = participantservice;
        }

        public override async void Start()
        {
            base.Start();
            Busy = true;
            if (_accountservice.AccountTypes == null || _accountservice.AccountTypes.Count == 0)
            {
                if (_participantservice.PlanMember != null && !string.IsNullOrEmpty(_participantservice.PlanMember.PlanMemberID))
                {
                    await _accountservice.GetSpendingAccountTypes(_participantservice.PlanMember.PlanMemberID);
                    AccountTypes = _accountservice.AccountTypes;
                }
            }
            else
            {
                await System.Threading.Tasks.Task.Yield();
                AccountTypes = _accountservice.AccountTypes;
            }       

            Busy = false;
        }

        private SpendingAccountType _selectedaccounttype;
        public SpendingAccountType SelectedAccountType
        {
            get => _selectedaccounttype;
            set
            {
                if (_selectedaccounttype != value)
                {
                    _selectedaccounttype = value;
                    _accountservice.SelectedSpendingAccountType = _selectedaccounttype;
                    RaisePropertyChanged(() =>  SelectedAccountType);
                };
            }
        }

        private List<SpendingAccountType> _accounttypes;
        public List<SpendingAccountType> AccountTypes
        {
            get => _accounttypes;
            set
            {
                if (value != null)
                {
                    _accounttypes = value;
                    RaisePropertyChanged(() => AccountTypes);
                }
            }
        }

		private bool _busy = false;
		public bool Busy
		{
			get => _busy;
		    set
			{
				_busy = value;
				_messenger.Publish<BusyIndicator>(new BusyIndicator(this)
				{
					Busy = _busy
				});
				RaisePropertyChanged(() => Busy);
			}
		}

        public ICommand FillAccountDetailCommand
        {
            get
            {
                return new MvxCommand<SpendingAccountType>((accountType) =>
                {
                    SelectedAccountType = accountType;
                });
            }
        }

        public ICommand FillAccountDetailAndNavigateCommand
        {
            get
            {
                return new MvxCommand<SpendingAccountType>(async (acctType) =>
                {
                    SelectedAccountType = acctType;
                    await ShowViewModel<SpendingAccountDetailViewModel>();
                });
            }
        }

		public ICommand SetSelectedModeCommand
		{
			get
            {
				return new MvxCommand<SpendingAccountType> ((accounttype) =>
                {
					this.SelectedAccountType = accounttype;
				});
			}
		}
    }
}