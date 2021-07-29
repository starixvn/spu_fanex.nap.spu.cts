namespace Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RawAssociationEntity
    {        
        public int BatchId { get; set; }

        public int AssociationId { get; set; }

        public int AccountId { get; set; }

        public int DeviceId { get; set; }

        public DateTime CreatedTime { get; set; }

        public int SubscriberId { get; set; }
    }
}
