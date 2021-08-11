namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer
{
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface IArchiveCustomerService
    {
        void ArchiveCTSCustomer(int batchSize);
    }

    public class ArchiveCustomerService : IArchiveCustomerService
    {
        private readonly ISystemParameterServices sysParamService;
        private readonly IArchiveCustomerDataAccess archiveCustomerDataAccess;
        private readonly ISentryClient sentryClient;

        public ArchiveCustomerService(ISystemParameterServices sysParamService, IArchiveCustomerDataAccess archiveCustomerDataAccess, ISentryClient sentryClient)
        {
            this.sysParamService = sysParamService;
            this.archiveCustomerDataAccess = archiveCustomerDataAccess;
            this.sentryClient = sentryClient;

        }

        public void ArchiveCTSCustomer(int batchSize)
        {
            var lastTimeArchive_Credit = sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit);
            var lastCustIdArchive_Credit = sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit);
            var archivedCusts_Credit = archiveCustomerDataAccess.GetArchivedCustomerFromMainDb(lastCustIdArchive_Credit, lastTimeArchive_Credit, batchSize, SubscriberType.Credit);
            if (archivedCusts_Credit is not null && archivedCusts_Credit.Any())
            {
                var jsonData = JsonConvert.SerializeObject(archivedCusts_Credit);
                archiveCustomerDataAccess.ArchiveCTSCustomer(jsonData, SubscriberType.Credit);
                return;
            }

            var lastTimeArchive_Deposit = sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Deposit);
            var lastCustIdArchive_Deposit = sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Deposit);
            var archivedCusts_Deposit = archiveCustomerDataAccess.GetArchivedCustomerFromMainDb(lastCustIdArchive_Deposit, lastTimeArchive_Deposit, batchSize, SubscriberType.Deposit);
            if (archivedCusts_Deposit is not null && archivedCusts_Deposit.Any())
            {
                var jsonData = JsonConvert.SerializeObject(archivedCusts_Deposit);
                archiveCustomerDataAccess.ArchiveCTSCustomer(jsonData, SubscriberType.Deposit);
            }
        }
    }
}