namespace Fanex.NAP.SPU.CTS.Features.AssociationDetection
{
    using Fanex.NAP.SPU.CTS.Features.AssociationDetection.Entities;
    using System.Collections.Generic;

    public interface IAssociationDetectionService
    {
        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList);
    }

    public class AssociationDetectionService : IAssociationDetectionService
    {
        private readonly IAssociationDetectionDataAccess _associationDetectionDataAccess;

        public AssociationDetectionService(IAssociationDetectionDataAccess associationDetectionDataAccess)
        {
            _associationDetectionDataAccess = associationDetectionDataAccess;
        }

        public IEnumerable<DetectAssBySharedIPMatchesModel> DetectAssociationBySharedMatches(string custIDList)
        {
            if (string.IsNullOrWhiteSpace(custIDList))
            {
                return new List<DetectAssBySharedIPMatchesModel>();
            }

            return _associationDetectionDataAccess.DetectAssociationBySharedMatches(custIDList);
        }
    }
}
