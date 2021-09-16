namespace Fanex.NAP.SPU.CTS.Features.AffectedEvidence
{
    using System.Collections.Generic;

    public class ResultAffectedEvidenceFromQueueEntity
    {
        public bool IsContinue { get; set; }

        public IEnumerable<long> CustIds { get; set; }
    }
}