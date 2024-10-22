namespace Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice
{
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public interface IAssociationByDeviceService
    {
        void TransformAssociationByDevice(IEnumerable<RawAssociationEntity> rawAssociations);

        IEnumerable<RawAssociationEntity> GetRawAssociation(int quantityPerBatch, int numOfBatch);                
    }

    public class AssociationByDeviceService : IAssociationByDeviceService
    {
        private readonly IAssociationByDeviceDataAccess dataAccess;

        
        public AssociationByDeviceService(IAssociationByDeviceDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RawAssociationEntity> GetRawAssociation(int quantityPerBatch, int numOfBatch)
        {
            return dataAccess.GetRawAssociation(quantityPerBatch, numOfBatch);
        }

        public void TransformAssociationByDevice(IEnumerable<RawAssociationEntity> rawAssociations)
        {
            if (rawAssociations != null && rawAssociations.Any())
            {                
                var associationJson = JsonConvert.SerializeObject(TransformData(rawAssociations));

                dataAccess.TransformAssociationByDevice(associationJson);
            }
        }

        private IEnumerable<RawAssociationModel> TransformData(IEnumerable<RawAssociationEntity> rawAssociations)
        {
            if (rawAssociations != null && rawAssociations.Any())
            {
                return rawAssociations.Select(x => new RawAssociationModel
                {
                    AccountId = x.AccountId,
                    AssociationId = x.AssociationId,
                    CreatedTime = x.CreatedTime.ToString("yyyy-MM-dd hh:mm:ss.fff"),
                    DeviceId = x.DeviceId,
                    SubscriberId = x.SubscriberId
                });
            }

            return new List<RawAssociationModel>();
        }
    }
}
