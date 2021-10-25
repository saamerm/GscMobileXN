using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class SpendingAccountDetailViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly ISpendingAccountService _accountservice;
        private readonly IParticipantService _participantservice;
        private readonly MvxSubscriptionToken _getspendingaccounts;
        private readonly MvxSubscriptionToken _getspendingaccountserror;
        private readonly MvxSubscriptionToken _nospendingaccounts;
        private object _sync = new object();

        private int preYearNumber = DateTime.Now.Year - 1;
        private int currentYearNumber = DateTime.Now.Year;
        private bool hasDataForPreviousYear = false;
        private bool hasDataForCurrentYear = false;

        private bool calledFromLanding = false;

        public SpendingAccountDetailViewModel(IMvxMessenger messenger, ISpendingAccountService accountservice, IParticipantService participantservice)
        {
            _messenger = messenger;
			_accountservice = accountservice;
            _participantservice = participantservice;

            if (_accountservice.SelectedSpendingAccountType != null)
            {
                AccountType = _accountservice.SelectedSpendingAccountType;
            }
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            if (!_accountservice.IsGetSpendingAccountsBusy) //only call if GetSpendingAccounts isn't done AND isn't currently running
            {
                Busy = true;
                await _accountservice.GetSpendingAccounts(_participantservice.PlanMember.PlanMemberID);
                //Busy = true;
                //WindowsBusy = true;
            }

            if (_accountservice.IsGetSpendingAccountsDone)
            {
                SpendingAccountDetails = _accountservice.GetSpendingAccountDetailsByType(AccountType);
                SpendingAccountTypeSummary = _accountservice.SpendingAccountTypeRollups;
            }
            Busy = false;
        }

        private bool _busy = false;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                if (_busy != value)
                {
                    _busy = value;
                    RaisePropertyChanged(() => Busy);
                };
            }
        }

        //Need a separate one because windows is retrieving everything at once
        private bool _winbusy = false;
        public bool WindowsBusy
        {
            get { return _winbusy; }
            set
            {
                if (_winbusy != value)
                {
                    _winbusy = value;
                    RaisePropertyChanged(() => WindowsBusy);
                };
            }
        }

        private SpendingAccountType _accounttype;
        public SpendingAccountType AccountType
        {
            get { return _accounttype; }
            set
            {
                if (_accounttype != value)
                {
                    _accounttype = value;
                    RaisePropertyChanged(() => AccountType);
                };
            }
        }

        private SpendingAccountTypeRollup _spendingAccountDetails;
        public SpendingAccountTypeRollup SpendingAccountDetails
        {
            get { return _spendingAccountDetails; }
            set
            {
                _spendingAccountDetails = value;
                RaisePropertyChanged(() => SpendingAccountDetails);
            }
        }

        private List<SpendingAccountTypeRollup> _spendingaccounttypesummary;
        public List<SpendingAccountTypeRollup> SpendingAccountTypeSummary
        {
            get { return _spendingaccounttypesummary; }
            set
            {
                _spendingaccounttypesummary = value;

                RaisePropertyChanged(() => SpendingAccountTypeSummary);
            }
        }

        public ICommand ShowMyAccountCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    _messenger.Publish<RequestNavToCard>(new RequestNavToCard(this));
                    await ShowViewModel<CardViewModel>();
                });
            }
        }

        public ICommand ShowBalancesCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    _messenger.Publish<RequestNavToSpendingAccounts>(new RequestNavToSpendingAccounts(this));
                    await ShowViewModel<ChooseSpendingAccountTypeViewModel>();
                });
            }
        }

        public ICommand ShowDrugLookupCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    _messenger.Publish<RequestNavToDrugLookup>(new RequestNavToDrugLookup(this));
                    await ShowViewModel<ViewModels.DrugLookupModelSelectionViewModel>();
                });
            }
        }

        public ICommand GetSpendingAccountDetailCommand
        {
            get
            {
                return new MvxCommand<SpendingAccountType>((accountType) =>
                {
                    _accountservice.GetSpendingAccountDetailsByType(accountType);
                },
                (accountType) =>
                {
                    return accountType != null;
                });
            }
        }

        //Used for windows
        public ICommand GetAllSpendingAccountDetailCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SpendingAccountTypeSummary = _accountservice.SpendingAccountTypeRollups;
                    WindowsBusy = false;
                });
            }
        }


        private List<YearTotalRemain> _yearTotalRemainCollection;
        public List<YearTotalRemain> YearTotalRemainCollection
        {
            get
            {
                return _yearTotalRemainCollection;
            }
            set
            {
                _yearTotalRemainCollection = value;
                RaisePropertyChanged(() => YearTotalRemainCollection);
            }
        }

        public class YearTotalRemain
        {

            public string AccountName { get; set; }
            public int preYear { get; set; }
            public int currentYear { get; set; }
            public double PreYearTotal { get; set; }
            public double currentYearTotal { get; set; }
            public double SumofTotalRemaining { get; set; }
        }

        private IMvxCommand _backBtnClickCommandDroid;
        public new IMvxCommand BackBtnClickCommandDroid
        {
            get
            {
                return _backBtnClickCommandDroid ?? (_backBtnClickCommandDroid = new MvxAsyncCommand(
                    async () =>
                    {
                        await Close(this);
                    }));
            }
        }
    }
}
