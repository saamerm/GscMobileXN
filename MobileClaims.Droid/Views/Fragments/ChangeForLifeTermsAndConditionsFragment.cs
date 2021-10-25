using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Util;
using MobileClaims.Core.ViewModels;
using System;
using System.IO;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
	public class ChangeForLifeTermsandConditionsFragment_ : BaseFragment
    {
	//	ChangeForLifeTermsandConditionsFragment changeForLifeTermsandConditionsFragment;
		ChangeForLifeTermsAndConditionsViewModel _model;
		bool dialogShown;
		string currLang;
		WebView webView;
		LinearLayout checkContainer;
		//CheckBox termsCheck;
		//private ILoginService _loginservice;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			// Set the layout
			return this.BindingInflate(Resource.Layout.ChangeForLifeTermsAndConditionsView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			currLang = Locale.Default.Language;
			_model = (ChangeForLifeTermsAndConditionsViewModel)ViewModel;
			try{
				webView = Activity.FindViewById<WebView> (Resource.Id.terms_text_view);
				MonkeyWebViewClient monkeyWebViewClient=new MonkeyWebViewClient();
				monkeyWebViewClient._c4lFragment=this;
				webView.SetWebViewClient(monkeyWebViewClient);

				_model = (ChangeForLifeTermsAndConditionsViewModel)ViewModel;

				checkContainer = Activity.FindViewById<LinearLayout> (Resource.Id.chk_container);

				Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
				Typeface nunitoFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");

				Button termscondition = Activity.FindViewById<Button> (Resource.Id.c4l_Condition_button);
				termscondition.Click += HandleTermsConditonClick;
				termscondition.SetTypeface (leagueFont, TypefaceStyle.Normal);

				Button privacyButton = Activity.FindViewById<Button> (Resource.Id.c4l_privacy_button);
				privacyButton.Click += HandlePrivacyClick;
				privacyButton.SetTypeface (leagueFont, TypefaceStyle.Normal);

				Button legalButton = Activity.FindViewById<Button> (Resource.Id.c4l_legal_button);
				legalButton.Click += HandleLegalClick;
				legalButton.SetTypeface (leagueFont, TypefaceStyle.Normal);

				Button securityButton = Activity.FindViewById<Button> (Resource.Id.c4l_security_button);
				securityButton.Click += HandleSecurityClick;
				securityButton.SetTypeface (leagueFont, TypefaceStyle.Normal);

				//TextView termsTextView = this.Activity.FindViewById<TextView> (Resource.Id.terms_text_view);
				//WebView termsTextView = this.Activity.FindViewById<WebView> (Resource.Id.terms_text_view);
				//termsTextView.MovementMethod = new ScrollingMovementMethod ();
				//termsTextView.Text = Resources.GetString(Resource.String.privacyContent);
				//termsTextView.SetTypeface (nunitoFont, TypefaceStyle.Normal);

				setButtonStates (0);

                ShowTermsAndConditions();
				
			} catch(Exception ex) {
				Console.Write (ex.StackTrace);
			}
		}

        private void ShowTermsAndConditions()
        {
            var content = currLang.Contains("fr") ? "C4LTermsAndConditions_fr.html" : "C4LTermsAndConditions_en.html";

            string termsAndConditions;
            using (var sr = new StreamReader(Application.Context.Assets.Open(content)))
            {
                termsAndConditions = sr.ReadToEnd();
                termsAndConditions = termsAndConditions
                    .Replace("{TEXT_COLOR}", GetHexColor(Resource.Color.brand_color))
                    .Replace("{FONT_SIZE}", Resources.GetString(Resource.String.c4l_terms_padding));
            }
            webView.LoadDataWithBaseURL(null, termsAndConditions, "text/html", "UTF-8", null);
        }

        void HandleTermsConditonClick (object sender, EventArgs e)
        {
            ShowTermsAndConditions();

            setButtonStates (0);
			checkContainer.Visibility = ViewStates.Visible;
//			if (_model.AcceptedTC) {
//				checkContainer.Visibility = Android.Views.ViewStates.Gone;
//			} else {
//				checkContainer.Visibility = Android.Views.ViewStates.Visible;
//			}
		}

        private string GetHexColor(int resourceIdColor)
        {
            var color = Resources.GetColor(resourceIdColor);
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
        
        void HandlePrivacyClick (object sender, EventArgs e)
		{
			if (currLang.Contains("fr")) {
				webView.LoadUrl ("file:///android_asset/Privacy_fr.html");
			}
			else {
				webView.LoadUrl ("file:///android_asset/Privacy_en.html");
			}
			setButtonStates (1);
			checkContainer.Visibility = ViewStates.Gone;
		}

		void HandleLegalClick (object sender, EventArgs e)
		{
			if (currLang.Contains("fr")) {
				webView.LoadUrl ("file:///android_asset/Legal_fr.html");
			}
			else {
				webView.LoadUrl ("file:///android_asset/Legal_en.html");
			}
			setButtonStates (2);
			checkContainer.Visibility = ViewStates.Gone;
		}

		void HandleSecurityClick (object sender, EventArgs e)
		{
			if (currLang.Contains("fr")) {
				webView.LoadUrl ("file:///android_asset/Security_fr.html");
			}
			else {
				webView.LoadUrl ("file:///android_asset/Security_en.html");
			}
			setButtonStates (3);
			checkContainer.Visibility = ViewStates.Gone;

		}
		class MonkeyWebViewClient : WebViewClient {
			public ChangeForLifeTermsandConditionsFragment_ _c4lFragment;
			public override bool ShouldOverrideUrlLoading(WebView view, string url)
			{
				 if(url.Contains("Privacy")) {
					
					if (_c4lFragment.currLang.Contains("fr")) {
						view.LoadUrl ("file:///android_asset/Privacy_fr.html");
					}
					else {
						view.LoadUrl ("file:///android_asset/Privacy_en.html");
					}
					_c4lFragment.setButtonStates (1);
					_c4lFragment.checkContainer.Visibility = ViewStates.Gone;
						return true;
				}
			    if(url.Contains("Legal")) {
			        if (_c4lFragment.currLang.Contains("fr")) {
			            view.LoadUrl ("file:///android_asset/Legal_fr.html");
			        }
			        else {
			            view.LoadUrl ("file:///android_asset/Legal_en.html");
			        }
			        _c4lFragment.setButtonStates (2);
			        _c4lFragment.checkContainer.Visibility = ViewStates.Gone;
			        return true;
			    }
			    if(url.Contains("Security")) {
			        if (_c4lFragment.currLang.Contains("fr")) {
			            view.LoadUrl ("file:///android_asset/Security_fr.html");
			        }
			        else {
			            view.LoadUrl ("file:///android_asset/Security_en.html");
			        }
			        _c4lFragment.setButtonStates (3);
			        _c4lFragment.checkContainer.Visibility = ViewStates.Gone;
			        return true;
			    }

			    return false;
			}
		}
		void setButtonStates(int index)
		{
			Button termscondition = Activity.FindViewById<Button> (Resource.Id.c4l_Condition_button);
			Button privacyButton = Activity.FindViewById<Button> (Resource.Id.c4l_privacy_button);
			Button legalButton = Activity.FindViewById<Button> (Resource.Id.c4l_legal_button);
			Button securityButton = Activity.FindViewById<Button> (Resource.Id.c4l_security_button);

			termscondition.Enabled = index != 0;
			privacyButton.Enabled = index != 1;
			legalButton.Enabled = index != 2;
			securityButton.Enabled = index != 3;
		}
	}
}