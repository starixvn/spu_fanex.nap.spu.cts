namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{   
    public class MatchBetTypeSettingEntity
    {
        public string ListNameDisplay { get; set; }

        public int BetTypeID { get; set; }

        public int BetID { get; set; }

        public string BetTypeName { get; set; }

        public int SportID { get; set; }

        public bool IsSelected { get; set; }

        /*************************************************/

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool IsDetectMatchMonitor { get; set; }

        public bool IsFilterBetID { get; set; }

        public bool IsFilterBetTeam { get; set; }

        public bool IsFilterHdp { get; set; }

        public bool IsOnlyGetHdp1 { get; set; }

    }
}
