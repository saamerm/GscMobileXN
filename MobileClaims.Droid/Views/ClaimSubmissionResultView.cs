using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimSubmissionResultView : BaseFragment
    {
        private ClaimSubmissionResultViewModel _model;
        private bool dialogShown;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimSubmissionResultView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimSubmissionResultViewModel)ViewModel;

            NunitoTextView atv = view.FindViewById<NunitoTextView>(Resource.Id.claimSecondaryAccountNotRegisteredMsgId);
            atv.Text = Resources.FormatterBrandKeywords(Resource.String.claimSecondaryAccountNotRegisteredMsg, new[] { Resources.GetString(Resource.String.gsc) });

            NunitoTextView claimSecondaryAccountDisabledMsg = view.FindViewById<NunitoTextView>(Resource.Id.claimSecondaryAccountDisabledMsgId);
            claimSecondaryAccountDisabledMsg.Text = Resources.FormatterBrandKeywords(Resource.String.claimSecondaryAccountDisabledMsg, new[] { Resources.GetString(Resource.String.gsc) });

            if (_model.IsSelectedForAudit)
            {
                TextView claimResultTitleLabel = Activity.FindViewById<TextView>(Resource.Id.claimResultTitleLabel);
                claimResultTitleLabel.Text = string.Format(Resources.GetString(Resource.String.claimResultAuditNotificationLabel));
            }

            NunitoTextView claimSubmissionGSIDLabelPhone = view.FindViewById<NunitoTextView>(Resource.Id.claimSubmissionGSIDLabelPhone);
            claimSubmissionGSIDLabelPhone.Text = Resources.FormatterBrandKeywords(Resource.String.claimSubmissionGSIDLabelPhone, new[] { Resources.GetString(Resource.String.greenShield) });

            SetUpClaimCounter();
        }

        //Done: Fix the following 
        private void SetUpClaimCounter()
        {            
            // Solution 2            
            Resources res = Resources;
            MvxLinearLayout mvx = (MvxLinearLayout)View.FindViewById<MvxLinearLayout>(Resource.Id.ClaimResultDetailLists);            
            int TotalTreatments = mvx.ChildCount;            
            for (int n = 0; n < TotalTreatments; n++)
            {                
                LinearLayout ItemLinearLayout1 = (LinearLayout)mvx.GetChildAt(n);
                TableLayout ItemTableLayout = (TableLayout)ItemLinearLayout1.GetChildAt(0);
                TableRow row = (TableRow)ItemTableLayout.GetChildAt(0);
                if (row != null)
                {
                    NunitoTextView rowCount = (NunitoTextView)row.GetChildAt(0);
                    int tempn = n + 1;
                    if (rowCount != null)
                    {
                        rowCount.Text = string.Format(res.GetString(Resource.String.claimCountOf), tempn, TotalTreatments);
                    }
                }
            }
        }

        void Alert_audit_notification()
        {
            if (!dialogShown)
            {
                dialogShown = true;
                Resources res = Resources;
                var builder = new AlertDialog.Builder(Activity);
                builder.SetTitle(string.Format(res.GetString(Resource.String.claimResultAuditNotificationLabel)));
                builder.SetMessage(string.Format(Resources.GetString(Resource.String.claimResultAuditNotificationAlertMsg, Resource.String.gsc, Resource.String.gsc)));
                builder.SetCancelable(true);
                builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
                {
                    dialogShown = false;
                });
                builder.Show();
            }
        }
    }
}