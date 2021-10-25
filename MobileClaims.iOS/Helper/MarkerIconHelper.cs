using UIKit;

namespace MobileClaims.iOS.Helper
{
    public class MarkerIconHelper
    {
        public static UIImage ScoreToFavouriteUiImage(double score, bool isEnglishLanguage)
        {
            UIImage pinIcon;

            switch (score)
            {
                case 1.0:
                {
                    pinIcon = UIImage.FromBundle("PinFavourite1");
                    break;
                }
                case 1.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("PinFavourite1_5") : UIImage.FromBundle("PinFavouriteFr1_5");
                    break;
                }
                case 2.0:
                {
                    pinIcon = UIImage.FromBundle("PinFavourite2");
                    break;
                }
                case 2.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("PinFavourite2_5") : UIImage.FromBundle("PinFavouriteFr2_5");
                    break;
                }
                case 3.0:
                {
                    pinIcon = UIImage.FromBundle("PinFavourite3");
                    break;
                }
                case 3.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("PinFavourite3_5") : UIImage.FromBundle("PinFavouriteFr3_5");
                    break;
                }
                case 4.0:
                {
                    pinIcon = UIImage.FromBundle("PinFavourite4");
                    break;
                }
                case 4.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("PinFavourite4_5") : UIImage.FromBundle("PinFavouriteFr4_5");
                    break;
                }
                case 5.0:
                {
                    pinIcon = UIImage.FromBundle("PinFavourite5");
                    break;
                }
                default:
                {
                    pinIcon = UIImage.FromBundle("PinFavourite");
                    break;
                }
            }

            return pinIcon;
        }

        public static UIImage ScoreToNotFavouriteUiImage(double score, bool isEnglishLanguage)
        {
            UIImage pinIcon;

            switch (score)
            {
                case 1.0:
                {
                    pinIcon = UIImage.FromBundle("Pin1");
                    break;
                }
                case 1.5:
                {
                        pinIcon = isEnglishLanguage ? UIImage.FromBundle("Pin1_5") : UIImage.FromBundle("PinFr1_5");
                    break;
                }
                case 2.0:
                {
                    pinIcon = UIImage.FromBundle("Pin2");
                    break;
                }
                case 2.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("Pin2_5") : UIImage.FromBundle("PinFr2_5");
                    break;
                }
                case 3.0:
                {
                    pinIcon = UIImage.FromBundle("Pin3");
                    break;
                }
                case 3.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("Pin3_5") : UIImage.FromBundle("PinFr3_5");
                    break;
                }
                case 4.0:
                {
                    pinIcon = UIImage.FromBundle("Pin4");
                    break;
                }
                case 4.5:
                {
                    pinIcon = isEnglishLanguage ? UIImage.FromBundle("Pin4_5") : UIImage.FromBundle("PinFr4_5");
                    break;
                }
                case 5.0:
                {
                    pinIcon = UIImage.FromBundle("Pin5");
                    break;
                }
                default:
                {
                    pinIcon = UIImage.FromBundle("PinDefault");
                    break;
                }
            }

            return pinIcon;
        }

        public static UIImage ProviderTypeToUiImage(int providerId)
        {
            UIImage pinIcon;

            switch (providerId)
            {
                case 1:
                {
                    pinIcon = UIImage.FromBundle("PinCounselling");
                    break;
                }
                case 2:
                {
                    pinIcon = UIImage.FromBundle("PinDental");
                    break;
                }
                case 3:
                {
                    pinIcon = UIImage.FromBundle("PinFootCare");
                    break;
                }
                case 4:
                {
                    pinIcon = UIImage.FromBundle("PinMedicalItem");
                    break;
                }
                case 6:
                {
                    pinIcon = UIImage.FromBundle("PinProfessionalServices");
                    break;
                }
                case 7:
                {
                    pinIcon = UIImage.FromBundle("PinPharmacy");
                    break;
                }
                case 8:
                {
                    pinIcon = UIImage.FromBundle("PinVision");
                    break;
                }
                default:
                {
                    pinIcon = UIImage.FromBundle("PinDefault");
                    break;
                }
            }

            return pinIcon;
        }
    }
}