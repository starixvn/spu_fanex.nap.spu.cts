namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface ISystemParameterDataAccess
    {
        SystemParameterEntity GetSystemParameterByID(SystemParameters paramID);

        void UpdateValueForSystemParameterByID(string paraValue, SystemParameters paramId);

        IEnumerable<BetTypeSettingEntity> GetBetTypeSettings(BetTypeSettingParaModel infos);
    }

    public class SystemParameterDataAccess : ISystemParameterDataAccess
    {
        private readonly IDynamicRepository repository;

        public SystemParameterDataAccess()
        {
            repository = new DynamicRepository();
        }

        public SystemParameterEntity GetSystemParameterByID(SystemParameters paramID)
        {
            var criteria = new GetSystemParameterCriteria
            {
                ParamID = (int)paramID
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<SystemParameterEntity>(criteria);
            }
        }

        public void UpdateValueForSystemParameterByID(string paraValue, SystemParameters paramId)
        {
            var criteria = new UpdateSystemParameterCriteria
            {
                ParamId = (int)paramId,
                ParamValue = paraValue
            };

            repository.Execute(criteria);
        }

        public IEnumerable<BetTypeSettingEntity> GetBetTypeSettings(BetTypeSettingParaModel infos)
        {
            var criteria = new GetCommonBetTypeSettingCriteria
            {
                FunctionID = infos.FunctionID,
                BettypeIDList = infos.BettypeIDList,
                SportTypeIDList = infos.SportTypeIDList,
                IsFilterMMDetection = infos.IsFilterMMDetection,
                IsFilterMMParlayDetection = infos.IsFilterMMParlayDetection,
                IsShowDetailsX = infos.IsShowDetailsX,
                Status = infos.Status
            };

            return repository.Fetch<BetTypeSettingEntity>(criteria);
        }
    }
}