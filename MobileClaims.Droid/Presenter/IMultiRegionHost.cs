using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid.Presenter
{
    /// <summary>
    ///     Implemented by the activity registered to the IMultiRegionPresenter.  Will show views in regions specitied by their
    ///     RegionAttribute.
    /// </summary>
    public interface IMultiRegionHost
    {
        /// <summary>
        ///     Shows the MvxFragment view in the region specified by its RegionAttribute.
        /// </summary>
        /// <param name="fragment"></param>
        void Show(MvxFragment fragment);

        /// <summary>
        ///     Closes the view associated with the given ViewModel.
        /// </summary>
        /// <param name="viewModel"></param>
        void CloseViewModel(IMvxViewModel viewModel);

        /// <summary>
        ///     Closes all active views.
        /// </summary>
        void CloseAll();
    }
}