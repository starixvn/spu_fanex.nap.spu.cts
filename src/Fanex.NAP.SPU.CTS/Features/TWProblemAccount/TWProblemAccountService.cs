namespace Fanex.NAP.SPU.CTS.Features.TWProblemAccount
{
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Criteria;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Entities;
    using System.Collections.Generic;

    public interface ITWProblemAccountService
    {
        IEnumerable<RescanTWProblemAccountModel> ScanSystemFormulaBetMonitor(out uint lastCustID);
        IEnumerable<RescanTWProblemAccountModel> GetCustomerPerformance(string custIdList);
        void RemoveSystemFormulaBetCustomer(string custIdList);
        void InsertProblemAccToNAP(NapProblemAccInsertCriteria paramFromTW);
        void ScanSystemFormulaBetMonitorComplete(uint lastCustId);
    }

    public class TWProblemAccountService : ITWProblemAccountService
    {
        private readonly ITWProblemAccountDataAccess dataAccess;

        public TWProblemAccountService(ITWProblemAccountDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RescanTWProblemAccountModel> ScanSystemFormulaBetMonitor(out uint lastCustID)
        {
            return dataAccess.ScanSystemFormulaBetMonitor(out lastCustID);
        }

        public IEnumerable<RescanTWProblemAccountModel> GetCustomerPerformance(string custIdList)
        {
            return dataAccess.GetCustomerPerformance(custIdList);
        }

        public void RemoveSystemFormulaBetCustomer(string custIdList)
        {
            dataAccess.RemoveSystemFormulaBetCustomer(custIdList);
        } 
        
        public void ScanSystemFormulaBetMonitorComplete(uint lastCustId)
        {
            dataAccess.ScanSystemFormulaBetMonitorComplete(lastCustId);
        }
        
        public void InsertProblemAccToNAP(NapProblemAccInsertCriteria paramFromTW)
        {
            dataAccess.InsertProblemAccToNAP(paramFromTW);
        }
    }
}
