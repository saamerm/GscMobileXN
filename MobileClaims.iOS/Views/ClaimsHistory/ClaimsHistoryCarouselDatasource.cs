using System;
using Carousels;
using CoreGraphics;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.iOS.UI.ClaimHistoryComponents;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimsHistoryCarouselDatasource : iCarouselDataSource
    {
        ClaimsHistoryResultDetailViewModel _model;
        public CGRect _viewbounds{ get; set;}
        public float ViewWidth{ get; set;}
        public ClaimsHistoryCarouselDatasource(ClaimsHistoryResultDetailViewModel model)
        {
            _model = model;
        }
        public ClaimsHistoryCarouselDatasource(ClaimsHistoryResultDetailViewModel model, CGRect bounds)
        {
            _model = model;
            _viewbounds = bounds;
        }
        public override nint GetNumberOfItems (iCarousel carousel)
        {
            // return the number of items in the data
            _viewbounds = carousel.Bounds;
            return _model.SearchResults.Count;
        }

        public override UIView GetViewForItem (iCarousel carousel, nint index, UIView view)
        {
            if (view != null)
            {
                (view as ClaimHistoryDetailComponent).ShowPaymentInformationEvent -= ShowPayMentInformation;
            }
            var ret = new ClaimHistoryDetailComponent(_model.SearchResults[(int)index], _model.PaymentInformationLabel);

            ret.ShowPaymentInformationEvent += ShowPayMentInformation;
            ret.Frame = _viewbounds;
            ret.LayoutSubviews();
            ret.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

            return ret;       
        }

        private void ShowPayMentInformation(object sender, EventArgs e)
        {
            ClaimState selectedResult = sender as ClaimState;
            _model.SelectedSearchResult = selectedResult; 
            _model.ShowPaymentInfoCommand.Execute(null);
        }

    }
}