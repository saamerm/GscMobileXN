using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.DirectDeposit.DirectDepositResult
{
    [Region(Resource.Id.phone_main_region)]
    public class DDResultsFragment : BaseFragment<DirectDepositResultsViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = base.OnCreateView(inflater, container, savedInstanceState);

            view = this.BindingInflate(Resource.Layout.DDResultsFragment, null);

            return view;
        }
    }
}
