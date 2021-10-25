using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Entities;
using MobileClaims.iOS.Converters;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Menu
{
    public partial class MenuItemCellView : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("MenuItemCellView");

        public MenuItemCellView()
        {
            InitializeBinding();
        }

        protected MenuItemCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            MenuItemLabelView.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LANDING_BUTTON_FONT_SIZE);
            MenuItemLabelView.TextColor = Colors.HIGHLIGHT_COLOR;
            MenuItemLabelView.HighlightedTextColor = Colors.BACKGROUND_COLOR;
            MenuItemIconImageView.TintColor = Colors.BACKGROUND_COLOR;
       
            CounterLabel.TextColor = UIColor.White;
            CounterContainerView.BackgroundColor = Colors.DARK_RED;
            CounterContainerView.Layer.CornerRadius = 5;
        }

        internal void SetConstraints(bool shouldShowCounter)
        {
            MenuLabelTrailingConstraintWhenNoCounter.Active = !shouldShowCounter;
            MenuLabelTrailingConstraintWithCounter.Active = shouldShowCounter;
        }

        public override void SetHighlighted(bool highlighted, bool animated)
        {
            base.SetHighlighted(highlighted, animated);
            ToggleContainerBackgroundColor(highlighted);
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            ToggleContainerBackgroundColor(selected);
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var boolOppositeValueConverter = new BoolOppositeValueConverter();
                var stringToBoolValueConverter = new StringToBoolValueConverter();

                var set = this.CreateBindingSet<MenuItemCellView, MenuItem>();
                set.Bind(MenuItemLabelView).To(x => x.DisplayLabel);
                set.Bind(MenuItemIconImageView).To(x => x.Links[0].MenuItemRel).WithConversion("MenuItemRelToNormalStateMenuItemIcon");
                set.Bind(MenuItemIconImageView).For(miiiv => miiiv.HighlightedImage).To(x => x.Links[0].MenuItemRel).WithConversion("MenuItemRelToHighlightedStateMenuItemIcon");

                set.Bind(CounterContainerView).For(x => x.Hidden).To(x => x.CountDisplayValue).WithConversion(stringToBoolValueConverter, true);
                
                set.Bind(CounterLabel).To(x => x.CountDisplayValue);
                set.Apply();
            });
        }

        private void ToggleContainerBackgroundColor(bool selected)
        {
            if (selected)
            {
                MenuItemCellContainer.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            }
            else
            {
                MenuItemCellContainer.BackgroundColor = Colors.LightGrayColor;
            }
            CounterContainerView.BackgroundColor = Colors.DARK_RED;
            SeperatorView.BackgroundColor = Colors.BACKGROUND_COLOR;
        }
    }
}