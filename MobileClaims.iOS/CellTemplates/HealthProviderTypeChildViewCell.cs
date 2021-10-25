using System;
using FFImageLoading.Cross;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.CellTemplates
{
    public partial class HealthProviderTypeChildViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HealthProviderTypeChildViewCell");
        public static readonly UINib Nib;

        static HealthProviderTypeChildViewCell()
        {
            Nib = UINib.FromName("HealthProviderTypeChildViewCell", NSBundle.MainBundle);
        }

        protected HealthProviderTypeChildViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public UILabel TitleLabel => titleLabel;

        public UIImageView CheckedImage => checkedImage;

        public MvxCachedImageView ProviderTypeImage => this.providerTypeImage;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            titleLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
        }
    }
}
