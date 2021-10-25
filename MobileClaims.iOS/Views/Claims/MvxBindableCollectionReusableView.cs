using System;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public class MvxBindableCollectionReusableView : UICollectionReusableView, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get
            {
                return BindingContext;
            }
            set
            {
                BindingContext.DataContext = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        protected MvxBindableCollectionReusableView(IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }
    }
}