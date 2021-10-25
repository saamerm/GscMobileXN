using Android.OS;
using Android.Text.Method;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Views;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimTermsAndConditionsView : BaseFragment, IMvxView
    {
		//ClaimSubmissionTypeViewModel _model;

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            // Set the layout
            return this.BindingInflate(Resource.Layout.ClaimTermsAndConditionsView, null);
        }

        public override void OnViewCreated(Android.Views.View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
			//_model = (ClaimSubmissionTypeViewModel)this.ViewModel;

			// Button acceptButton = this.Activity.FindViewById<Button> (Resource.Id.claimAgreeBtn);
            //acceptButton.Click += HandleAcceptAgreementClick;


            TextView termsTextView = this.Activity.FindViewById<TextView>(Resource.Id.claim_term_text);
            termsTextView.MovementMethod = new ScrollingMovementMethod();
            termsTextView.Text = Resources.GetString(Resource.String.privacyContent);

			TextView claimTermsTabletTitle = this.Activity.FindViewById<TextView> (Resource.Id.claimTermsTabletTitle);

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
			
				claimTermsTabletTitle.Visibility = Android.Views.ViewStates.Visible;
			} else {
				claimTermsTabletTitle.Visibility = Android.Views.ViewStates.Gone;
			}
        }
    }
}