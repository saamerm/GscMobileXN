
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.DirectDeposit.DirectDepositError
{
    [Region(Resource.Id.phone_main_region)]
    public class DDValidationErrorFragment : BaseFragment<DDValidationErrorViewModel>
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

            view = this.BindingInflate(Resource.Layout.DDValidationErrorFragment, null);

            return view;
        }
    }
}
