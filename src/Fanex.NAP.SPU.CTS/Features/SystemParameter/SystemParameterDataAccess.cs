namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface IPreviousParameterDataAccess
    {
        SystemParameterEntity GetPreviousTimingExecution(SystemParameters paramID);

        void UpdateTimingExecution(string paraValue, int paramId);
    }

    public class PreviousParameterDataAccess : IPreviousParameterDataAccess
    {
        private readonly IDynamicRepository repository;

        public PreviousParameterDataAccess()
        {
            repository = new DynamicRepository();
        }

        public SystemParameterEntity GetPreviousTimingExecution(SystemParameters paramID)
        {
            var criteria = new PreviousTimingExecutionGetCriteria
            {
                ParamID = (int)paramID
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<SystemParameterEntity>(criteria);
            }
        }

        public void UpdateTimingExecution(string paraValue, int paramId)
        {
            var criteria = new PreviousTimingExecutionUpdateCriteria
            {
                ParamId = paramId,
                ParamValue = paraValue
            };

            repository.Execute(criteria);
        }
    }
}