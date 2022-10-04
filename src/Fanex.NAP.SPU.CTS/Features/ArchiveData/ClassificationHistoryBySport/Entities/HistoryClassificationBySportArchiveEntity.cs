namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistoryBySport
{
    using System.Collections.Generic;

    public class HistoryClassificationBySportArchiveEntity
    {
        public IEnumerable<HistoryClassificationBySportEntity> CustClassificationHistory { get; set; }

        public bool IsExistData { get; set; }
    }
}