namespace Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ScanAssociatedAccountCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "op_GroupID")]
        public long GroupID { get; set; }

        [SpParam(SqlParamName = "op_ABI")]
        public int ABI { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_AssociatedGroup_Scan";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
