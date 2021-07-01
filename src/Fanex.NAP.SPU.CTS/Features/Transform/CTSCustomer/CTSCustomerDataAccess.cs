namespace Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer.Criteria;
    using Fanex.NAP.SPU.CTS.Features.Transform.CTSCustomer.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICTSCustomerDataAccess
    {
        LastScannedCustModel GetLastInsertCust();

        UpdateCustomerModel GetCustomerFromMainDB(int quantityPerBatch, Int64 lastScannedCustID);

        List<CustomerMainDBModel> GetCustSubFromMainDB(int quantityPerBatch, Int64 lastScannedCustID);

        IEnumerable<InsertCustomerModel> TransformCustomerFromMainDB(string customerJson);

        void UpdateCustInfoComplete(string customers, DateTime? lastUpdateTime, Int64? lastUpdateCustID);

        void UpdateCustomerComplete(string customers, DateTime? lastUpdateTime, Int64? lastUpdateCustID);
    }

    public class CTSCustomerDataAccess : ICTSCustomerDataAccess
    {
        readonly IDynamicRepository repository;

        public CTSCustomerDataAccess()
        {
            repository = new DynamicRepository();
        }

        public LastScannedCustModel GetLastInsertCust()
        {
            var criteria = new GetLastInsertCustCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var result = db.QueryMultiple(criteria))
                {
                    var LastCustID = result.Read<Int64>().FirstOrDefault();
                    var LastCustSubID = result.Read<Int64>().FirstOrDefault();

                    return new LastScannedCustModel()
                    {
                        LastCustID = LastCustID,
                        LastCustSubID = LastCustSubID
                    };
                }
            }
        }

        public UpdateCustomerModel GetCustomerFromMainDB(int quantityPerBatch, Int64 lastScannedCustID)
        {
            var criteria = new GetCustomersFromMainDbCriteria
            {
                LastCustId = lastScannedCustID,
                RowSize = quantityPerBatch,
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var result = db.QueryMultiple(criteria))
                {
                    var customers = result?.Read<CustomerMainDBModel>();
                    var LastCustID = result?.Read<Int64?>();

                    return new UpdateCustomerModel()
                    {
                        CustomerInfos = customers.ToList(),
                        LastInsertCustID = LastCustID?.FirstOrDefault()
                    };
                }
            }
        }

        public List<CustomerMainDBModel> GetCustSubFromMainDB(int quantityPerBatch, Int64 lastScannedCustID)
        {
            var criteria = new GetCustSubFromMainDbCriteria
            {
                LastCustSubId = lastScannedCustID,
                RowSize = quantityPerBatch
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<CustomerMainDBModel>(criteria).ToList();
            }
        }

        public IEnumerable<InsertCustomerModel> TransformCustomerFromMainDB(string customerJson)
        {
            var custInfos = new List<InsertCustomerModel>();

            var criteria = new TransformCustomerFromMainDbCriteria
            {
                CustomerJson = customerJson
            };

            return repository.Fetch<InsertCustomerModel>(criteria);
        }

        public void UpdateCustomerComplete(string customers, DateTime? lastUpdateTime, Int64? lastUpdateCustID)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateCustomerCompleteCriteria
            {
                CustomerInfo = customers,
                LastUpdateCustID = lastUpdateCustID,
                LastUpdateTime = lastUpdateTime
            };

            repository.Execute(criteria);
        }

        public void UpdateCustInfoComplete(string customers, DateTime? lastUpdateTime, Int64? lastUpdateCustID)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateCustInfoCompleteCriteria
            {
                CustInfo = customers,
                LastUpdateCustID = lastUpdateCustID,
                LastUpdateTime = lastUpdateTime
            };

            repository.Execute(criteria);
        }
    }
}
