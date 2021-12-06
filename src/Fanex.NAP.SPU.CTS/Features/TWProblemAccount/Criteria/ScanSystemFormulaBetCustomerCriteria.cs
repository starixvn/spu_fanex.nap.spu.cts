namespace Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ScanSystemFormulaBetCustomerCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_LastCustID")]
        public uint LastCustId { get; set; }

        public override string GetSettingKey()
            => "CTS_DC_TWProblemDetectionMonitor_Scan";

        public override bool IsValid()
            => true;
    }
}
