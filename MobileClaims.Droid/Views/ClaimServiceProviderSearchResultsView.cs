using Android.Content;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimServiceProviderSearchResultsView : BaseFragment
    {
        private View m_view;
        NestedScrollView nestedScrollView;
        ClaimServiceProviderSearchResultsViewModel _model;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            m_view = this.BindingInflate(Resource.Layout.ClaimServiceProviderSearchResultsView, null);
            nestedScrollView = m_view.FindViewById<NestedScrollView>(Resource.Id.scrollViewId);
            return m_view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var inputManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.NotAlways);

            _model = (ClaimServiceProviderSearchResultsViewModel)ViewModel;

            var providerlist = Activity.FindViewById<SingleSelectMvxListView>(Resource.Id.search_provider_list);
            providerlist.SetOnTouchListener(new ListViewTouchListener(nestedScrollView));

            if (providerlist.Count > 0)
            {
                Utility.setFullListViewHeight(providerlist);
            }

            var claimSelectProviderListTitle = Activity.FindViewById<TextView>(Resource.Id.claim_select_provider_list_title);
            claimSelectProviderListTitle.Text = string.Format(Resources.GetString(Resource.String.selectProviderFromListLabel), (_model.ClaimSubmissionType.Name).ToLower());

            var stillCantFindProviderLabel = Activity.FindViewById<TextView>(Resource.Id.still_cant_find_provider_label);
            stillCantFindProviderLabel.Text = string.Format(Resources.GetString(Resource.String.stillCantFindProviderLabel), (_model.ClaimSubmissionType.Name).ToLower());

            var stillCantFindProviderDescLabel = Activity.FindViewById<TextView>(Resource.Id.still_cant_find_provider_desclabel);
            stillCantFindProviderDescLabel.Text = stillCantFindProviderDescLabel.Text = Resources.FormatterBrandKeywords(Resource.String.stillCantFindProviderDescLabel, new string[] { Resources.GetString(Resource.String.gsc) });

            var myServiceProvider = Activity.FindViewById<Button>(Resource.Id.myServiceProvider);
            myServiceProvider.Text = string.Format(Resources.GetString(Resource.String.myServiceProviderLabel), (_model.ClaimSubmissionType.Name).ToLower());
        }

        private class ListViewTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            private NestedScrollView nestedScrollView;

            public ListViewTouchListener(NestedScrollView nestedScrollView)
            {
                this.nestedScrollView = nestedScrollView;
            }

            public bool OnTouch(View v, MotionEvent e)
            {
                nestedScrollView.RequestDisallowInterceptTouchEvent(true);
                MotionEventActions action = e.ActionMasked;
                switch(action)
                {
                    case MotionEventActions.PointerUp:
                        nestedScrollView.RequestDisallowInterceptTouchEvent(false);
                        break;
                }
                return false;
            }
        }
    }

}