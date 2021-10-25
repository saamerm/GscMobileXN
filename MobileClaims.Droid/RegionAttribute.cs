using System;

namespace MobileClaims.Droid
{
    /// <summary>
    /// Attribute for tagging views with the region in which they should displayed when shown with IMultiRegionPresenter.
    /// </summary>
    public class RegionAttribute : Attribute
    {
        public RegionAttribute(int id)
        {
            Id = id;
			BackstackBehaviour = BackstackTypes.ADD;
        }

		public RegionAttribute(int id, bool hidesNav)
		{
			Id = id;
			HidesNav = hidesNav;
			BackstackBehaviour = BackstackTypes.ADD;
		}

        public RegionAttribute(int id, Type loadWith)
        {
            Id = id;
            LoadWith = loadWith;
			BackstackBehaviour = BackstackTypes.ADD;
        }

		public RegionAttribute(int id, bool hidesNav, int backstackBehaviour)
		{
			Id = id;
			HidesNav = hidesNav;
			BackstackBehaviour = backstackBehaviour;
		}

		public RegionAttribute(int id, bool hidesNav, int backstackBehaviour, string simpleMessageText)
		{
			Id = id;
			HidesNav = hidesNav;
			BackstackBehaviour = backstackBehaviour;
			SimpleMessageText = simpleMessageText;
		}

		public RegionAttribute(int id, Type loadWith, bool noHistory)
		{
			Id = id;
			LoadWith = loadWith;
			NoHistory = noHistory;
			BackstackBehaviour = BackstackTypes.ADD;
		}


        /// <summary>
        /// The Android resource ID of the region.
        /// </summary>
        public int Id { get; private set; }  

        public Type LoadWith
        {
            get;
            set;
        }

		public bool NoHistory 
		{
			get;
			set;
		}

		public int BackstackBehaviour
		{
			get;
			set;
		}

		public string SimpleMessageText
		{
			get;
			set;
		}


		public bool HidesNav 
		{
			get;
			set;
		}
    }

	public class BackstackTypes
	{
		public const int ADD = 0;
		public const int FIRST_ITEM = 1;
		public const int WITHHOLD = 2;

	}
}