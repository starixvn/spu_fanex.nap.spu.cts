namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveAssociationCustomer
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveAssociationCustomer;
    using FluentAssertions;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ArchiveAssociationCustomerServiceTest
    {
        private readonly IArchiveAssociationCustomerDataAccess _dataAccess;
        private readonly IArchiveAssociationCustomerService _service;

        public ArchiveAssociationCustomerServiceTest()
        {
            _dataAccess = Substitute.For<IArchiveAssociationCustomerDataAccess>();
            _service = new ArchiveAssociationCustomerService(_dataAccess);
        }

        [Fact]
        public void ArchiveAssociationCustomer_WhenSuccess_ReturnNotException()
        {
            // Arrange

            // Act
            _dataAccess.ArchiveAssociationCustomer();

            Action action = () => _service.ArchiveAssociationCustomer();

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void ArchiveAssociationCustomer_WhenFail_ThrowException()
        {
            // Act            
            _dataAccess.When(x => x.ArchiveAssociationCustomer()).Throw<Exception>();

            Action action = () => _service.ArchiveAssociationCustomer();
           
            // Assert
            Assert.Throws<Exception>(action);
        }
    }
}
