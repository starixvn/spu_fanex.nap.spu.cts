using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer;
using Fanex.NAP.SPU.CTS.Features.ArchiveData.Customer.Entities;
using Fanex.NAP.SPU.CTS.Features.SystemParameter;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.Shares.Consts;

namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveData
{
    public class ArchiveCustomerServiceTest
    {
        private readonly ISystemParameterServices _sysParamService;
        private readonly IArchiveCustomerDataAccess _dataAccess;
        private readonly IArchiveCustomerService _service;

        public ArchiveCustomerServiceTest()
        {
            _sysParamService = Substitute.For<ISystemParameterServices>();
            _dataAccess = Substitute.For<IArchiveCustomerDataAccess>();
            _service = new ArchiveCustomerService(_sysParamService, _dataAccess);
        }

        [Fact]
        public void ArchiveCTSCustomer_Credit_WhenSuccess()
        {
            // Arrange

            // Act
            _sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit).Returns(DateTime.MinValue);
            _sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit).Returns(999);
            _dataAccess.GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Credit).Returns(new List<MainDBArchiveCustomer> { new MainDBArchiveCustomer() });

            _service.ArchiveCTSCustomer(123);

            // Assert
            _sysParamService.Received(1).GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit);
            _sysParamService.Received(1).GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit);
            _dataAccess.Received(1).GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Credit);
            _dataAccess.Received(1).ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Credit);
        }

        [Fact]
        public void ArchiveCTSCustomer_Credit_ThrowException()
        {
            // Arrange

            // Act
            _sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit).Returns(DateTime.MinValue);
            _sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit).Returns(999);
            _dataAccess.GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Credit).Returns(new List<MainDBArchiveCustomer> { new MainDBArchiveCustomer() });
            _dataAccess.When(x => x.ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Credit)).Throw<Exception>();

            Action action = () => _service.ArchiveCTSCustomer(123);

            // Assert
            _dataAccess.Received(0).ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Credit);

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void ArchiveCTSCustomer_Deposit_WhenSuccess()
        {
            // Arrange

            // Act
            _sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit).Returns(DateTime.MinValue);
            _sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit).Returns(999);
            _dataAccess.GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Credit).Returns(new List<MainDBArchiveCustomer>());

            _sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastCustID_Deposit).Returns(DateTime.MinValue);
            _sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Deposit).Returns(999);
            _dataAccess.GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Deposit).Returns(new List<MainDBArchiveCustomer> { new MainDBArchiveCustomer() });

            _service.ArchiveCTSCustomer(123);

            // Assert
            _sysParamService.Received(1).GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit);
            _sysParamService.Received(1).GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit);
            _dataAccess.Received(1).GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Credit);
            _dataAccess.Received(0).ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Credit);

            _sysParamService.Received(1).GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Deposit);
            _sysParamService.Received(1).GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Deposit);
            _dataAccess.Received(1).GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Deposit);
            _dataAccess.Received(1).ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Deposit);
        }

        [Fact]
        public void ArchiveCTSCustomer_Deposit_ThrowException()
        {
            // Arrange

            // Act
            _sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastTime_Credit).Returns(DateTime.MinValue);
            _sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Credit).Returns(999);
            _dataAccess.GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Credit).Returns(new List<MainDBArchiveCustomer>());

            _sysParamService.GetSystemParamById<DateTime>(SystemParameters.ArchiveCustomerLastCustID_Deposit).Returns(DateTime.MinValue);
            _sysParamService.GetSystemParamById<int>(SystemParameters.ArchiveCustomerLastCustID_Deposit).Returns(999);
            _dataAccess.GetArchivedCustomerFromMainDb(Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<int>(), SubscriberType.Deposit).Returns(new List<MainDBArchiveCustomer> { new MainDBArchiveCustomer() });
            _dataAccess.When(x => x.ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Deposit)).Throw<Exception>();

            Action action = () => _service.ArchiveCTSCustomer(123);

            // Assert
            _dataAccess.Received(0).ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Credit);
            _dataAccess.Received(0).ArchiveCTSCustomer(Arg.Any<string>(), SubscriberType.Deposit);

            Assert.Throws<Exception>(action);
        }
    }
}
