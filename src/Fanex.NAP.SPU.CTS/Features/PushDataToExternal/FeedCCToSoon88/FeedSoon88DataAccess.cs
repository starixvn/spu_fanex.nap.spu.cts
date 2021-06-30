namespace Fanex.NAP.SPU.CTS.Features.PushDataToExternal.FeedCCToSoon88
{
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public interface IFeedSoon88DataAccess
    {
        IEnumerable<AssociationWithCategoryEntity> GetAssociationWithCategory(IEnumerable<CustomerClassEntity> custClasses, bool isDelete);
    }

    public class FeedSoon88DataAccess : IFeedSoon88DataAccess
    {
        private readonly IDynamicRepository repository;

        public FeedSoon88DataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<AssociationWithCategoryEntity> GetAssociationWithCategory(IEnumerable<CustomerClassEntity> custClasses, bool isDelete)
        {
            var criteria = new AssociationWithCategoryGetCriteria
            {
                CustIds = string.Join(",", custClasses.Select(c => c.CustID).Distinct()),
                IsDelete = isDelete
            };

            return repository.Fetch<AssociationWithCategoryEntity>(criteria);
        }
    }
}