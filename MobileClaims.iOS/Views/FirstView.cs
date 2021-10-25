using CoreGraphics;
using ObjCRuntime;
using UIKit;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS.Views
{
	[Foundation.Register("FirstView")]
    public class FirstView : GSCBaseViewController
	{
		public override void ViewDidLoad()
		{
			View = new UIView(){ BackgroundColor = Colors.BACKGROUND_COLOR};
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
			   EdgesForExtendedLayout = UIRectEdge.None;
			   
			var label = new UILabel(new CGRect(10, 10, 300, 40));
			Add(label);
			var textField = new UITextField(new CGRect(10, 50, 300, 40));
			Add(textField);

			var set = this.CreateBindingSet<FirstView, Core.ViewModels.LoginViewModel>();
			//set.Bind(label).To(vm => vm.Hello);
			//set.Bind(textField).To(vm => vm.Hello);
			set.Apply();
		}
	}
}