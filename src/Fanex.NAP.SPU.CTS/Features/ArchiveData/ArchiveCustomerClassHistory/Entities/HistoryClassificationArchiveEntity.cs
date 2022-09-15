namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities
{
    using System.Collections.Generic;

    public class HistoryClassificationArchiveEntity
    {
        public IEnumerable<CustClassificationHistoryEntity> CustClassificationHistory { get; set; }

        public bool IsExistData { get; set; }
    }
}