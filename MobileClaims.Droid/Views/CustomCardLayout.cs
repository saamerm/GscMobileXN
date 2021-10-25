using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace MobileClaims.Droid
{
	public class CustomCardLayout : LinearLayout, LinearLayout.IOnTouchListener
	{
		private static string TAG = "MyTag";

		private static float mScaleFactor = 1f;
		private ScaleGestureDetector mScaleDetector;
		private float mPosX;
		private float mPosY;
		private static float mFocusX;
		private static float mFocusY;
		private ScaleListener sListener;

		private float mLastTouchX;
		private float mLastTouchY;
		private static int INVALID_POINTER_ID = -1;
		private int mActivePointerId = INVALID_POINTER_ID;

		private bool mHandlingGesture = true;

		public CustomCardLayout (Context context) :
			base (context)
		{

			Initialize ();
		}

		public CustomCardLayout (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public CustomCardLayout (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			//mScaleDetector = new ScaleGestureDetector(Context,new ScaleListener());

//			this.SetOnTouchListener(this);

			sListener = new ScaleListener();
			mScaleDetector = new ScaleGestureDetector(Context, sListener);
//			mScaleFactor = 1000F / (float)Math.Min(Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
		}

		public bool OnTouch (View v, MotionEvent ev)
		{
			// Let the ScaleGestureDetector inspect all events.
			mHandlingGesture = mScaleDetector.OnTouchEvent(ev);

			switch (ev.Action & MotionEventActions.Mask) {
			case MotionEventActions.Down: {
					float x = ev.GetX();
					float y = ev.GetY();

					mLastTouchX = x;
					mLastTouchY = y;
					mActivePointerId = ev.GetPointerId(0);
					break;
				}

			case MotionEventActions.Move: {
					int pointerIndex = ev.FindPointerIndex(mActivePointerId);
					float x = ev.GetX(pointerIndex);
					float y = ev.GetY(pointerIndex);

					// Only move if the ScaleGestureDetector isn't processing a gesture.
					if (!mScaleDetector.IsInProgress) {
						float dx = x - mLastTouchX;
						float dy = y - mLastTouchY;

						mPosX += dx;
						mPosY += dy;
						RequestLayout ();
					}

					mLastTouchX = x;
					mLastTouchY = y;

					break;
				}

			case MotionEventActions.Up: {
					mActivePointerId = INVALID_POINTER_ID;
					break;
				}

			case MotionEventActions.Cancel: {
					mActivePointerId = INVALID_POINTER_ID;
					break;
				}

			case MotionEventActions.PointerUp: {
					int pointerIndex = (int)(ev.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
					int pointerId = ev.GetPointerId(pointerIndex);
					if (pointerId == mActivePointerId) {
						// This was our active pointer going up. Choose a new
						// active pointer and adjust accordingly.
						int newPointerIndex = pointerIndex == 0 ? 1 : 0;
						mLastTouchX = ev.GetX(newPointerIndex);
						mLastTouchY = ev.GetY(newPointerIndex);
						mActivePointerId = ev.GetPointerId(newPointerIndex);
					}
					break;
				}
			}

			return true;
		}
			
		public override bool OnInterceptTouchEvent(MotionEvent ev) {
			return true;
		}

		protected override void DispatchDraw(Canvas canvas) {
			canvas.Save();
			canvas.Translate(mPosX, mPosY);
			if (mScaleDetector.IsInProgress) {
				canvas.Scale (mScaleFactor, mScaleFactor, mFocusX, mFocusY);
			} else {
				canvas.Scale (mScaleFactor, mScaleFactor,mLastTouchX,mLastTouchY);
			}
			base.DispatchDraw(canvas);
			canvas.Restore();
		}

		private class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener {
			public override bool OnScale(ScaleGestureDetector detector) {
				mScaleFactor *= detector.ScaleFactor;

				// Don't let the object get too small or too large.
				mScaleFactor = Math.Max(0.1f, Math.Min(mScaleFactor, 5.0f));

				mFocusX = detector.FocusX;
				mFocusY = detector.FocusY;
//				RequestLayout();

				return true;
			}
		}
	}
}

