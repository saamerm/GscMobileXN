using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimTypeView : BaseFragment
    {
        private TextView CollapsText;
        private RelativeLayout DescriptionLayout;
        private ImageButton ArrowButton;
        private LinearLayout description_label;
        private ClaimTypeViewModel _model;
        private MvxSpinner _claimtype;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.ClaimTypeView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _model = (ClaimTypeViewModel)ViewModel;

            _claimtype = view.FindViewById<MvxSpinner>(Resource.Id.claimtype_spinner);
            var set = this.CreateBindingSet<ClaimTypeView, ClaimTypeViewModel>();
            set.Bind(_claimtype).For(v => v.ItemsSource).To(vm => vm.ClaimTypes).Mode(MvxBindingMode.OneWay);
            set.Bind(_claimtype).For(v => v.SelectedItem).To(vm => vm.SelectedClaimType).Mode(MvxBindingMode.TwoWay);

            set.Apply();

            DescriptionLayout = view.FindViewById<RelativeLayout>(Resource.Id.description_layout);
            ArrowButton = view.FindViewById<ImageButton>(Resource.Id.collapse_expand_button);
            description_label = view.FindViewById<LinearLayout>(Resource.Id.title_label);
            CollapsText = view.FindViewById<TextView>(Resource.Id.gsc_tog_text);
            CollapsText.Visibility = ViewStates.Gone;

            if (this.Resources.GetBoolean(Resource.Boolean.isTablet))
            {
                CollapsText.Visibility = ViewStates.Visible;
                DescriptionLayout.SetBackgroundResource(Resource.Drawable.HCSABorderTextViewNormal);
                ArrowButton.Visibility = ViewStates.Gone;
            }
            else
            {
                description_label.Click += delegate
                {
                    if (_model.IsExpenseTypeDescriptionCollapsed == true)
                    {
                        CollapsText.Visibility = ViewStates.Visible;
                        _model.IsExpenseTypeDescriptionCollapsed = false;
                        ArrowButton.SetImageResource(Resource.Drawable.Collapse_down_arrow_icon);
                    }
                    else if (_model.IsExpenseTypeDescriptionCollapsed == false)
                    {
                        CollapsText.Visibility = ViewStates.Gone;
                        _model.IsExpenseTypeDescriptionCollapsed = true;
                        ArrowButton.SetImageResource(Resource.Drawable.Expand_down_arrow_icon);
                    }
                };
            }
        }
    }
}