namespace Fanex.NAP.SPU.CTS.Tests.Features.AssociatedGroupMonitor.Criteria
{
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Xunit;

    public class ScanAssociatedAccountCriteriaTest
    {
        private readonly ScanAssociatedAccountCriteria criteria;

        public ScanAssociatedAccountCriteriaTest()
        {
            criteria = new ScanAssociatedAccountCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            var expected = "CTS_DC_AssociatedGroup_Scan";

            var spName = criteria.GetSettingKey();

            Assert.Equal(expected, spName);
        }
    }
}
