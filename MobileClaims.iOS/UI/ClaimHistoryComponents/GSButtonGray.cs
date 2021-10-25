using Cirrious.FluentLayouts.Touch;
using Foundation;
using UIKit;

namespace MobileClaims.iOS
{
    public class GSButtonGray : UIButton
    {
        private UIImageView scoreButtonImageView;
        private UIView imageIconHolder;
        private float TEXT_HEIGHT = Constants.IsPhone() ? 25 : 45;
        public GSButtonGray()
        {
            this.BackgroundColor = Colors.LightGrayColor;
            this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);

            this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            this.TitleLabel.TextAlignment = UITextAlignment.Left;
            this.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 22f);
            this.ContentEdgeInsets = new UIEdgeInsets(3, 10, 0, 0);
            this.Enabled = true;

            imageIconHolder = new UIView();
            imageIconHolder.UserInteractionEnabled = false;
            imageIconHolder.BackgroundColor = Colors.Clear;
            Add(imageIconHolder);

            scoreButtonImageView = new UIImageView();
            scoreButtonImageView.Image  = UIImage.FromBundle("ClaimHistorySearchCriteriaFilter");
            scoreButtonImageView.HighlightedImage = UIImage.FromBundle("ClaimHistorySearchCriteriaFilterTouch");
            scoreButtonImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            imageIconHolder.AddSubview(scoreButtonImageView);

            SetConstraints();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            setHighlight(true);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            setHighlight(false);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            setHighlight(false);
        }

        protected void setHighlight(bool highlighted)
        {
            scoreButtonImageView.Highlighted = highlighted;
            if (highlighted)
            {
                this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);
            }
            else
            {
                this.BackgroundColor = Colors.LightGrayColor;
                this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            }
        }

        public float buttonSizeWithText()
        {
            float padding = Constants.BUTTON_TEXT_PADDING;
            float finalHeight = 0;

            this.TitleLabel.SizeToFit();

            finalHeight = (float)this.TitleLabel.Frame.Height;

            return finalHeight;
        }

        private void SetConstraints()
        {
            this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            imageIconHolder.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            if (scoreButtonImageView != null)
            {
                this.AddConstraints(
                    this.Height().EqualTo(ButtonHeight),

                    imageIconHolder.WithSameTop(this),
                    imageIconHolder.WithSameRight(this),
                    imageIconHolder.Width().EqualTo(Constants.LANDING_BUTTON_LOGO_AREA_WIDTH),
                    imageIconHolder.WithSameHeight(this),

                    scoreButtonImageView.WithSameCenterX(imageIconHolder),
                    scoreButtonImageView.WithSameCenterY(imageIconHolder),
                    scoreButtonImageView.Height().EqualTo(26)
                );
            }
        }

        private float _buttonHeight = Constants.LANDING_BUTTON_HEIGHT;
        public float ButtonHeight
        {
            get
            {
                return _buttonHeight;
            }
            set
            {
                _buttonHeight = value;
            }
        }
    }
}