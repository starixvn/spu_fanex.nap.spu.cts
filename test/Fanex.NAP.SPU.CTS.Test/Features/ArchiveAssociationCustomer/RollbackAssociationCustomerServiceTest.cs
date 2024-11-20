namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveAssociationCustomer
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer;
    using FluentAssertions;
    using NSubstitute;
    using System;
    using Xunit;

    public class RollbackAssociationCustomerServiceTest
    {
        private readonly IRollbackAssociationCustomerDataAccess _dataAccess;
        private readonly IRollbackAssociationCustomerService _service;

        public RollbackAssociationCustomerServiceTest()
        {
            _dataAccess = Substitute.For<IRollbackAssociationCustomerDataAccess>();
            _service = new RollbackAssociationCustomerService(_dataAccess);
        }

        [Fact]
        public void RollbackAssociationCustomer_WhenSuccess_ReturnNotException()
        {
            Action action = () => _service.RollbackAssociationCustomer();

            action.Should().NotThrow<Exception>();
        }
    }
}
