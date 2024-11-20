namespace Fanex.NAP.SPU.CTS.Tests.Features.Transform
{
    using Fanex.NAP.SPU.CTS.Features.UpdateLastLogin;
    using FluentAssertions;
    using NSubstitute;
    using System;  
    using Xunit;

    public class UpdateLastLoginServiceTest
    {
        private readonly IUpdateLastLoginDataAccess _dataAccess;
        private readonly IUpdateLastLoginService _service;

        public UpdateLastLoginServiceTest()
        {
            _dataAccess = Substitute.For<IUpdateLastLoginDataAccess>();
            _service = new UpdateLastLoginService(_dataAccess);
        }

        [Fact]
        public void UpdateLastLoginTime_WhenSuccess_ReturnNotException()
        {
            Action action = () => _service.UpdateLastLoginTime(1);

            action.Should().NotThrow<Exception>();
        }
    }
}
