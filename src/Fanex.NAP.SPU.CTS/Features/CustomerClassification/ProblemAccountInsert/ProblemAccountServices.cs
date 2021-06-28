namespace Fanex.NAP.SPU.CTS.Features.CustomerClassification.ProblemAccountInsert
{
    using Fanex.NAP.SPU.CTS.Shares;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IProblemAccountServices
    {
        IEnumerable<ProblemAccountsEntity> GetInitialProblemAccounts();

        IEnumerable<ProblemAccountsEntity> GetProblemAccountsByPreExecution(DateTime? dateTime);

        IEnumerable<ProblemAccountsEntity> GetProblemAccounts(DateTime? dateTime);

        InsertCategoryResultEntity InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts, bool isRescan);
    }


    public class ProblemAccountServices : IProblemAccountServices
    {
        private readonly IProblemAccountDataAccess problemAccountDataAccess;

        public ProblemAccountServices(IProblemAccountDataAccess problemAccountDataAccess)
        {
            this.problemAccountDataAccess = problemAccountDataAccess;
        }

        public IEnumerable<ProblemAccountsEntity> GetProblemAccounts(DateTime? dateTime)
        {
            return dateTime == null ? GetInitialProblemAccounts() : GetProblemAccountsByPreExecution(dateTime);
        }

        public IEnumerable<ProblemAccountsEntity> GetInitialProblemAccounts()
        {
            var problemAccounts = new List<ProblemAccountsEntity>();

            problemAccounts.AddRange(problemAccountDataAccess.GetProblemAccounts_IBC(Constants.InitialProblemAccounts, null));
            
            problemAccounts.AddRange(problemAccountDataAccess.GetProblemAccounts_CN88(Constants.InitialProblemAccounts, null));

            return problemAccounts;
        }

        public IEnumerable<ProblemAccountsEntity> GetProblemAccountsByPreExecution(DateTime? dateTime)
        {
            var problemAccounts = new List<ProblemAccountsEntity>();

            problemAccounts.AddRange(problemAccountDataAccess.GetProblemAccounts_IBC(Constants.ProblemAccountByDateTime, dateTime));

            problemAccounts.AddRange(problemAccountDataAccess.GetProblemAccounts_CN88(Constants.ProblemAccountByDateTime, dateTime));

            return problemAccounts;
        }

        public InsertCategoryResultEntity InsertProblemAccountToCTS(IEnumerable<ProblemAccountsEntity> problemAccounts, bool isRescan)
        {
            var prproblemAccountJson = JsonConvert.SerializeObject(problemAccounts);

            return problemAccountDataAccess.InsertProblemAccount(prproblemAccountJson, isRescan);
        }
    }
}
