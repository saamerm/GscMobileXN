using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class OrientationChange : MvxMessage
    {

        public string Message { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string OrientationStr { get; set; }
        public OrientationChange(object sender)
            : base(sender)
        {

        }
    }
}
