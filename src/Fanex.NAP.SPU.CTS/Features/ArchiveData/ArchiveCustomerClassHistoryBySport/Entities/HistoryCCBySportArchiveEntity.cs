namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities
{
    using System.Collections.Generic;

    public class HistoryCCBySportArchiveEntity
    {
        public IEnumerable<CustomerClassificationBySportEntity> CustClassificationHistory { get; set; }

        public bool IsExistData { get; set; }
    }
}