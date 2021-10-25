using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class AuditInformationView : BaseFragment
    {
        private AuditInformationViewModel _model;
        private bool _dialogShown;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            // Set the layout
            return this.BindingInflate(Resource.Layout.AuditInformationView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (AuditInformationViewModel)ViewModel;

            var claimResultAuditNotificationView = Activity.FindViewById<TextView>(Resource.Id.claimResultAuditNotification);
            claimResultAuditNotificationView.Text = string.Format(Resources.GetString(Resource.String.claimResultAuditNotificationMsgHeader), _model.ClaimResult?.ClaimFormID);

            var benefitSubmissionGSIDNocolonLabel = view.FindViewById<NunitoTextView>(Resource.Id.benefitSubmissionGSIDNocolonLabel);
            benefitSubmissionGSIDNocolonLabel.Text = Resources.FormatterBrandKeywords(Resource.String.benefitSubmissionGSIDNocolonLabel, new[] { Resources.GetString(Resource.String.greenShield) });

            var notificationButtonClick = Activity.FindViewById<Button>(Resource.Id.claimResultAuditNotificationBtn);
            notificationButtonClick.Click += (newSender, newE) =>
            {
                AlertAuditNotification();
            };

            var doneBtn = Activity.FindViewById<Button>(Resource.Id.doneBtn);
            doneBtn.Click += (newSender, newE) =>
            {
                _model.DoneCommand.Execute(null);
                Activity.FragmentManager.PopBackStack();
            };
        }

        private void AlertAuditNotification()
        {
            if (!_dialogShown)
            {
                Activity.RunOnUiThread(() =>
                {
                    _dialogShown = true;
                    var builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle(string.Format(Resources.GetString(Resource.String.claimResultAuditNotificationLabel)));
                    builder.SetMessage(string.Format(Resources.GetString(Resource.String.claimResultAuditNotificationAlertMsg, Resource.String.gsc, Resource.String.gsc)));
                    builder.SetCancelable(true);
                    builder.SetPositiveButton(string.Format(Resources.GetString(Resource.String.OK)), delegate
                    {
                        _dialogShown = false;
                    });
                    builder.Show();
                });
            }
        }
    }
}