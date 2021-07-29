namespace Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice
{
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Entities;
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAssociationByDeviceService
    {
        void TransformAssociationByDevice(IEnumerable<RawAssociationEntity> rawAssociations);

        IEnumerable<RawAssociationEntity> GetRawAssociation(int quantityPerBatch, int numOfBatch);                
    }

    public class AssociationByDeviceService : IAssociationByDeviceService
    {
        private readonly IAssociationByDeviceDataAccess dataAccess;

        private readonly ISentryClient sentryClient;
        
        public AssociationByDeviceService(IAssociationByDeviceDataAccess dataAccess, ISentryClient sentryClient)
        {
            this.dataAccess = dataAccess;
            this.sentryClient = sentryClient;
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
