using System;
using System.Globalization;
using MobileClaims.Core.Entities;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class MenuItemRelToHighlightedStateMenuItemIconConverter : MvxValueConverter<MenuItemRel, UIImage>
    {
        protected override UIImage Convert(MenuItemRel value, Type targetType, object parameter, CultureInfo culture)
        {
            UIImage image = null;
            switch (value)
            {
                case MenuItemRel.MyClaimsPage:
                    image = UIImage.FromBundle("MyClaimsTouch");
                    break;
                case MenuItemRel.IdCardPage:
                    image = UIImage.FromBundle("MyIdCardTouch");
                    break;
                case MenuItemRel.DrugsOnTheGo:
                    image = UIImage.FromBundle("DrugsOnTheGoTouch");
                    break;
                case MenuItemRel.MyBenefitsPage:
                    image = UIImage.FromBundle("MyBenefitsTouch");
                    break;
                case MenuItemRel.FindAProviderPage:
                    image = UIImage.FromBundle("FindAProviderTouch");
                    break;
                case MenuItemRel.FindAPharmacyPage:
                    image = UIImage.FromBundle("FindAPharmacyTouch");
                    break;
                case MenuItemRel.MyAlertsPage:
                    image = UIImage.FromBundle("MyAlertsTouch");
                    break;
                case MenuItemRel.MyBalancesPage:
                    image = UIImage.FromBundle("MyBalancesTouch");
                    break;
                case MenuItemRel.Change4lifePage:
                    image = UIImage.FromBundle("Change4LifeTouch");
                    break;
                case MenuItemRel.SureHealthPage:
                    image = UIImage.FromBundle("SureHealthTouch");
                    break;
                case MenuItemRel.SupportCentreAccessPage:
                    image = UIImage.FromBundle("SupportCenterTouch");
                    break;
                case MenuItemRel.TermAndConditionsPage:
                    image = UIImage.FromBundle("TermsAndConditionsTouch");
                    break;
                case MenuItemRel.MySettingsPage:
                    image = UIImage.FromBundle("MySettingsTouch");
                    break;
                case MenuItemRel.ContactUsExternal:
                    image = UIImage.FromBundle("ContactUsTouch");
                    break;
                case MenuItemRel.LogoutPage:
                    image = UIImage.FromBundle("LogoutTouch");
                    break;
                case MenuItemRel.DirectDepositPage:
                    image = UIImage.FromBundle("DirectDepositTouch");
                    break;
                default:
                    image = UIImage.FromBundle("ContactUsTouch");
                    break;
            }
            return image;
        }
    }
}
