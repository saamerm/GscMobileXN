
using System;
using UIKit;
using MobileClaims.Core.ViewModels;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS.Views
{
    public partial class ChangeForLifeNoAccessView : GSCBaseViewController
	{
		 
		private ChangeForLifeNoAccessViewModel _model;
		private GSCMessageLabel _noAccessMessage;
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			View = new UIView() { BackgroundColor = Colors.BACKGROUND_COLOR };
			base.ViewDidLoad ();

			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.Title = "C4L".tr();
			base.NavigationItem.SetHidesBackButton (true, false);

			_model = (ChangeForLifeNoAccessViewModel)ViewModel;


			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Colors.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Colors.BACKGROUND_COLOR;
				this.AutomaticallyAdjustsScrollViewInsets = false;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Colors.BACKGROUND_COLOR;
			} 

			_noAccessMessage = new GSCMessageLabel ();
			_noAccessMessage.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
			View.Add (_noAccessMessage);

			var set = this.CreateBindingSet<ChangeForLifeNoAccessView, ChangeForLifeNoAccessViewModel>();
			set.Bind(_noAccessMessage ).To(vm => vm.NoAccessMessage);
			set.Apply();  

			View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			View.AddConstraints(
				_noAccessMessage.WithSameCenterX (View),
				_noAccessMessage.WithSameCenterY (View),
				_noAccessMessage.WithRelativeWidth(View, .95f),
					_noAccessMessage.WithRelativeHeight(View,.4f)
				);
		}
	}
}

