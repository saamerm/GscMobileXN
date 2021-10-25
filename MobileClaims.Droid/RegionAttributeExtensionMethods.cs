using System;
using System.Linq;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid
{

    /// <summary>
    /// Extension methods that allow views to access an associated RegionAttribute.
    /// </summary>
    static public class RegionAttributeExtentionMethods
    {
        /// <summary>
        /// Returns true iff the view has a region attribute.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool HasRegionAttribute(this MvxFragment view)
        {
            try
            {
                var attributes = view
                    .GetType()
                    .GetCustomAttributes(typeof(RegionAttribute), true);
                return attributes.Count() > 0;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Gets the Android resource ID from the RecionAttribute associated with the view.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static int GetRegionId(this MvxFragment view)
        {
            var attributes = view
                .GetType()
                .GetCustomAttributes(typeof(RegionAttribute), true);
            if (attributes.Count() == 0)
            {
                throw new InvalidOperationException("The IMvxView has no region attribute.");
            }
            else
            {
                return ((RegionAttribute)attributes.First()).Id;
            }
        }

		public static bool GetHidesNav(this MvxFragment view)
		{
			var attributes = view
				.GetType()
				.GetCustomAttributes(typeof(RegionAttribute), true);

				return ((RegionAttribute)attributes.First()).HidesNav;

		}

		public static int GetBackstackBehaviour(this MvxFragment view)
		{
			var attributes = view
				.GetType()
				.GetCustomAttributes(typeof(RegionAttribute), true);

			return ((RegionAttribute)attributes.First()).BackstackBehaviour;
		}

		public static string GetSimpleMessageText(this MvxFragment view)
		{
			var attributes = view
				.GetType()
				.GetCustomAttributes(typeof(RegionAttribute), true);

			return ((RegionAttribute)attributes.First()).SimpleMessageText;

		}
    }

}