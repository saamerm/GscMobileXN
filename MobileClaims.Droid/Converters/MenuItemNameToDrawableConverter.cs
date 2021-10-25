using System;
using System.Globalization;
using Android.Support.V4.Content;



using MobileClaims.Core.Entities;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Converters
{
    public class MenuItemNameToDrawableConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentActiviy = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;     

            switch ((MenuItemRel)value)
            {
                case MenuItemRel.MyClaimsPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_claims);
                }
                case MenuItemRel.IdCardPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_idcard);
                }
                case MenuItemRel.DirectDepositPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_directDeposit);//To be changed when XML vector assest is provided.
                }
                case MenuItemRel.DrugsOnTheGo:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_drugs);
                }
                case MenuItemRel.MyBenefitsPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_benefits);
                }
                case MenuItemRel.FindAProviderPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_find_provider);
                }
                case MenuItemRel.FindAPharmacyPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_find_pharmacy);
                }
                case MenuItemRel.MyAlertsPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_alerts);
                }
                case MenuItemRel.MyBalancesPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_balances);
                }
                case MenuItemRel.Change4lifePage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_change4life);
                }
                case MenuItemRel.SureHealthPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_surehealth);
                }
                case MenuItemRel.SupportCentreAccessPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_support_centre);
                }
                case MenuItemRel.TermAndConditionsPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_terms_and_conditions);
                }
                case MenuItemRel.MySettingsPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_settings);
                }
                case MenuItemRel.ContactUsExternal:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_contact);
                }
                case MenuItemRel.LogoutPage:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_logout);
                }
                default:
                {
                    return ContextCompat.GetDrawable(currentActiviy, Resource.Drawable.menu_active_contact);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}