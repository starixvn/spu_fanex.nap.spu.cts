namespace Fanex.NAP.SPU.DCS.Tests.Features.Accounts
{
    using Fanex.NAP.SPU.CTS.Features.Accounts;
    using Fanex.NAP.SPU.CTS.Features.Accounts.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class MatchMonitorUnitTests
    {
        private readonly IMatchMonitorDataAccess matchMonitorDataAccess;
        private readonly IMatchMonitorServices matchMonitorServices;

        public MatchMonitorUnitTests()
        {
            matchMonitorDataAccess = Substitute.For<IMatchMonitorDataAccess>();
            matchMonitorServices = new MatchMonitorServices(matchMonitorDataAccess);
        }

        [Fact]
        public void GetLastScannedTransID_WhenSuccess_ReturnTransID()
        {
            var result = matchMonitorServices.GetLastScannedTransID((int)CTS.Shares.Constants.SYSTEMPARAMETERS.MATCHMONITOR_LASTSCANNEDTRANS);

            result.Should().NotBeNull();
        }
    }
}
