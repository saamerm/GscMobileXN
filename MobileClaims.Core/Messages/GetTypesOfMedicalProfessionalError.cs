using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTypesOfMedicalProfessionalError : MvxMessage
    {
        public string Message { get; set; }
        public GetTypesOfMedicalProfessionalError(object sender) : base(sender)
        {

        }
    }
}
