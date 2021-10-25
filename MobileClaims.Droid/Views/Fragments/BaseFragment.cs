using Android.OS;
using Android.Views;
using FluentValidation.Internal;
using Microsoft.AppCenter.Analytics;
using MobileClaims.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.ViewModels;

namespace MobileClaims.Droid
{
    public class BaseFragment : MvxFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Analytics.TrackEvent("PageView: " + this.GetType().Name.Replace("Fragment", string.Empty).SplitPascalCase());
            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }

    public class BaseFragment<T> : MvxFragment<T> where T : MvxViewModel
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Analytics.TrackEvent("PageView: " + this.GetType().Name.Replace("Fragment", string.Empty).SplitPascalCase());
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        protected MvxFluentBindingDescriptionSet<TTarget, TSource> SetBinding<TTarget, TSource>()
            where TTarget : BaseFragment<T>
            where TSource : ViewModelBase
        {
            var set = MvvmCross.Binding.BindingContext.MvxBindingContextOwnerExtensions.CreateBindingSet<TTarget, TSource>(this as TTarget);
            return set;
        }
    }
}
