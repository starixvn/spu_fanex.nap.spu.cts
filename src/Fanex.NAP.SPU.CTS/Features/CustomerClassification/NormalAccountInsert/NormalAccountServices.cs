namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.NormalAccountInsert
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface INormalAccountServices
    {
        IEnumerable<CustomerClassificationEntity> GetNewClassification();
    }

    public class NormalAccountServices : INormalAccountServices
    {
        private readonly INormalAccountDataAccess normalAccountDataAccess;

        public NormalAccountServices(INormalAccountDataAccess normalAccountDataAccess)
        {
            this.normalAccountDataAccess = normalAccountDataAccess;
        }

        public IEnumerable<CustomerClassificationEntity> GetNewClassification()
        {
            var newClassifications = normalAccountDataAccess.GetNewClassification();

            if(newClassifications!=null && newClassifications.Any())
            {
                return newClassifications.Where(c => c.IsGeneralCategory);
            }

            return newClassifications;
        }

        public IEnumerable<CustAssociationProblemAccountEntity> GetCustomerAssociationProblemAccount(IEnumerable<CustomerClassificationEntity> listClassification)
        {
            if (listClassification != null && listClassification.Any())
            {
                var custIds = listClassification.Select(x => x.CustId).Distinct();

                return normalAccountDataAccess.GetCustsAssociationProblemAccount(string.Join(",", custIds));
            }

            return new List<CustAssociationProblemAccountEntity>();
        }

        public string MergeCustomerClassificationInfo(IEnumerable<CustomerClassificationEntity> custClassification, IEnumerable<CustAssociationProblemAccountEntity> custAssociationPA)
        {
            if (custAssociationPA == null || !custAssociationPA.Any()
                || custClassification == null || !custClassification.Any())
            {
                return string.Empty;
            }

            var result = custClassification.GroupJoin(custAssociationPA,
                               custClass => custClass.CustId,
                               custAss => custAss.CustID,
                               (custClass, custPA) => new { custClass, custPA })
                               .SelectMany(
                                    group => group.custPA.DefaultIfEmpty(),
                                    (group, temp) => new CustomerClassificationEntity
                                    {
                                        CustId = group.custClass.CustId,
                                        SportGroupId = group.custClass.SportGroupId,
                                        CategoryId = group.custClass.CategoryId,
                                        CreatedTime = group.custClass.CreatedTime,
                                        TurnoverRM = group.custClass.TurnoverRM,
                                        WinlossRM = group.custClass.WinlossRM,
                                        BetCount = group.custClass.BetCount,
                                        ActiveDays = group.custClass.ActiveDays,
                                        LastXDaysMargin = group.custClass.LastXDaysMargin,
                                        ProbationPeriodWinloss = group.custClass.ProbationPeriodWinloss,
                                        IsGeneralCategory = group.custClass.IsGeneralCategory,
                                        TaggingID = (temp != null) ? temp.TaggingID : 0,
                                        TaggingType = (temp != null) ? temp.TaggingType : 0
                                    });

            return JsonConvert.SerializeObject(result);
        }

        public InsertCategoryResultEntity InsertMainCustomerClassification(string newClassification)
        {
            if(!string.IsNullOrEmpty(newClassification))
            {
               return normalAccountDataAccess.InsertMainCustomerClassification(newClassification);
            }

            return new InsertCategoryResultEntity();
        }
    }
}
