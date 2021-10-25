using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Views;

namespace MobileClaims.Droid.Views
{
	[Activity (Label = "DrugView")]			
	public class DrugView : MvxActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}

		public bool Show(MvvmCross.ViewModels.MvxViewModelRequest request)
		{
//			if (_detailFragment == null)
//				return false;
//
//			if (!_detailFragment.IsVisible)
//				return false;
//
//			if (request.ViewModelType != typeof(DetailViewModel))
//				return false;
//
//			// TODO - replace this with extension method when available
//			var loaderService = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
//			var viewModel = loaderService.LoadViewModel(request, null /*			 saved state */);
//			_detailFragment.ViewModel = viewModel;
//
			return true;
		}
    }
}