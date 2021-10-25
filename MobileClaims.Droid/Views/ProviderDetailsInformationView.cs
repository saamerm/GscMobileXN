using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Net;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Text.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.left_region, BackstackBehaviour = BackstackTypes.ADD)]
    public class ProviderDetailsInformationView : BaseFragment<ProviderDetailsInformationViewModel>, IOnStreetViewPanoramaReadyCallback
    {
        private const string PhoneDialUriPrefix = "tel:";
        private View _view;
        private StreetViewPanoramaView _googleStreetViewPanoramaView;
        private StreetViewPanorama _streetViewPanorama;
        private TextView _linkDetailTextView;
        private TextView _phoneNumberDetailsTextView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);
            _view = this.BindingInflate(Resource.Layout.ProviderDetailsInformationLayout, null);
            return _view;
        }

        public override void OnViewModelSet()
        {
            base.OnViewModelSet();

            if (_streetViewPanorama != null)
            {
                SetPanoramaPosition();
                CreateLinks();
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Init();

            _googleStreetViewPanoramaView.OnCreate(savedInstanceState);
            _googleStreetViewPanoramaView.GetStreetViewPanoramaAsync(this);
        }

        private void Init()
        {
            _googleStreetViewPanoramaView =
                _view.FindViewById<StreetViewPanoramaView>(Resource.Id.googleStreetViewPanoramaView);
            _linkDetailTextView =
                _view.FindViewById<TextView>(Resource.Id.linkDetailTextView);
            _phoneNumberDetailsTextView =
                _view.FindViewById<TextView>(Resource.Id.phoneNumberDetailsTextView);
            CreateLinks();
        }

        private void CreateLinks()
        {
            Linkify.AddLinks(_linkDetailTextView, MatchOptions.WebUrls);
            if (!Linkify.AddLinks(_phoneNumberDetailsTextView, MatchOptions.PhoneNumbers))
            {
                var text = _phoneNumberDetailsTextView.Text;
                var spannableString = new SpannableString(text);
                spannableString.SetSpan(new URLSpan(""), 0, spannableString.Length(), SpanTypes.ExclusiveExclusive);
                _phoneNumberDetailsTextView.SetText(spannableString, TextView.BufferType.Spannable);
                _phoneNumberDetailsTextView.Click += (sender, args) =>
                {
                    var uri = PhoneDialUriPrefix + text.Trim();
                    var intent = new Intent(Intent.ActionDial);
                    intent.SetData(Uri.Parse(uri));
                    StartActivity(intent);
                };
            }
        }

        public void OnStreetViewPanoramaReady(StreetViewPanorama panorama)
        {
            _streetViewPanorama = panorama;
            _streetViewPanorama.ZoomGesturesEnabled = true;
            SetPanoramaPosition();
        }

        private void SetPanoramaPosition()
        {
            _streetViewPanorama.SetPosition(
                new LatLng(ViewModel.ViewModelParameter.Model.Latitude,
                    ViewModel.ViewModelParameter.Model.Longitude));
        }
    }
}