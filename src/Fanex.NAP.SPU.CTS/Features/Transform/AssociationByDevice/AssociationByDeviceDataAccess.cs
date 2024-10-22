namespace Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Criteria;
    using Fanex.NAP.SPU.CTS.Features.Transform.AssociationByDevice.Entities;
    using System.Collections.Generic;

    public interface IAssociationByDeviceDataAccess
    {
        IEnumerable<RawAssociationEntity> GetRawAssociation(int quantityPerBatch, int numOfBatch);

        void TransformAssociationByDevice(string associationJson);
    }

    public class AssociationByDeviceDataAccess : IAssociationByDeviceDataAccess
    {
        readonly IDynamicRepository repository;

        public AssociationByDeviceDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<RawAssociationEntity> GetRawAssociation(int quantityPerBatch, int numOfBatch)
        {
            var criteria = new GetRawAssociationListCriteria
            {
                NumberOfAssociation = quantityPerBatch,
                NumberOfBatch = numOfBatch
            };

            return repository.Fetch<RawAssociationEntity>(criteria);
        }

        public void TransformAssociationByDevice(string associationJson)
        {
            var criteria = new TransformAssociationByDeviceCriteria
            {
                AssociationList = associationJson
            };

            repository.Execute(criteria);
        }
    }
}
