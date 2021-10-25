using Android.Content;
using Android.Util;
using Android.Webkit;
using MobileClaims.Core.Constants;
using MobileClaims.Droid.Helpers;
using MobileClaims.Core.Services.FacadeEntities;
using System;
using System.Text;
using Java.Util;
using System.IO;

namespace MobileClaims.Droid.Views
{
    public class BindableWebView : WebView
    {
        private string _text;
        private TextAlterationWithDate _textAlteration;

        public event EventHandler HtmlContentChanged;

        public BindableWebView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public TextAlterationWithDate TextAlteration
        {
            get => _textAlteration;
            set
            {
                if (value == null || string.Equals(value.TextAlteration, StringConstants.TextAlterationNotAvailable, StringComparison.OrdinalIgnoreCase)
                                  || string.Equals(value.TextAlterationDate, StringConstants.TextAlterationDateNotAvailable , StringComparison.OrdinalIgnoreCase))
                {
                    _text = GetStaticString();
                }
                else
                {
                    _text = GetStaticString(value.TextAlteration, value.TextAlterationDate);
                }
                LoadDataWithBaseURL(null, _text, "text/html", "utf-8", null);
                UpdatedHtmlContent();
            }
        }

        private void UpdatedHtmlContent()
        {
            var handler = HtmlContentChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private string GetStaticString(string dynamicText = null, string dynamicDate = null)
        {
            var sb = new StringBuilder();
            string htmlString;
#if CCQ || ARTA || WWL
            if (Locale.Default.Language.Contains("fr"))
            {
                using (var sr = new StreamReader(Context.Assets.Open("webagreement_fr.html")))
                {
                    htmlString = sr.ReadToEnd();
                }
            }
            else
            {
                using (var sr = new StreamReader(Context.Assets.Open("webagreement_en.html")))
                {
                    htmlString = sr.ReadToEnd();
                }
            }
#else
            sb.AppendFormat(@"<html><body style=\'font-family:AvenirLTStd-Book;color:#5F5E5F; '>");

            sb.AppendFormat("<p style='font-family:LeagueGothic; font-size:22px; color:"+ GetHexColor(Resource.Color.highlight_color) + ";font-weight:bold'>");
            sb.AppendFormat(Resources.GetString(Resource.String.webAgreementTitle));
            sb.AppendFormat("</p>");

            sb.AppendFormat("<p>");          
            sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent1, new[] { Resources.GetString(Resource.String.greenshieldcanada) }));
            sb.AppendFormat("</p>");

            sb.AppendFormat(Environment.NewLine);

            sb.AppendFormat("<p style='font-size:18px;color:#FF0000;text-align:center;font-weight:bold'>");
            if (!string.IsNullOrWhiteSpace(dynamicDate))
            {
                sb.AppendFormat(dynamicDate);
            }
            else
            {
                sb.AppendFormat(Resources.GetString(Resource.String.webContentSubTitle1_WithDate));
            }
            sb.AppendFormat("</p>");

            sb.AppendFormat(Environment.NewLine);

            sb.AppendFormat("<div style='text-align:center'>");
            sb.AppendFormat("<label style='font-size:18px;color:" + GetHexColor(Resource.Color.highlight_color) + ";text-align:center;font-weight:bold'>");
            sb.AppendFormat(Resources.GetString(Resource.String.greenshieldcanada));
            sb.AppendFormat("</label></br>");

            sb.AppendFormat("<label style='font-size:18px;color:" + GetHexColor(Resource.Color.highlight_color) + ";text-align:center;font-weight:bold'>");
            sb.AppendFormat(Resources.GetString(Resource.String.webContentSubTitle2));
            sb.AppendFormat("</label></br>");

            sb.AppendFormat("<label style='font-size:18px;color:" + GetHexColor(Resource.Color.highlight_color) + ";text-align:center;font-weight:bold'>");
            sb.AppendFormat(Resources.GetString(Resource.String.webContentSubTitle3));
            sb.AppendFormat("</label>");
            sb.AppendFormat("</div>");

            sb.AppendFormat(Environment.NewLine);

            sb.AppendFormat("<p>");
            sb.AppendFormat(Resources.GetString(Resource.String.webContent2));
            sb.AppendFormat("</p>");

            sb.AppendFormat(Environment.NewLine);

            sb.AppendFormat("<p style='font-size:18px;font-weight:bold'>");
            sb.AppendFormat(Resources.GetString(Resource.String.webContentSubTitle4));
            sb.AppendFormat("</p>");

            if (!string.IsNullOrWhiteSpace(dynamicText))
            {
                sb.AppendFormat(dynamicText);
            }
            else
            {
                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.GetString(Resource.String.webContent3));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent4, new[]
                {   Resources.GetString(Resource.String.greenshieldcanada),
                    Resources.GetString(Resource.String.greenshieldcanada),
                    Resources.GetString(Resource.String.greenshieldcanada)}));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent5, new[] { Resources.GetString(Resource.String.greenshieldcanada) }));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent6, new[] { Resources.GetString(Resource.String.greenshieldcanada) }));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.GetString(Resource.String.webContent7));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent8, new[] 
                { Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada)}));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent9, new[] { Resources.GetString(Resource.String.greenshieldcanada) }));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.GetString(Resource.String.webContent10));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.FormatterBrandKeywords(Resource.String.webContent11, new[]
                {
                    Resources.GetString(Resource.String.greenshieldcanada),
                    Resources.GetString(Resource.String.greenshieldcanada),
                    Resources.GetString(Resource.String.greenshieldcanada)
                }));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.GetString(Resource.String.webContent12));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.GetString(Resource.String.webContent13));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);

                sb.AppendFormat("<p>");
                sb.AppendFormat(Resources.GetString(Resource.String.webContent14));
                sb.AppendFormat("</p>");

                sb.AppendFormat(Environment.NewLine);
            }

            sb.AppendFormat("<p>");
            sb.AppendFormat(Resources.GetString(Resource.String.webContent15));
            sb.AppendFormat("</p>");

            sb.AppendFormat("</body></html>");
            htmlString = sb.ToString();
#endif
            return htmlString;
        }

        private string GetHexColor(int resourceIdColor)
        {
            var color = Resources.GetColor(resourceIdColor);
            return string.Format("#{0:X}{1:X}{2:X}", color.R, color.G, color.B);
        }
    }
}