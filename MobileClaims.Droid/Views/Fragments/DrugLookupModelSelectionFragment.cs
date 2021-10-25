using Android.OS;
using Android.Views;


using MobileClaims.Core.ViewModels;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.Views;
using Android.Widget;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region)]
    public class DrugLookupModelSelectionFragment_ : BaseFragment
    {
        private DrugLookupModelSelectionViewModel _model;
        private View _m_view;
        private ImageView _drugsDontWorkImage;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            _m_view = this.BindingInflate(Resource.Layout.DrugLookupModelSelectionFragment, null);
            return _m_view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _drugsDontWorkImage = _m_view.FindViewById<ImageView>(Resource.Id.drugsDontWork);

            _model = (DrugLookupModelSelectionViewModel)ViewModel;
            try
            {
                if (Resources.GetBoolean(Resource.Boolean.isTablet))
                {
# if GSC
                    if (_drugsDontWorkImage != null)
                    {
                        _drugsDontWorkImage.Visibility = ViewStates.Visible;
                    }

#else
                    if (_drugsDontWorkImage != null)
                    {
                        _drugsDontWorkImage.Visibility = ViewStates.Gone;
                    }
#endif
                    var list = Activity.FindViewById(Resource.Id.drug_lookup_type) as MvxListView;
                    list.Adapter = new SingleSelectionMvxAdapter(Activity, (IMvxAndroidBindingContext)BindingContext);

                    if (list.Count > 0)
                    {
                        // A static value for offset. This will not work if this model selection has more than two buttons.
                        Utility.setFullListViewHeightforHCSA(list, 80);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }
    }
}