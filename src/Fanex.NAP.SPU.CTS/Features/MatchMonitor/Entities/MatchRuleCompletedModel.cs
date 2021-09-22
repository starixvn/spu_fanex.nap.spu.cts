using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
   public class MatchRuleCompletedModel
    {
        [JsonProperty("GroupID")]
        public int GroupId { get; set; }               
                
        public bool IsHedging { get; set; }

        [JsonProperty("TransIDList")]
        public string TransIdList { get; set; }

        [JsonProperty("CustIDList")]
        public string CustIdList { get; set; }

        [JsonProperty("OldGroupIDList")]
        public string OldGroupIdList { get; set; }
    }
}
