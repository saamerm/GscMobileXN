﻿using System;
using Com.Google.Android.Play.Core.Review;
using Com.Google.Android.Play.Core.Review.Testing;
using Com.Google.Android.Play.Core.Tasks;
using MobileClaims.Core.Services;
using Plugin.CurrentActivity;

namespace MobileClaims.Droid.Services
{
    public class PlayCoreService : IPlayCoreService
    {
        public void LaunchReview()
        {
#if DEBUG
            // FakeReviewManager does not interact with the Play Store, so no UI is shown
            // and no review is performed. Useful for unit tests.
            var manager = new FakeReviewManager(CrossCurrentActivity.Current.Activity);
#else         
            var manager = ReviewManagerFactory.Create(CrossCurrentActivity.Current.Activity);
#endif            
            var request = manager.RequestReviewFlow();
            request.AddOnCompleteListener(new OnCompleteListener(manager));
        }
    }

    public class OnCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        FakeReviewManager _fakeReviewManager;
        IReviewManager _reviewManager;
        bool _usesFakeManager;
        void IOnCompleteListener.OnComplete(Task p0)
        {
            if (p0 == null || !p0.IsSuccessful)
                return;
            // Canceling the review raises an exception
            try
            {
                LaunchReview(p0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LaunchReview(Task p0)
        {
            var review = p0.GetResult(Java.Lang.Class.FromType(typeof(ReviewInfo)));
            if (_usesFakeManager)
            {
                var x = _fakeReviewManager.LaunchReviewFlow(CrossCurrentActivity.Current.Activity, (ReviewInfo)review);
                x.AddOnCompleteListener(new OnCompleteListener(_fakeReviewManager));
            }
            else
            {
                var x = _reviewManager.LaunchReviewFlow(CrossCurrentActivity.Current.Activity, (ReviewInfo)review);
                x.AddOnCompleteListener(new OnCompleteListener(_reviewManager));
            }
        }

        public OnCompleteListener(FakeReviewManager fakeReviewManager)
        {
            _fakeReviewManager = fakeReviewManager;
            _usesFakeManager = true;
        }
        public OnCompleteListener(IReviewManager reviewManager)
        {
            _reviewManager = reviewManager;
        }
    }
}
