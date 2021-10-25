using MvvmCross.Platforms.Android.Presenters;

namespace MobileClaims.Droid.Presenter
{
    public interface IMultiRegionPresenter : IMvxAndroidViewPresenter
    {
        /// <summary>
        ///     Allows the IMultiRegionHost to register itself with the presenter.  This is required as the presenter is
        ///     constructed before the IMultiRegionHost activity and
        ///     the activity is created outside of the Mvx IoC.
        /// </summary>
        void RegisterMultiRegionHost(IMultiRegionHost host);

        void ShowBottomNavigation();
    }
}