using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTypesOfMedicalProfessionalComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetTypesOfMedicalProfessionalComplete(object sender) : base(sender)
        {

        }
    }
}
