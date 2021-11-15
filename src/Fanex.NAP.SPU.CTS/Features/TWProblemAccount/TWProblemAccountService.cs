namespace Fanex.NAP.SPU.CTS.Features.TWProblemAccount
{
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Criteria;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITWProblemAccountService
    {
        IEnumerable<RescanTWProblemAccountModel> ScanSystemFormulaBetMonitor();
        IEnumerable<RescanTWProblemAccountModel> GetCustomerPerformance(string custIdList);
        void RemoveSystemFormulaBetCustomer(string custIdList);
        void InsertProblemAccToNAP(string usernameList, NapProblemAccInsertCriteria paramFromTW);
    }

    public class TWProblemAccountService : ITWProblemAccountService
    {
        private readonly ITWProblemAccountDataAccess dataAccess;

        public TWProblemAccountService(ITWProblemAccountDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<RescanTWProblemAccountModel> ScanSystemFormulaBetMonitor()
        {
            return dataAccess.ScanSystemFormulaBetMonitor();
        }

        public IEnumerable<RescanTWProblemAccountModel> GetCustomerPerformance(string custIdList)
        {
            return dataAccess.GetCustomerPerformance(custIdList);
        }

        public void RemoveSystemFormulaBetCustomer(string custIdList)
        {
            dataAccess.RemoveSystemFormulaBetCustomer(custIdList);
        }
        
        public void InsertProblemAccToNAP(string usernameList, NapProblemAccInsertCriteria paramFromTW)
        {
            dataAccess.InsertProblemAccToNAP(usernameList, paramFromTW);
        }
    }
}
