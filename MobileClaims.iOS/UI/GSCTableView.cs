using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("GSCTableView")]
    [DesignTimeVisible(true)]
    public class GSCTableView : UITableView
    {
        public GSCTableView(IntPtr handler) : base(handler)
        { }

        public GSCTableView()
        {
            Initialize();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            Initialize();
        }

        private void Initialize()
        {
            RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            TableHeaderView = new UIView();
            SeparatorColor = UIColor.Clear;
            ShowsVerticalScrollIndicator = false;
            ScrollEnabled = false;
        }
    }
}