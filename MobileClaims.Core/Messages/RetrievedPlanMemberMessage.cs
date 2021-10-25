using MobileClaims.Core.Entities;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class RetrievedPlanMemberMessage : MvxMessage
    {
        public PlanMember PlanMember
        {
            get;
            set;
        }
        public RetrievedPlanMemberMessage(object sender, PlanMember planmember) :base(sender)
        {
            this.PlanMember = planmember;
        }
        private RetrievedPlanMemberMessage(object sender)
            : base(sender)
        { }
    }
}
