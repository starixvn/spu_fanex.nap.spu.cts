namespace Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Criteria
{
    using Fanex.Data.Repository;

    public class ScanSystemFormulaBetCustomerCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "CTS_DC_TWProblemDetectionMonitor_Scan";

        public override bool IsValid()
            => true;
    }
}
