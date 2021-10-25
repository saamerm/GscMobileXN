using MobileClaims.Core.ViewModelParameters;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.Helpers
{
    public static class Extensions
    {
        public static MvxBundle PrepareShowDetailsCommand(this HealthProviderSummaryModel provider)
        {
            var bundle = new MvxBundle();
            var serializedSelectedProvider = JsonConvert.SerializeObject(provider);
            bundle.Data.Add(HealthProviderSummaryModel.SelectedProviderBundleKey, serializedSelectedProvider);
            return bundle;
        }
    }
}