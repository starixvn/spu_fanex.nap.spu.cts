using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
   public class MatchStagingModel
    {
        public int MatchId { get; set; }

        public int TotalScore { get; set; }

        public int BetTypeId { get; set; }
    }
}
