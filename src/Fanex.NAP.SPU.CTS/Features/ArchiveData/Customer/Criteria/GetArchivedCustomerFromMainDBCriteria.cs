namespace Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Criteria
{
    using Fanex.Data.Repository;
    using System;

    public class GetArchivedCustomerFromMainDBCriteria : CriteriaBase
    {
        public DateTime LastArchiveDate { get; set; }

        public int LastCustId { get; set; }

        public int RowSize { get; set; }

        public int CustType { get; set; }

        public override string GetSettingKey() => "CTS_ArchiveCustomer_Get";

        public override bool IsValid() => true;
    }
}
