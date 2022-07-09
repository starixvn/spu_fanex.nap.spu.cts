namespace Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor
{    
    using System.Collections.Generic;
    using System.Linq;   

    public class ScanAssociatedAccountModel
    {
        public ScanAssociatedAccountModel()
        {
            ScanAssociatedAccountList = Enumerable.Empty<ScanAssociatedAccountEntity>();
        }

        public IEnumerable<ScanAssociatedAccountEntity> ScanAssociatedAccountList { get; set; }

        public long GroupID { get; set; }

        public int ABI { get; set; }
    }
}
