namespace Fanex.NAP.SPU.CTS.Features.SystemParameter
{
    using System;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public interface ICommonServices
    {
        Int64 GetLastScannedTransID(SYSTEMPARAMETERS paraID);
    }

    public class SystemParameterServices : ICommonServices
    {
        private readonly ICommonDataAccess commonDataAccess;

        public SystemParameterServices(ICommonDataAccess commonDataAccess)
        {
            this.commonDataAccess = commonDataAccess;
        }

        public Int64 GetLastScannedTransID(SYSTEMPARAMETERS paraID)
        {
            var lastScannedTransID = commonDataAccess.GetPreviousTimingExecution(paraID)?.SingleOrDefault();

            return Int64.Parse(lastScannedTransID?.ParameterValue);
        }
    }
}