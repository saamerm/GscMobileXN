using MobileClaims.Core.Messages;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ChangeForLifeNoAccessViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ChangeForLifeNoAccessViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
            
            NoAccessMessage = Resource.ChangeForLife_MessageNoAccess;

            _shouldcloseself = _messenger.Subscribe<ClearChangeForLifeNoAccessViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearChangeForLifeNoAccessViewRequested>(_shouldcloseself);
                Close(this);
            });
        }

        private string _noAccessMessage = "Oops. We're sorry, you don't have access to Change4Life";
        public string NoAccessMessage
        {
            get
            {
                return _noAccessMessage;
            }
            set
            {
                _noAccessMessage = value;
                RaisePropertyChanged(() => NoAccessMessage);
            }
        }
    }
}
