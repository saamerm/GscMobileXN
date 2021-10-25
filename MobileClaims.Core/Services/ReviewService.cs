using System;
using System.Threading.Tasks;
using Plugin.StoreReview;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Xamarin.Essentials;

namespace MobileClaims.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IPlayCoreService _playCoreService;
        private readonly IDeviceService _deviceService;

        public ReviewService(IPlayCoreService playCoreService, IDeviceService deviceService)
        {
            _playCoreService = playCoreService;
            _deviceService = deviceService;
        }

        public async Task RequestReview()
        {
            int submissionCounter = Preferences.Get("AppRatingCounter", 0);


            if (ShouldShowReview(submissionCounter))
            {
                Analytics.TrackEvent("PageView: UsefulAppModal");

                bool goodExperience = await UserDialogs.Instance.ConfirmAsync("", Resource.AppReviewBody, Resource.Yes, Resource.No);
                if (goodExperience)
                {
                    Analytics.TrackEvent("SelectAction: UsefulAppPositiveResponse");
                    if (_deviceService.CurrentDevice == GSCHelper.OS.Droid)
                        _playCoreService.LaunchReview();
                    else
                        await CrossStoreReview.Current.RequestReview(false);
                }
            }
            submissionCounter++;
            Preferences.Set("AppRatingCounter", submissionCounter);
        }

        public bool ShouldShowReview(int count) => (count == 0 || count == 2 || count == 9);
    }
}
