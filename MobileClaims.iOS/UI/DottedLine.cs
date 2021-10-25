using System;
using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class DottedLine : UIView
	{
		static float kDashedBorderWidth     = (3.0f);
		static float kDashedPhase           = (1.0f);
		static float[] kDashedLinesLength   = {4.0f, 2.0f};
		static float kDashedCount            = (2.0f);

		public DottedLine ()
		{

			//this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			this.BackgroundColor = Colors.Clear;
		}

		public override void DrawRect (CGRect area, UIViewPrintFormatter formatter)
		{
			base.DrawRect ((CGRect)area, (UIViewPrintFormatter)formatter);
		}

		public override void Draw (CGRect rect)
		{
			base.Draw ((CGRect)rect);

			using(CGContext g = UIGraphics.GetCurrentContext ()){

				g.SetLineWidth ((nfloat)kDashedBorderWidth);
				g.SetStrokeColor (Colors.HIGHLIGHT_COLOR.CGColor);
				nfloat[] test = new nfloat[2] { 2.0f, 2.0f };


				g.SetLineDash ((nfloat)kDashedPhase,test);
				g.AddRect ((CGRect)rect);
				g.StrokeRect ((CGRect)rect);
			}

		}
	}
}

