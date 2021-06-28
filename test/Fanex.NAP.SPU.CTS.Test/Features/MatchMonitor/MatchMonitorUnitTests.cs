namespace Fanex.NAP.SPU.DCS.Tests.Features.Monitor
{
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using FluentAssertions;
    using NSubstitute;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.Shares.Constants;

    public class MatchMonitorUnitTests
    {
        private readonly IMatchMonitorDataAccess matchMonitorDataAccess;
        private readonly IMatchMonitorServices matchMonitorServices;
        private readonly ISystemParameterServices commonServices;
        private readonly IPreviousParameterDataAccess commonDataAccess;

        public MatchMonitorUnitTests()
        {
            matchMonitorDataAccess = Substitute.For<IMatchMonitorDataAccess>();
            matchMonitorServices = new MatchMonitorServices(matchMonitorDataAccess);
            commonDataAccess = Substitute.For<IPreviousParameterDataAccess>();
            commonServices = new SystemParameterServices(commonDataAccess);
        }

        [Fact]
        public void GetLastScannedTransID_WhenSuccess_ReturnTransID()
        {
            var result = commonServices.GetLastScannedId(SystemParameters.MatchMonitorLastScannedTrans);

            result.Should().NotBe(null);
        }
    }
}
