namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveData
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Entities;
    using Fanex.NAP.SPU.CTS.Features.SystemParameter;
    using FluentAssertions;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.Shares.Consts;

    public class ArchiveCustomerMissingServiceTest
    {
        private readonly ISystemParameterDataAccess _sysParameterDataAccess;
        private readonly ISystemParameterServices _sysParamService;
        private readonly IArchiveCustomerMissingDataAccess _dataAccess;
        private readonly IArchiveCustomerMissingService _service;

        public ArchiveCustomerMissingServiceTest()
        {
            _sysParameterDataAccess = Substitute.For<ISystemParameterDataAccess>();
            _dataAccess = Substitute.For<IArchiveCustomerMissingDataAccess>();

            _sysParamService = new SystemParameterServices(_sysParameterDataAccess);
            _service = new ArchiveCustomerMissingService(_sysParamService, _dataAccess);
        }

        [Fact]
        public void ArchiveCreditCustomerMissing_WhenSuccess()
        {
            // Arrange
            var batchSize = 10;
            var expectedParamInt = new SystemParameterEntity()
            {
                ParameterDataType = "INT",
                ParameterValue = "1" 
            };

            var expectedParamDateTime = new SystemParameterEntity()
            {
                ParameterDataType = "DateTime",
                ParameterValue = DateTime.Now.ToString()
            };

            // Act        
            _sysParameterDataAccess.GetSystemParameterByID(SystemParameters.CorrectArchiveCustomer_LastArchiveDate_Credit).Returns(expectedParamDateTime);
            _sysParameterDataAccess.GetSystemParameterByID(SystemParameters.CorrectArchiveCustomer_LastCustID_Credit).Returns(expectedParamInt);
            _sysParameterDataAccess.GetSystemParameterByID(SystemParameters.CorrectArchiveCustomer_MaxArchiveDate_Credit).Returns(expectedParamDateTime);

            var lastTimeArchive_Credit = _sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_LastArchiveDate_Credit);
            var lastCustIdArchive_Credit = _sysParamService.GetSystemParamById<int>(SystemParameters.CorrectArchiveCustomer_LastCustID_Credit);
            var maxCustIdArchive_Credit = _sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_MaxArchiveDate_Credit);

            _dataAccess.GetArchivedCustomerMissing(lastCustIdArchive_Credit, lastTimeArchive_Credit, maxCustIdArchive_Credit, batchSize, SubscriberType.Credit)
                .Returns(new List<MainDBArchiveCustomer>() { new MainDBArchiveCustomer { } });
         
            Action action = () => _service.ArchiveCreditCustomerMissing(batchSize);

            // Assert
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void ArchiveDepositCustomerMissing_WhenSuccess()
        {
            // Arrange
            var batchSize = 10;
            var expectedParamInt = new SystemParameterEntity()
            {
                ParameterDataType = "INT",
                ParameterValue = "1"
            };

            var expectedParamDateTime = new SystemParameterEntity()
            {
                ParameterDataType = "DateTime",
                ParameterValue = DateTime.Now.ToString()
            };

            // Act        
            _sysParameterDataAccess.GetSystemParameterByID(SystemParameters.CorrectArchiveCustomer_LastArchiveDate_Deposit).Returns(expectedParamDateTime);
            _sysParameterDataAccess.GetSystemParameterByID(SystemParameters.CorrectArchiveCustomer_LastCustID_Deposit).Returns(expectedParamInt);
            _sysParameterDataAccess.GetSystemParameterByID(SystemParameters.CorrectArchiveCustomer_MaxArchiveDate_Deposit).Returns(expectedParamDateTime);

            var lastTimeArchive_Deposit = _sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_LastArchiveDate_Deposit);
            var lastCustIdArchive_Deposit = _sysParamService.GetSystemParamById<int>(SystemParameters.CorrectArchiveCustomer_LastCustID_Deposit);
            var maxCustIdArchive_Deposit = _sysParamService.GetSystemParamById<DateTime>(SystemParameters.CorrectArchiveCustomer_MaxArchiveDate_Deposit);

            _dataAccess.GetArchivedCustomerMissing(lastCustIdArchive_Deposit, lastTimeArchive_Deposit, maxCustIdArchive_Deposit, batchSize, SubscriberType.Deposit)
                .Returns(new List<MainDBArchiveCustomer>() { new MainDBArchiveCustomer { } });

            Action action = () => _service.ArchiveDepositCustomerMissing(batchSize);

            // Assert
            action.Should().NotThrow<Exception>();
        }
    }
}
