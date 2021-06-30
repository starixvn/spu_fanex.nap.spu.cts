namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.DailyClassification
{
    using Newtonsoft.Json;
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;

    public interface IDailyClassificationServices
    {
        IEnumerable<CustClassificationDailyEntity> GetCustClassificationForDailyScanning(int batchQuantity, int rowQuantityPerBatch);

        void ScanningDailyClassification(IEnumerable<CustClassificationDailyEntity> customers, bool isCheckLast14Days);
    }

    public class DailyClassificationServices : IDailyClassificationServices
    {
        private readonly IDailyClassificationDataAccess dailyClassificationDataAccess;
        private readonly ISentryClient sentryClient;

        public DailyClassificationServices(IDailyClassificationDataAccess dailyClassificationDataAccess, ISentryClient sentryClient)
        {
            this.dailyClassificationDataAccess = dailyClassificationDataAccess;
            this.sentryClient = sentryClient;
        }

        public IEnumerable<CustClassificationDailyEntity> GetCustClassificationForDailyScanning(int batchQuantity, int rowQuantityPerBatch)
        {
            return dailyClassificationDataAccess.GetCustomerForDailyScanning(batchQuantity, rowQuantityPerBatch);
        }

        public void ScanningDailyClassification(IEnumerable<CustClassificationDailyEntity> customers, bool isCheckLast14Days)
        {
            if (customers != null && customers.Any())
            {
                var custGroupByBatch = customers.GroupBy(c => c.BatchID);

                foreach (var custGroup in custGroupByBatch)
                {
                    ClassifyCustomerGeneralCategoryDaily(custGroup, true);
                    UpdateDailyClassificationStatus(custGroup);
                }
            }
        }

        public void ClassifyCustomerGeneralCategoryDaily(IEnumerable<CustClassificationDailyEntity> customers, bool isCheckLast14Days)
        {
            if (customers != null && customers.Any())
            {
                var custIds = string.Join(",", customers.Select(c => c.CustId));

                dailyClassificationDataAccess.ClassifyCustomerGeneralCategoryDaily(custIds, isCheckLast14Days);
            }
        }

        public void UpdateDailyClassificationStatus(IEnumerable<CustClassificationDailyEntity> customers)
        {
            if (customers != null && customers.Any())
            {
                string jsonData = JsonConvert.SerializeObject(customers.Select(a => new
                {
                    a.CustId,
                    a.SportGroupId
                }));

                dailyClassificationDataAccess.ClassifyCustomerDailyDone(jsonData);
            }
        }
    }
}