using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Cheesebaron.MvvmCross.Bindings.Droid;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
	[Region(Resource.Id.right_region, false, BackstackTypes.ADD)]
    public class ClaimsHistoryResultDetailView : BaseFragment, ViewTreeObserver.IOnGlobalLayoutListener
	{
		public static ClaimsHistoryResultDetailViewModel _model;
        public static BindableViewPager pager;
        public static TextView pageNumber;
	    private View _rootView;
	    private CirclePageIndicator _titleIndicator;
      
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			return this.BindingInflate(Resource.Layout.ClaimHistoryDetailsViewPager, null);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (ClaimsHistoryResultDetailViewModel)ViewModel;

             _titleIndicator = Activity.FindViewById<CirclePageIndicator>(Resource.Id.indicator);
            pager= Activity.FindViewById<BindableViewPager>(Resource.Id.viewPager);
            pageNumber = Activity.FindViewById<TextView>(Resource.Id.currentPage);
          
            var selected = _model.SelectedSearchResult;
           // int index = _model.SearchResults.IndexOf(_model.SearchResults.Where(X => X.ClaimDetailID == selected.ClaimDetailID).FirstOrDefault());
            int index = _model.SearchResults.IndexOf(selected);
            Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

            pager.CurrentItem = index;
           
            _titleIndicator.SetViewPager(pager);
            _titleIndicator.SetOnPageChangeListener(new MyPageChangeListener(Activity));

            _titleIndicator.Centered = true;
            _titleIndicator.PageColor = Resources.GetColor(Resource.Color.highlight_color);

            pageNumber.Post(() =>
                {
                    pageNumber.Text = $"{index + 1}";
                });

            _rootView = view;
		}

	    public void OnGlobalLayout()
        {
            pager.Adapter.ReloadAllOnDataSetChange = true;
            pager.Adapter.NotifyDataSetChanged();

            _rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this); 
        }
     
        public class MyPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
        {
            Context _context;

            public MyPageChangeListener(Context context)
            {
                _context = context;

            }

            public void OnPageScrollStateChanged(int p0)
            {
            }

            public void OnPageScrolled(int p0, float p1, int p2)
            {
            
            }

            public void OnPageSelected(int position)
            {
                _model.SelectedSearchResult = _model.SearchResults[position];
                pageNumber.Post(() =>
                {           
                    pageNumber.Text = string.Format("{0}", position + 1);
                });

              
                pager.DestroyDrawingCache();                
                pager.RequestLayout();
            }
        }
	}
}