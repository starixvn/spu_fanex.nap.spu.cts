namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Criteria;
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface IAssociationDetectionDataAccess
    {
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList);
    }

    public class AssociationDetectionDataAccess : IAssociationDetectionDataAccess
    {
        private readonly IDynamicRepository _repository;

        public AssociationDetectionDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList)
        {
            var criteria = new DetectAssociationBySharedMatchesCriteria
            {
                CustIDList = custIDList,
                QueryType = (int)DetectAssociation_QueryType.DetectAss
            };

            return _repository.Fetch<DetectAssBySharedIPMatchesModel>(criteria);
        }
    }
}
