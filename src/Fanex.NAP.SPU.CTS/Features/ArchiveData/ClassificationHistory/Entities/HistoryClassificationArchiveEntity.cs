namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory
{
    using System.Collections.Generic;

    public class HistoryClassificationArchiveEntity
    {
        public IEnumerable<HistoryClassificationEntity> CustClassificationHistory { get; set; }

        public bool IsExistData { get; set; }
    }
}