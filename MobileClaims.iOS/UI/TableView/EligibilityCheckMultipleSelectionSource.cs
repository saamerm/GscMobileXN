using UIKit;
using Foundation;
using System.Collections.Generic;
using System;
using MobileClaims.iOS;
using MobileClaims.Core.Entities;
using MvvmCross.Platforms.Ios.Binding.Views;

public class EligibilityCheckMultipleSelectionSource : MvxTableViewSource
{
	public delegate void EventHandler(object sender, AssetEventArgs e);
	public event EventHandler<AssetEventArgs> SourceAssetsRemoved;
	public event EventHandler<AssetEventArgs> SourceAssetAdded;
	ParticipantEligibilityResult _asset;
	List<ParticipantEligibilityResult> _assets;
	public Type cellType;
	public String cellName;

	#region Constructors
	public EligibilityCheckMultipleSelectionSource(List<ParticipantEligibilityResult> assets, UITableView tableView, String cellName, Type cellType) : base(tableView)
	{
		this.cellType = cellType;
		this.cellName = cellName;
		_assets = assets;
		tableView.RegisterClassForCellReuse(cellType, new NSString(cellName));
	}
	#endregion

	protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
	{
		return tableView.DequeueReusableCell(cellName);
	}

	public override void RowDeselected (UITableView tableView, NSIndexPath indexPath)
	{
		ParticipantEligibilityResult contentAsset = _assets [indexPath.Row];
		if (this.SourceAssetsRemoved != null) {
			this.SourceAssetsRemoved (this, new AssetEventArgs (contentAsset));
		}


	}

	public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
	{
		ParticipantEligibilityResult contentAsset = _assets [indexPath.Row];
		if (this.SourceAssetAdded != null) {
			this.SourceAssetAdded (this, new AssetEventArgs (contentAsset));
		}
		base.RowSelected (tableView, indexPath);
	}
}