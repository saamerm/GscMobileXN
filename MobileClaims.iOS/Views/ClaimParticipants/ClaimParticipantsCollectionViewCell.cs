using System;
using Foundation;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimParticipants
{
    public partial class ClaimParticipantsCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("ClaimParticipantsCollectionViewCell");
        public static readonly UINib Nib;

        public override bool Highlighted
        {
            get => base.Highlighted;
            set
            {
                base.Highlighted = value;
                ToggleContainerBackgroundColor(Highlighted);
            }
        }

        public override bool Selected 
        {
            get => base.Selected;
            set
            {
                base.Selected = value;
                ToggleContainerBackgroundColor(Selected);
            }
        }

        static ClaimParticipantsCollectionViewCell()
        {
            Nib = UINib.FromName("ClaimParticipantsCollectionViewCell", NSBundle.MainBundle);
        }

        protected ClaimParticipantsCollectionViewCell(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
            SetBackground();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            NameLabel.TextColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
            NameLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.LIST_ITEM_FONT_SIZE);
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimParticipantsCollectionViewCell, Participant>();
                set.Bind(NameLabel).To(vm => vm.FullName);
                set.Apply();
            });
        }

        private void SetBackground()
        {
            ContentView.BackgroundColor = Colors.LightGrayColor;
            this.UserInteractionEnabled = true;
        }

        private void ToggleContainerBackgroundColor(bool highlighted)
        {
            if (highlighted)
            {
                ContentView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                ToggleLabelTextColorWhenHighlighted(Colors.SINGLE_SELECTION_LABEL_HIGHLIGHT_COLOR);
            }
            else
            {
                ContentView.BackgroundColor = Colors.LightGrayColor;
                ToggleLabelTextColorWhenHighlighted(Colors.SINGLE_SELECTION_LABEL_COLOR);
            }
        }

        private void ToggleLabelTextColorWhenHighlighted(UIColor fontColor)
        {
            NameLabel.TextColor = fontColor;
        }
    }
}
