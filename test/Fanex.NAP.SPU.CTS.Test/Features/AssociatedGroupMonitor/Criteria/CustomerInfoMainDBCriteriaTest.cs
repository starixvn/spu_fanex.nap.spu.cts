namespace Fanex.NAP.SPU.CTS.Tests.Features.AssociatedGroupMonitor.Criteria
{
    using Fanex.NAP.SPU.CTS.Features.AssociatedGroupMonitor;
    using Xunit;

    public class CustomerInfoMainDBCriteriaTest
    {
        private readonly CustomerInfoMainDBCriteria criteria;

        public CustomerInfoMainDBCriteriaTest()
        {
            criteria = new CustomerInfoMainDBCriteria();
        }

        public void GetSettingKey_Any_ReturnRightSPName()
        {
            var expected = "CTS_Customer_GetInfoByCustIdList";

            var spName = criteria.GetSettingKey();

            Assert.Equal(expected, spName);
        }
    }
}
