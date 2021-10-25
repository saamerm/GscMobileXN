using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;

using Java.Util;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using Uri = Android.Net.Uri;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class TermsAndConditionsFragment_ : BaseFragment
    {
        private TermsAndConditionsViewModel _model;
        private bool dialogShown;
        private string currLang;
        private WebView webView;
        private Button termscondition;
        private Button privacyButton;
        private Button legalButton;
        private Button securityButton;
        private CheckBox termsCheck;
        private string _termsandconditions;

        private ILoginService _loginservice;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            // Set the layout
            return this.BindingInflate(Resource.Layout.TermsAndConditionsView, null);
        }

        private void HandleLoginConnectionIssues()
        {
            termscondition.Visibility = ViewStates.Gone;
            privacyButton.Visibility = ViewStates.Gone;
            legalButton.Visibility = ViewStates.Gone;
            securityButton.Visibility = ViewStates.Gone;
            webView.LoadData(string.Format(Resources.GetString(Resource.String.loginErrorNetworkProblem)), "text/html; charset=utf-8", "UTF-8");
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            currLang = Locale.Default.Language;
            _model = (TermsAndConditionsViewModel)ViewModel;
            try
            {
                webView = Activity.FindViewById<WebView>(Resource.Id.terms_text_view);
                webView.SetWebViewClient(new MonkeyWebViewClient());
                
                termsCheck = Activity.FindViewById<CheckBox>(Resource.Id.terms_chk);
                var termsCheckLayout = Activity.FindViewById<LinearLayout>(Resource.Id.terms_chk_layout);

                Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
                Typeface nunitoFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");

                termscondition = Activity.FindViewById<Button>(Resource.Id.terms_Condition_button);
                termscondition.Click += HandleTermsConditonClick;
                termscondition.SetTypeface(leagueFont, TypefaceStyle.Normal);

                privacyButton = Activity.FindViewById<Button>(Resource.Id.terms_privacy_button);
                privacyButton.Click += HandlePrivacyClick;
                privacyButton.SetTypeface(leagueFont, TypefaceStyle.Normal);

                legalButton = Activity.FindViewById<Button>(Resource.Id.terms_legal_button);
                legalButton.Click += HandleLegalClick;
                legalButton.SetTypeface(leagueFont, TypefaceStyle.Normal);

                securityButton = Activity.FindViewById<Button>(Resource.Id.terms_security_button);
                securityButton.Click += HandleSecurityClick;
                securityButton.SetTypeface(leagueFont, TypefaceStyle.Normal);

                SetButtonStates(0);

                _loginservice = Mvx.IoCProvider.Resolve<ILoginService>();

                ProgressDialog progressDialog = null;
                _model.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "PhoneBusy" || e.PropertyName == "ClaimBusy")
                    {
                        if (_model.ClaimError || _model.PhoneError)
                        {
                            HandleLoginConnectionIssues();
                            progressDialog.Dismiss();
                        }
                        if (!_model.PhoneBusy && !_model.ClaimBusy)
                        {
                            var file = currLang.Contains("fr") ? "TermsAndConditions_fr.html" : "TermsAndConditions_en.html";
                            UpdateTextAlteration(file, !_model.NoPhoneAlteration, !_model.NoClaimAlteration, false);

                            progressDialog.Dismiss();
                        }
                    }
                    if (e.PropertyName == "AcceptedTC")
                    {
                        if (_model.AcceptedTC)
                        {
                            _model.AcceptTermsAndConditionsDroid.Execute(null);
                        }
                    }
                };
                if (_model.AcceptedTC)
                {
                    if (_loginservice.IsLoggedIn)
                    {
                        termsCheckLayout.Visibility = ViewStates.Gone;
                        if (_model.ClaimError || _model.PhoneError)
                        {
                            HandleLoginConnectionIssues();
                            progressDialog.Dismiss();
                        }
                        if (!_model.PhoneBusy && !_model.ClaimBusy)
                        {
                            var file = currLang.Contains("fr") ? "TermsAndConditions_fr.html" : "TermsAndConditions_en.html";
                            UpdateTextAlteration(file, !_model.NoPhoneAlteration, !_model.NoClaimAlteration, false);

                            progressDialog.Dismiss();
                        }
                        else
                        {
                            Activity.RunOnUiThread(() =>
                            {
                                progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                            });
                        }
                    }
                    else
                    {
                        termsCheck.Checked = false;
                    }
                }
                else
                {
                    var file = currLang.Contains("fr") ? "TermsAndConditions_fr.html" : "TermsAndConditions_en.html";
                    UpdateTextAlteration(file, !_model.NoPhoneAlteration, !_model.NoClaimAlteration, !_model.AcceptedTC);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        private void UpdateTextAlteration(string filename, bool updatePhone, bool updateClaim, bool onlyPhone)
        {
            using (var sr = new StreamReader(Application.Context.Assets.Open(filename)))
            {
                string replacement = "";
                _termsandconditions = sr.ReadToEnd();
                _termsandconditions = _termsandconditions.Replace("{GENERAL_TEXT_COLOR}", GetHexColor(Resource.Color.branded_grey))
                                .Replace("{GENERAL_FONT_SIZE}", Resources.GetString(Resource.String.terms_font_size))
                                .Replace("{GENERAL_PADDING}", Resources.GetString(Resource.String.terms_padding))
                                .Replace("{H3_TEXT_COLOR}", GetHexColor(Resource.Color.highlight_color));
                if (!onlyPhone)
                {
                    if (updateClaim && _model.ClaimAgreement != null)
                    {
                        int begin = _termsandconditions.IndexOf("<claimAgreement>");
                        int end = _termsandconditions.LastIndexOf("</claimAgreement>");
                        _termsandconditions = _termsandconditions.Remove(begin, end - begin + 17);
                        foreach (TextAlteration t in _model.ClaimAgreement)
                        {
                            replacement = replacement + "<li>" + t.Text + "</li>";
                        }
                        _termsandconditions = _termsandconditions.Insert(begin, replacement);
                    }
                    else
                    {
                        try
                        {
                            int begin = _termsandconditions.IndexOf("<claimAgreement>");
                            int end = _termsandconditions.LastIndexOf("</claimAgreement>");
                            _termsandconditions = _termsandconditions.Remove(begin, end - begin + 17);
                            replacement = "<li>" + _model.TermsAndConditionsContent1
                                                 + "<br/><br/>" + _model.TermsAndConditionsContent2
                                                 + "<br/><br/>" + _model.TermsAndConditionsContent3
                                                 + "<br/><br/>" + _model.TermsAndConditionsContent4
                                                 + "<br/><br/>" + _model.TermsAndConditionsContent5
                                                 + "<br/><br/>" + _model.TermsAndConditionsContent6
                                                 + "<br/><br/>" + _model.TermsAndConditionsContent7 + "</li>";
                            _termsandconditions = _termsandconditions.Insert(begin, replacement);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }

                BuildAndAddPhoneNumberString(updatePhone, replacement);

                webView.LoadDataWithBaseURL(null, _termsandconditions, "text/html", "UTF-8", null);
            }
        }

        private void BuildAndAddPhoneNumberString(bool updatePhone, string replacement)
        {
            if (!updatePhone)
            {
                return;
            }

            int begin = _termsandconditions.IndexOf("<phoneNumber>");
            int end = _termsandconditions.LastIndexOf("</phoneNumber>");

            // <phoneNumber> is missig therefore we are getting -1 for begin and end.
            if (begin != -1 && end != -1)
            {
                _termsandconditions = _termsandconditions.Remove(begin, end - begin + 14);

                string phoneNumber = _model.TermsAndConditionsContent_Phone;
                if (updatePhone && _model.PhoneNumber != null)
                {
                    phoneNumber = _model.PhoneNumber.Text;
                }

                replacement = "<a href=\"tel:" + phoneNumber + "\" style='color:" + GetHexColor(Resource.Color.highlight_color) + ";\'>" + phoneNumber + "</a>";
                _termsandconditions = _termsandconditions.Insert(begin, replacement);
            }
        }

        private string GetHexColor(int resourceIdColor)
        {
            var color = Resources.GetColor(resourceIdColor);
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }

        private void HandleTermsConditonClick(object sender, EventArgs e)
        {
            webView.ScrollTo(0, 0);

            SetButtonStates(0);
            var file = currLang.Contains("fr") ? "TermsAndConditions_fr.html" : "TermsAndConditions_en.html";
            UpdateTextAlteration(file, !_model.NoPhoneAlteration, !_model.NoClaimAlteration, !_model.AcceptedTC);
        }

        private void HandlePrivacyClick(object sender, EventArgs e)
        {
            webView.ScrollTo(0, 0);

            var file = currLang.Contains("fr") ? "Privacy_fr.html" : "Privacy_en.html";
            UpdateTextAlteration(file, !_model.NoPhoneAlteration, false, true);

            SetButtonStates(1);
        }

        private void HandleLegalClick(object sender, EventArgs e)
        {
            webView.ScrollTo(0, 0);

            var file = currLang.Contains("fr") ? "Legal_fr.html" : "Legal_en.html";
            UpdateTextAlteration(file, false, false, true);
            SetButtonStates(2);
        }

        private void HandleSecurityClick(object sender, EventArgs e)
        {
            webView.ScrollTo(0, 0);

            var file = currLang.Contains("fr") ? "Security_fr.html" : "Security_en.html";
            UpdateTextAlteration(file, false, false, true);
            SetButtonStates(3);
        }

        private class MonkeyWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                if (url.StartsWith("tel"))
                {
                    var intent = new Intent(Intent.ActionDial);
                    intent.SetData(Uri.Parse(url));
                    view.Context.StartActivity(intent);
                    return true;
                }

                if (url.StartsWith("http"))
                {
                    view.StopLoading();
                    TermsAndConditionsView.isExternalWebview = true;
                    var browserIntent = new Intent(Intent.ActionView, Uri.Parse(url));
                    view.Context.StartActivity(browserIntent);
                }
                return false;
            }
        }

        private void SetButtonStates(int index)
        {
            termscondition.Enabled = index != 0;
            privacyButton.Enabled = index != 1;
            legalButton.Enabled = index != 2;
            securityButton.Enabled = index != 3;
        }
    }
}