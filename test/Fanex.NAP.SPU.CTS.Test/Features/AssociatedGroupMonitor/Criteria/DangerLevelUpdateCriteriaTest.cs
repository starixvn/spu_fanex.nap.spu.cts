namespace Fanex.NAP.SPU.CTS.Tests.Features.AssociatedGroupMonitor.Criteria
{
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Xunit;

    public class DangerLevelUpdateCriteriaTest
    {
        private readonly DangerLevelUpdateCriteria criteria;

        public DangerLevelUpdateCriteriaTest()
        {
            criteria = new DangerLevelUpdateCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            var expected = "CTS_CustomerUpdDanger";

            var spName = criteria.GetSettingKey();

            Assert.Equal(expected, spName);
        }
    }
}
