namespace Fanex.NAP.SPU.CTS.Tests.Features.AssociatedGroupMonitor.Criteria
{
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Xunit;

    public class AddAssociatedAccountCriteriaTest
    {
        private readonly AddAssociatedAccountCriteria criteria;

        public AddAssociatedAccountCriteriaTest()
        {
            criteria = new AddAssociatedAccountCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            var expected = "CTS_DC_AssociatedGroup_Account_Add";

            var spName = criteria.GetSettingKey();

            Assert.Equal(expected, spName);
        }

    }
}
