using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Views;
using System.Collections.ObjectModel;
using Android.Content.Res;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid
{

    [Region(Resource.Id.phone_main_region)]
	public class ClaimResultsView : BaseFragment , IMvxView , ViewTreeObserver.IOnGlobalLayoutListener
	{
		
		public static ClaimResultsViewModel _model;
        public static Activity _activity;
        NonSelectableList ClaimDetailsList;
        ObservableCollection<ClaimDetail> Details;
        NonSelectableList EobMessages;
        View rootView;

        public void OnGlobalLayout()
        {
            ClaimDetailsList.Post(() =>
            {
                Utility.setFullListViewHeightwithEobListAsChild(ClaimDetailsList);
                setFullListViewHeightForEOB();
            });

            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimResultsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimResultsViewModel) ViewModel;

            _activity = Activity;

            ClaimDetailsList = Activity.FindViewById<NonSelectableList>(Resource.Id.claim_results_detailsLists);

            if (ClaimDetailsList.Count > 0)
            {
                ClaimDetailsList.Post(() =>
                {
                    Utility.setFullListViewHeightwithEobListAsChild(ClaimDetailsList);
                    setFullListViewHeightForEOB();
                });

                rootView = view;
            }
        }

        private void setFullListViewHeightForEOB()
        {
            ClaimDetailsList.Post(() =>
            {
                for (var i = 0; i <= ClaimDetailsList.LastVisiblePosition - ClaimDetailsList.FirstVisiblePosition; i++)
                {
                    var position = i - ClaimDetailsList.FirstVisiblePosition;
                    var item = ClaimDetailsList.GetChildAt(position);
                    EobMessages = item.FindViewById<NonSelectableList>(Resource.Id.eob_messages);
                    if (EobMessages.Count > 0)
                        Utility.setFullListViewHeightCH(EobMessages);
                }
            });
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }

    }
}