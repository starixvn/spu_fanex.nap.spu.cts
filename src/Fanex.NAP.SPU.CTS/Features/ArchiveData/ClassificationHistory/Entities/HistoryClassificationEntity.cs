namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.ClassificationHistory
{
    using System;

    public class HistoryClassificationEntity
    {
        public long ID { get; set; }

        public long CustID { get; set; }

        public long CTSCustID { get; set; }

        public int SportGroupID { get; set; }

        public int CategoryID { get; set; }

        public decimal TurnoverRM { get; set; }

        public decimal WinlossRM { get; set; }

        public long BetCount { get; set; }

        public int ActiveDays { get; set; }

        public int RobotCounter { get; set; }

        public decimal LastXDaysMargin { get; set; }

        public decimal ProbationPeriodWinloss { get; set; }

        public int TargetCC { get; set; }

        public int SourceTypeID { get; set; }

        public int FromCategoryID { get; set; }

        public int ToCategoryID { get; set; }

        public bool IsAppliedCC { get; set; }

        public bool IsDataChanged { get; set; }

        public int ActionType { get; set; }

        public bool IsAuto { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime InsertDate { get; set; }

        public int TaggingType { get; set; }

        public int TargetDangerLevel1 { get; set; }

        public bool IsAppliedDangerLevel1 { get; set; }

        public decimal TWGroupBettingRate { get; set; }

        public decimal TWTicketRejectRate { get; set; }

        public long TWBetCount { get; set; }

        public decimal TWDesktopUsageRate { get; set; }

        public string Remark { get; set; }

        public bool IsDirect { get; set; }

        public long? TVSRequestID { get; set; }

        public bool IsFromTVS { get; set; }

        public bool IsFromTW { get; set; }

        public bool IsFromCTS { get; set; }

        public bool IsFromAI { get; set; }

        public bool IsFromImperva { get; set; }
    }
}