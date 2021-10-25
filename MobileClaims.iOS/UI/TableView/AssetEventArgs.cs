using System;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	public class AssetEventArgs : EventArgs
	{
		private readonly ParticipantEligibilityResult _asset;

		public AssetEventArgs(ParticipantEligibilityResult asset)
		{
			_asset = asset;
		}

		public ParticipantEligibilityResult Asset
		{
			get { return _asset; }
		}
	}
}