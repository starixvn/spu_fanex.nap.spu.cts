namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities
{
    using System;

    public class CustomerClassificationBySportEntity
    {
        public long ID { get; set; }

        public long CustID { get; set; }

        public long CTSCustID { get; set; }

        public int SportID { get; set; }

        public int CategoryID { get; set; }

        public decimal TurnoverRM { get; set; }

        public decimal WinlossRM { get; set; }

        public long BetCount { get; set; }

        public int ActiveDays { get; set; }

        public int TargetCC { get; set; }

        public int ActionType { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime InsertDate { get; set; }
    }
}