namespace MobileClaims.Core.Services
{
    public interface ILanguageService
    {
        string CurrentLanguage { get; }
        void SetCurrentLanguage(string languageCode);
        string AH { get; }
        string AHX { get; }
        bool IsEnglishLanguage { get; }
    }
}
