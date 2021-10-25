using MobileClaims.Core.Messages;
using System;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IMvxMessenger _messenger;

        public LanguageService(IMvxMessenger messenger)
        {
            _messenger = messenger;
        }

        private string _currentLanguage = "en-CA";
        public string CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
        }

        public bool IsEnglishLanguage => CurrentLanguage.StartsWith("en", StringComparison.OrdinalIgnoreCase);

        public void SetCurrentLanguage(string languageCode)
        {
            if (languageCode != _currentLanguage)
            {
                _currentLanguage = languageCode;
                _messenger.Publish<LanguageUpdatedMessage>(new LanguageUpdatedMessage(this));
            }
        }

        public string AH
        {
            get
            {
                // NOTE: Don't modifiy the following line of code. Modifying it will most likely break a pre-build script
                string ahValue = "xlMjAxNDpnc2Ntb";
                return ahValue;
            }
        }

        public string AHX
        {
            get
            {
                // NOTE: Don't modifiy the following line of code. Modifying it will most likely break a pre-build script
                string ahxValue = "hJZFA6aW50W";
                return ahxValue;
            }
        }
    }
}
