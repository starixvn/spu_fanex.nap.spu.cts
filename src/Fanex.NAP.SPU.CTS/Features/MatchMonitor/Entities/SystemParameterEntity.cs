namespace Fanex.NAP.SPU.CTS.Features.MatchMonitor.Entities
{
    using System;

    public class SystemParameterEntity
    {
        public string ParameterValue { get; set; }

        public string ParameterDataType { get; set; }

        public Int64 LastScannedTransId
        {
            get
            {
                if (string.IsNullOrEmpty(ParameterValue))
                {
                    throw new InvalidCastException($"MatchMonitor - Invalid LastScannedTranId - Data: {ParameterDataType}");
                }

                return Int64.Parse(ParameterValue);
            }   
        }
    }
}