namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Criteria
{
    using Fanex.Data.Repository;
    using Fanex.Data;

    public class DetectAssociationByIPCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ListCustId")]
        public string ListCustId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_Association_IpSharedLast3Days";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}