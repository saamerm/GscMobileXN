using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using DK.Ostebaronen.Droid.ViewPagerIndicator;
using MobileClaims.Core;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;
using System.Threading.Tasks;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class CardViewFragment_ : BaseFragment
    {
        private CardViewModel _model;
        private bool cardFlip;
        private ViewPager pager;
        private DisplayMetrics metrics;

        private static LinearLayout mainLayout;
        private static LinearLayout mainLayoutTouchHandle;
        static ScaleGestureDetector scaleGestureDetector;

        private static PointF touchPoint;
        private static PointF pan = new PointF();

        private static bool isScaling;
        private static bool endScalingNextUp;

        private static float MIN_ZOOM = 1f;
        private static float MAX_ZOOM = 3f;
        private static float scale = 1;
        private static float lastScaleFactor = 0f;

        private static float SCALE_SPEED = 0.04f;


        private static float startX = 0f;
        private static float startY = 0f;

        private static float translateX = 0f;
        private static float translateY = 0f;

        private static readonly int InvalidPointerId = -1;
        private static int mActivePointerId = InvalidPointerId;


        static GestureDetector gestureDetector;
        static PointF boundaryPoint = new PointF(0.0f, 0.0f);
        static float displayDensityValue = 1.0f;
        string _planMemberId;
        Context _appContext = Android.App.Application.Context.ApplicationContext;
        byte[] _result;
        string _directoryname;
        int _retryNumber = 0;
        private MvxSubscriptionToken _permissionsStorageGrantedToken;
        private IParticipantService _participantservice;
        private IDataService _dataservice;

        class CardPagerAdapter : PagerAdapter
        {
            public override Java.Lang.Object InstantiateItem(View collection, int position)
            {

                int resId = 0;
                switch (position)
                {
                    case 0:
                        resId = Resource.Id.cardContainer1Handle;
                        break;
                    case 1:
                        resId = Resource.Id.cardContainer2Handle;
                        break;
                }
                return collection.FindViewById(resId);
            }

            public override int Count => 2;

            public override bool IsViewFromObject(View arg0, Java.Lang.Object arg1)
            {
                return arg0 == ((View)arg1);
            }
        }

        #region Android Lifecycle overrides
        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.CardViewFragment, null);
            scaleGestureDetector = new Android.Views.ScaleGestureDetector(this.Activity, new ExampleScaleGestureListener());

            gestureDetector = new GestureDetector(this.Activity, new MyDoubleTapListener());

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            cardFlip = false;

            //rotate card container if in portrait
            metrics = this.Activity.Resources.DisplayMetrics;
            displayDensityValue = metrics.Density;
            Console.WriteLine("density -- {0}", metrics.Density);
            Activity.WindowManager.DefaultDisplay.GetMetrics(metrics);

            Button androidButton = this.Activity.FindViewById<Button>(Resource.Id.androidButton);
            androidButton.Text = Resources.GetString(Resource.String.walletButton);
            androidButton.SetBackgroundColor(Color.Black);
            androidButton.SetTextColor(Color.White);
            
            ViewGroup.MarginLayoutParams marginLayoutParams = (ViewGroup.MarginLayoutParams)androidButton.LayoutParameters;
            marginLayoutParams.BottomMargin = 5;

            androidButton.LayoutParameters = marginLayoutParams;

            androidButton.Click += HandleClickAsync;

            _model = (CardViewModel)ViewModel;

            if (_model.FromLoginScreen)
            {
                androidButton.Visibility = ViewStates.Gone;
            }

            // Set the ViewPager adapter
            CardPagerAdapter adapter = new CardPagerAdapter();
            pager = this.Activity.FindViewById<ViewPager>(Resource.Id.idcardpager);
            pager.Adapter = adapter;

            mainLayoutTouchHandle = (LinearLayout)this.Activity.FindViewById(Resource.Id.cardContainer1Handle);
            mainLayout = (LinearLayout)this.Activity.FindViewById(Resource.Id.cardContainer1);

            mainLayoutTouchHandle.SetOnTouchListener(new MyOnTouchListener());

            var titleIndicator = this.Activity.FindViewById<CirclePageIndicator>(Resource.Id.indicator);
            titleIndicator.PageColor = Resources.GetColor(Resource.Color.highlight_color);
            titleIndicator.SetViewPager(pager);
            titleIndicator.SetOnPageChangeListener(new MyPageChangeListener(this.Activity));
            reset();

            // Using the call the API on a different thread so that it doesn't hold up the UI thread
            Task.Run(async () => await GetPassInformation());
        }
        #endregion

        #region ID Card Permissions and Retrieval
        /// <summary>
        /// API Call to Get the information needed to Add the ID card to wallet
        /// </summary>
        /// <returns></returns>
        private async Task GetPassInformation()
        {
            _planMemberId = Mvx.IoCProvider.Resolve<IParticipantService>().PlanMember.PlanMemberID.ToString();

            int stringId = _appContext.ApplicationInfo.LabelRes;

            // POST request
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("description", BrandResource.cardDescription);
            parameters.Add("logoText", _appContext.GetString(stringId));

            ApiClient<HttpResponseMessage> service = new ApiClient<HttpResponseMessage>(new Uri(GSCHelper.GSC_SERVICE_BASE_URL), HttpMethod.Post, string.Format("/" + GSCHelper.GSC_SERVICE_BASE_URL_SUB + "/api/planmember/{0}/pkpass", _planMemberId), apiBody: parameters, useDefaultHeaders: true);
            // Somehow removing these two lines causes the api to not return 500 error for QA 17 ARTA, even though it doesnt look like it's passed anywhere
            // And this is also the behavior in iOS, so I kept it as is.
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/vnd.apple.pkpass");

            var response = await service.ExecuteRequest();

            // file attachment as byte[]
            _result = await response.Content.ReadAsByteArrayAsync();
            var path = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath + "/IDCard";
            string trimmedPlanMemberID = _planMemberId;
            if (trimmedPlanMemberID.IndexOf('-') > -1)
            {
                trimmedPlanMemberID = trimmedPlanMemberID.Substring(0, trimmedPlanMemberID.IndexOf('-'));
            }
            _directoryname = System.IO.Path.Combine(path, string.Format("{0}.pkpass", trimmedPlanMemberID));
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(_directoryname, _result);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, "Unable to store pass. " + ex.Message, ToastLength.Long).Show();
            }
        }

        private async void HandleClickAsync(object sender, EventArgs e)
        {
            await CardPermissionsAndRetrieval();
        }

        public async Task CardPermissionsAndRetrieval()
        {
            Intent startIntent = new Intent();

            if (Build.VERSION.SdkInt < BuildVersionCodes.M
                || Activity.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted)
            {
                // getting POST response
                await RetrieveWalletCard(startIntent);
            }
            else
            {
                var messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
                _permissionsStorageGrantedToken = messenger.Subscribe<PermissionsStorageGrantedMessage>(async (message) =>
                {
                    messenger.Unsubscribe<PermissionsStorageGrantedMessage>(_permissionsStorageGrantedToken);
                    await RetrieveWalletCard(startIntent);
                });
                Activity.RequestPermissions(new[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 1204);
            }
        }

        private async Task RetrieveWalletCard(Intent startIntent)
        {          
            try
            {
                await InvokeAddToWalletIntent(startIntent, _directoryname);
            }
            catch (Exception ex)
            {
                Toast.MakeText(Activity, "Unable to retrieve pass. " + ex.Message, ToastLength.Long).Show();
            }
        }

        private async Task InvokeAddToWalletIntent(Intent startIntent, string directoryname)
        {
            try
            {
                startIntent.SetDataAndType(Android.Net.Uri.Parse(directoryname), "application/vnd.apple.pkpass");
                startIntent.AddFlags(ActivityFlags.NewTask);
                StartActivityForResult(startIntent, (int)ActivityRequestCodes.WalletAppCode);
            }
            catch (Exception exception)
            {
                Console.WriteLine("EXCEPTION ERROR : {0}", exception.ToString());
                if (exception.Message == "uriString" && _retryNumber < 10)
                {
                    _retryNumber++;
                    await GetPassInformation();
                    await CardPermissionsAndRetrieval();
                }
                else
                    Toast.MakeText(Activity, "Unable to retrieve pass. " + exception.Message, ToastLength.Long).Show();
            }
        }

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == (int)ActivityRequestCodes.WalletAppCode)
            {
                Toast.MakeText(Activity, Resources.GetString(Resource.String.thirdPartyAppMessage), ToastLength.Long).Show();
            }
        }
        #endregion

        private static void setPivot(float focusX, float focusY)
        {
            mainLayout.PivotX = focusX;
            mainLayout.PivotY = focusY;
            //			mainLayout.PivotX(focusX);
            //			mainLayout.PivotY(focusY);
        }

        private static void scaleView()
        {
            if (scale >= MIN_ZOOM && scale < MAX_ZOOM)
            {
                mainLayout.ScaleX = scale;
                mainLayout.ScaleY = scale;
                mainLayout.Invalidate();
            }
            //			mainLayout.ScaleX(scale);
            //			mainLayout.ScaleY(scale);
        }

        private static void panView()
        {
            //			Y - 480
            //			X - 320
            mainLayout.TranslationX = pan.X;
            mainLayout.TranslationY = pan.Y;
            mainLayout.Invalidate();
            //			mainLayout.TranslationX(pan.x);
            //			mainLayout.TranslationY(pan.y);

        }
        private static void reset()
        {
            scale = 1;
            translateX = 0;
            translateY = 0;
            scaleView();
            pan = new PointF(translateX, translateY);
            panView();
            //setPivot(0, 0);
            isScaling = false;
        }

        private bool IsAppInstalled(string packageName)
        {
            var appContext = Android.App.Application.Context.ApplicationContext;
            try
            {
                var packageInfo = appContext.PackageManager.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public class MyPageChangeListener : Java.Lang.Object, ViewPager.IOnPageChangeListener
        {
            Context _context;

            public MyPageChangeListener(Context context)
            {
                _context = context;

            }


            #region IOnPageChangeListener implementation
            public void OnPageScrollStateChanged(int p0)
            {
            }

            public void OnPageScrolled(int p0, float p1, int p2)
            {
            }

            public void OnPageSelected(int position)
            {
                reset();
                if (position == 0)
                {
                    mainLayoutTouchHandle = (LinearLayout)((Activity)_context).FindViewById(Resource.Id.cardContainer1Handle);
                    mainLayout = (LinearLayout)((Activity)_context).FindViewById(Resource.Id.cardContainer1);
                    if (((Activity)_context).Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
                    {
                        if (!((Activity)_context).Resources.GetBoolean(Resource.Boolean.isTablet))
                        {
                            boundaryPoint.X = displayDensityValue * 240;
                            boundaryPoint.Y = displayDensityValue * 250;// layout.Top + lp.Height;
                        }
                        else
                        {
                            boundaryPoint.X = displayDensityValue * 480;
                            boundaryPoint.Y = displayDensityValue * 320;// layout.Top + lp.Height;
                        }
                    }
                    else
                    {
                        if (!((Activity)_context).Resources.GetBoolean(Resource.Boolean.isTablet))
                        {
                            boundaryPoint.X = displayDensityValue * 210;
                            boundaryPoint.Y = displayDensityValue * 240;// layout.Top + lp.Height;
                        }
                        else
                        {
                            boundaryPoint.X = displayDensityValue * 480;
                            boundaryPoint.Y = displayDensityValue * 320;// layout.Top + lp.Height;
                        }
                    }
                    // cardFrontContainer
                }
                else if (position == 1)
                {
                    mainLayoutTouchHandle = (LinearLayout)((Activity)_context).FindViewById(Resource.Id.cardContainer2Handle);
                    mainLayout = (LinearLayout)((Activity)_context).FindViewById(Resource.Id.cardContainer2);
                    if (((Activity)_context).Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
                    {
                        if (!((Activity)_context).Resources.GetBoolean(Resource.Boolean.isTablet))
                        {
                            boundaryPoint.X = displayDensityValue * 240;
                            boundaryPoint.Y = displayDensityValue * 250;// layout.Top + lp.Height;
                        }
                        else
                        {
                            boundaryPoint.X = displayDensityValue * 480;
                            boundaryPoint.Y = displayDensityValue * 320;// layout.Top + lp.Height;
                        }
                    }
                    else
                    {
                        if (!((Activity)_context).Resources.GetBoolean(Resource.Boolean.isTablet))
                        {
                            boundaryPoint.X = displayDensityValue * 210;
                            boundaryPoint.Y = displayDensityValue * 240;// layout.Top + lp.Height;
                        }
                        else
                        {
                            boundaryPoint.X = displayDensityValue * 480;
                            boundaryPoint.Y = displayDensityValue * 320;// layout.Top + lp.Height;
                        }
                    }
                    // cardBackContainer
                }
                mainLayout.Invalidate();
                //				if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait) {
                //					UpdatePoints (mainLayoutTouchHandle);
                //				}

                mainLayoutTouchHandle.SetOnTouchListener(new MyOnTouchListener());
            }
            #endregion
        }


        public class MyOnTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            public bool OnTouch(View v, MotionEvent Event)
            {
                gestureDetector.OnTouchEvent(Event);
                /*if (isScaling) {
					if (endScalingNextUp && Event.Action == MotionEventActions.Cancel) {
						//  && Event.Action == MotionEventActions.Up
						endScalingNextUp = false;
						isScaling = false;
					}
				}*/

                int pointerIndex;

                switch (Event.ActionMasked)
                {
                    case MotionEventActions.Down:
                        if (scale > MIN_ZOOM && !scaleGestureDetector.IsInProgress)
                        {
                            startX = Event.GetX();
                            startY = Event.GetY();
                            /*if (mainLayout.GetX () > boundaryPoint.X) {
								mainLayout.SetX (boundaryPoint.X - 10);
							} else if (mainLayout.GetX () < -boundaryPoint.X) {
								mainLayout.SetX (-boundaryPoint.X + 10);
							}
							if (mainLayout.GetY () > boundaryPoint.Y) {
								mainLayout.SetY (boundaryPoint.Y - 10);
							} else if (mainLayout.GetY () < -boundaryPoint.Y) {
								mainLayout.SetY (-boundaryPoint.Y + 10);
							}*/
                            mainLayout.Invalidate();
                            //							Console.WriteLine ("mainLayout X -- {0}", mainLayout.GetX());
                            //							Console.WriteLine ("mainLayout Y -- {0}", mainLayout.GetY());
                        }
                        mActivePointerId = Event.GetPointerId(0);
                        break;
                    case MotionEventActions.Move:
                        float x = 0, y = 0;
                        for (int i = 0; i < Event.PointerCount; ++i)
                        {
                            int id = Event.GetPointerId(i);
                            pointerIndex = Event.FindPointerIndex(id);
                            try
                            {
                                // Using what's mentioned here https://stackoverflow.com/a/32121447
                                x = Event.GetX(pointerIndex);
                                y = Event.GetY(pointerIndex);
                            }
                            catch (Exception ex)
                            {
                                return true;
                            }
                        }
                        
                        if (!scaleGestureDetector.IsInProgress)
                        {
                            float dx = x - startX;
                            float dy = y - startY;


                            if (scale > MIN_ZOOM)
                            {
                                bool shouldInvalidate = false;
                                bool shouldSwipe = false;
                                if ((mainLayout.GetX() >= 0 && mainLayout.GetX() < boundaryPoint.X) || (mainLayout.GetX() < 0 && mainLayout.GetX() > -boundaryPoint.X))
                                {
                                    translateX += dx;
                                    shouldInvalidate = true;
                                    shouldSwipe = false;
                                }
                                else if ((mainLayout.GetX() >= boundaryPoint.X && dx < 0) || (mainLayout.GetX() <= -boundaryPoint.X && dx > 0))
                                {
                                    translateX += dx;
                                    shouldInvalidate = true;
                                    shouldSwipe = false;
                                }
                                else
                                {
                                    shouldSwipe = true;
                                }
                                if ((mainLayout.GetY() >= 0 && mainLayout.GetY() < boundaryPoint.Y) || (mainLayout.GetY() < 0 && mainLayout.GetY() > -boundaryPoint.Y))
                                {
                                    translateY += dy;
                                    shouldInvalidate = true;
                                }
                                else if ((mainLayout.GetY() >= boundaryPoint.Y && dy < 0) || (mainLayout.GetY() <= -boundaryPoint.Y && dy > 0))
                                {
                                    translateY += dy;
                                    shouldInvalidate = true;
                                }
                                if (shouldInvalidate)
                                {
                                    v.Parent.RequestDisallowInterceptTouchEvent(true);
                                    pan = new PointF(translateX, translateY);
                                    panView();
                                }
                                if (shouldSwipe)
                                {
                                    v.Parent.RequestDisallowInterceptTouchEvent(false);
                                }
                                //v.Invalidate ();
                            }
                            else if (scale == MIN_ZOOM)
                            {
                                translateX = 0;
                                translateY = 0;
                                pan = new PointF(translateX, translateY);
                                panView();
                                //v.Invalidate ();
                            }

                        }
                        startX = x;
                        startY = y;

                        break;

                    case MotionEventActions.PointerUp:
                        int pointerIndex1 = (int)(Event.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
                        int pointerId = Event.GetPointerId(pointerIndex1);
                        if (pointerId == mActivePointerId)
                        {
                            // This was our active pointer going up. Choose a new
                            // active pointer and adjust accordingly.
                            int newPointerIndex = pointerIndex1 == 0 ? 1 : 0;
                            startX = Event.GetX(newPointerIndex);
                            startY = Event.GetY(newPointerIndex);
                            mActivePointerId = Event.GetPointerId(newPointerIndex);
                        }
                        break;
                    case MotionEventActions.Up:
                    case MotionEventActions.Cancel:
                        mActivePointerId = InvalidPointerId;
                        break;
                }
                scaleGestureDetector.OnTouchEvent(Event);


                //				if (scale > MIN_ZOOM) {
                //					v.Parent.RequestDisallowInterceptTouchEvent (true);
                //				} else {
                //					v.Parent.RequestDisallowInterceptTouchEvent (false);
                //				}
                return true;
            }
        }

        public class MyDoubleTapListener : GestureDetector.SimpleOnGestureListener
        {
            public override bool OnDown(MotionEvent e)
            {
                return true;
            }
            public override bool OnDoubleTap(MotionEvent e)
            {
                //Your code here
                if (scale > MIN_ZOOM)
                {
                    scale = 1;
                    translateX = 0;
                    translateY = 0;
                    mainLayout.ScaleX = scale;
                    mainLayout.ScaleY = scale;
                    pan = new PointF(translateX, translateY);
                    panView();
                }
                return true;
            }
        }
        class ExampleScaleGestureListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            float spanValue = 0.0f;
            public override void OnScaleEnd(ScaleGestureDetector GestureDetector)
            {
                endScalingNextUp = true;
            }

            public ExampleScaleGestureListener() : base()
            {

            }

            public override bool OnScaleBegin(ScaleGestureDetector detector)
            {
                float focusX = detector.FocusX;
                float focusY = detector.FocusY;
                //				setPivot(focusX, focusY);
                isScaling = true;
                endScalingNextUp = false;
                return true;
            }

            public override bool OnScale(ScaleGestureDetector detector)
            {
                isScaling = true;
                endScalingNextUp = false;

                float scaleFactor = detector.ScaleFactor;

                //				Console.WriteLine ("getPreviousSpan value -- {0}", detector.PreviousSpan.ToString ());
                if (scaleFactor > 0.1 && (scaleFactor - lastScaleFactor > 0.0025 || lastScaleFactor - scaleFactor > 0.0025))
                {
                    if (lastScaleFactor == 0 || (Math.Sign(scaleFactor) == Math.Sign(lastScaleFactor)))
                    {
                        scale *= scaleFactor;
                        scale = Math.Max(MIN_ZOOM, Math.Min(scale, MAX_ZOOM));
                        lastScaleFactor = scaleFactor;
                    }
                    else
                    {
                        lastScaleFactor = 0;
                    }
                }
                else
                {
                    //					Console.WriteLine ("scale failed -- {0}", scaleFactor.ToString ());
                }

                if (spanValue - detector.PreviousSpan > 5.0f || detector.PreviousSpan - spanValue > 5.0f)
                {
                    if (scale > MIN_ZOOM && scale < MAX_ZOOM)
                    {
                        scaleView();
                    }
                }
                spanValue = detector.PreviousSpan;

                return true;
            }

        }
    }
}