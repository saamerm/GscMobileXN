using System;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimsHistoryResultsListView : BaseFragment, ViewTreeObserver.IOnGlobalLayoutListener
    {
        public static ClaimsHistoryResultsListViewModel _model;
        public SingleSelectMvxListView _list;
        public static View rootView;
        private Activity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            View _view = this.BindingInflate(Resource.Layout.ClaimsHistoryResultsListView, null);

            return _view;
        }

        public override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _model = (ClaimsHistoryResultsListViewModel)ViewModel;
        }
        
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            _activity = activity;
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _list = view.FindViewById<SingleSelectMvxListView>(Resource.Id.result_list_view);

            //view.Post(() =>
            //    {

            //        new System.Threading.Thread(new ThreadStart(() =>
            //        {
            //            _model.ShowLoadingCommand.Execute(null);
            //            heightForList = Utility.getFullListViewHeightCH(_list);
            //            this.Activity.RunOnUiThread(() => onSuccessfulLogin(heightForList));
            //        })).Start();


            //    });

            Activity.RunOnUiThread(() =>
            {
                _model.ShowLoadingCommand.Execute(null);
            });
            view.PostDelayed(() =>
                {
                    Utility.setFullListViewHeightCH(_list, _model);
                }, 500);

            rootView = view;

            _model.CloseClaimsHistoryResultsListVM -= _model_CloseClaimsHistoryResultsListVM;
            _model.CloseClaimsHistoryResultsListVM += _model_CloseClaimsHistoryResultsListVM;
        }

        void _model_CloseClaimsHistoryResultsListVM(object sender, EventArgs e)
        {
            _activity.RunOnUiThread(() =>
            {
                _activity.OnBackPressed();
            });
        }

        public void onSuccessfulLogin(int listViewHeight)
        {
            ViewGroup.LayoutParams lparams;
            lparams = _list.LayoutParameters;
            lparams.Height = listViewHeight;
            _list.LayoutParameters = lparams;

            _model.HideLoadingCommand.Execute(null);
        }

        public void OnGlobalLayout()
        {
            Utility.setFullListViewHeightCH(_list);
            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);

        }
    }
}