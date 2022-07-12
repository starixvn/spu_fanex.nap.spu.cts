namespace Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor
{
    using Fanex.Logging;
    using Fanex.NAP.SPU.CTS.Shares.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;   

    public interface IAssociatedGroupMonitorService 
    {
        ScanAssociatedAccountModel ScanAssociatedAccount();

        DBResult AddAssociatedAccount(AddAssociatedAccountModel model);        

        IEnumerable<CustomerInfoMainDBEntity> GetCustomerInfoMainDB(string custIdList, int sourceType);

        DBResult UpdateDangerLevelToMainDB(IEnumerable<CustomerInfoMainDBEntity> customerInfoMainDBs, int dangerLevel, int dangerType);
    }

    public class AssociatedGroupMonitorService : IAssociatedGroupMonitorService
    {
        private readonly IAssociatedGroupMonitorDataAccess dataAccess;

        public AssociatedGroupMonitorService(IAssociatedGroupMonitorDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public ScanAssociatedAccountModel ScanAssociatedAccount()
        {
            return dataAccess.ScanAssociatedAccount();
        }

        public DBResult AddAssociatedAccount(AddAssociatedAccountModel model)
        {
            return dataAccess.AddAssociatedAccount(model);
        }

        public IEnumerable<CustomerInfoMainDBEntity> GetCustomerInfoMainDB(string custIdList, int sourceType)
        {
            return dataAccess.GetCustomerInfoMainDB(custIdList, sourceType);
        }

        public DBResult UpdateDangerLevelToMainDB(IEnumerable<CustomerInfoMainDBEntity> customerInfoMainDBs, int dangerLevel, int dangerType)
        {
            var dangerLevels = customerInfoMainDBs
                .Where(x => x.Danger2 == 0)
                .Select(x => new DangerLevelModel
                {
                    CustId = x.CustID,
                    DangerLevel = dangerLevel
                });            

            if (dangerLevels.Any())
            {
                return dataAccess.UpdateDangerLevelToMainDB(dangerLevels, dangerType);
            }

            return new DBResult { ErrorCode = 0 };
        }
    }
}
