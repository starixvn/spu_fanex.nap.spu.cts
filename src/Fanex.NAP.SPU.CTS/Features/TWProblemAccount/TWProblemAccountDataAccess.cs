namespace Fanex.NAP.SPU.CTS.Features.TWProblemAccount
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Criteria;
    using Fanex.NAP.SPU.CTS.Features.TWProblemAccount.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ITWProblemAccountDataAccess
    {
        IEnumerable<RescanTWProblemAccountModel> ScanSystemFormulaBetMonitor(out uint lastCustID);
        IEnumerable<RescanTWProblemAccountModel> GetCustomerPerformance(string custIdList);
        void RemoveSystemFormulaBetCustomer(string custIdList);
        void InsertProblemAccToNAP(string usernameList, NapProblemAccInsertCriteria paramFromTW);
        void ScanSystemFormulaBetMonitorComplete(uint lastCustId);
    }

    public class TWProblemAccountDataAccess : ITWProblemAccountDataAccess
    {
        private readonly IDynamicRepository repository;

        public TWProblemAccountDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<RescanTWProblemAccountModel> ScanSystemFormulaBetMonitor(out uint lastCustID)
        {
            var criteria = new ScanSystemFormulaBetCustomerCriteria();
            var result = repository.Fetch<RescanTWProblemAccountModel>(criteria);
            lastCustID = criteria.LastCustId;
            return result;
        }

        public IEnumerable<RescanTWProblemAccountModel> GetCustomerPerformance(string custIdList)
        {
            var criteria = new GetCustomerPerformanceCriteria
            {
                CustIdList = custIdList
            };

            return repository.Fetch<RescanTWProblemAccountModel>(criteria);
        }
        
        public void RemoveSystemFormulaBetCustomer(string custIdList)
        {
            var criteria = new RemoveSystemFormulaBetCustomerCriteria
            {
                CustIdList = custIdList
            };

            repository.Execute(criteria);
        }
        
        public void ScanSystemFormulaBetMonitorComplete(uint lastCustId)
        {
            var criteria = new ScanSystemFormulaBetCustomerCompleteCriteria
            {
                LastCustId = lastCustId
            };

            repository.Execute(criteria);
        }

        public void InsertProblemAccToNAP(string usernameList, NapProblemAccInsertCriteria paramFromTW)
        {
            var criteria = new NapProblemAccInsertCriteria
            {
                CustomerList = usernameList,
                RoleId = paramFromTW.RoleId,
                ReasonId = paramFromTW.ReasonId,
                PreventActionId = paramFromTW.PreventActionId,
                Comment = paramFromTW.Comment,
                VoidTime = paramFromTW.VoidTime,
                LimitAmount = paramFromTW.LimitAmount,
                UserId = paramFromTW.UserId,
                IP = paramFromTW.IP
            };

            repository.Execute(criteria);
        }
    }
}
