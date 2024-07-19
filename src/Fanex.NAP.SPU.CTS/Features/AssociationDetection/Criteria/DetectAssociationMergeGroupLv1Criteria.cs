namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class DetectAssociationMergeGroupLv1Criteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_Device")]
        public string DeviceJson { get; set; }

        [SpParam(SqlParamName = "ip_AI")]
        public string AIJson { get; set; }

        [SpParam(SqlParamName = "ip_IP")]
        public string IPJson { get; set; }

        [SpParam(SqlParamName = "ip_SharedMatches")]
        public string SharedMatchesJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_Association_CustGroup";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
