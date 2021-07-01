namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProbationClassification
{
    public class ProbationEntity
    {
        public long CustId { get; set; }

        public int SportGroupId { get; set; }

        // 0. VIP & Normal OR 1.Update to Smart, High Risk, VIP & Normal
        public int IsProbationLastDay { get; set; }

        public int CategoryId { get; set; }

        public bool IsPassed { get; set; }
    }
}