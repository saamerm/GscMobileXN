using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MupApps.MvvmCross.Plugins.ControlsNavigation.Touch;

namespace MobileClaims.iOS.Controls
{
	public partial class DrugLookupByNameControl : MvxTouchControl
    {
        public DrugLookupByNameControl() : base("DrugLookupByNameControl", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			this.EmptyControlBehaviour = MupApps.MvvmCross.Plugins.ControlsNavigation.EmptyControlBehaviours.Hide;
            // Perform any additional setup after loading the view, typically from a nib.
        }
		public override void ViewDidLayoutSubviews()
		{

		}
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
		}

    }
}

