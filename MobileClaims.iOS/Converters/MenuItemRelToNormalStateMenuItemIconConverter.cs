using System;
using System.Globalization;
using MobileClaims.Core.Entities;
using MvvmCross.Converters;
using UIKit;

namespace MobileClaims.iOS.Converters
{
    public class MenuItemRelToNormalStateMenuItemIconConverter : MvxValueConverter<MenuItemRel, UIImage>
    {
        protected override UIImage Convert(MenuItemRel value, Type targetType, object parameter, CultureInfo culture)
        {
            UIImage image = null;
            switch (value)
            {
                case MenuItemRel.MyClaimsPage:
                    image = UIImage.FromBundle("MyClaimsActive");
                    break;
                case MenuItemRel.IdCardPage:
                    image = UIImage.FromBundle("MyIdCardActive");
                    break;
                case MenuItemRel.DrugsOnTheGo:
                    image = UIImage.FromBundle("DrugsOnTheGoActive");
                    break;
                case MenuItemRel.MyBenefitsPage:
                    image = UIImage.FromBundle("MyBenefitsActive");
                    break;
                case MenuItemRel.FindAProviderPage:
                    image = UIImage.FromBundle("FindAProviderActive");
                    break;
                case MenuItemRel.FindAPharmacyPage:
                    image = UIImage.FromBundle("FindAPharmacyActive");
                    break;
                case MenuItemRel.MyAlertsPage:
                    image = UIImage.FromBundle("MyAlertsActive");
                    break;
                case MenuItemRel.MyBalancesPage:
                    image = UIImage.FromBundle("MyBalancesActive");
                    break;
                case MenuItemRel.Change4lifePage:
                    image = UIImage.FromBundle("Change4LifeActive");
                    break;
                case MenuItemRel.SureHealthPage:
                    image = UIImage.FromBundle("SureHealthActive");
                    break;
                case MenuItemRel.SupportCentreAccessPage:
                    image = UIImage.FromBundle("SupportCenterActive");
                    break;
                case MenuItemRel.TermAndConditionsPage:
                    image = UIImage.FromBundle("TermsAndConditionsActive");
                    break;
                case MenuItemRel.MySettingsPage:
                    image = UIImage.FromBundle("MySettingsActive");
                    break;
                case MenuItemRel.ContactUsExternal:
                    image = UIImage.FromBundle("ContactUsActive");
                    break;
                case MenuItemRel.LogoutPage:
                    image = UIImage.FromBundle("LogoutActive");
                    break;
                case MenuItemRel.DirectDepositPage:
                    image = UIImage.FromBundle("DirectDepositActive");
                    break;
                default:
                    image = UIImage.FromBundle("ContactUsActive");
                    break;

            }
            return image;
        }
    }
}
