using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.Droid.Interfaces;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Fragments;
using System;
using System.ComponentModel;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.full_region, true, BackstackTypes.ADD)]
    public class ClaimsHistorySearchView : BaseFragment, ViewTreeObserver.IOnGlobalLayoutListener
    {
        public static ClaimsHistorySearchViewModel _model;
        FrameLayout layout, MainNavLayout;
        Button ClaimHistoryTypesButton1, ClaimHistoryTypesButton2, ClaimHistoryTypesButton3;
        View rootView;
        ImageButton errorbutton1, errorbutton2;
        CustomAdapter adapter;
        bool dialogShown;
        public static MvxListView BenefitsWithoutAll;
        private Activity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimsHistorySearchCriteria, null);
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            _activity = activity;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ViewModelSetup();
            UISetup(view);

            BenefitsSetup(view);
        }

        public void OnGlobalLayout()
        {
            if (BenefitsWithoutAll.Count > 0)
                Utility.setFullListViewHeightCH(BenefitsWithoutAll);
            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }


        private void HandleCloseClaimsHistorySearchVM(object sender, EventArgs e)
        {
            _activity.RunOnUiThread(() =>
            {
                _activity.OnBackPressed();
            });
        }

        private void ClaimHistoryTypesButton1_Click(object sender, EventArgs e)
        {
            resetButtons();
            var thisButton = sender as Button;
            thisButton.Selected = true;
            _model.SelectedClaimHistoryType = _model.ClaimHistoryTypes[0];

        }

        private void ClaimHistoryTypesButton2_Click(object sender, EventArgs e)
        {
            resetButtons();
            var thisButton = sender as Button;
            thisButton.Selected = true;
            _model.SelectedClaimHistoryType = _model.ClaimHistoryTypes[1];

        }

        private void ClaimHistoryTypesButton3_Click(object sender, EventArgs e)
        {
            resetButtons();
            var thisButton = sender as Button;
            thisButton.Selected = true;
            _model.SelectedClaimHistoryType = _model.ClaimHistoryTypes[2];

        }

        public void resetButtons()
        {
            ClaimHistoryTypesButton1.Selected = false;
            ClaimHistoryTypesButton2.Selected = false;
            ClaimHistoryTypesButton3.Selected = false;
        }

        private void ViewModelSetup()
        {
            _model = (ClaimsHistorySearchViewModel)ViewModel;

            _model.PropertyChanged -= _model_PropertyChanged;
            _model.PropertyChanged += _model_PropertyChanged;

            _model.CloseClaimsHistorySearchVM -= HandleCloseClaimsHistorySearchVM;
            _model.CloseClaimsHistorySearchVM += HandleCloseClaimsHistorySearchVM;
        }

        private void UISetup(View view)
        {
            ClaimHistoryTypesButton1 = view.FindViewById<Button>(Resource.Id.ClaimHistoryTypesButton1);
            ClaimHistoryTypesButton2 = view.FindViewById<Button>(Resource.Id.ClaimHistoryTypesButton2);
            ClaimHistoryTypesButton3 = view.FindViewById<Button>(Resource.Id.ClaimHistoryTypesButton3);

            errorbutton1 = view.FindViewById<ImageButton>(Resource.Id.errorButtonClick1);
            errorbutton2 = view.FindViewById<ImageButton>(Resource.Id.errorButtonClick2);

            ClaimHistoryTypesButton1.Click -= ClaimHistoryTypesButton1_Click;
            ClaimHistoryTypesButton2.Click -= ClaimHistoryTypesButton2_Click;
            ClaimHistoryTypesButton3.Click -= ClaimHistoryTypesButton3_Click;

            ClaimHistoryTypesButton1.Click += ClaimHistoryTypesButton1_Click;
            ClaimHistoryTypesButton2.Click += ClaimHistoryTypesButton2_Click;
            ClaimHistoryTypesButton3.Click += ClaimHistoryTypesButton3_Click;

            if (_model.SelectedClaimHistoryType.ID == "CL")
            {
                ClaimHistoryTypesButton1.Selected = true;
            }
            else if (_model.SelectedClaimHistoryType.ID == "PD")
            {
                ClaimHistoryTypesButton2.Selected = true;
            }
            else if (_model.SelectedClaimHistoryType.ID == "MI")
            {
                ClaimHistoryTypesButton3.Selected = true;
            }
        }

        private void BenefitsSetup(View view)
        {
            BenefitsWithoutAll = view.FindViewById<MvxListView>(Resource.Id.benefit_list);

            view.Post(() =>
            {
                if (BenefitsWithoutAll.Count > 0)
                    Utility.setFullListViewHeightCH(BenefitsWithoutAll);

                SetCheckBoxEnabled();
            });

            rootView = view;
        }
        
        void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsRightSideGreyedOut")
            {
                if (_model.IsRightSideGreyedOut = false && layout != null)
                    layout.Visibility = ViewStates.Gone;
            }

            if (e.PropertyName == "BenefitsWithoutAll")
            {
                if (BenefitsWithoutAll != null)
                    BenefitsWithoutAll.Post(() =>
                        {
                            if (BenefitsWithoutAll.Count > 0)
                                Utility.setFullListViewHeightCH(BenefitsWithoutAll);
                        });
            }

            if (e.PropertyName == "IsFullBenefitsListVisible")
            {
                if (BenefitsWithoutAll != null)
                    BenefitsWithoutAll.Post(() =>
                    {
                        Console.WriteLine("Item Source Type = {0}", BenefitsWithoutAll.ItemsSource.GetType());
                        if (BenefitsWithoutAll.Count > 0)
                            Utility.setFullListViewHeightCHBnft(BenefitsWithoutAll);

                        SetCheckBoxEnabled();
                    });
            }

            if (e.PropertyName == "IsClaimHistoryBenefitEnabled")
            {
                if (BenefitsWithoutAll != null)

                    SetCheckBoxEnabled();

            }

            if (e.PropertyName == "BenefitValidationMessage")
            {
                if (errorbutton1 != null)
                    errorbutton1.Tag = string.Format("{0}", _model.BenefitValidationMessage);
            }

            if (e.PropertyName == "PayeeValidationMessage")
            {
                if (errorbutton2 != null)
                    errorbutton2.Tag = string.Format("{0}", _model.PayeeValidationMessage);
            }

        }

        public void SetCheckBoxEnabled()
        {
            try
            {
                if (_model.IsFullBenefitsListVisible)
                {
                    if (BenefitsWithoutAll != null)
                        BenefitsWithoutAll.Post(() =>
                        {
                            for (int i = 0; i <= BenefitsWithoutAll.LastVisiblePosition - BenefitsWithoutAll.FirstVisiblePosition; i++)
                            {
                                var item = BenefitsWithoutAll.GetChildAt(i);
                                var checkBox = item.FindViewById<CheckBox>(Resource.Id.check_box_ch);
                                var textBox = item.FindViewById<TextView>(Resource.Id.text_box_ch_benefititem);
                                textBox.Enabled = _model.IsClaimHistoryBenefitEnabled;
                                checkBox.Enabled = _model.IsClaimHistoryBenefitEnabled;
                            }
                        });


                }
            }
            catch { }
        }

    }
}