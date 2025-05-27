namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Models
{
    using Fanex.NAP.SPU.CTS.Shares;
    using System.Collections.Generic;
    using System.Linq;

    public class MatchDetectAssociationSettingModel
    {
        public MatchDetectAssociationSettingModel() { }

        public MatchDetectAssociationSettingModel(
            string sportsDevice,
            string sportsAI,
            string sportsIP,
            string sports3MatchesLast7Days,
            string sportsIPLast3Days)
        {
            if (sportsDevice.IsNotNullOrEmpty())
            {
                DeviceSports = new HashSet<int>(sportsDevice.Split(",").Select(x => int.Parse(x)));
            }

            if (sportsAI.IsNotNullOrEmpty())
            {
                AISports = new HashSet<int>(sportsAI.Split(",").Select(x => int.Parse(x)));
            }

            if (sportsIP.IsNotNullOrEmpty())
            {
                IPSports = new HashSet<int>(sportsIP.Split(",").Select(x => int.Parse(x)));
            }

            if (sports3MatchesLast7Days.IsNotNullOrEmpty())
            {
                _3MatchesLast7DaysSports = new HashSet<int>(sports3MatchesLast7Days.Split(",").Select(x => int.Parse(x)));
            }

            if (sportsIPLast3Days.IsNotNullOrEmpty())
            {
                IPLast3DaysSports = new HashSet<int>(sportsIPLast3Days.Split(",").Select(x => int.Parse(x)));
            }
        }

        public HashSet<int> DeviceSports { get; set; }
        public HashSet<int> AISports { get; set; }
        public HashSet<int> IPSports { get; set; }
        public HashSet<int> _3MatchesLast7DaysSports { get; set; }
        public HashSet<int> IPLast3DaysSports { get; set; }
    }
}
