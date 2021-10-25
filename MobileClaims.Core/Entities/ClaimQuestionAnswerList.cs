using System.Collections.Generic;

namespace MobileClaims.Core.Entities
{
    public class ClaimQuestionAnswerList : List<ClaimQuestionAnswerPair>
    {

    }

    public class ClaimResultDetailQAndAList : ClaimQuestionAnswerList
    {
        public List<string> EOB { get; set; }
    }
}