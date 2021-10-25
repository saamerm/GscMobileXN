using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.full_region, BackstackBehaviour = BackstackTypes.ADD)]
    public class HealthProviderTypeListView : BaseFragment<HealthProviderTypeListViewModel>
    {
        private View _view;
        private MvxExpandableListView _mvxExpandableListView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.HealthProviderTypeListLayout, null);

            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Init();
        }

        private void Init()
        {
            _mvxExpandableListView = _view.FindViewById<MvxExpandableListView>(Resource.Id.expandableListView);

            View headerView = View.Inflate(this.Activity, Resource.Layout.health_provider_type_header_layout, null);
            var refineSearchText = headerView.FindViewById<TextView>(Resource.Id.refineSearchHeaderTextView);

            var typeface = Typeface.CreateFromAsset(Activity.Assets, "fonts/LeagueGothic.ttf");
            refineSearchText.SetTypeface(typeface, TypefaceStyle.Normal);

            _mvxExpandableListView.AddHeaderView(headerView);
            _mvxExpandableListView.GroupTemplateId = Resource.Layout.health_provider_type_group_layout;
            _mvxExpandableListView.ItemTemplateId = Resource.Layout.health_provider_type_item_layout;

            var adapter = new HealthProviderTypeListAdapter(_mvxExpandableListView, ViewModel.HealthProviderTypeList, this.Activity);
            _mvxExpandableListView.SetAdapter(adapter);

            _mvxExpandableListView.GroupExpand += MvxExpandableListViewOnGroupExpand;
            _mvxExpandableListView.ChildClick += MvxExpandableListViewOnChildClick;
            ((ExpandableListView)_mvxExpandableListView).GroupClick += MvxExpandableListViewOnGroupClick;
        }

        private void MvxExpandableListViewOnChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            var providerTypeElement = ViewModel.HealthProviderTypeList[e.GroupPosition].ChildItems[e.ChildPosition];
            providerTypeElement.IsSelected = true;
            ViewModel.ProviderTypeSelectedCommand.Execute(providerTypeElement);
            ((sender as MvxExpandableListView).ExpandableListAdapter as IMarkElement).UnmarkAllButThis(providerTypeElement);
            ((sender as MvxExpandableListView).ExpandableListAdapter as HealthProviderTypeListAdapter).NotifyDataSetChanged();
        }

        private void MvxExpandableListViewOnGroupClick(object sender, ExpandableListView.GroupClickEventArgs e)
        {
            var providerTypeElement = ViewModel.HealthProviderTypeList[e.GroupPosition];
            if (providerTypeElement.IsSearchable == false)
            {
                if (_mvxExpandableListView.IsGroupExpanded(e.GroupPosition))
                {
                    _mvxExpandableListView.CollapseGroup(e.GroupPosition);
                }
                else
                {
                    _mvxExpandableListView.ExpandGroup(e.GroupPosition);
                }
                return;
            }
            providerTypeElement.IsSelected = true;
            ViewModel.ProviderTypeSelectedCommand.Execute(providerTypeElement);
            ((sender as MvxExpandableListView).ExpandableListAdapter as IMarkElement).UnmarkAllButThis(providerTypeElement);
            ((sender as MvxExpandableListView).ExpandableListAdapter as HealthProviderTypeListAdapter).NotifyDataSetChanged();
        }

        private void MvxExpandableListViewOnGroupExpand(object sender, ExpandableListView.GroupExpandEventArgs e)
        {
            var currentProviderGroup = ViewModel.HealthProviderTypeList[e.GroupPosition];
            if (currentProviderGroup.ChildItems.Count == 0)
            {
                currentProviderGroup.IsSelected = true;
                ViewModel.ProviderTypeSelectedCommand.Execute(currentProviderGroup);
                ((sender as MvxExpandableListView).ExpandableListAdapter as IMarkElement).UnmarkAllButThis(currentProviderGroup);
                ((sender as MvxExpandableListView).ExpandableListAdapter as HealthProviderTypeListAdapter).NotifyDataSetChanged();
            }
        }
    }

    public interface IMarkElement
    {
        void UnmarkAllButThis(HealthProviderTypeViewModel providerTypeElement);
    }
}