using System.Collections.Generic;
using System.Windows.Input;
using MobileClaims.Core.Attributes;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    [RequiresAuthentication(true)]
    public class DrugLookupModelSelectionViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly MvxSubscriptionToken _participantselected;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private object _sync = new object();

        public DrugLookupModelSelectionViewModel(IMvxMessenger messenger, ILoginService loginservice)
            : base (messenger, loginservice)
        {
			_messenger = messenger;

            Modes = new List<string> {"Search by Drug Name", "Search by DIN"};

            _participantselected = _messenger.Subscribe<DrugParticipantSelected>(message =>
            {
                SelectedParticipant = message.SelectedParticipant;
            });

            _shouldcloseself = _messenger.Subscribe<ClearDrugLookupModelSelectionRequested>(message =>
            {
                _messenger.Unsubscribe<ClearSearchByDrugNameRequested>(_shouldcloseself);
                SelectedMode = null;
                Close(this);
            });
        }

        public Participant SelectedParticipant { get; set; }

        private List<string> _modes;
        public List<string> Modes
        {
            get { return _modes; }
            set
            {
                if (_modes != value)
                {
                    _modes = value;
                    RaisePropertyChanged(() => Modes);
                };
            }
        }

        private string _selectedmode;
        public string SelectedMode
        {
            get { return _selectedmode; }
            set
            {
                if (_selectedmode != value)
                {
                    _selectedmode = value;
                    RaisePropertyChanged(() => SelectedMode);
                };
            }
        }

        public ICommand ByNameCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Unsubscribe();
					_messenger.Publish(new RequestNavToSearchDrugByName(this));
					ShowViewModel<DrugLookupByNameViewModel>();
				});
            }
        }

        public ICommand ByDINCommand
        {
            get
            {
                return new MvxCommand(() => 
				{
                    Unsubscribe();
					ShowViewModel<DrugLookupByDINViewModel>();
				});
            }
        }

        public ICommand NavigateBySelectedModeCommand
        {
            //TODO: FIX THIS!!  Need to populate a mode dictionary that (somehow) maps the localizable strings to commands
            get
            {
                return new MvxCommand<string>(mode =>
                {
                    switch(mode)
                    {
                        case "Search by DIN":
                        {
                            _messenger.Publish(new ClearDrugSearchByDINRequested(this));
                            _messenger.Publish(new ClearSearchByDrugNameRequested(this));
                            _messenger.Publish(new ClearDrugSearchByNameResultsRequested(this));
                            _messenger.Publish(new ClearDrugSearchResultsRequested(this));
                            ByDINCommand.Execute(null);
                            break;
                        }
                        case "Search by Drug Name":
                        {
                            _messenger.Publish(new ClearDrugSearchByDINRequested(this));
                            _messenger.Publish(new ClearSearchByDrugNameRequested(this));
                            _messenger.Publish(new ClearDrugSearchByNameResultsRequested(this));
                            _messenger.Publish(new ClearDrugSearchResultsRequested(this));
                            ByNameCommand.Execute(null);
                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                });
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<DrugParticipantSelected>(_participantselected);
        }
    }
}