namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using System;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public interface ISystemParameterServices
    {
        Int64 GetLastScannedId(SystemParameters paraID);

        DateTime? GetLastScannedDateTime(SystemParameters paraID);

        bool UpdateValueForSystemParamById(string paraValue, SystemParameters paramId);

        T GetSystemParamById<T>(SystemParameters paraID);
    }

    public class SystemParameterServices : ISystemParameterServices
    {
        private readonly ISystemParameterDataAccess systemParameterDataAccess;

        public SystemParameterServices(ISystemParameterDataAccess systemParameterDataAccess)
        {
            this.systemParameterDataAccess = systemParameterDataAccess;
        }

        public Int64 GetLastScannedId(SystemParameters paraID)
        {
            var lastScannedTransID = systemParameterDataAccess.GetSystemParameterByID(paraID);

            return lastScannedTransID != null ? Int64.Parse(lastScannedTransID?.ParameterValue) : 0;
        }

        public DateTime? GetLastScannedDateTime(SystemParameters paraID)
        {
            var lastScannedTransID = systemParameterDataAccess.GetSystemParameterByID(paraID);

            return lastScannedTransID != null ? DateTime.Parse(lastScannedTransID?.ParameterValue) : null;
        }

        public bool UpdateValueForSystemParamById(string paraValue, SystemParameters paramId)
        {
            if (!string.IsNullOrEmpty(paraValue))
            {
                systemParameterDataAccess.UpdateValueForSystemParameterByID(paraValue, paramId);
            }

            return true;
        }

        public T GetSystemParamById<T>(SystemParameters paraID)
        {
            var systemParam = systemParameterDataAccess.GetSystemParameterByID(paraID);

            return (T)Convert.ChangeType(systemParam.ParameterValue, typeof(T));
        }
    }
}