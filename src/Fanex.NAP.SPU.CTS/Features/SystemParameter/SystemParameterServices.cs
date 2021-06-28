namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using System;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ISystemParameterServices
    {
        Int64 GetLastScannedId(SystemParameters paraID);

        DateTime? GetLastScannedDateTime(SystemParameters paraID);

        void UpdateTimingExecution(string paraValue, int paramId);
    }

    public class SystemParameterServices : ISystemParameterServices
    {
        private readonly IPreviousParameterDataAccess systemParameterDataAccess;

        public SystemParameterServices(IPreviousParameterDataAccess systemParameterDataAccess)
        {
            this.systemParameterDataAccess = systemParameterDataAccess;
        }

        public Int64 GetLastScannedId(SystemParameters paraID)
        {
            var lastScannedTransID = systemParameterDataAccess.GetPreviousTimingExecution(paraID);

            return lastScannedTransID != null ? Int64.Parse(lastScannedTransID?.ParameterValue) : 0;
        }

        public DateTime? GetLastScannedDateTime(SystemParameters paraID)
        {
            var lastScannedTransID = systemParameterDataAccess.GetPreviousTimingExecution(paraID);

            return lastScannedTransID != null ? DateTime.Parse(lastScannedTransID?.ParameterValue) : null;
        }


        public void UpdateTimingExecution(string paraValue, int paramId)
        {
            systemParameterDataAccess.UpdateTimingExecution(paraValue, paramId);
        }
    }
}