namespace Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Entities
{
    public class RawAssociationModel
    {        
        public int AssociationId { get; set; }

        public int AccountId { get; set; }

        public int DeviceId { get; set; }

        public string CreatedTime { get; set; }

        public int SubscriberId { get; set; }
    }
}
