namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ICommonDataAccess
    {
        IEnumerable<SystemParameterEntity> GetPreviousTimingExecution(SYSTEMPARAMETERS paramID);
    }

    public class CommonDataAccess : ICommonDataAccess
    {
        private readonly IDynamicRepository repository;

        public CommonDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<SystemParameterEntity> GetPreviousTimingExecution(SYSTEMPARAMETERS paramID)
        {
            var criteria = new PreviousTimingExecutionGetCriteria
            {
                ParamID = (int)paramID
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<SystemParameterEntity>(criteria).ToList();
            }
        }
    }
}