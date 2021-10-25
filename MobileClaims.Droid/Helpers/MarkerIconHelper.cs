namespace MobileClaims.Droid.Helpers
{
    public static class MarkerIconHelper
    {
        public static int ScoreToFavouriteResourceId(double score)
        {
            int resourceId;

            switch (score)
            {
                case 1.0:
                {
                    resourceId = Resource.Drawable.pin_fav_1_0;
                    break;
                }
                case 1.5:
                {
                    resourceId = Resource.Drawable.pin_fav_1_5;
                    break;
                }
                case 2.0:
                {
                    resourceId = Resource.Drawable.pin_fav_2_0;
                    break;
                }
                case 2.5:
                {
                    resourceId = Resource.Drawable.pin_fav_2_5;
                    break;
                }
                case 3.0:
                {
                    resourceId = Resource.Drawable.pin_fav_3_0;
                    break;
                }
                case 3.5:
                {
                    resourceId = Resource.Drawable.pin_fav_3_5;
                    break;
                }
                case 4.0:
                {
                    resourceId = Resource.Drawable.pin_fav_4_0;
                    break;
                }
                case 4.5:
                {
                    resourceId = Resource.Drawable.pin_fav_4_5;
                    break;
                }
                case 5.0:
                {
                    resourceId = Resource.Drawable.pin_fav_5_0;
                    break;
                }
                default:
                {
                    resourceId = Resource.Drawable.pin_fav_default;
                    break;
                }
            }

            return resourceId;
        }

        public static int ScoreToNotFavouriteResourceId(double score)
        {
            int resourceId;

            switch (score)
            {
                case 1.0:
                {
                    resourceId = Resource.Drawable.pin_def_1_0;
                    break;
                }
                case 1.5:
                {
                    resourceId = Resource.Drawable.pin_def_1_5;
                    break;
                }
                case 2.0:
                {
                    resourceId = Resource.Drawable.pin_def_2_0;
                    break;
                }
                case 2.5:
                {
                    resourceId = Resource.Drawable.pin_def_2_5;
                    break;
                }
                case 3.0:
                {
                    resourceId = Resource.Drawable.pin_def_3_0;
                    break;
                }
                case 3.5:
                {
                    resourceId = Resource.Drawable.pin_def_3_5;
                    break;
                }
                case 4.0:
                {
                    resourceId = Resource.Drawable.pin_def_4_0;
                    break;
                }
                case 4.5:
                {
                    resourceId = Resource.Drawable.pin_def_4_5;
                    break;
                }
                case 5.0:
                {
                    resourceId = Resource.Drawable.pin_def_5_0;
                    break;
                }
                default:
                {
                    resourceId = Resource.Drawable.pin_default;
                    break;
                }
            }

            return resourceId;
        }

        public static int ProviderTypeToResourceId(int providerId)
        {
            int resourceId;

            switch (providerId)
            {
                case 1:
                {
                    //Counselling Services
                    resourceId = Resource.Drawable.pin_counselling;
                    break;
                }
                case 2:
                {
                    //Dental
                    resourceId = Resource.Drawable.pin_dental;
                    break;
                }
                case 3:
                {
                    //Foot Care
                    resourceId = Resource.Drawable.pin_foot_care;
                    break;
                }
                case 4:
                {
                    //Medical Items/Services
                    resourceId = Resource.Drawable.pin_medical_items;
                    break;
                }
                case 6:
                {
                    //Professional Services
                    resourceId = Resource.Drawable.pin_professional_services;
                    break;
                }
                case 7:
                {
                    //Pharmacy
                    resourceId = Resource.Drawable.pin_pharmacy;
                    break;
                }
                case 8:
                {
                    //Vision
                    resourceId = Resource.Drawable.pin_vision;
                    break;
                }
                default:
                {
                    resourceId = Resource.Drawable.pin_default;
                    break;
                }
            }

            return resourceId;
        }
    }
}