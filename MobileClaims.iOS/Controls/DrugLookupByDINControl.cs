using System;
using System.Drawing;

using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MupApps.MvvmCross.Plugins.ControlsNavigation.Touch;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.iOS.Controls
{
    [Register("DrugLookupByDINControl")]
	public class DrugLookupByDINControl : MvxTouchControl
    {
		protected UITableView participantsTable;
		public DrugLookupByDINControl() : base(null,null)
		{
			Initialize();
			this.EmptyControlBehaviour = MupApps.MvvmCross.Plugins.ControlsNavigation.EmptyControlBehaviours.Hide;

		}
        private RectangleF _frame;
        public RectangleF Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                this.View.Frame = _frame;
            }
        }
        void Initialize()
        {
			this.View.BackgroundColor = UIColor.Red;
            UILabel ScreenLabel = new UILabel();
            ScreenLabel.Text = "Hello from Drug Lookup By DIN!";
            ScreenLabel.Frame = new RectangleF(40,40,400,40);
            this.Add(ScreenLabel);

        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			participantsTable = new UITableView(new RectangleF(10, 10, this.View.Frame.Width - 10, this.View.Frame.Height - 10));
			this.View.AddSubview(participantsTable);
            var source = new MvxSimpleTableViewSource(participantsTable, typeof(ParticipantCellTemplate), "ParticipantCellTemplate");
			participantsTable.Source = source;

			var set = this.CreateBindingSet<DrugLookupByDINControl,DrugLookupByDINViewModel>();
			set.Bind(source).To(vm => vm.Participants);
			set.Bind(source).For(s => s.SelectedItem).To(vm => vm.SelectedParticipant);
			set.Apply();
		}
    }
}