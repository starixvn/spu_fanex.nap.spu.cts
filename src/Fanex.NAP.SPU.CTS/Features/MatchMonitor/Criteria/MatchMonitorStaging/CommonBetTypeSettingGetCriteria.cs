namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class CommonBetTypeSettingGetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_FunctionID")]
        public int FunctionID { get; set; }

        [SpParam(SqlParamName = "ip_SportTypeIDList")]
        public string SportTypeIDList { get; set; }

        [SpParam(SqlParamName = "ip_BettypeIDList")]
        public string BettypeIDList { get; set; }

        [SpParam(SqlParamName = "ip_IsShowDetailsX")]
        public bool IsShowDetailsX { get; set; }

        public override string GetSettingKey()
                => "CTS_DC_Common_BettypeSetting_Get";

        public override bool IsValid()
            => true;
    }
}
