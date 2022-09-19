namespace Fanex.NAP.SPU.CTS.Tests.Features.ArchiveData
{
    using Fanex.NAP.SPU.CTS.Features.ArchiveData;
    using Fanex.NAP.SPU.CTS.Features.ArchiveData.Entities;
    using Fanex.NAP.SPU.CTS.Features.MatchMonitor;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class ArchiveDataTests
    {
        private readonly IArchiveCustClassificationHistoryDataAccess dataAccess;
        private readonly IArchiveCustClassificationHistoryServices services;

        public ArchiveDataTests()
        {
            dataAccess = Substitute.For<IArchiveCustClassificationHistoryDataAccess>();

            services = new ArchiveCustClassificationHistoryServices(dataAccess);
        }

        [Fact]
        public void ArchiveChangedCustClassificationHistory_WhenSuccess_ReturnNonException()
        {
            var historyData = new HistoryClassificationArchiveEntity
            {
                CustClassificationHistory = new List<CustClassificationHistoryEntity>
                {
                  new CustClassificationHistoryEntity
                  {
                      ID = 1,
                      CustID = 1,
                      CTSCustID = 1
                  },
                  new CustClassificationHistoryEntity
                  {
                      ID = 2,
                      CustID = 2,
                      CTSCustID = 2
                  }
                },
                IsExistData = false
            };

            var historyDataJson = JsonConvert.SerializeObject(historyData.CustClassificationHistory);

            dataAccess.ArchiveCustClassificationHistory().Returns(historyData);

            dataAccess.InsertCustClassificationHistoryToArchiveDB(historyDataJson);

            Action action = () => services.ArchiveCustClassificationHistory();

            action.Should().NotThrow<Exception>();
        }
    }
}