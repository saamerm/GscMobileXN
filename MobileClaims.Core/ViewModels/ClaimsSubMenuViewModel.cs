using MobileClaims.Core.ViewModels.ClaimsHistory;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimsSubMenuViewModel : ViewModelBase
    {
        #region Member Variables
        private readonly IMvxMessenger _messenger;
        #endregion

        #region ctors
        public ClaimsSubMenuViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
        }
        #endregion

        #region Commands
        public ICommand SubmitAClaimCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    //TODO: Kick off rehydration logic here.
                    this.ShowViewModel<ClaimSubmissionTypeViewModel>();
                });
            }
        }

        public ICommand ClaimsHistoryCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    this.ShowViewModel<ClaimsHistoryResultsCountViewModel>();
                });
            }
        }
        #endregion
    }
}
