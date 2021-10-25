using System;
using FFImageLoading.Cross;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.CellTemplates
{
    public partial class HealthProviderTypeParentViewCell : UITableViewHeaderFooterView
    {
        public static readonly NSString Key = new NSString("HealthProviderTypeParentViewCell");
        public static readonly UINib Nib;

        static HealthProviderTypeParentViewCell()
        {
            Nib = UINib.FromName("HealthProviderTypeParentViewCell", NSBundle.MainBundle);
        }

        protected HealthProviderTypeParentViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public UILabel TitleLabel => titleLabel;

        public UIButton ExpandButton => expandButton;

        public UIImageView CheckedImage => this.checkedImage;

        public UIButton SelectButton => this.selectButton;

        public MvxCachedImageView ProviderTypeImage => this.providerTypeImage;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            titleLabel.Font = UIFont.FromName("Roboto-Bold", 14.0f);
        }
    }
}
