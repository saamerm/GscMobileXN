namespace MobileClaims.Core.ViewModelParameters
{
    public class CardViewModelParameter
    {
        public bool FromLoginScreen { get; set; }

        public CardViewModelParameter(bool fromLoginScreen)
        {
            FromLoginScreen = fromLoginScreen;
        }
    }
}