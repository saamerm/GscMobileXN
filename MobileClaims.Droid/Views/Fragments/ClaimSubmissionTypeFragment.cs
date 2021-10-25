using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class ClaimSubmissionTypeFragment_ : BaseFragment
    {
        private ClaimSubmissionTypeViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var _view = this.BindingInflate(Resource.Layout.ClaimSubmissionTypeView, null);
            TextView textView = _view.FindViewById<TextView>(Resource.Id.benefits_disclaimer_text);
            textView.TextFormatted = GetTextFormatted(Resource.String.benefitsDisclaimer);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _model = (ClaimSubmissionTypeViewModel)ViewModel;
            try
            {
                ProgressDialog progressDialog = null;
                if (_model != null)
                {
                    if (_model.Busy)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                        });
                    }
                }

                SetSelectedClaimSubmissionType();

                _model.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Busy")
                    {
                        if (_model.Busy)
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                if (progressDialog == null)
                                {
                                    progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                                }
                                else
                                {
                                    if (!progressDialog.IsShowing)
                                    {
                                        progressDialog.Show();
                                    }
                                }
                            });
                        }
                        else
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                if (progressDialog != null && progressDialog.IsShowing)
                                    progressDialog.Dismiss();
                            });
                        }
                    }
                    else if (e.PropertyName == "SelectedClaimSubmissionType")
                    {
                        SetSelectedClaimSubmissionType();
                    }
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        private void SetSelectedClaimSubmissionType()
        {
            var selectedParticipant = _model.SelectedClaimSubmissionType;
            if (selectedParticipant != null)
            {
                var participantlist = Activity.FindViewById(Resource.Id.claim_type_list) as MvxListView;
                int index = _model.ClaimSubmissionTypes.FindIndex(x => x.ID == selectedParticipant.ID);
                participantlist.SetItemChecked(index, true);
            }
        }
    }
}